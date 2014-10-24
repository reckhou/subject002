using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class startSceneController : MonoBehaviour {

	public Image FlashText;
	public float FlashSpeed;

	private float lastFlashUpdate;
	private float flashDeltaTime;
	private bool isTextActive = true;

	// Use this for initialization
	void Start () {
		singleMusicInstance.Instance.Play(0);
		if (FlashSpeed <= 0) {
			FlashSpeed = 1;
		}
		flashDeltaTime = 1 / FlashSpeed;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown("space")) {
			Application.LoadLevel("testScene");
		}

		if (Time.time - lastFlashUpdate > flashDeltaTime) {
			lastFlashUpdate = Time.time;
			isTextActive = !isTextActive;
			FlashText.gameObject.SetActive(isTextActive);
		}
	}
}
