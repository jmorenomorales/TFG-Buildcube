using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// This class allow us to modify the text foreach languages by having a string "param" that automatically will take our text and translate it to the language that we've selected.
/// </summary>
public class TranslatedText : MonoBehaviour {

    public LanguageManager lang;

    [Tooltip("The string name (Index) in the XML field.")]
    public string param;

	// Use this for initialization
	void OnEnable () {

        lang = GameObject.FindWithTag("LanguageManager").GetComponent<LanguageManager>();

        //Every time that we 
        if (lang != null && lang.langReader != null)
            UpdateText();
    }

    // Update is called once per frame
    void Update () {

        //Here we check if the language has changed, if so, update the text again
        if (lang != null && lang.langReader != null)
            if (lang.langReader.update)
                UpdateText();

	}

    /// <summary>
    /// This function will update our 2D text or 3D text depending on what we have.
    /// We change the message thanks to the "getString" function of LanguageReader.cs
    /// </summary>
    void UpdateText()
    {
        if(GetComponent<Text>() != null)
            GetComponent<Text>().text = lang.langReader.getString(param);
        else if(GetComponent<TextMesh>() != null)
            GetComponent<TextMesh>().text = lang.langReader.getString(param);

    }
}
