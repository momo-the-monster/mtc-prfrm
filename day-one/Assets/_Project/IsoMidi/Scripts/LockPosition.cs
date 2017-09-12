using UnityEngine;
using System.Collections;

public class LockPosition : MonoBehaviour {

    internal Vector3 startPosition;
    [SerializeField]
    bool lockX = false, lockY = false, lockZ = false;

	void Start () {
        startPosition = transform.position;    	
	}
	
    // TODO probably update this so it doesn't run EVERY SINGLE FRAME
	void Update () {
        Vector3 position = transform.position;

        if (lockX && position.x != startPosition.x)
            position.x = startPosition.x;

        if (lockY && position.y != startPosition.y)
            position.y = startPosition.y;

        if (lockZ && position.z != startPosition.z)
            position.z = startPosition.z;

        transform.position = position;
    }
}
