using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour {

    // a besoin de ces deux refrences pour afficher des trucs a l'ecran
    public Text nameText;
    public Text sentenceText;

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

}
