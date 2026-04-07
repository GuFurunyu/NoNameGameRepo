using System.Collections.Generic;
using System.Threading.Tasks.Sources;
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
    List<GameObject> savePoints = new List<GameObject>();

    Transform catTransform;

    GameObject catIniPositionPoint;

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

        #region ImportConstants
        savePoints = CONS.savePoints;
        catTransform = CONS.catTransform;
        catIniPositionPoint = CONS.catIniPositionPoint;
        maxEnergy = CONS.maxEnergy;
        storedVoidBlocksEmpty = CONS.storedVoidBlocksEmpty;
        #endregion

        #region ImportReferenceVariables
        #endregion

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
        #region ImportValueVariables
        #endregion

        if (VARS.IsToDie)
        {
            Die();

            VARS.IsToDie = false;
        }
    }
    void Die()
    {
        ////turnIntoVoid
        //storedVoidBlocks[curStoredVoidBlockIndex].transform.position = new Vector3
        //    (Mathf.RoundToInt(catTransform.position.x), Mathf.RoundToInt(catTransform.position.y), Mathf.RoundToInt(catTransform.position.z));

        ////voidTempChildToCurPlaneEmpty
        //storedVoidBlocks[curStoredVoidBlockIndex].transform.SetParent(VARS.curPlaneEmpty.transform, true);

        ////activateVoidBlock
        //storedVoidBlocks[curStoredVoidBlockIndex].SetActive(true);

        //curStoredVoidBlockIndex++;

        //clearKey
        if (VARS.IsCarryingAKey)
        {
            VARS.curCarriedKey.transform.parent = VARS.curCarriedKeyIniParent.transform;
            VARS.curCarriedKey.transform.localPosition = VARS.curCarriedKeyIniLocalPosition;

            VARS.curCarriedKey = null;

            VARS.IsCarryingAKey = false;
        }

        //catTransform.position = VARS.catIniPosition;
        catTransform.position = catIniPositionPoint.transform.position;

        catIniPositionPoint.transform.position = savePoints[VARS.curActivatedSavePointIndex].transform.position - VARS.roomStableForwards[VARS.curActivatedSavePointRoomIndex] * 0.1f;

        //VARS.curEnergy = maxEnergy;
        //VARS.curEnergy = 0.1f;
        //UFL.SetCurEnergy(0.1f);
        VARS.curEnergy = 0.1f;
        //UFL.SetCurTargetEnergy(0.1f);
        VARS.curTargetEnergy = 0.1f;
        //VARS.horCurSpeed = 0;
        //UFL.SetHorCurSpeed(0);
        VARS.horCurSpeed = 0;
        //VARS.verCurSpeed = 0;
        //UFL.SetVerCurSpeed(0);
        VARS.verCurSpeed = 0;

        VARS.IsToInitializeSight = true;

        VARS.outIniRotationStartTime = 0.1f;

        //strawBerry
        //isCarryingStrawberries = false;

        //for (int i = 0; i < carriedStrawberries.Count; i++)
        //{
        //    carriedStrawberries[i].transform.position = carriedStrawberriesIniPositions[i];
        //}

        //carriedStrawberries.Clear();
        //carriedStrawberriesIniPositions.Clear();

        VARS.IsToLoseCarriedStrawberries = true;

        //VARS.IsIntoNewRoom = true;

        VARS.IsInNewRoomCameraManagerResetOver = false;
        VARS.IsInNewRoomCatRotateResetOver = false;

        VARS.IsToWriteCatWorldData = true;
    }
}
