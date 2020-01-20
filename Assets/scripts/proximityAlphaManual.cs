using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class proximityAlphaManual : MonoBehaviour {

  public float proximity;
  public Transform target;

  void Start() {

  }

  void Update () {
    // float proximity = globalVars.markupAlphaRadius;
    // Transform target = globalVars.markupHitbox.transform;
    GetComponent<Renderer>().material.SetFloat("_Alpha", Mathf.InverseLerp (proximity, 0.0f, Vector3.Distance (target.position, transform.position)));
    GetComponent<Renderer>().material.SetFloat("_Brightness", Mathf.InverseLerp (proximity, 0.0f, Vector3.Distance (target.position, transform.position)));
  }

}
