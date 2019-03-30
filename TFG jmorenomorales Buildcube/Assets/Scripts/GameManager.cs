using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Block
{
    public Transform blockTransform;
    public BlockColor color;
}

public enum BlockColor
{
    White = 0,
    Grey = 1,
    Black = 2,
    LightRed = 3,
    Red = 4,
    DarkRed = 5,
    LightPink = 6,
    Pink = 7,
    DarkPink = 8,
    LightPurple = 9,
    Purple = 10,
    DarkPurple = 11,
    LightBlue = 12,
    Blue = 13,
    DarkBlue = 14,
    LightFancyBlue = 15,
    FancyBlue = 16,
    DarkFancyBlue = 17,
    LightTurquoise = 18,
    Turquoise = 19,
    DarkTurquoise = 20,
    LightGreen = 21,
    Green = 22,
    DarkGreen = 23,
    LightYellow = 24,
    Yellow = 25,
    DarkYellow = 26,
    LightOrange = 27,
    Orange = 28,
    DarkOrange = 29,
}

public struct BlockAction
{
    public bool delete;
    public Vector3 index;
    public BlockColor color;
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { set; get; }

    // Definimos la escala de los bloques
    private float blockSize = 0.125f;

    // Array contenedor de los bloques
    public Block[,,] blocks = new Block[20, 100, 20];
    // Prefab de los bloques
    public GameObject blockPrefab;

    // Enum del color seleccionado
    public BlockColor selectedColor;
    // Array contenedor de los materiales de los bloques
    public Material[] blockMaterials;

    // Guardamos el objeto base (plano)
    private GameObject foundationObject;
    // Damos un offset a los cubos ya que su pivote esta en el centro
    private Vector3 blockOffset;
    // Establecemos el centro del plano
    private Vector3 foundationCenter = new Vector3(1.25f, 0, 1.25f);
    // Comprobamos si el botón de borrar está activo
    private bool isDeleting;

    public Button deleteButton, colorButton;
    public Sprite[] deleteButtons;

    public GameObject colorPanel;
    public Button[] colorButtons;
    public Sprite[] colorButtonsSprites;
    public Sprite colorDes;

    private BlockAction previewAction;

    private void Start()
    {
        Instance = this;
        // Buscamos en la escena el objeto con nombre "Foundation"
        foundationObject = GameObject.Find("Foundation");
        blockOffset = (Vector3.one * 0.5f) / 8;

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
                        BlockColor previousColor = blocks[(int)oldCubeIndex.x, (int)oldCubeIndex.y, (int)oldCubeIndex.z].color;
                        Destroy(blocks[(int)oldCubeIndex.x, (int)oldCubeIndex.y, (int)oldCubeIndex.z].blockTransform.gameObject);
                        blocks[(int)oldCubeIndex.x, (int)oldCubeIndex.y, (int)oldCubeIndex.z] = null;

                        previewAction = new BlockAction()
                        {
                            delete = true,
                            index = oldCubeIndex,
                            color = previousColor
                        };
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
                if (blocks[x, y, z] == null)
                {
                    GameObject go = CreateBlock();

                    PositionBlock(go.transform, index);
                    blocks[x, y, z] = new Block
                    {
                        blockTransform = go.transform,
                        color = selectedColor
                    };

                    previewAction = new BlockAction()
                    {
                        delete = false,
                        index = new Vector3(x,y,z),
                        color = selectedColor
                    };
                }
                // En caso de que lo haya, se crea uno en la cara
                // hiteada usando la normal del bloque seleccionado
                else
                {
                    GameObject go = CreateBlock();

                    Vector3 newIndex = BlockPosition(hit.point + (hit.normal * blockSize));

                    blocks[(int)newIndex.x, (int)newIndex.y, (int)newIndex.z] = new Block
                    {
                        blockTransform = go.transform,
                        color = selectedColor
                    };

                    PositionBlock(go.transform, newIndex);

                    previewAction = new BlockAction()
                    {
                        delete = false,
                        index = newIndex,
                        color = selectedColor
                    };
                }
            }
        }
    }

    // Método de creación de bloques
    private GameObject CreateBlock()
    {
        GameObject go = Instantiate(blockPrefab) as GameObject;
        go.GetComponent<Renderer>().material = blockMaterials[(int)selectedColor];
        go.transform.localScale = Vector3.one * blockSize;

        return go;
    }

    public GameObject CreateBlock(int x, int y, int z, Block b)
    {
        GameObject go = Instantiate(blockPrefab) as GameObject;
        go.GetComponent<Renderer>().material = blockMaterials[(int)b.color];
        go.transform.localScale = Vector3.one * blockSize;

        b.blockTransform = go.transform;
        blocks[x, y, z] = b;

        PositionBlock(b.blockTransform, new Vector3(x,y,z));

        return go;
    }

    // Calculamos el indice del bloque en el array con la colision del rayo
    private Vector3 BlockPosition(Vector3 hit)
    {
        int x = (int)(hit.x / blockSize);
        int y = (int)(hit.y / blockSize);
        int z = (int)(hit.z / blockSize);

        return new Vector3(x,y,z);
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

        for(int i = 0; i < colorButtons.Length; i++)
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
            
        if (previewAction.delete)
        {
            //Spawn it back
            
            GameObject go = CreateBlock();

            blocks[(int)previewAction.index.x, (int)previewAction.index.y, (int)previewAction.index.z] = new Block
            {
                blockTransform = go.transform,
                color = selectedColor
            };

            PositionBlock(go.transform, previewAction.index);

            previewAction = new BlockAction()
            {
                delete = false,
                index = previewAction.index,
                color = previewAction.color
            };
        }
        else
        {
            //Delete the block
            
            Destroy(blocks[(int)previewAction.index.x, (int)previewAction.index.y, (int)previewAction.index.z].blockTransform.gameObject);
            blocks[(int)previewAction.index.x, (int)previewAction.index.y, (int)previewAction.index.z] = null;

            previewAction = new BlockAction()
            {
                delete = true,
                index = previewAction.index,
                color = previewAction.color
            };
        }
    }

    public void ResetGrid()
    {
        if (colorPanel.activeSelf)
            colorPanel.SetActive(false);
        for (int i = 0; i< 20; i++)
        {
            for (int j = 0; j < 100; j++)
            {
                for (int k = 0; k < 20; k++)
                {
                    if (blocks[i, j, k] == null)
                        continue;
                    Destroy(blocks[i, j, k].blockTransform.gameObject);
                    blocks[i, j, k] = null;
                }
            }
        }
    }
}