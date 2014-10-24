using UnityEngine;
using System.Collections;

public class removeMob : MonoBehaviour {
	// Update is called once per frame
	void OnTriggerEnter(Collider other) {
		Destroy(other.gameObject);
	}
}
