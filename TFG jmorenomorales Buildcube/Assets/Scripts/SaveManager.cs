using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveManager : MonoBehaviour
{
    public GameObject saveMenu, confirmSaveMenu, confirmDeleteMenu, confirmLoadMenu, settingsMenu, confirmMenuRefresh, colorPanel, savePrefab;
    public InputField buildNameInput;
    public Transform saveList;
    public Dictionary<string, int> saves;
    public Button saveButton;

    private int saveCounter = 0;
    private bool isSaving, isEnglish;
    
    public Sprite saveDes;

    void Start()
    {
        RefreshSaves();
        SetLanguage(PlayerPrefs.GetString("LANGUAGE"));
    }

    private void RefreshSaves()
    {
        saves =  new Dictionary<string, int>();
        saveCounter = 0;
        while (PlayerPrefs.HasKey(saveCounter.ToString()))
        {
            string name = PlayerPrefs.GetString(saveCounter.ToString());
            saves.Add(name.Split('%')[0], saveCounter);
            saveCounter++;
        }
    }

    private void SetLanguage(string language)
    {
        switch (language)
        {
            case "ESP":
                // Save Menu

                Text [] textosEspSM = saveMenu.GetComponentsInChildren<Text>();
                textosEspSM[0].text = "GUARDAR Y CARGAR";
                textosEspSM[textosEspSM.Length - 6].text = "NOMBRE: ";
                textosEspSM[textosEspSM.Length - 5].text = "INTRODUCE TEXTO...";
                textosEspSM[textosEspSM.Length - 4].text = "GUARDAR";
                textosEspSM[textosEspSM.Length - 3].text = "CARGAR";
                textosEspSM[textosEspSM.Length - 2].text = "BORRAR";
                textosEspSM[textosEspSM.Length - 1].text = "ATRÁS";

                // Confirm Save Menu

                confirmSaveMenu.GetComponentsInChildren<Text>()[0].text = "CONFIRMACIÓN";
                confirmSaveMenu.GetComponentsInChildren<Text>()[1].text = "¿SEGURO QUE QUIERES GUARDAR ESTA CONSTRUCCIÓN?";
                confirmSaveMenu.GetComponentsInChildren<Text>()[2].text = "SÍ";
                confirmSaveMenu.GetComponentsInChildren<Text>()[3].text = "CANCELAR";

                // Confirm Delete Menu

                confirmDeleteMenu.GetComponentsInChildren<Text>()[0].text = "CONFIRMACIÓN";
                confirmDeleteMenu.GetComponentsInChildren<Text>()[1].text = "¿SEGURO QUE QUIERES BORRAR ESTA CONSTRUCCIÓN?";
                confirmDeleteMenu.GetComponentsInChildren<Text>()[2].text = "SÍ";
                confirmDeleteMenu.GetComponentsInChildren<Text>()[3].text = "CANCELAR";

                // Confirm Load Menu

                confirmLoadMenu.GetComponentsInChildren<Text>()[0].text = "CONFIRMACIÓN";
                confirmLoadMenu.GetComponentsInChildren<Text>()[1].text = "¿SEGURO QUE QUIERES CARGAR ESTA CONSTRUCCIÓN?";
                confirmLoadMenu.GetComponentsInChildren<Text>()[2].text = "SÍ";
                confirmLoadMenu.GetComponentsInChildren<Text>()[3].text = "CANCELAR";

                // Settings Menu
                
                settingsMenu.GetComponentsInChildren<Text>()[0].text = "AJUSTES";
                settingsMenu.GetComponentsInChildren<Text>()[1].text = "IDIOMA";
                settingsMenu.GetComponentsInChildren<Text>()[2].text = "VER LA CUADRÍCULA";
                settingsMenu.GetComponentsInChildren<Text>()[3].text = "DESARROLLADOR";
                settingsMenu.GetComponentsInChildren<Text>()[5].text = "CONTINUAR";
                settingsMenu.GetComponentsInChildren<Text>()[6].text = "MENÚ PRINCIPAL";

                // Confirm Refresh Menu

                confirmMenuRefresh.GetComponentsInChildren<Text>()[0].text = "CONFIRMACIÓN";
                confirmMenuRefresh.GetComponentsInChildren<Text>()[1].text = "¿SEGURO QUE QUIERES REINICIAR LA CONSTRUCCIÓN?";
                confirmMenuRefresh.GetComponentsInChildren<Text>()[2].text = "SÍ";
                confirmMenuRefresh.GetComponentsInChildren<Text>()[3].text = "CANCELAR";

                break;
            case "ENG":
                // Save Menu

                Text[] textosEngSM = saveMenu.GetComponentsInChildren<Text>();
                textosEngSM[0].text = "SAVE AND LOAD";
                textosEngSM[textosEngSM.Length - 6].text = "NAME: ";
                textosEngSM[textosEngSM.Length - 5].text = "ENTER TEXT...";
                textosEngSM[textosEngSM.Length - 4].text = "SAVE";
                textosEngSM[textosEngSM.Length - 3].text = "LOAD";
                textosEngSM[textosEngSM.Length - 2].text = "DELETE";
                textosEngSM[textosEngSM.Length - 1].text = "BACK";

                // Confirm Save Menu

                confirmSaveMenu.GetComponentsInChildren<Text>()[0].text = "CONFIRMATION";
                confirmSaveMenu.GetComponentsInChildren<Text>()[1].text = "ARE YOU SURE YOU WANT TO SAVE THIS BUILD?";
                confirmSaveMenu.GetComponentsInChildren<Text>()[2].text = "YES";
                confirmSaveMenu.GetComponentsInChildren<Text>()[3].text = "CANCEL";

                // Confirm Delete Menu

                confirmDeleteMenu.GetComponentsInChildren<Text>()[0].text = "CONFIRMATION";
                confirmDeleteMenu.GetComponentsInChildren<Text>()[1].text = "ARE YOU SURE YOU WANT TO DELETE THIS BUILD?";
                confirmDeleteMenu.GetComponentsInChildren<Text>()[2].text = "YES";
                confirmDeleteMenu.GetComponentsInChildren<Text>()[3].text = "CANCEL";

                // Confirm Load Menu

                confirmLoadMenu.GetComponentsInChildren<Text>()[0].text = "CONFIRMATION";
                confirmLoadMenu.GetComponentsInChildren<Text>()[1].text = "ARE YOU SURE YOU WANT TO LOAD THIS BUILD?";
                confirmLoadMenu.GetComponentsInChildren<Text>()[2].text = "YES";
                confirmLoadMenu.GetComponentsInChildren<Text>()[3].text = "CANCEL";

                // Settings Menu

                settingsMenu.GetComponentsInChildren<Text>()[0].text = "SETTINGS";
                settingsMenu.GetComponentsInChildren<Text>()[1].text = "LANGUAGE";
                settingsMenu.GetComponentsInChildren<Text>()[2].text = "SEE THE GRID";
                settingsMenu.GetComponentsInChildren<Text>()[3].text = "DEVELOPER";
                settingsMenu.GetComponentsInChildren<Text>()[5].text = "RESUME";
                settingsMenu.GetComponentsInChildren<Text>()[6].text = "MAIN MENU";

                // Confirm Refresh Menu

                confirmMenuRefresh.GetComponentsInChildren<Text>()[0].text = "CONFIRMATION";
                confirmMenuRefresh.GetComponentsInChildren<Text>()[1].text = "ARE YOU SURE YOU WANT TO REFRESH THIS BUILD?";
                confirmMenuRefresh.GetComponentsInChildren<Text>()[2].text = "YES";
                confirmMenuRefresh.GetComponentsInChildren<Text>()[3].text = "CANCEL";

                break;
            default:
                break;
        }
    }

    public void OnSaveMenuClick()
    {
        if (GameObject.Find("EventSystem").GetComponent<TouchScript.Layers.UI.TouchScriptInputModule>())
        {
            GameObject.Find("EventSystem").GetComponent<TouchScript.Layers.UI.TouchScriptInputModule>().enabled = false;
        }
        // Desactivamos el panel de color
        if (colorPanel.activeSelf)
            colorPanel.SetActive(false);
        
        saveMenu.GetComponent<Animator>().SetBool("ActiveSaveAndLoad", true);
        RefreshSaveList();
    }

    public void OnSaveClick()
    {
        confirmSaveMenu.SetActive(true);

        // Desactivamos los botones de acción
        Button [] saveMenuButtons = saveMenu.GetComponentsInChildren<Button>();
        saveMenuButtons[saveMenuButtons.Length - 1].interactable = false;
        saveMenuButtons[saveMenuButtons.Length - 2].interactable = false;
        saveMenuButtons[saveMenuButtons.Length - 3].interactable = false;
        saveMenuButtons[saveMenuButtons.Length - 4].interactable = false;
    }


    public void OnConfirmOKorCancel(string actionType)
    {
        switch (actionType)
        {
            case "SAVE":
                Save();
                confirmSaveMenu.SetActive(false);
                saveMenu.GetComponent<Animator>().SetBool("ActiveSaveAndLoad", false);
                break;
            case "LOAD":
                Load();
                confirmLoadMenu.SetActive(false);
                saveMenu.GetComponent<Animator>().SetBool("ActiveSaveAndLoad", false);
                break;
            case "DELETE":
                OnDelete();
                confirmDeleteMenu.SetActive(false);
                saveMenu.GetComponent<Animator>().SetBool("ActiveSaveAndLoad", false);
                break;
            case "CANCELSAVE":
                confirmSaveMenu.SetActive(false);
                break;
            case "CANCELLOAD":
                confirmLoadMenu.SetActive(false);
                break;
            case "CANCELDELETE":
                confirmDeleteMenu.SetActive(false);
                break;
            default:
                break;
        }

        // Desactivamos los botones de acción
        Button[] saveMenuButtons = saveMenu.GetComponentsInChildren<Button>();
        saveMenuButtons[saveMenuButtons.Length - 1].interactable = true;
        saveMenuButtons[saveMenuButtons.Length - 2].interactable = true;
        saveMenuButtons[saveMenuButtons.Length - 3].interactable = true;
        saveMenuButtons[saveMenuButtons.Length - 4].interactable = true;
    }

    public void OnLoadClick()
    {
        confirmLoadMenu.SetActive(true);

        // Desactivamos los botones de acción
        Button[] saveMenuButtons = saveMenu.GetComponentsInChildren<Button>();
        saveMenuButtons[saveMenuButtons.Length - 1].interactable = false;
        saveMenuButtons[saveMenuButtons.Length - 2].interactable = false;
        saveMenuButtons[saveMenuButtons.Length - 3].interactable = false;
        saveMenuButtons[saveMenuButtons.Length - 4].interactable = false;
    }

    public void OnCancelClick()
    {
        saveMenu.GetComponent<Animator>().SetBool("ActiveSaveAndLoad", false);
        saveButton.image.sprite = saveDes;
    }


    public void OnConfirmOk()
    {
        if (isSaving)
            Save();
        else
            Load();
        
        if (confirmSaveMenu.activeSelf)
            saveMenu.GetComponent<Animator>().SetBool("ActiveSaveAndLoad", false);
        if(confirmSaveMenu.activeSelf)
            confirmSaveMenu.SetActive(false);
        if (confirmMenuRefresh.activeSelf)
            confirmMenuRefresh.SetActive(false);
        if (confirmLoadMenu.activeSelf)
            confirmLoadMenu.SetActive(false);
        if (confirmDeleteMenu.activeSelf)
            confirmDeleteMenu.SetActive(false);

        saveButton.image.sprite = saveDes;
    }

    public void OnConfirmCancel()
    {
        if (confirmSaveMenu.activeSelf)
            saveMenu.GetComponent<Animator>().SetBool("ActiveSaveAndLoad", false);
        if (confirmMenuRefresh.activeSelf)
            confirmMenuRefresh.SetActive(false);
        if (confirmLoadMenu.activeSelf)
            confirmLoadMenu.SetActive(false);
        if (confirmDeleteMenu.activeSelf)
            confirmDeleteMenu.SetActive(false);

        saveButton.image.sprite = saveDes;
    }

    public void OnDelete()
    {
        string buildName;
        buildName = buildNameInput.text;
        int k;
        saves.TryGetValue(buildName, out k);

        if (!saves.ContainsValue(k))
        {
            Debug.Log("Unable to delete build");
            return;
        }

        PlayerPrefs.DeleteKey(k.ToString());
        saveCounter--;
        while(PlayerPrefs.HasKey((k+1).ToString()))
        {
            string data = PlayerPrefs.GetString((k+1).ToString());
            PlayerPrefs.SetString(k.ToString(), data);
            PlayerPrefs.DeleteKey((k+1).ToString());
            k++;
        }

        RefreshSaves();
        //GameManager.Instance.ResetGrid();

        saveMenu.GetComponent<Animator>().SetBool("ActiveSaveAndLoad", true);
        saveButton.image.sprite = saveDes;
    }

    private void Save()
    {
        string buildName;
        buildName = buildNameInput.text;
        Debug.Log("BuildName " + buildName);
        bool isUsed = (saves.ContainsKey(buildName));

        if (string.IsNullOrEmpty(buildName))
        {
            buildName = saveCounter.ToString();
        }
        string saveData = buildName + '%';

        Block[,,] b = GameManager.Instance.blocks;

        for (int i = 0; i < GameManager.Instance.blocks.GetLength(0); i++)
        {
            for (int j = 0; j < GameManager.Instance.blocks.GetLength(1); j++)
            {
                for (int k = 0; k < GameManager.Instance.blocks.GetLength(2); k++)
                {
                    Block currentBlock = b[i, j, k];
                    if (currentBlock == null)
                        continue;

                    saveData += i.ToString() + "|" +
                                j.ToString() + "|" +
                                k.ToString() + "|" +
                                ((int)currentBlock.color).ToString() + "%";
                }
            }
        }

        saveData += GameManager.Instance.blocks.GetLength(0) + "%";
        Debug.Log(saveData);

        if (isUsed)
        {
            // Overwrite
            int k;
            saves.TryGetValue(buildName, out k);

            PlayerPrefs.SetString(k.ToString(), saveData);
        }
        else
        {
            string name = PlayerPrefs.GetString(saveCounter.ToString());
            saves.Add(buildName, saveCounter);
            PlayerPrefs.SetString(saveCounter.ToString(), saveData);
            saveCounter++;
        }
    }

    private void Load()
    {
        string buildName;
        buildName = buildNameInput.text;
        int k;
        saves.TryGetValue(buildName, out k);

        if (!saves.ContainsValue(k))
        {
            Debug.Log("Unable to find build");
            return;
        }

        string save = PlayerPrefs.GetString(k.ToString());
        string[] blockData = save.Split('%');

        GameManager.Instance.ResetGrid();
        
        Debug.Log(blockData[blockData.Length - 2]);
        
        switch(blockData[blockData.Length-2])
        {
            case "5":
                GameManager.Instance.GridSettings(0);
                break;
            case "10":
                GameManager.Instance.GridSettings(1);
                break;
            case "20":
                GameManager.Instance.GridSettings(2);
                break;
        }

        for (int i = 1; i < blockData.Length - 2; i++)
        {
            string[] currentBlock = blockData[i].Split('|');
            int x = int.Parse(currentBlock[0]);
            int y = int.Parse(currentBlock[1]);
            int z = int.Parse(currentBlock[2]);

            int c = int.Parse(currentBlock[3]);

            Block b = new Block() { color = (BlockColor)c };

            GameManager.Instance.CreateBlock(x, y, z, b);
        }
    }

    private void RefreshSaveList()
    {
        foreach (Transform t in saveList)
        {
            Destroy(t.gameObject);
        }

        for (int i = 0; i < saveCounter; i++)
        {
            GameObject go = Instantiate(savePrefab) as GameObject;
            go.transform.SetParent(saveList);

            string[] saveData = PlayerPrefs.GetString(i.ToString()).Split('%');

            go.GetComponentInChildren<Text>().text = saveData[0];

            string s = saveData[0];
            go.GetComponent<Button>().onClick.AddListener(() => OnSaveClick(s));
        }
    }

    public void OnRefreshClick()
    {
        if (GameObject.Find("EventSystem").GetComponent<TouchScript.Layers.UI.TouchScriptInputModule>())
        {
            GameObject.Find("EventSystem").GetComponent<TouchScript.Layers.UI.TouchScriptInputModule>().enabled = false;
        }

        confirmMenuRefresh.SetActive(true);
    }

    public void OnRefreshOk()
    {
        confirmMenuRefresh.SetActive(false);

        GameManager.Instance.ResetGrid();
    }

    public void OnRefreshCancel()
    {
        confirmMenuRefresh.SetActive(false);
    }

    private void OnSaveClick(string name)
    {
        buildNameInput.text = name;
    }

    public void OnDeleteClick()
    {
        confirmDeleteMenu.SetActive(true);

        // Desactivamos los botones de acción
        Button[] saveMenuButtons = saveMenu.GetComponentsInChildren<Button>();
        saveMenuButtons[saveMenuButtons.Length - 1].interactable = false;
        saveMenuButtons[saveMenuButtons.Length - 2].interactable = false;
        saveMenuButtons[saveMenuButtons.Length - 3].interactable = false;
        saveMenuButtons[saveMenuButtons.Length - 4].interactable = false;
    }
    

    public void OnSpanishFlagClick()
    {
        PlayerPrefs.SetString("LANGUAGE", "ESP");
        SetLanguage(PlayerPrefs.GetString("LANGUAGE"));
    }

    public void OnEnglishFlagClick()
    {
        PlayerPrefs.SetString("LANGUAGE", "ENG");
        SetLanguage(PlayerPrefs.GetString("LANGUAGE"));
    }

    public void OnSettingsClick()
    {
        if (GameObject.Find("EventSystem").GetComponent<TouchScript.Layers.UI.TouchScriptInputModule>())
        {
            GameObject.Find("EventSystem").GetComponent<TouchScript.Layers.UI.TouchScriptInputModule>().enabled = false;
        }

        settingsMenu.SetActive(true);
        if (confirmSaveMenu.activeSelf)
        {
            confirmSaveMenu.SetActive(false);
            saveButton.image.sprite = saveDes;
        }
        if (confirmMenuRefresh.activeSelf)
        {
            confirmMenuRefresh.SetActive(false);
            saveButton.image.sprite = saveDes;
        }
        if (confirmLoadMenu.activeSelf)
        {
            confirmLoadMenu.SetActive(false);
            saveButton.image.sprite = saveDes;
        }
        if (confirmDeleteMenu.activeSelf)
        {
            confirmDeleteMenu.SetActive(false);
            saveButton.image.sprite = saveDes;
        }
        if (saveMenu.activeSelf)
        {
            saveMenu.GetComponent<Animator>().SetBool("ActiveSaveAndLoad", false);
            saveButton.image.sprite = saveDes;
        }
    }

    public void OnResumeClick()
    {
        settingsMenu.SetActive(false);
    }

    public void OnMainMenuClick()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Menu");
    }
}
