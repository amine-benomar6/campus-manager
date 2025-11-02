using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    public GameObject panelBIGO;

    public void OnClick()
    {
        ClosePanel();
        SceneManager.LoadScene("ReunionScene", LoadSceneMode.Additive);
    }

    public void ClosePanel()
    {
        if (panelBIGO != null)
        {
            panelBIGO.SetActive(false);

        }
    }

    /* public void UnloadSecondaryScene()
    {
        SceneManager.UnloadSceneAsync("ReunionScene");
    } */
}