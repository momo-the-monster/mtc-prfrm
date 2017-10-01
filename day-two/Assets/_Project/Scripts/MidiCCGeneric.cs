using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using MMM.Midi;
using System;

public class MidiCCGeneric : MidiBehaviour {

    public bool learn = false;

    MidiCCGeneric()
    {
        activeEvents = MidiEvents.CC;
    }

    public MidiFloatEvent ccEvent;
    [Range(1,127)]
    public int ccNumber = 0;

    [ExecuteInEditMode]
    public override void HandleControlChange(int channel, int controlNumber, float value)
    {
        if (learn)
        {
            learn = false;
            ccNumber = controlNumber;
        }
        if(Application.isPlaying && controlNumber == ccNumber)
        {
            ccEvent.Invoke(value * magnitude);
        }
    }
}

[System.Serializable]
public class MidiFloatEvent : UnityEvent<float> { }

public interface ICCHandler
{
    void ApplyCC(float value);
}