/*
  GenericTone.cpp - SmartMaker library
  Copyright (C) 2015 ojh6t3k.  All rights reserved.
*/

//******************************************************************************
//* Includes
//******************************************************************************
#include "UnityApp.h"
#include "GenericTone.h"


//******************************************************************************
//* Constructors
//******************************************************************************

GenericTone::GenericTone(int id, int pin) : AppAction(id)
{
	_pin = pin;
}

//******************************************************************************
//* Override Methods
//******************************************************************************
void GenericTone::OnSetup()
{
}

void GenericTone::OnStart()
{
}

void GenericTone::OnStop()
{
	noTone(_pin);
}

void GenericTone::OnProcess()
{
}

void GenericTone::OnUpdate()
{
	UnityApp.pop(&_frequency);
}

void GenericTone::OnExcute()
{
	if(_frequency == 0)
		noTone(_pin);
	else
		tone(_pin, _frequency);
}

void GenericTone::OnFlush()
{
}
