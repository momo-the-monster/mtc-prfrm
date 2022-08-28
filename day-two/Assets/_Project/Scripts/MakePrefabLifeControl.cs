using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MMM.Midi;
using DG.Tweening;
using MidiJack;

public class MakePrefabLifeControl : MidiBehaviour
{

    public GameObject prefab;
    public Vector3 startPoint;
    public Vector3 endPoint;
    public Transform container;

    Dictionary<int, GameObject> noteObjects = new Dictionary<int, GameObject>();

    public MidiChannel activeChannel = MidiChannel.All;
    public Color color;

    MakePrefabLifeControl()
    {
        activeEvents = MidiEvents.NoteOn | MidiEvents.NoteOff;
    }

    public override void HandleNoteOn(int channel, int note, float velocity)
    {
        if (activeChannel == MidiChannel.All || (int)activeChannel == channel)
        {
            Vector3 position = PositionFromFloat(GetNormalizedNote(note));
            GameObject g = MakeAt(position);
            noteObjects.Add(note, g);
        }
    }

    Vector3 PositionFromFloat(float value)
    {
        return Vector3.Lerp(startPoint, endPoint, value);
    }

    // Trigger with dynamic float for easy Action Binding
    public void Trigger(float value)
    {
        GameObject g = MakeAt(PositionFromFloat(value));
        DOVirtual.DelayedCall(durationOut, () => DestroyTimed(g));
    }

    GameObject MakeAt(Vector3 position)
    {
        GameObject g = Instantiate(prefab, position, Quaternion.identity, container);
        g.transform.localPosition = position;
        g.GetComponent<Renderer>().material.color = color;
        return g;
    }

    public override void HandleNoteOff(int channel, int note)
    {

        if (activeChannel == MidiChannel.All || (int)activeChannel == channel)
        {

            if (noteObjects.ContainsKey(note))
            {
                GameObject g = noteObjects[note];
                noteObjects.Remove(note);
                //DestroySimple(g);
                DestroyTimed(g);
            }
            else
            {
                Debug.LogErrorFormat("Tried to remove note at {0} which is not in noteObjects", note);
            }
        }
    }

    public void DestroySimple(GameObject g)
    {
        Destroy(g);
    }

    public void DestroyTimed(GameObject g)
    {
        g.transform.DOScale(0, durationOut).OnComplete(() => Destroy(g));
    }

    // Draw start and endpoints into scene view
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawCube(startPoint, Vector3.one * 0.25f);

        Gizmos.color = Color.red;
        Gizmos.DrawCube(endPoint, Vector3.one * 0.25f);
    }

}
