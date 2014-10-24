using UnityEngine;
using System.Collections;

public class moonMover : MonoBehaviour {
	public float endPosY;
	public float speed, lightSmooth;
	public float startPosY;
	public float moonLight, moonLightBack, envLight, lampLight;
	public float startWait;
	public GameObject sceneFar;
	public GameObject char_0;

	private bool startSpawn = false;
	private float spawnTime;

	private Vector3 endPos;

	// Use this for initialization
	void Start () {
		Vector3 pos = transform.localPosition;
		pos.y = startPosY;
		transform.localPosition = pos;
		spawnTime = (endPosY - startPosY) / speed;
		endPos = pos;
		endPos.y = endPosY;
		StartCoroutine (SpawnMoon());
	}

	IEnumerator SpawnMoon ()
	{
		yield return new WaitForSeconds (startWait);
		 
		startSpawn = true;

		yield return null;
	}
	
	// Update is called once per frame
	void Update () {
		if (startSpawn)
		{
			if (transform.localPosition.y >= endPosY) {
				startSpawn = false;
			} else {
				transform.localPosition = Vector3.Lerp(transform.localPosition, endPos, speed * Time.deltaTime);
				GameObject ml = GameObject.FindGameObjectWithTag("moonlight");
				GameObject mlb = GameObject.FindGameObjectWithTag("moonlightback");
				GameObject envl = GameObject.FindGameObjectWithTag("envLight");

				if (ml != null) {
					ml.light.intensity = Mathf.Lerp(ml.light.intensity, moonLight, lightSmooth * Time.deltaTime);
				}

				if (mlb != null) {
					mlb.light.intensity = Mathf.Lerp(mlb.light.intensity, moonLightBack, lightSmooth * Time.deltaTime);
				}

				if (envl != null) {
					envl.light.intensity = Mathf.Lerp(envl.light.intensity, envLight, lightSmooth * Time.deltaTime);
				}

//				float intensityPerStep = moonLightBack / spawnTime;
				float grayScalePerStep = 140.0f / spawnTime;
//				float changeRate = moonLightBack / moonLight;
//				float changeRate2 = moonLightBack / envLight;
//				float changeRate3 = moonLightBack / lampLight;

//				GameObject lmpl = GameObject.FindGameObjectWithTag("lampLight");
//				if (ml != null && ml.light.intensity < moonLight) {
//					ml.light.intensity += intensityPerStep * Time.deltaTime / changeRate;
//				}
//				if (mlb != null && mlb.light.intensity < moonLightBack) {
//					mlb.light.intensity += intensityPerStep * Time.deltaTime;
//				}
//				if (envl != null && envl.light.intensity < envLight) {
//					envl.light.intensity += intensityPerStep * Time.deltaTime / changeRate2 ;
//				}
//				if (lmpl != null && lmpl.light.intensity < lampLight) {
//					lmpl.light.intensity += intensityPerStep * Time.deltaTime / changeRate3 ;
//				}
				SpriteRenderer sp = char_0.GetComponent<SpriteRenderer>();
				Color newColor =  sp.color;
				newColor.r += (1.0f/255.0f)*grayScalePerStep * Time.deltaTime;
				newColor.g += (1.0f/255.0f)*grayScalePerStep * Time.deltaTime;
				newColor.b += (1.0f/255.0f)*grayScalePerStep * Time.deltaTime;
				sp.color = newColor;

			}
		}
	}
}
