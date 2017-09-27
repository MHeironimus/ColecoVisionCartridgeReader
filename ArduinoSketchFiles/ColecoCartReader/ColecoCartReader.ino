// ColecoVision / ADAM Cartridge Reader
// for the Arduino UNO
// 2014-11-25
//----------------------------------------------------------------------------------

// Arduino Pins
const int gcChipSelectLine[4] = { A0, A1, A2, A3 };
const int gcShiftRegisterClock = 11;
const int gcStorageRegisterClock = 12;
const int gcSerialAddress = 10;
const int gcDataBit[8] = { 2, 3, 4, 5, 6, 7, 8, 9 };

// Shifts a 16-bit value out to a shift register.
// Parameters:
//   dataPin - Arduino Pin connected to the data pin of the shift register.
//   clockPin - Arduino Pin connected to the data clock pin of the shift register.
//----------------------------------------------------------------------------------
void shiftOut16(int dataPin, int clockPin, int bitOrder, int value)
{
  // Shift out highbyte for MSBFIRST
  shiftOut(dataPin, clockPin, bitOrder, (bitOrder == MSBFIRST ? (value >> 8) : value));  
  // shift out lowbyte for MSBFIRST
  shiftOut(dataPin, clockPin, bitOrder, (bitOrder == MSBFIRST ? value : (value >> 8)));
}

// Select which chip on the cartridge to read (LOW = Active).
// Use -1 to set all chip select lines HIGH.
//----------------------------------------------------------------------------------
void SelectChip(byte chipToSelect)
{
  for(int currentChipLine = 0; currentChipLine < 4; currentChipLine++)
  {
    digitalWrite(gcChipSelectLine[currentChipLine], (chipToSelect != currentChipLine));
  }
}

// Set Address Lines
//----------------------------------------------------------------------------------
void SetAddress(unsigned int address)
{
    SelectChip(-1);
  
    // Disable shift register output while loading address
    digitalWrite(gcStorageRegisterClock, LOW);
    
    // Write Out Address
    shiftOut16(gcSerialAddress, gcShiftRegisterClock, MSBFIRST, address);  

    // Enable shift register output
    digitalWrite(gcStorageRegisterClock, HIGH);
    
    int chipToSelect;
    
    if (address < 0xA000) {
      chipToSelect = 0;
    } else if (address < 0xC000) {
      chipToSelect = 1;
    } else if (address < 0xE000) {
      chipToSelect = 2;
    } else {
      chipToSelect = 3;
    }
    SelectChip(chipToSelect);
}

// Read data lines
//----------------------------------------------------------------------------------
void ReadDataLines()
{
  const char cHexLookup[16] = {
    '0', '1', '2', '3', '4', '5', '6', '7', 
    '8', '9', 'A', 'B', 'C', 'D', 'E', 'F'};
    
  int highNibble = 0;
  int lowNibble = 0;
  boolean dataBits[8];
  char byteReadHex[4];

  for(int currentBit = 0; currentBit < 8; currentBit++)
  {
    dataBits[currentBit] = digitalRead(gcDataBit[currentBit]);
  }

  highNibble = (dataBits[7] << 3) + (dataBits[6] << 2) + (dataBits[5] << 1) + dataBits[4];
  lowNibble = (dataBits[3] << 3) + (dataBits[2] << 2) + (dataBits[1] << 1) + dataBits[0];

  Serial.write(cHexLookup[highNibble]);
  Serial.write(cHexLookup[lowNibble]);
  Serial.println();
}

// Read all of the data from the cartridge.
//----------------------------------------------------------------------------------
void ReadCartridge()
{
  unsigned int baseAddress = 0x8000;
  
  Serial.println("START:");
  
  // Read Current Chip (cartridge is 32K, each chip is 8k)
  for (unsigned int currentAddress = 0; currentAddress < 0x8000; currentAddress++) 
  {
    SetAddress(baseAddress + currentAddress);
    ReadDataLines();  
  }
  
  Serial.println(":END");
}

// Returns the next line from the serial port as a String.
//----------------------------------------------------------------------------------
String SerialReadLine()
{
  const int BUFFER_SIZE = 81;
  char lineBuffer[BUFFER_SIZE];
  int currentPosition = 0;
  int currentValue;
  
  do
  {
    // Read until we get the next character
    do
    {
      currentValue = Serial.read();
    } while (currentValue == -1);
    
    // ignore '\r' characters
    if (currentValue != '\r')
    {
      lineBuffer[currentPosition] = currentValue;
      currentPosition++;
    } 
  
  } while ((currentValue != '\n') && (currentPosition < BUFFER_SIZE));
  lineBuffer[currentPosition-1] = 0;
  
  return String(lineBuffer);
}

// Indicate to remote computer Arduino is ready for next command.
//----------------------------------------------------------------------------------
void ReadyForCommand()
{
  Serial.println("READY:");
}

void setup()
{
  // Setup Serial Monitor
  Serial.begin(57600);
  
  // Setup Chip Select Pins
  for(int chipLine = 0; chipLine < 4; chipLine++)
  {
    pinMode(gcChipSelectLine[chipLine], OUTPUT);
  }
  
  // Setup Serial Address Pins
  pinMode(gcShiftRegisterClock, OUTPUT);
  pinMode(gcStorageRegisterClock, OUTPUT);
  pinMode(gcSerialAddress, OUTPUT);
  
  // Setup Data Pins
  for(int currentBit = 0; currentBit < 8; currentBit++)
  {
    pinMode(gcDataBit[currentBit], INPUT_PULLUP);
  }
  
  while (!Serial) {
    ; // wait for serial port to connect. Needed for Leonardo only.
  }  
  
  // Reset Output Lines
  SetAddress(0);
  
  ReadyForCommand();
}

void loop()
{
  if (Serial.available() > 0)
  {
    String lineRead = SerialReadLine();
    lineRead.toUpperCase();
    
    if (lineRead == "READ ALL")
    {
      ReadCartridge();
    } // lineRead = "Read All"
    
    ReadyForCommand();
    
  } // Serial.available
}

