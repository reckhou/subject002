using UnityEngine;
using System.Collections;

public class BeatObjectDefault : BeatObject {
	public float AnimationPerBar = 2;
	protected Animator anim;

	public override void Init(float barTime) {
		base.Init(barTime);
		anim = GetComponent<Animator>();
		if (anim == null) {
			Debug.Log("ERROR! animator not get");
		}
		updateSpeed();
  }

	public override void Beat() {
		base.Beat();
		if (curBeat == 0) {
			updateSpeed();
		}
  }

	public virtual void updateSpeed() {
		/* All animation is 1 sec length by default */
		float animationTime = BarTime / AnimationPerBar;
		if (animationTime == 0) {
			anim.speed = 0;
		} else {
		    anim.speed = 1.0f / animationTime;
		}
  }
}
