using System.Collections;
using System.Collections.Generic;
using System.Data;
using Mono.Data.Sqlite;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SocialPlatforms.Impl;
using System;
using System.Linq;
using Unity.VisualScripting;

public class ScenarioManager : MonoBehaviour
{
    
    private string _context;
    private int _scenarioId;
    private int _impactId;


    public Text TextDuContexte;
    public GameObject Button1;
    public GameObject Button2;
    public GameObject Button3;
    public GameObject Button4;

    public GameObject Text1;
    public GameObject Text2;
    public GameObject Text3;
    public GameObject Text4;

    private GameObject[] _lstButton;

    /// <summary>
    /// Méthode Start qui rendre invisible tous les boutons et créér une liste des boutons qui sont dans le panel
    /// </summary>
    void Start()
    {
        Button1.SetActive(false);
        Button2.SetActive(false);
        Button3.SetActive(false);
        Button4.SetActive(false);
        Text1.SetActive(false);
        Text2.SetActive(false);
        Text3.SetActive(false);
        Text4.SetActive(false);

        _lstButton =new  GameObject[] { Button1, Button2, Button3, Button4 }; 
    }

    /// <summary>
    /// Cette méthode va afficher le nombre de bouton à afficher selon le scénario lié en demandant interrogeant la base de données
    /// </summary>
    
    public void OnClick(int scenarioId)
    {
        /*              Récupération des infos dans la base  */

        this._scenarioId = scenarioId;
         this._impactId = scenarioId;
        string dbPath = Path.Combine(Application.streamingAssetsPath, "scenario_database.sqlite");
        string connectionString = $"URI=file:{dbPath.Replace("\\", "/")}";

        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            string query = $"SELECT contexte FROM Scenario where scenario_id={this._scenarioId} AND jauge={this._impactId}";
            using (var command = new SqliteCommand(query, connection))
            {
                using (IDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        this._context = reader.GetString(0);
                    }
                }
            }
            TextDuContexte.text = this._context;

            // Recherche du nombre de solution
            int nbrSolution = 0;
            query = $"SELECT count(*) FROM choix WHERE scenario={this._scenarioId} AND jauge={this._impactId}";
            using (var command = new SqliteCommand(query, connection))
            {
                using (IDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read()) // Vérifie qu'une ligne est disponible
                    {
                        nbrSolution = reader.GetInt32(0);
                    }
                    else
                    {
                        Debug.LogWarning("Aucune donnée retournée pour la requête : " + query);
                        nbrSolution = 0; // Valeur par défaut en cas de problème
                    }
                }
            }
            int i = 0;
            query = $"SELECT nom FROM choix WHERE scenario={this._scenarioId} AND jauge={this._impactId}";
            using (var command = new SqliteCommand(query, connection))
            {
                using (IDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        _lstButton[i].GetComponentInChildren<Text>().text = reader.GetString(0);
                        i++;
                    }
                }
            }
            

            switch (nbrSolution) // Valeur du nbr de solution pour un scénario
            {
                case 1:
                    Button1.gameObject.SetActive(true);
                    Text1.SetActive(true);
                    break;
                case 2:
                    Button1.gameObject.SetActive(true);
                    Button2.gameObject.SetActive(true);
                    Text1.SetActive(true);
                    Text2.SetActive(true);
                    break;
                case 3:
                    Button1.gameObject.SetActive(true);
                    Button2.gameObject.SetActive(true);
                    Button3.gameObject.SetActive(true);
                    Text1.SetActive(true);
                    Text2.SetActive(true);
                    Text3.SetActive(true);
                    break;
                case 4:
                    Button1.gameObject.SetActive(true);
                    Button2.gameObject.SetActive(true);
                    Button3.gameObject.SetActive(true);
                    Button4.gameObject.SetActive(true);
                    Text1.SetActive(true);
                    Text2.SetActive(true);
                    Text3.SetActive(true);
                    Text4.SetActive(true);
                    break;
                default:
                    Debug.LogError("Erreur lors du switchCase : valeur inattendue.");
                    break;
            }
            int nbrImpact = 0;
            string temp = "";
            List<string> list = new List<string>();

            // 1. Obtenir le nombre d'impacts pour chaque choix
            for (int k = 1; k <= nbrSolution; k++)
            {
                 query = $"SELECT count(*) FROM choix_impact WHERE choix = {k}";
                using (var command = new SqliteCommand(query, connection))
                {
                    nbrImpact = Convert.ToInt32(command.ExecuteScalar());
                  // print($"Nombre d'impacts pour le choix {k}: {nbrImpact}");
                }

                // 2. Récupérer les effets et impacts associés au choix k
                string queryImpact = $"SELECT impact, jauge FROM choix_impact WHERE choix = {k}";
                using (var command = new SqliteCommand(queryImpact, connection))
                {
                    using (IDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int effet = reader.GetInt32(0);
                            int impact = reader.GetInt32(1);

                            // Ajouter le texte obtenu dans la liste
                            temp += GetText(effet, impact);
                        }
                    }
                }
                list.Add(temp);
                temp = string.Empty;
            }

            // 3. Afficher les résultats de la liste
            for (int a = 0; a < list.Count; a++)
            {
                PutText(list[a],a+ 1);
            }
        }

    }
    
    /// <summary>
    /// Cette méthode va afficher le texte passé en parametre au boutton numero index
    /// </summary>
    /// <param name="text"> le  texte a afficher</param>
    /// <param name="index"> le numero du button compris entre 1 et 4</param>
    public void PutText(string text, int index)
    {
           switch (index)
        {
            case 1: Text1.GetComponent<Text>().text = text; break;
            case 2: Text2.GetComponent<Text>().text = text; break;
            case 3: Text3.GetComponent<Text>().text = text; break;
            case 4: Text4.GetComponent<Text>().text = text; break;
        }
    }

    /// <summary>
    /// Permet selon l'effet et le choix associé mettre en forme le dtring des impacts lié a un choix
    /// </summary>
    /// <param name="effet"> le numero de l'effet lié qu choix</param>
    /// <param name="choix"> le numero du choix </param>
    /// <returns> return le texte attendu pour montrer les impacts </returns>
    private string GetText(int effet , int choix)
    {

        string temp = effet switch
        {
            -10 => "-",
            10 => "+",
            25 => "++",
            -25 => "--",
            -1 => "- 1",
            _ => effet.ToString()
        };
        temp += choix switch
        {
            1 => " Satisfaction\n",
            2 => " Réputation\n",
            3 => " Nombre d'élèves\n",
            4 => " Argent\n",
            5 => " Point d'action\n",
            _ => " Inconnu\n" // Gestion par défaut pour les cas imprévus
        };

        return temp;
    }
}
