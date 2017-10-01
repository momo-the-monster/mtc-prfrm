using UnityEngine;
using System.Collections;

public class KeepDistanceFromTarget : MonoBehaviour {

    public Transform target;
    internal Vector3 distance;
    public Vector3 smoothing;

	void Start () {
        distance = target.transform.position - transform.position;
	}
	
	void Update () {
        Vector3 newPosition = target.transform.position - distance;
        if (smoothing.x > 0)
            newPosition.x = Mathf.Lerp(transform.position.x, newPosition.x, smoothing.x);
        if (smoothing.y > 0)
            newPosition.y = Mathf.Lerp(transform.position.y, newPosition.y, smoothing.y);
        if (smoothing.z > 0)
            newPosition.z = Mathf.Lerp(transform.position.z, newPosition.y, smoothing.z);
        transform.position = newPosition;
	}
}
