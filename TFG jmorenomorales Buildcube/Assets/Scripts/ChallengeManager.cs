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

    private void Start()
    {
        Instance = this;
        // Buscamos en la escena el objeto con nombre "Foundation"
        foundationObject = GameObject.Find("Foundation");

        playerActions = new Stack<BlockAction>();

        GridSettingsChallenge(0);

        // Por defecto ponemos el color blanco
        selectedColor = BlockColor.White;
        colorButtons[0].image.sprite = colorButtonsSprites[1];

        // Por defecto el botón y el booleano estará desactivado
        isDeleting = false;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject() /*EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId)*/)
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


                Debug.Log(x + ", " + y + ", " + z);


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

    public void GridSettingsChallenge(int gridtype)
    {
        switch (gridtype)
        {
            case 0: // 5x5
                blockSize = 0.5f;
                blockOffset = (Vector3.one * 0.5f) / 2;
                blocksChallenge = new Block[5, 25, 5];
                foundationObject.GetComponent<Renderer>().material = gridMaterials[0];
                break;
            case 1: // 10x10
                blockSize = 0.25f;
                blockOffset = (Vector3.one * 0.5f) / 4;
                blocksChallenge = new Block[10, 50, 10];
                foundationObject.GetComponent<Renderer>().material = gridMaterials[1];
                break;
            case 2: // 20x20
                blockSize = 0.125f;
                blockOffset = (Vector3.one * 0.5f) / 8;
                blocksChallenge = new Block[20, 100, 200];
                foundationObject.GetComponent<Renderer>().material = gridMaterials[2];
                break;
            default:
                break;
        }
    }
}
