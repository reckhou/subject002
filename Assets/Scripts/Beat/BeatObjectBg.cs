using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BeatObjectBg : BeatObject {
	public List<Color> colors;

	public override void Beat() {
		base.Beat();
		Material[] curMaterials = this.GetComponent<Renderer>().materials;
		curMaterials[0].color = colors[curBeat % colors.Count];
		this.GetComponent<Renderer>().materials = curMaterials;
	}
}
