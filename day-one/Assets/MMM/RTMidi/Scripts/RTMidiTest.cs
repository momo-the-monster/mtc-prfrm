using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CatchCo;
using MMM.RTMidi;
using System;

namespace MMM.RTMidi.Demo
{
    public class RTMidiTest : MidiBehaviour
    {

        private void Start()
        {
            // Create a port to use
            OpenPort();
        }

        public override void HandleNoteOn(int channel, int note, float velocity)
        {
            Debug.LogFormat("Got note on as {0} {1} {2}", channel.ToString(), note, velocity);
        }

        public override void HandleNoteOff(int channel, int note)
        {
            Debug.LogFormat("Got note off as {0} {1}", channel.ToString(), note);
        }

        public override void HandleControlChange(int channel, int controlNumber, float value)
        {
            Debug.LogFormat("Set control {0} to {1}", controlNumber, value);
        }

        [ExposeMethodInEditor]
        void PrintPortNames()
        {
            var names = plugin.GetPortNames();
            for (int i = 0; i < names.Count; i++)
            {
                Debug.LogFormat(names[i]);
            }
        }

        [ExposeMethodInEditor]
        void OpenPort()
        {
            plugin.OpenPort(0);
        }

        [ExposeMethodInEditor]
        void ClosePort()
        {
            plugin.ClosePort();
        }

    }
}