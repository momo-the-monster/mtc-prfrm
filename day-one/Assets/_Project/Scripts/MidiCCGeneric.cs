using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using MMM.Midi;
using System;

public class MidiCCGeneric : MidiBehaviour {

    MidiCCGeneric()
    {
        activeEvents = MidiEvents.CC;
    }

    public MidiFloatEvent ccEvent;
    [Range(1,127)]
    public int ccNumber = 0;

    public override void HandleControlChange(int channel, int controlNumber, float value)
    {
        if(controlNumber == ccNumber)
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