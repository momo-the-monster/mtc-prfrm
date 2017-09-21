using System;
using UnityEngine;
using MidiJack;

namespace MMM.Midi
{
    public class MidiBehaviour : MonoBehaviour
    {

        internal MidiJack.MidiDriver plugin;

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

        public virtual void HandleNoteOn(int channel, int note, float velocity)
        {
            Debug.LogFormat("{0} On {1}", note, velocity);
        }

        public virtual void HandleNoteOff(int channel, int note)
        {
            Debug.LogFormat("{0} Off", note);
        }

        public virtual void HandleControlChange(int channel, int controlNumber, float value)
        {
            Debug.LogFormat("Set control {0} to {1}", controlNumber, value);
        }

        private void NoteOn(MidiChannel channel, int note, float velocity)
        {
            HandleNoteOn((int)channel, note, velocity);
        }

        private void NoteOff(MidiChannel channel, int note)
        {
            HandleNoteOff((int)channel, note);
        }

        private void ControlChange(MidiChannel channel, int knobNumber, float knobValue)
        {
            HandleControlChange((int)channel, knobNumber, knobValue);
        }
        #endregion

        #region Animation
        // We hide these in the inspector because we draw them in a special way in MidiBehaviourEditor
        [HideInInspector]
        public float durationIn;
        [HideInInspector]
        public float durationOut;
        [HideInInspector]
        public float magnitude = 1;

        #endregion

        #region Enable / Disable
        public void OnEnable()
        {
            // Cache plugin instance
            plugin = MidiDriver.Instance;

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
                plugin.knobDelegate += ControlChange;
            }
        }

        private void OnDisable()
        {
            if ((activeEvents & MidiEvents.NoteOn) != 0)
            {
                plugin.noteOnDelegate -= NoteOn;
            }

            if ((activeEvents & MidiEvents.NoteOff) != 0)
            {
                plugin.noteOffDelegate -= NoteOff; ;
            }

            if ((activeEvents & MidiEvents.CC) != 0)
            {
                plugin.knobDelegate -= ControlChange;
            }
        }

        #endregion

        #region data handling
        private void OnValidate()
        {
            durationIn = Mathf.Max(durationIn, 0);
            durationOut = Mathf.Max(durationOut, 0);
        }
        #endregion

    }

}