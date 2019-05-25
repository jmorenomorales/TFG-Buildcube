using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using UnityEngine.UI;
using System.IO;


/// <summary>
/// This class is the manager of the system.
/// it allow us to open XMLs files and read them.
/// </summary>
public class LanguageManager : MonoBehaviour
{


    [Header("Languages")]

    //Those variables must be marked with "HideInInspector" due we want to dinamically show-hide them, this work is managed by "LanguageManagerEditor.cs".
    //Those variables contains the XML files as TextAssets. Those XMLs must be present in the "GAMEPROJECT_Data" when we build a project.

    public static LanguageManager instance; //For the singleton pattern.
    
    public LanguageReader langReader; //We must have a LanguageReader inside our class to "strip the flesh off" of the XML.

    public Message _message; //Also we need a reference to the Message.

    [HideInInspector]
    public bool opened = false; //This bool will tell to us if the XML is opened and so totally readed


    public string CurrentLanguage = "English"; //This variable will contain as a string the current language that we've selected

    //This bool that's also modificable at runtime will tell the script to look for the XML files on the web or locally.
    public bool isLocal;

    /// <summary>
    /// Singleton pattern
    /// </summary>
    private void Awake()
    {

        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            DestroyImmediate(gameObject);

    }


    ///At the start, by default we set the English language, the XML file took depends with the bool "isLocal"
    private void Start()
    {
        if(!isLocal)
        StartCoroutine(OpenWebXML("English"));
        else
        OpenLocalXML("English");

    }


    /// <summary>
    /// This function allow us to open a XML file stored on the internet, we use a WWW class that returns a WWW.text, that will contains the XML text.
    /// </summary>
    IEnumerator OpenWebXML(string Language)
    {
        //We're opening a file, so we reset all the states of this script
        opened = false; 
        WWW wwwXML = null;
        langReader = null;
        CurrentLanguage = null;

        //Switch for the "Language" (as parameter), foreach language present in the game we have a different link for each file.
        switch (Language)
        {
            case "English":
                wwwXML = new WWW("http://yourdomainname.dx.am/MLS_Languages/ENG.xml");
                break;
            case "Espanol":
                wwwXML = new WWW("http://yourdomainname.dx.am/MLS_Languages/ESP.xml");
                break;
            case "Italian":
                wwwXML = new WWW("http://yourdomainname.dx.am/MLS_Languages/ITA.xml");
                break;
            default:
                wwwXML = new WWW("http://yourdomainname.dx.am/MLS_Languages/ENG.xml");
                break;
        }
        yield return wwwXML; //we wait for the reading

        CurrentLanguage = Language;

        langReader = new LanguageReader(wwwXML.text, CurrentLanguage, false); //Instantitate a new language reader that will read and store the XML file choosed

        opened = true; //We've just opened a file

        StartCoroutine("ResetUpdateState"); //Reset the update state

    }


    /// <summary>
    /// This function allow us to open a XML file stored on the computer, in the GAMENAME_Data folder created by the unity build.
    /// </summary>
    public void OpenLocalXML(string Language)
    {
        //We're opening a file, so we reset all the states of this script
        opened = false;
        langReader = null;
        CurrentLanguage = null;

        //Switch for the "Language" (as parameter), foreach language present in the game we have a different name file, but the location of those is the same.
        //Despite from the Web opening, here we instantiate the LanguageReader instantaniely, because the file must be not loaded from the web cause we've got it on the hard-disk.
        switch (Language)
        {
            case "English":
                langReader = new LanguageReader(Path.Combine(Application.dataPath, "Lang/ENG.xml"), "English", true);
                break;
            case "Espanol":
                langReader = new LanguageReader(Path.Combine(Application.dataPath, "Lang/ESP.xml"), "Espanol", true);
                break;
            case "Italian":
                langReader = new LanguageReader(Path.Combine(Application.dataPath, "Lang/ITA.xml"), "Italian", true);
                break;
            default:
                langReader = new LanguageReader(Path.Combine(Application.dataPath, "Lang/ENG.xml"), "English", true);
                break;
        }

        CurrentLanguage = Language;

        opened = true; //The file is opened

        StartCoroutine("ResetUpdateState");

    }

    /// <summary>
    /// This function will allow us to change the language of the game.
    /// </summary>

    public void SelectLanguage(string Language, bool isLocal)
    {
        //_message.gameObject.SetActive(true);
        //_message.ChangeMessage(langReader.getString("MESSAGE_LOADING_LANGUAGE"));

        if(Language != CurrentLanguage) //If we are not selecting the same language we have right now
        if (isLocal) //if we need the files stored locally
            OpenLocalXML(Language); //we open locally
        else StartCoroutine(OpenWebXML(Language)); //otherwise we look for the file from the web


    }

    IEnumerator ResetUpdateState()
    {
        yield return new WaitForSeconds(0.01f);
        langReader.update = false;
    }

}
