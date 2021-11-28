using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayField : MonoBehaviour
{
    public static PlayField instance;
    [Header("Grid Size")]
    public int gridSizeX, gridSizeY, gridSizeZ;

    [Header("Blocks")]
    public GameObject[] blockList;
    public GameObject[] ghostList;

    [Header("Playfield Visuals")]
    public GameObject bottomPlane;
    public GameObject N, S, W, E;

    public Transform[,,] theGrid;


    int randomIndex;
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        theGrid = new Transform[gridSizeX, gridSizeY, gridSizeZ];
        CalculatePreview();
        SpawnNewBlock();
    }
    //rounds the numbers to nearst full number
    public Vector3 Round(Vector3 vec)
    {
        return new Vector3(Mathf.RoundToInt(vec.x), Mathf.RoundToInt(vec.y), Mathf.RoundToInt(vec.z));
    }
    //checks if the object is in the field
    public bool CheckInsideGrid(Vector3 pos)
    {
        return ((int)pos.x >= 0 && (int)pos.x < gridSizeX && (int)pos.z >= 0 && (int)pos.z < gridSizeZ && (int)pos.y >= 0);
    }

    public void UpdateGrid(TetrisBlock block)
    {
        //delete possible parent objects
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int z = 0; z < gridSizeZ; z++)
            {
                for (int y = 0; y < gridSizeY; y++)
                {
                    if(theGrid[x,y,z] != null)
                    {
                        if (theGrid[x, y, z].parent == block.transform)
                        {
                            theGrid[x, y, z] = null;
                        }
                    }

                }
            }
        }

        //fill in all child objects
        foreach(Transform child in block.transform)
        {
            Vector3 pos = Round(child.position);
            if(pos.y < gridSizeY)
            {
                theGrid[(int)pos.x, (int)pos.y, (int)pos.z] = child;
            }
        }
    }

    public Transform GetTransfromOnGridPos(Vector3 pos)
    {
        if(pos.y > gridSizeY - 1)
        {
            return null;
        }
        else
        {
            return theGrid[(int)pos.x, (int)pos.y, (int)pos.z];
        }
    }

    public void SpawnNewBlock()
    {
        Vector3 spwanPoint = new Vector3((int)(transform.position.x + (float)gridSizeX / 2), 
                                        (int)transform.position.y + gridSizeY, 
                                        (int)(transform.position.z + (float)gridSizeZ / 2));

        //spawn the block
        GameObject newBlock = Instantiate(blockList[randomIndex], spwanPoint, Quaternion.identity) as GameObject;
        //Ghost
        GameObject newGhost = Instantiate(ghostList[randomIndex], spwanPoint, Quaternion.identity) as GameObject;
        newGhost.GetComponent<GhostBlock>().SetParent(newBlock);

        CalculatePreview();
        //sends what block should be in the preview
        PreviewNextBlock.instance.ShowPreview(randomIndex);
    }

    public void CalculatePreview()
    {
        randomIndex = Random.Range(0, blockList.Length);

    }
    public void DeleteLayer()
    {
        int layersCleard = 0;
        for (int y = gridSizeY-1; y >= 0; y--)
        {
            //check full layer
            if (CheckFullLayer(y))
            {
                layersCleard++;
                //delete all blocks in a layer
                DeleteLayerAt(y);
                //move all down by 1
                MoveAllLayerDown(y);
            }
        }
        if(layersCleard > 0)
        {
            GameManager.instance.LeayerCleared(layersCleard);
        }
    }
    
    bool CheckFullLayer(int y)
    {
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int z = 0; z < gridSizeZ; z++)
            {
                if(theGrid[x,y,z] == null)
                {
                    return false;
                }
            }
        }
        return true;
    }

    void DeleteLayerAt(int y)
    {
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int z = 0; z < gridSizeZ; z++)
            {
                Destroy(theGrid[x, y, z].gameObject);
                theGrid[x, y, z] = null;
            }
        }
    }

    void MoveAllLayerDown(int y)
    {
        for (int i = y; i < gridSizeY; i++)
        {
            MoveOneLayerDown(i);
        }
    }

    void MoveOneLayerDown(int y)
    {
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int z = 0; z < gridSizeZ; z++)
            {
                if(theGrid[x,y,z]!= null)
                {
                    theGrid[x, y - 1, z] = theGrid[x, y, z];
                    theGrid[x, y, z] = null;
                    theGrid[x, y - 1, z].position += Vector3.down;

                }
            }
        }
    }
    //unity function that draw gizmos that are also pickable and always drawn. 
    private void OnDrawGizmos()
    {
        if(bottomPlane != null)
        {
            //resize bottom plane
            Vector3 scaler = new Vector3((float)gridSizeX / 10, 1, (float)gridSizeZ / 10);
            bottomPlane.transform.localScale = scaler;

            //repostion bottom plane
            bottomPlane.transform.position = new Vector3(transform.position.x + (float)gridSizeX / 2,
                                                         transform.position.y, 
                                                         transform.position.z + (float)gridSizeZ / 2);
            //makes the grid resize in the correct size
            bottomPlane.GetComponent<MeshRenderer>().sharedMaterial.mainTextureScale = new Vector2(gridSizeX, gridSizeZ);
        }
        if (N != null)
        {
            //resize wall plane
            Vector3 scaler = new Vector3((float)gridSizeX / 10, 1, (float)gridSizeY / 10);
            N.transform.localScale = scaler;

            //repostion wall plane
            N.transform.position = new Vector3(transform.position.x + (float)gridSizeX /2,
                                                         transform.position.y + (float)gridSizeY /2,
                                                         transform.position.z + gridSizeZ);
            //makes the grid resize in the correct size
            N.GetComponent<MeshRenderer>().sharedMaterial.mainTextureScale = new Vector2(gridSizeX, gridSizeY);
        }
        if (S != null)
        {
            //resize wall plane
            Vector3 scaler = new Vector3((float)gridSizeX / 10, 1, (float)gridSizeY / 10);
            S.transform.localScale = scaler;

            //repostion wall plane
            S.transform.position = new Vector3(transform.position.x + (float)gridSizeX / 2,
                                                         transform.position.y + (float)gridSizeY / 2,
                                                         transform.position.z );
            //makes the grid resize in the correct size
            //S.GetComponent<MeshRenderer>().sharedMaterial.mainTextureScale = new Vector2(gridSizeX, gridSizeY);
        }
        if (E != null)
        {
            //resize wall plane
            Vector3 scaler = new Vector3((float)gridSizeZ / 10, 1, (float)gridSizeY / 10);
            E.transform.localScale = scaler;

            //repostion wall plane
           E.transform.position = new Vector3(transform.position.x + gridSizeX,
                                                         transform.position.y + (float)gridSizeY / 2,
                                                         transform.position.z + (float)gridSizeZ/2);
            //makes the grid resize in the correct size
            E.GetComponent<MeshRenderer>().sharedMaterial.mainTextureScale = new Vector2(gridSizeZ, gridSizeY);
        }
        if (W != null)
        {
            //resize wall plane
            Vector3 scaler = new Vector3((float)gridSizeZ / 10, 1, (float)gridSizeY / 10);
            W.transform.localScale = scaler;

            //repostion wall plane
            W.transform.position = new Vector3(transform.position.x ,
                                                          transform.position.y + (float)gridSizeY / 2,
                                                          transform.position.z + (float)gridSizeZ / 2);
            //makes the grid resize in the correct size
            //W.GetComponent<MeshRenderer>().sharedMaterial.mainTextureScale = new Vector2(gridSizeZ, gridSizeY);
        }

    }
}
