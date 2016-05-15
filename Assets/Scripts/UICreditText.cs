using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UICreditText : MonoBehaviour
{

  // Use this for initialization
  void Update ()
  {
    Text text = GetComponent<Text> ();
    string str = Local.Instance.GetText (49) + Local.Instance.GetText (47);
    str += " " + PublicData.Instance._Ending.ToString () + " / 4\n\n" + Local.Instance.GetText (48);
    text.text = str;

    Destroy (this);
  }
}
