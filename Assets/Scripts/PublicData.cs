using UnityEngine;
using System.Collections;

public class PublicData : HLSingleton<PublicData>
{

  public int _Ending;

  void Awake ()
  {
    DontDestroyOnLoad (transform.gameObject);
  }
}
