using UnityEngine;
using System.Text;
using System.Collections;
using System.Collections.Generic;

public class Local : HLSingleton<Local> {
    List<List<string>> _LocalList;
    
    public enum eLangType
    {
        English,
        Chinese
    }

    public eLangType _LangType;

    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }

    // Use this for initialization
    void Start () {
        TextAsset asset = Resources.Load("localization") as TextAsset;
        if (asset == null)
        {
            Debug.LogWarning("Load localization file failed!");
            return;
        }
        _LocalList = new List<List<string>>();
        CSVWorker.Format(asset.text, ref _LocalList);
        DebugAsset();

    }

    void DebugAsset()
    {
        StringBuilder strBuilder = new StringBuilder();
        int curLine = 0;
        foreach (List<string> list in _LocalList)
        {
            curLine++;
            for(int i = 0; i < list.Count; i++)
            {
                string str = list[i];
                strBuilder.Append(str);
                if (str.Length < 1)
                {
                    Debug.LogWarning("Warning: Element in line " + curLine + " number " + (i+1) + " is empty, is that correct?");
                }
                if (i < list.Count - 1)
                {
                    strBuilder.Append(",");
                }
            }
            strBuilder.Append("\n");
        }

        Debug.Log(strBuilder.ToString());
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
