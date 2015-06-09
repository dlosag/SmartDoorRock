/*
  MPU9150.cpp - SmartMaker library
  Copyright (C) 2015 ojh6t3k.  All rights reserved.
*/

//******************************************************************************
//* Includes
//******************************************************************************
#include "UnityApp.h"
#include "MPU9150.h"
#include "mpu.h"
#include "I2Cdev.h"

//******************************************************************************
//* Constructors
//******************************************************************************

MPU9150::MPU9150(int id) : AppAction(id)
{
	_qX = 0;
    _qY = 0;
    _qZ = 0;
	_qW = 0;
}


//******************************************************************************
//* Override Methods
//******************************************************************************
signed char imu_orientation[9] = { 1, 0, 0
				,0, 1, 0
				,0, 0, 1};

void MPU9150::OnSetup()
{
	Fastwire::setup(400, 0);
  
    if(!mympu_open(200, imu_orientation))
	{
	}
}

void MPU9150::OnStart()
{
	_preTime = millis();
}

void MPU9150::OnStop()
{	
}

void MPU9150::OnProcess()
{
	if(!mympu_update()) // Successful MPU9150 DMP read
	{
		SetQuarternion(mympu.xyzw[0], mympu.xyzw[1], mympu.xyzw[2], mympu.xyzw[3]);
	}
}

void MPU9150::OnUpdate()
{
}

void MPU9150::OnExcute()
{
}

void MPU9150::OnFlush()
{
	unsigned long curTime = millis();
	_intervalTime = (word)(curTime - _preTime);
	_preTime = curTime;

	UnityApp.push(_qX);
    UnityApp.push(_qY);
    UnityApp.push(_qZ);
	UnityApp.push(_qW);
	UnityApp.push(_intervalTime);
}

void MPU9150::SetQuarternion(float x, float y, float z, float w)
{
	_qX = (short)(x * 10000);
    _qY = (short)(y * 10000);
    _qZ = (short)(z * 10000);
	_qW = (short)(w * 10000);
}