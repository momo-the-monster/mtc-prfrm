using System;
using UnityEngine;
using DG.Tweening;
/*
 * Scale a Quad to fill an Orthographic Camera's Field of View
 * */

namespace MMM
{
    [ExecuteInEditMode]
    public class FillScreen : MonoBehaviour
    {
        private bool doUpdate = true;

        private float aspectRatio = 1f;
        public float AspectRatio
        {
            get { return aspectRatio; }
            set { aspectRatio = value;  doUpdate = true; }
        }

        public bool orthoMode = true;

        private float scaleMult;

        // Set scaleMult to the larger value between width or height on Enable
        private void OnEnable()
        {
            scaleMult = (Mathf.Max(transform.localScale.x, transform.localScale.y));
        }

        void Update()
        {
            // Exit early if not needed
            if (!doUpdate)
                return;
            else
                doUpdate = false;

            if (orthoMode)
            {
                OrthoUpdate();
            }
            else
            {
                PerspectiveUpdate();
            }
        }

        private void PerspectiveUpdate()
        {
            Camera cam = Camera.main;

            Vector3 worldMin = cam.ViewportToWorldPoint(new Vector3(0, 0, 0));
            Vector3 worldMax = cam.ViewportToWorldPoint(new Vector3(1, 1, 0));
            float width = worldMax.x - worldMin.x;
            float height = worldMax.y - worldMin.y;
            Vector3 scale = new Vector3(scaleMult, scaleMult, scaleMult);
            // Find smaller dimension and scale it down proportionally to larger one
            if (width >= height)
                scale.y = scaleMult / aspectRatio;
            else
                scale.x = scaleMult / aspectRatio;

            // Apply new scale
            transform.DOScale(scale, 2f).SetEase(Ease.OutExpo);
        }

        void OrthoUpdate()
        {
            Camera cam = Camera.main;
            // Use the Viewport bounds as the target scale by converting it to World Space
            Vector3 worldMin = cam.ViewportToWorldPoint(new Vector3(0, 0, 0));
            Vector3 worldMax = cam.ViewportToWorldPoint(new Vector3(1, 1, 0));
            float width = worldMax.x - worldMin.x;
            float height = worldMax.y - worldMin.y;
            Vector3 scale = new Vector3(width, height, 0f);

            // Find smaller dimension and scale it down proportionally to larger one
            if (width >= height)
                scale.y = width / aspectRatio;
            else
                scale.x = height * aspectRatio;

            // Apply new scale
            transform.localScale = scale;
        }
    }
}