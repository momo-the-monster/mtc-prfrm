using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace MMM
{
    public class MMMColors : MMMSingleton<MMMColors>
    {

        public List<Color> palette;
        public Color GetRandomColor()
        {
            if (palette != null)
                return palette[Random.Range(0, palette.Count)];
            else
                return Color.gray;
        }

        public Color GetColorAt(int index)
        {
            if (palette != null)
                return palette[index % palette.Count];
            else
                return Color.gray;
        }
    }

}