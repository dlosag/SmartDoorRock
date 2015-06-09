#include "mpu.h"
#include "inv_mpu.h"
#include "inv_mpu_dmp_motion_driver.h"

#define FSR 2000
//#define GYRO_SENS       ( 131.0f * 250.f / (float)FSR )
#define GYRO_SENS       16.375f 
#define QUAT_SENS       1073741824.f //2^30

#define EPSILON         0.0001f
#define PI_2            1.57079632679489661923f

struct s_mympu mympu;

struct s_quat { float w, x, y, z; }; 
struct s_vec3 { float x, y, z; }; 

union u_quat {
	struct s_quat _f;
	long _l[4];
} q;

static int ret;
static short gyro[3];
static short sensors;
static unsigned char fifoCount;

////////////////////////////////////////////////////////////////////////////
//
//  The functions below are from the InvenSense SDK example code.
//
//  Original copyright notice below:

/*
 $License:
    Copyright (C) 2011-2012 InvenSense Corporation, All Rights Reserved.
    See included License.txt for License information.
 $
 */

/* These next two functions converts the orientation matrix (see
 * gyro_orientation) to a scalar representation for use by the DMP.
 * NOTE: These functions are borrowed from InvenSense's MPL.
 */

unsigned short inv_row_2_scale(signed char *row)
{
    unsigned short b;

    if (row[0] > 0)
        b = 0;
    else if (row[0] < 0)
        b = 4;
    else if (row[1] > 0)
        b = 1;
    else if (row[1] < 0)
        b = 5;
    else if (row[2] > 0)
        b = 2;
    else if (row[2] < 0)
        b = 6;
    else
        b = 7;      // error
    return b;
}

/* The sensors can be mounted onto the board in any orientation. The mounting
 * matrix seen below tells the MPL how to rotate the raw data from thei
 * driver(s).
 * TODO: The following matrices refer to the configuration on an internal test
 * board at Invensense. If needed, please modify the matrices to match the
 * chip-to-body matrix for your particular set up.
 */
unsigned short inv_orientation_matrix_to_scalar(signed char *mtx)
{
    unsigned short scalar;
    /*
       XYZ  010_001_000 Identity Matrix
       XZY  001_010_000
       YXZ  010_000_001
       YZX  000_010_001
       ZXY  001_000_010
       ZYX  000_001_010
     */
    scalar = inv_row_2_scale(mtx);
    scalar |= inv_row_2_scale(mtx + 3) << 3;
    scalar |= inv_row_2_scale(mtx + 6) << 6;
    return scalar;
}

//
////////////////////////////////////////////////////////////////////////////

int mympu_open(unsigned int rate, signed char *orientMatrix)
{
  	mpu_select_device(0);
   	mpu_init_structures();

	ret = mpu_init(NULL);
#ifdef MPU_DEBUG
	if (ret) return 10+ret;
#endif
	
	ret = mpu_set_sensors(INV_XYZ_GYRO|INV_XYZ_ACCEL); 
#ifdef MPU_DEBUG
	if (ret) return 20+ret;
#endif

        ret = mpu_set_gyro_fsr(FSR);
#ifdef MPU_DEBUG
	if (ret) return 30+ret;
#endif

        ret = mpu_set_accel_fsr(2);
#ifdef MPU_DEBUG
	if (ret) return 40+ret;
#endif

        mpu_get_power_state((unsigned char *)&ret);
#ifdef MPU_DEBUG
	if (!ret) return 50+ret;
#endif

        ret = mpu_configure_fifo(INV_XYZ_GYRO|INV_XYZ_ACCEL);
#ifdef MPU_DEBUG
	if (ret) return 60+ret;
#endif

	dmp_select_device(0);
	dmp_init_structures();

	ret = dmp_load_motion_driver_firmware();
#ifdef MPU_DEBUG
	if (ret) return 80+ret;
#endif

	ret = dmp_set_fifo_rate(rate);
#ifdef MPU_DEBUG
	if (ret) return 90+ret;
#endif

	ret = mpu_set_dmp_state(1);
#ifdef MPU_DEBUG
	if (ret) return 100+ret;
#endif

	dmp_set_orientation(inv_orientation_matrix_to_scalar(orientMatrix)); // set up the correct orientation

	ret = dmp_enable_feature(DMP_FEATURE_6X_LP_QUAT|DMP_FEATURE_SEND_CAL_GYRO|DMP_FEATURE_GYRO_CAL);
//	ret = dmp_enable_feature(DMP_FEATURE_SEND_CAL_GYRO|DMP_FEATURE_GYRO_CAL);
#ifdef MPU_DEBUG
	if (ret) return 110+ret;
#endif

	return 0;
}

static inline float rad2deg( float rad )
{
        //return (180.f/PI) * rad;
	return 57.2957795131f * rad;
}

void quaternionToYawPitchRoll(const struct s_quat *q, float* yaw, float* pitch, float* roll)
{
	// Get Gravity
	s_vec3 gravity;
	gravity.x = 2 * (q -> x*q -> z - q -> w*q -> y);
    gravity.y = 2 * (q -> w*q -> x + q -> y*q -> z);
    gravity.z = q -> w*q -> w - q -> x*q -> x - q -> y*q -> y + q -> z*q -> z;

    // yaw: (about Z axis)
    *yaw = atan2(2*q -> x*q -> y - 2*q -> w*q -> z, 2*q -> w*q -> w + 2*q -> x*q -> x - 1);
    // pitch: (nose up/down, about Y axis)
    *pitch  = atan(gravity.x / sqrt(gravity.y * gravity.y + gravity.z * gravity.z));
    // roll: (tilt left/right, about X axis)
    *roll  = atan(gravity.y / sqrt(gravity.x * gravity.x + gravity.z * gravity.z));
}

void quaternionToEuler(const struct s_quat *q, float* x, float* y, float* z)
{
    *z = atan2(2 * q->x * q->y - 2 * q->w * q->z, 2 * q->w * q->w + 2 * q->x * q->x - 1);   // psi
    *y = -asin(2 * q->x * q->z + 2 * q->w * q->y);                              // theta
    *x = atan2(2 * q->y * q->z - 2 * q->w * q->x, 2 * q->w * q->w + 2 * q->z * q->z - 1);   // phi
}

static inline float wrap_180(float x) {
	return (x<-180.f?x+360.f:(x>180.f?x-180.f:x));
}

int mympu_update() {

	do {
		ret = dmp_read_fifo(gyro, NULL, q._l, NULL, &sensors, &fifoCount);
		/* will return:
			0 - if ok
			1 - no packet available
			2 - if BIT_FIFO_OVERFLOWN is set
			3 - if frame corrupted
		       <0 - if error
		*/

		if (ret!=0) return ret; 
	} while (fifoCount>1);

	q._f.w = (float)q._l[0] / (float)QUAT_SENS;
	q._f.x = (float)q._l[1] / (float)QUAT_SENS;
	q._f.y = (float)q._l[2] / (float)QUAT_SENS;
	q._f.z = (float)q._l[3] / (float)QUAT_SENS;

	mympu.xyzw[0] = q._f.x;
	mympu.xyzw[1] = q._f.y;
	mympu.xyzw[2] = q._f.z;
	mympu.xyzw[3] = q._f.w;

//	quaternionToEuler( &q._f, &mympu.xyz[0], &mympu.xyz[1], &mympu.xyz[2] );
	
	/* need to adjust signs and do the wraps depending on the MPU mount orientation */ 
	/* if axis is no centered around 0 but around i.e 90 degree due to mount orientation */
	/* then do:  mympu.ypr[x] = wrap_180(90.f+rad2deg(mympu.ypr[x])); */
//	mympu.xyz[0] = rad2deg(mympu.xyz[0]);
//	mympu.xyz[1] = rad2deg(mympu.xyz[1]);
//	mympu.xyz[2] = rad2deg(mympu.xyz[2]);

	/* need to adjust signs depending on the MPU mount orientation */ 
	mympu.gyro[0] = -(float)gyro[2] / GYRO_SENS;
	mympu.gyro[1] = (float)gyro[1] / GYRO_SENS;
	mympu.gyro[2] = (float)gyro[0] / GYRO_SENS;

	return 0;
}

