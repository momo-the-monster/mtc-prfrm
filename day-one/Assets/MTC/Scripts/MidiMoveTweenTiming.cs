using UnityEngine;
using MMM.Midi;
using DG.Tweening;

public class MidiMoveTweenTiming : MidiBehaviour
{
    public float amplitude = 1;
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
        Vector3 newPosition = ogPosition + (Vector3.back * velocity * amplitude);
        transform.DOKill();
        transform.DOMove(newPosition, timing.attack);
    }

    // Move to world center when midi note is released
    public override void HandleNoteOff(int channel, int note)
    {
        transform.DOKill();
        transform.DOMove(ogPosition, timing.release);
    }

}