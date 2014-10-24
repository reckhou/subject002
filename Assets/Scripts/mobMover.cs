using UnityEngine;
using System.Collections;

public class mobMover : MonoBehaviour {
	[Range(0.1f, 2.0f)] public float speed;
	public float deltaX;
	public float scaleFactor;

	private float scaleDeltaStep;

	private float moveTime;
	void Start() {
		Vector3 scale = transform.localScale;
		transform.localScale *= scaleFactor;
		moveTime = 7.05f / speed; // 7.05: total range of deltaY
		scaleDeltaStep = (1.0f - scaleFactor) * scale.x / moveTime;
//		scaleDeltaStep = 0.4f;
//		Debug.Log(moveTime);
//		Debug.Log(scaleDeltaStep);
	}

	void Update() {

	}

	void FixedUpdate() {
		Vector3 moveVector = new Vector3(deltaX / moveTime, -1, 0);
		transform.Translate(moveVector * speed * Time.deltaTime);

		Vector3 scale = transform.localScale;
		scale.x += scaleDeltaStep * Time.deltaTime;
		scale.y += scaleDeltaStep * Time.deltaTime;
		transform.localScale = scale;

	}
}
