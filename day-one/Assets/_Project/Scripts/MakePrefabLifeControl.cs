using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MMM.Midi;
using DG.Tweening;

public class MakePrefabLifeControl : MidiBehaviour
{

    public GameObject prefab;
    public Vector3 startPoint;
    public Vector3 endPoint;
    public Transform container;

    Dictionary<int, GameObject> noteObjects = new Dictionary<int, GameObject>();

    MakePrefabLifeControl()
    {
        activeEvents = MidiEvents.NoteOn | MidiEvents.NoteOff;
    }

    public override void HandleNoteOn(int channel, int note, float velocity)
    {
        Vector3 position = Vector3.Lerp(startPoint, endPoint, GetNormalizedNote(note));
        Trigger(position, note);
    }

    void Trigger(Vector3 position, int note)
    {
        GameObject g = Instantiate(prefab, position, Quaternion.identity, container);
        noteObjects.Add(note, g);
    }

    public override void HandleNoteOff(int channel, int note)
    {
        if (noteObjects.ContainsKey(note))
        {
            GameObject g = noteObjects[note];
            noteObjects.Remove(note);
            DestroySimple(g);
            //DestroyTimed(g);
        }
        else
        {
            Debug.LogErrorFormat("Tried to remove note at {0} which is not in noteObjects", note);
        }
    }

    public void DestroySimple(GameObject g)
    {
        Destroy(g);
    }

    public void DestroyTimed(GameObject g)
    {
        g.transform.DOScale(0, timing.release).OnComplete(() => Destroy(g));
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
