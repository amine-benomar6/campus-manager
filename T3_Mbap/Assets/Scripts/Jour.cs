using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;
using Mono.Data.Sqlite;
using System.Data.Common;
using System;
using System.Collections;
using System.Data;

public class Jour : MonoBehaviour
{
    public int nbEleves = 0;
    public int satisfaction = 0;
    public int reputation = 0;
    public int argent = 0;
    public int ignorer  = 0;
    public GameObject panelAppContexte;
    public GameObject finChoix;
    public GameObject panelTel;
    public GameObject buttonMediation;
    public GameObject buttonAppel;
    public GameObject panelPC;
    public GameObject txtMediation;
    public GameObject txtAppel;
    public ProgressBar progressBarReputation ;
    public ProgressBar progressBarSatisfaction;
    public ProgressBar progressBarNbEleve;
    public Text txtPointAction;
    public Text txtArgent;
    public int nbPointAction;
    public Text[] txtImpact = new Text[4];
    private UnityAction[] choixListeners = new UnityAction[4];
    private List<int> typeRDV = new List<int>();
  


    public void Initialize(GameObject finChoix, GameObject panelAppAcc, ProgressBar[] progresse, Text txtPointAction, Text txtArgent, GameObject panelTel, GameObject buttonMediation, GameObject buttonAppel, GameObject panelPC,GameObject txtMediation, GameObject txtAppel)
    {
        this.txtMediation = txtMediation;
        this.txtAppel = txtAppel;
        this.panelPC = panelPC;
        this.panelTel = panelTel;
        this.buttonMediation = buttonMediation;
        this.buttonAppel = buttonAppel;
        this.finChoix = finChoix;
        this.panelAppContexte = panelAppAcc;
        this.nbPointAction = 4;
        this.progressBarReputation = progresse[0];
        this.progressBarSatisfaction = progresse[1];
        this.progressBarNbEleve = progresse[2];
        this.txtPointAction = txtPointAction;
        txtPointAction.text = "4/4";
        this.txtArgent = txtArgent;
        this.txtImpact = this.panelAppContexte.GetComponentsInChildren<Text>(true);
        this.txtImpact = txtImpact.Skip(Math.Max(0, txtImpact.Length - 4)).ToArray();

    }

    /// <summary>
    /// initialisation du scenario
    /// </summary>
    /// <param name="descJauge"></param>
    /// <param name="scenario_id"></param>
    /// <param name="jauge"></param>
    /// <param name="scenarios"></param>
    public void scenario(Text descJauge, int scenario_id, int jauge, List<(int, int)> scenarios)
    {
        MiseAJourContexte(descJauge, scenario_id, jauge);
        SqliteDataReader reader = GetNomChoixByScenario(scenario_id, jauge);
        Button[] buttons = panelAppContexte.GetComponentsInChildren<Button>(true);

        int nbrSolutions = GetNbrSolutions(scenario_id, jauge); // Nouvelle méthode pour obtenir le nombre de solutions
        ConfigurerBtnTxt(nbrSolutions, buttons);
        RécupérerImpacts(scenario_id, jauge);
        int i = 0;
        while (i < nbrSolutions && reader.Read())
        {
            int choix = reader.GetInt32(0);
            string nom = reader.GetString(1);
            buttons[i].GetComponentInChildren<Text>().text = nom;
            // Retirer l'ancien listener spécifique s'il existe
            if (choixListeners[i] != null)
            {
                buttons[i].onClick.RemoveListener(choixListeners[i]);
            }
            // Créer un nouveau listener et l'ajouter
            choixListeners[i] = () => reponseClick(nom, choix, scenarios);
            buttons[i].onClick.AddListener(choixListeners[i]);
            i++;
        }
        // Gestion du telephone
        DatabaseConnection.GetInstance().CloseConnection();
    }

    /// <summary>
    /// avoir le nbr de solutions pour un scénario
    /// </summary>
    /// <param name="scenario">id du scénario</param>
    /// <param name="jauge">id de la jauge</param>
    /// <returns></returns>
    private int GetNbrSolutions(int scenario, int jauge)
    {
        string query = $"SELECT COUNT(*) FROM choix WHERE scenario = @scenario AND jauge = @jauge";
        DatabaseConnection database = DatabaseConnection.GetInstance();
        database.OpenConnection();
        database.CreateCommand(query);
        database.AddParametre("@scenario", scenario);
        database.AddParametre("@jauge", jauge);
        int nbrSolutions = Convert.ToInt32(database.GetSqliteCommand().ExecuteScalar());
        return nbrSolutions;
    }
    /// <summary>
    /// mettre les textes dans les boutons
    /// </summary>
    /// <param name="nbrSolution"></param>
    /// <param name="buttons"></param>
    void ConfigurerBtnTxt(int nbrSolution, Button[] buttons)
    {
        switch (nbrSolution)
        {
            case 3:
                buttons[0].gameObject.SetActive(true);
                buttons[1].gameObject.SetActive(true);
                buttons[2].gameObject.SetActive(true);
                buttons[3].gameObject.SetActive(false);
                txtImpact[0].gameObject.SetActive(true);
                txtImpact[1].gameObject.SetActive(true);
                txtImpact[2].gameObject.SetActive(true);
                txtImpact[3].gameObject.SetActive(false);
                break;
            case 4:
                buttons[0].gameObject.SetActive(true);
                buttons[1].gameObject.SetActive(true);
                buttons[2].gameObject.SetActive(true);
                buttons[3].gameObject.SetActive(true);
                txtImpact[0].gameObject.SetActive(true);
                txtImpact[1].gameObject.SetActive(true);
                txtImpact[2].gameObject.SetActive(true);
                txtImpact[3].gameObject.SetActive(true);
                break;
            default:
                Debug.LogError("Erreur lors du switchCase : valeur inattendue.");
                break;
        }
    }
    /// <summary>
    /// click sur le bouton choix
    /// </summary>
    /// <param name="nom">nom du choxi</param>
    /// <param name="idChoix">id du choix</param>
    /// <param name="scenarios">LST scénarios</param>
    void reponseClick(string nom ,int idChoix, List<(int, int)> scenarios)
    {
        if (nom.ToLower().Contains("ignorer"))
        {
            ignorer++;
        }
        DatabaseConnection database = DatabaseConnection.GetInstance();
        database.OpenConnection();
        if(!testPointAction(idChoix))
        {
            MiseAJourChoixPasCliquable();
            return;
        }
        MiseAJourFinChoix(idChoix);
        scenarios.RemoveAt(0);
        string query = @"SELECT ci.jauge, ci.impact, j.valeur, j.maximum
                         FROM choix_impact ci JOIN jauge j ON ci.jauge = j.jauge_id
                         WHERE ci.choix = @choix ";

        database.CreateCommand(query);
        database.AddParametre("@choix", idChoix);
        SqliteDataReader reader = database.GetSqliteDataReader();
        while (reader.Read())
        {
            int jauge = reader.GetInt32(0);
            int impact = reader.GetInt32(1);
            int valeur = reader.GetInt32(2);
            int newValeur = impact + valeur;
            int maxValeur = reader.GetInt32(3);

            if (newValeur >= 0 && newValeur <= reader.GetInt32(3))
            {
                string query2 = @"UPDATE jauge
                                  SET valeur = @newValeur
                                  WHERE jauge_id = @jauge ";
                database.CreateCommand(query2);
                database.AddParametre("@jauge", jauge);
                database.AddParametre("@newValeur", newValeur);
                database.ExecuteNonQuery();
                StartCoroutine(UpdateProgressBar(jauge, impact, newValeur, maxValeur));
            }
        }
        query = $" Select type_rdv from choix where choix_id={idChoix}";
        database.CreateCommand(query);
        reader = database.GetSqliteDataReader();
        int type = -1;
        if (!reader.IsDBNull(0))
        {
            if (reader.Read()) type = reader.GetInt32(0);

        }
        if (type == 1)
        {
            print("test");

            panelTel.SetActive(true);
            buttonMediation.SetActive(true);
            buttonAppel.SetActive(false);
            panelPC.SetActive(false);
            txtAppel.SetActive(false);
        }
        else if (type == 0)
        {
            print("appeler");

            panelTel.SetActive(true);
            buttonAppel.SetActive(true);
            buttonMediation.SetActive(false);
            panelPC.SetActive(false);
            txtMediation.SetActive(false);
        }
        StartCoroutine(ShowPanelForSeconds(2));
    }

    /// <summary>
    /// affiche panel pour un choix impossible
    /// </summary>
    public void MiseAJourChoixPasCliquable()
    {
        finChoix.GetComponentInChildren<Text>().text = "CE CHOIX N'EST PAS JOUABLE PUISQUE VOS POINTS D'ACTION SONT NÉGATIFS";
        StartCoroutine(ShowPanelForSeconds(6));
    }

    /// <summary>
    /// vérification si le choix peut etre prit selon les points d'action
    /// </summary>
    /// <param name="idChoix"></param>
    /// <returns></returns>
    bool testPointAction(int idChoix)
    {
        DatabaseConnection database = DatabaseConnection.GetInstance();
        string queryPoint = @"SELECT j.valeur + ci.impact
                              FROM choix_impact ci JOIN jauge j ON ci.jauge = j.jauge_id
                              WHERE ci.choix = @choix and ci.jauge = 5 ";
        database.CreateCommand(queryPoint);
        database.AddParametre("@choix", idChoix);
        int newvaleurPoint = Convert.ToInt32(database.GetSqliteCommand().ExecuteScalar());
        return newvaleurPoint >= 0;
    }

    /// <summary>
    /// montrer un panel
    /// </summary>
    /// <param name="seconds">durée de l'affichage</param>
    /// <returns></returns>
    IEnumerator ShowPanelForSeconds(float seconds)
    {
        finChoix.SetActive(true); // Activer le panel
        yield return new WaitForSeconds(seconds); // Attendre le temps spécifié
        finChoix.SetActive(false); // Désactiver le panel
    }

    /// <summary>
    /// MAJ des fins de choix 
    /// </summary>
    /// <param name="idChoix"></param>
    public void MiseAJourFinChoix(int idChoix)
    {
        string message = "";

        List<string> list = new List<string>();
        DatabaseConnection database = DatabaseConnection.GetInstance();

        string query = "SELECT jauge, impact FROM choix_impact WHERE choix = @choix";
        database.CreateCommand(query);
        database.AddParametre("@choix", idChoix);
        SqliteDataReader readerImpact = database.GetSqliteDataReader();
        while (readerImpact.Read())
        {
            int impact = readerImpact.GetInt32(1);
            message += GetMessageFin(impact, readerImpact.GetInt32(0));
        }
        finChoix.GetComponentInChildren<Text>().text = message;
    }

    /// <summary>
    /// récupérer le message de fin lié à une fin 
    /// </summary>
    /// <param name="impact"></param>
    /// <param name="jauge"></param>
    /// <returns></returns>
    private string GetMessageFin(int impact, int jauge)
    {
        string temp = impact switch
        {
            -10 => "Vous avez perdu peu",
            -50 => "Vous avez perdu peu",
            -25000 => "Vous avez perdu peu",
            10 => "Vous avez gagné peu",
            50 => "Vous avez gagné peu",
            25000 => "Vous avez gagné peu",
            25 => "Vous avez gagné beaucoup",
            100 => "Vous avez gagné beaucoup",
            50000 => "Vous avez gagné beaucoup",
            -25 => "Vous avez perdu beaucoup",
            -100 => "Vous avez perdu beaucoup",
            -50000 => "Vous avez perdu beaucoup",
            -1 => "- 1",
            _ => impact.ToString()
        };

        temp += jauge switch
        {
            1 => " de satisfaction\n",
            2 => " de réputation\n",
            3 => " d'élèves\n",
            4 => " d'argents\n",
            5 => " de point d'action\n",
            _ => " Inconnu\n" // Gestion par défaut pour les cas imprévus
        };

        return temp;
    }

    /// <summary>
    /// récupérer les impacts selon un choix
    /// </summary>
    /// <param name="scenario">id du scénario</param>
    /// <param name="jauge"></param>
    public void RécupérerImpacts(int scenario, int jauge)
    {
        SqliteDataReader readerChoix = GetNomChoixByScenario(scenario, jauge);
        List<string> list = new List<string>();
        string temp = "";
        DatabaseConnection database = DatabaseConnection.GetInstance();

        while (readerChoix.Read())
        {
            // 1. Récupérer les impacts associés à chaque choix via SqliteDataReader
            string query = "SELECT jauge, impact FROM choix_impact WHERE choix = @choix";
            database.CreateCommand(query);
            database.AddParametre("@choix", readerChoix.GetInt32(0));
            SqliteDataReader readerImpact = database.GetSqliteDataReader();
            while (readerImpact.Read())
            {
                int impact = readerImpact.GetInt32(1);
                temp += GetText(impact, readerImpact.GetInt32(0));
            }
            list.Add(temp);
            temp = string.Empty;
        }
        for (int a = 0; a < list.Count; a++)
        {
            PutText(list[a], a + 1);
        }
    }

    public void PutText(string text, int index)
    {
        switch (index)
        {
            case 1: txtImpact[0].text = text; break;
            case 2: txtImpact[1].text = text; break;
            case 3: txtImpact[2].text = text; break;
            case 4: txtImpact[3].text = text; break;
        }
    }

    private string GetText(int impact, int jauge)
    {
        string temp = impact switch
        {
            -10 => "-",
            -50 => "-",
            -8000 => "-",           
            10 => "+",
            50 => "+",
            25000 => "+",
            25 => "++",
            100 => "++",
            50000 => "++",
            -25 => "--",
            -100 => "--",
            -50000 => "--",
            -1 => "- 1",
            _ => impact.ToString()
        };

        temp += jauge switch
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

    /// <summary>
    /// Méthode pour avoir les informations sur les scénarios
    /// </summary>
    /// <param name="scenario">id du scénario</param>
    /// <param name="jauge">id de la jauge courante</param>
    /// <returns></returns>
    public SqliteDataReader GetNomChoixByScenario(int scenario, int jauge)
    {
        string query = "SELECT choix_id, nom,type_rdv FROM choix WHERE scenario = @scenario AND jauge = @jauge";
        try
        {
            DatabaseConnection database = DatabaseConnection.GetInstance();
            database.OpenConnection();
            database.CreateCommand(query);
            database.AddParametre("@scenario", scenario);
            database.AddParametre("@jauge", jauge);
            return database.GetSqliteDataReader();
        }
        catch (DllNotFoundException e)
        {
            Debug.LogError("Erreur : " + e.Message);
            return null;
        }
    }

    public bool jourTerminer()
    {
        return nbPointAction == 0 || ignorer == 3;
    }

    public int GetIgnorer() { return ignorer; }

    /// <summary>
    /// Méthode qui mets a jour le contexte du scénario
    /// </summary>
    /// <param name="jaugeCurrent">la jauge courante</param>
    /// <param name="scenario">L'ID du scenario</param>
    /// <param name="jauge"> La jauge qu'on veut mettre</param>
    public void MiseAJourContexte(Text jaugeCurrent, int scenario, int jauge)
    {
        string query = @"SELECT contexte
                         FROM scenario
                         WHERE scenario_id = @scenario AND jauge = @jauge";
        DatabaseConnection database = DatabaseConnection.GetInstance();
        database.OpenConnection();
        database.CreateCommand(query);
        database.AddParametre("@scenario", scenario);
        database.AddParametre("@jauge", jauge);
        string contexte = (string)database.GetSqliteCommand().ExecuteScalar();
        Text[] texts = panelAppContexte.GetComponentsInChildren<Text>();
        texts[0].text = "Contexte " + jaugeCurrent.text;
        texts[1].text = contexte;
        database.CloseConnection();
    }

    /// <summary>
    /// Méthode pour changer la barre
    /// </summary>
    /// <param name="jauge">La jauge représente les impacts </param>
    /// <param name="impact"></param>
    /// <param name="newValeur"></param>
    /// <param name="maxValeur"></param>
    /// <returns>return un IEnumerator</returns>
    IEnumerator UpdateProgressBar(int jauge, int impact,int newValeur, int maxValeur)
    {
        float progress = (float)newValeur / maxValeur;

        // Mettre à jour la barre de progression correspondante
        switch (jauge)
        {
            case 1: // Satisfaction
                satisfaction += impact;
                progressBarSatisfaction.setFillFidelite(progress * 100);
                break;
            case 2: // Reputation
                reputation += impact;
                progressBarReputation.setFillFidelite(progress * 100);
                break;
            case 3: // Nombre d'élèves
                nbEleves += impact;
                progressBarNbEleve.setFillFidelite(progress * 100);
                break;
            case 4:
                argent += impact;
                txtArgent.text = newValeur.ToString();
                break;
            case 5:

                nbPointAction = newValeur;
                txtPointAction.text = newValeur + "/4";
                break;
            default:
                break;
        }
        yield return new WaitForSeconds(8f);

    }

    /// <summary>
    /// Méthode pour retirer les choix dans la list
    /// </summary>
    public void removeClickChoix()
    {
        Button[] buttons = panelAppContexte.GetComponentsInChildren<Button>(true);
        for (int i = 0; i < buttons.Length; i++)
        {
            if (choixListeners[i] != null)
            {
                buttons[i].onClick.RemoveListener(choixListeners[i]);
            }
        }
    }

    public int GetNbEleves() 
    {
        return nbEleves;
    }

    public int GetSatisfaction()
    {
        return satisfaction;
    }

    public int GetArgent()
    { 
        return argent; 
    }

    public int GetReputation() 
    {
        return reputation;
    }

    /// <summary>
    /// Méthode qui reset toutes les données
    /// </summary>
    public void Reset()
    {
        ignorer = 0;
        nbEleves = 0; 
        argent = 0;
        satisfaction = 0;
        reputation = 0;
        this.removeClickChoix();
        txtPointAction.text = "4/4";
        nbPointAction = 4;
    }
}