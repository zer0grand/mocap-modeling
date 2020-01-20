using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System;

public class markup : MonoBehaviour {

  public AudioClip markupIn;
  public AudioClip markupOut;
  public AudioClip markupSelect;
  public AudioSource MusicSource;

  public GameObject controller;
  public GameObject controllerPoint;
  public GameObject markupMenu;
  // public List<GameObject> menus;
  public Material highlight;
  public Material unhighlight;
  public Stack<GameObject> path = new Stack<GameObject>();
  
  public GameObject brushTip;

  public class MarkupClass {
    
    // tools
    public string MarkupBrush() {
      globalVars.tool = "brush";
      return "0";
    }
    public string MarkupPlanelock() {
      bool temp = globalVars.planelock;
      if (temp) {
        globalVars.planelock = false;
      } else {
        globalVars.planelock = true;
      }
      return "0";
    }
    public string MarkupMarker() {
      globalVars.tool = "marker";
      return "0";
    }
    public string MarkupLine() {
      globalVars.tool = "line";
      return "0";
    }
    
    // select
    public string MarkupSelectLayers() {
      globalVars.selectionMode = "layers";
      return "0";
    }
    public string MarkupSelectObjects() {
      globalVars.selectionMode = "objects";
      return "0";
    }
    public string MarkupSelectComponents() {
      globalVars.selectionMode = "components";
      return "0";
    }
    public string MarkupSelectAll() {
      return "0";
    }
    public string MarkupSelectNone() {
      return "0";
    }
    
    // materials
    public string MarkupNewMaterial() {
      globalVars.toolOld = globalVars.tool;
      globalVars.tool = "colorPicking";
      return "0";
    }
    public string MarkupMaterialLibrary() {
      globalVars.materialLibrary = !globalVars.materialLibrary;
      return "0";
    }
    
    // layers
    public string MarkupLayerMenu() {
      globalVars.layerMenu = !globalVars.layerMenu;
      return "0";
    }
    public string MarkupNewLayer() {
      globalVars.newLayer = true;
      return "0";
    }
    
    // edit
    public string MarkupDelete() {
      globalVars.tool = "delete";
      return "0";
    }
    public string MarkupUndo() {
      return "0";
    }
    public string MarkupRedo() {
      return "0";
    }

  }

  void Start() {
    
  }


  void CleanStack() {
    MusicSource.clip = markupOut;
    MusicSource.Play();
    while (path.Count != 0) {
      GameObject temp = path.Pop();
      temp.transform.GetComponent<MeshRenderer>().material = unhighlight;
      temp.transform.parent.parent.gameObject.SetActive(false);
      for (int i=0; i<temp.transform.parent.parent.childCount; i++) { // disable siblings
        if (temp.transform.parent.parent.GetChild(i) != temp.transform.parent) {
          temp.transform.parent.parent.GetChild(i).gameObject.SetActive(true);
        }
      }
    }
    brushTip.SetActive(true);
  }

  void Update() {
    if (Input.GetButtonDown("markupON")) {
      brushTip.SetActive(false);
      if (globalVars.tool == "colorPicking") {
        globalVars.tool = globalVars.toolOld;
      }
      if (path.Count == 0) { // if menu is disabled
        markupMenu.transform.position = controllerPoint.transform.position;
        markupMenu.transform.rotation = controller.transform.rotation;
        markupMenu.SetActive(true);
        
        // MusicSource.clip = markupIn;
        // MusicSource.Play();
      }
    }
    if (Input.GetButtonDown("markupOFF") || Input.GetButtonDown("cancelON")) {
      // get function to be executed
      Type type = typeof(MarkupClass);
      GameObject temp = path.Peek();
      if (temp.name != "NULL") {
        MethodInfo method = type.GetMethod("Markup"+temp.name);
        MarkupClass c = new MarkupClass();
        string result = (string)method.Invoke(c, null);
        MusicSource.clip = markupOut;
        MusicSource.Play();
      }
      CleanStack();
    }
  }

  void OnCollisionEnter(Collision col) {
    if (col.gameObject.tag == "markup") {
      if (path.Count > 0 && path.Peek() == col.gameObject) {
        return;
      }
      if (path.Contains(col.gameObject) && col.gameObject != markupMenu.transform.GetChild(0).GetChild(0)) { // if tracing backward
        while (path.Peek() != col.gameObject) {
          GameObject temp = path.Pop();
          temp.transform.GetComponent<MeshRenderer>().material = unhighlight;
          temp.transform.parent.parent.gameObject.SetActive(true);
          if (temp.transform.parent.childCount != 1) { // test if leaf node
            temp.transform.parent.GetChild(1).gameObject.SetActive(false);
          }
          for (int i=0; i<temp.transform.parent.parent.childCount; i++) { // enable parent siblings
            if (temp.transform.parent.parent.GetChild(i) != temp.transform.parent) {
              temp.transform.parent.parent.GetChild(i).gameObject.SetActive(true);
            }
          }
        }
        
      } else { // if tracing forward
        if (col.gameObject.transform.parent.childCount > 1) { // normal node
          col.gameObject.transform.parent.GetChild(1).gameObject.SetActive(true); // enable child nodes
          col.gameObject.transform.GetComponent<MeshRenderer>().material = highlight;
          for (int i=0; i<col.gameObject.transform.parent.parent.childCount; i++) { // disable parent siblings
            if (col.gameObject.transform.parent.parent.GetChild(i) != col.gameObject.transform.parent) {
              col.gameObject.transform.parent.parent.GetChild(i).gameObject.SetActive(false);
            }
          }
          if (path.Count > 0) {
            MusicSource.clip = markupSelect;
            MusicSource.Play();
          } else {
            MusicSource.clip = markupIn;
            MusicSource.Play();
          }
          path.Push(col.gameObject);
        } else { // leaf node
          if (path.Count > 0 && col.gameObject.transform.parent.parent == path.Peek().transform.parent.parent) { // check to see if sibling
            path.Pop().transform.GetComponent<MeshRenderer>().material = unhighlight;
          }
          col.gameObject.transform.GetComponent<MeshRenderer>().material = highlight;
          path.Push(col.gameObject);
          MusicSource.clip = markupSelect;
          MusicSource.Play();
        }
      }
    }
  }
}

















