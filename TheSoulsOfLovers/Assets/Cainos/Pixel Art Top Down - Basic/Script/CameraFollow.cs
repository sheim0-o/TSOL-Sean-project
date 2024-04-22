using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cainos.PixelArtTopDown_Basic
{
    //let camera follow target
    public class CameraFollow : MonoBehaviour
    {
        public Transform target;
        public float lerpSpeed = 1.0f;
        public Collider2D mapBoundsCollider;
        private Bounds mapBounds;
        private Camera camera;
        private float cameraHalfOfHeight;
        private float cameraHalfOfWidth;
        private Vector3 targetPos;

        private void Start()
        {
            if (target == null) return;

            if (mapBoundsCollider != null)
            {
                mapBounds = mapBoundsCollider.bounds;
                camera = GetComponent<Camera>();

                cameraHalfOfHeight = camera.orthographicSize;
                cameraHalfOfWidth = cameraHalfOfHeight * camera.aspect;

                targetPos = new Vector3(target.position.x, target.position.y, transform.position.z);
                targetPos.x = Mathf.Clamp(targetPos.x, mapBounds.min.x + cameraHalfOfWidth, mapBounds.max.x - cameraHalfOfWidth);
                targetPos.y = Mathf.Clamp(targetPos.y, mapBounds.min.y + cameraHalfOfHeight, mapBounds.max.y - cameraHalfOfHeight);
                transform.position = targetPos;
            }
            else
            {
                transform.position = target.position;
            }
        }

        private void Update()
        {
            if (target == null) return;
            else if (transform.position != target.position) {
                if (mapBounds != null)
                {
                    targetPos = new Vector3(target.position.x, target.position.y, transform.position.z);
                    targetPos.x = Mathf.Clamp(targetPos.x, mapBounds.min.x + cameraHalfOfWidth, mapBounds.max.x - cameraHalfOfWidth);
                    targetPos.y = Mathf.Clamp(targetPos.y, mapBounds.min.y + cameraHalfOfHeight, mapBounds.max.y - cameraHalfOfHeight);
                    transform.position = Vector3.Lerp(transform.position, targetPos, lerpSpeed * Time.deltaTime);
                }
                else
                    transform.position = Vector3.Lerp(transform.position, target.position, lerpSpeed * Time.deltaTime);
            }
        }
    }
}
