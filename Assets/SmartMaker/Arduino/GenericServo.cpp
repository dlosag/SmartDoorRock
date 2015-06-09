/*
  GenericServo.cpp - SmartMaker library
  Copyright (C) 2015 ojh6t3k.  All rights reserved.
*/

//******************************************************************************
//* Includes
//******************************************************************************
#include "UnityApp.h"
#include "GenericServo.h"


//******************************************************************************
//* Constructors
//******************************************************************************

GenericServo::GenericServo(int id, int pin) : AppAction(id)
{
	_pin = pin;
}

//******************************************************************************
//* Override Methods
//******************************************************************************
void GenericServo::OnSetup()
{
}

void GenericServo::OnStart()
{
	_servo.attach(_pin);
}

void GenericServo::OnStop()
{
	_servo.detach();
}

void GenericServo::OnProcess()
{	
}

void GenericServo::OnUpdate()
{
	byte newAngle;
	UnityApp.pop(&newAngle);
	if(_angle != newAngle)
		_angle = newAngle;
}

void GenericServo::OnExcute()
{
	_servo.write(_angle);
}

void GenericServo::OnFlush()
{
}

//******************************************************************************
//* Private Methods
//******************************************************************************

