using UnityEngine;
using MMM.Midi;
using DG.Tweening;

public class MidiMoveTweenTiming : MidiBehaviour
{
    Vector3 ogPosition;

    /// <summary>
    /// Set default activeEvents
    /// </summary>
    MidiMoveTweenTiming()
    {
        activeEvents = MidiEvents.NoteOn | MidiEvents.NoteOff;
    }

    private void Start()
    {
        ogPosition = transform.position;
    }

    // Move towards user when a midi note is pressed
    public override void HandleNoteOn(int channel, int note, float velocity)
    {
        Vector3 newPosition = ogPosition + (Vector3.back * velocity * magnitude);
        transform.DOKill();
        transform.DOMove(newPosition, durationIn);
    }

    // Move to world center when midi note is released
    public override void HandleNoteOff(int channel, int note)
    {
        transform.DOKill();
        transform.DOMove(ogPosition, durationOut);
    }

}