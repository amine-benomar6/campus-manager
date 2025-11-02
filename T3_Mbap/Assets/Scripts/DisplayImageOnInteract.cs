using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Nécessaire pour gérer le UI
using UnityEngine.SceneManagement;

public class DisplayImageOnInteract : MonoBehaviour
{
    public GameObject imageToDisplay; // L'image à afficher
    public AudioSource audioSrc;
    public GameObject btnClose;
    public Text interactionMessage; // Le texte d'instruction
    public Text txtAffiche;
    public KeyCode interactionKey = KeyCode.E; // La touche d'interaction

    private bool isPlayerInRange = false; // Pour détecter si le joueur est dans la zone
    private readonly string txt = "Si vous appuyez sur ce bouton, une réunion d'urgence se lancera et vous devez faire face a un questionnaire. Vos réponses impacteront la suite de vos aventures";
    private bool isDisplayingText = false;
    private readonly float sceneLoadDelay = 0.03f;

    public GameObject canvas;

    /// <summary>
    /// Appelé lorsque le joueur entre dans la zone d'interaction.
    /// </summary>
    /// <param name="other">Le collider de l'objet entrant dans la zone.</param>
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Vérifie si le joueur entre dans la zone
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player est entré dans la zone.");
            isPlayerInRange = true;

            // Affiche le message d'interaction
            if (interactionMessage != null)
            {
                interactionMessage.gameObject.SetActive(true);
            }
        }
    }


    /// <summary>
    /// Appelé lorsque le joueur quitte la zone d'interaction.
    /// </summary>
    /// <param name="other">Le collider de l'objet sortant de la zone.</param>
    private void OnTriggerExit2D(Collider2D other)
    {
        // Vérifie si le joueur sort de la zone
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player est sorti de la zone.");
            isPlayerInRange = false;

            // Désactive le message d'interaction
            if (interactionMessage != null)
            {
                interactionMessage.gameObject.SetActive(false);
            }
        }
    }

    /// <summary>
    /// Coroutine pour afficher progressivement un texte, lettre par lettre.
    /// </summary>
    /// <param name="temp">L'objet Text où afficher le texte.</param>
    /// <returns>Un IEnumerator pour gérer la progression.</returns>
    IEnumerator DisplayText(Text temp)
    {
        if (isDisplayingText) yield break; // Empêche l'exécution si déjà en cours
        isDisplayingText = true; // Marque l'état comme en cours

        btnClose.GetComponent<Button>().enabled = false; // Désactive le bouton pendant l'affichage
        temp.text = ""; // Réinitialise le texte affiché

        foreach (char letter in txt) // Parcourt chaque lettre du texte complet
        {
            temp.text += letter; // Ajoute lettre par lettre
            yield return new WaitForSeconds(0.03f); // Attente entre lettres
        }

        btnClose.GetComponent<Button>().enabled = true; // Réactive le bouton après affichage
        btnClose.gameObject.SetActive(true);
        isDisplayingText = false; // Réinitialise l'état
    }

    /// <summary>
    /// Met à jour l'état de l'interaction et gère l'affichage du texte ou de l'image.
    /// </summary>
    void Update()
    {

        if (isPlayerInRange && Input.GetKeyDown(interactionKey))
        {
            Debug.Log("Touche E pressée. Modification de l'état de l'image.");
            imageToDisplay.SetActive(!imageToDisplay.activeSelf); // Active/Désactive l'image
            interactionMessage.gameObject.SetActive(false);
        }

        if (!imageToDisplay.gameObject.activeSelf)
        {
            if (!isDisplayingText) // Si aucune coroutine n'est en cours
            {
                StartCoroutine(DisplayText(txtAffiche));
            }

        }
    }

    /// <summary>
    /// Appelé lors du clic sur le bouton, joue le son et déclenche le changement de scène.
    /// </summary>
    public void OnClick()
    {
        if (audioSrc != null)
        {
            // S'assurer que l'AudioSource est actif et prêt à jouer
            audioSrc.enabled = true;
            audioSrc.gameObject.SetActive(true);

            // Jouer le son
            audioSrc.Play();

            // Démarrer la coroutine pour attendre la fin du son
            StartCoroutine(WaitForSoundToFinish());
        }

    }

    /// <summary>
    /// Coroutine pour attendre la fin du son avant de changer de scène.
    /// </summary>
    /// <returns>Un IEnumerator pour gérer l'attente.</returns>
    IEnumerator WaitForSoundToFinish()
    {
        // Attendre que le son se termine
        yield return new WaitForSeconds(audioSrc.clip.length);

        // Après la fin du son, désactiver l'AudioSource
        audioSrc.enabled = false;
        audioSrc.gameObject.SetActive(false);

        // Charger la nouvelle scène après un délai supplémentaire
        yield return new WaitForSeconds(sceneLoadDelay);

        // Charger la scène 3
        SceneManager.LoadScene("Emerngency", LoadSceneMode.Additive);
        canvas.SetActive(false);
    }

}
