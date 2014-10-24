using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Text;
using System.IO;
using System.Collections.Generic;

public class BeatController : MonoBehaviour {
	public List<BeatObject> BeatObjects;
	private TextAsset BeatFile;

	public List<float> BeatTimeList = new List<float>();
	private float curTime = 0;
	private int curBeat = 0;

	private float averageUpdateGap;
	private int updateCnt;

	private float nextBarTime; // BeatTimeList[i+2] - BeatTimeList[i]

	public Text path;

	public CamController camController;

	private bool LoadBeat(string fileName)
	{
		BeatFile = Resources.Load("Beats/"+fileName) as TextAsset;
		if (BeatFile == null) {
			return false;
		}

		string text = BeatFile.text;
		string[] sArray= text.Split('\n');
		foreach (string curStr in sArray) {
			float cur;
			try {
				cur = float.Parse(curStr, System.Globalization.CultureInfo.InvariantCulture);
			} catch {
				continue;
			}
			BeatTimeList.Add(cur);
		}

		return true;
    }

	// Use this for initialization
	void Start () {
		path.text = "";
		var result = LoadBeat(GlobalConfig.Music);
//		Debug.Log(BeatFile.text);
		if (!result) {
			Debug.Log("Read beat:" + GlobalConfig.Music + " error!");
			return;
		}

		AudioClip clip = Resources.Load("Musics/"+GlobalConfig.Music) as AudioClip;
		audio.clip = clip;

		// init all beat objects
		updateNextBarTime();
		for (int i = 0; i < BeatObjects.Count; i++) {
			BeatObjects[i].Init(nextBarTime);
			updateBeatObject(BeatObjects[i]);    
    	}
  }
	
	// Update is called once per frame
	void Update () {
		if (!audio.isPlaying) {
			audio.Play();
		}
		curTime += Time.deltaTime;
		if (updateCnt != 0) {
			averageUpdateGap = curTime / updateCnt;
		} else {
			updateCnt++;
			return;
		}
//		Debug.Log(curTime+ " " + BeatTimeList[curBeat]);
		while (curTime + averageUpdateGap >= BeatTimeList[curBeat]) {
			curBeat++;
			updateNextBarTime();
//			camController.SwitchCam();
			for (int i = 0; i < BeatObjects.Count; i++) {
				updateBeatObject(BeatObjects[i]);
				BeatObjects[i].Beat();
			}
		}

		updateCnt++;
	}

	private void updateBeatObject(BeatObject obj) {
		if (obj is BeatObjectDefault) {
			BeatObjectDefault curObj = obj as BeatObjectDefault;
			curObj.BarTime = nextBarTime;
    	}
  	}

	private void updateNextBarTime() {
		if (curBeat %4 != 0 || curBeat + 4 > BeatTimeList.Count) {
			return;
		}

		nextBarTime = BeatTimeList[curBeat+4] - BeatTimeList[curBeat];
//		print (nextBarTime);
	}
}
