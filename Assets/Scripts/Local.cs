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
        TextAsset asset = Resources.Load("localization") as TextAsset;
        if (asset == null)
        {
            Debug.LogWarning("Load localization file failed!");
            return;
        }
        _LocalList = new List<List<string>>();
        CSVWorker.Format(asset.text, ref _LocalList);
    }

    // Use this for initialization
    void Start () {
       
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
	
	public string GetText(int ID)
    {
        int index = 0;
        if (_LangType == eLangType.Chinese)
        {
            index = 1;
        } else if (_LangType == eLangType.English)
        {
            index = 2;
        }

        return _LocalList[ID][index];
    }
}
