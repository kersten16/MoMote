import processing.serial.*;
String remoteKey="null";
//float xAngle;
//float yAngle;
boolean menuOpen=false;
boolean itemSelected = false;
boolean flag=false;
Cube cube = new Cube();
String data="" ;
int joystickx=0;
int joysticky=0;
int mode=0; //0 for rotation 1 for colour
int selected=8;
int limitswitch =0;
int button = 0;
float xAngle, yAngle;
float gradients [] = new float[8];
float colorHue [] = {5,0,0};
Serial port;

void setup(){
  size(500, 800, P3D);
  frameRate(5);
  port = new Serial(this, "/dev/ttyACM0", 9600);
  ///dev/ttyACM0
  for ( int i=0;i<8;i++){
    float gradient = map(i, 0, 8, 50, 255);
    gradients[i]=gradient;
  }
 // ortho(-200, 200, -300, 300,10,2000);
  lights();
  background(255);
  port.bufferUntil('\n');
}

//------------------------------------------------------------------ 
void draw(){
  parseData(data);
  noStroke();
  fill(0);
  rect(0,0,500,800);
  fill(#b5b5b0);
  xAngle=0;
  yAngle=0;
  if(mode==1){
    if(itemSelected){
      menuOpen=false;
      itemSelected=false;
      delay(800);
      colorHue[2]=(selected == 8)?0:gradients[selected];
      cube.changeColor(colorHue[2]);
      //cube.drawCube();
    }
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
    if(menuOpen){
      //System.out.println(selected);
      circularMenu(selected);
      if(limitswitch==0&& selected<8){
        fill(#ffffff);
        arc(250, 400, 200, 200, (selected+1)*radians(360/8), (selected+2)*radians(360/8));
        itemSelected=true;
      }
    }
      else{
        cube.updateFace(selected);
        lights();
        cube.drawCube(xAngle, yAngle);
      }
  }else{
    //translate(200 , 250, 0);
    xAngle=joysticky*PI/8;
    yAngle=joystickx*PI/8;
    lights();
    cube.drawCube(xAngle, yAngle);
  }



 
}

void circularMenu(int selected){
  float lastAngle = radians((360/8));
  for (int i = 0; i < 8; i++) {
    noStroke();
    if(i==selected){
      strokeWeight(5);
      stroke(#E83839);
  }
    fill(color(20*(gradients[i]%colorHue[0]),255-gradients[i],gradients[i]));
    arc(250, 400, 200, 200, lastAngle, lastAngle+radians(360/8));
    lastAngle += radians(360/8);
  }
}

void parseData(String data){
  System.out.print(data);
  if(data=="")return;
  if(flag){
    menuOpen=!menuOpen;
    flag=false;
    return;
  }
  String sectioned []= data.split("\n")[0].split("\\?");
  String joystick[]=sectioned[0].split(":");
  joystickx=Integer.parseInt(joystick[0]);
  joysticky=Integer.parseInt(joystick[1]);
  limitswitch = Integer.parseInt(sectioned[2].trim());

  if(button != Integer.parseInt(sectioned[1])&& button==0){
    mode=(-1)*mode +1;
    cube.resetView();
  }
  button = Integer.parseInt(sectioned[1]);
}
void serialEvent(Serial myPort){
  data=myPort.readStringUntil('\n');
  if(data.contains("z"))flag=true;
  
}
