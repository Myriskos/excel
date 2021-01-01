#include <MFRC522.h>
#include <LiquidCrystal_I2C.h>
#include <Keypad.h>
#include <SPI.h>

//*****************************************************
byte FI[8] = {
    0B01110,0B10101,0B10101,0B10101,0B01110,0B00100,0B00100,0B00000
};

byte PSI[8] = {
    0B10101,0B10101,0B10101,0B01110,0B00100,0B00100,0B00100,0B0000
};

byte OMEGA[8] = {
    0B01110,0B10001,0B10001,0B10001,0B01110,0B00000,0B11111,0B00000
};

byte GAMMA[8] = {
    0B11111,0B10000,0B10000,0B10000,0B10000,0B10000,0B10000,0B00000
};

byte DELTA[8] = {
    0B00100,0B01010,0B10001,0B10001,0B10001,0B10001,0B11111,0B00000
};

byte LAMDA[8] = {
    0B00100,0B01010,0B10001,0B10001,0B10001,0B10001,0B10001,0B00000
};

byte KSI[8] = {
    0b11111,0b00000,0b00000,0b01110,0b00000,0b00000,0b11111,0b00000
};

byte PEE[8] = {
    0b11111,0b10001,0b10001,0b10001,0b10001,0b10001,0b10001,0b00000
};
//********************************************

 //Το 246 είναι το dec του χαρακτήρα Σ 
 //Το 242 είναι το dec του χαρακτήρα Θ 

LiquidCrystal_I2C lcd(0x3F, 16, 2);
MFRC522 mfrc522(10, 9); // MFRC522 mfrc522(SS_PIN, RST_PIN)

// Initialize Pins for led's,  and buzzer
constexpr uint8_t greenLed = 7;
constexpr uint8_t redLed = 6;
constexpr uint8_t buzzerPin = 5;
constexpr uint8_t buttonPin = 4;
constexpr uint8_t ledPin = 3;
constexpr uint8_t modPin = 8;

char initial_password[4] = {'1', '2', '3', '4'};  // Variable to store initial password
String tagUID = "F5 B9 3C 5B";  // String to store UID of tag. Change it with your tag's UID
String tagUID2 = "xx xx xx xx";  // String to store UID of tag. Change it with your tag's UID
String tagUID3 = "xx xx xx xx";  // String to store UID of tag. Change it with your tag's UID
String tagUID4 = "xx xx xx xx";  // String to store UID of tag. Change it with your tag's UID

// String tagUID = "F5 B9 3C 5B";  // String to store UID of tag. Change it with your tag's UID
char password[4];   // Variable to store users password
boolean RFIDMode = true; // boolean to change modes
char key_pressed = 0; // Variable to store incoming keys
uint8_t i = 0;  // Variable used for counter
boolean buttonpress = false; // boolean to change modes
int clearonce = 0;


//================== ΡΥΘΜΙΣΕΙ΅=======================
//==== Χρόνος για το Κυπρί (δευτερόλεπτα *1000) =====
unsigned int reletime = 3000;
//====== ΓΛΩΣΣΑ =====================================
boolean lang = 0;         //0= ΕΛληνικά - 1 = Αγγλικά
//===================================================
const int SHORT_PRESS_TIME = 500; // 500 milliseconds
const int LONG_PRESS_TIME = 3000; // 500 milliseconds

//===================================================

int lastState = LOW;  // the previous state from the input pin
int currentState;     // the current reading from the input pin
unsigned long pressedTime  = 0;
unsigned long releasedTime = 0;

     
// defining how many rows and columns our keypad have
const byte rows = 4;
const byte columns = 4;

// Keypad pin map
char hexaKeys[rows][columns] = {
  {'1', '2', '3', 'A'},
  {'4', '5', '6', 'B'},
  {'7', '8', '9', 'C'},
  {'*', '0', '#', 'D'}
};

// Initializing pins for keypad
byte row_pins[rows] = {A0, A1, A2, A3};
byte column_pins[columns] = {2, 1, 0};

// Create instance for keypad
Keypad keypad_key = Keypad( makeKeymap(hexaKeys), row_pins, column_pins, rows, columns);

void setup() {

  lcd.init();
  lcd.backlight(); 
  lcd.createChar(0, FI);
  lcd.createChar(1, PSI);
  lcd.createChar(2, OMEGA);
  lcd.createChar(3, GAMMA);
  lcd.createChar(4, DELTA);
  lcd.createChar(5, LAMDA);
  lcd.createChar(6, KSI);
  lcd.createChar(7, PEE);  

  
  // Arduino Pin configuration
  pinMode(buzzerPin, OUTPUT);
  pinMode(redLed, OUTPUT);
  pinMode(greenLed, OUTPUT);
  pinMode(buttonPin, INPUT);
  pinMode(ledPin, OUTPUT);
  pinMode(modPin, INPUT);

  SPI.begin();      // Init SPI bus
  mfrc522.PCD_Init();   // Init MFRC522

  lcd.clear(); // Clear LCD screen

    if (digitalRead(modPin) == HIGH) {
      RFIDMode = true;
      digitalWrite (ledPin, HIGH); 
  }else{
      RFIDMode = false;
      digitalWrite (ledPin, LOW); 
  }
}



//==============================================================
void loop() {
//==============================================================

  // read the state of the switch/button:
  currentState = digitalRead(buttonPin);
  if(lastState == HIGH && currentState == LOW)        // button is pressed
    pressedTime = millis();
  else if(lastState == LOW && currentState == HIGH) { // button is released
    releasedTime = millis();
    long pressDuration = releasedTime - pressedTime;

    if( pressDuration > LONG_PRESS_TIME )
      lang = !lang;
  }
  lastState = currentState;


  // System will first look for mode  
  if (RFIDMode == true) {
    digitalWrite (ledPin, HIGH);
    

   if (digitalRead(buttonPin) == LOW) {
    lcd.clear();
    digitalWrite (buzzerPin, HIGH); 
    //digitalWrite(ledPin, HIGH);  
    RFIDMode = false;
    delay(80);
    digitalWrite (buzzerPin, LOW);      
   delay(500);
    return; 
   }
    lcd.setCursor(0, 0);
    if(lang ==0){
      lcd.write(7);  
      lcd.print("OPTA K");   
      lcd.write(5);        
      lcd.print("EI");  
      lcd.write(4);     
      lcd.write(2);  
      lcd.print("MENH");  
      lcd.setCursor(0, 1);
      lcd.print(" BA");     
      lcd.write(5); 
      lcd.print("E ID KAPTA  ");       
      lcd.print("  Scan ID card  ");
    }
    else
    {
      lcd.print("   Door Lock    ");
      lcd.setCursor(0, 1);
      lcd.print("  Scan ID card  ");              
    }
    // Look for new cards
    if ( ! mfrc522.PICC_IsNewCardPresent()) {
      return;
    }

    // Select one of the cards
    if ( ! mfrc522.PICC_ReadCardSerial()) {
      return;
    }

    //Reading from the card
    String tag = "";
    for (byte j = 0; j < mfrc522.uid.size; j++)
    {
      tag.concat(String(mfrc522.uid.uidByte[j] < 0x10 ? " 0" : " "));
      tag.concat(String(mfrc522.uid.uidByte[j], HEX));
    }
    tag.toUpperCase();

    //Checking the card
    if (tag.substring(1) == tagUID || tag.substring(1) == tagUID2 || tag.substring(1) == tagUID3 || tag.substring(1) == tagUID4)
    {
      // If UID of tag is matched.
      digitalWrite(buzzerPin, HIGH);    
      delay(200);      
      digitalWrite(buzzerPin, LOW);
      lcd.clear();
      if(lang == 0){
        lcd.print(" ID KAPTA ");  
        lcd.write(4);  
        lcd.print("EKTH ");
        lcd.write(246);
        lcd.setCursor(0, 1);
        lcd.print("= KA");  
        lcd.write(5);   
        lcd.write(2);  
        lcd.write(246);                       
        lcd.print(" HP");
        lcd.write(242);      
        lcd.print("ATE =");      
      }
      else
      {
        lcd.print("ID card Matched ");
        lcd.setCursor(0, 1);
        lcd.print("==  WELCOME!  ==");       
      }   
      digitalWrite(greenLed, HIGH);
      delay(reletime);
      digitalWrite(greenLed, LOW);
      lcd.clear();     
      
      if (digitalRead(modPin) == HIGH) {// Make RFID mode 
         RFIDMode = true;
         digitalWrite (ledPin, HIGH); 
       }else{
         RFIDMode = false;
         digitalWrite (ledPin, LOW); 
       }      
    } 
    else 
    {
      // If UID of tag is not matched.
      lcd.clear();
      lcd.setCursor(0, 0);
      if(lang ==0){
        lcd.print(" ");  
        lcd.write(5);  
        lcd.print("A");
        lcd.write(242);  
        lcd.print("O");
        lcd.write(246);       
        lcd.print(" ID KAPTA  ");
        lcd.setCursor(0, 1);
        lcd.print(" X");  
        lcd.write(2);            
        lcd.print("PI");
        lcd.write(246);      
        lcd.print(" ");      
        lcd.write(7); 
        lcd.print("PO");
        lcd.write(246);
        lcd.print("BA");
        lcd.write(246);        
        lcd.print("H");   
      }
      else
      {
        lcd.print(" Wrong ID Card  ");
        lcd.setCursor(0, 1);
        lcd.print(" Access Denied  ");
      }
      digitalWrite(buzzerPin, HIGH);
      digitalWrite(redLed, HIGH);      
      delay(800);      
      digitalWrite(buzzerPin, LOW);
       delay(1000);     
      digitalWrite(redLed, LOW);
      lcd.clear();
      
      if (digitalRead(modPin) == HIGH) {// Make RFID mode 
         RFIDMode = true;
         digitalWrite (ledPin, HIGH); 
       }else{
         RFIDMode = false;
         digitalWrite (ledPin, LOW); 
       }
    }
  }
//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
  // If RFID mode is false, it will look for keys from keypad
  if (RFIDMode == false) {
    
  if (digitalRead(buttonPin) == LOW) {
    lcd.clear();
    digitalWrite (buzzerPin, HIGH); 
    digitalWrite(ledPin, HIGH);  
    RFIDMode = true;
    delay(80);
    digitalWrite (buzzerPin, LOW);   
    delay(500);   
    return; 
  }

 
    digitalWrite (ledPin, LOW);
    lcd.setCursor(0, 0);
    if(lang ==0){
      lcd.print(" ");  
      lcd.write(4);  
      lcd.write(2); 
      lcd.write(246);    
      lcd.print("TE K");
      lcd.write(2);  
      lcd.write(4);    
      lcd.print("IKO:  ");
    }
    else
    {
      lcd.print("Enter Password: ");
    }
      lcd.setCursor(0, 1);
      
    RFIDMode = false; // Make RFID mode false
    key_pressed = keypad_key.getKey(); // Storing keys
    if (key_pressed)
    {
      digitalWrite (buzzerPin, HIGH); 
      delay(80);
      digitalWrite (buzzerPin, LOW); 
      password[i++] = key_pressed; // Storing in password variable
      lcd.setCursor(i, 1);      
      lcd.print("*");
    }
    if (i == 4) // If 4 keys are completed
    {
      delay(200);
      if (!(strncmp(password, initial_password, 4))) // If password is matched
      {
      lcd.clear();

      if(lang ==0){
        lcd.print(" ");        
        lcd.write(246);
        lcd.write(2);
        lcd.write(246);
        lcd.print("TO");   
        lcd.write(246);                      
        lcd.print(" K");
        lcd.write(2);  
        lcd.write(4);    
        lcd.print("IKO");
        lcd.write(246);      
        lcd.print("  ");
        lcd.setCursor(0, 1);
        lcd.print("= KA");  
        lcd.write(5);   
        lcd.write(2);  
        lcd.write(246);                       
        lcd.print(" HP");
        lcd.write(242);      
        lcd.print("ATE =");  
      } 
      else
      { 
        lcd.print(" Pass Accepted  ");
        lcd.setCursor(0, 1);
        lcd.print("==  WELCOME!  ==");
      }
        digitalWrite(buzzerPin, HIGH);    
        delay(200);      
        digitalWrite(buzzerPin, LOW);  
        digitalWrite(greenLed, HIGH);
        delay(reletime);
        digitalWrite(greenLed, LOW);
  //      sg90.write(0); // Door Closed
        lcd.clear();
        i = 0;

       if (digitalRead(modPin) == HIGH) {// Make RFID mode 
          RFIDMode = true;
          digitalWrite (ledPin, HIGH); 
        }else{
          RFIDMode = false;
          digitalWrite (ledPin, LOW); 
        }
       // RFIDMode = true; // Make RFID mode true
      }
      else    // If password is not matched
      {
        lcd.clear();
      if(lang == 0){  
        lcd.print(" ");  
        lcd.write(5);  
        lcd.print("A");
        lcd.write(242);  
        lcd.print("O");
        lcd.write(246);    
        lcd.print(" K");
        lcd.write(2);  
        lcd.write(4);    
        lcd.print("IKO");
        lcd.write(246);      
        lcd.print("  ");
        lcd.setCursor(0, 1);
        lcd.print(" X");  
        lcd.write(2);            
        lcd.print("PI");
        lcd.write(246);      
        lcd.print(" ");      
        lcd.write(7); 
        lcd.print("PO");
        lcd.write(246);
        lcd.print("BA");
        lcd.write(246);        
        lcd.print("H");    
      }
      else
      {
        lcd.print(" Wrong Password "); 
        lcd.setCursor(0, 1); 
        lcd.print(" Access denied  ");
      }
        
        digitalWrite(buzzerPin, HIGH);
        digitalWrite(redLed, HIGH);      
        delay(800);      
        digitalWrite(buzzerPin, LOW);
         delay(1000);     
        digitalWrite(redLed, LOW);
        lcd.clear();
        i = 0;
        if (digitalRead(modPin) == HIGH) {// Make RFID mode 
          RFIDMode = true;
          digitalWrite (ledPin, HIGH); 
        }else{
          RFIDMode = false;
          digitalWrite (ledPin, LOW); 
        }
      }
    }
  }
}
