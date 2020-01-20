using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class materialLibrary : MonoBehaviour {
  public GameObject libraryObj;
  public GameObject materialList;
  public GameObject toolTip;
  private bool matInit = false;
  public bool moveable = false;
  public bool initMove = false;
  
  void Start() {
  }
  
  void OnCollisionEnter(Collision col) {
    if (col.gameObject == toolTip) {
      moveable = true;
    }
  }
  void OnCollisionExit(Collision col) {
    if (col.gameObject == toolTip) {
      moveable = false;
    }
  }

  void Update() {
    if (initMove) {
      libraryObj.transform.position = toolTip.transform.position;
      libraryObj.transform.rotation = toolTip.transform.rotation;
      if (Input.GetButtonDown("moveOFF")) {
        initMove = false;
      }
    }
    if (moveable && globalVars.moving) {
      libraryObj.transform.position = toolTip.transform.position;
      libraryObj.transform.rotation = toolTip.transform.rotation;
    }
    if (!matInit && globalVars.materialLibrary) {
      libraryObj.transform.GetChild(0).gameObject.SetActive(true);
      initMove = true;
    } else if (!globalVars.materialLibrary) {
      libraryObj.transform.GetChild(0).gameObject.SetActive(false);
    }
    matInit = globalVars.materialLibrary;
  }
}
