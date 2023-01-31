using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace HCIUD.ArduinoLSL
{
    public class UIManager : MonoBehaviour
    {
        [Header("UI Components")]
        [SerializeField]
        private TMP_InputField streamName;
        [SerializeField]
        private TMP_Dropdown deviceName;
        [SerializeField]
        private Button linkButton;
        [SerializeField]
        private TextMeshProUGUI linkButtonText;
        private bool isLinked = false;
        [SerializeField]
        private TMP_InputField timeOut;
        [SerializeField]
        private Toggle enableDebug;

        [Header("Arduino Connection")]
        [SerializeField]
        private ArduinoConnection arduinoConnection;

        [Header("LSL Connection")]
        [SerializeField]
        private LSLConnection lslConnection;

        // Start is called before the first frame update
        private void Start()
        {
            if(arduinoConnection == null)
                arduinoConnection = GetComponent<ArduinoConnection>();

            RefreshDevices();
        }

        public void RefreshDevices()
        {
            if (arduinoConnection.avaialablePorts.Length > 0)
            {
                Debug.Log("Refresh Devices");

                deviceName.ClearOptions();

                List<TMP_Dropdown.OptionData> _options = new List<TMP_Dropdown.OptionData>();
                TMP_Dropdown.OptionData option = new TMP_Dropdown.OptionData();
                option.text = "Select a device";
                _options.Add(option);

                foreach (var i in arduinoConnection.avaialablePorts)
                {
                    TMP_Dropdown.OptionData optionDevice = new TMP_Dropdown.OptionData();
                    optionDevice.text = i.ToString();
                    _options.Add(optionDevice);
                }

                deviceName.AddOptions(_options);

                arduinoConnection.CloseSerialPort();
            }
        }

        public void ChooseADevice()
        {
            int _id = deviceName.value;

            if(_id > 0)
            {
                _id = _id - 1; //Exclude the first option
                Debug.Log("Selected id: "+ _id);
                
                if (!string.IsNullOrEmpty(timeOut.text) && timeOut.text.Length > 0)
                {
                    int _timeOut = int.Parse(timeOut.text);
                    arduinoConnection.SetTimeOut(_timeOut);
                }

                arduinoConnection.SetShowDebug(enableDebug.isOn);
                arduinoConnection.SerialPortSelection(_id);
            }

        }

        public void LinkDeviceLSL()
        {
            if (!isLinked)
            {
                linkButtonText.text = "Unlink";
                isLinked = true;
            }
            else
            {
                linkButtonText.text = "Link";
                isLinked = false;
            }                

            if (lslConnection)
            {
                lslConnection.SetStreamName(streamName.text);
                lslConnection.StartLog = isLinked;
                lslConnection.enabled = isLinked;
                Debug.Log("Enable LSL Connection");
            }
        }

        public void EnableDebug()
        {
            arduinoConnection.SetShowDebug(enableDebug.isOn);
        }

        public void SetTimeOutInterval()
        {
            if (!string.IsNullOrEmpty(timeOut.text) && timeOut.text.Length > 0)
            {
                int _timeOut = int.Parse(timeOut.text);
                arduinoConnection.SetTimeOut(_timeOut);
            }
        }
    }
}
