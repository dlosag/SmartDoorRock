/*
  AppAction.h - SmartMaker library
  Copyright (C) 2015 ojh6t3k.  All rights reserved.
*/

#ifndef AppAction_h
#define AppAction_h


class AppAction
{
public:
	AppAction* nextAction;

	AppAction(int id);

	void setup();
	void start();
	void stop();
	void process();
	boolean update(byte id);
	void excute();
	void flush();

protected:
	virtual void OnSetup() {}
	virtual void OnStart() {}
	virtual void OnStop() {}
	virtual void OnProcess() {}
	virtual void OnUpdate() {}
	virtual void OnExcute() {}
	virtual void OnFlush() {}

private:
	byte _id;
	byte _enableFlush;	
	boolean _updated;
};

#endif

