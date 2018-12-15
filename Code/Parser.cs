using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class Parser : MonoBehaviour {

    /*
     * Charge toutes les phrases d'un fichier text 
     * En créer une liste de string
     * qu'on va interpreter pour determiner ce que le jeu va faire
     * (afficher un texte, modifier une image, demander un choix au joueur, etc...)
     * 
     * TODO::
     *  Faire un systeme de choix
     *  Changer le tableau de string en Queue de string
     *  Optimiser le chargement des lignes de texte (notamment la memoire prise par le programme)
     *  Créer une structure de commande pour en ajouter ou modifier facilement (aka les <end> / <choix> / etc) sans avoir a tout hardcoder
     *
     */
     
    public DialogManager dm;    // reference au script qui va afficher les phrases a l'ecran

    private string[] dialog;
    private int i;

    private void Start()
    {
        LoadFile("test.txt");
        i = 0;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            i = ParseLine(i);
        }
    }

    #region Functions
    private int ParseLine(int index)
    {
        if (index < dialog.Length -1)
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

            if (CustomStartsWith(dialog[index], "<choix>") || CustomStartsWith(dialog[index], "\r\n<choix>"))
            {
                index++;
                while (!CustomStartsWith(dialog[index], "<finchoix>"))
                {
                    //Faire apparaitre une boite de choix
                    //public void PopChoice(string txt, string(|| int) key);
                    // But : créer un bouton, qui appelera une fonction pour modifier l'index en fonction du retour
                    DebugLine(index++);
                }
                index++;
            }
            else
            {
                dm.DisplaySentence(dialog[index++]);
            }
        }
        return index;
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
    #endregion

}
