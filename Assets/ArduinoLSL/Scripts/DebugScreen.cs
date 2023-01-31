using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HCIUD.ArduinoLSL
{
    public class DebugScreen : MonoBehaviour
    {
        [SerializeField]
        private TMPro.TextMeshProUGUI logText;
        [SerializeField]
        private int limitedText = 100;

        void OnEnable()
        {
            Application.logMessageReceived += LogMessage;
        }

        void OnDisable()
        {
            Application.logMessageReceived -= LogMessage;
        }

        public void LogMessage(string message, string stackTrace, LogType type)
        {
            if(logText != null)
            {
                if (logText.text.Length > limitedText)
                {
                    logText.text = message + "\n";
                }
                else
                {
                    logText.text += message + "\n";
                }
            }            
        }
    }

}


