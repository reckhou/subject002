using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class VoiceController : MonoBehaviour
{
  public Dictionary<int, AudioSource> Audios;
  public Dictionary<int, Dictionary<int, string>> AudioTexts;
  private static VoiceController instance;

  public static VoiceController Instance {
    get { return instance; }

  }

  void Awake ()
  {
    if (instance != null && instance != this) {
      Destroy (this.gameObject);
      return;
    } else {
      instance = this;		
    }
  }

  // Use this for initialization
  void Start ()
  {
    Audios = new Dictionary<int, AudioSource> ();
    AudioTexts = new Dictionary<int, Dictionary<int, string>> ();
    AudioSource[] sources = transform.GetComponents<AudioSource> ();
    foreach (AudioSource tmpSource in sources) {
      string name = tmpSource.clip.name;
      string[] parseArray = name.Split ('_');
      if (parseArray.Length < 2) {
        continue;
      }
      if (parseArray [0] != "shout") {
        continue;
      }
      int level = int.Parse (parseArray [1]);
      /*int group = int.Parse(parseArray[1]);
			int id = int.Parse(parseArray[2]);

			if (!Audios.ContainsKey(group)) {
				Audios[group] = new Dictionary<int, AudioSource>();
			}*/
      Audios [level] = tmpSource;
    }

    AudioTexts [0] = new Dictionary<int, string> ();
    AudioTexts [0] [0] = Local.Instance.GetText (50);
    AudioTexts [0] [1] = Local.Instance.GetText (51);
    AudioTexts [1] = new Dictionary<int, string> ();
    AudioTexts [1] [0] = Local.Instance.GetText (52);
    AudioTexts [1] [1] = Local.Instance.GetText (53);
    AudioTexts [2] = new Dictionary<int, string> ();
    AudioTexts [2] [0] = Local.Instance.GetText (54);
    AudioTexts [2] [1] = Local.Instance.GetText (55);
    AudioTexts [2] [2] = Local.Instance.GetText (56);
    AudioTexts [2] [3] = Local.Instance.GetText (57);
    AudioTexts [3] = new Dictionary<int, string> ();
    AudioTexts [3] [0] = Local.Instance.GetText (58);
    AudioTexts [3] [1] = Local.Instance.GetText (59);
    AudioTexts [3] [2] = Local.Instance.GetText (60);
    AudioTexts [3] [3] = Local.Instance.GetText (61);
    AudioTexts [4] = new Dictionary<int, string> ();
    AudioTexts [4] [0] = "......";
    AudioTexts [5] = new Dictionary<int, string> ();
    AudioTexts [5] [0] = Local.Instance.GetText (62);
  }

  public string Play (int voltageLevel)
  {
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

    print (level + "_" + uid);
    Dictionary <int, string> dict = AudioTexts [level];
    if (!dict.ContainsKey (uid)) {
      print ("AudioTexts " + level.ToString () + "_" + uid.ToString () + " not found!");
      return "ERROR: 404 NOT FOUND";
    }

    int shoutLevel = 0;
    if (level == 0) {
      shoutLevel = 0;
    } else if (level == 1 || level == 2) {
      shoutLevel = 1;
    } else if (level == 3) {
      shoutLevel = 2;
    } else if (level == 5) {
      shoutLevel = 3;
    } else {
      shoutLevel = -1;
    }

    if (Audios.ContainsKey (shoutLevel)) {
      Audios [shoutLevel].Play ();
    } else {
      print ("Audio" + level + "_" + uid + " not found!");
    }


    return AudioTexts [level] [uid];
  }
}
