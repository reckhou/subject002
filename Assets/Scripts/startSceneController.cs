using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class startSceneController : MonoBehaviour {

	public GameObject FlashText;
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

        if (Local.Instance._LangType == Local.eLangType.English)
        {
            FlashText.GetComponent<Text>().text = "Press SPACE to Begin Experiment";
        } else if (Local.Instance._LangType == Local.eLangType.Chinese)
        {
            FlashText.GetComponent<Text>().text = "按 SPACE 键开始实验";
        }
    }
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown("space")) {
			Application.LoadLevel("mainScene");
		}

		if (Time.time - lastFlashUpdate > flashDeltaTime) {
			lastFlashUpdate = Time.time;
			isTextActive = !isTextActive;
			FlashText.gameObject.SetActive(isTextActive);
		}
	}
}
