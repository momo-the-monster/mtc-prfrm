using UnityEngine;
using MMM.RTMidi;
using DG.Tweening;

public class MidiMoveTween : MidiBehaviour
{
    public float amplitude = 1;
    public float duration = 0.25f;

    /// <summary>
    /// Set default activeEvents
    /// </summary>
    MidiMoveTween()
    {
        activeEvents = MidiEvents.NoteOn | MidiEvents.NoteOff;
    }

    // Move towards user when a midi note is pressed
    public override void HandleNoteOn(int channel, int note, float velocity)
    {
        Vector3 newPosition = transform.position + Vector3.back * velocity * amplitude;
        transform.DOMove(newPosition, duration);
    }

    // Move to world center when midi note is released
    public override void HandleNoteOff(int channel, int note)
    {
        transform.DOMove(Vector3.zero, duration);
    }

}