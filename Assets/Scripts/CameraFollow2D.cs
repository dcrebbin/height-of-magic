using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow2D : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float smoothSpeed = 0.125f;
    [SerializeField] private Vector3 offset = new Vector3(0f, 0f, -10f);

    [SerializeField] private float minX = -10f;
    [SerializeField] private float maxX = 10f;
    [SerializeField] private float minY = -10f;
    [SerializeField] private float maxY = 10f;

    private void FixedUpdate()
    {
        if (target == null)
            return;

        // First clamp the target position within bounds
        float clampedTargetX = Mathf.Clamp(target.position.x, minX, maxX);
        float clampedTargetY = Mathf.Clamp(target.position.y, minY, maxY);
        Vector3 clampedTargetPosition = new Vector3(clampedTargetX, clampedTargetY, target.position.z);

        // Then calculate desired position based on clamped target
        Vector3 desiredPosition = clampedTargetPosition + offset;

        // Smooth the camera movement
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        transform.position = smoothedPosition;
    }
}
