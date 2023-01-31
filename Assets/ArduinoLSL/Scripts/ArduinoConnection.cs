using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;


namespace HCIUD.ArduinoLSL
{
    public class ArduinoConnection : MonoBehaviour
    {
        private SerialPort serialPort;

        public string[] avaialablePorts;
        private string portName;
        [SerializeField]
        private int baudRate = 9600;
        [SerializeField]
        private int readTimeOut = 150;
        private bool isPortOpened = false;
        private bool isRunning = false;
        [SerializeField]
        private bool showDebug = true;
        public int portId { get; private set; }

        public string dataValue { get; private set; }

        [Header("LSL Connection")]
        [SerializeField]
        private LSLConnection lslConnection;

        private void Awake()
        {
            // Get a list of serial port names.
            avaialablePorts = SerialPort.GetPortNames();

            // Display each port name to the console.
            if (avaialablePorts.Length > 0)
            {
                foreach (string port in avaialablePorts)
                {
                    Debug.Log("Port: "+ port);
                }
            }
            else
            {
                Debug.Log("No port connection available.");
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (isPortOpened && !isRunning)
                StartCoroutine(ReadDataAsyn());
        }


        public void SerialPortSelection(int id)
        {
            if(avaialablePorts.Length > 0)
            {
                portId = id;
                portName = avaialablePorts[id].ToString();
                serialPort = new SerialPort(portName, baudRate);

                try
                {
                    serialPort.Open();
                    serialPort.ReadTimeout = readTimeOut;
                    Debug.Log("Serial Port Open");
                    isPortOpened = true;
                }
                catch
                {
                    Debug.Log("Unable to open COM port.");
                }
            }
        }

        public void CloseSerialPort()
        {
            if (isPortOpened)
            {
                if (serialPort.IsOpen)
                {
                    isRunning = false;
                    serialPort.Close();
                }
            }
        }

        private IEnumerator ReadDataAsyn()
        {
            isRunning = true;
            float readTimeOutInSecond = readTimeOut / 1000;

            yield return new WaitForSeconds(readTimeOutInSecond);
            
            if (serialPort.IsOpen)
            {
                try
                {
                    dataValue = serialPort.ReadLine();

                    if (lslConnection)
                        lslConnection.ResistanceChanges = float.Parse(dataValue);

                    if(showDebug)
                        Debug.Log(dataValue);
                }
                catch
                {
                    Debug.Log("Time out exception.");
                }
            }

            isRunning = false;
        }

        public void SetTimeOut(int _time)
        {
            readTimeOut = _time;
        }

        public void SetShowDebug(bool _enable)
        {
            showDebug = _enable;
        }

        private void OnDestroy()
        {
            CloseSerialPort();
        }
    }
}
