using System.Collections;
using System.Collections.Generic;
using System.Net;
using TMPro;
using UnityEditor;
using UnityEngine;

public class SpreadsheetLoader : MonoBehaviour
{
    private readonly string _spreadsheetUrl = 
    "https://docs.google.com/spreadsheets/d/e/2PACX-1vQnX18Jz7y4nqthJbxZWmXtETzF__SOSooLhVDNKXaAHZ8tfBo62zqzbauCLkGjgFkJiJVferC2u9WP/pub?output=csv";

    [SerializeField] private TMP_Text outputText;
    
    public void DownloadSpreadsheet()
    {
        WebClient client = new WebClient();
        string html = client.DownloadString(_spreadsheetUrl);
        outputText.text = html;
        //string[] entries = html.Split(',');
    }
}

[CustomEditor(typeof(SpreadsheetLoader))]
public class SpreadsheetLoaderEditor : Editor
{
    public override void OnInspectorGUI()
    {
        if (GUILayout.Button("Download"))
        {
            (target as SpreadsheetLoader).DownloadSpreadsheet();
        }
        
        base.OnInspectorGUI();
    }
}