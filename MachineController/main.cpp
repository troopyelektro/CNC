/*
 * MachineController.cpp
 *
 * Created: 02.12.2019 19:22:42
 * Author : Ing. Petr Rösch
 */ 

#include <avr/io.h>
#include <avr/interrupt.h>
#include <stdint.h>

// IO
#define BTN1	(1 << 2)
#define BTN2	(1 << 3)
#define LED		(1 << 6)
#define XSTEP	(1 << 4)
#define XDIR	(1 << 5)
#define YSTEP	(1 << 5)
#define YDIR	(1 << 4)
#define ZSTEP	(1 << 3)
#define ZDIR	(1 << 2)

#define commandShort (1<<7)
#define commandAcknowledge 20
#define commandGoTo 30
#define commandSetOrigin 31 | commandShort
#define commandLedOn 32 | commandShort
#define commandLedOff 33 | commandShort
#define rxBufferSize 15
#define rxBufferCommand 0
#define rxBufferX 1
#define rxBufferY 3
#define rxBufferZ 5
#define rxBufferSpeed 7
char rxBuffer [rxBufferSize];
unsigned int rxIndex = 0;

#define minTimePulseOn 500 // [us] 200
#define minTimePulseOff 500 // [us] 300

// Function prototypes:
void init(void);
void executeCommand(void);
void setStepX(bool value);
void setStepY(bool value);
void setStepZ(bool value);
void setDirX(bool up);
void setDirY(bool up);
void setDirZ(bool up);
void calcLinearTimeX(void);
void calcLinearTimeY(void);
void calcLinearTimeZ(void);
void setLed(bool on);

// Position memory
bool moving = false;
bool ledOn;
bool dirX = false;
bool dirY = false;
bool dirZ = false;
bool stepX = false;
bool stepY = false;
bool stepZ = false;

short startX = 0;
short startY = 0;
short startZ = 0;
short currentX = 0;
short currentY = 0;
short currentZ = 0;
short targetX = 0;
short targetY = 0;
short targetZ = 0;
short stepsX = 0;
short stepsY = 0;
short stepsZ = 0;
short stepsXtotal = 0;
short stepsYtotal = 0;
short stepsZtotal = 0;

uint64_t timeElapsed = 0;
uint64_t timeLength = 0;
uint64_t timeIntervalX = 0;
uint64_t timeIntervalY = 0;
uint64_t timeIntervalZ = 0;
uint64_t timeOnX = 0;
uint64_t timeOnY = 0;
uint64_t timeOnZ = 0;
uint64_t timeOffX = 0;
uint64_t timeOffY = 0;
uint64_t timeOffZ = 0;

int main(void)
{
	init();		
    while (1) 
    {						
		if(moving) {			
									
			if(timeElapsed < timeLength) {
				setLed(true);
			
				
				// Motion X
				if(stepsX < stepsXtotal) {
					if(!stepX && timeElapsed >= timeOnX && timeElapsed <= timeOffX) {
						setStepX(true);						
					}
					if(stepX && timeElapsed > timeOffX)  {
						setStepX(false);
						stepsX++;
						if(dirX) {
							currentX++;
						} else {
							currentX--;
						}						
						calcLinearTimeX();
					}					
				}
				
				// Motion Y
				if(stepsY < stepsYtotal) {
					if(!stepY && timeElapsed >= timeOnY && timeElapsed <= timeOffY) {
						setStepY(true);
					}
					if(stepY && timeElapsed > timeOffY)  {
						setStepY(false);
						stepsY++;
						if(dirY) {
							currentY++;
						} else {
							currentY--;
						}
						calcLinearTimeY();
					}
				}
				
				// Motion Z
				if(stepsZ < stepsZtotal) {
					if(!stepZ && timeElapsed >= timeOnZ && timeElapsed <= timeOffZ) {
						setStepZ(true);
					}
					if(stepZ && timeElapsed > timeOffZ)  {
						setStepZ(false);
						stepsZ++;
						if(dirZ) {
							currentZ++;
						} else {
							currentZ--;
						}
						calcLinearTimeZ();
					}
				}
				
			} else {											
				setLed(false);
				moving = false;	
				UDR = commandAcknowledge;
			}
															
		} else {
			setLed(true);
			for(int i=0; i<20; i++) {
				setLed(false);
			}		
		}
		
    }
}

void init(void)
{
	// SET LED as OUTPUT
	DDRD = LED | XSTEP | XDIR;
	DDRC = YSTEP | YDIR | ZSTEP | ZDIR;
	
    // USART initialization
    // Communication Parameters: 8 Data, 1 Stop, No Parity
    // USART Receiver: On
    // USART Transmitter: On
    // USART0 Mode: Asynchronous
    // USART Baud Rate: 115200
    UCSRA=0x00;
    UCSRB=0xD8;
    UCSRC=0x06;
    UBRRH=0x00;
    UBRRL=0x03;
	
	// TIMER
	TCCR0 = 0x01; // No Compare mode, no no waveform generator, no prescaller, timer running
	TIMSK = 0x01; // Interrupt enable for Timer 0
	
    // Global enable interrupts
    sei();	
}

ISR(TIMER0_OVF_vect)
{
	timeElapsed += 35; // Add 35 us to time
}

ISR(USART_RXC_vect)
{
	char data = UDR;
	rxBuffer[rxIndex] = data;
	rxIndex++;
				
	// SHORT COMMAND
	if(rxIndex == 1 && ((data & commandShort) > 0)) {
		rxIndex = 0;
		executeCommand();
	}
	
	// COMMAND WITH 16 BYTES OF ADDITIONAL DATA
	if(rxIndex==rxBufferSize) {
		rxIndex = 0;							
		executeCommand();
	}	
}

ISR(USART_TXC_vect)
{

}

void executeCommand(void)
{
	char cmd = rxBuffer[rxBufferCommand];
	
	if(cmd == commandGoTo) {					

		// Read Command
		for(int i=0; i<4 ; i++) {
			unsigned int bufOfset = 0;
			switch(i) {
				// X
				case 0:
					bufOfset = rxBufferX;
					break;					
				// Y
				case 1:
					bufOfset = rxBufferY;
					break;								
				// Z
				case 2:
					bufOfset = rxBufferZ;
					break;								
				// Speed
				case 3:
					bufOfset = rxBufferSpeed;
					break;				
			}
						
				
			char buf[8];
			
			// Construct number for buffer piece
			switch(i) {
				// X
				case 0:					
					for(int j=0; j<2; j++) {
						buf[j] = rxBuffer[bufOfset + j];
					}
					union {
						short integer;
						unsigned char byte[2];
					} vx;
					vx.byte[1] = buf[1];
					vx.byte[0] = buf[0];					
					targetX = vx.integer;
					break;
				// Y
				case 1:					
					for(int j=0; j<2; j++) {
						buf[j] = rxBuffer[bufOfset + j];
					}
					union {
						short integer;
						unsigned char byte[2];
					} vy;
					vy.byte[1] = buf[1];
					vy.byte[0] = buf[0];				
					targetY = vy.integer;
					break;
				// Z
				case 2:					
					for(int j=0; j<2; j++) {
						buf[j] = rxBuffer[bufOfset + j];
					}
					union {
						short integer;
						unsigned char byte[2];
					} vz;
					vz.byte[1] = buf[1];
					vz.byte[0] = buf[0];				
					targetZ = vz.integer;
					break;
				// Speed
				case 3:					
					for(int j=0; j<8; j++) {
						buf[j] = rxBuffer[bufOfset + j];
					}
					union {
						int64_t integer;
						unsigned char byte[8];
					} vt;
					vt.byte[7] = buf[7];
					vt.byte[6] = buf[6];
					vt.byte[5] = buf[5];
					vt.byte[4] = buf[4];
					vt.byte[3] = buf[3];
					vt.byte[2] = buf[2];
					vt.byte[1] = buf[1];
					vt.byte[0] = buf[0];
					timeLength = vt.integer * 1000;
					break;
			}			
		}
		
		// Store Current -> Start
		startX = currentX;
		startY = currentY;
		startZ = currentZ;
		
		// Set direction
		if(targetX >= startX) {
			setDirX(true);
			stepsXtotal = targetX - startX;
		} else {			
			setDirX(false);
			stepsXtotal = startX - targetX;
		}

		if(targetY >= startY) {
			setDirY(true);
			stepsYtotal = targetY - startY;
		} else {
			setDirY(false);
			stepsYtotal = startY - targetY;
		}
		
		if(targetZ >= startZ) {
			setDirZ(true);
			stepsZtotal = targetZ - startZ;
		} else {
			setDirZ(false);
			stepsZtotal = startZ - targetZ;
		}
		
		// Find axis with most density steps
		long int mdsa = 0;
		if(stepsXtotal > mdsa) {mdsa = stepsXtotal;}
		if(stepsYtotal > mdsa) {mdsa = stepsYtotal;}
		if(stepsZtotal > mdsa) {mdsa = stepsZtotal;}
		
		// reset steps
		stepsX = 0;
		stepsY = 0;
		stepsZ = 0;

		if(mdsa > 0) {			
			// Calculate lowest minimum time
			uint64_t minTime = (mdsa+1)*(minTimePulseOn+minTimePulseOff);
			
			// Apply minimum time if needed
			if(minTime > timeLength) {
				timeLength = minTime;				
			}
			
			// Calculate Step Intervals
			timeIntervalX = timeLength / (stepsXtotal + 1);
			timeIntervalY = timeLength / (stepsYtotal + 1);
			timeIntervalZ = timeLength / (stepsZtotal + 1);
			
			// Calculate next times
			calcLinearTimeX();
			calcLinearTimeY();
			calcLinearTimeZ();
			
			// reset time						
			timeElapsed = 0;
			moving = true;														
		} else {
			// No operation
			UDR = commandAcknowledge;
		}
		return;
	
	} else if(cmd == commandSetOrigin) {
	
		// SET CURRENT LOCATION AS ORIGIN
		currentX = 0;
		currentY = 0;
		currentZ = 0;
		UDR = commandAcknowledge;
		return;
	}	
}



void calcLinearTimeX(void)
{
	timeOnX = (stepsX + 1)*timeIntervalX;
	timeOffX = timeOnX + minTimePulseOn;
	return;
}

void calcLinearTimeY(void)
{
	timeOnY = (stepsY + 1)*timeIntervalY;
	timeOffY = timeOnY + minTimePulseOn;
	return;
}

void calcLinearTimeZ(void)
{
	timeOnZ = (stepsZ + 1)*timeIntervalZ;
	timeOffZ = timeOnZ + minTimePulseOn;
	return;
}

void setStepX(bool value)
{
	if(value) {
		PORTD |= XSTEP;
	} else {
		PORTD &= ~XSTEP;
	}
	stepX = value;
	return;
}

void setStepY(bool value)
{
	if(value) {
		PORTC |= YSTEP;
	} else {
		PORTC &= ~YSTEP;
	}
	stepY = value;
	return;
}

void setStepZ(bool value)
{
	if(value) {
		PORTC |= ZSTEP;
	} else {
		PORTC &= ~ZSTEP;
	}
	stepZ = value;
	return;
}

void setDirX(bool up)
{
	if(up) {
		PORTD |= XDIR;		
	} else {
		PORTD &= ~XDIR;					
	}
	dirX = up;
	return;
}

void setDirY(bool up)
{
	if(up) {
		PORTC |= YDIR;		
	} else {
		PORTC &= ~YDIR;		
	}
	dirY = up;
	return;
}

void setDirZ(bool up)
{
	if(up) {
		PORTC |= ZDIR;		
	} else {
		PORTC &= ~ZDIR;		
	}
	dirZ = up;
	return;
}

void setLed(bool on)
{
	if(on) {				
		PORTD |= LED;	
	} else {
		PORTD &= ~LED;	
	}
	ledOn = on;
	return;
}
