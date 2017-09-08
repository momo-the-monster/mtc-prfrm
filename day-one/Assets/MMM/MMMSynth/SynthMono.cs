using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SynthMono : MonoBehaviour {

    AudioSource source;
    public SynthModule module;

    private float _volume;
    private float _pan = 0;

    void Start () {
        source = gameObject.AddComponent<AudioSource>();
        module = new SynthModule();
    }

    public Lope KeyOn(int note, float attack = 0, float release = 0.01f)
    {
        module.env.attack = attack;
        module.env.release = release;
        module.osc.SetNote(note);
        module.env.KeyOn();
        return module.env;
    }

    public void SetParam(float fm_mul, float fm_mod, float volume)
    {
        module.osc.multiplier = fm_mul;
        module.osc.modulation = fm_mod;
        _volume = volume;
    }

    bool rising = false;
    public float rate = 1f;
    public int multMax = 4;
    public int multMin = 1;

    void OnAudioFilterRead(float[] data, int channels)
    {
        for (var i = 0; i < data.Length; i ++)
        {
            data[i] = module.Run() * _volume;
        }

        if (rising)
        {
            if (module.osc.multiplier >= multMax)
            {
                rising = false;
            }
        }
        else
        {
            if (module.osc.multiplier <= multMin)
            {
                rising = true;
            }
        }

        module.osc.multiplier += rate * (rising ? 1 : -1);
    }
}
