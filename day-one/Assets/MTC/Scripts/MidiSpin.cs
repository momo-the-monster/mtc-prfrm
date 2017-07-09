using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MMM.RTMidi;
using DG.Tweening;

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

    public override void HandleControlChange(int channel, int controlNumber, float value)
    {
        int directionIndex = controlNumber % 4;
        Vector3 direction = Vector3.forward;
        var directions = new Vector3[]{ Vector3.forward, Vector3.up, Vector3.right, Vector3.back};
        Quaternion newRotation = Quaternion.AngleAxis((value - 0.5f) * 180 * amplitude, directions[directionIndex]);
        transform.DORotate(newRotation.eulerAngles, timing.release, RotateMode.Fast);
    }

}
