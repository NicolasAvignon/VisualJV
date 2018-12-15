using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DialogManager : MonoBehaviour {

    /*
     * TODO::
     * Créer une pool de bouton pour eviter de faire beaucoup de Instantiate et de Destroy (grosse opti)
     * 
     */

    public Parser parser;           // reference au parser

    // a besoin de ces deux refrences pour afficher des trucs a l'ecran
    public Text nameText;           // un text object (ui)
    public Text sentenceText;       // un text object (ui)

    // a besoin de ces deux references pour le systeme de choix
    public Button buttonPrefab;     // prefab d'un bouton (qu'on peut ameliorer)
    public Transform buttonParent;  // c'est juste un panel avec un "grid layout group"

    private List<Button> buttonList = new List<Button>();

    public void DisplaySentence(string txt)
    {
        StopAllCoroutines();
        StartCoroutine(TypeSentence(txt));
    }

    // animation d'écriture
    IEnumerator TypeSentence(string sentence)
    {
        sentenceText.text = "";
        foreach(char letter in sentence.ToCharArray())
        {
            sentenceText.text += letter;
            yield return null;
        }
    }

    public void DisplayName(string name)
    {
        nameText.text = name;
    }

    #region buttons
    public void CreateButton(string desc, string rep)
    {
        Button obj = Instantiate(buttonPrefab, buttonParent);
        obj.GetComponentInChildren<Text>().text = desc;
        obj.name = rep;
        obj.onClick.AddListener(ButtonListener);
        buttonList.Add(obj);
    }

    // OnClick() du bouton
    public void ButtonListener()
    {
        parser.ChoiceSetter(false);
        parser.SearchRep(EventSystem.current.currentSelectedGameObject.name);
        ClearButton();
    }

    public void ClearButton()
    {
        foreach(Button b in buttonList)
        {
            Destroy(b.gameObject);
        }
        buttonList.Clear();
    }
    #endregion
}
