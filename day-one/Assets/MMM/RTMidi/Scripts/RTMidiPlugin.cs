using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Runtime.InteropServices;

namespace MMM.RTMidi
{

    public enum MidiStatusCode
    {
        NoteOn = 9,
        NoteOff = 8,
        ControlChange = 11,
    }

    public class RTMidiPlugin : Tools.Singleton<RTMidiPlugin>
    {
        public const byte MD_CHANNELMASK = 0x0F;
        public const byte MD_STATUSMASK = 0xF0;

        public const string PluginName = "RTMidiUnity";
        public delegate void DataCallback(byte status, byte data1, byte data2);
        private DataCallback m_DataCallback;

        List<string> m_PortNames = new List<string>();

        #region Event Delegates

        public delegate void NoteOnDelegate(int channel, int note, float velocity);
        public delegate void NoteOffDelegate(int channel, int note);
        public delegate void ControlChangeDelegate(int channel, int knobNumber, float knobValue);

        public NoteOnDelegate noteOnDelegate { get; set; }
        public NoteOffDelegate noteOffDelegate { get; set; }
        public ControlChangeDelegate controlChangeDelegate { get; set; }

        #endregion

        #region Imported Functions

        [DllImport(PluginName)]
        private static extern bool Setup();

        [DllImport(PluginName, EntryPoint = "Teardown")]
        private static extern bool RTTeardown();

        [DllImport(PluginName, EntryPoint = "GetPortCount")]
        private static extern uint RTGetPortCount();

        [DllImport(PluginName, EntryPoint = "GetPortName")]
        private static extern IntPtr RTGetPortName(uint index);

        [DllImport(PluginName, EntryPoint = "OpenPort")]
        private static extern IntPtr RTOpenPort(uint portNumber);

        [DllImport(PluginName, EntryPoint = "ClosePort")]
        private static extern IntPtr RTClosePort();

        [DllImport(PluginName)]
        private static extern void SetCallback(DataCallback cb);
        #endregion

        void Start()
        {
            m_DataCallback = new DataCallback(OnDataCallback);
            SetCallback(m_DataCallback);
            Debug.LogFormat("Setup: {0}", Setup());
            OpenPort(0);
        }

        void OnDataCallback(byte status, byte data1, byte data2)
        {
            var rawStatusCode = status >> 4;
            var channelNumber = status & MD_CHANNELMASK;

            var velocity = 1.0f / 127 * data2;

            if (Enum.IsDefined(typeof(MidiStatusCode), rawStatusCode))
            {
                MidiStatusCode statusCode = (MidiStatusCode)rawStatusCode;

                switch (statusCode)
                {
                    case MidiStatusCode.NoteOn:
                        if(noteOnDelegate != null)
                        {
                            noteOnDelegate(channelNumber, data1, velocity);
                        }
                        break;
                    case MidiStatusCode.NoteOff:
                        if(noteOffDelegate != null)
                        {
                            noteOffDelegate(channelNumber, data1);
                        }
                        break;
                    case MidiStatusCode.ControlChange:
                        if(controlChangeDelegate != null)
                        {
                            controlChangeDelegate(channelNumber, data1, velocity);
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        string GetPortNameString(uint index)
        {
            return Marshal.PtrToStringAnsi(RTGetPortName(index));
        }

        public int GetPortCount()
        {
            return (int)RTGetPortCount();
        }

        public List<string> GetPortNames()
        {
            m_PortNames.Clear();
            uint portCount = RTGetPortCount();
            for (uint i = 0; i < portCount; i++)
            {
                m_PortNames.Add(GetPortNameString(i));
            }
            return m_PortNames;
        }

        //TODO: reject if it's not a valid number (or check inside plugin?)
        public void OpenPort(int portNumber)
        {
            RTOpenPort((uint)portNumber);
        }

        public void ClosePort()
        {
            RTClosePort();
        }

        void Teardown()
        {
            SetCallback(null);
            m_DataCallback = null;
            ClosePort();
            RTTeardown();
        }

        void OnDestroy()
        {
            Teardown();
        }
    }

}