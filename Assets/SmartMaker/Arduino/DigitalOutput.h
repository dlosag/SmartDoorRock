/*
  DigitalOutput.h - SmartMaker library
  Copyright (C) 2015 ojh6t3k.  All rights reserved.
*/
#ifndef DigitalOutput_h
#define DigitalOutput_h

#include "AppAction.h"


class DigitalOutput : AppAction
{
public:
	DigitalOutput(int id, int pin);	

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
	byte _state;
};

#endif

