using UnityEngine;
using System.Collections;

public class CamController : MonoBehaviour {
	public Camera camA;
	public Camera camB;

	// Use this for initialization
	void Start () {
		camA.enabled = true;
		camB.enabled = false;
	
	}
	
	// Update is called once per frame
	public void SwitchCam () {
		camA.enabled = !camA.enabled;
		camB.enabled = !camB.enabled;
	}
}
