using UnityEngine;
using System.Collections.Generic;
using System;

// Adapted from https://github.com/JZito/Unity-Synthesizer-in-C-Sharp/blob/develop/Assets/Scripts/Synth/SynthControl.cs
public class SynthControl : MonoBehaviour {
    public Lope modEnv;
    public int voices = 2;
    private int switcher = 0;
    List<SynthMono> modules;

    [Range(0.0f, 1.0f)] public float volume = 0.5f;
    [Range(-1.0f, 1.0f)] public float stereo = 0.5f;
    [Range(1, 24f)] public float fm_mul = 8f;
    [Range(0.0f, 1.0f)] public float fm_mod = 0.08f;
    [Range(0f, 5f)]
    public float rate = 1;
    [Range(1, 24)]
    public int mult_min = 1;
    [Range(1, 24)]
    public int mult_max = 4;

    private float _volume;
    private float _stereo;
    private float _fm_mul;
    private float _fm_mod;
    private float _rate = 1f;
    private int _mult_max = 4;
    private int _mult_min = 1;

    void Start()
    {
        modules = new List<SynthMono>();
        // Create one GameObject per SynthPair
        for (int i = 0; i < voices; i++)
        {
            GameObject g = new GameObject("SynthMono" + i, typeof(SynthMono));
            g.transform.SetParent(transform);
            modules.Add(g.GetComponent<SynthMono>());
        }
    }

    void Update() {

        CheckForParamChanges();

    }

    private void CheckForParamChanges()
    {
        if(_fm_mul != fm_mul || _fm_mod != fm_mod || _volume != volume || _stereo != stereo || _mult_min != mult_min || _mult_max != mult_max || _rate != rate)
        {
            // Set private parameters from public
            _fm_mod = fm_mod;
            _fm_mul = fm_mul;
            _stereo = stereo;
            _volume = volume;
            _rate = rate;
            _mult_max = mult_max;
            _mult_min = mult_min;

            // set parameters on all modules
            for (int i = 0; i < modules.Count; i++)
            {
                modules[i].SetParam(_fm_mul, _fm_mod, _volume);
                modules[i].rate = _rate;
                modules[i].multMin = _mult_min;
                modules[i].multMax = _mult_max;
            }

        }
    }

    public SynthMono KeyOn(int note, float attack = 0) {
        // increment switcher
        switcher = (switcher + 1) % voices;

        var module = modules[switcher];
        module.KeyOn(note, attack);
        return module;
    }
}
