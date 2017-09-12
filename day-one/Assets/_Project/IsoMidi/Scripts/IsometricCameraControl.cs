using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

namespace mmm
{
    [System.Serializable]
    public struct Offset
    {
        public Vector3 position, rotation;
        public Offset(Vector3 position, Vector3 rotation)
        {
            this.position = position;
            this.rotation = rotation;
        }
    }

}
public class IsometricCameraControl : MonoBehaviour {

    public Camera camera;
    [SerializeField]
    public int currentOffsetIndex = 0;
    public List<mmm.Offset> offsets;
    public bool userMoved = false;
    public Transform target;
    public bool doFollow = false;

    public bool velocityControlsZoom = false;
    public Vector3 targetVelocity;
    public Vector3 targetDeltaPosition;

	void Start () {

        if (velocityControlsZoom)
        {
            camera.DOOrthoSize(orthoSizeRange.x, 1f);
        }
	}

    void OnDisable()
    {
        CancelInvoke("Recenter");
    }

    public void IncrementOffset()
    {
        currentOffsetIndex = (currentOffsetIndex + 1) % offsets.Count;
    }

    public void DecrementOffset()
    {
        currentOffsetIndex -= 1;
        if (currentOffsetIndex < 0)
            currentOffsetIndex = (offsets.Count - 1);
    }

    public mmm.Offset GetOffset()
    {
        return offsets[currentOffsetIndex];
    }


    public float targetMagnitude;
    public float magMult = 1f;
    [SerializeField]
    Vector2 orthoSizeRange;
    public float zoomSmooth = 0.1f;
    public float cameraMoveDuration = 0.25f;
    public void Recenter()
    {
        if(target != null)
        {

        Vector3 newPosition = target.position + GetOffset().position;
        //   newPosition.y = camera.transform.position.y;
        if (velocityControlsZoom)
        {
            float newTargetMagnitude = Vector3.Distance(camera.transform.position, newPosition) * magMult;
            targetMagnitude = Mathf.Lerp(targetMagnitude, newTargetMagnitude, zoomSmooth);
            float newSize = Mathf.Lerp(orthoSizeRange.x, orthoSizeRange.y, targetMagnitude);
           // camera.DOOrthoSize(newSize, 0.1f).SetEase(Ease.InOutCirc);
            camera.orthographicSize = newSize;
        }
        camera.transform.DOMove(newPosition, cameraMoveDuration);
        }
    }


    void FixedUpdate()
    {
        if (doFollow)
        {
            Recenter();
        }
    }
	void Update () {

        bool pageUp = Input.GetKeyDown(KeyCode.UpArrow);
        bool pageDown = Input.GetKeyDown(KeyCode.DownArrow);
        if (pageUp || pageDown)
        {
            userMoved = true;
            float currentSize = camera.orthographicSize;
            float newSize = currentSize;
            float amountToMove = 0.1f;
            newSize += pageDown ? amountToMove : -amountToMove;

        //    DOTween.To(() => camera.orthographicSize, x => camera.orthographicSize = x, newSize, 1);
        }

        bool left = Input.GetKeyDown(KeyCode.LeftArrow);
        bool right = Input.GetKeyDown(KeyCode.RightArrow);
        bool leftOrRight = left || right;
        if (leftOrRight)
        {
            userMoved = true;
            Vector3 currentPosition = camera.transform.position;
            Vector3 newPosition = currentPosition;
            float amountToMove = 1f;
            newPosition.x += left ? amountToMove : -amountToMove;
            transform.DOMove(newPosition, 1f);
        }

	}
}
