using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class Trial
{
  public string Face;
  public string Zoom;

  Trial(string f, string z){
    Face = f;
    Zoom = z;
  }
}
