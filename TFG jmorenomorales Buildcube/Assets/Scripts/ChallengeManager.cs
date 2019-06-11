using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ChallengeManager : MonoBehaviour
{
    public static ChallengeManager Instance { set; get; }

    private float[][] gridSettings;                                     // Recogemos la configuración del grid elegida por el usuario
    private float blockSize;                                            // Definimos la escala de los bloques
    public Block[,,] blocksChallenge;                                   // Array contenedor de los bloques
    public GameObject blockPrefab;                                      // Prefab de los bloques
    public GameObject cubesParent;

    public BlockColor selectedColor;                                    // Enum del color seleccionado
    public Material[] blockMaterials;                                   // Array contenedor de los materiales de los bloques
    public Material[] gridMaterials;

    private GameObject foundationObject;                                // Guardamos el objeto base (plano)

    private Vector3 blockOffset;                                        // Damos un offset a los cubos ya que su pivote esta en el centro
    private Vector3 foundationCenter = new Vector3(1.25f, 0, 1.25f);    // Establecemos el centro del plano

    private bool isDeleting;                                            // Comprobamos si el botón de borrar está activo

    public Button deleteButton, colorButton;
    public Sprite[] deleteButtons;

    public GameObject colorPanel;
    public Button[] colorButtons;
    public Sprite[] colorButtonsSprites;
    public Sprite colorDes;

    private Stack<BlockAction> playerActions;
    private BlockAction playerAction;
    private string gameMode, gridTypeLoadedMap;
    
    private string[] maps;
    private string currentMap;
    private int selectedLevel;
    private bool isCorrectBool;
    public Sprite[] levelHints;
    public GameObject hintsPanel;
    public Image hintImage;

    private void Start()
    {
        Instance = this;
        // Buscamos en la escena el objeto con nombre "Foundation"
        foundationObject = GameObject.Find("Foundation");
        
        playerActions = new Stack<BlockAction>();
       
        isCorrectBool = false;
        selectedLevel = PlayerPrefs.GetInt("SELECTEDLEVEL");

        //hintsPanel.GetComponentInChildren<Image>().sprite = levelHints[selectedLevel];

        hintImage.sprite = levelHints[selectedLevel];

        currentMap = null;

        maps = new string[10];

        maps[0] = "0|0|1|25%0|0|3|25%0|0|4|2%1|0|0|25%1|0|1|25%1|0|2|3%1|0|3|2%2|0|1|25%2|0|2|25%2|0|3|25%3|0|0|25%3|0|1|25%3|0|2|3%3|0|3|2%4|0|1|25%4|0|3|25%4|0|4|2%"; //Pikachu
        maps[1] = "0|0|0|2%0|0|1|2%0|0|2|29%1|0|0|2%1|0|1|28%1|0|2|29%1|0|3|28%2|0|0|2%2|0|1|2%2|0|2|29%2|0|3|28%2|0|4|28%3|0|0|2%3|0|1|28%3|0|2|29%3|0|3|29%4|0|0|2%4|0|1|2%4|0|2|29%"; // Wd
        maps[2] = "0|0|0|0%0|0|1|0%0|0|2|29%0|0|4|2%1|0|0|2%1|0|1|0%1|0|2|2%1|0|3|29%1|0|4|2%2|0|0|5%2|0|1|2%2|0|2|0%2|0|3|29%3|0|0|2%3|0|1|0%3|0|2|2%3|0|3|29%3|0|4|2%4|0|0|0%4|0|1|0%4|0|2|29%4|0|4|2%";  // Perrete
        maps[3] = "0|0|0|22%0|0|1|22%0|0|2|26%0|0|3|22%1|0|0|22%1|0|1|26%1|0|2|26%1|0|3|22%1|0|4|22%2|0|0|23%2|0|1|26%2|0|2|26%2|0|3|23%2|0|4|23%3|0|0|22%3|0|1|26%3|0|2|26%3|0|3|22%3|0|4|22%4|0|1|22%4|0|2|26%4|0|3|22%";   //Casco
        maps[4] = "0|0|0|16%0|0|3|29%1|0|0|16%1|0|2|29%1|0|3|29%2|0|0|16%2|0|1|29%2|0|2|29%2|0|3|29%3|0|0|16%3|0|1|29%3|0|2|25%3|0|3|29%3|0|4|15%3|0|5|15%3|0|6|15%3|0|7|15%3|0|8|15%4|0|0|16%4|0|1|29%4|0|2|29%4|0|3|29%4|0|5|15%4|0|6|15%4|0|7|15%4|0|8|15%5|0|0|16%5|0|1|29%5|0|2|25%5|0|3|29%5|0|5|15%5|0|6|15%5|0|7|15%5|0|8|15%6|0|0|16%6|0|1|29%6|0|2|29%6|0|3|29%6|0|5|15%6|0|6|15%6|0|7|15%7|0|0|16%7|0|1|29%7|0|2|25%7|0|3|29%7|0|5|15%7|0|6|15%7|0|7|15%8|0|0|16%8|0|1|29%8|0|2|29%8|0|3|29%8|0|6|15%8|0|7|15%9|0|0|16%9|0|6|15%"; // Barco
        maps[5] = "0|0|7|2%1|0|5|4%1|0|6|5%1|0|7|1%1|0|8|1%1|0|9|1%2|0|4|4%2|0|5|5%2|0|6|5%2|0|7|1%2|0|8|2%2|0|9|1%3|0|3|5%3|0|4|5%3|0|5|5%3|0|6|5%3|0|7|5%3|0|8|1%3|0|9|1%4|0|3|5%4|0|4|5%4|0|5|2%4|0|6|1%4|0|7|1%4|0|8|1%4|0|9|1%5|0|3|5%5|0|4|1%5|0|5|0%5|0|6|2%5|0|7|1%6|0|0|2%6|0|1|2%6|0|2|2%6|0|3|1%6|0|4|0%6|0|5|2%6|0|6|2%6|0|7|1%7|0|3|1%7|0|4|1%7|0|5|2%7|0|6|2%7|0|7|1%8|0|4|2%8|0|5|2%8|0|6|2%9|0|5|2%";    // Pájaro
        maps[6] = "0|0|1|29%1|0|0|29%1|0|1|25%1|0|3|4%1|0|4|4%2|0|0|29%2|0|1|25%2|0|3|25%2|0|4|28%2|0|5|4%3|0|0|29%3|0|1|25%3|0|3|22%3|0|4|25%3|0|5|28%3|0|6|4%4|0|0|29%4|0|1|25%4|0|3|16%4|0|4|22%4|0|5|25%4|0|6|28%4|0|7|4%5|0|0|29%5|0|1|25%5|0|3|8%5|0|4|16%5|0|5|22%5|0|6|25%5|0|7|28%5|0|8|4%6|0|0|29%6|0|1|25%6|0|4|8%6|0|5|16%6|0|6|22%6|0|7|25%6|0|8|28%6|0|9|4%7|0|1|29%7|0|5|8%7|0|6|16%7|0|7|22%7|0|8|25%7|0|9|28%8|0|6|8%8|0|7|16%8|0|8|22%8|0|9|25%9|0|7|8%9|0|8|16%9|0|9|22%";   // Arcoiris
        maps[7] = "0|0|0|14%0|0|1|14%0|0|2|14%0|0|3|14%0|0|4|14%0|0|5|14%0|0|6|14%0|0|7|14%0|0|8|14%0|0|9|14%0|0|10|14%0|0|11|14%0|0|12|14%0|0|13|14%0|0|14|14%0|0|15|14%0|0|16|14%0|0|17|14%0|0|18|14%0|0|19|14%1|0|0|14%1|0|1|25%1|0|2|25%1|0|3|25%1|0|4|14%1|0|5|14%1|0|6|14%1|0|7|14%1|0|8|14%1|0|9|14%1|0|10|14%1|0|11|14%1|0|12|14%1|0|13|14%1|0|14|14%1|0|15|14%1|0|16|14%1|0|17|14%1|0|18|14%1|0|19|14%2|0|0|25%2|0|1|14%2|0|2|14%2|0|3|25%2|0|4|25%2|0|5|25%2|0|6|14%2|0|7|2%2|0|8|2%2|0|9|2%2|0|10|2%2|0|11|14%2|0|12|14%2|0|13|14%2|0|14|14%2|0|15|14%2|0|16|14%2|0|17|15%2|0|18|14%2|0|19|14%3|0|0|25%3|0|1|25%3|0|2|14%3|0|3|14%3|0|4|14%3|0|5|25%3|0|6|25%3|0|7|5%3|0|8|5%3|0|9|5%3|0|10|5%3|0|11|2%3|0|12|2%3|0|13|14%3|0|14|14%3|0|15|14%3|0|16|15%3|0|17|0%3|0|18|15%3|0|19|14%4|0|0|14%4|0|1|25%4|0|2|14%4|0|3|14%4|0|4|2%4|0|5|5%4|0|6|5%4|0|7|5%4|0|8|5%4|0|9|5%4|0|10|5%4|0|11|5%4|0|12|5%4|0|13|2%4|0|14|14%4|0|15|14%4|0|16|14%4|0|17|15%4|0|18|14%4|0|19|14%5|0|0|14%5|0|1|25%5|0|2|25%5|0|3|2%5|0|4|5%5|0|5|5%5|0|6|5%5|0|7|5%5|0|8|5%5|0|9|4%5|0|10|4%5|0|11|4%5|0|12|4%5|0|13|4%5|0|14|2%5|0|15|14%5|0|16|14%5|0|17|14%5|0|18|14%5|0|19|14%6|0|0|14%6|0|1|14%6|0|2|25%6|0|3|25%6|0|4|5%6|0|5|5%6|0|6|5%6|0|7|4%6|0|8|4%6|0|9|4%6|0|10|4%6|0|11|4%6|0|12|4%6|0|13|4%6|0|14|4%6|0|15|2%6|0|16|14%6|0|17|14%6|0|18|14%6|0|19|14%7|0|0|14%7|0|1|14%7|0|2|25%7|0|3|25%7|0|4|25%7|0|5|5%7|0|6|4%7|0|7|4%7|0|8|4%7|0|9|4%7|0|10|4%7|0|11|4%7|0|12|4%7|0|13|4%7|0|14|4%7|0|15|2%7|0|16|14%7|0|17|14%7|0|18|14%7|0|19|14%8|0|0|14%8|0|1|2%8|0|2|5%8|0|3|5%8|0|4|25%8|0|5|25%8|0|6|4%8|0|7|4%8|0|8|4%8|0|9|4%8|0|10|4%8|0|11|4%8|0|12|4%8|0|13|4%8|0|14|4%8|0|15|3%8|0|16|2%8|0|17|14%8|0|18|14%8|0|19|14%9|0|0|14%9|0|1|2%9|0|2|5%9|0|3|5%9|0|4|4%9|0|5|25%9|0|6|25%9|0|7|4%9|0|8|4%9|0|9|4%9|0|10|4%9|0|11|4%9|0|12|4%9|0|13|4%9|0|14|4%9|0|15|3%9|0|16|2%9|0|17|14%9|0|18|14%9|0|19|14%10|0|0|14%10|0|1|2%10|0|2|5%10|0|3|4%10|0|4|4%10|0|5|25%10|0|6|25%10|0|7|25%10|0|8|4%10|0|9|4%10|0|10|4%10|0|11|4%10|0|12|4%10|0|13|4%10|0|14|3%10|0|15|3%10|0|16|2%10|0|17|14%10|0|18|14%10|0|19|14%11|0|0|14%11|0|1|2%11|0|2|5%11|0|3|4%11|0|4|4%11|0|5|4%11|0|6|25%11|0|7|25%11|0|8|25%11|0|9|4%11|0|10|4%11|0|11|4%11|0|12|4%11|0|13|3%11|0|14|3%11|0|15|3%11|0|16|2%11|0|17|14%11|0|18|14%11|0|19|14%12|0|0|14%12|0|1|14%12|0|2|2%12|0|3|4%12|0|4|4%12|0|5|4%12|0|6|4%12|0|7|25%12|0|8|25%12|0|9|25%12|0|10|4%12|0|11|4%12|0|12|3%12|0|13|3%12|0|14|3%12|0|15|2%12|0|16|14%12|0|17|14%12|0|18|14%12|0|19|14%13|0|0|14%13|0|1|14%13|0|2|2%13|0|3|4%13|0|4|4%13|0|5|4%13|0|6|4%13|0|7|4%13|0|8|4%13|0|9|25%13|0|10|25%13|0|11|3%13|0|12|3%13|0|13|3%13|0|14|3%13|0|15|2%13|0|16|14%13|0|17|14%13|0|18|14%13|0|19|14%14|0|0|14%14|0|1|14%14|0|2|14%14|0|3|2%14|0|4|4%14|0|5|4%14|0|6|4%14|0|7|4%14|0|8|4%14|0|9|3%14|0|10|25%14|0|11|25%14|0|12|25%14|0|13|3%14|0|14|2%14|0|15|25%14|0|16|25%14|0|17|14%14|0|18|14%14|0|19|14%15|0|0|14%15|0|1|14%15|0|2|14%15|0|3|14%15|0|4|2%15|0|5|4%15|0|6|4%15|0|7|4%15|0|8|3%15|0|9|3%15|0|10|3%15|0|11|25%15|0|12|25%15|0|13|25%15|0|14|14%15|0|15|14%15|0|16|25%15|0|17|14%15|0|18|14%15|0|19|14%16|0|0|14%16|0|1|15%16|0|2|14%16|0|3|14%16|0|4|14%16|0|5|2%16|0|6|2%16|0|7|3%16|0|8|3%16|0|9|3%16|0|10|3%16|0|11|2%16|0|12|2%16|0|13|25%16|0|14|25%16|0|15|14%16|0|16|14%16|0|17|25%16|0|18|14%16|0|19|14%17|0|0|15%17|0|1|0%17|0|2|15%17|0|3|14%17|0|4|14%17|0|5|14%17|0|6|14%17|0|7|2%17|0|8|2%17|0|9|2%17|0|10|2%17|0|11|14%17|0|12|14%17|0|13|14%17|0|14|25%17|0|15|14%17|0|16|14%17|0|17|25%17|0|18|14%17|0|19|14%18|0|0|14%18|0|1|15%18|0|2|14%18|0|3|14%18|0|4|14%18|0|5|14%18|0|6|14%18|0|7|14%18|0|8|14%18|0|9|14%18|0|10|14%18|0|11|14%18|0|12|14%18|0|13|14%18|0|14|14%18|0|15|25%18|0|16|25%18|0|17|14%18|0|18|14%18|0|19|14%19|0|0|14%19|0|1|14%19|0|2|14%19|0|3|14%19|0|4|14%19|0|5|14%19|0|6|14%19|0|7|14%19|0|8|14%19|0|9|14%19|0|10|14%19|0|11|14%19|0|12|14%19|0|13|14%19|0|14|14%19|0|15|14%19|0|16|14%19|0|17|14%19|0|18|14%19|0|19|14%"; // Saturno
        maps[8] = "0|0|0|10%0|0|1|5%0|0|2|9%0|0|3|9%0|0|4|9%0|0|5|9%0|0|6|9%0|0|7|9%0|0|8|9%0|0|9|9%0|0|10|9%0|0|11|9%0|0|12|9%0|0|13|9%0|0|14|9%0|0|15|9%0|0|16|9%0|0|17|9%0|0|18|9%0|0|19|9%1|0|0|10%1|0|1|5%1|0|2|5%1|0|3|9%1|0|4|9%1|0|5|9%1|0|6|9%1|0|7|9%1|0|8|9%1|0|9|9%1|0|10|9%1|0|11|9%1|0|12|9%1|0|13|9%1|0|14|9%1|0|15|9%1|0|16|9%1|0|17|9%1|0|18|9%1|0|19|9%2|0|0|10%2|0|1|5%2|0|2|5%2|0|3|5%2|0|4|5%2|0|5|9%2|0|6|9%2|0|7|9%2|0|8|9%2|0|9|9%2|0|10|9%2|0|11|9%2|0|12|9%2|0|13|9%2|0|14|9%2|0|15|9%2|0|16|9%2|0|17|9%2|0|18|9%2|0|19|9%3|0|0|0%3|0|1|0%3|0|2|5%3|0|3|5%3|0|4|5%3|0|5|5%3|0|6|5%3|0|7|5%3|0|8|9%3|0|9|9%3|0|10|9%3|0|11|9%3|0|12|9%3|0|13|9%3|0|14|9%3|0|15|9%3|0|16|9%3|0|17|9%3|0|18|9%3|0|19|9%4|0|0|10%4|0|1|0%4|0|2|0%4|0|3|0%4|0|4|0%4|0|5|4%4|0|6|5%4|0|7|5%4|0|8|5%4|0|9|5%4|0|10|5%4|0|11|9%4|0|12|9%4|0|13|9%4|0|14|9%4|0|15|9%4|0|16|9%4|0|17|9%4|0|18|9%4|0|19|9%5|0|0|10%5|0|1|10%5|0|2|4%5|0|3|0%5|0|4|0%5|0|5|0%5|0|6|0%5|0|7|4%5|0|8|4%5|0|9|5%5|0|10|5%5|0|11|5%5|0|12|5%5|0|13|5%5|0|14|9%5|0|15|9%5|0|16|9%5|0|17|9%5|0|18|9%5|0|19|9%6|0|0|10%6|0|1|10%6|0|2|4%6|0|3|4%6|0|4|4%6|0|5|0%6|0|6|0%6|0|7|0%6|0|8|0%6|0|9|4%6|0|10|4%6|0|11|5%6|0|12|5%6|0|13|5%6|0|14|5%6|0|15|1%6|0|16|9%6|0|17|9%6|0|18|9%6|0|19|9%7|0|0|10%7|0|1|10%7|0|2|29%7|0|3|4%7|0|4|4%7|0|5|4%7|0|6|0%7|0|7|0%7|0|8|0%7|0|9|29%7|0|10|29%7|0|11|1%7|0|12|1%7|0|13|1%7|0|14|1%7|0|15|1%7|0|16|9%7|0|17|9%7|0|18|9%7|0|19|9%8|0|0|10%8|0|1|10%8|0|2|29%8|0|3|29%8|0|4|29%8|0|5|29%8|0|6|4%8|0|7|4%8|0|8|4%8|0|9|0%8|0|10|29%8|0|11|29%8|0|12|1%8|0|13|1%8|0|14|1%8|0|15|1%8|0|16|9%8|0|17|9%8|0|18|9%8|0|19|9%9|0|0|10%9|0|1|10%9|0|2|4%9|0|3|4%9|0|4|29%9|0|5|29%9|0|6|1%9|0|7|1%9|0|8|1%9|0|9|4%9|0|10|4%9|0|11|4%9|0|12|4%9|0|13|4%9|0|14|2%9|0|15|1%9|0|16|9%9|0|17|9%9|0|18|9%9|0|19|9%10|0|0|10%10|0|1|10%10|0|2|4%10|0|3|4%10|0|4|4%10|0|5|4%10|0|6|2%10|0|7|2%10|0|8|1%10|0|9|1%10|0|10|1%10|0|11|4%10|0|12|4%10|0|13|2%10|0|14|2%10|0|15|1%10|0|16|1%10|0|17|1%10|0|18|1%10|0|19|1%11|0|0|10%11|0|1|10%11|0|2|5%11|0|3|5%11|0|4|4%11|0|5|4%11|0|6|4%11|0|7|4%11|0|8|1%11|0|9|1%11|0|10|1%11|0|11|1%11|0|12|1%11|0|13|1%11|0|14|1%11|0|15|1%11|0|16|1%11|0|17|1%11|0|18|1%11|0|19|1%12|0|0|10%12|0|1|10%12|0|2|5%12|0|3|5%12|0|4|5%12|0|5|5%12|0|6|4%12|0|7|4%12|0|8|4%12|0|9|1%12|0|10|1%12|0|11|1%12|0|12|1%12|0|13|1%12|0|14|1%12|0|15|1%12|0|16|1%12|0|17|1%12|0|18|2%12|0|19|1%13|0|0|10%13|0|1|10%13|0|2|29%13|0|3|29%13|0|4|29%13|0|5|29%13|0|6|1%13|0|7|1%13|0|8|1%13|0|9|1%13|0|10|1%13|0|11|1%13|0|12|1%13|0|13|1%13|0|14|1%13|0|15|1%13|0|16|1%13|0|17|2%13|0|18|2%13|0|19|1%14|0|0|10%14|0|1|10%14|0|2|29%14|0|3|9%14|0|4|9%14|0|5|29%14|0|6|2%14|0|7|2%14|0|8|1%14|0|9|1%14|0|10|1%14|0|11|1%14|0|12|5%14|0|13|2%14|0|14|2%14|0|15|1%14|0|16|9%14|0|17|9%14|0|18|9%14|0|19|9%15|0|0|10%15|0|1|10%15|0|2|9%15|0|3|9%15|0|4|9%15|0|5|9%15|0|6|9%15|0|7|9%15|0|8|9%15|0|9|9%15|0|10|9%15|0|11|0%15|0|12|1%15|0|13|1%15|0|14|2%15|0|15|1%15|0|16|9%15|0|17|9%15|0|18|9%15|0|19|9%16|0|0|10%16|0|1|10%16|0|2|10%16|0|3|9%16|0|4|9%16|0|5|9%16|0|6|9%16|0|7|9%16|0|8|9%16|0|9|9%16|0|10|0%16|0|11|0%16|0|12|1%16|0|13|0%16|0|14|0%16|0|15|1%16|0|16|0%16|0|17|9%16|0|18|9%16|0|19|9%17|0|0|10%17|0|1|10%17|0|2|10%17|0|3|9%17|0|4|9%17|0|5|9%17|0|6|9%17|0|7|9%17|0|8|0%17|0|9|0%17|0|10|0%17|0|11|0%17|0|12|0%17|0|13|29%17|0|14|29%17|0|15|0%17|0|16|0%17|0|17|0%17|0|18|9%17|0|19|9%18|0|0|10%18|0|1|10%18|0|2|10%18|0|3|10%18|0|4|9%18|0|5|9%18|0|6|9%18|0|7|9%18|0|8|0%18|0|9|0%18|0|10|0%18|0|11|0%18|0|12|0%18|0|13|29%18|0|14|29%18|0|15|0%18|0|16|0%18|0|17|0%18|0|18|9%18|0|19|9%19|0|0|10%19|0|1|10%19|0|2|10%19|0|3|10%19|0|4|9%19|0|5|9%19|0|6|9%19|0|7|9%19|0|8|0%19|0|9|0%19|0|10|0%19|0|11|0%19|0|12|0%19|0|13|0%19|0|14|0%19|0|15|0%19|0|16|0%19|0|17|0%19|0|18|9%19|0|19|9%";    // Caballero

        ChangeGrid();
        
        // Por defecto ponemos el color blanco
        selectedColor = BlockColor.White;

        // Por defecto el botón y el booleano estará desactivado
        isDeleting = false;
    }
    
    private void ChangeGrid()
    {
        switch (selectedLevel)
        {
            case 0:
            case 1:
            case 2:
            case 3:// 5x5
                blockSize = 0.5f;
                blockOffset = (Vector3.one * 0.5f) / 2;
                blocksChallenge = new Block[5, 1, 5];
                foundationObject.GetComponent<Renderer>().material = gridMaterials[0];
                break;
            case 4:
            case 5:
            case 6:// 10x10
                blockSize = 0.25f;
                blockOffset = (Vector3.one * 0.5f) / 4;
                blocksChallenge = new Block[10, 1, 10];
                foundationObject.GetComponent<Renderer>().material = gridMaterials[1];
                break;
            case 7:
            case 8: // 20x20
                blockSize = 0.125f;
                blockOffset = (Vector3.one * 0.5f) / 8;
                blocksChallenge = new Block[20, 1, 20];
                foundationObject.GetComponent<Renderer>().material = gridMaterials[2];
                break;
            default:
                break;
        }
    }

    private void BlockChallengeToString()
    {
        for (int i = 0; i < blocksChallenge.GetLength(0); i++)
        {
            for (int j = 0; j < blocksChallenge.GetLength(1); j++)
            {
                for (int k = 0; k < blocksChallenge.GetLength(2); k++)
                {
                    Block currentBlock = blocksChallenge[i, j, k];
                    if (currentBlock == null)
                        continue;

                    currentMap += i.ToString() + "|" +
                                j.ToString() + "|" +
                                k.ToString() + "|" +
                                ((int)currentBlock.color).ToString() + "%";
                }
            }
        }

        //Debug.Log(currentMap);
    }

    public void ToogleHintMenu()
    {
        if (hintsPanel.activeSelf)
            hintsPanel.SetActive(false);
        else
            hintsPanel.SetActive(true);
    }

    private void Update()
    {
        if (/*Hay tiempo &&*/  !isCorrectBool)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (/*EventSystem.current.IsPointerOverGameObject()*/ EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
                    return;

                RaycastHit hit;
                // Hacemos un Raycast desde la camara y el punto en la pantalla al mundo con 30m
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 30.0f))
                {
                    // Estamos hiteando un cubo con el boton de borrar
                    if (isDeleting)
                    {
                        if (hit.transform.name != "Foundation")
                        {
                            Vector3 oldCubeIndex = BlockPosition(hit.point - (hit.normal * (blockSize - 0.01f)));
                            BlockColor previousColor = blocksChallenge[(int)oldCubeIndex.x, (int)oldCubeIndex.y, (int)oldCubeIndex.z].color;
                            Destroy(blocksChallenge[(int)oldCubeIndex.x, (int)oldCubeIndex.y, (int)oldCubeIndex.z].blockTransform.gameObject);
                            blocksChallenge[(int)oldCubeIndex.x, (int)oldCubeIndex.y, (int)oldCubeIndex.z] = null;

                            playerAction = new BlockAction()
                            {
                                delete = true,
                                index = oldCubeIndex,
                                color = previousColor
                            };

                            playerActions.Push(playerAction);
                        }

                        return;
                    }

                    // Calculamos el indice del bloque en el array con la colision del rayo
                    Vector3 index = BlockPosition(hit.point);

                    // Nos ayudamos de variables para hacer comprobaciones
                    int x = (int)index.x
                        , y = (int)index.y
                        , z = (int)index.z;

                    // Si no hay un bloque en esa posicion del plano
                    if (blocksChallenge[x, y, z] == null)
                    {
                        GameObject go = CreateBlock();

                        PositionBlock(go.transform, index);
                        blocksChallenge[x, y, z] = new Block
                        {
                            blockTransform = go.transform,
                            color = selectedColor
                        };

                        playerAction = new BlockAction()
                        {
                            delete = false,
                            index = new Vector3(x, y, z),
                            color = selectedColor
                        };

                        playerActions.Push(playerAction);
                    }
                    // En caso de que lo haya, se crea uno en la cara
                    // hiteada usando la normal del bloque seleccionado
                    else
                    {
                        GameObject go = CreateBlock();

                        Vector3 newIndex = BlockPosition(hit.point + (hit.normal * blockSize));

                        blocksChallenge[(int)newIndex.x, (int)newIndex.y, (int)newIndex.z] = new Block
                        {
                            blockTransform = go.transform,
                            color = selectedColor
                        };

                        PositionBlock(go.transform, newIndex);

                        playerAction = new BlockAction()
                        {
                            delete = false,
                            index = newIndex,
                            color = selectedColor
                        };

                        playerActions.Push(playerAction);
                    }
                }

                // Método de rellenar el string currentMap
                BlockChallengeToString();

                // Método de si es correcto
                isCorrect();

                currentMap = null;

                if (Input.GetKeyDown("space"))
                {
                    isCorrectBool = true;
                }
            }
        }
        if (isCorrectBool)
        {
            isCorrectBool = false;
            // Lo ha resuelto

            if(PlayerPrefs.GetInt("SELECTEDLEVEL") < 8)
            {
                PlayerPrefs.SetInt("SELECTEDLEVEL", selectedLevel + 1);

                if (PlayerPrefs.GetInt("SELECTEDLEVEL") == PlayerPrefs.GetInt("AVAILABLELEVELS"))
                {
                    PlayerPrefs.SetInt("AVAILABLELEVELS", PlayerPrefs.GetInt("AVAILABLELEVELS") + 1);
                }
                
                playerActions = new Stack<BlockAction>();
                selectedLevel = PlayerPrefs.GetInt("SELECTEDLEVEL");

                hintImage.sprite = levelHints[selectedLevel];
                hintsPanel.SetActive(true);

                currentMap = null;
                ResetGrid();
                ChangeGrid();

                //UnityEngine.SceneManagement.SceneManager.LoadScene("Challenge");
            }
            else
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene("Menu");
            }
        }
        /*
        {
            // No hay tiempo
            currentMap = null;
        }*/

    }
    private void isCorrect()
    {
        Debug.Log("Vamo a comprobarlo");
        if (currentMap == maps[selectedLevel])
        {
            isCorrectBool = true;
            Debug.Log("Ta chido");
        }
    }
    

    // Método de creación de bloques
    private GameObject CreateBlock()
    {
        GameObject go = Instantiate(blockPrefab) as GameObject;
        go.layer = 11;
        go.transform.parent = cubesParent.transform;
        go.GetComponent<Renderer>().material = blockMaterials[(int)selectedColor];
        go.transform.localScale = Vector3.one * blockSize;

        return go;
    }

    private GameObject CreateBlock(BlockColor color)
    {
        GameObject go = Instantiate(blockPrefab) as GameObject;
        go.layer = 11;
        go.transform.parent = cubesParent.transform;
        go.GetComponent<Renderer>().material = blockMaterials[(int)color];
        go.transform.localScale = Vector3.one * blockSize;

        return go;
    }
    
    // Calculamos el indice del bloque en el array con la colision del rayo
    private Vector3 BlockPosition(Vector3 hit)
    {
        int x = (int)(hit.x / blockSize);
        int y = (int)(hit.y / blockSize);
        int z = (int)(hit.z / blockSize);

        return new Vector3(x, y, z);
    }

    public void PositionBlock(Transform t, Vector3 index)
    {
        t.position = ((index * blockSize) + blockOffset) + (foundationObject.transform.position - foundationCenter);
    }

    // Cambiamos el color de los bloques
    public void ChangeBlockGolor(int color)
    {
        selectedColor = (BlockColor)color;
        if (isDeleting == true) isDeleting = false;

        deleteButton.image.sprite = deleteButtons[0];

        for (int i = 0; i < colorButtons.Length; i++)
        {
            if (color == i)
                colorButtons[i].image.sprite = colorButtonsSprites[1];
            else
                colorButtons[i].image.sprite = colorButtonsSprites[0];
        }

        colorButton.image.sprite = colorDes;

        colorPanel.SetActive(false);
    }

    // Boton de borrar bloques
    public void ToggleDelete()
    {
        if (colorPanel.activeSelf)
        {
            colorPanel.SetActive(false);
            colorButton.image.sprite = colorDes;
        }
        isDeleting = !isDeleting;
        deleteButton.image.sprite = (!isDeleting) ? deleteButtons[0] : deleteButtons[1];
    }

    public void Undo()
    {
        if (colorPanel.activeSelf)
        {
            colorPanel.SetActive(false);
            colorButton.image.sprite = colorDes;
        }

        if (playerActions.Count != 0)
        {
            BlockAction helper = playerActions.Pop();

            // Si la última acción ha sido de colocar un bloque
            if (!helper.delete)
            {
                Destroy(blocksChallenge[(int)helper.index.x, (int)helper.index.y, (int)helper.index.z].blockTransform.gameObject);
                blocksChallenge[(int)helper.index.x, (int)helper.index.y, (int)helper.index.z] = null;
            }
            //Si la última acción ha sido de borrar
            else
            {
                GameObject go = CreateBlock(helper.color);

                blocksChallenge[(int)helper.index.x, (int)helper.index.y, (int)helper.index.z] = new Block
                {
                    blockTransform = go.transform,
                    color = selectedColor
                };

                PositionBlock(go.transform, helper.index);
            }
        }
        else
        {
            Debug.Log("Ya no queda nada...");
        }
    }

    public void ResetGrid()
    {
        if (colorPanel.activeSelf)
            colorPanel.SetActive(false);
        for (int i = 0; i < blocksChallenge.GetLength(0); i++)
        {
            for (int j = 0; j < blocksChallenge.GetLength(1); j++)
            {
                for (int k = 0; k < blocksChallenge.GetLength(2); k++)
                {
                    if (blocksChallenge[i, j, k] == null)
                        continue;
                    Destroy(blocksChallenge[i, j, k].blockTransform.gameObject);
                    blocksChallenge[i, j, k] = null;
                }
            }
        }
    }
}
