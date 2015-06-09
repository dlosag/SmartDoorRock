#ifndef MPU_H
#define MPU_H

struct s_mympu {
	float xyzw[4];
	float gyro[3];
};

extern struct s_mympu mympu;

int mympu_open(unsigned int rate, signed char *orientMatrix);
int mympu_update();

#endif

