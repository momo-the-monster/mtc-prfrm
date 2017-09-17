using UnityEngine;
using MMM.Midi;
using DG.Tweening;

public class MidiToPosition : MidiBehaviour {

    public Ease easeType = Ease.OutCubic;

    Vector3 ogPosition;
    Vector3 ogVPosition;
    public Vector2 xMinMax = new Vector2(0, 1);
    public Vector2 yMinMax = new Vector2(0, 1);
    MidiHistory history = new MidiHistory();
    public UnityEngine.UI.Text field;

    private void Start()
    {
        ogPosition = transform.position;
        ogVPosition = Camera.main.WorldToViewportPoint(ogPosition);
    }

    public override void HandleNoteOn(int channel, int note, float velocity)
    {
        history.UpdateNoteHistory(note);

        float value = history.GetNormalizedHistoryPosition(note);
        value = Mathf.Lerp(xMinMax.x, xMinMax.y, value);
        Vector3 position = Camera.main.ViewportToWorldPoint(new Vector3(value, Mathf.Lerp(yMinMax.x, yMinMax.y, velocity), ogVPosition.z));
        MoveTo(position);

        // Write debug message to text field
        if(field != null)
        {
            string result = string.Format("History:{0}\nRange:{1} - {2}\nInput {3} Becomes {4}", history.ToString(), history.GetRange().x, history.GetRange().y, note, value);
            field.text = result;
        }
    }

    void MoveTo(Vector3 position)
    {
        transform.DOMove(position, envelope.attack);
    }
}
