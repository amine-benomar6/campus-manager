using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ImageManager : MonoBehaviour
{
    public Image image;          // Image à faire clignoter
    public Button stopButton;    // Bouton pour arrêter le clignotement
    public float duree = 1f;     // Durée totale d'un cycle (apparition + disparition)
    private bool clignoter = false;

    void Start()
    {
        if (image == null)
            image = GetComponent<Image>();

        if (stopButton != null)
        {
            stopButton.onClick.AddListener(ArreterClignotement); // Lier le bouton pour arrêter
        }

        DemarrerClignotement();
    }

    public void DemarrerClignotement()
    {
        clignoter = true;
        StartCoroutine(FaireClignoter());
    }

    public void ArreterClignotement()
    {
        clignoter = false;
        SetImageAlpha(0); // Réinitialiser l'image à complètement visible
    }

    private IEnumerator FaireClignoter()
    {
        while (clignoter)
        {
            // Faire apparaître progressivement
            for (float t = 0; t <= duree / 2; t += Time.deltaTime)
            {
                float alpha = Mathf.Lerp(0, 1, t / (duree / 2));
                SetImageAlpha(alpha);
                yield return null;
            }

            // Faire disparaître progressivement
            for (float t = 0; t <= duree / 2; t += Time.deltaTime)
            {
                float alpha = Mathf.Lerp(1, 0, t / (duree / 2));
                SetImageAlpha(alpha);
                yield return null;
            }
        }
    }

    private void SetImageAlpha(float alpha)
    {
        if (image != null)
        {
            Color color = image.color;
            color.a = alpha;
            image.color = color;
        }
    }
}
