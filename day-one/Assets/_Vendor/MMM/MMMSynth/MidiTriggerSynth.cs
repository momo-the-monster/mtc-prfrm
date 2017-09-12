﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MMM.Midi;

public class MidiTriggerSynth : MidiBehaviour {

    [Range(24, 48)]
    public int scale = 24;
    private SynthControl synth;
    private Dictionary<int, Lope> envelopeLookup;

    public void Start()
    {
        synth = GetComponent<SynthControl>();
        envelopeLookup = new Dictionary<int, Lope>();
    }

    public override void HandleNoteOn(int channel, int note, float velocity)
    {
        if (!envelopeLookup.ContainsKey(note))
        {
            timing.attack = (velocity < 0.5f) ? Mathf.Lerp(0.5f, 0f, velocity) : 0f;
            Lope envelope = synth.KeyOn(note, timing.attack);
            envelopeLookup.Add(note, envelope);
        }
    }

    public override void HandleNoteOff(int channel, int note)
    {
        Lope envelope;
        if (envelopeLookup.TryGetValue(note, out envelope))
        {
            envelope.KeyOff(timing.release);
            envelopeLookup.Remove(note);
        }
    }
}