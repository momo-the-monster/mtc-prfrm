using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MidiJack;
using DG.Tweening;
using System.Linq;

public class RoadBuilderMidi : MonoBehaviour {

    public RoadBuilderHelpful builder;
    internal Tween chordTween;
    Dictionary<int, List<NoteData>> notesInChord;
    Dictionary<int, Dictionary<int, Transform>> heldNoteTransforms;

    // Play Style Parameters
    public Vector2 velocityRange = new Vector2(0.1f, 2f);
    Dictionary<int, int> lastNote;
    public float noteDiffOffset = 1;
    public float noteIntervalMoveSpeed = 0.2f;
    public Ease noteIntervalEaseType = Ease.OutBack;
    public float growSpeed = 0.02f;

    public MidiChannel activeChannel = MidiChannel.Ch1;

    public struct NoteData
    {
       public int note;
       public float velocity;
       public int channel;

        public NoteData(int note, float velocity, int channel)
        {
            this.note = note;
            this.velocity = velocity;
            this.channel = channel;
        }
    }

    void OnEnable()
    {
        MidiMaster.noteOnDelegate += MidiNoteOn;
        MidiMaster.noteOffDelegate += MidiNoteOff;
    }

    void OnDisable()
    {
        MidiMaster.noteOnDelegate -= MidiNoteOn;
        MidiMaster.noteOffDelegate -= MidiNoteOff;
    }

    private void Start()
    {
        Reset();
    }

    void Update()
    {

        // cull dead transforms
        foreach (var lookup in heldNoteTransforms)
        {
            List<int> deadNotes = new List<int>();
            foreach (var item in lookup.Value)
            {

                if (item.Value != null)
                {
                    Vector3 position = item.Value.position;
                    Vector3 scale = item.Value.localScale;
                    scale.x += growSpeed;
                    //scale *= 1.02f;
                    position.x += (scale.x * 0.05f);
                    item.Value.position = position;
                    item.Value.localScale = scale;
                }
                else
                {
                    deadNotes.Add(item.Key);
                }

            }

            foreach (var note in deadNotes)
            {
                heldNoteTransforms[lookup.Key].Remove(note);
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        }

        if (Input.GetKeyDown(KeyCode.Alpha1)) activeChannel = MidiChannel.Ch1;
        if (Input.GetKeyDown(KeyCode.Alpha2)) activeChannel = MidiChannel.Ch2;
        if (Input.GetKeyDown(KeyCode.Alpha3)) activeChannel = MidiChannel.Ch3;
        if (Input.GetKeyDown(KeyCode.Alpha4)) activeChannel = MidiChannel.Ch4;
        if (Input.GetKeyDown(KeyCode.Alpha5)) activeChannel = MidiChannel.Ch5;
        if (Input.GetKeyDown(KeyCode.Alpha6)) activeChannel = MidiChannel.Ch6;
        if (Input.GetKeyDown(KeyCode.Alpha7)) activeChannel = MidiChannel.Ch7;
        if (Input.GetKeyDown(KeyCode.Alpha8)) activeChannel = MidiChannel.Ch8;
        if (Input.GetKeyDown(KeyCode.Alpha9)) activeChannel = MidiChannel.Ch9;
        if (Input.GetKeyDown(KeyCode.Alpha0)) activeChannel = MidiChannel.Ch10;
    }

    void MidiNoteOn(MidiChannel channel, int note, float velocity)
    {
        if (channel != activeChannel) return;

        if (notesInChord[(int)channel].Count > 0)
        {
            // Already holding for a chord
        }
        else
        {
            // Start new chord
            DOVirtual.DelayedCall(chordWaitTime, ()=>PlayChord((int)channel), false);
        }
        notesInChord[(int)channel].Add(new NoteData(note, velocity, (int)channel));
    }

    public float chordWaitTime = 0.25f;
    public float xOffsetAdd = 1;

    void PlayChord(int channel)
    {

        if (notesInChord[channel].Count == 1)
        {
            PlayNote(notesInChord[channel][0]);
        }
        else
        {
            // Special first note case
            List<NoteData> sortedChord = notesInChord[channel].OrderBy(o => o.note).ToList();
            Vector3 newPosition = PlayNote(sortedChord[0]);
            float scale = Mathf.Lerp(velocityRange.x, velocityRange.y, sortedChord[0].velocity);
            for (int i = 1; i < sortedChord.Count; i++)
            {
                // Draw the rest of the notes with xOffset
                builder.doRandomShape = false;
                newPosition.x -= (i * xOffsetAdd * scale);
                Transform t = builder.GrowOneAtWithScale(newPosition, new Vector3(scale, scale, scale));
                HoldNote(sortedChord[i].note, t, channel);
            }

        }

        notesInChord[channel].Clear();
    }

    Vector3 PlayNote( NoteData data)
    {
        float scale = Mathf.Lerp(velocityRange.x, velocityRange.y, data.velocity);
        int noteDiff = data.note - lastNote[data.channel];
        builder.doRandomShape = (noteDiff != 0);
        Transform t = builder.GrowOneWithScaleAndIndex(new Vector3(scale, scale, scale), data.note);
        Vector3 newPosition = t.position;
        if(lastNote[data.channel] > -1)
        {
            newPosition.y += (noteDiff * noteDiffOffset);
            t.DOMove(newPosition, noteIntervalMoveSpeed).SetEase(noteIntervalEaseType);
        }
        lastNote[data.channel] = data.note;
        HoldNote(data.note, t, data.channel);
        return newPosition;
    }

    void HoldNote(int note, Transform t, int channel)
    {
        if(!heldNoteTransforms[channel].ContainsKey(note))
            heldNoteTransforms[channel].Add(note, t);
    }

    void MidiNoteOff(MidiChannel channel, int note)
    {
    //    if (channel != activeChannel) return;

        heldNoteTransforms[(int)channel].Remove(note);
    }

    public void Reset()
    {
        if(heldNoteTransforms != null)
        {
            foreach (var lookup in heldNoteTransforms)
            {
                foreach (var item in lookup.Value)
                {
                    Destroy(item.Value.gameObject);
                }
            }
        }

        lastNote = new Dictionary<int, int>();
        notesInChord = new Dictionary<int, List<NoteData>>();
        heldNoteTransforms = new Dictionary<int, Dictionary<int, Transform>>();

        for (int i = 0; i < 16; i++)
        {
            lastNote.Add(i, -1);
            notesInChord.Add(i, new List<NoteData>());
            heldNoteTransforms.Add(i, new Dictionary<int, Transform>());
        }
    }
}
