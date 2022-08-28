using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MMM.Midi;
using ChannelLookup = System.Collections.Generic.Dictionary<int, MMM.Midi.MidiBehaviour>;

namespace MMM.Midi
{
    public class MakeStrips : MidiBehaviour
    {
        [SerializeField] private GameObject spawnerPrefab;
        [SerializeField] private Transform container;
        private ChannelLookup lookup = new ChannelLookup();
        [SerializeField] private Quantizer quantizer;

        public override void OnEnable()
        {
            base.OnEnable();
            quantizer.OnPaletteUpdate += OnPaletteUpdate;
        }

        private void OnPaletteUpdate(List<Color> colors)
        {
            Clear();
        }

        private void OnDisable()
        {
            quantizer.OnPaletteUpdate -= OnPaletteUpdate;
        }

        public override void HandleNoteOn(int channel, int note, float velocity)
        {
            // Create generator with given channel if it doesn't exist
            if (!lookup.ContainsKey(channel))
            {
                var g = Instantiate(spawnerPrefab, container);
                var generator = g.GetComponent<MidiBehaviour>();

                // Set the listening channel
                if (generator != null)
                {
                    generator.midiChannel = (MidiJack.MidiChannel)channel;
                    lookup.Add(channel, generator);
                }

                // reset idle timer
            }
        }

        public override void HandleNoteOff(int channel, int note)
        {
            // set idle timer
        }

        public void Clear()
        {
            foreach (var item in lookup)
            {
                Destroy(item.Value.gameObject);
            }

            lookup.Clear();
        }
    }

}