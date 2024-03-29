﻿using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Spin : MonoBehaviour {

    public KeyCode triggerKey = KeyCode.Space;
    Rigidbody body;
    public float magnitude = 1;

	void Start () {
        body = GetComponent<Rigidbody>();
	}
	
	void Update () {
        if (Input.GetKeyDown(triggerKey))
        {
            body.angularVelocity += Random.onUnitSphere * magnitude;
        }
	}
}
