using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MMM.Midi;

public class MakePrefabSimple : MidiBehaviour {

    public GameObject prefab;
    public Vector3 startPoint;
    public Vector3 endPoint;
    public Transform container;

    MakePrefabSimple()
    {
        activeEvents = MidiEvents.NoteOn;
    }

    public override void HandleNoteOn(int channel, int note, float velocity)
    {
        Vector3 position = Vector3.Lerp(startPoint, endPoint, GetNormalizedNote(note));
        Trigger(position);
    }

    void Trigger(Vector3 position)
    {
        Instantiate(prefab, position, Quaternion.identity, container);
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
