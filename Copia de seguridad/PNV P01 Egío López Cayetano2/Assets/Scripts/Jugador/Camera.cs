using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Camera : MonoBehaviour
{
    [Header("Target Settings")]
    public Transform target;          // El objetivo que la c�mara sigue (el jugador).

    [Header("Camera Movement Settings")]
    public float smoothSpeed = 0.125f; // Velocidad de suavizado de la c�mara.
    public Vector3 offset;             // Desplazamiento de la c�mara con respecto al jugador.

    [Header("Look Ahead Settings")]
    public float lookAheadDistanceX = 2f;  // Cu�nto se adelanta la c�mara en la direcci�n del movimiento horizontal del jugador.
    public float verticalOffset = 1f;      // Offset vertical para que la c�mara no est� centrada exactamente en el jugador.

    [Header("Level Bounds")]
    public bool useLevelBounds = false;    // Si queremos restringir la c�mara dentro de los l�mites del nivel.
    public Vector2 minBounds;              // L�mite m�nimo de la c�mara (X, Y).
    public Vector2 maxBounds;              // L�mite m�ximo de la c�mara (X, Y).

    private Vector3 velocity = Vector3.zero; // Para almacenar la velocidad del suavizado.

    private void LateUpdate()
    {
        if (!target) return;  // Si no hay objetivo, no hacer nada.

        // Posici�n deseada de la c�mara.
        Vector3 desiredPosition = target.position + offset;

        // Desplazar la c�mara ligeramente en la direcci�n del movimiento horizontal.
        float playerVelocityX = Input.GetAxisRaw("Horizontal");
        desiredPosition.x += playerVelocityX * lookAheadDistanceX;

        // A�adir el desplazamiento vertical.
        desiredPosition.y += verticalOffset;

        // Si estamos usando l�mites de nivel, ajustamos la posici�n de la c�mara para que no salga de los l�mites.
        if (useLevelBounds)
        {
            desiredPosition.x = Mathf.Clamp(desiredPosition.x, minBounds.x, maxBounds.x);
            desiredPosition.y = Mathf.Clamp(desiredPosition.y, minBounds.y, maxBounds.y);
        }

        // Aplicar suavizado a la transici�n de la c�mara.
        Vector3 smoothedPosition = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothSpeed);

        // Establecer la posici�n suavizada a la c�mara.
        transform.position = smoothedPosition;
    }

    // Mostrar los l�mites del nivel en el editor de Unity.
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