using UnityEngine;
using System.Collections;

public class ScrollWithProgress : MonoBehaviour {

    public GameObject target;
    public float moveAmount = 1f;
    internal Vector2 scrollStartEnd;
    public Vector2 targetStartEnd;

	void Start () {
        scrollStartEnd.x = transform.position.z;
        scrollStartEnd.y = transform.position.z - moveAmount;
	}
	
	// Update is called once per frame
	void Update () {
        float value = Mathf.InverseLerp(targetStartEnd.x, targetStartEnd.y, target.transform.position.z);
        Vector3 newPosition = transform.position;
        newPosition.z = Mathf.Lerp(scrollStartEnd.x, scrollStartEnd.y, value);
        transform.position = newPosition;
	}
}
