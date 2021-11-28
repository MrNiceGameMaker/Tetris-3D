using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrisBlock : MonoBehaviour
{
    float prevTime;
    float fallTime = 1f;


    // Start is called before the first frame update
    void Start()
    {
        ButtonInput.instance.SetActiveBlock(gameObject, this);
        fallTime = GameManager.instance.ReadFallSpeed();
        if (!CheckValidMove())
        {
            GameManager.instance.SetGameIsOver();
        }
    }

    // Update is called once per frame
    void Update()
    {
        //makes the block fall 1 unit every 1 sec(falltime)
        if(Time.time - prevTime > fallTime)
        {
            transform.position += Vector3.down;

            //checks if the block reached the bottom
            if (!CheckValidMove())
            {
                transform.position += Vector3.up;
                PlayField.instance.DeleteLayer();
                enabled = false;
                if (!GameManager.instance.ReadGameIsOver())
                {
                    PlayField.instance.SpawnNewBlock();
                }
                //create new tetris block
                

            }
            prevTime = Time.time;
        }
        else
        {
            //update the grid
            PlayField.instance.UpdateGrid(this);
        }

        //move with the keyboard
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            SetInput(Vector3.left);
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            SetInput(Vector3.right);
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            SetInput(Vector3.forward);
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            SetInput(Vector3.back);
        }
        //rotate with the W,S,A,D
        if (Input.GetKeyDown(KeyCode.A))
        {
            SetRotationInput(new Vector3(0,90,0));
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            SetRotationInput(new Vector3(0, -90, 0));
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            SetRotationInput(new Vector3(90, 0, 0));
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            SetRotationInput(new Vector3(-90, 0, 0));
        }

    }


    public void SetInput(Vector3 direction)
    {
        transform.position += direction;
        if (!CheckValidMove())
        {
            transform.position -= direction;
        }
        else
        {
            PlayField.instance.UpdateGrid(this);
        }
    }

    public void SetRotationInput(Vector3 rotation)
    {
        transform.Rotate(rotation, Space.World);
        if (!CheckValidMove())
        {
            transform.Rotate(-rotation, Space.World);

        }
        else
        {
            PlayField.instance.UpdateGrid(this);
        }
    }
    bool CheckValidMove()
    {
        //calls the function round from playfield
        foreach(Transform child in transform)
        {
            Vector3 pos = PlayField.instance.Round(child.position);
            if (!PlayField.instance.CheckInsideGrid(pos))
            {
                return false;
            }
        }
        foreach(Transform child in transform)
        {
            Vector3 pos = PlayField.instance.Round(child.position);
            Transform t = PlayField.instance.GetTransfromOnGridPos(pos);
            if(t!=null && t.parent != transform)
            {
                return false;
            }
        }
        return true;
    }

    public void SetSpeed()
    {
        fallTime = 0.1f;
    }
}
