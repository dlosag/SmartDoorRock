/*
  GenericServo.h - SmartMaker library
  Copyright (C) 2015 ojh6t3k.  All rights reserved.
*/

#ifndef GenericServo_h
#define GenericServo_h

#include <Servo.h>
#include "AppAction.h"


class GenericServo : AppAction
{
public:
	GenericServo(int id, int pin);	

protected:
	void OnSetup();
	void OnStart();
	void OnStop();
	void OnProcess();
	void OnUpdate();
	void OnExcute();
	void OnFlush();

private:
    int _pin;
	Servo _servo;
	byte _angle;
};

#endif

