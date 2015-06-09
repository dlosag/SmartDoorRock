/*
  GenericTone.h - SmartMaker library
  Copyright (C) 2015 ojh6t3k.  All rights reserved.
*/

#ifndef GenericTone_h
#define GenericTone_h

#include "AppAction.h"


class GenericTone : AppAction
{
public:
	GenericTone(int id, int pin);

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
	word _frequency;
};

#endif

