using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

public class ReportTextController : MonoBehaviour {
	public float TextSpeed = 10;
	public AudioSource Type1Audio;
	public AudioSource Type2Audio;
	public AudioSource EOFAudio;
	private float textDeltaTime;
	private List<char> textBuffer;
	private float lastUpdateTime;
	public bool isPlaying;
	private bool playAudio1;

	// Use this for initialization
	void Start () {
		if (TextSpeed != 0) {
			textDeltaTime = 1.0f/TextSpeed;
		} else {
			textDeltaTime = 1;
		}
	}
	
	public void SetText(string text) {
		textBuffer = new List<char> (text.ToCharArray ());
		GetComponent<Text> ().text = "";
	}

	public void ClearText() {
		textBuffer = new List<char> ();
		isPlaying = false;
		GetComponent<Text> ().text = "";
	}

	public void Continue() {
		if (isPlaying) {
			return;
		}
		isPlaying = true;
		updateText();
	}
	
	// Update is called once per frame
	void Update () {
		if (textBuffer == null || textBuffer.Count < 1) {
			return;
		}

		if (textBuffer[0] == '\n' && textBuffer[1] != '\n') {
			isPlaying = false;
			return;
		}

		if (!isPlaying) {
			return;
		}

		if (Time.time - lastUpdateTime > textDeltaTime) {
			updateText();
			lastUpdateTime = Time.time;
		}
	}

	void updateText() {
		string curText = GetComponent<Text> ().text;
		char curChar = textBuffer [0];
		curText += textBuffer[0];
		GetComponent<Text> ().text = curText;
		textBuffer.RemoveAt(0);

		if (curChar == '\n') {
			EOFAudio.Play ();		
		} else if (!Type1Audio.isPlaying && !Type2Audio.isPlaying) {
			int iSeed = 10; 
			System.Random ro = new System.Random (1000); 
			long tick = DateTime.Now.Ticks; 
			System.Random ran = new System.Random ((int)(tick & 0xffffffffL) | (int)(tick >> 32)); 
			int a = ran.Next (1, 3);

			if (playAudio1) {
				Type1Audio.Play ();
				playAudio1 = !playAudio1;
			} else {
				Type2Audio.Play ();		
			}
		}
	}

	public bool AllTextPlayed() {
		if (textBuffer != null && textBuffer.Count > 0) {
			return false;		
		}

		return true;
	}
}
