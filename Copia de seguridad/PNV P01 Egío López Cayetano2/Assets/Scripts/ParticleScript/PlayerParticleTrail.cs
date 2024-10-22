using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerParticleTrail : MonoBehaviour
{
    public ParticleSystem footstepsParticles;  // El sistema de part�culas
    public Transform groundCheck;              // Punto para verificar si est� en el suelo
    public LayerMask groundLayer;              // Capa de suelo
    public float checkRadius = 0.2f;           // Radio para detectar suelo

    private bool isGrounded;

    void Update()
    {
        // Verificar si el personaje est� en el suelo
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer);

        // Activar o desactivar part�culas seg�n si el jugador est� en el suelo
        if (isGrounded && Mathf.Abs(Input.GetAxisRaw("Horizontal")) > 0)
        {
            if (!footstepsParticles.isPlaying)
            {
                footstepsParticles.Play();  // Iniciar el sistema de part�culas si est� en el suelo y movi�ndose
            }
        }
        else
        {
            if (footstepsParticles.isPlaying)
            {
                footstepsParticles.Stop();  // Detener el sistema de part�culas cuando no se est� moviendo o est� en el aire
            }
        }
    }
}