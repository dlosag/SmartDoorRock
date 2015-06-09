/*
  Arduino.h - Arduino 포팅을 위한 소스
*/

#ifndef Arduino_h
#define Arduino_h

//-----------------------------------------------------------//
// 만약, 아래 선언이 현재 환경과 충돌(중복 선언 등)된다면 주석 처리하십시오.
typedef unsigned char byte;
typedef unsigned int word;
typedef unsigned char boolean;

#define HIGH 0x1
#define LOW  0x0

#define INPUT 0x0
#define OUTPUT 0x1
#define INPUT_PULLUP 0x2

#define true 0x1
#define false 0x0
//------------------------------------------------------------//


void pinMode(int, int);
int digitalRead(int);
unsigned long millis(void);


class HardwareSerial
{
  private:

  public:
    HardwareSerial();
    void begin(unsigned long);
    int available(void);
    byte read(void);
    void write(int);
};
extern HardwareSerial Serial;

#endif

