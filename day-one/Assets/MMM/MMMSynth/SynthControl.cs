using UnityEngine;
using System.Collections.Generic;
using System;

// Adapted from https://github.com/JZito/Unity-Synthesizer-in-C-Sharp/blob/develop/Assets/Scripts/Synth/SynthControl.cs
public class SynthControl : MonoBehaviour {
    public Lope modEnv;
    public int voices = 2;
    private int switcher = 0;
    List<SynthPair> modules;

    [Range(0.0f, 1.0f)] public float volume = 0.5f;
    [Range(-1.0f, 1.0f)] public float stereo = 0.5f;
    [Range(1, 24f)] public float fm_mul = 8f;
    [Range(0.0f, 1.0f)] public float fm_mod = 0.08f;

    private float _volume;
    private float _stereo;
    private float _fm_mul;
    private float _fm_mod;

    void Start()
    {
        modules = new List<SynthPair>();
        // Create one GameObject per SynthPair
        for (int i = 0; i < voices; i++)
        {
            GameObject g = new GameObject("SynthPair" + i, typeof(SynthPair));
            g.transform.SetParent(transform);
            modules.Add(g.GetComponent<SynthPair>());
        }
    }

    void Update() {

        CheckForParamChanges();

    }

    private void CheckForParamChanges()
    {
        if(_fm_mul != fm_mul || _fm_mod != fm_mod || _volume != volume || _stereo != stereo)
        {
            // Set private parameters from public
            _fm_mod = fm_mod;
            _fm_mul = fm_mul;
            _stereo = stereo;
            _volume = volume;

            // set parameters on all modules
            for (int i = 0; i < modules.Count; i++)
            {
                modules[i].SetParam(_fm_mul, _fm_mod, _volume, _stereo);
            }

        }
    }

    bool doLeft = true;

    public Lope KeyOn(int note) {
        // increment switcher
        switcher = (switcher + 1) % voices;

        var module = modules[switcher];
        return module.KeyOn(note);
    }
}
