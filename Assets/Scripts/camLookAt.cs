using UnityEngine;
using System.Collections;

public class camLookAt : MonoBehaviour {

	public Transform target;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 relativePos = target.position - transform.position;
		transform.rotation = Quaternion.LookRotation(relativePos);
//		transform.LookAt(target);
	}
}
