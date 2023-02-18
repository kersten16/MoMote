#include <ezButton.h>
#define VRX_PIN  A1 // Arduino pin connected to VRX pin
#define VRY_PIN  A0 // Arduino pin connected to VRY pin
#define BUTTON_PIN 7 
#define SW_PIN   2

ezButton button(SW_PIN);

int xValue = 0; // To store value of the X axis
int yValue = 0; // To store value of the Y axis
int buttonState = 0;
int bValue = 0;

void setup() {
  Serial.begin(9600) ;
  button.setDebounceTime(50);
}

void loop() {
   button.loop();
   
  // read analog X and Y analog values
  xValue = analogRead(VRX_PIN);
  yValue = analogRead(VRY_PIN);
 // buttonState = digitalRead(BUTTON_PIN);

bValue = button.getState();

  if (button.isPressed()) {
    Serial.println("The button is pressed");
    // TODO do something here
  }

  if (button.isReleased()) {
    Serial.println("The button is released");
    // TODO do something here
  }
  // print data to Serial Monitor on Arduino IDE
  Serial.print("x = ");
  Serial.print(xValue);
  Serial.print(", y = ");
  Serial.println(yValue);
  Serial.println(bValue);
  delay(500);
}
