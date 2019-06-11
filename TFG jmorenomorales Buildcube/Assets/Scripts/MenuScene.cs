using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuScene : MonoBehaviour
{
    public Material[] mats;
    public GameObject blockPrefab;
    public GameObject previewContainer;
    public Text previewNameText;
    public Button[] levelSelectButtons;
    public Sprite[] levelSelectSprites;

    private int saveCounter, previewIndex, availableLevels;
    private string language, previewName;
    private string gridtype = "0";
    private float gridtypeFloat;

    private Vector3 startClick;
    private bool isSwipingContainer;
    
    public GameObject mainMenu, gameModeSelect, gridSelect, levelSelect, settingsMenu, UI;
    private Animator mainMenuAnimator, gameModeSelectAnimator, gridSelectAnimator, levelSelectAnimator, settingsMenuAnimator;
    public Toggle qRToggle;

    private void Start()
    {
        Application.targetFrameRate = 60;
        language = PlayerPrefs.GetString("LANGUAGE");
        ChangeLanguage(language);
        PlayerPrefs.SetInt("AVAILABLELEVELS", 1);
        availableLevels = PlayerPrefs.GetInt("AVAILABLELEVELS");
        
        qRToggle.onValueChanged.AddListener(delegate { ToggleQRValueChanged(qRToggle); });

        if (PlayerPrefs.GetInt("ISUSINGQR") == 1)
            qRToggle.isOn = true;
        else
            qRToggle.isOn = false;

        GetAnimators();
        ChangeLevelSelectIcons();
        
        saveCounter = 0;
        previewIndex = 0;

        while (PlayerPrefs.HasKey(saveCounter.ToString()))
        {
            saveCounter++;
        }

        BuildPreview(previewIndex);
    }

    private void Update()
    {
        RotatePreview(gridtype);

        // Rotar container
        if(Input.GetMouseButtonDown(0) && Input.mousePosition.y<=520)
        {
            startClick = Input.mousePosition;
            isSwipingContainer = true;
        }

        // Cambiar De Menú
        else if(Input.GetMouseButtonDown(0) && Input.mousePosition.y > 520)
        {
            startClick = Input.mousePosition;
            isSwipingContainer = false;
        }

        /*
        if(Input.GetMouseButtonUp(0))
        {
            Vector3 delta = Input.mousePosition - startClick;

            if (isSwipingContainer)
            {
                if (Mathf.Abs(delta.x) > 2.5f)
                {
                    if (delta.x < 0)
                        Swipe(true);
                    else
                        Swipe(false);
                }
            }
            else
            {
                if (Mathf.Abs(delta.x) > 2.5f)
                {/*
                    if (delta.x < 0)
                    {
                        mainMenuAnimator.SetBool("GoLeftMM", true);
                        gameModeSelectAnimator.SetBool("GoLeftGM", true);
                        UI.SetActive(false);
                        previewContainer.SetActive(false);
                    }
                    else
                    {
                        mainMenuAnimator.SetBool("GoLeftMM", false);
                        gameModeSelectAnimator.SetBool("GoLeftGM", false);
                        UI.SetActive(true);
                        previewContainer.SetActive(true);
                    }
                }
            }
        }*/
    }

    private void ToggleQRValueChanged(Toggle qRToggle)
    {
        if (qRToggle.isOn)
            PlayerPrefs.SetInt("ISUSINGQR", 1);
        else
            PlayerPrefs.SetInt("ISUSINGQR", 0);
    }

    public void Swipe(bool left)
    {
        if(left)
        {
            previewIndex -= 1;
            if(previewIndex < 0)
            {
                previewIndex = saveCounter-1;
            }
        }
        else
        {
            previewIndex += 1;
            if (previewIndex > saveCounter-1)
            {
                previewIndex = 0;
            }
        }

        BuildPreview(previewIndex);
            
    }

    private void BuildPreview(int key)
    {
        if (!PlayerPrefs.HasKey(key.ToString()))
            return;

        foreach(Transform t in previewContainer.transform)
        {
            Destroy(t.gameObject);
        }

        string data = PlayerPrefs.GetString(key.ToString());
        string[] blockData = data.Split('%');

        previewNameText.text = blockData[0];
        previewName = blockData[0];

        gridtype = blockData[blockData.Length - 2];

        // Cambiar posicion de camara
        switch (blockData[blockData.Length - 2])
        {
            case "5":
                ChangeGridtypeFloat("0");
                GameObject.Find("Main Camera").transform.position = new Vector3(2.4f, 27.7f, -22.8f);
                break;
            case "10":
                ChangeGridtypeFloat("1");
                GameObject.Find("Main Camera").transform.position = new Vector3(5.5f, 45f, -34.8f);
                break;
            case "20":
                ChangeGridtypeFloat("2");
                GameObject.Find("Main Camera").transform.position = new Vector3(10.62f, 55.5f, -44.5f);
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

            GameObject go = Instantiate(blockPrefab) as GameObject;
            go.transform.SetParent(previewContainer.transform);
            go.transform.position = new Vector3(x,y,z);
            go.GetComponent<Renderer>().material = mats[(int)c];
        }
    }

    private void RotatePreview(string gridtype)
    {
        previewContainer.transform.RotateAround(new Vector3(gridtypeFloat, 0, gridtypeFloat), Vector3.up, 35 * Time.deltaTime);
    }

    private void ChangeGridtypeFloat(string gridtype)
    {
        switch (gridtype)
        {
            case "0":
                gridtypeFloat = 2.5f;
                break;
            case "1":
                gridtypeFloat = 5f;
                break;
            case "2":
                gridtypeFloat = 10f;
                break;
        }
    }

    public void OnFreeClick()
    {
        gameModeSelectAnimator.SetBool("GoMoreLeft", !gameModeSelectAnimator.GetBool("GoMoreLeft"));
        gridSelectAnimator.SetBool("IsGridSelectLeft", true);

        PlayerPrefs.SetString("GAMEMODE", "FREE");
    }

    public void OnPixelArtClick()
    {
        gameModeSelectAnimator.SetBool("GoMoreLeft", !gameModeSelectAnimator.GetBool("GoMoreLeft"));
        gridSelectAnimator.SetBool("IsGridSelectLeft", true);

        PlayerPrefs.SetString("GAMEMODE", "PIXELART");
    }

    public void OnChallengeClick()
    {
        gameModeSelectAnimator.SetBool("GoMoreLeft", !gameModeSelectAnimator.GetBool("GoMoreLeft"));
        levelSelectAnimator.SetBool("IsLevelSelectLR", true);
    }
    
    public void OnBackGameModeClick()
    {
        mainMenuAnimator.SetBool("GoLeftMM", false);
        gameModeSelectAnimator.SetBool("GoLeftGM", false);
        UI.SetActive(true);
        previewContainer.SetActive(true);
    }

    public void OnBackGridSelectClick()
    {
        gameModeSelectAnimator.SetBool("GoMoreLeft", !gameModeSelectAnimator.GetBool("GoMoreLeft"));
        gridSelectAnimator.SetBool("IsGridSelectLeft", false);
    }

    public void OnBackLevelSelectClick()
    {
        gameModeSelectAnimator.SetBool("GoMoreLeft", !gameModeSelectAnimator.GetBool("GoMoreLeft"));
        levelSelectAnimator.SetBool("IsLevelSelectLR", false);
    }

    public void OnPlayClick(int gridnum)
    {
        PlayerPrefs.SetInt("GRIDTYPE", gridnum);
        PlayerPrefs.SetString("MAP_ID", "NONE");
        if (PlayerPrefs.GetInt("ISUSINGQR") == 1)
            UnityEngine.SceneManagement.SceneManager.LoadScene("GameCustomQR");
        else
            UnityEngine.SceneManagement.SceneManager.LoadScene("GameDefault");
    }

    public void OnPlayClick()
    {
        if (mainMenuAnimator != null)
        {
            mainMenuAnimator.SetBool("GoLeftMM", !mainMenuAnimator.GetBool("GoLeftMM"));
            gameModeSelectAnimator.SetBool("GoLeftGM", !gameModeSelectAnimator.GetBool("GoLeftGM"));
            UI.SetActive(false);
            previewContainer.SetActive(false);
        }
    }

    public void OnSettingsClick()
    {
        gameModeSelectAnimator.SetBool("GoUpGM", true);
        settingsMenuAnimator.SetBool("IsSwippingUp", true);
    }

    public void OnBackSettingsClick()
    {
        gameModeSelectAnimator.SetBool("GoUpGM", false);
        settingsMenuAnimator.SetBool("IsSwippingUp", false);
    }

    public void OnSpanish()
    {
        PlayerPrefs.SetString("LANGUAGE", "ESP");
        language = "ESP";
        ChangeLanguage("ESP");
    }

    public void OnEnglish()
    {
        // guardar en la preferencia el idioma inglés
        PlayerPrefs.SetString("LANGUAGE", "ENG");
        language = "ENG";
        ChangeLanguage("ENG");
    }

    private void GetAnimators()
    {
        mainMenuAnimator = mainMenu.GetComponent<Animator>();
        gameModeSelectAnimator = gameModeSelect.GetComponent<Animator>();
        gridSelectAnimator = gridSelect.GetComponent<Animator>();
        levelSelectAnimator = levelSelect.GetComponent<Animator>();
        settingsMenuAnimator = settingsMenu.GetComponent<Animator>();
    }

    private void ChangeLanguage(string lng)
    {
        switch (lng)
        {
            case "ESP":
                // Settings Menu
                settingsMenu.GetComponentsInChildren<Text>()[0].text = "AJUSTES";
                settingsMenu.GetComponentsInChildren<Text>()[1].text = "IDIOMA";
                settingsMenu.GetComponentsInChildren<Text>()[2].text = "QR PERSONALIZADO";
                settingsMenu.GetComponentsInChildren<Text>()[3].text = "DESARROLLADOR";

                settingsMenu.GetComponentsInChildren<Button>()[4].GetComponentInChildren<Text>().text = "VOLVER";

                // Gamemode Menu
                gameModeSelect.GetComponentInChildren<Text>().text = "ELIGE EL MODO DE JUEGO";

                gameModeSelect.GetComponentsInChildren<Text>()[1].text = "LIBRE";
                gameModeSelect.GetComponentsInChildren<Text>()[2].text = "PIXEL ART";
                gameModeSelect.GetComponentsInChildren<Text>()[3].text = "DESAFÍO";

                // GridSelect Menu
                gridSelect.GetComponentInChildren<Text>().text = "ELIGE EL TAMAÑO DEL MAPA";

                // LevelSelect Menu
                levelSelect.GetComponentInChildren<Text>().text = "SELECCIÓN DE NIVEL";

                break;
            case "ENG":
                // Settings Menu
                settingsMenu.GetComponentsInChildren<Text>()[0].text = "SETTINGS";
                settingsMenu.GetComponentsInChildren<Text>()[1].text = "LANGUAGE";
                settingsMenu.GetComponentsInChildren<Text>()[2].text = "CUSTOM QR";
                settingsMenu.GetComponentsInChildren<Text>()[3].text = "DEVELOPER";

                settingsMenu.GetComponentsInChildren<Button>()[4].GetComponentInChildren<Text>().text = "BACK";

                // Gamemode Menu
                gameModeSelect.GetComponentInChildren<Text>().text = "CHOOSE GAMEMODE";

                gameModeSelect.GetComponentsInChildren<Text>()[1].text = "FREE";
                gameModeSelect.GetComponentsInChildren<Text>()[2].text = "PIXEL ART";
                gameModeSelect.GetComponentsInChildren<Text>()[3].text = "CHALLENGE";

                // GridSelect Menu
                gridSelect.GetComponentInChildren<Text>().text = "CHOOSE THE SIZE OF THE MAP";

                // LevelSelect Menu
                levelSelect.GetComponentInChildren<Text>().text = "LEVEL SELECTOR";

                break;
            default:
                break;
        }
    }

    private void ChangeLevelSelectIcons()
    {
        for(int i=0; i< availableLevels; i++)
        {
            levelSelectButtons[i].image.sprite = levelSelectSprites[i];
        }
    }

    public void OnLevelSelection(int levelNum)
    {
        if (levelNum < availableLevels)
        {
            // Cargamos el nivel seleccionado
            PlayerPrefs.SetInt("SELECTEDLEVEL", levelNum);
            UnityEngine.SceneManagement.SceneManager.LoadScene("Challenge");

        }
        else
        {
            Debug.Log("AÚN NO TIENES ACCESO A ESTE NIVEL");
        }

        Debug.Log("Pues nada, que el nivel es: " + PlayerPrefs.GetInt("SELECTEDLEVEL"));
    }

    public void OnTextPreviewClick()
    {
        PlayerPrefs.SetString("MAP_ID", previewIndex.ToString());
        // Comprobamos si hay customQR o no
        if(PlayerPrefs.GetInt("ISUSINGQR") == 1)
            UnityEngine.SceneManagement.SceneManager.LoadScene("GameCustomQR");
        else
            UnityEngine.SceneManagement.SceneManager.LoadScene("GameDefault");
    }

    public void OnDeveloperButtonClick(int whatUrl)
    {
        switch (whatUrl)
        {
            case 0:
                Application.OpenURL("https://twitter.com/kokebr_");
                break;
            case 1:
                Application.OpenURL("https://github.com/jmorenomorales");
                break;
        }
    }
}