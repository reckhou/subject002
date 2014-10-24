using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class MachineController : MonoBehaviour {
	private int Voltage;
	public int minVoltage = 60;
	public int maxVoltage = 300;
	public int minVoltageStep = 20;
	public int maxVoltageStep = 60;
	public int defaultVoltageStep = 40;
	public int lastShockVoltage;

	public Button PlusBtn;
	public Button MinusBtn;
	public Button PwrBtn;
	public Button ExecuteBtn;

	public GameController gameController;
	public Image mainScreen;
	
	private Text voltageText;
	private Text voltageDescribe;
	private Animator heartAnim;
	private Text heartRate;
	private string[] voltageDescribeArray;
	private bool firstAction = true;
	public int MaxVoltageLevel;

	public float heartBeatUpdateTime = 3.0f;
	private float lastHeartBeatUpdate;

	public AudioSource switchAudio;
	public AudioSource buttonAudio;
	public AudioSource shockAudio;
	
	public AudioSource shoutLowAudio;
	public AudioSource shoutMidAudio;
	public AudioSource shoutHighAudio;
	public AudioSource shoutDeathAudio;

	public AudioSource heartBeatAudio;
	public AudioSource heartStopAudio;
	private bool heartStopAudioPlayed;

	private bool shockCalled;
	public bool ShockSyncLock = false;

	// Use this for initialization
	void Start () {
		voltageText = GameObject.Find("Voltage").GetComponent<Text>();
		voltageDescribe = GameObject.Find("VoltageDescribe").GetComponent<Text>();
		mainScreen = GameObject.Find("MainScreen").GetComponent<Image>();
		heartAnim = GameObject.Find("HeartAnim").GetComponent<Animator>();
		heartRate = GameObject.Find("HeartRate").GetComponent<Text>();
		SetVoltage(minVoltage);
		Disable();
		lastShockVoltage = minVoltage;

		voltageDescribeArray = new string[7];
		voltageDescribeArray[0] = "安全电击";
		voltageDescribeArray[1] = "温和电击";
		voltageDescribeArray[2] = "较强电击";
		voltageDescribeArray[3] = "中强电击";
		voltageDescribeArray[4] = "强烈电击";
		voltageDescribeArray[5] = "超强电击";
		voltageDescribeArray[6] = "致命电击";
		MaxVoltageLevel = voltageDescribeArray.Length -1;
		heartAnim.speed = 1;
	}

	public void SetVoltage(int voltage) {
		if (voltage > maxVoltage) {
			voltage = maxVoltage;
		} else if (voltage < minVoltage) {
			voltage = minVoltage;
		}

		Voltage  = voltage;
		voltageText.text = "当前电压\n" + Voltage + "V";
	}

	public void AddVoltage() {
		if (Voltage + 5 > lastShockVoltage + maxVoltageStep) {
			return;
		}
		Voltage += 5;
		SetVoltage(Voltage);
	}

	public void MinusVoltage() {
		if (Voltage - 5 < lastShockVoltage + minVoltageStep) {
			return;
		}

		Voltage -= 5;
		SetVoltage(Voltage);
	}

	public void PlayButtonAudio() {
		buttonAudio.Play();
	}

	public void Shock() {
		shockCalled = true;
	}

	public void PlayShock() {
		shockAudio.PlayDelayed(1.0f);
		shoutDeathAudio.PlayDelayed(1.0f);
	}

	public void ReadyShock() {
		Enable();
		if (firstAction) {
			firstAction = false;
			return;
		}
		SetVoltage(lastShockVoltage + defaultVoltageStep);
	}

	public void SwitchOFF() {
		mainScreen.gameObject.SetActive(false);
		switchAudio.Play();
		Disable();
	}

	public void SwitchON() {
		switchAudio.Play();
		mainScreen.gameObject.SetActive(true);
	}

	public void Enable() {
		PlusBtn.interactable = true;
		MinusBtn.interactable = true;
		PwrBtn.interactable = true;
		ExecuteBtn.interactable = true;
	}

	public void Disable() {
		PlusBtn.interactable = false;
		MinusBtn.interactable = false;
		PwrBtn.interactable = false;
		ExecuteBtn.interactable = false;
	}

	public void UpdateHeartBeat() {
		int voltageLevel = (lastShockVoltage - minVoltage) / defaultVoltageStep;
		lastHeartBeatUpdate = Time.time;
		int baseHeartBeat = 80;
		if (voltageLevel >= 0 && voltageLevel <= 4) {
			baseHeartBeat += 30 * voltageLevel;
			heartAnim.speed = baseHeartBeat / 60.0f;
		} else if (voltageLevel == 5) {
			heartAnim.speed = 1;
			heartAnim.Play("heartbeatSlow");
			baseHeartBeat = 30;
		} else if (voltageLevel > 5) {
			heartAnim.speed = 1;
			heartAnim.Play("heartbeatStop");
			heartRate.text = "心率\n0";
			return;
		}

		int iSeed=10; 
		System.Random ro = new System.Random(10); 
		long tick = DateTime.Now.Ticks; 
		System.Random ran = new System.Random((int)(tick & 0xffffffffL) | (int) (tick >> 32)); 
		int a = ran.Next(-5, 5);
		baseHeartBeat += a;
		heartRate.text = "心率\n" + baseHeartBeat;

	}

	// Update is called once per frame
	void Update () {

	}

	public void CheckStatus() {
		// check shock already started.
		if (shockCalled) {
			print ("Shock!!!");
			lastShockVoltage = Voltage;
			gameController.ShockStepCallback(Voltage);
			shockAudio.Play();
			int voltageLevel = (lastShockVoltage - minVoltage) / defaultVoltageStep;
			if (voltageLevel < 2) {
				shoutLowAudio.Play();
			} else if (voltageLevel >= 2 && voltageLevel <= 3) {
				shoutMidAudio.Play();
			} else if (voltageLevel > 3 && voltageLevel <= 4) {
				shoutHighAudio.Play();
			} else if (voltageLevel > 4) {
        		shoutDeathAudio.Play();
      		}	
      		Disable();
			shockCalled = false;
			ShockSyncLock = true;
		}

		// update display
		int voltageLevelNow = (Voltage - minVoltage) / defaultVoltageStep;
		if (voltageLevelNow > voltageDescribeArray.Length -1) {
			voltageLevelNow = voltageDescribeArray.Length -1;
		}
		voltageDescribe.text = "\n" + voltageDescribeArray[voltageLevelNow];
		if (Time.time - lastHeartBeatUpdate > heartBeatUpdateTime) {
			UpdateHeartBeat();
    	}
	}

	public int GetLastShockVoltageLevel() {
		int voltageLevel = (lastShockVoltage - minVoltage) / defaultVoltageStep;
		return voltageLevel;
	}

	public void PlayHeartBeatAudio() {
		heartBeatAudio.Play ();
	}

	public void PlayHeartStopAudio() {
		if (!heartStopAudioPlayed) {
			heartStopAudio.Play ();
			heartStopAudioPlayed = true;
		}
	}

	public void StopHeartStopAudio() {
		heartStopAudio.Stop ();
	}
}
