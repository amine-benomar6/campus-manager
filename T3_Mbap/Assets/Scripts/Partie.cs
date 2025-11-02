using Mono.Data.Sqlite;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Partie : MonoBehaviour
{
    public Button btnClosePc;
    public GameObject finChoix;
    public GameObject finJour;
    public GameObject panelNavBar;
    public GameObject panelAppContexte;
    public GameObject panelAppNavBarVer;
    public GameObject panelMail;
    public GameObject panelTel;
    public GameObject buttonMediation;
    public GameObject buttonAppel;
    public GameObject panelPC;
    public GameObject txtMediation;
    public GameObject txtAppel;
    private int nbJour = 5;
    private int jourCourant = 1;
    private Jour jour;
    private List<(int, int)> satisfaction = new();
    private List<(int, int)> reputation = new();
    private List<(int, int)> nbEleve = new();
    public int ignorer = 0;

    void Start()
    {
        updateJauge();
        generationScenario();
        GameObject jourObject = new GameObject("Jour");
        jour = jourObject.AddComponent<Jour>();
        jour.Initialize(finChoix, panelAppContexte, GetProgressBars(), GetPointText(), GetArgentText(),panelTel,buttonMediation,buttonAppel,panelPC,txtMediation,txtAppel);
        SetupButtons();
    }

    void Update()
    {
        if (jourCourant == 2)
        {
            panelMail.SetActive(true);
        }
        
        if (ignorer + jour.GetIgnorer() >= 5)
        {
            finIgnorer();
            return;
        }
        else if (!partieTerminer() && jour.jourTerminer())
        {
            afficheJourTerminer();
            ignorer += jour.GetIgnorer();
            jourCourant++;
            jour.Reset();
            updatePointAction();
        }
        else if (partieTerminer() && jour.jourTerminer())
        {
            affichageSceneFin();
        }
    }

    void updatePointAction()
    {
        string query = @"UPDATE jauge
                         SET valeur = 4
                         WHERE jauge_id = 5";
        DatabaseConnection database = DatabaseConnection.GetInstance();
        database.OpenConnection();
        database.CreateCommand(query);
        database.ExecuteNonQuery();
        database.CloseConnection();
    }

    void updateJauge()
    {
        string query;
        DatabaseConnection database = DatabaseConnection.GetInstance();
        database.OpenConnection();
        query = @"UPDATE jauge SET valeur = 50 WHERE jauge_id = 1";
        database.CreateCommand(query);
        database.ExecuteNonQuery();
        query = @"UPDATE jauge SET valeur = 50 WHERE jauge_id = 2";
        database.CreateCommand(query);
        database.ExecuteNonQuery();
        query = @"UPDATE jauge SET valeur = 1000 WHERE jauge_id = 3";
        database.CreateCommand(query);
        database.ExecuteNonQuery();
        query = @"UPDATE jauge SET valeur = 500000 WHERE jauge_id = 4";
        database.CreateCommand(query);
        database.ExecuteNonQuery();
        query = @"UPDATE jauge
                         SET valeur = 4
                         WHERE jauge_id = 5";
        database.CreateCommand(query);
        database.ExecuteNonQuery();
        database.CloseConnection();
    }

    void generationScenario()
    {
        string query = @"SELECT scenario_id, jauge
                         FROM scenario";
        DatabaseConnection database = DatabaseConnection.GetInstance();
        database.OpenConnection();
        database.CreateCommand(query);
        SqliteDataReader reader = database.GetSqliteDataReader();
        while (reader.Read())
        {
            int scenario = reader.GetInt32(0);
            int jauge = reader.GetInt32(1);
            switch (jauge)
            {
                case 1:
                    satisfaction.Add((scenario, jauge));
                    break;
                case 2:
                    reputation.Add((scenario, jauge));
                    break;
                case 3:
                    nbEleve.Add((scenario, jauge));
                    break;
                default:
                    break;
            }
        }
        database.CloseConnection();
    }

    public ProgressBar[] GetProgressBars()
    {
        return panelAppNavBarVer.GetComponentsInChildren<ProgressBar>();
    }

    public Text GetPointText()
    {
        return panelAppNavBarVer.GetComponentsInChildren<Text>(true)
                                     .FirstOrDefault(t => t.gameObject.name == "TxtPoint");
    }

    public Text GetArgentText()
    {
        return panelAppNavBarVer.transform.Find("Argt").GetComponentInChildren<Text>(true);
    }

    public void SetupButtons()
    {
        Button[] buttons = panelNavBar.GetComponentsInChildren<Button>();
        foreach (Button button in buttons)
        {
            string nom = button.name;
            Text txtButton = button.GetComponentInChildren<Text>();
            switch (nom)
            {
                case "BtnSatisf":
                    button.onClick.AddListener(() =>
                    {
                        jour.scenario(txtButton, satisfaction[0].Item1, satisfaction[0].Item2, satisfaction);
                    });
                    break;
                case "BtnRep":
                    button.onClick.AddListener(() =>
                    {
                        jour.scenario(txtButton, reputation[0].Item1, reputation[0].Item2, reputation);
                    }); break;
                case "BtnNbE":
                    button.onClick.AddListener(() =>
                    {
                        jour.scenario(txtButton, nbEleve[0].Item1, nbEleve[0].Item2, nbEleve);
                    }); break;
                default:
                    break;
            }
        }
    }

    IEnumerator ShowPanelForSeconds(float seconds)
    {
        finJour.SetActive(true); // Activer le panel
        yield return new WaitForSeconds(seconds); // Attendre le temps sp�cifi�
        finJour.SetActive(false); // D�sactiver le panel
        btnClosePc.onClick.Invoke();

    }

    public void afficheJourTerminer()
    {
        string fin = "JOUR " + jourCourant + " TERMINÉ \n";
        fin += "VOICI VOTRE BILAN DU JOUR " + "\n";
        fin += "Satisfaction : " + jour.GetSatisfaction() + "\n";
        fin += "Réputation : " + jour.GetReputation() + "\n";
        fin += "Nombre Élèves " + jour.GetNbEleves() + "\n";
        fin += "Argent " + jour.GetArgent() + "\n";
        finJour.GetComponentInChildren<Text>().text = fin;
        StartCoroutine(ShowPanelForSeconds(3));
    }

    public bool partieTerminer()
    {
        return jourCourant == nbJour || ignorer == 5;
    }
    void finIgnorer()
    {
        string query;
        DatabaseConnection database = DatabaseConnection.GetInstance();
        database.OpenConnection();
        query = @"UPDATE jauge SET valeur = 1 WHERE jauge_id = 1";
        database.CreateCommand(query);
        database.ExecuteNonQuery();
        query = @"UPDATE jauge SET valeur = 1 WHERE jauge_id = 2";
        database.CreateCommand(query);
        database.ExecuteNonQuery();
        query = @"UPDATE jauge SET valeur = 1 WHERE jauge_id = 3";
        database.CreateCommand(query);
        database.ExecuteNonQuery();
        query = @"UPDATE jauge SET valeur = 1 WHERE jauge_id = 4";
        database.CreateCommand(query);
        database.ExecuteNonQuery();
        database.CloseConnection();
        SceneManager.LoadScene("BadEndScene");
    }

    public void affichageSceneFin()
    {
        // Récupère les barres de progression
        ProgressBar[] bars = GetProgressBars();
        float[] valeurBars = new float[3];

        // Remplissage des valeurs des barres
        for (int i = 0; i < bars.Length; i++)
        {
            valeurBars[i] = bars[i].getFillAmount();
        }

        // Détermine et affiche la scène de fin
        AfficherFin(valeurBars);
    }


    /// <summary>
    /// Transition vers la scène de fin en fonction des jauges.
    /// </summary>
    private void AfficherFin(float[] valeurBars)
    {
        if (EstMauvaiseFin(valeurBars))
        {
            SceneManager.LoadScene("BadEndScene");
        }
        else if (EstAverageFin(valeurBars))
        {
            SceneManager.LoadScene("AverageEndScene");
        }
        else if (EstBonneFin(valeurBars))
        {
            SceneManager.LoadScene("GoodEndScene");
        }
        else if (ignorer == 5)
        {
            finIgnorer();
        }
    }

    // Méthode qui vérifie si c'est la mauvaise fin
    private bool EstMauvaiseFin(float[] valeurBars)
    {
        int count = 0;
        foreach (float valeur in valeurBars)
        {
            if (valeur < 0.36f)
            {
                count++;
            }
        }
        return count >= 2; // Si 2 ou plus sont < 0.36f
    }

    // Méthode qui vérifie si c'est la fin moyenne
    private bool EstAverageFin(float[] valeurBars)
    {
        int count = 0;
        foreach (float valeur in valeurBars)
        {
            if (valeur > 0.45f && valeur < 0.65f)
            {
                count++;
            }
        }
        return count >= 2; // Si 2 ou plus sont entre 0.45f et 0.65f
    }

    // Méthode qui vérifie si c'est la bonne fin
    private bool EstBonneFin(float[] valeurBars)
    {
        int count = 0;
        foreach (float valeur in valeurBars)
        {
            if (valeur > 0.65f)
            {
                count++;
            }
        }
        return count >= 2; // Si 2 ou plus sont > 0.65f
    }
}