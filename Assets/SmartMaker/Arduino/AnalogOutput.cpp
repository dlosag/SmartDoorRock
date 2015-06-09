/*
  AnalogOutput.cpp - SmartMaker library
  Copyright (C) 2015 ojh6t3k.  All rights reserved.
*/

//******************************************************************************
//* Includes
//******************************************************************************
#include "UnityApp.h"
#include "AnalogOutput.h"


//******************************************************************************
//* Constructors
//******************************************************************************

AnalogOutput::AnalogOutput(int id, int pin) : AppAction(id)
{
	_pin = pin;
}

//******************************************************************************
//* Override Methods
//******************************************************************************
void AnalogOutput::OnSetup()
{
	pinMode(_pin, OUTPUT);
	_value = 0;
	analogWrite(_pin, _value);
}

void AnalogOutput::OnStart()
{
}

void AnalogOutput::OnStop()
{
	_value = 0;
	analogWrite(_pin, _value);
}

void AnalogOutput::OnProcess()
{	
}

void AnalogOutput::OnUpdate()
{
	byte newValue;
	UnityApp.pop(&newValue);
	if(_value != newValue)
		_value = newValue;
}

void AnalogOutput::OnExcute()
{
	analogWrite(_pin, _value);
}

void AnalogOutput::OnFlush()
{
}

//******************************************************************************
//* Private Methods
//******************************************************************************

