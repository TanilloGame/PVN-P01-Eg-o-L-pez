using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Camera : MonoBehaviour
{
    [Header("Target Settings")]
    public Transform target;          // El objetivo que la cámara sigue (el jugador).

    [Header("Camera Movement Settings")]
    public float smoothSpeed = 0.125f; // Velocidad de suavizado de la cámara.
    public Vector3 offset;             // Desplazamiento de la cámara con respecto al jugador.

    [Header("Look Ahead Settings")]
    public float lookAheadDistanceX = 2f;  // Cuánto se adelanta la cámara en la dirección del movimiento horizontal del jugador.
    public float verticalOffset = 1f;      // Offset vertical para que la cámara no esté centrada exactamente en el jugador.

    [Header("Level Bounds")]
    public bool useLevelBounds = false;    // Si queremos restringir la cámara dentro de los límites del nivel.
    public Vector2 minBounds;              // Límite mínimo de la cámara (X, Y).
    public Vector2 maxBounds;              // Límite máximo de la cámara (X, Y).

    private Vector3 velocity = Vector3.zero; // Para almacenar la velocidad del suavizado.

    private void LateUpdate()
    {
        if (!target) return;  // Si no hay objetivo, no hacer nada.

        // Posición deseada de la cámara.
        Vector3 desiredPosition = target.position + offset;

        // Desplazar la cámara ligeramente en la dirección del movimiento horizontal.
        float playerVelocityX = Input.GetAxisRaw("Horizontal");
        desiredPosition.x += playerVelocityX * lookAheadDistanceX;

        // Añadir el desplazamiento vertical.
        desiredPosition.y += verticalOffset;

        // Si estamos usando límites de nivel, ajustamos la posición de la cámara para que no salga de los límites.
        if (useLevelBounds)
        {
            desiredPosition.x = Mathf.Clamp(desiredPosition.x, minBounds.x, maxBounds.x);
            desiredPosition.y = Mathf.Clamp(desiredPosition.y, minBounds.y, maxBounds.y);
        }

        // Aplicar suavizado a la transición de la cámara.
        Vector3 smoothedPosition = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothSpeed);

        // Establecer la posición suavizada a la cámara.
        transform.position = smoothedPosition;
    }

    // Mostrar los límites del nivel en el editor de Unity.
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