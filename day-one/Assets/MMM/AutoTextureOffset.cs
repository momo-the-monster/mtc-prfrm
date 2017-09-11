using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoTextureOffset : MonoBehaviour {

    Renderer renderer;
    public Vector2 speed = Vector2.zero;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(renderer == null)
        {
            renderer = GetComponent<Renderer>();
        }
        if(renderer != null)
        {
            var offset = renderer.sharedMaterial.mainTextureOffset;
            offset += speed;
            renderer.sharedMaterial.mainTextureOffset = offset;
        }
	}
}
