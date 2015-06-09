/*
  UnityApp.h - SmartMaker library
  Copyright (C) 2015 ojh6t3k.  All rights reserved.
*/

#ifndef UnityApp_h
#define UnityApp_h

#include "Arduino.h"
#include "AppAction.h"

// Do not edit below contents
#define MAX_ARGUMENT_BYTES    116
#define CMD_START         0x80 // start
#define CMD_EXIT	      0x81 // exit
#define CMD_UPDATE        0x82 // update
#define CMD_ACTION        0x83 // action
#define CMD_READY         0x84 // ready
#define CMD_PING	      0x85 // ping


class UnityAppClass
{
public:
	UnityAppClass(HardwareSerial *s);

	// for application
	void attachSerial(HardwareSerial *s);
	void begin(long speed);
    void process(void);
    void attachAction(AppAction* action);
	void detachAction(AppAction* action);

    // for module
	void select(byte id);
	void flush();
	boolean push(byte value);
	boolean push(word value);
	boolean push(short value);
	boolean push(byte* value, byte count);
	boolean pop(byte* value);
	boolean pop(word* value);
	boolean pop(short* value);
	boolean pop(byte* value, byte count);

private:
    HardwareSerial* UnityAppSerial;
	AppAction* firstAction;

	boolean readyReceived;
	byte processUpdate;
	byte ID;
	byte numData;
	byte currentNumData;
    byte storedData[MAX_ARGUMENT_BYTES + (MAX_ARGUMENT_BYTES / 8) + 1];
	byte numArgument;
	byte storedArgument[MAX_ARGUMENT_BYTES];
	
    void Reset(void);
};

extern UnityAppClass UnityApp;

#endif

