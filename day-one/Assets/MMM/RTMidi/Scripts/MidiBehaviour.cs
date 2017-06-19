using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MMM.RTMidi
{
    public class MidiBehaviour : MonoBehaviour
    {

        internal RTMidiPlugin plugin;

        #region Convenience Methods
        public static float GetNormalizedNote(int note)
        {
            return Mathf.InverseLerp(0, 11, note % 12);
        }
        #endregion

        #region Midi Events
        public enum MidiEvents
        {
            NoteOn = 1,
            NoteOff = 2,
            CC = 4
        }

        [EnumFlag]
        public MidiEvents activeEvents;

        public virtual void HandleNoteOff(int channel, int note)
        {
            Debug.LogFormat("{0} Off", note);
        }

        public virtual void HandleControlChange(int channel, int controlNumber, float value)
        {
            Debug.LogFormat("Set control {0} to {1}", controlNumber, value);
        }

        public virtual void HandleNoteOn(int channel, int note, float velocity)
        {
            Debug.LogFormat("{0} On {1}", note, velocity);
        }

        private void NoteOff(int channel, int note)
        {
            UnityMainThreadDispatcher.Enqueue(() => HandleNoteOff(channel, note));
        }

        private void ControlChange(int channel, int controlNumber, float value)
        {
            UnityMainThreadDispatcher.Enqueue(() => HandleControlChange(channel, controlNumber, value));
        }

       private void NoteOn(int channel, int note, float velocity)
        {
            UnityMainThreadDispatcher.Enqueue(() => HandleNoteOn(channel, note, velocity));
        }

        #endregion

        #region Animation

        [Serializable]
        public struct ADSR
        {
            public float attack;
            public float decay;
            public float sustain;
            public float release;

            float a { get { return attack; } }
            float d { get { return decay; } }
            float s { get { return sustain; } }
            float r { get { return release; } }
        }

        [SerializeField]
        public ADSR timing;

        #endregion

        #region Enable / Disable

        public void OnEnable()
        {
            // Cache plugin instance
            plugin = RTMidiPlugin.Instance;

            if ((activeEvents & MidiEvents.NoteOn) != 0)
            {
                plugin.noteOnDelegate += NoteOn;
            }

            if ((activeEvents & MidiEvents.NoteOff) != 0)
            {
                plugin.noteOffDelegate += NoteOff;
            }

            if ((activeEvents & MidiEvents.CC) != 0)
            {
                plugin.controlChangeDelegate += ControlChange;
            }
        }

        private void OnDisable()
        {
            if ((activeEvents & MidiEvents.NoteOn) != 0)
            {
                plugin.noteOnDelegate -= HandleNoteOn;
            }

            if ((activeEvents & MidiEvents.NoteOff) != 0)
            {
                plugin.noteOffDelegate -= HandleNoteOff; ;
            }

            if ((activeEvents & MidiEvents.CC) != 0)
            {
                plugin.controlChangeDelegate -= HandleControlChange;
            }
        }

        #endregion

    }

}