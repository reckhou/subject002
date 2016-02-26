using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


public class SelectBox : MonoBehaviour {
	public Text TextBox;
	public Button[] selectionArray;
	public float TextSpeed;
	private float textDeltaTime;
	private List<string> textList;
	private List<char> textBuffer;
	private float lastUpdateTime;
	private GameController gameController;

	void Start () {
		if (TextSpeed != 0) {
			textDeltaTime = 1.0f/TextSpeed;
		} else {
			textDeltaTime = 1;
		}
		
		textBuffer = new List<char>();
		
		gameController = GameObject.Find("GameController").GetComponent<GameController>();
		if (gameController == null) {
			print ("failed to get game controller!");
		}
	}

	public void AddSelection(string question, string[] selections) {
		CleanUp();
		TextBox.text = question;
		for (int i = 0; i < selectionArray.Length; i++) {
			if (i < selections.Length) {
				selectionArray[i].interactable = true;
			    selectionArray[i].transform.FindChild("ButtonText").GetComponent<Text>().text = selections[i];
			} else {
				selectionArray[i].interactable = false;
				selectionArray[i].transform.FindChild("ButtonText").GetComponent<Text>().text = "";
			}
		}
	}

	public void CleanUp() {
		TextBox.text = "";
		for (int i = 0; i < selectionArray.Length; i++) {
			selectionArray[i].transform.FindChild("ButtonText").GetComponent<Text>().text = "";
		}
	}
}
