using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ChatBox : MonoBehaviour
{
		public Text TextBox;
		public float TextSpeed;
		private float textDeltaTime;
		private List<string> textList;
		private List<char> textBuffer;
		private float lastUpdateTime;
		private GameController gameController;
		private Image avatar;

		public Sprite DoctorAvatar;
		public Sprite SubjectAvatar;
		public AudioSource pressAudio;
		public AudioSource typeAudio;
		public Button nextButton;

		public bool playTypeAudio;

		private bool textPlaying;
		public bool isDebug;

		public enum Character
		{
				Doctor,
				Subject}

		;

		void Awake ()
		{
				textBuffer = new List<char> ();
        textList = new List<string>();
    }

		// Use this for initialization
		void Start ()
		{
        if (Local.Instance._LangType == Local.eLangType.Chinese)
        {
            TextBox.lineSpacing = 1.5f;
        } else if (Local.Instance._LangType == Local.eLangType.English)
        {
            TextBox.lineSpacing = 1.0f;
        }

				avatar = GameObject.Find ("Avatar").GetComponent<Image> ();

				if (TextSpeed != 0) {
						textDeltaTime = 1.0f / TextSpeed;
				} else {
						textDeltaTime = 1;
				}
						
				gameController = GameObject.Find ("GameController").GetComponent<GameController> ();
				if (gameController == null) {
						print ("failed to get game controller!");
				}

				
				playTypeAudio = true;
		}

		public void SetAvatar (Character type)
		{
				if (type == Character.Doctor) {
						avatar.sprite = DoctorAvatar;
				} else if (type == Character.Subject) {
						avatar.sprite = SubjectAvatar;
				}
		}

		public void SetText (List<string> list)
		{
				playTypeAudio = true;
				clearText ();
				textList = new List<string> ();
				if (list == null) {
						return;
				}

				textList.AddRange (list);
				for (int i = 0; i < textList.Count; i++) {
						textList [i] += "\n";
				}
				PlayLine ();
		}

		public void AddText (string text)
		{
				textList.Add (text + "\n");
		}

		public void PlayLine ()
		{
				updateButtonStatus ();
				if (textList.Count < 1) {
						textEnd ();
						return;
				}

				if (textList [0] == "\r\n") {
						clearText ();
						textList.RemoveAt (0);
				}

				if (textList.Count < 1) {
						textEnd ();
						return;
				}
				List<char> tmpList = textList [0].ToList ();
				textBuffer.AddRange (tmpList);
				textBuffer.Add ('\n');
				textList.RemoveAt (0);
				if (!typeAudio.isPlaying && playTypeAudio) {
						typeAudio.Play ();
				}
		}

		void textEnd ()
		{
        Debug.Log("textend");
				gameController.TextEndCallback ();
		}

		public void PlayPressAudio ()
		{
				pressAudio.Play ();
		}

		public void Update ()
		{
				updateButtonStatus ();
		}

		void updateButtonStatus ()
		{
				if (textPlaying && !isDebug) {
						nextButton.interactable = false;
						return;		
				} else {
						nextButton.interactable = true;
				}
		}

		public void CheckStatus ()
		{
				if (Time.time - lastUpdateTime > textDeltaTime) {
						updateText ();
						lastUpdateTime = Time.time;
				}
		
//		if (Input.GetKeyUp("space")) {
//			PlayLine();
//    	}
		}

		private void updateText ()
		{
				if (textBuffer == null) {
						textBuffer = new List<char> ();
				}

				if (textBuffer.Count < 1) {
						textPlaying = false;
						return;
				}

				textPlaying = true;
				string curText = TextBox.text;
				curText += textBuffer [0];
				TextBox.text = curText;
				textBuffer.RemoveAt (0);

				if (!typeAudio.isPlaying && playTypeAudio) {
						typeAudio.Play ();
				}
		}

		private void clearText ()
		{
				textBuffer.Clear ();
				TextBox.text = "";
		}
}
