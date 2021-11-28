using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonInput : MonoBehaviour
{
    public static ButtonInput instance;

    public GameObject[] rotateCanvases;
    public GameObject moveCanvas;

    GameObject activeBlock;
    TetrisBlock activeTetris;

    bool moveIsOn = true;
    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        SetInput();
    }
    void RepositionToActiveBlock()
    {
        if(activeBlock != null)
        {
            transform.position = activeBlock.transform.position;
        }
    }

    public void SetActiveBlock(GameObject block, TetrisBlock tetris)
    {
        activeBlock = block;
        activeTetris = tetris;
    }

    // Update is called once per frame
    void Update()
    {
        RepositionToActiveBlock();
    }

    public void MoveBlock(string direction)
    {
        if(activeBlock != null)
        {
            if(direction == "left")
            {
                activeTetris.SetInput(Vector3.left);
            }
            if (direction == "right")
            {
                activeTetris.SetInput(Vector3.right);
            }
            if (direction == "forward")
            {
                activeTetris.SetInput(Vector3.forward);
            }
            if (direction == "back")
            {
                activeTetris.SetInput(Vector3.back);
            }
        }
    }
    public void RotateBloack(string rotation)
    {
        if(activeBlock != null)
        {
            //X rotation
            if(rotation == "posX")
            {
                activeTetris.SetRotationInput(new Vector3(90, 0, 0));
            }
            if (rotation == "negX")
            {
                activeTetris.SetRotationInput(new Vector3(-90, 0, 0));
            }
            //Y rotation
            if (rotation == "posY")
            {
                activeTetris.SetRotationInput(new Vector3(0, 90, 0));
            }
            if (rotation == "negY")
            {
                activeTetris.SetRotationInput(new Vector3(0, -90, 0));
            }
            //Z rotation
            if (rotation == "posZ")
            {
                activeTetris.SetRotationInput(new Vector3(0, 0, 90));
            }
            if (rotation == "negZ")
            {
                activeTetris.SetRotationInput(new Vector3(0, 0, -90));
            }
        }
    }

    public void SwitchInput()
    {
        moveIsOn = !moveIsOn;
        SetInput();
    }
    public void SetInput()
    {
        moveCanvas.SetActive(moveIsOn);
        foreach (GameObject c in rotateCanvases)
        {
            c.SetActive(!moveIsOn);
        }
    }

    public void SetHighSpeed()
    {
        activeTetris.SetSpeed();
    }
}
