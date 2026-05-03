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
    List<GameObject> curCarriedFragments = new List<GameObject>();
    List<int> curCarriedFragmentFaceIndexes = new List<int>();
    List<int> curCarriedFragmentIndexes = new List<int>();
    List<GameObject> curCarriedFragmentIniParents = new List<GameObject>();
    List<Vector3> curCarriedFragmentIniLocalPositions = new List<Vector3>();

    List<int> curToBeEmbededFragmentIndexes = new List<int>();
    List<Vector3> curToBeEmbededFragmentPositions = new List<Vector3>();

    List<GameObject> curBlocks = new List<GameObject>();
    List<TileData> curBlockTileDatas = new List<TileData>();
    List<Vector3> curCoordVectors = new List<Vector3>();

    //storedBlocks
    GameObject[] storedSandBlocks = new GameObject[512];
    GameObject[] storedWaterBlocks = new GameObject[512];
    GameObject[] storedAcidBlocks = new GameObject[512];
    GameObject[] storedVaporBlocks = new GameObject[512];
    GameObject[] storedGasBlocks = new GameObject[512];
    GameObject[] storedElectricMistBlocks = new GameObject[512];
    GameObject[] storedLightElectricMistBlocks = new GameObject[512];
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
        curCarriedFragments = VARS.curCarriedFragments;
        curCarriedFragmentFaceIndexes = VARS.curCarriedFragmentFaceIndexes;
        curCarriedFragmentIndexes = VARS.curCarriedFragmentIndexes;
        curCarriedFragmentIniParents = VARS.curCarriedFragmentIniParents;
        curCarriedFragmentIniLocalPositions = VARS.curCarriedFragmentIniLocalPositions;
        curToBeEmbededFragmentIndexes = VARS.curToBeEmbededFragmentIndexes;
        curToBeEmbededFragmentPositions = VARS.curToBeEmbededFragmentPositions;
        curBlocks = VARS.curBlocks;
        curBlockTileDatas = VARS.curBlockTileDatas;
        curCoordVectors = VARS.curCoordVectors;
        storedSandBlocks = VARS.storedSandBlocks;
        storedWaterBlocks = VARS.storedWaterBlocks;
        storedAcidBlocks = VARS.storedAcidBlocks;
        storedVaporBlocks = VARS.storedVaporBlocks;
        storedGasBlocks = VARS.storedGasBlocks;
        storedElectricMistBlocks = VARS.storedElectricMistBlocks;
        storedLightElectricMistBlocks = VARS.storedLightElectricMistBlocks;
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
        UFL.DebugLog("die");

        ////turnIntoVoid
        //storedVoidBlocks[curStoredVoidBlockIndex].transform.position = new Vector3
        //    (Mathf.RoundToInt(catTransform.position.x), Mathf.RoundToInt(catTransform.position.y), Mathf.RoundToInt(catTransform.position.z));

        ////voidTempChildToCurPlaneEmpty
        //storedVoidBlocks[curStoredVoidBlockIndex].transform.SetParent(VARS.curPlaneEmpty.transform, true);

        ////activateVoidBlock
        //storedVoidBlocks[curStoredVoidBlockIndex].SetActive(true);

        //curStoredVoidBlockIndex++;

        //resetMovableBlockLocalPositions
        for (int i = 0; i < curBlocks.Count; i++)
        {
            if (curBlockTileDatas[i].isMovable)
            {
                curBlocks[i].transform.localPosition = curBlockTileDatas[i].iniLocalPosition;
                curCoordVectors[i] = curBlockTileDatas[i].iniLocalPosition;
            }
        }
        //resetStoredBlockPositions
        if (VARS.curStoredSandBlockIndex > 0)
        {
            for (int i = 0; i < VARS.curStoredSandBlockIndex + 1; i++)
            {
                storedSandBlocks[i].transform.position = Vector3.zero;
            }
            VARS.curStoredSandBlockIndex = 0;
        }
        if (VARS.curStoredWaterBlockIndex > 0)
        {
            for (int i = 0; i < VARS.curStoredWaterBlockIndex + 1; i++)
            {
                storedWaterBlocks[i].transform.position = Vector3.zero;
            }
            VARS.curStoredWaterBlockIndex = 0;
        }
        if (VARS.curStoredAcidBlockIndex > 0)
        {
            for (int i = 0; i < VARS.curStoredAcidBlockIndex + 1; i++)
            {
                storedAcidBlocks[i].transform.position = Vector3.zero;
            }
            VARS.curStoredAcidBlockIndex = 0;
        }
        if (VARS.curStoredVaporBlockIndex > 0)
        {
            for (int i = 0; i < VARS.curStoredVaporBlockIndex + 1; i++)
            {
                storedVaporBlocks[i].transform.position = Vector3.zero;
            }
            VARS.curStoredVaporBlockIndex = 0;
        }
        if (VARS.curStoredGasBlockIndex > 0)
        {
            for (int i = 0; i < VARS.curStoredGasBlockIndex + 1; i++)
            {
                storedGasBlocks[i].transform.position = Vector3.zero;
            }
            VARS.curStoredGasBlockIndex = 0;
        }
        if (VARS.curStoredElectricMistBlockIndex > 0)
        {
            for (int i = 0; i < VARS.curStoredElectricMistBlockIndex + 1; i++)
            {
                storedElectricMistBlocks[i].transform.position = Vector3.zero;
            }
            VARS.curStoredElectricMistBlockIndex = 0;
        }
        if (VARS.curStoredLightElectricMistBlockIndex > 0)
        {
            for (int i = 0; i < VARS.curStoredLightElectricMistBlockIndex + 1; i++)
            {
                storedLightElectricMistBlocks[i].transform.position = Vector3.zero;
            }
            VARS.curStoredLightElectricMistBlockIndex = 0;
        }

        VARS.IsInNewRoomBlocksManagerResetOver = false;

        //clearKey
        if (VARS.IsCarryingAKey)
        {
            VARS.curCarriedKey.transform.parent = VARS.curCarriedKeyIniParent.transform;
            VARS.curCarriedKey.transform.localPosition = VARS.curCarriedKeyIniLocalPosition;

            VARS.curMinimapKey.SetActive(true);

            VARS.curCarriedKey = null;

            VARS.IsCarryingAKey = false;
        }
        //clearCarriedFragments
        if (VARS.IsCarryingFragments)
        {
            for (int i = 0; i < curCarriedFragments.Count; i++)
            {
                curCarriedFragments[i].transform.parent = curCarriedFragmentIniParents[i].transform;
                curCarriedFragments[i].transform.localPosition = VARS.curCarriedFragmentIniLocalPositions[i];
            }

            curCarriedFragments.Clear();
            curCarriedFragmentFaceIndexes.Clear();
            curCarriedFragmentIndexes.Clear();
            curCarriedFragmentIniParents.Clear();
            curCarriedFragmentIniLocalPositions.Clear();

            curToBeEmbededFragmentIndexes.Clear();
            curToBeEmbededFragmentPositions.Clear();

            VARS.IsCarryingAKey = false;
        }

        //catTransform.position = VARS.catIniPosition;
        catTransform.position = catIniPositionPoint.transform.position;

        catIniPositionPoint.transform.position = savePoints[VARS.curActivatedSavePointIndex].transform.position - VARS.roomStableForwards[VARS.curActivatedSavePointRoomIndex] * 0.1f;

        //VARS.curEnergy = maxEnergy;
        //VARS.curEnergy = 0.1f;
        //UFL.SetCurEnergy(0.1f);
        //VARS.curEnergy = 0.1f;
        VARS.curEnergy = maxEnergy + VARS.maxEnergyBonus;
        //UFL.SetCurTargetEnergy(0.1f);
        VARS.curTargetEnergy = 0.1f;
        //VARS.horCurSpeed = 0;
        //UFL.SetHorCurSpeed(0);
        VARS.horCurSpeed = 0;
        //VARS.verCurSpeed = 0;
        //UFL.SetVerCurSpeed(0);
        VARS.verCurSpeed = 0;
        VARS.catCurTemperature = 0;
        VARS.catCurElectricity = 0;
        VARS.catCurToxicity = 0;

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
