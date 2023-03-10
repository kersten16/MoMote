import processing.opengl.*;

class Cube{
color faces [] = new color[6];
float xAngle=-PI/8;
float yAngle=PI/10;

Cube(){
  for(int i=0; i<6;i++){
    faces[i]= color(100,0,0);
  }
}

void changeColor( float value){
  faces[0]=color(20*(value%5),255-value,value);
}

void updateFace(int rotation){
  color temp [] = new color[6];
  switch(rotation){
        case 0:
        // y=-1
          temp[0]=faces[4];
          temp[1]=faces[0];
          temp[2]=faces[2];
          temp[3]=faces[3];
          temp[4]=faces[5];
          temp[5]=faces[1];
        break;
        case 1:
        //x=1y=-1
          temp[0]=faces[2];
          temp[1]=faces[0];
          temp[2]=faces[1];
          temp[3]=faces[4];
          temp[4]=faces[5];
          temp[5]=faces[3];
        break;
        case 2:
        //x=1
          temp[0]=faces[2];
          temp[1]=faces[1];
          temp[2]=faces[5];
          temp[3]=faces[0];
          temp[4]=faces[4];
          temp[5]=faces[3];
        break;
        case 3:
        //x=1y=1
          temp[0]=faces[1];
          temp[1]=faces[3];
          temp[2]=faces[5];
          temp[3]=faces[0];
          temp[4]=faces[2];
          temp[5]=faces[4];
        break;
        case 4:
        //y=1
          temp[0]=faces[1];
          temp[1]=faces[5];
          temp[2]=faces[2];
          temp[3]=faces[3];
          temp[4]=faces[0];
          temp[5]=faces[4];
        break;
        case 5:
        //x=-1y=1
          temp[0]=faces[3];
          temp[1]=faces[5];
          temp[2]=faces[1];
          temp[3]=faces[4];
          temp[4]=faces[0];
          temp[5]=faces[2];
        break;
        case 6:
        //x=-1
          temp[0]=faces[3];
          temp[1]=faces[1];
          temp[2]=faces[0];
          temp[3]=faces[5];
          temp[4]=faces[4];
          temp[5]=faces[2];
        break;
        case 7:
        //x=-1y=-1
          temp[0]=faces[4];
          temp[1]=faces[3];
          temp[2]=faces[0];
          temp[3]=faces[5];
          temp[4]=faces[2];
          temp[5]=faces[1];
        break;
        case 8:
        //no movement
        temp=faces;
        break;
      }
      faces=temp;
}

//void draw()
//{
//  background(0, 100);

//  directionalLight(204, 204, 204, 0, 0, -1); 

//  translate(width/2 + transx, height/2 + transy, 0);
//  scale(100);

//  rotateX(rotx);
//  rotateY(roty);

//  drawCube();
//}

void drawCube(float xChange, float yChange)
{
  translate(250 , 350, 0);
  xAngle=xAngle+xChange;
  yAngle= yAngle+yChange;
  rotateX(xAngle);
  rotateY(yAngle);
  beginShape(QUAD);

  // +Z "front" face
  fill(faces[0]);
  vertex(-100, -100, 100, 0, 84);
  vertex( 100, -100, 100, 84, 84);
  vertex( 100, 100, 100, 84, 0);
  vertex(-100, 100, 100, 0, 0);
  //vertex(-100, -100,  1);
  endShape();

  beginShape(QUAD);
  // -Z "back" face
  fill(faces[5]);
  vertex( -100, -100, -100, 0, 84);
  vertex(100, -100, -100, 84, 84);
  vertex( 100, 100, -100, 84, 0);
  vertex(-100, 100, -100, 0, 0);
  endShape();

  beginShape(QUAD);
  fill(faces[1]);
  // +Y "bottom" face
  vertex(-100, 100, 100, 0, 84);
  vertex( 100, 100, 100, 84, 84);
  vertex( 100, 100, -100, 84, 0);
  vertex(-100, 100, -100, 0, 0);
  endShape();

  beginShape(QUAD);
  fill(faces[4]);
  // -Y "top" face
  vertex(-100, -100, -100, 0, 84);
  vertex( 100, -100, -100, 84, 84);
  vertex( 100, -100, 100, 84, 0);
  vertex(-100, -100, 100, 0, 0);
  endShape();

  beginShape(QUAD);
  fill(faces[2]);
  // +X "right" face
  vertex( 100, -100, 100, 0, 84);
  vertex( 100, -100, -100, 84, 84);
  vertex( 100, 100, -100, 84, 0);
  vertex( 100, 100, 100, 0, 0);
  endShape();

  beginShape(QUAD);
  fill(faces[3]);
  // -X "left" face
  vertex(-100, -100, -100, 0, 84);
  vertex(-100, -100, 100, 84, 84);
  vertex(-100, 100, 100, 84, 0);
  vertex(-100, 100, -100, 0, 0);
  endShape();
}


void resetView() 
{
  xAngle=-PI/8;
  yAngle=PI/10;
}

}
