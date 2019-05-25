using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// This class will allow us to have a "DropDown" totally translated.
/// For the dropdown was not possible to have a class like "TranslatedText.cs", because it works with Options and not present textes.
/// This class allow us to have a dropdown with any lenght you want
/// </summary>
public class TranslatedDropdownOption : MonoBehaviour
{
    //We check if the dropdown is updated or not
    public bool updated = false;

    public LanguageManager lang;

    //This list will contain all the Indexs of the string that we need to be in the dropdown element, THIS is the list that you have to fulfill.
    public List<string> OptionsKeys;

    //This list will contain the Translated Textes of the keys that we've inserted in the editor
    public List<string> TranslatedKeys;

    // Use this for initialization
    void Start()
    {

        lang = GameObject.FindWithTag("LanguageManager").GetComponent<LanguageManager>();


        //Like always, at the start we update the dropdown
        if (lang != null && lang.langReader != null)
                UpdateDropDown();
    }

    void Update()
    {
        //We check if we should update the dropdown
        if (lang != null && lang.langReader != null)
            if (lang.langReader.update && !updated)
                UpdateDropDown();


        //We check if we've updated the dropdown and should reset the "update" state
        if (lang != null && lang.langReader != null)
            if (updated && lang.langReader.update)
            updated = false;
    }

    /// <summary>
    /// This function will update the dropdown by adding the result of the keys that we've inserted in the "TranslatedKeys" list
    /// </summary>
    void UpdateDropDown()
    {
        TranslatedKeys.Clear(); //Firstly we remove every translated text

        //Secondly we translate everything we've inserted and put them in the TranslatedKeys list
        for (int i = 0; i < OptionsKeys.Count; i++)
        {
            TranslatedKeys.Add(lang.langReader.getString(OptionsKeys[i]));
        }

        Dropdown thisDrop = GetComponent<Dropdown>(); //then we get a reference to the dropdown element

        thisDrop.ClearOptions(); //we remove all the previous options

        thisDrop.AddOptions(TranslatedKeys); //we add the new options

        updated = true; //we set the update state to true

    }

}
