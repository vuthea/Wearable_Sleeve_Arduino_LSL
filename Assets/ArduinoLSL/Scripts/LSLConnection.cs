using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LSL;
using LSL4Unity.Utils;
using System;

namespace HCIUD.ArduinoLSL
{
    public class LSLConnection : AFloatOutlet
    {
        public float ResistanceChanges { get; set; }
        public bool StartLog { get; set; } = false;

        public override List<string> ChannelNames
        {
            get { return new List<string>(new string[] { "CH1"}); } 
        }


        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();
            Debug.Log("Try to connect LSL: " + LSL.LSL.library_version());
            Debug.Log(outlet.ToString());
            //Reset();
        }


        protected override bool BuildSample()
        {
            if(StartLog)
            {
                //Debug.Log(outlet.ToString());

                sample[0] = ResistanceChanges;

                return true;
            }else { return false; }
            
        }

        public void SetStreamName(string _name)
        {
            StreamName = _name;
        }
 
    }
}
