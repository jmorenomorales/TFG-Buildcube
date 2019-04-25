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

    private int saveCounter, previewIndex;
    private string language, previewName;
    private string gridtype = "0";
    private float gridtypeFloat;

    private Vector3 startClick;
    private bool isSwipingContainer;

    public GameObject UI, mainMenu, settingsCanvasENG, settingsCanvasESP, gridSelect;

    private void Start()
    {
        language = PlayerPrefs.GetString("LANGUAGE");

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
                Animator animatorMM = mainMenu.GetComponentInChildren<Animator>();
                Animator animatorGM = gridSelect.GetComponentInChildren<Animator>();
                if (Mathf.Abs(delta.x) > 2.5f)
                {
                    if (delta.x < 0)
                    {
                        animatorMM.SetBool("GoLeftMM", true);
                        animatorGM.SetBool("GoLeftGM", true);
                    }
                    else
                    {
                        animatorMM.SetBool("GoLeftMM", false);
                        animatorGM.SetBool("GoLeftGM", false);
                    }
                }
            }
        }
    }

    private void Swipe(bool left)
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

    public void OnPlayClick(int gridnum)
    {
        PlayerPrefs.SetInt("GRIDTYPE", gridnum);
        UnityEngine.SceneManagement.SceneManager.LoadScene("Game");
    }

    public void OnPlayClick()
    {
        Debug.Log("He pulsado en play");
        Animator animatorMM = mainMenu.GetComponentInChildren<Animator>();
        Animator animatorGM = gridSelect.GetComponentInChildren<Animator>();
        if (animatorMM != null)
        {
            animatorMM.SetBool("GoLeftMM", !animatorMM.GetBool("GoLeftMM"));
            animatorGM.SetBool("GoLeftGM", !animatorGM.GetBool("GoLeftGM"));
        }

        gridSelect.SetActive(true);
    }

    public void OnSettingsClick()
    {
        mainMenu.SetActive(false);
        UI.SetActive(false);

        if (language == "ENG")
        {
            previewContainer.SetActive(false);
            settingsCanvasENG.SetActive(true);
        }
        else
        {
            previewContainer.SetActive(false);
            settingsCanvasESP.SetActive(true);
        }
    }

    public void OnBackSettingsClick()
    {
        if (settingsCanvasENG.activeSelf)
        {
            settingsCanvasENG.SetActive(false);
            previewContainer.SetActive(true);
        }
        else
        {
            settingsCanvasESP.SetActive(false);
            previewContainer.SetActive(true);
        }
        mainMenu.SetActive(true);
        UI.SetActive(true);
    }

    public void OnSpanish()
    {
        // guardar en la preferencia el idioma español
        PlayerPrefs.SetString("LANGUAGE", "ESP");
        language = "ESP";
        settingsCanvasENG.SetActive(false);
        settingsCanvasESP.SetActive(true);
    }

    public void OnEnglish()
    {
        // guardar en la preferencia el idioma inglés
        PlayerPrefs.SetString("LANGUAGE", "ENG");
        language = "ENG";
        settingsCanvasESP.SetActive(false);
        settingsCanvasENG.SetActive(true);
    }
}