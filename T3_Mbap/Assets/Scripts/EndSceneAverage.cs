using Mono.Data.Sqlite;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class EndSceneAverage : MonoBehaviour
{
    private ProgressBar progressBarSatisfaction;
    private ProgressBar progressBarReputation;
    private ProgressBar progressBarNbEleve;
    public GameObject panelRecap;
    // Start is called before the first frame update
    void Start()
    {
        ProgressBar[] progressBar = panelRecap.GetComponentsInChildren<ProgressBar>();
        progressBarSatisfaction = progressBar[0];
        progressBarReputation = progressBar[1];
        progressBarNbEleve = progressBar[2];
        miseAJourDesBarre();
    }

    void miseAJourDesBarre()
    {
        string query = @"SELECT jauge_id, valeur, maximum FROM jauge";
        DatabaseConnection connection = DatabaseConnection.GetInstance();
        connection.OpenConnection();
        connection.CreateCommand(query);
        SqliteDataReader reader = connection.GetSqliteDataReader();
        while (reader.Read())
        {
            int jauge = reader.GetInt32(0);
            int valeur = reader.GetInt32(1);
            int maxValeur = reader.GetInt32(2);
            float progress = (float)valeur / maxValeur;

            switch (jauge)
            {
                case 1: // Satisfaction
                    progressBarSatisfaction.setFillFidelite(progress * 100);
                    break;
                case 2: // Reputation
                    progressBarReputation.setFillFidelite(progress * 100);
                    break;
                case 3: // Nombre d'élèves
                    progressBarNbEleve.setFillFidelite(progress * 100);
                    break;
                case 4:
                    GetText().text = valeur.ToString();
                    break;
            }
        }
    }

    public Text GetText()
    {
        return panelRecap.transform.Find("Argt").GetComponentInChildren<Text>();
    }

    public void QuitGame()
    {
        // Vérifie si l'application tourne dans l'éditeur ou en build
        #if UNITY_EDITOR
        // Arrête le jeu uniquement dans l'éditeur
        UnityEditor.EditorApplication.isPlaying = false;
        #else
            // Quitte le jeu dans une version buildée
            Application.Quit();
        #endif
    }
}
