using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MMM.RTMidi;

public class MidiSpin : MidiBehaviour {

    Rigidbody body;
    public float amplitude = 1;

    void Start()
    {
        body = GetComponent<Rigidbody>();
    }

    public override void HandleNoteOn(int channel, int note, float velocity)
    {
        // This approach spins the same for each key
        body.angularVelocity += Random.onUnitSphere * amplitude;
    }

}
