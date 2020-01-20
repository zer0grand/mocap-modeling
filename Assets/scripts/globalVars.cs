using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class globalVars : MonoBehaviour {
  // controller
  public static bool click = false;
  public static bool firstClick = false;
  public bool cancel = false;

  // brush and line
  private LineRenderer currLine;
  private int numClicks = 0;
  
  // layers
  public GameObject layers;
  public static GameObject currentLayer;
  public static bool layerMenu;
  public static bool laySelect;
  public Material layerActive;
  public Material layerInactive;
  public Material eyeActive;
  public Material eyeInactive;
  public GameObject defaultLayer;
  public static bool newLayer;
  public GameObject layerActual;
  
  public bool drawing = false;
  
  // selection
  public static string selectionMode = "component";
  private List<GameObject> selection;
  
  // materials
  public static bool materialLibrary = false;
  public static List<Material> materials = new List<Material>();
  public static Material currentMaterial;
  public static bool matSelect;
  
  public static bool moving = false;
  
  public GameObject vertexObject;
  public Material lineMat;
  public Material componentMat;
  public float lineWidth;
  
  public GameObject _markupHitbox;
  public static GameObject markupHitbox;
  public GameObject _toolPoint;
  public static GameObject toolPoint;
  public GameObject toolTip;
  public GameObject brushTip;
  
  public static string tool = "brush";
  public static string toolOld = "brush";
  public static bool planelock = true;
  public static GameObject currLayer;
  public static GameObject currGroup;
  public static GameObject currComponents;
  
  public static bool busy = false;
  public static GameObject busySelect;
  
  public static float markupAlphaRadius = 4f;
    
  void Start() {
    markupHitbox = _markupHitbox;
    toolPoint = _toolPoint;
    GameObject derp = Instantiate(defaultLayer.transform.parent.gameObject, layers.transform);
    derp.SetActive(true);
    derp.transform.GetChild(0).GetComponent<Renderer>().material = layerActive;
    currentLayer = new GameObject("layer");
    currentLayer.transform.parent = layerActual.transform;
  }

  void Update() {
    if (click == false && Input.GetButtonDown("clickON")) {
      firstClick = true;
    } else {
      firstClick = false;
    }
    if (Input.GetButtonDown("clickON") && !busy) {
      click = true;
    }
    if (Input.GetButtonDown("clickOFF")) {
      click = false;
    }
    if (Input.GetButtonDown("cancelON")) {
      cancel = true;
    }
    if (Input.GetButtonDown("cancelOFF")) {
      cancel = false;
    }
    if (Input.GetButtonDown("moveON")) {
      moving = true;
    }
    if (Input.GetButtonDown("moveOFF")) {
      moving = false;
    }
    
    if (newLayer) {
      GameObject uiLayer = Instantiate(defaultLayer.transform.parent.gameObject, layers.transform);
      uiLayer.SetActive(true);
      layers.transform.GetChild(currentLayer.transform.GetSiblingIndex()+1).GetChild(0).GetComponent<Renderer>().material = layerInactive;
      uiLayer.transform.GetChild(0).GetComponent<Renderer>().material = layerActive;
      currentLayer = new GameObject("layer");
      currentLayer.transform.parent = layerActual.transform;
      uiLayer.transform.localPosition += new Vector3((layers.transform.childCount-2)*.829f, 0, 0);
      newLayer = false;
    }
    
    if (click) {
      if (tool == "brush") {
        if (firstClick) { // brush init
          GameObject go = new GameObject();
          go.transform.parent = currentLayer.transform;
          currLine = go.AddComponent<LineRenderer>();
          currLine.material = currentMaterial;
          currLine.SetWidth(.1f, .1f);
          numClicks = 0;
        } else { // after init
          currLine.SetVertexCount(numClicks+1);
          currLine.SetPosition(numClicks, toolPoint.transform.position);
          numClicks++;
        }
      }
    }
    
    if (Input.GetButtonDown("clickON")) {
      if (matSelect && busy) {
        currentMaterial = busySelect.GetComponent<Renderer>().material;
        brushTip.GetComponent<Renderer>().material = currentMaterial;
        return;
      }
      if (laySelect && busy) {
        if (busySelect.name == "layer") {
          layers.transform.GetChild(currentLayer.transform.GetSiblingIndex()+1).GetChild(0).GetComponent<Renderer>().material = layerInactive;
          currentLayer = layerActual.transform.GetChild(busySelect.transform.parent.GetSiblingIndex()-1).gameObject;
          busySelect.GetComponent<Renderer>().material = layerActive;
        }
        if (busySelect.name == "eye") {
          GameObject layer = layerActual.transform.GetChild(busySelect.transform.parent.parent.GetSiblingIndex()-1).gameObject;
          // GameObject layer = busySelect.transform.parent.parent.GetChild(0).GetChild(0).gameObject;
          if (layer.activeSelf) {
            layer.SetActive(false);
            busySelect.GetComponent<Renderer>().material = eyeInactive;
          } else {
            layer.SetActive(true);
            busySelect.GetComponent<Renderer>().material = eyeActive;
          }
        }
        if (busySelect.name == "plus") {
          GameObject uiLayer = Instantiate(defaultLayer.transform.parent.gameObject, layers.transform);
          uiLayer.SetActive(true);
          layers.transform.GetChild(currentLayer.transform.GetSiblingIndex()+1).GetChild(0).GetComponent<Renderer>().material = layerInactive;
          uiLayer.transform.GetChild(0).GetComponent<Renderer>().material = layerActive;
          currentLayer = new GameObject("layer");
          currentLayer.transform.parent = layerActual.transform;
          uiLayer.transform.localPosition += new Vector3((layers.transform.childCount-2)*.829f, 0, 0);
        }
        return;
      }

      // line tool
      if (tool == "line" && !busy) {
        if (drawing == false) { // line init
          currGroup = new GameObject("line");
          currComponents = new GameObject("components");
          currComponents.transform.parent = currGroup.transform;
          currGroup.transform.parent = currentLayer.transform;
          GameObject go = new GameObject();
          go.transform.parent = currGroup.transform;
          currLine = go.AddComponent<LineRenderer>();
          currLine.GetComponent<Renderer>().material = currentMaterial;
          currLine.SetWidth(.1f, .1f);
          numClicks = 0;
          
          // create line vertex
          currLine.SetVertexCount(numClicks+1);
          currLine.SetPosition(numClicks, toolPoint.transform.position);
          numClicks++;
          
          // create cube vertex
          GameObject vertex = Instantiate(vertexObject, new Vector3(0, 0, 0), Quaternion.identity);
          vertex.AddComponent<BoxCollider>();
          Rigidbody body = vertex.AddComponent<Rigidbody>();
          body.useGravity = false;
          body.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ;
          vertex.GetComponent<Renderer>().material = componentMat;
          vertex.transform.localScale = new Vector3(lineWidth, lineWidth, lineWidth);
          vertex.transform.position = toolPoint.transform.position;
          vertex.transform.parent = currComponents.transform;
          
          // create current vertex
          currLine.SetVertexCount(numClicks+1);
          currLine.SetPosition(numClicks, toolPoint.transform.position);
          numClicks++;
          
          drawing = true;
          
        } else { // after init
          // create cube vertex
          GameObject vertex = Instantiate(vertexObject, new Vector3(0, 0, 0), Quaternion.identity);
          vertex.AddComponent<BoxCollider>();
          Rigidbody body = vertex.AddComponent<Rigidbody>();
          body.useGravity = false;
          body.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ;
          vertex.GetComponent<Renderer>().material = componentMat;
          vertex.transform.localScale = new Vector3(lineWidth, lineWidth, lineWidth);
          vertex.transform.position = toolPoint.transform.position;
          vertex.transform.parent = currComponents.transform;
          
          // create line vertex
          currLine.SetVertexCount(numClicks+1);
          currLine.SetPosition(numClicks, toolPoint.transform.position);
          numClicks++;
        }
      } else if (tool == "marker" && !busy) {
        GameObject vertex = Instantiate(vertexObject, toolTip.transform.position, Quaternion.identity);
        vertex.transform.parent = currentLayer.transform;
        vertex.AddComponent<BoxCollider>();
        Rigidbody body = vertex.AddComponent<Rigidbody>();
        body.useGravity = false;
        body.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ;
        vertex.GetComponent<Renderer>().material = currentMaterial;
        vertex.transform.localScale = new Vector3(100, 100, 100);
        vertex.transform.position = toolTip.transform.position;
      }
      
    }

    
    if (drawing == true && tool == "line") { // move current vertex to controller
      currLine.SetPosition(numClicks-1, toolPoint.transform.position);
    }
    
    
    if (Input.GetButtonDown("cancelON") || Input.GetButtonDown("markupON")) {
      if (drawing == true) {
        currLine.SetVertexCount(numClicks-1);
        if (numClicks < 3) {
          Destroy(currLine.transform.parent.gameObject);
        }
        drawing = false;
      }
    }
  }

  
}
