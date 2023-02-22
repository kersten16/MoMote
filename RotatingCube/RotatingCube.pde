import processing.serial.*;
String remoteKey="null";
PShape trigger;
float xAngle;
float yAngle;
boolean menuOpen;
String data="" ;
String joystickx="";
String joysticky="";
String limitswitch ="";
String button = "";
Serial port;

void setup(){
  size(1000, 800, P3D);
  //perspective(PI/3, float(width)/height, 1, 200);
  port = new Serial(this, "COM10", 9600);
  lights();
  background(255);
  trigger=createShape(RECT,700,350,50,100);
  port.bufferUntil('\n');
}

//------------------------------------------------------------------ 
void draw(){
  
  fill(0);
  rect(0,0,400,600);
  fill(#b5b5b0);
  if(menuOpen){
    circularMenu();
    shape(trigger);
    trigger.setFill(color(0));
    if(remoteKey == "trigger"){
      trigger.setFill(color(127,0,40));
      stroke(color(255,0,0));
      System.out.println("trigger");
    }else{
       noStroke();
    }

     //take joystick input and map to option
  }
  //ellipse(500,400,50,50);
  //float x = (mouseX/360.0)*PI;
  //float y = (mouseY/360.0)*PI;

 
  lights();
  //fill(50);
  fill(100, 200, 150);
  pushMatrix();
  translate(200, 200,0);
  if(mousePressed){
    yAngle=map(mouseX,0,width,-2*PI,2*PI);
    xAngle=map(mouseY,0,height,2*PI,-2*PI);
  }

  rotateY(yAngle);
  rotateX(xAngle);
  box(80);
  popMatrix();

 
}

void circularMenu(){
  float lastAngle = 0;
  for (int i = 0; i < 6; i++) {
    float gradient = map(i, 0, 6, 50, 255);
    fill(gradient);
    arc(200, 300, 100, 100, lastAngle, lastAngle+radians(360/5));
    lastAngle += radians(360/5);
  }
}

void keyPressed(){
 
  /*else*/ if (key==CODED) {
    if(keyCode==DOWN){
      remoteKey="button";
      menuOpen=!menuOpen;
      redraw();
    }else if (keyCode==CONTROL){
      remoteKey="trigger";
    }   
  }
  redraw();
}
void keyReleased(){
  remoteKey="null";
  redraw();
}
void serialEvent(Serial myPort){
  data=myPort.readStringUntil('\n');
  String sectioned []= data.split("-");
  joystickx=sectioned[0].split(":")[0];
  joysticky=sectioned[0].split(":")[1];
  button = sectioned[1];
  limitswitch = sectioned[2];
}
