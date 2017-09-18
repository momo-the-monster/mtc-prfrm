using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MMM.Midi;
using DG.Tweening;

public class MidiTriggerSynth : MidiBehaviour {

    private SynthControl synth;
    private Dictionary<int, Lope> envelopeLookup;
    private float ogMult;

    public void Start()
    {
        synth = GetComponent<SynthControl>();
        envelopeLookup = new Dictionary<int, Lope>();
        ogMult = synth.fm_mul;
    }

    public override void HandleNoteOn(int channel, int note, float velocity)
    {
        if (!envelopeLookup.ContainsKey(note))
        {
            var synthMono = synth.KeyOn(note, durationIn);
            envelopeLookup.Add(note, synthMono.module.env);
        }
    }

    public override void HandleNoteOff(int channel, int note)
    {
        Lope envelope;
        if (envelopeLookup.TryGetValue(note, out envelope))
        {
            envelope.KeyOff(durationOut);
            envelopeLookup.Remove(note);
        }
    }
}
