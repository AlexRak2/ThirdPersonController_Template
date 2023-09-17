
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlsHide : MonoBehaviour
{
    public GameObject controlObj;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab)) 
        {
            controlObj.SetActive(!controlObj.activeSelf);
        }
    }
}
