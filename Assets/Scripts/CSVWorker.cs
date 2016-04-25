using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class CSVWorker {

	public static void Format(string input, ref List<List<string>> outputList)
    {
        string[] sArray = input.Split('\n');
        foreach (string str in sArray)
        {
            List<string> list = new List<string>();
            string[] sElements = str.Split(',');
            foreach (string cur in sElements)
            {
                string result = cur;
                if (result.StartsWith("\""))
                {
                    result = result.TrimStart('\"');
                }

                if (result.EndsWith("\""))
                {
                    result = result.TrimEnd('\"');
                }

                result = result.Replace("\"\"", "\"");
                list.Add(result);
            }

            outputList.Add(list);
        }
    }
}
