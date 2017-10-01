using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MMM.Midi;

[RequireComponent(typeof(Animator))]
public class MidiAnimator : MidiBehaviour {

    Animator animator;

    public int key0Animation = -1;
    public int key1Animation = -1;
    public int key2Animation = -1;
    public int key3Animation = -1;
    public int key4Animation = -1;
    public int key5Animation = -1;
    public int key6Animation = -1;
    public int key7Animation = -1;
    public int key8Animation = -1;
    public int key9Animation = -1;
    public int key10Animation = -1;
    public int key11Animation = -1;

    List<string> animations;

	void Start () {
        animator = GetComponent<Animator>();
        animations = new List<string>();
        foreach (var item in animator.runtimeAnimatorController.animationClips)
        {
            animations.Add(item.name);
        }
    }

    public override void HandleNoteOn(int channel, int note, float velocity)
    {
        int[]animationKeys = {key0Animation, key1Animation, key2Animation, key3Animation, key4Animation, key5Animation, key6Animation, key7Animation, key8Animation, key9Animation, key10Animation, key11Animation};
        var animationToPlay = animations[animationKeys[note % 12]];
        animator.Play(animationToPlay);
    }
}
