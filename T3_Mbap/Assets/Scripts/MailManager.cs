using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using Mono.Data.Sqlite;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MailManager : MonoBehaviour
{
    public GameObject popUp;
    public GameObject mail;
    public GameObject panelVertical;
    public GameObject panelMenu;
    private bool isAccepted =false;

    private ProgressBar progressBarSatisfaction;
    private ProgressBar progressBarReputation;
    private ProgressBar progressBarNbEleve;
    // Start is called before the first frame update
    /// <summary>
    /// Initialise les références des jauges de progression au démarrage.
    /// </summary>
    void Start()
    {
        ProgressBar[] progressBar = panelVertical.GetComponentsInChildren<ProgressBar>();
        this.progressBarReputation = progressBar[0];
        this.progressBarSatisfaction = progressBar[1];
        this.progressBarNbEleve = progressBar[2];
    }

    /// <summary>
    /// Permet d'afficher l'interface des mails et de masquer les autres panneaux.
    /// </summary>
    public void OnClickMail()
    {
        panelMenu.SetActive(false);
        panelVertical.SetActive(false);
        mail.SetActive(true);
    }

    /// <summary>
    /// Ferme la fenêtre contextuelle sans valider l'action.
    /// </summary>
    public void CroixQuit()
    {
        isAccepted = false;
        popUp.SetActive(false);
        Final();
    }

    /// <summary>
    /// Valide l'action dans la fenêtre contextuelle.
    /// </summary>
    public void Confirm()
    {
        isAccepted = true;
        popUp.SetActive(false);
        Final();
    }

    /// <summary>
    /// Termine le processus après une action acceptée ou refusée.
    /// </summary>
    public void Final()
    {
        if (isAccepted)
        {
            bonusJauge();
        }
        Cancel();
    }

    /// <summary>
    /// Met à jour les jauges de progression à partir des données de la base SQLite.
    /// </summary>
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
                case 2: // Réputation
                    progressBarReputation.setFillFidelite(progress * 100);
                    break;
                case 3: // Nombre d'élèves
                    progressBarNbEleve.setFillFidelite(progress * 100);
                    break;
                case 4: // Argent
                    GetText().text = valeur.ToString();
                    break;
            }
        }
    }

    /// <summary>
    /// Applique des bonus ou malus aux jauges dans la base SQLite et met à jour les jauges affichées.
    /// </summary>
    void bonusJauge()
    {
        string query;
        DatabaseConnection database = DatabaseConnection.GetInstance();
        database.OpenConnection();
        query = @"UPDATE jauge SET valeur = valeur - 20 WHERE jauge_id = 1";
        database.CreateCommand(query);
        database.ExecuteNonQuery();
        query = @"UPDATE jauge SET valeur = valeur - 20 WHERE jauge_id = 2";
        database.CreateCommand(query);
        database.ExecuteNonQuery();
        query = @"UPDATE jauge SET valeur = valeur - 100 WHERE jauge_id = 3";
        database.CreateCommand(query);
        database.ExecuteNonQuery();
        query = @"UPDATE jauge SET valeur = 500000 WHERE jauge_id = 4";
        database.CreateCommand(query);
        database.ExecuteNonQuery();
        database.CloseConnection();
        miseAJourDesBarre();
    }

    /// <summary>
    /// Récupère le texte affichant la valeur d'argent.
    /// </summary>
    /// <returns>Composant Text associé à l'argent.</returns>
    public Text GetText()
    {
        return panelVertical.transform.Find("Argt").GetComponentInChildren<Text>();
    }

    /// <summary>
    /// Réinitialise l'état des panneaux après avoir quitté ou validé un mail.
    /// </summary>
    public void Cancel()
    {
        popUp.SetActive(false);
        mail.SetActive(false);
        panelMenu.SetActive(true);
        panelVertical.SetActive(true);
    }
}
