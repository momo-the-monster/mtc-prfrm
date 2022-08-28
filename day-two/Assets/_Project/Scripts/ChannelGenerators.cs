using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MMM.Midi;
using ChannelLookup = System.Collections.Generic.Dictionary<int, MakePrefabLifeControl>;

namespace MMM.Midi
{
    public class ChannelGenerators : MidiBehaviour
    {
        [SerializeField] private GameObject spawnerPrefab;
        private ChannelLookup lookup = new ChannelLookup();
        [SerializeField] private Gradient gradient;

        public override void HandleNoteOn(int channel, int note, float velocity)
        {
            // Create generator with given channel if it doesn't exist
            if (!lookup.ContainsKey(channel))
            {
                var g = Instantiate(spawnerPrefab);
                var generator = g.GetComponent<MakePrefabLifeControl>();

                // Set the listening channel
                if (generator != null)
                {
                    generator.activeChannel = (MidiJack.MidiChannel)channel;
                    generator.color = gradient.colorKeys[Random.Range(0, gradient.colorKeys.Length)].color;
                    generator.endPoint.y = generator.startPoint.y = channel/2;
                    lookup.Add(channel, generator);
                }

                // Set some random variables
            }
        }

        public override void HandleNoteOff(int channel, int note)
        {

        }
    }

}