using UnityEngine;
using System.Collections;


public class singleMusicInstance : MonoBehaviour {
	private static singleMusicInstance instance = null;
	public AudioSource[] musicAudios = new AudioSource[2];

	public static singleMusicInstance Instance {
		get { return instance; }
	}

	void Awake() {
		if (instance != null && instance != this) {
			Destroy(this.gameObject);
			return;
		} else {
			instance = this;
		}
		DontDestroyOnLoad(this.gameObject);
	}

	public void Play(int index) {
		if (index > musicAudios.Length - 1 || index < 0) {
			return;
		}
		if (!musicAudios[index].isPlaying) {
			musicAudios[index].Play();
			for (int i = 0; i < musicAudios.Length; i++) {
				if (i == index) {
					continue;
				}
				musicAudios[i].Stop();
			}
		}
	}

	public void Stop() {
		for (int i = 0; i < musicAudios.Length; i++) {
			musicAudios[i].Stop();
		}
	}
}
