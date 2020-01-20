using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class layers : MonoBehaviour {
  public GameObject layersObj;
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
      layersObj.transform.position = toolTip.transform.position;
      layersObj.transform.rotation = toolTip.transform.rotation;
      if (Input.GetButtonDown("moveOFF")) {
        initMove = false;
      }
    }
    if (moveable && globalVars.moving) {
      layersObj.transform.position = toolTip.transform.position;
      layersObj.transform.rotation = toolTip.transform.rotation;
    }
    if (!matInit && globalVars.layerMenu) {
      layersObj.transform.GetChild(0).gameObject.SetActive(true);
      initMove = true;
    } else if (!globalVars.layerMenu) {
      layersObj.transform.GetChild(0).gameObject.SetActive(false);
    }
    matInit = globalVars.layerMenu;
  }
}
