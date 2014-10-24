using UnityEngine;
using System.Collections;

public class BeatObjectScale : BeatObject {
	public override void Beat() {
		base.Beat();
		float scale = originScale.x + curBeat * 0.05f;
		Vector3 newScale = transform.localScale;
		newScale.x = scale;
		newScale.y = scale;
		transform.localScale = newScale;
	}
}
