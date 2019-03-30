using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveManager : MonoBehaviour
{
    public GameObject saveMenuENG, saveMenuESP;
    public GameObject confirmMenuENG, confirmMenuESP;
    public GameObject settingsMenuENG, settingsMenuESP;
    public GameObject confirmMenuRefreshENG, confirmMenuRefreshESP;
    public GameObject colorPanel;

    public InputField buildNameInput;

    public Transform saveListENG, saveListESP;
    public GameObject savePrefab;

    private int saveCounter = 0;
    private bool isSaving, isEnglish;

    public Dictionary<string, int> saves;

    public Button saveButton;
    public Sprite saveDes;

    private void Start()
    {
        RefreshSaves();
        if (PlayerPrefs.GetString("LANGUAGE") == "ENG")
            isEnglish = true;
        else
            isEnglish = false;
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

    public void OnSaveMenuClick()
    {
        if (colorPanel.activeSelf)
            colorPanel.SetActive(false);

        if(isEnglish)
        {
            if (saveMenuENG.activeSelf)
                saveMenuENG.SetActive(false);
            else
            {
                saveMenuENG.SetActive(true);
                RefreshSaveList();
            }
        }
        else
        {
            if (saveMenuESP.activeSelf)
                saveMenuESP.SetActive(false);
            else
            {
                saveMenuESP.SetActive(true);
                RefreshSaveList();
            }
        }
    }

    public void OnSaveClick()
    {
        if(isEnglish)
        {
            saveMenuENG.SetActive(false);
            confirmMenuENG.SetActive(true);
        }
            
        else
        {
            saveMenuESP.SetActive(false);
            confirmMenuESP.SetActive(true);
        }

        isSaving = true;
    }

    public void OnLoadClick()
    {
        if (isEnglish)
        {
            saveMenuENG.SetActive(false);
            confirmMenuENG.SetActive(true);
        }

        else
        {
            saveMenuESP.SetActive(false);
            confirmMenuESP.SetActive(true);
        }

        isSaving = false;
    }

    public void OnCancelClick()
    {
        if (isEnglish)
        {
            saveMenuENG.SetActive(false);
        }

        else
        {
            saveMenuESP.SetActive(false);
        }

        saveButton.image.sprite = saveDes;
    }

    public void OnConfirmOk()
    {
        if (isSaving)
            Save();
        else
            Load();

        if (isEnglish)
        {
            confirmMenuENG.SetActive(false);
        }

        else
        {
            confirmMenuESP.SetActive(false);
        }

        saveButton.image.sprite = saveDes;
    }

    public void OnConfirmCancel()
    {
        if (isEnglish)
        {
            confirmMenuENG.SetActive(false);
        }

        else
        {
            confirmMenuESP.SetActive(false);
        }

        saveButton.image.sprite = saveDes;
    }

    public void OnDelete()
    {
        string buildName = buildNameInput.text;
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

        if (isEnglish)
        {
            saveMenuENG.SetActive(false);
        }

        else
        {
            saveMenuESP.SetActive(false);
        }

        saveButton.image.sprite = saveDes;
    }

    private void Save()
    {
        string buildName = buildNameInput.text;
        bool isUsed = (saves.ContainsKey(buildName));

        if (string.IsNullOrEmpty(buildName))
        {
            buildName = saveCounter.ToString();
        }
        string saveData = buildName + '%';

        Block[,,] b = GameManager.Instance.blocks;

        for (int i = 0; i < 20; i++)
        {
            for (int j = 0; j < 100; j++)
            {
                for (int k = 0; k < 20; k++)
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

        if(isUsed)
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
        string buildName = buildNameInput.text;
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

        for (int i = 1; i < blockData.Length - 1; i++)
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
        if(isEnglish)
        {
            foreach (Transform t in saveListENG)
            {
                Destroy(t.gameObject);
            }

            for (int i = 0; i < saveCounter; i++)
            {
                GameObject go = Instantiate(savePrefab) as GameObject;
                go.transform.SetParent(saveListENG);

                string[] saveData = PlayerPrefs.GetString(i.ToString()).Split('%');

                go.GetComponentInChildren<Text>().text = saveData[0];

                string s = saveData[0];
                go.GetComponent<Button>().onClick.AddListener(() => OnSaveClick(s));
            }
        }
        else
        {
            foreach (Transform t in saveListESP)
            {
                Destroy(t.gameObject);
            }

            for (int i = 0; i < saveCounter; i++)
            {
                GameObject go = Instantiate(savePrefab) as GameObject;
                go.transform.SetParent(saveListESP);

                string[] saveData = PlayerPrefs.GetString(i.ToString()).Split('%');

                go.GetComponentInChildren<Text>().text = saveData[0];

                string s = saveData[0];
                go.GetComponent<Button>().onClick.AddListener(() => OnSaveClick(s));
            }
        }
    }

    public void OnRefreshClick()
    {
        if(isEnglish)
        {
            confirmMenuRefreshENG.SetActive(true);
        }
        else
        {
            confirmMenuRefreshESP.SetActive(true);
        }
    }

    public void OnRefreshOk()
    {
        if (isEnglish)
        {
            confirmMenuRefreshENG.SetActive(false);
        }
        else
        {
            confirmMenuRefreshESP.SetActive(false);
        }

        GameManager.Instance.ResetGrid();
    }

    public void OnRefreshCancel()
    {
        if (isEnglish)
        {
            confirmMenuRefreshENG.SetActive(false);
        }
        else
        {
            confirmMenuRefreshESP.SetActive(false);
        }
    }

    private void OnSaveClick(string name)
    {
        buildNameInput.text = name;
    }

    public void OnSpanishFlagClick()
    {
        isEnglish = false;
        settingsMenuESP.SetActive(true);
        settingsMenuENG.SetActive(false);
        PlayerPrefs.SetString("LANGUAGE", "ESP");
    }

    public void OnEnglishFlagClick()
    {
        isEnglish = true;
        settingsMenuENG.SetActive(true);
        settingsMenuESP.SetActive(false);
        PlayerPrefs.SetString("LANGUAGE", "ENG");
    }

    public void OnSettingsClick()
    {
        if (PlayerPrefs.GetString("LANGUAGE") == "ENG")
        {
            settingsMenuENG.SetActive(true);
            if (saveMenuENG.activeSelf)
            {
                saveMenuENG.SetActive(false);
                saveButton.image.sprite = saveDes;
            }
            if (confirmMenuENG.activeSelf)
            {
                confirmMenuENG.SetActive(false);
                saveButton.image.sprite = saveDes;
            }
            if (confirmMenuRefreshENG.activeSelf)
            {
                confirmMenuRefreshENG.SetActive(false);
                saveButton.image.sprite = saveDes;
            }
        }
        else
        {
            settingsMenuESP.SetActive(true);
            if (saveMenuESP.activeSelf)
            {
                saveMenuESP.SetActive(false);
                saveButton.image.sprite = saveDes;
            }
            if (confirmMenuESP.activeSelf)
            {
                confirmMenuESP.SetActive(false);
                saveButton.image.sprite = saveDes;
            }
            if (confirmMenuRefreshESP.activeSelf)
            {
                confirmMenuRefreshESP.SetActive(false);
                saveButton.image.sprite = saveDes;
            }
        }
    }

    public void OnResumeClick()
    {
        if (isEnglish)
            settingsMenuENG.SetActive(false);
        else
            settingsMenuESP.SetActive(false);
    }

    public void OnMainMenuClick()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Menu");
    }
}
