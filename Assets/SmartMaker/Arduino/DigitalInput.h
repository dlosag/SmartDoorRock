/*
  DigitalInput.h - SmartMaker library
  Copyright (C) 2015 ojh6t3k.  All rights reserved.
*/

#ifndef DigitalInput_h
#define DigitalInput_h

#include "AppAction.h"


class DigitalInput : AppAction
{
public:
	DigitalInput(int id, int pin, boolean pullup);	

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
	boolean _pullup;
	byte _state;
};

#endif

