using UnityEngine;
using System.Collections;

public class InstanceFactory : MonoBehaviour
{
  public GameObject _LocalPrefab;
  public GameObject _PublicDataPrefab;
  // Use this for initialization
  void Start ()
  {
    if (Local.Instance == null) {
      Instantiate (_LocalPrefab);
    }

    if (PublicData.Instance == null) {
      Instantiate (_PublicDataPrefab);
    }
  }
	
  // Update is called once per frame
  void Update ()
  {
    Destroy (gameObject);
  }
}
