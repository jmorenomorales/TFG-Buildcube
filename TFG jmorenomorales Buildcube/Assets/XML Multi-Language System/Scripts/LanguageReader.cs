using System;
using System.Collections;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

using UnityEngine;


/// <summary>
/// This class is the core of MLS, it allow us to read the XML file (using System.Xml) and to transfer all it contains in a hashtable.
/// We can get all this datas thanks to the "getLine()" function, that reads the hashtable with the index that we give as parameter and return us 
/// the string content
/// 
/// For more information on the hashtables read: https://en.wikipedia.org/wiki/Hash_table
/// </summary>

public class LanguageReader
{

    public bool update = false; //To check if we've changed the language. This is done to update all the texts.

    Hashtable XML_Strings; //The hashtable that we create to contain the data


    ///This constructor is called when we instantiate this class in the "LanguageManager" class.
    ///It's called when we set a new language to read it thanks to the functions "SetLanguageWeb" and "SetLocalLanguage".
    ///
    ///The parameters are: the XML file is gived as WWW.text result for the Web and as the physical file for the local.
    ///The "language" is the language that we've selected
    ///The "isLocal" define what function to call, if the one to open a web file or the one to open a file stored on the computer
    
    public LanguageReader(string xml, string language, bool isLocal)
    {
        if (!isLocal)
            SetLanguageOnWeb(xml, language);
        else SetLocalLanguage(xml, language);
    }

 
     /// Read a XML stored on the web
     public void SetLanguageOnWeb(string xmlText, string language)
    {
        var xml = new XmlDocument();
        xml.Load(new StringReader(xmlText));

        XML_Strings = new Hashtable();

        var doc_element = xml.DocumentElement[language]; //The element is always the language that we've selected, that's why we must insert the tag in the
                                                         //XML file.

        if (doc_element != null) //if the tag exists
        {
            var elemEnum = doc_element.GetEnumerator();

            while (elemEnum.MoveNext()) //While we find data
                XML_Strings.Add((elemEnum.Current as XmlElement).GetAttribute("name"), (elemEnum.Current as XmlElement).InnerText); //Save it on the hashtable
            
            update = true; //at the end of the cycle, we've just updated the hashtable
        }
        else
            Debug.LogError("Language does not exists: " + language);
        
    }

    ///Read a XML stored on the computer
    public void SetLocalLanguage(string path, string language)
    {
        var xml = new XmlDocument();
        xml.Load(path);

        XML_Strings = new Hashtable();
        var element = xml.DocumentElement[language];
        if (element != null)
        {
            var elemEnum = element.GetEnumerator();

            while (elemEnum.MoveNext())
                XML_Strings.Add((elemEnum.Current as XmlElement).GetAttribute("name"), (elemEnum.Current as XmlElement).InnerText);

            update = true;
        }
        else
            Debug.LogError("The specified language does not exist: " + language);
    }

    /// Get a string from the hastable by the index gived in it.
    public string getString(string _name)
    {
        if (!XML_Strings.ContainsKey(_name))
        {
            Debug.LogError("This string is not present in the XML file where you're reading: " + _name);

            return "";
        }

        return (string)XML_Strings[_name];
    }
    
}