/*
  MPU9150.h - SmartMaker library
  Copyright (C) 2015 ojh6t3k.  All rights reserved.
*/

#ifndef MPU9150_h
#define MPU9150_h

#include "AppAction.h"


class MPU9150 : AppAction
{
public:
	MPU9150(int id);

	void SetQuarternion(float x, float y, float z, float w);

protected:
	void OnSetup();
	void OnStart();
	void OnStop();
	void OnProcess();
	void OnUpdate();
	void OnExcute();
	void OnFlush();

private:
    short _qX;
    short _qY;
    short _qZ;
	short _qW;
	word _intervalTime;
	unsigned long _preTime;
};

#endif

