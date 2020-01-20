using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class busy : MonoBehaviour {
  public GameObject matSelector;
  public float scale = .1f;
  public float scaleT = .1f;
  void Start() {
    
  }

  void Update() {

  }
  
  void OnCollisionEnter(Collision col) {
    if (col.gameObject.tag == "material") {
      globalVars.busy = true;
      globalVars.matSelect = true;
      globalVars.busySelect = col.gameObject;
      col.gameObject.transform.localScale += new Vector3(scale, scale, scale);
    }
    if (col.gameObject.tag == "layer") {
      globalVars.busy = true;
      globalVars.laySelect = true;
      globalVars.busySelect = col.gameObject;
      // col.gameObject.transform.localScale += new Vector3(scaleT, scaleT, scaleT);
    }
  }
  
  void OnCollisionExit(Collision col) {
    if (col.gameObject.tag == "material") {
      globalVars.busy = false;
      col.gameObject.transform.localScale -= new Vector3(scale, scale, scale);
    }
    if (col.gameObject.tag == "layer") {
      globalVars.busy = false;
      globalVars.laySelect = false;
      globalVars.busySelect = col.gameObject;
      // col.gameObject.transform.localScale -= new Vector3(scaleT, scaleT, scaleT);
    }
  }
}
