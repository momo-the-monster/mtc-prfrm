using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MidiHistory {

    public MidiHistory(int cacheSize = 16)
    {
        historySize = cacheSize;
    }

    int historySize = 16;
    List<int> noteHistory = new List<int>();
    Vector2 minMaxHistory = Vector2.zero;

    public float GetNormalizedHistoryPosition(int note)
    {
        return Mathf.InverseLerp(minMaxHistory.x, minMaxHistory.y, note);
    }

    public void UpdateNoteHistory(int newNote)
    {
        // Should we clear history?
        if (noteHistory.Count > historySize)
        {
            noteHistory.RemoveAt(0);
        }
        noteHistory.Add(newNote);

        UpdateMinMax();
    }

    void UpdateMinMax()
    {
        // Update sorting afterwards
        var sorted = noteHistory.OrderBy(item => item).ToList<int>();
        minMaxHistory.x = sorted[0];
        minMaxHistory.y = sorted[sorted.Count - 1];
    }

}
