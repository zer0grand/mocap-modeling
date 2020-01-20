using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class cameraGen : MonoBehaviour {
	public Camera camLeft;
	public Camera camRight;
	public GameObject rectLeft;
	public GameObject rectRight;
	private Camera camera;
	private RenderTexture rend;
	
	void Start () {
		camera = camLeft;
		if (camera.targetTexture != null) {
			camera.targetTexture.Release();
		}
		rend = new RenderTexture(Screen.width/2, Screen.height, 24);
		camera.targetTexture = rend;
		rectLeft.GetComponent<RawImage>().texture = rend;
		
		
		
		camera = camRight;
		if (camera.targetTexture != null) {
			camera.targetTexture.Release();
		}
		rend = new RenderTexture(Screen.width/2, Screen.height, 24);
		camera.targetTexture = rend;
		rectRight.GetComponent<RawImage>().texture = rend;
	}
}
