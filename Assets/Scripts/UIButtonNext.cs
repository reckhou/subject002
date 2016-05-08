using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIButtonNext : MonoBehaviour
{
		
		void Update ()
		{
				Text text = transform.FindChild ("Text").GetComponent<Text> ();
				if (Local.Instance._LangType == Local.eLangType.Chinese) {
						text.text = "继 续";
				} else if (Local.Instance._LangType == Local.eLangType.English) {
						text.text = "Next";
				}

				Destroy (this);
		}
}
