#include <SD.h>
#include <Wire.h>
#include <Adafruit_ADS1015.h>

#define chipSelect 10
#define sensorPin 0

Adafruit_ADS1115 ads(0x48);
Adafruit_ADS1115 ads1115;


float sensorTime;
byte sensorFlag;
float sensor;
float sensor2;
int button = 0;
int counter = 1;


void setup() {

  ads.begin();
  ads1115.setGain(GAIN_TWOTHIRDS); // for an input range of +/- 6.144V see https://learn.adafruit.com/adafruit-4-channel-adc-breakouts/arduino-code for more details. 
  
  Serial.begin(9600);
 // Serial.print("Initializing SD card... ");

  pinMode(chipSelect, OUTPUT);

  // if (!SD.begin(chipSelect)){
  //     Serial.println("Card initialization failed. ");
  //     delay(100);
  //     return;
  // }
  // Serial.println("Card initialized. ");

  
// \
//   delay(100);
//   for (int i=1; i<10; i++){
//     String testString = String(i)+".csv";
//     if(SD.exists(testString)){
//         SD.remove(testString);
//     }
//   }



  pinMode(2,INPUT);     // setting pin 2 for switch input
}

void loop() {

    button = digitalRead(2);
    String fileName = "";

    // If the switch is on, the measurements are recorded.  The name of the file on the SD card uses the counter, so the tests must be kept in order.
    
    while (button == 0){
      String dataString = "";

      float Vout = 0.0;
      float V2 = 0.0;
      int ADCPin1 = 0;
      int ADCPin2 = 1;
      int Vin = 5;
      int known = 9840;
      int adcRead1 = 0;
      int adcRead2 = 0;

      adcRead1 = ads.readADC_SingleEnded(ADCPin1);
      adcRead2 = ads.readADC_SingleEnded(ADCPin2);
      Vout = (0.1875/1000)*adcRead1; // 6.144 volts is the default mode for ADS1115 and 6.144/2^15=0.1875/1000
      V2 = (0.1875 / 1000) * adcRead2; 
      sensor = known * Vout / Vin / (1 - Vout/Vin);
      //sensor2 = known * V2 / Vin / (1 - V2/Vin);
      sensorTime = millis();  

      dataString = String(sensor);

      
      // fileName = String(counter) +".csv";
      // File dataFile = SD.open(fileName, FILE_WRITE);

      // if(dataFile){
      //   dataFile.println(dataString);
      //   dataFile.close();

      //   Serial.println(dataString);
      // }
      // else{
      //   Serial.println("Error opening datalog.txt");
      // }

      Serial.println(dataString);

      

      //delay(50); //This is commented to increase the frequency to 30Hz
      if (digitalRead(2) == 1){
        counter = counter + 1;
        }
      button = digitalRead(2);
      }
  }
