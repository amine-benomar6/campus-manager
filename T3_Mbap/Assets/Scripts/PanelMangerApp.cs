using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelMangerApp : MonoBehaviour
{
    public GameObject targetPanel;  // Référence au Panel
    public GameObject targetPanel2;
    public GameObject targetPanel3;
    public GameObject targetPanel4;

    private bool hasShownTargetPanel = false; // Flag pour savoir si targetPanel a été affiché une fois

    // Afficher le Panel
    /// <summary>
    /// Affiche le premier panneau une seule fois et les autres panneaux à chaque appel.
    /// </summary>
    public void ShowPanel()
    {
        // Affiche targetPanel uniquement lors du premier appel
        if (!hasShownTargetPanel && targetPanel != null)
        {
            targetPanel.SetActive(true);
            hasShownTargetPanel = true; // Marque targetPanel comme déjà affiché
        }

        // Affiche targetPanel2, targetPanel3 et targetPanel4 à chaque appel
        if (targetPanel2 != null)
        {
            targetPanel2.SetActive(true);
        }
        if (targetPanel3 != null)
        {
            targetPanel3.SetActive(true);
        }
        if (targetPanel4 != null)
        {
            targetPanel4.SetActive(true);
        }
    }

    /// <summary>
    /// Masque tous les panneaux gérés par ce script.
    /// </summary>
    public void HidePanel()
    {
        if (targetPanel != null)
        {
            targetPanel.SetActive(false);
        }
        if (targetPanel2 != null)
        {
            targetPanel2.SetActive(false);
        }
        if (targetPanel3 != null)
        {
            targetPanel3.SetActive(false);
        }
        if (targetPanel4 != null)
        {
            targetPanel4.SetActive(false);
        }
    }

    /// <summary>
    /// Bascule l'état des panneaux entre affiché et masqué en fonction de leur état actuel.
    /// </summary>
    public void TogglePanel()
    {
        // Vérifie si tous les panneaux sont définis
        if (targetPanel != null && targetPanel2 != null && targetPanel3 != null && targetPanel4 != null)
        {
            // Si l'un des panneaux est déjà actif, on les cache tous
            if (targetPanel.activeSelf || targetPanel2.activeSelf || targetPanel3.activeSelf || targetPanel4.activeSelf)
            {
                HidePanel();
            }
            else
            {
                // Si tous les panneaux sont inactifs, on les affiche selon les règles
                ShowPanel();
            }
        }
    }
}
