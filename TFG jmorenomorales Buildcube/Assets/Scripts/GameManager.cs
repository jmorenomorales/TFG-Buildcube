using System;
using System.Collections.Generic;
using UnityEngine;

public class Block
{
    public Transform blockTransform;
}

public enum BlockColor
{
    White = 0,
    Red = 1,
    Green = 2,
    Blue = 3
}

public class GameManager : MonoBehaviour
{
    // Definimos la escala de los bloques
    private float blockSize = 0.25f;

    public Block[,,] blocks = new Block[20, 20, 20];
    public GameObject blockPrefab;

    public BlockColor selectedColor;
    public Material[] blockMaterials;

    // Guardamos el objeto base (plano)
    private GameObject foundationObject;
    // Damos un offset a los cubos ya que su pivote esta en el centro
    private Vector3 blockOffset;
    // Establecemos el centro del plano
    private Vector3 foundationCenter = new Vector3(1.25f, 0, 1.25f);

    private void Start()
    {
        // Buscamos en la escena el objeto con nombre "Foundation"
        foundationObject = GameObject.Find("Foundation");
        blockOffset = (Vector3.one * 0.5f) / 4;

        selectedColor = BlockColor.White;
        
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            // Hacemos un Raycast desde la camara y el punto en la pantalla al mundo con 30m
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 30.0f))
            {

                Vector3 index = BlockPosition(hit.point);

                int x = (int)index.x
                    , y = (int)index.y
                    , z = (int)index.z;

                if (blocks[x, y, z] == null)
                {
                    GameObject go = CreateBlock();
                    go.transform.localScale = Vector3.one * blockSize;

                    PositionBlock(go.transform, index);
                    blocks[x, y, z] = new Block
                    {
                        blockTransform = go.transform
                    };
                }
                else
                {
                    GameObject go = CreateBlock();
                    go.transform.localScale = Vector3.one * blockSize;

                    Vector3 newIndex = BlockPosition(hit.point + (hit.normal * blockSize));
                    PositionBlock(go.transform, newIndex);
                }
            }
        }
    }

    private GameObject CreateBlock()
    {
        GameObject go = Instantiate(blockPrefab) as GameObject;
        go.GetComponent<Renderer>().material = blockMaterials[(int)selectedColor];

        return go;
    }

    private Vector3 BlockPosition(Vector3 hit)
    {
        int x = (int)(hit.x / blockSize);
        int y = (int)(hit.y / blockSize);
        int z = (int)(hit.z / blockSize);

        return new Vector3(x,y,z);
    }

    private void PositionBlock(Transform t, Vector3 index)
    {
        t.position = ((index * blockSize) + blockOffset) + (foundationObject.transform.position - foundationCenter);
    }

    public void ChangeBlockGolor(int color)
    {
        selectedColor = (BlockColor)color;
    }
}
