using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class colorPicker : MonoBehaviour {
  public Material mainMat;
  public GameObject colors;
  public GameObject pickerMaster;
  public GameObject picker;
  public GameObject displayMaster;
  public GameObject display;
  public GameObject dropper;
  public GameObject dropperDifference;
  public GameObject toolTip;
  public GameObject materialList;
  public GameObject brushTip;
  public Color color = new Color(0,0,0,1);
  private bool newMat = false;
  private bool coll = false;
  
  public GameObject red;
  public GameObject green;
  public GameObject blue;
  public float proximity;
  private bool nextF;
  
  void Start() {
    Material material = new Material(Shader.Find("SFHologram/HologramShader"));
    material.CopyPropertiesFromMaterial(mainMat);
    material.SetColor("_MainColor", new Color(1, 1, 1, 1));
    material.SetColor("_RimColor", new Color(1, 1, 1, 1));
    globalVars.materials.Add(material);
    globalVars.currentMaterial = material;
    
    GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
    sphere.tag = "material";
    
    Rigidbody body = sphere.AddComponent<Rigidbody>();
    body.useGravity = false;
    body.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ;
    
    sphere.transform.SetParent(materialList.transform);
    sphere.transform.localScale = new Vector3(.5f, .5f, .5f);
    sphere.transform.position = materialList.transform.position;
    Renderer rend = sphere.GetComponent<Renderer> ();
    rend.material = material;
    sphere.transform.localPosition += new Vector3(0, 0, 0);
    brushTip.GetComponent<Renderer>().material = material;
  }
  
  void Update() {
    if (nextF) {
      globalVars.busy = false;
      nextF = false;
    }
    if (globalVars.tool == "colorPicking") {
      globalVars.busy = true;
      if (Input.GetButtonDown("cancelON")) {
        picker.SetActive(false);
        newMat = false;
        coll = false;
        globalVars.tool = globalVars.toolOld;
        brushTip.GetComponent<Renderer>().material = globalVars.currentMaterial;
      }
      if (Input.GetButtonDown("clickON")) {
        picker.SetActive(false);
        newMat = false;
        coll = false;
        // Renderer rend = GetComponent<Renderer>();
        Material material = new Material(Shader.Find("SFHologram/HologramShader"));
        material.CopyPropertiesFromMaterial(mainMat);
        material.SetColor("_MainColor", color);
        material.SetColor("_RimColor", color);
        globalVars.materials.Add(material);
        globalVars.click = false;
        globalVars.firstClick = false;
        globalVars.tool = globalVars.toolOld;
        globalVars.currentMaterial = material;
        
        int i = globalVars.materials.Count-1;
        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphere.tag = "material";
        
        Rigidbody body = sphere.AddComponent<Rigidbody>();
        body.useGravity = false;
        body.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ;
        
        sphere.transform.SetParent(materialList.transform);
        sphere.transform.localScale = new Vector3(.5f, .5f, .5f);
        sphere.transform.position = materialList.transform.position;
        Renderer rend = sphere.GetComponent<Renderer>();
        rend.material = material;
        sphere.transform.localPosition += new Vector3((i%4)*.78f, -((i/4)*.78f), 0);
        nextF = true;
        brushTip.GetComponent<Renderer>().material = globalVars.currentMaterial;
      }
      
      if (newMat == false) {
        pickerMaster.transform.position = toolTip.transform.position;
        pickerMaster.transform.rotation = toolTip.transform.rotation;
        picker.SetActive(true);
        newMat = true;

      }
      } else {
        picker.SetActive(false);
        newMat = false;
      }
      
      if (coll) {
        dropper.transform.position = toolTip.transform.position;
        displayMaster.transform.localScale = new Vector3(1, (dropper.transform.localPosition.y - dropperDifference.transform.localPosition.y) / pickerMaster.transform.localScale.y, 1);
        
        Vector3 newpos = colors.transform.position;
        newpos.y = dropper.transform.position.y;
        colors.transform.position = newpos;
        
        
        // Vector3 unitySucks = dropper.transform.position;
        // colors.transform.position = new Vector3(unitySucks);
        
        float rDist = Mathf.InverseLerp(proximity, 0.0f, Vector3.Distance(toolTip.transform.position, red.transform.position));
        float gDist = Mathf.InverseLerp(proximity, 0.0f, Vector3.Distance(toolTip.transform.position, blue.transform.position));
        float bDist = Mathf.InverseLerp(proximity, 0.0f, Vector3.Distance(toolTip.transform.position, green.transform.position));

        rDist += displayMaster.transform.localScale.y-.5f;
        gDist += displayMaster.transform.localScale.y-.5f;
        bDist += displayMaster.transform.localScale.y-.5f;
        Renderer rend = display.GetComponent<Renderer>();
        color = new Color(rDist, gDist, bDist, 1);
        rend.material.SetColor("_MainColor", color);
        rend.material.SetColor("_RimColor", color);
      }
      
    }
  
  void OnCollisionEnter(Collision col) {
    if (col.gameObject == toolTip) {
      coll = true;
    }
  }
  void OnCollisionExit(Collision col) {
    if (col.gameObject == toolTip) {
      coll = false;
    }
  }
  
}
