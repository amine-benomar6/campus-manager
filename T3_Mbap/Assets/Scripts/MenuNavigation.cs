using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
public class MenuNavigation : MonoBehaviour
{
    public GameObject accueilPanel;
    public GameObject section1Panel;
    //public GameObject section2Panel;
    //public GameObject section3Panel;
    public GameObject panelNavBar;
    public GameObject panelNavBarVer;
    private GameObject[] allPanels;

    public void ShowAccueil()
    {
        ShowPanel(accueilPanel);
    }

    public void ShowSection1()
    {
        ShowPanel(section1Panel);
    }

    /* public void ShowSection2()
    {
        ShowPanel(section2Panel);
    }

    public void ShowSection3()
    {
        ShowPanel(section3Panel);
    } */

    // Fonction pour afficher un panel et masquer les autres
    private void ShowPanel(GameObject panelToShow)
    {
        // Masquer tous les Panels
        foreach (var panel in allPanels)
        {
            panel.SetActive(false);
        }

        // Afficher le panel sélectionné
        panelToShow.SetActive(true);
    }

    // Start is called before the first frame update
    void Start()
    {
        //allPanels = new GameObject[] { accueilPanel, section1Panel, section2Panel, section3Panel };
        allPanels = new GameObject[] { accueilPanel, section1Panel };
        Button[] buttonsAction = section1Panel.GetComponentsInChildren<Button>(true);
        foreach (var btnAction in buttonsAction)
        {
            btnAction.onClick.AddListener(() => desactiveBtnScenarioCourant(buttonsAction));
        }
    }

    void desactiveBtnScenarioCourant(Button[] buttonsAction)
    {
        Button[] buttons = panelNavBar.GetComponentsInChildren<Button>();
        foreach (var action in buttonsAction)
        {
            action.interactable = false;
        }
    }

    public void jaugeCourante(GameObject button)
    {
        Image buttonImage = button.GetComponent<Image>();
        buttonImage.type = Image.Type.Sliced;
        buttonImage.color = new Color(1f, 0.709f, 0.0431f);
        autreJauge(button.GetComponent<Button>());
    }

    public void autreJauge(Button button)
    {
        Button[] buttons = panelNavBar.GetComponentsInChildren<Button>();
        foreach (Button button1 in buttons)
        {
            if (!button.Equals(button1))
            {
                button1.GetComponent<Image>().color = Color.black;
            }
        }
        Button[] buttonsAction = section1Panel.GetComponentsInChildren<Button>(true);
        foreach (Button btnAction in buttonsAction)
        {
            btnAction.interactable = true;
        }
    }
}