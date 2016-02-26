using UnityEngine;
using System.Collections;

public class endSceneController : MonoBehaviour {
	public float DisplayTime = 5;
	private float startDisplay;

	// Use this for initialization
	void Start () {
		startDisplay = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.anyKeyDown && Time.time - startDisplay > DisplayTime) {
			Application.LoadLevel("startScene");
		}
	}
}
