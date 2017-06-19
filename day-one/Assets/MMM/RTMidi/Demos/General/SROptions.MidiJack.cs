using System.ComponentModel;
using UnityEngine;

public partial class SROptionProperties
{
    public const string RefreshMidi = "RefreshMidi";
}
public partial class SROptions
{
#if UNITY_ANDROID && !UNITY_EDITOR
    [Category("Midi")]
    public void RefreshMidi()
    {
        MidiJack.MidiDroid droid = MidiJack.MidiDriver.Instance.midiDroid;
        droid.FindADevice();
    }
#endif
}