using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class proximityAlphaMesh : MonoBehaviour {

  void Start() {

  }
  
  // void SetAlpha (this Material material, float value) {
  //   Color color = material.color;
  //   color.a = value;
  //   material.color = color;
  // }

  void Update () {
    float proximity = globalVars.markupAlphaRadius;
    Transform target = globalVars.markupHitbox.transform;
    
    Color color = GetComponent<Renderer>().material.color;
    color.a = Mathf.InverseLerp (proximity, 0.0f, Vector3.Distance (target.position, transform.position));

    GetComponent<Renderer>().material.SetColor("_Color", color);
    
  }

}
