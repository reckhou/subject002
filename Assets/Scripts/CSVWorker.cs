using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class CSVWorker
{

  public static void Format (string input, ref List<List<string>> outputList)
  {
    input = input.Replace ("\r", "");
    string[] sArray = input.Split ('\n');
    foreach (string str in sArray) {
      List<string> list = new List<string> ();
      string[] sElements = str.Split (',');
      for (int i = 0; i < sElements.Length; i++) {
        string result = sElements [i];
        if (result.StartsWith ("\"")) {
          result = result.TrimStart ('\"');
          if (!result.EndsWith ("\"")) {
            while (i < sElements.Length - 1) {
              i++;
                        
              if (!sElements [i].EndsWith ("\"")) {
                result += "," + sElements [i];
              } else {
                result += "," + sElements [i].TrimEnd ('\"');
                break;
              }
            }
          } else {
            result = result.TrimEnd ('\"');
          }
          result = result.Replace ("\"\"", "\""); 
        }

        list.Add (result);
      }
            
      outputList.Add (list);
    }
  }
}
