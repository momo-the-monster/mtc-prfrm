using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using MMM.Midi;

public class ChordFill : MidiBehaviour {

    public GameObject prefab;
    public Transform container;
    public float alpha = 1f;
    public float widthMult = 1f;
    public float heightMult = 1f;
    Dictionary<int, GameObject> activeNotes = new Dictionary<int, GameObject>();

    ChordFill()
    {
        activeEvents = MidiEvents.NoteOn | MidiEvents.NoteOff;
    }

    public override void HandleNoteOn(int channel, int note, float velocity)
    {
        if (!activeNotes.ContainsKey(note))
        {

            durationIn = 1 - velocity;
            // Spawn object
            GameObject g = Instantiate(prefab, Vector3.zero, Quaternion.identity);
            g.transform.SetParent(container,false);

            float normalizedNote = GetNormalizedNote(note);

            Image image = g.GetComponent<Image>();
            if (image != null)
            {
                Color color = MMM.MMMColors.Instance.GetColorAt(note);
                color.a = alpha;
                image.color = color;
            }

            LayoutElement layout = g.GetComponent<LayoutElement>();
            if (layout != null)
            {
                layout.preferredWidth = 0;
                layout.DOPreferredSize(new Vector2(Screen.width * widthMult, Screen.height * heightMult), (durationIn > 0.1f) ? durationIn : 0.1f);
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
                layout.transform.DOKill();
                layout.DOKill();
                DOTween.Sequence()
                .Append(layout.DOPreferredSize(new Vector2(0, 0), durationOut))
                .AppendCallback(() => Destroy(g));
            }
        }
    }

}
