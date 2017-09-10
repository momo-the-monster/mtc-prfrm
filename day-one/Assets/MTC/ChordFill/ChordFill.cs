using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using MMM.Midi;

public class ChordFill : MidiBehaviour {

    public GameObject prefab;
    public Transform container;

    Dictionary<int, GameObject> activeNotes = new Dictionary<int, GameObject>();

    ChordFill()
    {
        activeEvents = MidiEvents.NoteOn | MidiEvents.NoteOff;
    }

    public override void HandleNoteOn(int channel, int note, float velocity)
    {
        if (!activeNotes.ContainsKey(note))
        {

            timing.attack = 1 - velocity;
            // Spawn object
            GameObject g = Instantiate(prefab, Vector3.zero, Quaternion.identity);
            g.transform.SetParent(container);

            float normalizedNote = GetNormalizedNote(note);

            Image image = g.GetComponent<Image>();
            if (image != null)
            {
                image.color = UnityEngine.Random.ColorHSV(normalizedNote- 0.02f, normalizedNote, 0.9f, 0.9f, velocity, velocity + 1);
            }
            LayoutElement layout = g.GetComponent<LayoutElement>();
            if (layout != null)
            {
                layout.preferredWidth = 0;
                layout.DOPreferredSize(new Vector2(Screen.width, Screen.height), timing.attack);
            }

            // Add to lookup
            activeNotes.Add(note, g);
        }
    }

    public override void HandleNoteOff(int channel, int note)
    {
        // Try to get gameobject
        GameObject g;
        if (activeNotes.TryGetValue(note, out g))
        {

            // If it's there, remove it from the lookup and Destroy it
            activeNotes.Remove(note);
            Image image = g.GetComponent<Image>();
            LayoutElement layout = g.GetComponent<LayoutElement>();
            if (image != null && layout != null)
            {
                DOTween.Sequence()
                .AppendInterval(timing.sustain)
                .Append(layout.DOPreferredSize(new Vector2(0, Screen.height), timing.release))
                .AppendCallback(() => Destroy(g));
            }
        }
    }

}
