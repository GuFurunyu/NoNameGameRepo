using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[DefaultExecutionOrder((int)ScriptsExecutionOrder.ExecutionOrder.test)]
public class Test : MonoBehaviour
{
    Constants CONS;
    Variables VARS;
    ScriptsExecutionController SEC;

    GameObject gameManager;

    void Start()
    {
        gameManager = GameObject.Find("GameManager");

        CONS = gameManager.GetComponent<Constants>();
        VARS = gameManager.GetComponent<Variables>();
        SEC = gameManager.GetComponent<ScriptsExecutionController>();
    }

    //void Update()
    //{
    //    
    //}
}
