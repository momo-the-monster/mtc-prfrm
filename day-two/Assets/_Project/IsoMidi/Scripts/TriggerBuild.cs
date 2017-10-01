using UnityEngine;
using System.Collections;
using System;

public class TriggerBuild : MonoBehaviour
{

    public string triggerTag;
    public RoadBuilderHelpful builder;

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag(triggerTag))
            Trigger();
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
        builder.GrowOne();
    }
}
