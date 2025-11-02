using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PatronManager : MonoBehaviour
{

    public Text panelTxt;
    public GameObject panelBulle;

    public GameObject panelBulle2;
    public GameObject panelBulle3;
    public GameObject btnOne;
    public GameObject btnTwo;
    public GameObject btnThree;
    public Animator animator;  // Référence à l'Animator pour l'animation
    public string animationTrigger = "YourAnimationTrigger"; // Nom du trigger pour l'animation

    private bool isDisplayingText = false;
    private string txt = "Je suis un inspecteur general du Ministere de l'education. Je vais donc vous poser une seule question et repondez-moi franchement.";
    private int index = 0;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DisplayText(panelTxt));
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void ClickSuivant()
    {

        if (!isDisplayingText)
        {
            index++;
            txt = "Je prefre Audi ou BMW ?";
            StartCoroutine(DisplayText(panelTxt));

        }

    }

    public void OnClickOne()
    {
        if (animator != null)
        {
            animator.SetTrigger(animationTrigger);  // Déclenche l'animation
        }
        btnOne.SetActive(false);
        btnTwo.SetActive(false);
        panelBulle.SetActive(false);

        panelBulle2.SetActive(true);
    }
    public void OnClickTwo()
    {

        btnOne.SetActive(false);
        btnTwo.SetActive(false);

        panelBulle.SetActive(false);
        panelBulle3.SetActive(true);
    }


    IEnumerator DisplayText(Text temp)
    {

        if (isDisplayingText) yield break; // Emp�che l'ex�cution si d�j� en cours
        isDisplayingText = true; // Marque l'�tat comme en cours

        index++;
        temp.text = ""; // R�initialise le texte affich�

        foreach (char letter in txt) // Parcourt chaque lettre du texte complet
        {
            temp.text += letter; // Ajoute lettre par lettre
            yield return new WaitForSeconds(0.03f); // Attente entre lettres
        }

        if (index >= 2)
        {
            btnThree.SetActive(false);
            btnOne.SetActive(true);
            btnTwo.SetActive(true);
            panelTxt.gameObject.SetActive(false);
        }
        isDisplayingText = false; // R�initialise l'�tat
    }



}
