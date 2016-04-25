using UnityEngine;
using System.Collections;

public class InstanceFactory : MonoBehaviour {
    public GameObject _LocalPrefab;

	// Use this for initialization
	void Start () {
	    if (Local.Instance == null)
        {
            Instantiate(_LocalPrefab);
        }
	}
	
	// Update is called once per frame
	void Update () {
        Destroy(gameObject);
	}
}
