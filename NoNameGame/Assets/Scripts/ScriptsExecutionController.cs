using System.Threading;
using UnityEngine;

[DefaultExecutionOrder((int)ScriptsExecutionOrder.ExecutionOrder.scriptsExecutionController)]
public class ScriptsExecutionController : MonoBehaviour
{
    Constants CONS;
    Variables VARS;

    GameObject gameManager;

    private void Start()
    {
        gameManager = GameObject.Find("GameManager");

        CONS = gameManager.GetComponent<Constants>();
        VARS = gameManager.GetComponent<Variables>();
    }
}
