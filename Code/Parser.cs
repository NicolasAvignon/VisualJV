using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class Parser : MonoBehaviour {

    /*
     * Charge toutes les phrases d'un fichier text 
     * En créer une liste de string
     * qu'on va interpreter pour determiner ce que le jeu va faire
     * (afficher un texte, modifier une image, demander un choix au joueur, etc...)
     * 
     * TODO::
     *  Changer le tableau de string en Queue de string
     *  Créer une structure de commande pour en ajouter ou modifier facilement (aka les <end> / <choix> / etc) sans avoir a tout hardcoder
     *
     */

     
    public DialogManager dm;    // reference au script qui va afficher les phrases a l'ecran

    private string[] dialog;
    private int ii;

    private bool inChoice = false;

    private void Start()
    {
        LoadFile("test.txt");
        ii = 0;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ii = ParseLine(ii);
        }
    }

    #region Functions
    private int ParseLine(int index)
    {
        if (!inChoice && index < dialog.Length -1)
        {
            // modife le nom de la personne qui parle
            if(CustomStartsWith(dialog[index], "<n>"))
            {
                string sub = dialog[index++].Substring(3);
                dm.DisplayName(sub);
            }
            // saut inconditionnel
            if (CustomStartsWith(dialog[index], "<j>"))
            {
                string sub = dialog[index].Substring(3);
                index += int.Parse(sub);
                index = ParseLine(index);
                return index;
            }
            // saut selon une clef
            else if(CustomStartsWith(dialog[index], "<jc>"))
            {
                string sub = dialog[index].Substring(4);
                index = SearchKey(sub, index) + 1;

            }

            // demarre une sequence de choix
            if (CustomStartsWith(dialog[index], "<choix>") || CustomStartsWith(dialog[index], "\r\n<choix>"))
            {
                ChoiceSetter(true);
                //dm.ClearButton();
                index++;
                while (!CustomStartsWith(dialog[index], "<finchoix>"))
                {
                    //DebugLine(index++);
                    string[] c = dialog[index++].Split(new string[] { "::" }, System.StringSplitOptions.None);
                    dm.CreateButton(c[0], c[1]);
                }
            }
            // Affiche simplement la phrase
            else
            {
                dm.DisplaySentence(dialog[index++]);
            }
        }
        return index;
    }
    
    private int SearchKey(string key, int i)
    {
        while (!CustomStartsWith(dialog[++i], key))
        {
            //Debug.Log(dialog[i]);
        }
        return i;
    }

    public void SearchRep(string key)
    {
        ii = SearchKey(key, ii) +1;
        //Debug.Log(key);
    }
    
    private void DebugLine(int index)
    {
        if (index < dialog.Length)
        {
            Debug.Log(dialog[index++]);
        }
    }
    #endregion

    #region Helper
    // charge un fichier pour qu'il puisse etre parsé
    private void LoadFile(string fname)
    {
        string text = System.IO.File.ReadAllText(fname);
        dialog = text.Split(new string[] { "<end>\r\n" }, System.StringSplitOptions.None);
    }

    private bool CustomStartsWith(string a, string b)
    {
        int aLen = a.Length;
        int bLen = b.Length;
        int ap = 0; int bp = 0;

        while (ap < aLen && bp < bLen && a[ap] == b[bp])
        {
            ap++;
            bp++;
        }

        return (bp == bLen && aLen >= bLen) || (ap == aLen && bLen >= aLen);
    }

    // setter
    public void ChoiceSetter(bool b)
    {
        inChoice = b;
    }
    #endregion

}
