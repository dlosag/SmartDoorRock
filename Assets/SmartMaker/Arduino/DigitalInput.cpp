/*
  DigitalInput.cpp - SmartMaker library
  Copyright (C) 2015 ojh6t3k.  All rights reserved.
*/

//******************************************************************************
//* Includes
//******************************************************************************
#include "UnityApp.h"
#include "DigitalInput.h"


//******************************************************************************
//* Constructors
//******************************************************************************

DigitalInput::DigitalInput(int id, int pin, boolean pullup) : AppAction(id)
{
	_pin = pin;
	_pullup = pullup;
}

//******************************************************************************
//* Override Methods
//******************************************************************************
void DigitalInput::OnSetup()
{
	if(_pullup == false)
		pinMode(_pin, INPUT);
	else
		pinMode(_pin, INPUT_PULLUP);
}

void DigitalInput::OnStart()
{
}

void DigitalInput::OnStop()
{
}

void DigitalInput::OnProcess()
{	
}

void DigitalInput::OnUpdate()
{
}

void DigitalInput::OnExcute()
{
}

void DigitalInput::OnFlush()
{
	_state = digitalRead(_pin);
	UnityApp.push(_state);
}

//******************************************************************************
//* Private Methods
//******************************************************************************

