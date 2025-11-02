using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class LoadingManager : MonoBehaviour
{
    public Slider loadingBar;      // Barre de progression
    public GameObject playButton; // Bouton Play
    public float loadingTime = 3f; // Dur�e du chargement en secondes

    private float progress = 0f;
    // Start is called before the first frame update
    /// <summary>
    /// Initialisation de la scène de chargement. Cache le bouton Play et démarre la coroutine de chargement.
    /// </summary>
    void Start()
    {
        playButton.SetActive(false); // Masquer le bouton Play au début
        StartCoroutine(LoadGame());
    }

    /// <summary>
    /// Coroutine simulant un processus de chargement en augmentant progressivement la barre de progression.
    /// </summary>
    /// <returns>Une instance IEnumerator pour la coroutine.</returns>
    IEnumerator LoadGame()
    {
        while (progress < 1f)
        {
            // Simule le chargement (ajustez la progression ici)
            progress += Time.deltaTime / loadingTime;
            loadingBar.value = progress; // Met à jour la barre
            yield return null;
        }

        // Une fois le chargement terminé
        loadingBar.gameObject.SetActive(false);
        playButton.SetActive(true);
    }

    /// <summary>
    /// Fonction appelée lorsque le bouton Play est cliqué. Charge la scène principale.
    /// </summary>
    public void OnClick()
    {
        SceneManager.LoadScene("MainScene");
    }
}
