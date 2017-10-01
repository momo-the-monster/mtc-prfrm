using UnityEngine;
using System.Collections;
using DG.Tweening;

public class KeepRotation : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 e = transform.rotation.eulerAngles;
        if(e.y != 180 && e.y != 0)
        {
            if(e.y < 180)
            {
                e.y = 0;
            } else
            {
                e.y = 180;
            }

            transform.DORotate(e, 0.25f);
        }
        Debug.LogFormat("Rotation: {0}", e.y);
	}
}
