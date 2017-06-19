using UnityEngine;
using MMM.RTMidi;
using DG.Tweening;

public class MidiMoveTweenTiming : MidiBehaviour
{
    public float amplitude = 1;

    /// <summary>
    /// Set default activeEvents
    /// </summary>
    MidiMoveTweenTiming()
    {
        activeEvents = MidiEvents.NoteOn | MidiEvents.NoteOff;
    }

    // Move towards user when a midi note is pressed
    public override void HandleNoteOn(int channel, int note, float velocity)
    {
        Vector3 newPosition = Vector3.back * velocity * amplitude;
        transform.DOKill();
        transform.DOMove(newPosition, timing.attack);
    }

    // Move to world center when midi note is released
    public override void HandleNoteOff(int channel, int note)
    {
        transform.DOKill();
        transform.DOMove(Vector3.zero, timing.release);
    }

}