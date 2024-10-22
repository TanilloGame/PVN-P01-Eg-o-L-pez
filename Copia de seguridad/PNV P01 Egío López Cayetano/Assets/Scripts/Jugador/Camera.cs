using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Camera : MonoBehaviour
{
    [Header("Target Settings")]
    public Transform target;          

    [Header("Camera Movement Settings")]
    public float smoothSpeed = 0.125f; 
    public Vector3 offset;             

    [Header("Look Ahead Settings")]
    public float lookAheadDistanceX = 2f;  
    public float verticalOffset = 1f;      

    [Header("Level Bounds")]
    public bool useLevelBounds = false;    
    public Vector2 minBounds;              
    public Vector2 maxBounds;              

    private Vector3 velocity = Vector3.zero; 

    private void LateUpdate()
    {
        if (!target) return;  

        
        Vector3 desiredPosition = target.position + offset;

        
        float playerVelocityX = Input.GetAxisRaw("Horizontal");
        desiredPosition.x += playerVelocityX * lookAheadDistanceX;

        
        desiredPosition.y += verticalOffset;

        
        if (useLevelBounds)
        {
            desiredPosition.x = Mathf.Clamp(desiredPosition.x, minBounds.x, maxBounds.x);
            desiredPosition.y = Mathf.Clamp(desiredPosition.y, minBounds.y, maxBounds.y);
        }

        
        Vector3 smoothedPosition = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothSpeed);

        
        transform.position = smoothedPosition;
    }

    
    private void OnDrawGizmosSelected()
    {
        if (useLevelBounds)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(new Vector3(minBounds.x, minBounds.y, 0), new Vector3(minBounds.x, maxBounds.y, 0));
            Gizmos.DrawLine(new Vector3(maxBounds.x, minBounds.y, 0), new Vector3(maxBounds.x, maxBounds.y, 0));
            Gizmos.DrawLine(new Vector3(minBounds.x, maxBounds.y, 0), new Vector3(maxBounds.x, maxBounds.y, 0));
            Gizmos.DrawLine(new Vector3(minBounds.x, minBounds.y, 0), new Vector3(maxBounds.x, minBounds.y, 0));
        }
    }
}