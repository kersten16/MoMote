String remoteKey="null";
color colors [] = {#7ef27c,#b77cf2,#f29b7c,#7cc1f2};
int colorIndex = 0;
PShape trigger;

void setup() {
  size(800, 560);
  background(255);
  fill(102);
  rect(0,0,800,300);
  fill(0);
  trigger=createShape(RECT,300,350,50,100);
  
}

void draw() {
  stroke(255);
  shape(trigger);
  if (mousePressed == true && mouseX>=0 && mouseY<=300) {
    stroke(colors[colorIndex]);
    strokeWeight(1);
    trigger.setFill(color(0));
    if(remoteKey == "trigger"){
      trigger.setFill(color(127,0,40));
      strokeWeight(3);
      System.out.println("trigger");
    }
    line(mouseX, mouseY, pmouseX, pmouseY);
  }
  fill(#b5b5b0);
  ellipse(100,400,50,50);
  if(remoteKey == "button"){
     colorIndex+=1;
    if(colorIndex>=colors.length){
      colorIndex=0;
    }
  }else{
    System.out.println("button "+colorIndex);
    fill(colors[colorIndex]);
    ellipse(100,400,50,50);
  }
}



void keyPressed(){
 
  /*else*/ if (key==CODED) {
    if(keyCode==DOWN){
      remoteKey="button";
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
