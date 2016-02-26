using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class VoiceController : MonoBehaviour {
	public Dictionary<int, Dictionary<int, AudioSource>> Audios;
	public Dictionary<int, Dictionary<int, string>> AudioTexts;
	private static VoiceController instance;
	public static VoiceController Instance {
		get { return instance; }

	}

	void Awake () {
		if (instance != null && instance != this) {
				Destroy (this.gameObject);
				return;
		} else {
			instance = this;		
		}
	}

	// Use this for initialization
	void Start () {
		Audios = new Dictionary<int, Dictionary<int, AudioSource>> ();
		AudioTexts = new Dictionary<int, Dictionary<int, string>> ();
		AudioSource[] sources = transform.GetComponents<AudioSource>();
		foreach (AudioSource tmpSource in sources) {
			string name = tmpSource.clip.name;
			string[] parseArray = name.Split('_');
			if (parseArray.Length < 3) {
				continue;
			}

			int group = int.Parse(parseArray[1]);
			int id = int.Parse(parseArray[2]);

			if (!Audios.ContainsKey(group)) {
				Audios[group] = new Dictionary<int, AudioSource>();
			}
			Audios[group][id] = tmpSource;
		}

		AudioTexts [0] = new Dictionary<int, string> ();
		AudioTexts [0] [0] = "噢...";
		AudioTexts [0] [1] = "啊...";
		AudioTexts [1] = new Dictionary<int, string> ();
		AudioTexts [1] [0] = "好痛...";
		AudioTexts [1] [1] = "额啊...";
		AudioTexts [2] = new Dictionary<int, string> ();
		AudioTexts [2] [0] = "放我出去！";
		AudioTexts [2] [1] = "放我出去！为什么要把我关在这里！放我出去！";
		AudioTexts [2] [2] = "啊啊啊啊啊啊啊啊!!";
		AudioTexts [2] [3] = "啊！！你这个畜生，难道一点同情心都没有吗！";
		AudioTexts [3] = new Dictionary<int, string> ();
		AudioTexts [3] [0] = "求求你...放我出去...救命...";
		AudioTexts [3] [1] = "求求你放过我...我的心脏要爆炸了...";
		AudioTexts [3] [2] = "我不行了...救救我...";
		AudioTexts [3] [3] = "救命...我快要死了...";
		AudioTexts [4] = new Dictionary<int, string> ();
		AudioTexts [4] [0] = "......";
		AudioTexts [5] = new Dictionary<int, string> ();
		AudioTexts [5] [0] = "啊啊啊啊啊啊啊啊啊啊啊！！！";
	}
	
	public string Play(int voltageLevel) {
		int range;
		int uid;
		int level;
		if (voltageLevel >= 2 && voltageLevel <= 3) {
			level = 2;
		} else if (voltageLevel == 4) {
			level = 3;
		} else if (voltageLevel > 4) {
			level = voltageLevel - 1;		
		} else {
			level = voltageLevel;		
		}

		if (AudioTexts.ContainsKey (level)) {
			range = AudioTexts [level].Count;
			if (range <= 0) {
					print ("AudioTexts" + level + " not found!");
					return "ERROR: 404 NOT FOUND";
			}

			System.Random ro = new System.Random (10); 
			long tick = DateTime.Now.Ticks; 
			System.Random ran = new System.Random ((int)(tick & 0xffffffffL) | (int)(tick >> 32)); 
			uid = ran.Next (0, range);
	} else {
			uid = 0;		
		}
		print (level+"_"+uid);
		Dictionary <int, string> dict = AudioTexts [level];
		if (!dict.ContainsKey(uid)) {
			print ("AudioTexts"+level.ToString()+"_"+uid.ToString()+" not found!");
			return "ERROR: 404 NOT FOUND";
		}

		if (Audios.ContainsKey(level) && Audios [level].ContainsKey(uid)) {
			Audios [level] [uid].Play ();
	} else {
			print ("Audio"+level+"_"+uid+" not found!");
		}

		return AudioTexts [level] [uid];
	}
}
