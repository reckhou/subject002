using UnityEngine;
using System.Collections;

public class charMover : MonoBehaviour {
	public float moveXRange, speed;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		Vector3 targetPos = transform.localPosition;
		targetPos.x += Input.acceleration.x;
		if (Mathf.Abs(targetPos.x) > moveXRange) {
			if (targetPos.x >= 0) {
				targetPos.x = moveXRange;
			} else {
				targetPos.x = -moveXRange;
			}
		}
		transform.localPosition = Vector3.Lerp(transform.localPosition, targetPos, speed * Time.deltaTime);
	}
}
