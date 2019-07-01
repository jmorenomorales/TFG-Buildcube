using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuScene : MonoBehaviour
{
    #region Variables

    public Material[] mats;
    public GameObject blockPrefab;
    public GameObject previewContainer;
    public Text previewNameText;
    public Button[] levelSelectButtons;
    public Sprite[] levelSelectSprites;
    public Sprite[] levelSelectSpritesDeactivated;

    private int saveCounter, previewIndex, availableLevels;
    private string language, previewName;
    private string gridtype = "0";
    private float gridtypeFloat;

    private Vector3 startClick;
    private bool isSwipingContainer;
    
    public GameObject mainMenu, gameModeSelect, gridSelect, levelSelect, settingsMenu, UI, challengeSettingsMenu, tutorialMenu;
    private Animator mainMenuAnimator, gameModeSelectAnimator, gridSelectAnimator, levelSelectAnimator, settingsMenuAnimator;
    public Toggle qRToggle, timeAttackToggle, seeBestTimeToggle;
    public Button[] mainMenuButtons;

    #endregion

    private void Start()
    {
        Application.targetFrameRate = 60;
        if(PlayerPrefs.GetString("LANGUAGE", "NOLANGUAGE") == "NOLANGUAGE")
        {
            PlayerPrefs.SetString("LANGUAGE", "ESP");
            PlayerPrefs.SetString("0", "Nibloniano%5|0|12|2%6|0|12|2%6|1|12|2%6|2|12|2%6|3|12|2%7|0|7|28%7|0|8|2%7|0|9|2%7|0|10|0%7|0|11|0%7|0|12|0%7|0|13|0%7|0|14|0%7|0|15|5%7|0|16|5%7|1|7|28%7|1|10|0%7|1|11|0%7|1|12|0%7|1|13|0%7|1|14|0%7|1|15|5%7|2|10|2%7|2|11|2%7|2|12|2%7|2|13|2%7|2|14|2%7|2|15|5%7|3|10|2%7|3|11|2%7|3|12|2%7|3|13|2%7|3|14|2%7|3|15|5%7|4|11|5%7|4|12|5%7|4|13|5%7|4|14|5%7|4|15|5%7|5|10|27%7|5|11|2%7|5|12|2%7|5|13|2%7|5|14|2%7|6|10|0%7|6|11|2%7|6|12|2%7|6|13|2%7|6|14|2%7|7|10|0%7|7|11|2%7|7|12|2%7|7|13|2%7|7|14|2%8|0|9|0%8|0|15|5%8|0|16|5%8|0|17|5%8|0|18|5%8|1|9|0%8|1|15|5%8|2|10|27%8|2|15|5%8|3|10|2%8|3|15|5%8|4|10|0%8|4|15|5%8|5|10|27%8|5|14|2%8|6|10|2%8|6|14|2%8|7|10|0%8|7|11|2%8|7|12|2%8|7|13|2%8|7|14|2%8|8|11|2%8|8|12|2%8|8|13|2%9|0|9|0%9|0|15|5%9|0|16|5%9|0|17|5%9|0|18|5%9|1|9|0%9|1|15|5%9|2|10|27%9|2|15|5%9|3|10|2%9|3|15|5%9|4|15|5%9|5|10|27%9|5|11|2%9|5|12|2%9|5|13|2%9|5|14|2%9|6|10|27%9|6|13|2%9|6|14|2%9|7|10|2%9|7|11|2%9|7|12|2%9|7|13|2%9|7|14|2%9|8|11|2%9|8|12|2%9|8|13|2%9|9|12|2%9|10|12|2%9|11|12|0%10|0|9|0%10|0|15|5%10|0|16|5%10|0|17|5%10|0|18|5%10|1|9|0%10|1|15|5%10|2|10|27%10|2|15|5%10|3|10|2%10|3|15|5%10|4|10|0%10|4|15|5%10|5|10|27%10|5|11|2%10|5|12|2%10|5|13|2%10|5|14|2%10|6|10|2%10|6|13|2%10|6|14|2%10|7|10|0%10|7|11|2%10|7|12|2%10|7|13|2%10|7|14|2%10|8|11|2%10|8|12|2%10|8|13|2%11|0|7|28%11|0|8|2%11|0|9|2%11|0|10|0%11|0|11|0%11|0|12|0%11|0|13|0%11|0|14|0%11|0|15|5%11|0|16|5%11|1|7|28%11|1|10|0%11|1|11|0%11|1|12|0%11|1|13|0%11|1|14|0%11|1|15|5%11|2|10|2%11|2|11|2%11|2|12|2%11|2|13|2%11|2|14|2%11|2|15|5%11|3|10|2%11|3|11|2%11|3|12|2%11|3|13|2%11|3|14|2%11|3|15|5%11|4|11|5%11|4|12|5%11|4|13|5%11|4|14|5%11|4|15|5%11|5|10|27%11|5|11|2%11|5|12|2%11|5|13|2%11|5|14|2%11|6|10|0%11|6|11|2%11|6|12|2%11|6|13|2%11|6|14|2%11|7|10|0%11|7|11|2%11|7|12|2%11|7|13|2%11|7|14|2%12|0|12|2%12|1|12|2%12|2|12|2%12|3|12|2%13|0|12|2%20%");
            PlayerPrefs.SetString("1", "Loro%2|3|9|29%2|4|9|29%2|5|9|29%3|3|9|29%3|4|9|29%3|5|9|29%3|6|9|2%3|7|9|2%3|8|9|2%3|9|9|2%3|13|9|2%3|14|9|2%3|15|9|2%3|16|9|2%4|3|9|29%4|4|9|29%4|5|9|2%4|6|9|13%4|7|9|13%4|8|9|13%4|9|9|13%4|10|9|2%4|12|9|2%4|13|9|2%4|14|9|4%4|15|9|0%4|16|9|0%4|17|9|4%4|18|9|2%4|19|9|2%5|3|9|29%5|4|9|29%5|5|9|29%5|6|9|2%5|7|9|13%5|8|9|13%5|9|9|25%5|10|9|25%5|11|9|2%5|12|9|4%5|13|9|4%5|14|9|0%5|15|9|0%5|16|9|0%5|17|9|0%5|18|9|4%5|19|9|4%5|20|9|2%6|3|9|29%6|4|9|29%6|5|9|2%6|6|9|4%6|7|9|4%6|8|9|4%6|9|9|4%6|10|9|4%6|11|9|4%6|12|9|4%6|13|9|4%6|14|9|0%6|15|9|2%6|16|9|0%6|17|9|0%6|18|9|4%6|19|9|4%6|20|9|4%6|21|9|2%7|2|9|2%7|3|9|29%7|4|9|29%7|5|9|2%7|6|9|4%7|7|9|4%7|8|9|4%7|9|9|4%7|10|9|4%7|11|9|4%7|12|9|4%7|13|9|0%7|14|9|0%7|15|9|0%7|16|9|0%7|17|9|4%7|18|9|4%7|19|9|4%7|20|9|4%7|21|9|2%8|1|9|2%8|2|9|4%8|3|9|29%8|4|9|29%8|5|9|2%8|6|9|4%8|7|9|4%8|8|9|4%8|9|9|4%8|10|9|4%8|11|9|4%8|12|9|2%8|13|9|2%8|14|9|2%8|15|9|2%8|16|9|2%8|17|9|4%8|18|9|4%8|19|9|4%8|20|9|4%8|21|9|4%8|22|9|2%9|0|9|2%9|1|9|4%9|2|9|4%9|3|9|29%9|4|9|29%9|5|9|29%9|6|9|2%9|7|9|4%9|8|9|4%9|9|9|4%9|10|9|4%9|11|9|2%9|12|9|2%9|13|9|2%9|14|9|2%9|15|9|2%9|16|9|2%9|17|9|4%9|18|9|4%9|19|9|4%9|20|9|4%9|21|9|4%9|22|9|2%10|0|9|2%10|1|9|4%10|2|9|4%10|3|9|29%10|4|9|29%10|5|9|29%10|6|9|2%10|7|9|4%10|8|9|4%10|9|9|4%10|10|9|4%10|11|9|2%10|12|9|2%10|13|9|2%10|14|9|2%10|15|9|2%10|16|9|2%10|17|9|4%10|18|9|4%10|19|9|4%10|20|9|4%10|21|9|4%10|22|9|2%11|1|9|2%11|2|9|4%11|3|9|29%11|4|9|29%11|5|9|2%11|6|9|4%11|7|9|4%11|8|9|4%11|9|9|4%11|10|9|4%11|11|9|4%11|12|9|2%11|13|9|2%11|14|9|2%11|15|9|2%11|16|9|2%11|17|9|4%11|18|9|4%11|19|9|4%11|20|9|4%11|21|9|4%11|22|9|2%12|2|9|2%12|3|9|29%12|4|9|29%12|5|9|2%12|6|9|4%12|7|9|4%12|8|9|4%12|9|9|4%12|10|9|4%12|11|9|4%12|12|9|4%12|13|9|0%12|14|9|0%12|15|9|0%12|16|9|0%12|17|9|4%12|18|9|4%12|19|9|4%12|20|9|4%12|21|9|2%13|3|9|29%13|4|9|29%13|5|9|2%13|6|9|4%13|7|9|4%13|8|9|4%13|9|9|4%13|10|9|4%13|11|9|4%13|12|9|4%13|13|9|4%13|14|9|0%13|15|9|2%13|16|9|0%13|17|9|0%13|18|9|4%13|19|9|4%13|20|9|4%13|21|9|2%14|3|9|29%14|4|9|29%14|5|9|29%14|6|9|2%14|7|9|13%14|8|9|13%14|9|9|25%14|10|9|25%14|11|9|2%14|12|9|4%14|13|9|4%14|14|9|0%14|15|9|0%14|16|9|0%14|17|9|0%14|18|9|4%14|19|9|4%14|20|9|2%15|3|9|29%15|4|9|29%15|5|9|2%15|6|9|13%15|7|9|13%15|8|9|13%15|9|9|13%15|10|9|2%15|12|9|2%15|13|9|2%15|14|9|4%15|15|9|0%15|16|9|0%15|17|9|4%15|18|9|2%15|19|9|2%16|3|9|29%16|4|9|29%16|5|9|29%16|6|9|2%16|7|9|2%16|8|9|2%16|9|9|2%16|14|9|2%16|15|9|2%16|16|9|2%16|17|9|2%17|3|9|29%17|4|9|29%17|5|9|29%20%");
        }
        language = PlayerPrefs.GetString("LANGUAGE");
        ChangeLanguage(language);

        if(PlayerPrefs.GetInt("AVAILABLELEVELS", -100) == -100)
        {
            PlayerPrefs.SetInt("AVAILABLELEVELS", 1);
        }

        if (tutorialMenu.activeSelf)
            tutorialMenu.SetActive(false);

        //PlayerPrefs.SetInt("TIMEATTACK", 0);
        availableLevels = PlayerPrefs.GetInt("AVAILABLELEVELS");
        
        qRToggle.onValueChanged.AddListener(delegate { ToggleQRValueChanged(qRToggle); });
        timeAttackToggle.onValueChanged.AddListener(delegate { ToggleTimeAttack(timeAttackToggle); });
        seeBestTimeToggle.onValueChanged.AddListener(delegate { ToggleSeeBestTime(seeBestTimeToggle); });

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

        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
    }

    private void ToggleTimeAttack(Toggle timeAttackToggle)
    {
        if (timeAttackToggle.isOn)
            PlayerPrefs.SetInt("TIMEATTACK", 1);
        else
            PlayerPrefs.SetInt("TIMEATTACK", 0);
    }
    private void ToggleSeeBestTime(Toggle seeBestTimeToggle)
    {
        if (seeBestTimeToggle.isOn)
            levelSelect.GetComponentsInChildren<Text>()[1].enabled = true;
        else
            levelSelect.GetComponentsInChildren<Text>()[1].enabled = false;
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
                GameObject.Find("Main Camera").transform.position = new Vector3(10.62f, 67.6f, -46f);
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
        StartCoroutine("ShowPreview");
    }

    IEnumerator ShowPreview()
    {
        yield return new WaitForSeconds(0.2f);
        UI.SetActive(true);
        previewContainer.SetActive(true);
        yield return null;
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
                gameModeSelect.GetComponentsInChildren<Text>()[4].text = "DESCARGA EL PDF";

                // GridSelect Menu
                gridSelect.GetComponentInChildren<Text>().text = "ELIGE EL TAMAÑO DEL MAPA";

                // LevelSelect Menu
                levelSelect.GetComponentsInChildren<Text>()[0].text = "SELECCIÓN DE NIVEL";
                //levelSelect.GetComponentsInChildren<Text>()[1].text = "TU MEJOR TIEMPO: " + PlayerPrefs.GetString("BESTIME");

                // Challenge Settings Menu
                challengeSettingsMenu.GetComponentsInChildren<Text>()[0].text = "AJUSTES MODO DESAFÍO";
                challengeSettingsMenu.GetComponentsInChildren<Text>()[1].text = "RESETEAR NIVELES";
                challengeSettingsMenu.GetComponentsInChildren<Text>()[2].text = "VOLVER";

                // Tutorial Menu
                tutorialMenu.GetComponentsInChildren<Text>()[0].text = "¿QUIERES VER EL TUTORIAL EN YOUTUBE?";
                tutorialMenu.GetComponentsInChildren<Text>()[1].text = "SÍ";
                tutorialMenu.GetComponentsInChildren<Text>()[2].text = "VOLVER";

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
                gameModeSelect.GetComponentsInChildren<Text>()[4].text = "DOWNLOAD PDF";

                // GridSelect Menu
                gridSelect.GetComponentInChildren<Text>().text = "CHOOSE THE SIZE OF THE MAP";

                // LevelSelect Menu
                levelSelect.GetComponentsInChildren<Text>()[0].text = "LEVEL SELECTOR";
                //levelSelect.GetComponentsInChildren<Text>()[1].text = "YOUR BEST TIME: " + PlayerPrefs.GetString("BESTIME");
                
                // Challenge Settings Menu
                challengeSettingsMenu.GetComponentsInChildren<Text>()[0].text = "CHALLENGE MODE SETTINGS";
                challengeSettingsMenu.GetComponentsInChildren<Text>()[1].text = "RESET LEVELS";
                challengeSettingsMenu.GetComponentsInChildren<Text>()[2].text = "BACK";

                // Tutorial Menu
                tutorialMenu.GetComponentsInChildren<Text>()[0].text = "DO YOU WANT TO SEE THE TUTORIAL ON YOUTUBE?";
                tutorialMenu.GetComponentsInChildren<Text>()[1].text = "YES";
                tutorialMenu.GetComponentsInChildren<Text>()[2].text = "BACK";

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
    }

    public void OnHelpButtonClick()
    {
        tutorialMenu.SetActive(true);

        mainMenuButtons[0].interactable = false;
        mainMenuButtons[1].interactable = false;
        mainMenuButtons[2].interactable = false;
        mainMenuButtons[3].interactable = false;
    }

    public void OnHelpPanelClick(int clickType)
    {
        if(clickType == 0)
        {
            //Cargar video
            Application.OpenURL("https://youtu.be/476SwDBX3RI");
        }

        tutorialMenu.SetActive(false);
        mainMenuButtons[0].interactable = true;
        mainMenuButtons[1].interactable = true;
        mainMenuButtons[2].interactable = true;
        mainMenuButtons[3].interactable = true;
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

    public void OnDeveloperButtonClick(int whichUrl)
    {
        switch (whichUrl)
        {
            case 0:
                Application.OpenURL("https://twitter.com/kokebr_");
                break;
            case 1:
                Application.OpenURL("https://github.com/jmorenomorales");
                break;
        }
    }

    public void OnLevelSelectorSettings()
    {
        challengeSettingsMenu.SetActive(true);
        levelSelect.GetComponentsInChildren<Button>()[0].interactable = false;
        levelSelect.GetComponentsInChildren<Button>()[10].interactable = false;
    }

    public void ResetLevels()
    {
        PlayerPrefs.SetInt("AVAILABLELEVELS", 1);
        availableLevels = 1;
        for (int i = 1; i < 9; i++)
        {
            levelSelectButtons[i].image.sprite = levelSelectSpritesDeactivated[i];
        }
    }

    public void BackLevelSelectorSettings()
    {
        challengeSettingsMenu.SetActive(false);
        levelSelect.GetComponentsInChildren<Button>()[0].interactable = true;
        levelSelect.GetComponentsInChildren<Button>()[10].interactable = true;
    }

    public void OnPDFDownloadClick()
    {
        Application.OpenURL("https://docdro.id/g6XiMC7");
    }
}