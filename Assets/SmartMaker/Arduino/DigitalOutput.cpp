/*
  DigitalOutput.cpp - SmartMaker library
  Copyright (C) 2015 ojh6t3k.  All rights reserved.
*/

//******************************************************************************
//* Includes
//******************************************************************************
#include "UnityApp.h"
#include "DigitalOutput.h"


//******************************************************************************
//* Constructors
//******************************************************************************

DigitalOutput::DigitalOutput(int id, int pin) : AppAction(id)
{
	_pin = pin;
}

//******************************************************************************
//* Override Methods
//******************************************************************************
void DigitalOutput::OnSetup()
{
	digitalWrite(_pin, LOW); // disable PWM
	pinMode(_pin, OUTPUT);
	_state = 0;
	digitalWrite(_pin, _state);
}

void DigitalOutput::OnStart()
{
}

void DigitalOutput::OnStop()
{
	_state = 0;
	digitalWrite(_pin, _state);
}

void DigitalOutput::OnProcess()
{	
}

void DigitalOutput::OnUpdate()
{
	byte newState;
	UnityApp.pop(&newState);
	if(_state != newState)
		_state = newState;
}

void DigitalOutput::OnExcute()
{
	digitalWrite(_pin, _state);
}

void DigitalOutput::OnFlush()
{
}

//******************************************************************************
//* Private Methods
//******************************************************************************

