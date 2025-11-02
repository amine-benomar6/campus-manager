using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelManagerPC : MonoBehaviour
{
    public GameObject targetPanel; // Référence au Panel

    // Afficher le Panel
    /// <summary>
    /// Affiche le panneau.
    /// </summary>
    public void ShowPanel()
    {
        // Vérifie si targetPanel n'est pas nul avant de l'activer
        if (targetPanel != null)
        {
            targetPanel.SetActive(true); // Active le panneau
        }
    }

    /// <summary>
    /// Masque le panneau.
    /// </summary>
    public void HidePanel()
    {
        // Vérifie si targetPanel n'est pas nul avant de le désactiver
        if (targetPanel != null)
        {
            targetPanel.SetActive(false); // Désactive le panneau
        }
    }

    /// <summary>
    /// Bascule l'état du panneau entre affiché et masqué.
    /// </summary>
    public void TogglePanel()
    {
        // Vérifie si targetPanel n'est pas nul avant de basculer son état
        if (targetPanel != null)
        {
            bool isActive = targetPanel.activeSelf; // Vérifie si le panneau est actuellement actif
            targetPanel.SetActive(!isActive); // Bascule l'état du panneau
        }
    }
}
