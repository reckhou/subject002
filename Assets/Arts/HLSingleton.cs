using UnityEngine;
using System.Collections;

public class HLSingleton<T> : MonoBehaviour
	where T : MonoBehaviour
{
	private static T _instance;
	public static T Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = (T)FindObjectOfType(typeof(T));
				
				if (_instance == null)
				{
					Debug.LogWarning(typeof(T).Name + " not in scene!");
				}
			}
			
			return _instance;
		}
	}
	
	protected void OnDestroy()
	{
		if (_instance == this)
		{
			_instance = default(T);
		}
	}
	
	public static bool IsValid()
	{
		return (_instance != null);
	}
}
