/*
  AnalogOutput.h - SmartMaker library
  Copyright (C) 2015 ojh6t3k.  All rights reserved.
*/

#ifndef AnalogOutput_h
#define AnalogOutput_h

#include "AppAction.h"


class AnalogOutput : AppAction
{
public:
	AnalogOutput(int id, int pin);	

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
	byte _value;
};

#endif

