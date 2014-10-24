using UnityEngine;
using System.Collections;

public class CharController : MonoBehaviour {

	enum Status {
		left,
		right,
		up,
		down,
		jump,
		idle
	};

	public float speed = 1;
	public int status = (int)Status.idle;
	public float jumpForce = 100;

	// Use this for initialization
	void Start () {
	
	}

	public void Move (bool toRight) {
		status = (int)(toRight ? Status.right : Status.left);
	}

	public void Idle () {
		status = (int)Status.idle;
	}

	public void Jump() {
		status = (int)Status.jump;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (status == (int)Status.left) {
			transform.Translate(Vector3.left * Time.deltaTime * speed);
		} else if (status == (int)Status.right) {
			transform.Translate(Vector3.right * Time.deltaTime * speed);
		} else if (status == (int)Status.jump) {
			status = (int)Status.idle;
			rigidbody2D.AddForce(Vector2.up * jumpForce);
//			Rigidbody2D body = GetComponent<Rigidbody2D>();
//			body.AddForce(Vector2.up);
//			transform.Translate(Vector3.up * Time.deltaTime * speed);
		}
	}
}
