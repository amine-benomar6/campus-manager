using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmergencyScriptHandler : MonoBehaviour
{
    public Animator animator;      // Référence à l'Animator
    public float interval = 2f;    // Intervalle de changement de sprite

    private float timer = 0f;

    void Update()
    {
        // Incrémente le timer
        timer += Time.deltaTime;

        // Si l'intervalle est écoulé
        if (timer >= interval)
        {
            // Réinitialise le timer
            timer = 0f;

            // Déclenche le changement d'animation (le Trigger dans l'Animator)
            animator.SetBool("IsRight", true);
        }
    }
}
