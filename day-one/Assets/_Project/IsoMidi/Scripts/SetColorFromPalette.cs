using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SetColorFromPalette : MonoBehaviour {

    void Start () {
	}

    public void Trigger(int index)
    {

        // Get Color
        Color color = MMM.MMMColors.Instance.GetColorAt(index);

        // Set Light Color
        Light light = GetComponent<Light>();
        if (light != null)
        {
            light.color = color;
        }

        MeshRenderer r = GetComponent<MeshRenderer>();
        if (r != null)
        {
            r.material.color = color;
        }
    }

    void Trigger()
    {
        Trigger(Random.Range(0, 100));
    }
	
}
