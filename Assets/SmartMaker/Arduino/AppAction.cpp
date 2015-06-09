/*
  AppAction.cpp - SmartMaker library
  Copyright (C) 2015 ojh6t3k.  All rights reserved.
*/

//******************************************************************************
//* Includes
//******************************************************************************
#include "UnityApp.h"
#include "AppAction.h"


//******************************************************************************
//* Constructors
//******************************************************************************

AppAction::AppAction(int id)
{
	_id = (byte)id;
	_enableFlush = false;
	nextAction = 0;
}

//******************************************************************************
//* Public Methods
//******************************************************************************
void AppAction::setup()
{
	_updated = false;
	OnSetup();
}

void AppAction::start()
{
	_updated = false;
	_enableFlush = false;
	OnStart();
}

void AppAction::stop()
{
	_updated = false;
	OnStop();
}

void AppAction::process()
{
	OnProcess();
}

boolean AppAction::update(byte id)
{
	if(_id == id)
	{
		OnUpdate();		
		UnityApp.pop(&_enableFlush);
		_updated = true;
		return true;
	}

	return false;
}

void AppAction::excute()
{
	if(_updated == true)
	{
		OnExcute();		
		_updated = false;
	}
}

void AppAction::flush()
{
	if(_enableFlush == true)
	{
		UnityApp.select(_id);
		OnFlush();
		UnityApp.flush();
	}
}

