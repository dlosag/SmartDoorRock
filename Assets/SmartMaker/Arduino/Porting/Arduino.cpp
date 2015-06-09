/*
  Arduino.cpp - Arduino 포팅을 위한 소스
*/

#include "Arduino.h"


void pinMode(int pin, int mode)
{
	/* 포팅 팁
	   설명
	   : 이 함수는 GPIO핀의 사용 상태를 결정하기 위해 사용된다.

	   pin
	   : Arduino 핀 번호 (현재 환경에 적절한 pin 매핑 필요)
	   
	   mode
	   : OUTPUT: 현재 pin을 OUTPUT상태로 만든다.
	   : INPUT: 현재 pin을 INPUT상태로 만든다.
	   : INPUT_PULLUP: 현재 pin을 내부 PULL UP기반 INPUT상태로 만든다.
	*/
}

int digitalRead(int pin)
{
	/* 포팅 팁
	   설명
	   : 이 함수를 사용 전에 pinMode를 통해 INPUT 혹은 INPUT_PULLUP이 설정되어야 한다.

	   pin
	   : Arduino 핀 번호 (현재 환경에 적절한 pin 매핑 필요)
	   
	   return
	   : 현재 핀 상태를 0 혹은 1로 반환
	*/
}

unsigned long millis()
{
	/* 포팅 팁
	   사전 설정
	   : 이 함수는 시스템이 최초 부팅된 이후부터 msec단위의 시간을 반환한다.
	   : 32bit이므로 약 20시간을 측정할 수 있다.
	   : 주로 절대 시간이 아닌 상대 시간(시간 가격)을 측정하는데 사용된다.

	   return
	   : 부팅된 이후부터 msec단위의 시간
	*/
}



HardwareSerial::HardwareSerial()
{
	/* 포팅 팁
	   설명
	   : UART를 초기화시킨다.
	*/
}

void HardwareSerial::begin(unsigned long bps)
{
	/* 포팅 팁
	   설명
	   : UART를 사용 준비시킨다.

	   bps
	   : baudrate speed
	*/
}

int HardwareSerial::available()
{
	/* 포팅 팁
	   설명
	   : 이 클래스는 받은 데이터를 Ring 버퍼에 보관한다.
	   
	   return
	   : 현재 버퍼에 있는 데이터 수 (byte)
	*/
}

int HardwareSerial::read()
{
	/* 포팅 팁
	   설명
	   : 버퍼에서 데이터를 꺼내고, 버퍼에서 삭제한다.
	   : 이 함수를 사용하기 전에 available을 통해 데이터 존재 유무를 확인해야 한다.

	   return
	   : 현재 버퍼 제일 앞에 있는 데이터 반환
	*/
}

void HardwareSerial::write(int data)
{
	/* 포팅 팁
	   설명
	   : UART를 통해 데이터를 내보낸다.
	   : 비동기 통신을 지원하지 않으므로 데이터 내보내기가 완료될때까지 Block시킨다.
	   : Block대기를 최소화하기 위해 송신 버퍼를 사용할 수 있으며, 만약 버퍼가 차면 Block시켜야 한다.

	   data
	   : 내보낼 데이터 (byte)
	*/
}

HardwareSerial Serial;
