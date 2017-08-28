using UnityEngine;
using MMM.Midi;

public class MidiMove : MidiBehaviour
{
    public float amplitude = 1;

    /// <summary>
    /// Move towards user when a midi note is pressed
    /// </summary>
    /// <param name="channel">between 0 and 15</param>
    /// <param name="note">between 0 and 127</param>
    /// <param name="velocity"> normalized between 0 and 1</param>
    public override void HandleNoteOn(int channel, int note, float velocity)
    {
        Vector3 newPosition = transform.position + Vector3.back * velocity * amplitude;
        transform.position = newPosition;
    }

    /// <summary>
    /// Move to world center when midi note is released
    /// </summary>
    /// <param name="channel">between 0 and 15</param>
    /// <param name="note">between 0 and 127</param>
    public override void HandleNoteOff(int channel, int note)
    {
        transform.position = Vector3.zero;
    }

}