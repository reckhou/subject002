using UnityEngine;
using System.Collections;

public class cloudMover : MonoBehaviour {
	public float startX, endX;
	public float speed;
	public bool moveToLeft;

	// Use this for initialization
	void Start () {
		Vector3 pos = transform.position;
		pos.x = startX;
		transform.position = pos;
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 pos = transform.position;
		if (moveToLeft) {
			if (pos.x <= endX) {
				pos.x = startX;
				transform.position = pos;
			} else {
				pos.x -= speed * Time.deltaTime;
				transform.position = pos;
			}
		} else {
			if (pos.x >= endX) {
				pos.x = startX;
				transform.position = pos;
			} else {
				pos.x += speed * Time.deltaTime;
				transform.position = pos;
			}
		}
	}
}
