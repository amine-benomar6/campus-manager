using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TutoManager : MonoBehaviour
{
    public Text textToDisplay; // Référence au composant Text
    public Button btnAffiche; // Référence au bouton

    private string txtThree = @"Vous disposez de 4 points d'action par jour, et chaque action peut coûter un certain nombre de points. Si vous atteignez 0 points, la journée se termine.
Veuillez cliquer sur l'une des trois catégories ci-dessus.";

    private int index = 0; // Pour suivre le nombre de clics
    private bool isDisplayingText = false; // Empêche l'exécution simultanée de plusieurs coroutines
    public float letterDelay = 0.1f; // Délai entre chaque lettre affichée

    public void OnClick()
    {
        index++;

        if (index == 1)
        {
            StartCoroutine(DisplayText(textToDisplay, textToDisplay.text));
        }
        if (index == 2)
        {
            StartCoroutine(DisplayText(textToDisplay, txtThree));
        }

    }

    IEnumerator DisplayText(Text temp, string text)
    {
        if (isDisplayingText) yield break; // Empêche l'exécution si déjà en cours
        isDisplayingText = true;

        btnAffiche.GetComponent<Button>().enabled = false;
        temp.text = "";

        foreach (char letter in text) 
        {
            temp.text += letter; 
            yield return new WaitForSeconds(letterDelay);
        }

        btnAffiche.GetComponent<Button>().enabled = true; 
        isDisplayingText = false; 
    }
}
