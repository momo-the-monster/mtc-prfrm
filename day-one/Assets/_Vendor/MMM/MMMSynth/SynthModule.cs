using UnityEngine;
using System.Collections;

public class SynthModule {
	public Osc osc;
    public Lope env;
    float level;

    public SynthModule()
    {
        Start();
    }

    private void Start()
    {
        osc = new Osc();
        env = new Lope();
    }

    public void SetParam(float fm_mul, float  fm_mod) {
        osc.multiplier = fm_mul;
        osc.modulation = fm_mod;
    }

    public float Run() {
        level = osc.Run() * env.GetLevel();
        env.Update();
        return level;
    }
}