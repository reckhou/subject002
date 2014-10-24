using UnityEngine;
using System.Collections;

public class BeatObject : MonoBehaviour {
	protected int curBeat { get; set; }
	public int beatStatCnt { get; set; }
	public float BarTime { get; set; }
	public Vector3 originScale { get; set; }

	void Start() {
	}

	public virtual void Init(float barTime) {
		beatStatCnt = GlobalConfig.BeatStatCnt;
		originScale = transform.localScale;
		BarTime = barTime;
	}

	public virtual void Beat() {
		curBeat++;
		if (curBeat >= beatStatCnt) {
			curBeat = 0;
		}
	}
}
