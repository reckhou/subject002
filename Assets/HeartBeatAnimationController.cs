using UnityEngine;
using System.Collections;

public class HeartBeatAnimationController : MonoBehaviour {
	public MachineController machineController;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void PlayHeartBeatAudio() {
		machineController.PlayHeartBeatAudio ();
	}

	void PlayHeartStopAudio() {
		machineController.PlayHeartStopAudio ();
	}
}
