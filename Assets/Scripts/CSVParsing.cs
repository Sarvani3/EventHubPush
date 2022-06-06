using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;
using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine.Events;

public class CSVParsing : MonoBehaviour
{
    private string csvFile = "originaldata";
    private object[][] AllDataArray;
    [SerializeField] private EventHubUIScript EventHubUIScript;
    private void Awake()
    {
        ReadData();
        StartCoroutine(PostJSON());
    }

    // Read data from CSV file
    private void ReadData()
    {
        TextAsset fileName = Resources.Load<TextAsset>(csvFile);
        string[] lines = fileName.text.Split('\n');
        Debug.Log(lines.Length);
        AllDataArray = new object[lines.Length][];
        for (int i = 0; i < lines.Length; i++)
        {
            string[] strArray = lines[i].Split(';');
            AllDataArray[i] = strArray;
        }
    }
    public IEnumerator PostJSON()
    {
        Debug.Log(AllDataArray.GetLength(0) + "length of all data array");
        for (int i = GetLastPostedValue(); i < AllDataArray.GetLength(0) - 1; i++)
        {
            if (float.Parse(AllDataArray[i][2].ToString())!=0f)
            {
                RealTimeValue realTimeValue = new RealTimeValue();
                realTimeValue.rpm = float.Parse(AllDataArray[i][2].ToString());
                realTimeValue.temperature = float.Parse(AllDataArray[i][1].ToString());
                realTimeValue.bearings = float.Parse(AllDataArray[i][4].ToString());
                Debug.Log(realTimeValue.rpm);
                EventHubUIScript.PostData(realTimeValue);
                yield return new WaitForSeconds(5f);
            }
            if(i== AllDataArray.GetLength(0) - 2)
            {
                i = 2;
            }
            UpdatePostedValue(i);
        }
    }
    private void UpdatePostedValue(int index)
    {
        PlayerPrefs.SetInt("LastValue", index);
    }
    private int GetLastPostedValue()
    {
        return PlayerPrefs.GetInt("LastValue",2);
    }

}



public class RealTimeDataClass
{
    public List<float> rpm { get; set; }
    public List<float> temperature { get; set; }
    public List<float> bearings { get; set; }
}

[Serializable]
public class RealTimeValue
{
    public float rpm;
    public float temperature;
    public float bearings;
}