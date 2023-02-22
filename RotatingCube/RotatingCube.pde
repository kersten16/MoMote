import processing.serial.*;
String remoteKey="null";
PShape trigger;
float xAngle;
float yAngle;
boolean menuOpen=false;
boolean itemSelected = false;
String data="" ;
int joystickx=0;
int joysticky=0;
int selected=8;
int limitswitch =0;
int button = 0;
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
  parseData(data);
  noStroke();
  fill(0);
  rect(0,0,400,600);
  fill(#b5b5b0);
  if(itemSelected){
    menuOpen=false;
    itemSelected=false;
    delay(800);
  }
  if(menuOpen){
    System.out.println("coord"+joystickx+","+joysticky);
    if(joystickx==1){
      switch(joysticky){
        case -1:
          selected=1;
          break;
        case 0:
          selected=2;
          break;
        case 1:
          selected=3;
          break;
      }
    }
    else if(joystickx==-1){
        switch(joysticky){
          case -1:
            selected=7;
            break;
          case 0:
            selected=6;
            break;
          case 1:
            selected=5;
            break;
      }
    }else{
      switch(joysticky){
          case -1:
            selected=0;
            break;
          case 0:
            selected=8;
            break;
          case 1:
            selected=4;
            break;
      }
    }
    //System.out.println(selected);
    circularMenu(selected);
    System.out.println(limitswitch);
    System.out.println(limitswitch==0);
    if(limitswitch==0&& selected<8){
      fill(#E83839);
      arc(200, 300, 200, 200, (selected)*radians(360/8), (selected+1)*radians(360/8));
      itemSelected=true;
    }
    }
    else{
      lights();
      //fill(50);
      fill(100, 200, 150);
      pushMatrix();
      translate(200, 200,0);
       yAngle=joystickx;
       xAngle=joysticky;
       System.out.println(xAngle+ yAngle);
        //map(Integer.parseInt(joysticky),0,1023,2*PI,-2*PI);
      rotateY(yAngle);
      rotateX(xAngle);
      box(80);
      popMatrix();
    }

 
}

void circularMenu(int selected){
  float lastAngle = 0;
  for (int i = 0; i < 8; i++) {
    noStroke();
    if(i==selected){
      strokeWeight(5);
      stroke(#E83839);
  }
    float gradient = map(i, 0, 8, 50, 255);
    fill(gradient);
    arc(200, 300, 200, 200, lastAngle, lastAngle+radians(360/8));
    lastAngle += radians(360/8);
  }
}

void parseData(String data){
  if(data=="")return;
  String sectioned []= data.split("\n")[0].split("\\?");
  String joystick[]=sectioned[0].split(":");
  joystickx=Integer.parseInt(joystick[0]);
  joysticky=Integer.parseInt(joystick[1]);
  limitswitch = Integer.parseInt(sectioned[2].trim());

  if(button != Integer.parseInt(sectioned[1])&& button==0){
    System.out.println(button);
    menuOpen=!menuOpen;
  }
  button = Integer.parseInt(sectioned[1]);
}
void serialEvent(Serial myPort){
  data=myPort.readStringUntil('\n');
}
