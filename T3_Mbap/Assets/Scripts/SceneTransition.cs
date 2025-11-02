using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    public Image fadeImage; // Image utilisée pour le fondu
    public float fadeDuration = 1f; // Durée du fondu
    public string nextSceneName; // Nom de la scène suivante
    public float delayBeforeTransition = 5f; // Temps d'attente avant la transition

    private void Start()
    {
        // Commence avec un fondu entrant
        StartCoroutine(FadeIn());

        // Démarre une transition automatique après un délai
        StartCoroutine(AutoChangeScene());
    }

    public void ChangeScene(string sceneName)
    {
        // Démarre un fondu sortant et change de scène
        StartCoroutine(FadeOut(sceneName));
    }

    IEnumerator FadeIn()
    {
        // Commence avec une image complètement opaque
        fadeImage.color = new Color(0, 0, 0, 1);

        // Réduit progressivement l'opacité
        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            float alpha = 1 - (t / fadeDuration);
            fadeImage.color = new Color(0, 0, 0, alpha);
            yield return null;
        }

        // Rend l'image totalement transparente
        fadeImage.color = new Color(0, 0, 0, 0);
    }

    IEnumerator FadeOut(string sceneName)
    {
        // Commence avec une image complètement transparente
        fadeImage.color = new Color(0, 0, 0, 0);

        // Augmente progressivement l'opacité
        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            float alpha = t / fadeDuration;
            fadeImage.color = new Color(0, 0, 0, alpha);
            yield return null;
        }

        // Charge la nouvelle scène une fois le fondu terminé
        //SceneManager.LoadScene(sceneName);

        SceneManager.UnloadSceneAsync("ReunionScene");
    }

    IEnumerator AutoChangeScene()
    {
        // Attendre un certain temps avant de changer de scène
        yield return new WaitForSeconds(delayBeforeTransition);

        // Lancer la transition avec fondu
        ChangeScene(nextSceneName);
    }
}
