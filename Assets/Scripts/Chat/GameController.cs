using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

public class GameController : MonoBehaviour {
	public ChatBox chatBox;
	public SelectBox selectBox;
	public enum LogicStep {
		Start,
		Experiment,
		Question,
		Answer,
		Judge,
		Warning,
		Shock,
		ShockEnd,
		SwitchStepEnd,
		End,
		Report,
		PostReport,
	};
	public LogicStep NextStep;

	public int WarningTimes = 4;
	public int SwitchTimes = 4;
	public int ShockCount = 0;
	public int MaxVoltageCount = 3;

	public MachineController machineController;

	private string[] questions;
	private int questionIndex;
	private int[] answers;
	private int curAnswer;
	private int correctAnswer;

	private LogicStep curStep;

	public int gameOverIndex;

	public Canvas UICanvas;
	public AudioSource shotAudio;
	public Canvas BlackCanvas;
	public Image ReportUI;

	public Text ReportText;
	public int reportStep;

	public ReportTextController reportTextController;
	public int ReportWait = 5;
	private bool reportWaiting;
	private bool reportWaitEnd;
  
	// Use this for initialization
	void Start () {
		questions = new string[4];
		answers = new int[4];
		chatBox.gameObject.SetActive(true);
		selectBox.gameObject.SetActive(false);
		LogicStart();
	}

	void LogicStart() {
		print("Logic start!");
		List<string> textList = new List<string>();
		textList.Add(Local.Instance.GetText(0));
		textList.Add("\r");
		textList.Add(Local.Instance.GetText(1));
		textList.Add("\r");
		textList.Add(Local.Instance.GetText(2));
		textList.Add("\r");
		textList.Add(Local.Instance.GetText(3));
		textList.Add("\r");
		textList.Add(Local.Instance.GetText(4));
		textList.Add("\r");
		textList.Add(Local.Instance.GetText(5));
		textList.Add("\r");
		chatBox.SetText(textList);
		NextStep = LogicStep.Experiment;
	}

	void LogicExperiment() {
		chatBox.SetText(null);
		chatBox.AddText(Local.Instance.GetText(6));
		chatBox.PlayLine();
		NextStep = LogicStep.Question;
	}

	void questionStep() {
		chatBox.SetAvatar(ChatBox.Character.Doctor);
		chatBox.gameObject.SetActive(false);
		selectBox.gameObject.SetActive(true);

		for (int i = 0; i < questions.Length; i++) {
			int iSeed=10; 
			System.Random ro = new System.Random(1000); 
			long tick = DateTime.Now.Ticks; 
			System.Random ran = new System.Random((int)(tick & 0xffffffffL) | (int) (tick >> 32)); 
			int a = ran.Next(1, 10);
			int b = ran.Next(1, 10);
			int method = ran.Next(0, 2);
			if (method == 0) {
				questions[i] = a + " + " + b;
				answers[i] = a + b;
			} else if (method == 1) {
				questions[i] = a + " - " + b;
				answers[i] = a - b;
			} /*else if (method == 2) {
				questions[i] = a + " * " + b + " = ?";
				answers[i] = a * b;
      		}*/
			System.Threading.Thread.Sleep(10);
    	}

		selectBox.AddSelection(Local.Instance.GetText(7), questions);
		NextStep = LogicStep.Answer;
	}

	void answerStep(int index) {
		questionIndex = index;
		print("answer step!");
		chatBox.SetAvatar(ChatBox.Character.Subject);
		chatBox.SetText(null);
		int shockLevel = machineController.GetLastShockVoltageLevel ();
		if (shockLevel == 0) {
			chatBox.AddText (Local.Instance.GetText(8));
		} else if (shockLevel == 1) {
			chatBox.AddText (Local.Instance.GetText(9));
		} else if (shockLevel == 2) {
			chatBox.AddText (Local.Instance.GetText(10));
		} else if (shockLevel == 3) {
			chatBox.AddText (Local.Instance.GetText(11));
		} else if (shockLevel == 4) {
			chatBox.AddText (Local.Instance.GetText(12));
		} else if (shockLevel == 5) {
			chatBox.AddText (Local.Instance.GetText(13));
		}
		int iSeed=10; 
		System.Random ro = new System.Random(10); 
		long tick = DateTime.Now.Ticks; 
		System.Random ran = new System.Random((int)(tick & 0xffffffffL) | (int) (tick >> 32)); 
		int a = ran.Next(1, 10);
		int b = ran.Next(1, 10);
		int isCorrect = ran.Next(0, 4);
		correctAnswer = answers[index];
		if (isCorrect == 0) {
			// correct
			curAnswer = answers[index];
		} else {
			// wrong
			int delta = ran.Next(0, 7);
			int method = ran.Next(0, 1);
			if (method == 0) {
				curAnswer = answers[index] + delta;
			} else {
				curAnswer = answers[index] - delta;
			}
		}
		string answerStr;
		if (shockLevel == 0) {
			answerStr = Local.Instance.GetText(14);
		} else if (shockLevel == 1) {
			answerStr = Local.Instance.GetText(15);
		} else if (shockLevel == 2) {
			answerStr = Local.Instance.GetText(16);
		} else if (shockLevel == 3) {
			answerStr = Local.Instance.GetText(17);
		} else if (shockLevel == 4) {
			answerStr = Local.Instance.GetText(18);
		} else if (shockLevel == 5) {
			answerStr = Local.Instance.GetText(19);
		} else {
			answerStr = curAnswer.ToString();
		}

        if (Local.Instance._LangType == Local.eLangType.Chinese)
        {
            answerStr += curAnswer + "吧？";
        } else
        {
            answerStr += curAnswer;
        }
		chatBox.AddText(answerStr);
		chatBox.PlayLine();
		NextStep = LogicStep.Judge;
	}

	void judgeStep() {
		chatBox.gameObject.SetActive(false);
		selectBox.gameObject.SetActive(true);
		string[] selections = new string[2];
		selections[0] = Local.Instance.GetText(19);
		selections[1] = Local.Instance.GetText(20);
		selectBox.AddSelection(Local.Instance.GetText(21) + questions[questionIndex] + " = " + curAnswer, selections);
		questionIndex = -1;
	}

	void judgeStepCallback(int index) {
		chatBox.gameObject.SetActive(true);
		selectBox.gameObject.SetActive(false);

		bool warnPlayer;
		if (curAnswer != correctAnswer && index == 0) {
			// if player choose to judge wrong answer to correct, warn player.
			warnPlayer = true;
		} else if (!(curAnswer == correctAnswer && index == 0))
        {
            WarningTimes = 4;
            warnPlayer = false;
        } else {
			warnPlayer = false;
        }

		if (!warnPlayer) {
			if (index == 0) {
				// right answer
				NextStep = LogicStep.Question;

				int iSeed=10; 
				System.Random ro = new System.Random(10); 
				long tick = DateTime.Now.Ticks; 
				System.Random ran = new System.Random((int)(tick & 0xffffffffL) | (int) (tick >> 32)); 
				int a = ran.Next(0, 3);
				if (a == 0) {
					chatBox.SetText(null);
					chatBox.SetAvatar(ChatBox.Character.Doctor);
					chatBox.AddText(Local.Instance.GetText(22));
					chatBox.PlayLine();
				} else if (a == 1) {
					chatBox.SetText(null);
					chatBox.SetAvatar(ChatBox.Character.Doctor);
					chatBox.AddText(Local.Instance.GetText(23));
					chatBox.PlayLine();
				} else {
					questionStep();
				}
			} else {
			    NextStep = LogicStep.Shock;
			    shockStep();
			}
		} else {
			NextStep = LogicStep.Warning;
			warningStep();
		}
	}

	void warningStep() {
		chatBox.SetText(null);
		chatBox.SetAvatar(ChatBox.Character.Doctor);
		if (WarningTimes == 4) {
			chatBox.AddText(Local.Instance.GetText(24));
		} else if (WarningTimes == 3) {
			chatBox.AddText(Local.Instance.GetText(25));
		} else if (WarningTimes == 2) {
			chatBox.AddText(Local.Instance.GetText(26));
		} else if (WarningTimes == 1) {
			chatBox.AddText(Local.Instance.GetText(27));
		} else if (WarningTimes <= 0) {
			chatBox.AddText(Local.Instance.GetText(28));
			chatBox.PlayLine();
			NextStep = LogicStep.End;
			return;
		}

		chatBox.PlayLine();
		WarningTimes--;
		NextStep = LogicStep.Question;
	}

	void shockStep() {
		chatBox.SetAvatar(ChatBox.Character.Doctor);
		ShockCount++;
		chatBox.SetText(null);
		chatBox.AddText(Local.Instance.GetText(29));
		chatBox.PlayLine();
		machineController.ReadyShock();
		NextStep = LogicStep.ShockEnd;
	}

	public void ShockStepCallback(int voltage) {
		print ("shock callback!");
		chatBox.SetAvatar(ChatBox.Character.Subject);

		print (machineController.GetLastShockVoltageLevel() + " " + machineController.MaxVoltageLevel);
		if (machineController.GetLastShockVoltageLevel() >= machineController.MaxVoltageLevel) {
			print ("ShockStepCallback:chatBox.gameObject.SetActive(true);");
			chatBox.gameObject.SetActive(true);
			chatBox.SetText(null);
			chatBox.AddText(Local.Instance.GetText(30));
			VoiceController.Instance.Play(machineController.GetLastShockVoltageLevel());
			gameOverIndex = 0;
			chatBox.PlayLine();
			NextStep = LogicStep.End;
		} else {
			print ("ShockStepCallback:chatBox.gameObject.SetActive(true);");
			chatBox.gameObject.SetActive(true);
			chatBox.SetText(null);
			string text = VoiceController.Instance.Play(machineController.GetLastShockVoltageLevel());
			chatBox.AddText(text);
			chatBox.playTypeAudio = false;
			chatBox.PlayLine();
			print ("NextStep = LogicStep.Question;");
			NextStep = LogicStep.Question;
		}
	}


	public void SwitchStep() { // device on/off
		print("switch step");
		chatBox.gameObject.SetActive(true);
		chatBox.SetText(null);

		SwitchTimes -= 1;

		if (SwitchTimes == 3) {
			chatBox.AddText(Local.Instance.GetText(31));
		} else if (SwitchTimes == 2) {
			chatBox.AddText(Local.Instance.GetText(32));
		} else if (SwitchTimes == 1) {
			chatBox.AddText(Local.Instance.GetText(33));
		} else if (SwitchTimes == 0) {
			chatBox.AddText(Local.Instance.GetText(34));
		} else if (SwitchTimes < 0) {
			chatBox.AddText(Local.Instance.GetText(28));
			chatBox.PlayLine();
			NextStep = LogicStep.End;
			return;
		}

		chatBox.PlayLine();
		NextStep = LogicStep.SwitchStepEnd;
	}

	public void SwitchStepCallback() {
		machineController.SwitchON();
		NextStep = LogicStep.Question;
		questionStep();
	}

	void LogicEnd() {
		chatBox.SetAvatar(ChatBox.Character.Doctor);
		chatBox.SetText(null);

		if (WarningTimes <= 0 || SwitchTimes < 0) {
			chatBox.AddText(Local.Instance.GetText(35));
			chatBox.AddText("\r");
			chatBox.AddText(Local.Instance.GetText(36));
			chatBox.AddText("\r");
			chatBox.AddText(Local.Instance.GetText(37));
			chatBox.AddText("\r");
			chatBox.PlayLine();
			gameOverIndex = 1;
			NextStep = LogicStep.Report;
			return;
		}

		if (gameOverIndex == 0) {
			chatBox.AddText(Local.Instance.GetText(38));
			chatBox.AddText("\r");
			chatBox.AddText(Local.Instance.GetText(39));
			chatBox.AddText("\r");
			chatBox.AddText(Local.Instance.GetText(40));
			chatBox.AddText("\r");
			chatBox.AddText(Local.Instance.GetText(41));
			chatBox.AddText("\r");
			chatBox.PlayLine();

			gameOverIndex = 0;
			NextStep = LogicStep.Report;
			return;
		}
	}

	void LogicReport() {
		if (singleMusicInstance.Instance != null) {
			singleMusicInstance.Instance.Stop ();
		}
		machineController.SwitchOFF();
		if (gameOverIndex == 0) {
		    machineController.PlayShock();
		} else if (gameOverIndex == 1) {
			// gunshot
			shotAudio.Play();
		}

		UICanvas.gameObject.SetActive(false);
		BlackCanvas.gameObject.SetActive(true);
		NextStep = LogicStep.PostReport;
	}

	void ReportNext() {
		if (reportStep == 0) {
			ReportUI.gameObject.SetActive(true);
			reportStep++;
			reportTextController.SetText(Local.Instance.GetText(42));
			reportTextController.Continue();
			return;
		}

		if (reportStep == 1) {
			reportTextController.ClearText();
			if (gameOverIndex == 0) {
				if (ShockCount <= 5 && WarningTimes == 4 && SwitchTimes == 4) {
					reportTextController.SetText(Local.Instance.GetText(43));
				} else {
					reportTextController.SetText(Local.Instance.GetText(44));
				}
			} else if (gameOverIndex == 1) {
                string txt = Local.Instance.GetText(45);
                if (SwitchTimes < 0)
                {
                    txt = Local.Instance.GetText(46);
                }
                reportTextController.SetText(txt);
			}
			reportTextController.Continue();
			reportStep++;
			return;
		}

		if (reportStep == 2) {
//			WaitForSeconds(5);
			Application.LoadLevel("endScene");

			reportStep++;
			return;
		}
	}

	public void TextEndCallback() {
		print("textEndCallback");
		print ("NextStep: "+NextStep);
		if (NextStep == LogicStep.Experiment) {
			LogicExperiment();
		} else if (NextStep == LogicStep.Question) {
			questionStep();
		} else if (NextStep == LogicStep.Judge) {
			judgeStep();
		} else if (NextStep == LogicStep.End) {
			LogicEnd();
		} else if (NextStep == LogicStep.ShockEnd) {
			chatBox.gameObject.SetActive(false);
		} else if (NextStep == LogicStep.SwitchStepEnd) {
			SwitchStepCallback();
		} else if (NextStep == LogicStep.Report) {
			LogicReport();
		}
	}

	public void SelectionCallback(int index) {
		print("Select: " + index);
		chatBox.gameObject.SetActive(true);
		selectBox.gameObject.SetActive(false);
		if (NextStep == LogicStep.Answer) {
			answerStep(index);
		} else if (NextStep == LogicStep.Judge) {
			judgeStepCallback(index);
		}
	}

	public void Update() {
		machineController.CheckStatus();
		chatBox.CheckStatus();

		if (NextStep == LogicStep.PostReport) {
			if (reportWaitEnd) {
				if (singleMusicInstance.Instance != null) {
					singleMusicInstance.Instance.Play (1);
				}
				ReportNext();
				reportWaitEnd = false;
				reportWaiting = false;
				return;
			}

			if (Input.anyKeyDown) {
				if (reportTextController.AllTextPlayed()) {
					if (!reportWaiting) {
					    StartCoroutine("ReportTimer");
					}
				} else if (!reportTextController.isPlaying) {
					reportTextController.Continue();
				}
			}
		}
	}

	void ReportWaitEnd() {
		reportWaitEnd = true;
	}

	IEnumerator ReportTimer() {
		reportWaiting = true;
		yield return new WaitForSeconds(5);
		ReportWaitEnd();
		yield return null;
	}
}
