using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrisBlock : MonoBehaviour
{
    float prevTime;
    float fallTime = 1f;
    Vector3 mouseTurn;

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
        mouseTurn.x = Input.GetAxis("Mouse X");
        mouseTurn.y = Input.GetAxis("Mouse Y");
        //makes the block fall 1 unit every 1 sec(falltime)
        if (Time.time - prevTime > fallTime)
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
        if ((Input.GetKeyDown(KeyCode.LeftArrow)) || ((mouseTurn.x < 0f) && (Input.GetMouseButtonDown(1))))
        {
            SetInput(Vector3.left);
        }
        if (Input.GetKeyDown(KeyCode.RightArrow) || ((mouseTurn.x > 0f) && (Input.GetMouseButtonDown(1))))
        {
            SetInput(Vector3.right);
        }
        if (Input.GetKeyDown(KeyCode.UpArrow) || ((mouseTurn.y > 0f) && (Input.GetMouseButtonDown(1))))
        {
            SetInput(Vector3.forward);
        }
        if (Input.GetKeyDown(KeyCode.DownArrow) || ((mouseTurn.y < 0f) && (Input.GetMouseButtonDown(1))))
        {
            SetInput(Vector3.back);
        }
        if (Input.GetKeyDown(KeyCode.Space) || ((Input.GetMouseButtonDown(1)) && (Input.GetMouseButtonDown(0))))
        {
            SetSpeed();
        }
        //rotate with the W,S,A,D
        if (Input.GetKeyDown(KeyCode.A) || ((mouseTurn.x < 0f) && (Input.GetMouseButtonDown(2))))
        {
            SetRotationInput(new Vector3(0,90,0));
        }
        if (Input.GetKeyDown(KeyCode.D) || ((mouseTurn.x > 0f) && (Input.GetMouseButtonDown(2))))
        {
            SetRotationInput(new Vector3(0, -90, 0));
        }
        if (Input.GetKeyDown(KeyCode.W) || ((mouseTurn.y < 0f) && (Input.GetMouseButtonDown(2))))
        {
            SetRotationInput(new Vector3(90, 0, 0));
        }
        if (Input.GetKeyDown(KeyCode.S) || ((mouseTurn.y > 0f) && (Input.GetMouseButtonDown(2))))
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
