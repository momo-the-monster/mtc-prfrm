using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(SynthControl))]

public class SynthTriggerKeyboard : MonoBehaviour
{

    [Range(0.003f, 3.0f)]
    public float env_atk = 1.0f;
    [Range(0.003f, 3.0f)]
    public float env_rel = 1.0f;
    [Range(24, 48)]
    public int scale = 24;
    private Lope envelope;
    private SynthControl synth;
    public NReverb reverb;

    public void Start()
    {
        synth = GetComponent<SynthControl>();
        envelope = GetComponent<Lope>();

        envelope.sustain = false;
        reverb = GetComponent<NReverb>();
    }

    public void Update()
    {


        if (Input.GetKeyDown(KeyCode.Space))
        {
            envelope = synth.KeyOn(8).module.env;
        }
        if(Input.GetKeyUp(KeyCode.Space))
        {
            envelope.KeyOff();
        }
        
        envelope.attack = env_atk;
        envelope.release = env_rel;
    }

}
