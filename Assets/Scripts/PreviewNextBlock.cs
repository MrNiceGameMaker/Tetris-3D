using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreviewNextBlock : MonoBehaviour
{
    public static PreviewNextBlock instance;
    public GameObject[] previewBloacks;

    GameObject currentActive;
    private void Awake()
    {
        instance = this;
    }

    public void ShowPreview(int index)
    {
        Destroy(currentActive);

        currentActive = Instantiate(previewBloacks[index], transform.position, Quaternion.identity) as GameObject;
    }
}
