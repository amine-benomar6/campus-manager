using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class AfficheStrat : MonoBehaviour
{
    // Déclaration des objets associés à l'UI
    public GameObject strat;  // Panneau contenant le texte de stratégie
    public GameObject btnAffiche;  // Bouton qui déclenche l'affichage du texte
    public GameObject affiche;  // Panneau d'affichage du texte
    public Image img;  // Image à cacher lors de l'affichage du texte
    public Text textToDisplay;  // Référence au composant Text qui affichera le texte
    public Text textTuto;  // Référence au composant Text pour le tutoriel (pas utilisé dans ce code)

    // Texte complet à afficher, contenant plusieurs paragraphes
    private string fullText = "Bienvenue dans votre nouveau rôle de Responsable des Ressources Humaines (RH) à l'IUT !\r\nVotre mission est cruciale : gérer le personnel, prendre des décisions stratégiques et maintenir un équilibre entre satisfaction, réputation et budget.\r\n\r\nChaque décision influencera directement la vie de l'IUT. Motivez les équipes, résolvez les conflits, répondez aux attentes des élèves et gérez les ressources avec discernement.\r\n\r\nSoyez prêt à relever des défis, à faire des choix difficiles et à affronter les imprévus. Votre objectif : assurer la prospérité de l'IUT tout en préservant l'harmonie.\r\n\r\nBonne chance !";

    // Temps entre chaque lettre lors de l'affichage progressif du texte
    public float letterDelay = 0.1f;

    // Variable booléenne pour empêcher plusieurs coroutines simultanées
    private bool isDisplayingText = false;

    /// <summary>
    /// Méthode appelée au démarrage du jeu pour initialiser l'état de l'UI.
    /// Cache le panneau strat et ajoute un écouteur pour le bouton btnAffiche.
    /// </summary>
    void Start()
    {
        strat.SetActive(false);  // Cache le panneau strat au démarrage du jeu
        btnAffiche.GetComponent<Button>().onClick.AddListener(ButtonClick); // Ajout listener
    }

    /// <summary>
    /// Méthode appelée lorsque le bouton btnAffiche est cliqué.
    /// Affiche le texte de stratégie dans le panneau après avoir caché l'image.
    /// </summary>
    public void ButtonClick()
    {
        img.gameObject.SetActive(false);  // Cache l'image (probablement utilisée pour un fond ou un logo)

        // Si aucune coroutine n'est en cours d'exécution, commence l'affichage du texte
        if (!isDisplayingText)
        {
            print("Strat panel displayed."); // Affiche un message dans la console pour déboguer
            strat.SetActive(true);  // Affiche le panneau de stratégie
            affiche.SetActive(true); // Affiche le texte
            StartCoroutine(DisplayText(textToDisplay)); // Lance l'affichage progressif du texte
        }
    }

    /// <summary>
    /// Coroutine pour afficher le texte lettre par lettre avec un délai entre chaque lettre.
    /// </summary>
    /// <param name="temp">Composant Text où le texte sera affiché.</param>
    /// <returns></returns>
    IEnumerator DisplayText(Text temp)
    {
        // Si une autre coroutine est déjà en cours, on sort de la fonction
        if (isDisplayingText) yield break;
        isDisplayingText = true;  // Marque l'état comme en cours

        // Désactive le bouton pendant l'affichage du texte pour éviter plusieurs clics
        btnAffiche.GetComponent<Button>().enabled = false;
        temp.text = "";  // Réinitialise le texte affiché

        // Affiche le texte lettre par lettre
        foreach (char letter in fullText)
        {
            temp.text += letter;  // Ajoute lettre par lettre
            yield return new WaitForSeconds(letterDelay);  // Attente entre lettres
        }

        // Réactive le bouton après l'affichage du texte
        btnAffiche.GetComponent<Button>().enabled = true;
        isDisplayingText = false;  // Réinitialise l'état
    }

    /// <summary>
    /// Méthode appelée pour annuler l'affichage du texte et cacher le panneau.
    /// </summary>
    public void CancelButton()
    {
        strat.SetActive(false);  // Cache le panneau de stratégie
        affiche.SetActive(false); // Cache le texte affiché
    }
}
