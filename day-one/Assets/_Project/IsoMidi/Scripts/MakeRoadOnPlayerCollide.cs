using UnityEngine;
using System.Collections;
using System;

public class MakeRoadOnPlayerCollide : MonoBehaviour
{
    public string triggerTag;
    public RoadBuilderHelpful builder;
    public Vector3 position;
    public Vector3 scale;

    void Start()
    {

    }

    void Update()
    {

    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(triggerTag))
        {
            Trigger();
        }
    }

    private void Trigger()
    {
        // Create optimal position to catch player
        Vector3 newPosition = transform.position + position;
        // Grow it
        builder.GrowOneAtWithScale(newPosition, scale);
    }
}