using UnityEngine;
using System.Collections;

public class SetLightIntensityFromTargetDistance : MonoBehaviour {

    public Light light;
    public Transform target;
    public Vector2 range;

	void Update () {
        // Generate new intensity value
        float distance = Vector3.Distance(transform.position, target.position);
        float normalized = Mathf.InverseLerp(range.x, range.y, distance);
        float intensity = Mathf.Lerp(0f, 8f, normalized);
        // Set the light if it's there
        if (light != null)
            light.intensity = intensity;
	}
}
