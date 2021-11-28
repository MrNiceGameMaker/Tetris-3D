﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamCtrl : MonoBehaviour
{
    Transform target;
    Transform rotTarget;
    Vector3 lastPos;

    float sensitivity = 0.25f;

    // Start is called before the first frame update
    void Awake()
    {
        rotTarget = transform.parent;
        target = rotTarget.transform.parent;
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(target);
        if (Input.GetMouseButtonDown(0))
        {
            lastPos = Input.mousePosition;
        }
        Orbit();
    }
    void Orbit()
    {
        if (Input.GetMouseButton(0))
        {
            Vector3 delta = Input.mousePosition - lastPos;
            float angleY = -delta.y * sensitivity;
            float anglex = -delta.x * sensitivity;

            //X axis
            Vector3 angels = rotTarget.transform.eulerAngles;
            angels.x += angleY;
            angels.x = ClampAngle(angels.x, -85, 85);

            rotTarget.transform.eulerAngles = angels;


            //Y Axis
            target.RotateAround(target.position, Vector3.up, anglex);
            lastPos = Input.mousePosition;
        }
    }
    float ClampAngle(float angle, float from, float to)
    {
        //doest allow the camera go to the very top or bottom 
        if (angle < 0) angle = 360 + angle;
        if (angle > 180f) return Mathf.Max(angle, 360 + from);

        return Mathf.Min(angle, to);
    }
}
