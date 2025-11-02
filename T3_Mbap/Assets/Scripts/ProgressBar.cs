using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    public Image progressBarFillFidelite;
    public Image progressColor2;

    string greenC = "#05844B";
    string yellowC = "#FFFF00";
    string redC = "#FF0000";

    Color greenColor;
    Color yellowColor;
    Color redColor;

    float fillAmount2;

    void Start()
    {
        // Convertir les couleurs hexadécimales en objets Color
        ColorUtility.TryParseHtmlString(greenC, out greenColor);
        ColorUtility.TryParseHtmlString(yellowC, out yellowColor);
        ColorUtility.TryParseHtmlString(redC, out redColor);
        fillAmount2 = progressBarFillFidelite.fillAmount;
    }

    void Update()
    {
        progressBarFillFidelite.fillAmount = fillAmount2;

        // Appliquer les couleurs en fonction de la progression
        if (fillAmount2 > 0.65f)
        {
            progressColor2.color = greenColor;
        }
        else if (fillAmount2 > 0.35f)
        {
            progressColor2.color = yellowColor;
        }
        else
        {
            progressColor2.color = redColor;
        }
    }

    public void setFillFidelite(float amount)
    {
        fillAmount2 = amount / 100;
        progressBarFillFidelite.fillAmount = fillAmount2;
    }

    public float getFillAmount() 
    {
        return fillAmount2;
    }
}