using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuScene : MonoBehaviour
{
    public Material[] mats;
    public GameObject blockPrefab;
    public GameObject previewContainer;
    public Text previewNameENG, previewNameESP;

    private int saveCounter;
    private int previewIndex;

    private Vector3 startClick;

    public GameObject mainMenuCanvasENG;
    public GameObject mainMenuCanvasESP;
    public GameObject settingsCanvasENG;
    public GameObject settingsCanvasESP;

    private void Start()
    {
        if (PlayerPrefs.GetString("LANGUAGE") == "ENG")
            mainMenuCanvasENG.SetActive(true);
        else
            mainMenuCanvasESP.SetActive(true);

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
        RotatePreview();

        if(Input.GetMouseButtonDown(0))
        {
            startClick = Input.mousePosition;
        }

        if(Input.GetMouseButtonUp(0))
        {
            Vector3 delta = Input.mousePosition - startClick;

            if(Mathf.Abs(delta.x) > 2.5f)
            {
                if (delta.x < 0)
                    Swipe(true);
                else
                    Swipe(false);
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

        previewNameENG.text = blockData[0];
        previewNameESP.text = blockData[0];


        for (int i = 1; i < blockData.Length - 1; i++)
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

    private void RotatePreview()
    {
        previewContainer.transform.RotateAround(new Vector3(10,0,10),Vector3.up, 35 * Time.deltaTime);
    }

    public void OnPlayClick()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Game");
    }
    
    public void OnSettingsClick()
    {
        if (mainMenuCanvasENG.activeSelf)
        {
            mainMenuCanvasENG.SetActive(false);
            previewContainer.SetActive(false);
            settingsCanvasENG.SetActive(true);
        }
        else
        {
            mainMenuCanvasESP.SetActive(false);
            previewContainer.SetActive(false);
            settingsCanvasESP.SetActive(true);
        }
       

        // activar cosas del settings
    }

    public void OnBackSettingsClick()
    {

        if (settingsCanvasENG.activeSelf)
        {
            settingsCanvasENG.SetActive(false);
            mainMenuCanvasENG.SetActive(true);
            previewContainer.SetActive(true);
        }
        else
        {
            settingsCanvasESP.SetActive(false);
            mainMenuCanvasESP.SetActive(true);
            previewContainer.SetActive(true);
        }
    }

    public void OnSpanish()
    {
        // guardar en la preferencia el idioma español
        PlayerPrefs.SetString("LANGUAGE", "ESP");
        settingsCanvasENG.SetActive(false);
        settingsCanvasESP.SetActive(true);
    }

    public void OnEnglish()
    {
        // guardar en la preferencia el idioma inglés
        PlayerPrefs.SetString("LANGUAGE", "ENG");
        settingsCanvasESP.SetActive(false);
        settingsCanvasENG.SetActive(true);
    }
}
