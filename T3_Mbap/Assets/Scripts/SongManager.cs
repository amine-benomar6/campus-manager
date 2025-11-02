using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SongManager : MonoBehaviour
{
    public AudioSource audioSource; // Référence à l'AudioSource
    public AudioClip backgroundMusic; // Le son que vous voulez jouer en boucle
    private static SongManager instance;

    void Start()
    {
        // Vérifie si un AudioSource est assigné
        if (audioSource != null && backgroundMusic != null)
        {
            // Assigner le clip audio à l'AudioSource
            audioSource.clip = backgroundMusic;

            // Jouer le son en boucle
            audioSource.loop = true;

            // Démarrer la lecture du son
            audioSource.Play();
        }
    }

    void Awake()
    {
        if (instance == null)
        {
            // Assurer que cet objet ne soit pas détruit lors du changement de scène
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject); // Si l'objet existe déjà, détruire ce nouvel objet
        }
    }
}
