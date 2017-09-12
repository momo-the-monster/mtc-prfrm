using UnityEngine;

public class CameraFollowRoadCubes : MonoBehaviour {

    public IsometricCameraControl control;

	void OnEnable()
    {
        RoadCube.OnIncarnate += OnIncarnate;
    }

    private void OnIncarnate(RoadCube cube)
    {
        control.target = cube.transform;
    }

    void OnDisable()
    {
        RoadCube.OnIncarnate -= OnIncarnate;
    }
}
