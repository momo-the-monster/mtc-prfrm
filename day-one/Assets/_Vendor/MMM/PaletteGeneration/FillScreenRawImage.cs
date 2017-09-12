using System;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
/*
 * Scale a Quad to fill an Orthographic Camera's Field of View
 * */

namespace MMM
{
    [ExecuteInEditMode]
    public class FillScreenRawImage : MonoBehaviour
    {
        private bool doUpdate = true;
        RawImage image;

        private float aspectRatio = 1f;
        public float AspectRatio
        {
            get { return aspectRatio; }
            set { aspectRatio = value; doUpdate = true; }
        }

        private float scaleMult;

        // Set scaleMult to the larger value between width or height on Enable
        private void OnEnable()
        {
            scaleMult = (Mathf.Max(transform.localScale.x, transform.localScale.y));
        }

        private void Start()
        {
            image = GetComponent<RawImage>();
        }

        void Update()
        {
            // Exit early if not needed
            if (!doUpdate)
                return;
            else
                doUpdate = false;

            PerspectiveUpdate();
        }

        private void PerspectiveUpdate()
        {
            Camera cam = Camera.main;

            Vector3 worldMin = cam.ViewportToWorldPoint(new Vector3(0, 0, 0));
            Vector3 worldMax = cam.ViewportToWorldPoint(new Vector3(1, 1, 0));
            float width = worldMax.x - worldMin.x;
            float height = worldMax.y - worldMin.y;
            Rect targetRect = image.uvRect;
            Rect startRect = image.uvRect;
            Rect currentRect = image.uvRect;

            // Apply new scale
            if (width >= height)
            {
                targetRect.width = aspectRatio;
                DOVirtual.Float(0f, 1f, 2f, (float value) => { currentRect.width = Mathf.Lerp(startRect.width, targetRect.width, value); image.uvRect = currentRect; }).SetEase(Ease.OutExpo);
            }
            else
            {
                targetRect.height = aspectRatio;
                DOVirtual.Float(0f, 1f, 2f, (float value) => { currentRect.height = Mathf.Lerp(startRect.height, targetRect.height, value); image.uvRect = currentRect; }).SetEase(Ease.OutExpo);
            }

        }
    }
}