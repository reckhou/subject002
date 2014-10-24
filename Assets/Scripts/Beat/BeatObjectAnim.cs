using UnityEngine;
using System.Collections;

public class BeatObjectAnim : BeatObjectDefault {
	protected int actionCnt = 2;
	protected int curAction = 0;
	public override void Init(float barTime) {
		AnimationPerBar = 4;
		base.Init(barTime);
	}

	public override void Beat() {
		base.Beat();
		if (curBeat % (4 / AnimationPerBar) == 0) {
			curAction++;
			if (curAction >= actionCnt) {
				curAction = 0;
			}
		}
		anim.SetInteger("actionStat", curAction);
	}
}
