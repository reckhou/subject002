using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class MachineController : MonoBehaviour
{
  private int Voltage;
  public int minVoltage = 60;
  public int maxVoltage = 420;
  public int minVoltageStep = 20;
  public int maxVoltageStep = 200;
  public int voltageLevelStep = 60;
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

  public AudioSource heartBeatAudio;
  public AudioSource heartStopAudio;
  private bool heartStopAudioPlayed;

  private bool shockCalled;
  public bool ShockSyncLock = false;

  // Use this for initialization
  void Start ()
  {
    voltageText = GameObject.Find ("Voltage").GetComponent<Text> ();
    voltageDescribe = GameObject.Find ("VoltageDescribe").GetComponent<Text> ();
    mainScreen = GameObject.Find ("MainScreen").GetComponent<Image> ();
    heartAnim = GameObject.Find ("HeartAnim").GetComponent<Animator> ();
    heartRate = GameObject.Find ("HeartRate").GetComponent<Text> ();
    SetVoltage (minVoltage);
    Disable ();
    lastShockVoltage = minVoltage;

    voltageDescribeArray = new string[7];
    if (Local.Instance._LangType == Local.eLangType.Chinese) {
      voltageDescribeArray [0] = "安全电击";
      voltageDescribeArray [1] = "温和电击";
      voltageDescribeArray [2] = "较强电击";
      voltageDescribeArray [3] = "中强电击";
      voltageDescribeArray [4] = "强烈电击";
      voltageDescribeArray [5] = "超强电击";
      voltageDescribeArray [6] = "致命电击";
    } else if (Local.Instance._LangType == Local.eLangType.English) {
      voltageDescribeArray [0] = "Safe";
      voltageDescribeArray [1] = "Mild";
      voltageDescribeArray [2] = "Weak";
      voltageDescribeArray [3] = "Strong";
      voltageDescribeArray [4] = "Severe";
      voltageDescribeArray [5] = "Extreme";
      voltageDescribeArray [6] = "Lethal";		
    }

    MaxVoltageLevel = voltageDescribeArray.Length - 1;
    heartAnim.speed = 1;
    UpdateHeartBeat ();
  }

  public void SetVoltage (int voltage)
  {
    if (voltage > maxVoltage) {
      voltage = maxVoltage;
    } else if (voltage < minVoltage) {
      voltage = minVoltage;
    }
      
    Voltage = voltage;

    if (Local.Instance._LangType == Local.eLangType.Chinese) {
      voltageText.text = "当前电压\n" + Voltage + "V";
    } else if (Local.Instance._LangType == Local.eLangType.English) {
      voltageText.text = "Voltage\n" + Voltage + "V";  
    }

  }

  public void AddVoltage ()
  {
    if (Voltage + 5 > lastShockVoltage + maxVoltageStep) {
      return;
    }
    Voltage += 5;
    SetVoltage (Voltage);
  }

  public void MinusVoltage ()
  {
    if (Voltage - 5 < lastShockVoltage + minVoltageStep) {
      return;
    }

    Voltage -= 5;
    SetVoltage (Voltage);
  }

  public void PlayButtonAudio ()
  {
    buttonAudio.Play ();
  }

  public void Shock ()
  {
    shockCalled = true;
  }

  public void PlayShock ()
  {
    shockAudio.PlayDelayed (1.0f);
    VoiceController.Instance.Play (GetLastShockVoltageLevel ());
  }

  public void ReadyShock ()
  {
    Enable ();
    if (firstAction) {
      firstAction = false;
      return;
    }
    SetVoltage (lastShockVoltage + defaultVoltageStep);
  }

  public void SwitchOFF ()
  {
    mainScreen.gameObject.SetActive (false);
    switchAudio.Play ();
    Disable ();
  }

  public void SwitchON ()
  {
    switchAudio.Play ();
    mainScreen.gameObject.SetActive (true);
  }

  public void Enable ()
  {
    PlusBtn.interactable = true;
    MinusBtn.interactable = true;
    PwrBtn.interactable = true;
    ExecuteBtn.interactable = true;
  }

  public void Disable ()
  {
    PlusBtn.interactable = false;
    MinusBtn.interactable = false;
    PwrBtn.interactable = false;
    ExecuteBtn.interactable = false;
  }

  public void UpdateHeartBeat ()
  {
    int voltageLevel = (lastShockVoltage - minVoltage) / voltageLevelStep;
    lastHeartBeatUpdate = Time.time;
    int baseHeartBeat = 80;
    if (voltageLevel >= 0 && voltageLevel <= 4) {
      baseHeartBeat += 30 * voltageLevel;
      heartAnim.speed = baseHeartBeat / 60.0f;
    } else if (voltageLevel == 5) {
      heartAnim.speed = 1;
      heartAnim.Play ("heartbeatSlow");
      baseHeartBeat = 30;
    } else if (voltageLevel > 5) {
      heartAnim.speed = 1;
      heartAnim.Play ("heartbeatStop");

      if (Local.Instance._LangType == Local.eLangType.Chinese) {
        heartRate.text = "心率\n0";
      } else if (Local.Instance._LangType == Local.eLangType.English) {
        heartRate.text = "Heart Rate\n0";  
      }
      return;
    }

    int iSeed = 10; 
    System.Random ro = new System.Random (10); 
    long tick = DateTime.Now.Ticks; 
    System.Random ran = new System.Random ((int)(tick & 0xffffffffL) | (int)(tick >> 32)); 
    int a = ran.Next (-5, 5);
    baseHeartBeat += a;

    if (Local.Instance._LangType == Local.eLangType.Chinese) {
      heartRate.text = "心率\n" + baseHeartBeat;
    } else if (Local.Instance._LangType == Local.eLangType.English) {
      heartRate.text = "Heart Rate\n" + baseHeartBeat; 
    }

  }

  // Update is called once per frame
  void Update ()
  {

  }

  public void CheckStatus ()
  {
    // check shock already started.
    if (shockCalled) {
      print ("Shock!!!");
      lastShockVoltage = Voltage;
      gameController.ShockStepCallback (Voltage);
      shockAudio.Play ();

      Disable ();
      shockCalled = false;
      ShockSyncLock = true;
    }

    // update display
    int voltageLevelNow = (Voltage - minVoltage) / voltageLevelStep;
    if (voltageLevelNow > voltageDescribeArray.Length - 1) {
      voltageLevelNow = voltageDescribeArray.Length - 1;
    }
    voltageDescribe.text = "\n" + voltageDescribeArray [voltageLevelNow];
    if (Time.time - lastHeartBeatUpdate > heartBeatUpdateTime) {
      UpdateHeartBeat ();
    }
  }

  public int GetLastShockVoltageLevel ()
  {
    int voltageLevel = (lastShockVoltage - minVoltage) / voltageLevelStep;
    return voltageLevel;
  }

  public void PlayHeartBeatAudio ()
  {
    heartBeatAudio.Play ();
  }

  public void PlayHeartStopAudio ()
  {
    if (!heartStopAudioPlayed) {
      heartStopAudio.Play ();
      heartStopAudioPlayed = true;
    }
  }

  public void StopHeartStopAudio ()
  {
    heartStopAudio.Stop ();
  }
}
