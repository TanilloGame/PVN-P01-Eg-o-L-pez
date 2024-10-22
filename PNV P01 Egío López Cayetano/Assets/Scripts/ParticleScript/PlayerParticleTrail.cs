using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerParticleTrail : MonoBehaviour
{
    public ParticleSystem footstepsParticles;  // El sistema de partículas
    public Transform groundCheck;              // Punto para verificar si está en el suelo
    public LayerMask groundLayer;              // Capa de suelo
    public float checkRadius = 0.2f;           // Radio para detectar suelo

    private bool isGrounded;

    void Update()
    {
        // Verificar si el personaje está en el suelo
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer);

        // Activar o desactivar partículas según si el jugador está en el suelo
        if (isGrounded && Mathf.Abs(Input.GetAxisRaw("Horizontal")) > 0)
        {
            if (!footstepsParticles.isPlaying)
            {
                footstepsParticles.Play();  // Iniciar el sistema de partículas si está en el suelo y moviéndose
            }
        }
        else
        {
            if (footstepsParticles.isPlaying)
            {
                footstepsParticles.Stop();  // Detener el sistema de partículas cuando no se esté moviendo o esté en el aire
            }
        }
    }
}