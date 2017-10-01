using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MMM.Midi.Demo
{
    public class Trigger12Bars : MidiBehaviour
    {

        public List<Image> noteImages;
        public float fadeOutDuration = 0.25f;

        private void Start()
        {
            for (int i = 0; i < noteImages.Count; i++)
            {
                noteImages[i].CrossFadeAlpha(0, 0, false);
            }
        }

        public override void HandleNoteOn(int channel, int note, float velocity)
        {
            Image target = noteImages[note % noteImages.Count];
            if (target != null)
            {
                target.CrossFadeAlpha(1, 0, false);
            }
        }

        public override void HandleNoteOff(int channel, int note)
        {
            Image target = noteImages[note % noteImages.Count];
            if (target != null)
            {
               target.CrossFadeAlpha(0, fadeOutDuration, false);
            }
        }
    }

}