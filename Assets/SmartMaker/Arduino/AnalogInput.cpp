/*
  AnalogInput.cpp - SmartMaker library
  Copyright (C) 2015 ojh6t3k.  All rights reserved.
*/

//******************************************************************************
//* Includes
//******************************************************************************
#include "UnityApp.h"
#include "AnalogInput.h"


//******************************************************************************
//* Constructors
//******************************************************************************

AnalogInput::AnalogInput(int id, int pin) : AppAction(id)
{
	_pin = pin;
}


//******************************************************************************
//* Override Methods
//******************************************************************************
void AnalogInput::OnSetup()
{
}

void AnalogInput::OnStart()
{
}

void AnalogInput::OnStop()
{	
}

void AnalogInput::OnProcess()
{	
}

void AnalogInput::OnUpdate()
{
}

void AnalogInput::OnExcute()
{
}

void AnalogInput::OnFlush()
{
	UnityApp.push((word)analogRead(_pin));
}
