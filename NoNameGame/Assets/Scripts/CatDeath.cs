using UnityEngine;

[DefaultExecutionOrder((int)ScriptsExecutionOrder.ExecutionOrder.catDeath)]
public class CatDeath : MonoBehaviour
{
    Constants CONS;
    Variables VARS;
    UniversalFunctionsLibrary UFL;
    ScriptsExecutionController SEC;

    GameObject gameManager;

    //storedVoidBlocks
    GameObject[] storedVoidBlocks = new GameObject[512];
    int curStoredVoidBlockIndex;

    Transform tempTransform;

    #region ConstantsUsed
    Transform catTransform;

    float maxEnergy;

    GameObject storedVoidBlocksEmpty;
    #endregion

    #region VariablesUsed
    #endregion

    void Start()
    {
        gameManager = GameObject.Find("GameManager");

        CONS = gameManager.GetComponent<Constants>();
        VARS = gameManager.GetComponent<Variables>();
        UFL = gameManager.GetComponent<UniversalFunctionsLibrary>();
        SEC = gameManager.GetComponent<ScriptsExecutionController>();

        catTransform = CONS.catTransform;
        maxEnergy = CONS.maxEnergy;
        storedVoidBlocksEmpty = CONS.storedVoidBlocksEmpty;

        VARS.catIniPosition = catTransform.position;

        //loadStoredVoidBlocks
        tempTransform = storedVoidBlocksEmpty.transform;
        for (int i = 0; i < 512; i++)
        {
            storedVoidBlocks[i] = tempTransform.GetChild(i).gameObject;
        }
    }

    void Update()
    {
        if (VARS.isToDie)
        {
            Die();

            VARS.isToDie = false;
        }
    }
    void Die()
    {
        //turnIntoVoid
        storedVoidBlocks[curStoredVoidBlockIndex].transform.position = new Vector3
            (Mathf.RoundToInt(catTransform.position.x), Mathf.RoundToInt(catTransform.position.y), Mathf.RoundToInt(catTransform.position.z));

        //voidTempChildToCurPlaneEmpty
        storedVoidBlocks[curStoredVoidBlockIndex].transform.SetParent(VARS.curPlaneEmpty.transform, true);

        //activateVoidBlock
        storedVoidBlocks[curStoredVoidBlockIndex].SetActive(true);

        curStoredVoidBlockIndex++;

        catTransform.position = VARS.catIniPosition;

        //VARS.curEnergy = maxEnergy;
        //VARS.curEnergy = 0.1f;
        UFL.SetCurEnergy(0.1f);
        //VARS.horCurSpeed = 0;
        UFL.SetHorCurSpeed(0);
        //VARS.verCurSpeed = 0;
        UFL.SetVerCurSpeed(0);

        VARS.isToInitializeSight = true;

        VARS.outIniRotationStartTime = 0.1f;

        //strawBerry
        //isCarryingStrawberries = false;

        //for (int i = 0; i < carriedStrawberries.Count; i++)
        //{
        //    carriedStrawberries[i].transform.position = carriedStrawberriesIniPositions[i];
        //}

        //carriedStrawberries.Clear();
        //carriedStrawberriesIniPositions.Clear();

        VARS.isToLoseCarriedStrawberries = true;

        VARS.isIntoNewRoom = true;
    }
}
