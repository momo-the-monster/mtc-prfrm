using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SynthPair : MonoBehaviour {

    AudioSource source;
    public SynthModule moduleLeft;
    public SynthModule moduleRight;
    bool doLeft = true;

    private float _volume;
    private float _stereo;

    void Start () {
        source = gameObject.AddComponent<AudioSource>();
        moduleLeft = new SynthModule();
        moduleRight = new SynthModule();
	}

    public Lope KeyOn(int note)
    {
        doLeft = !doLeft;
        var module = doLeft ? moduleLeft : moduleRight;
        module.osc.SetNote(note);
        module.env.KeyOn();
        return module.env;
    }

    public void SetParam(float fm_mul, float fm_mod, float volume, float stereo)
    {
        moduleLeft.osc.multiplier = fm_mul;
        moduleLeft.osc.modulation = fm_mod;
        moduleRight.osc.multiplier = fm_mul;
        moduleRight.osc.modulation = fm_mod;
        _volume = volume;
        _stereo = stereo;
    }

    void OnAudioFilterRead(float[] data, int channels)
    {
        float lv = _volume * 0.5f * (_stereo + 1f);
        float rv = _volume - lv;
        for (var i = 0; i < data.Length; i += 2)
        {
            float s1 = moduleLeft.Run();
            float s2 = moduleRight.Run();
            data[i] = s1 * lv + s2 * rv;
            data[i + 1] = s1 * rv + s2 * lv;
        }
    }
}