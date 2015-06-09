/*
  UnityApp.cpp - SmartMaker library
  Copyright (C) 2015 ojh6t3k.  All rights reserved.
*/


//******************************************************************************
//* Includes
//******************************************************************************

#include "UnityApp.h"
#include "HardwareSerial.h"

extern "C" {
#include <string.h>
#include <stdlib.h>
}

//******************************************************************************
//* Constructors
//******************************************************************************

UnityAppClass::UnityAppClass(HardwareSerial* s) : UnityAppSerial(s)
{
	firstAction = 0;
}

//******************************************************************************
//* Public Methods
//******************************************************************************

void UnityAppClass::attachSerial(HardwareSerial* s)
{
	UnityAppSerial = s;
}

void UnityAppClass::begin(long speed)
{
	UnityAppSerial->begin(speed);
	readyReceived = false;
	processUpdate = 0;
	Reset();
}

void UnityAppClass::process(void)
{
	AppAction* action;

	while(UnityAppSerial->available() > 0)
	{
		byte bit = 1;
		int inputData = UnityAppSerial->read(); // this is 'int' to handle -1 when no data	

		if(inputData >= 0)
		{
			if(inputData & 0x80)
			{
				if(inputData == CMD_PING)
				{
					UnityAppSerial->write(CMD_PING);				
				}
				else if(inputData == CMD_START)
				{
					action = firstAction;
					while(action != 0)
					{
						action->start();
						action = action->nextAction;
					}
					
					UnityAppSerial->write(CMD_READY);
				}
				else if(inputData == CMD_EXIT)
				{
					action = firstAction;
					while(action != 0)
					{
						action->stop();
						action = action->nextAction;
					}					
				}
				else if(inputData == CMD_READY)
				{
					readyReceived = true;
				}
				else if(inputData == CMD_ACTION)
				{
					if(processUpdate > 0)
					{
						action = firstAction;
						while(action != 0)
						{
							action->excute();
							action = action->nextAction;
						}
						
						UnityAppSerial->write(CMD_READY);
						processUpdate = 0;
					}
				}
			
				if(inputData == CMD_UPDATE)
					processUpdate = 1;
				else
					Reset();
			}
			else if(processUpdate > 0)
			{
				if(processUpdate == 1)
				{
					ID = inputData;
					processUpdate = 2;
				}
				else if(processUpdate == 2)
				{
					numData = inputData;
					if(numData > MAX_ARGUMENT_BYTES)
						Reset();
					else
					{
						processUpdate = 3;
						currentNumData = 0;
					}
				}
				else if(processUpdate == 3)
				{
					if(currentNumData < numData)
						storedData[currentNumData++] = inputData;

					if(currentNumData >= numData)
					{
						// Decoding 7bit bytes
						numData = 0;
						for(int i=0; i<currentNumData; i++)
						{
							if(bit == 1)
							{
								storedData[numData] = storedData[i] << bit;
								bit++;
							}
							else if(bit == 8)
							{
								storedData[numData] |= storedData[i];
								bit = 1;
							}
							else
							{
								storedData[numData++] |= storedData[i] >> (7 - bit + 1);
								storedData[numData] = storedData[i] << bit;
								bit++;
							}
						}

						currentNumData = 0;
						
						action = firstAction;
						while(action != 0)
						{
							if(action->update(ID) == true)
								break;
							action = action->nextAction;
						}
						
						processUpdate = 1;
					}
				}
			}
			else
				Reset();
		}
	}

	if(readyReceived == true)
	{
		UnityAppSerial->write(CMD_UPDATE);
		
		action = firstAction;
		while(action != 0)
		{
			action->flush();				
			action = action->nextAction;
		}

		UnityAppSerial->write(CMD_ACTION);
		readyReceived = false;
	}

	action = firstAction;
	while(action != 0)
	{
		action->process();				
		action = action->nextAction;
	}
}

void UnityAppClass::select(byte id) 
{
	UnityAppSerial->write(id & 0x7F);
	numArgument = 0;
}

void UnityAppClass::flush() 
{
	float a = numArgument / 7;
	float b = numArgument % 7;
	byte addedNum = (byte)a;
	if(b > 0)
		addedNum++;

	UnityAppSerial->write((numArgument + addedNum) & 0x7F);
	// Encoding 7bit bytes
	byte bit = 1;
	byte value = 0;
	for(byte i=0; i<numArgument; i++)
	{
		UnityAppSerial->write((value | (storedArgument[i] >> bit)) & 0x7F);
		if(bit == 7)
		{
			UnityAppSerial->write(storedArgument[i] & 0x7F);
			bit = 1;
			value = 0;
		}
		else
		{
			value = storedArgument[i] << (7 - bit);
			if(i == (numArgument - 1))
				UnityAppSerial->write(value & 0x7F);
			bit++;
		}		
	}
}

void UnityAppClass::attachAction(AppAction* action)
{
	AppAction* a = firstAction;
	while(true)
	{
		if(a == 0)
		{
			firstAction = action;
			break;
		}
		if(a->nextAction == 0)
		{
			a->nextAction = action;
			break;
		}

		a = a->nextAction;
	}
	
	action->setup();
}

void UnityAppClass::detachAction(AppAction* action)
{
	AppAction* a = firstAction;
	AppAction* a1 = 0;

	while(a != 0)
	{
		if(a == action)
		{
			if(a1 == 0)
				firstAction = action->nextAction;
			else
				a1->nextAction = action->nextAction;
			break;
		}
		
		a1 = a;
		a = a->nextAction;
	}
}

boolean UnityAppClass::push(byte value)
{
	if((MAX_ARGUMENT_BYTES - numArgument) < 1)
		return false;

	storedArgument[numArgument++] = value;
	return true;
}

boolean UnityAppClass::push(word value)
{
	if((MAX_ARGUMENT_BYTES - numArgument) < 2)
		return false;

	storedArgument[numArgument++] = (byte)(value & 0xFF);
	storedArgument[numArgument++] = (byte)((value >> 8) & 0xFF);
	return true;
}

boolean UnityAppClass::push(short value)
{
	word binary;
	if(value < 0)
	{
		value *= -1;
		binary = (word)value;
		binary |= 0x8000;
	}
	else
		binary = (word)value;

	return push(binary);
}

boolean UnityAppClass::push(byte* value, byte count)
{
	if((MAX_ARGUMENT_BYTES - numArgument) < count)
		return false;

	for(byte i=0; i<count; i++)
		storedArgument[numArgument + i] = value[i];
	numArgument += count;
	return true;
}

boolean UnityAppClass::pop(byte* value)
{
	if((numData - currentNumData) < 1)
		return false;

	*value = storedData[currentNumData++];
	return true;
}

boolean UnityAppClass::pop(word* value)
{
	if((numData - currentNumData) < 2)
		return false;

	*value = storedData[currentNumData++];
	*value |= (storedData[currentNumData++] << 8) & 0xFF00;
	return true;
}

boolean UnityAppClass::pop(short* value)
{
	word binary;
	if(pop(&binary) == false)
		return false;

	*value = (short)(binary & 0x7FFF);
	if(binary & 0x8000)
		*value *= -1;
	return true;
}

boolean UnityAppClass::pop(byte* value, byte count)
{
	if((numData - currentNumData) < count)
		return false;

	for(byte i=0; i<count; i++)
		*(value + i) = storedData[currentNumData + i];
	currentNumData += count;
	return true;
}


//******************************************************************************
//* Private Methods
//******************************************************************************

// resets the system state upon a SYSTEM_RESET message from the host software
void UnityAppClass::Reset(void)
{
	if(processUpdate > 0)
		UnityAppSerial->write(CMD_READY);
	
	processUpdate = 0;
	numData = 0;
	currentNumData = 0;
	numArgument = 0;
}


UnityAppClass UnityApp(&Serial);


