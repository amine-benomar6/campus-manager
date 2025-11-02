using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PcClickHandler : MonoBehaviour
{
    public GameObject panelPC;
    public GameObject panelBigo;
    public GameObject panelCall;

    public GameObject panelBtnRed;
    public AudioSource audioSource;
    public Text interactionMessage; // Le texte d'instruction
    public KeyCode interactionKey = KeyCode.E; // La touche d'interaction

    private bool isPlayerInRange = false; // Pour détecter si le joueur est dans la zone

    /// <summary>
    /// Déclenché lorsque le joueur quitte la zone d'interaction.
    /// </summary>
    /// <param name="other">Collider de l'objet sortant.</param>
    private void OnTriggerExit2D(Collider2D other)
    {
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
    /// Gère les entrées utilisateur pour ouvrir le panneau PC lorsque le joueur est dans la zone d'interaction.
    /// </summary>
    private void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(interactionKey))
        {
            Debug.Log("Touche E pressée. Modification de l'état de l'image.");
            panelPC.SetActive(true); // Active le panneau PC
            interactionMessage.gameObject.SetActive(false); // Cache le message
        }
    }

    /// <summary>
    /// Ferme tous les panneaux ouverts liés au PC, BIGO et au bouton rouge.
    /// </summary>
    public void ClosePanel()
    {
        if (panelPC != null || panelBigo != null || panelBtnRed != null)
        {
            panelPC.SetActive(false);
            panelBigo.SetActive(false);
            panelBtnRed.SetActive(false);
        }
    }

    /// <summary>
    /// Active les panneaux BIGO et appel, joue un son, et gère l'affichage.
    /// </summary>
    /// <param name="obj">L'objet à désactiver lors de l'appel.</param>
    public void ClickCall(GameObject obj)
    {
        if (audioSource != null)
        {
            obj.SetActive(false);
            panelBigo.SetActive(true);
            panelCall.SetActive(true);
            audioSource.enabled = true;
            audioSource.gameObject.SetActive(true);
            audioSource.Play();
            StartCoroutine(WaitForSoundToFinish());
        }
    }

    /// <summary>
    /// Coroutine qui attend la fin de la lecture du son avant de fermer les panneaux d'appel.
    /// </summary>
    /// <returns>Coroutine pour la gestion des sons.</returns>
    private IEnumerator WaitForSoundToFinish()
    {
        yield return new WaitUntil(() => !audioSource.isPlaying);

        // Une fois le son terminé, désactive les panneaux et le son
        panelCall.SetActive(false);
        audioSource.enabled = false;
        audioSource.gameObject.SetActive(false);
        ClosePanel();
    }



    /* 
    public GameObject panelPC;
    public GameObject panelBIGO;
    public GameObject panelMainBtn;


    public void ShowPanelPC()
    {
        if (panelPC != null)
        {
            panelPC.SetActive(true); // Active le Panel
            panelMainBtn.SetActive(false);
        }
    }

    public void ShowPanelBIGO()
    {
        if (panelBIGO != null)
        {
            panelBIGO.SetActive(true); // Active le Panel
            panelMainBtn.SetActive(false);
        }
    }

    public void ClosePanel()
    {
        if (panelPC != null || panelBIGO != null)
        {
            panelPC.SetActive(false); // Active le Panel
            panelBIGO.SetActive(false);
            panelMainBtn.SetActive(true);
        }
    } */

}
