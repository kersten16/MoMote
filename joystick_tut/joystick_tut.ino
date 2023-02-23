#include <ezButton.h>
#define VRX_PIN  A3 // Arduino pin connected to VRX pin
#define VRY_PIN  A0 // Arduino pin connected to VRY pin
#define BUTTON_PIN 2 
#define SWITCH_PIN 7
#define SW_PIN   5

ezButton button(SW_PIN);

int xValue = 0; // To store value of the X axis
int yValue = 0; // To store value of the Y axis
int buttonState = 0;
int switchState=0;
int bValue = 0;
int centerX=0;
int centerY = 0;

void setup() {
  Serial.begin(9600) ;
  pinMode(SWITCH_PIN,INPUT_PULLUP);
  button.setDebounceTime(50);
}

void loop() {
   button.loop();
   
  // read analog X and Y analog values
  xValue = analogRead(VRX_PIN);
  yValue = analogRead(VRY_PIN);
  buttonState = digitalRead(BUTTON_PIN);
  switchState = digitalRead(SWITCH_PIN);

  bValue = button.getState();

  // if (button.isPressed()) {
  //   Serial.println("The button is pressed");
  //   // TODO do something here
  // }

  if (button.isReleased()) {
    Serial.println('z');
    // TODO do something here
  }
  // print data to Serial Monitor on Arduino IDE
  String xDirection="";
  String yDirection ="";
  if(xValue>=810){
    xDirection="-1";
  }else if(xValue<=790){
    xDirection="1";
  }else{xDirection="0";}
  
  if(yValue>=810){
    yDirection="-1";
  }else if(yValue<=790){
    yDirection="1";
  }else{yDirection="0";}

  String jsString = xDirection +':'+ yDirection+'?';
  String bString =String(buttonState)+'?';
  String sString= String(switchState);
  Serial.print(jsString);
  Serial.print(bString);
  Serial.println(sString);
  delay(100);
}
