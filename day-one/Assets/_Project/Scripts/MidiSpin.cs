using UnityEngine;
using MMM.Midi;
using DG.Tweening;

public class MidiSpin : MidiBehaviour {

    Rigidbody body;

    /// <summary>
    /// Set default activeEvents
    /// </summary>
    MidiSpin()
    {
        activeEvents = MidiEvents.NoteOn;
    }

    void Start()
    {
        body = GetComponent<Rigidbody>();
    }

    public override void HandleNoteOn(int channel, int note, float velocity)
    {
        // This approach spins the same for each key
        body.angularVelocity += Random.onUnitSphere * magnitude;
    }

    public override void HandleNoteOff(int channel, int note)
    {
        body.angularVelocity = Vector3.zero;
    }

    public override void HandleControlChange(int channel, int controlNumber, float value)
    {
        int directionIndex = controlNumber % 4;
        Vector3 direction = Vector3.forward;
        var directions = new Vector3[]{ Vector3.forward, Vector3.up, Vector3.right, Vector3.back};
        Quaternion newRotation = Quaternion.AngleAxis((value - 0.5f) * 180 * magnitude, directions[directionIndex]);
        transform.DORotate(newRotation.eulerAngles, envelope.release, RotateMode.Fast);
    }

}
