/*
  AnalogInput.h - SmartMaker library
  Copyright (C) 2015 ojh6t3k.  All rights reserved.
*/

#ifndef AnalogInput_h
#define AnalogInput_h

#include "AppAction.h"


class AnalogInput : AppAction
{
public:
	AnalogInput(int id, int pin);

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
};

#endif

