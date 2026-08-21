using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

[DefaultExecutionOrder((int)ScriptsExecutionOrder.ExecutionOrder.catTrigger)]
public class CatTrigger : MonoBehaviour
{
    Constants CONS;
    Variables VARS;
    UniversalFunctionsLibrary UFL;
    ScriptsExecutionController SEC;

    GameObject gameManager;

    //fragment
    public float curNearestMinimapFragmentDistance;
    public int curNearestMinimapFragmentIndex;

    //edgeGate
    public float curNearestEdgeGateDistance;
    public int curNearestEdgeGateIndex;
    public GameObject curToEdgeGate;
    public float throughEdgeGateTime;
    //Vector3 curEdgeGatesBetweenVector;
    //public float edgeGateTransportThres;
    public Vector3 curEdgeGatesCommonLineVector;
    public float curEdgeGatesAngle;

    //savePoint
    GameObject storedSavePointBlock;
    //GameObject storedActivatedSavePointBlock;

    //minimapKeyAndLock
    float curNearestMinimapKeyDistance;
    int curNearesetMinimapKeyIndex;
    float curNearestMinimapLockDistance;
    int curNearestMinimapLockIndex;

    //strawberry
    public List<Vector3> carriedStrawberriesIniPositions = new List<Vector3>();
    //float strawberriesRotationStartTime;

    //energyCrystal
    public List<GameObject> gotEnergyCrystals = new List<GameObject>();
    public List<float> energyCrystalGotTimes = new List<float>();
    public bool isAllGotEnergyCrystalsRespawned;

    int tempInt;
    float tempFloat;
    float tempFloat1;
    float tempFloat2;
    Vector3 tempVector;
    Vector3 tempVector1;
    Vector3 tempVector2;
    Transform tempTransform;
    GameObject tempGameObject;

    #region ConstantsUsed
    GameObject[] faces = new GameObject[6];

    Vector3[] faceStableUps = new Vector3[6];
    Vector3[] faceStableRights = new Vector3[6];

    GameObject[] roomPlanes = new GameObject[54];

    Transform camTransform;

    Transform catTransform;

    GameObject catIniPositionPoint;

    float maxEnergy;

    float energyCrystalEnergyRestoreAmount;

    float fragmentDistance;
    float fragmentSpeed;
    float energyFragmentSpeed;
    float energyFragmentBackDistance;
    float absorbingEnergyFragmentWaitingTime;

    GameObject[] energyFragments = new GameObject[6];
    //GameObject[] holeBlocks = new GameObject[6];

    float throughEdgeGateGapTime;

    List<GameObject> edgeGates = new List<GameObject>();

    List<GameObject> savePoints = new List<GameObject>();

    GameObject storedActivatedSavePointBlock;

    //GameObject storedActivatedSavePointBlockEmpty;

    List<GameObject> keys = new List<GameObject>();
    List<GameObject> locks = new List<GameObject>();

    float energyFragmentMaxEnergyBonus;
    float separatedEnergyFragmentMaxEnergyBonus;

    float keySpeed;
    float keyDistance;

    Material connectedGateColor;

    float strawberriesDistance;
    float strawberriesSpeed;
    float strawberriesContractionMin;
    float strawberriesContractionSpeed;

    float energyCrystalRespawnTime;

    GameObject[] minimapRoomPlanes = new GameObject[54];

    List<GameObject> minimapRedFragments = new List<GameObject>();
    List<GameObject> minimapYellowFragments = new List<GameObject>();
    List<GameObject> minimapBlueFragments = new List<GameObject>();
    List<GameObject> minimapOrangeFragments = new List<GameObject>();
    List<GameObject> minimapGreenFragments = new List<GameObject>();
    List<GameObject> minimapPurpleFragments = new List<GameObject>();

    List<GameObject> minimapKeys = new List<GameObject>();
    List<GameObject> minimapLocks = new List<GameObject>();

    Material minimapCollectibleCollectedColor;

    GameObject oneColorFragmentCollectingTextEmpty;
    GameObject allColorsFragmentCollectingTextEmpty;
    GameObject keysAndLocksCollectingTextEmpty;
    GameObject oneColorFragmentCollectingTextLeft;
    GameObject allColorsFragmentCollectingTextLeft1;
    GameObject allColorsFragmentCollectingTextLeft2;
    GameObject keysAndLocksCollectingTextLeft1;
    GameObject keysAndLocksCollectingTextLeft2;
    GameObject oneColorFragmentCollectingTextRight;
    GameObject allColorsFragmentCollectingTextRight;
    GameObject keysAndLocksCollectingTextRight;

    Sprite[] TBNumberSprites = new Sprite[10];
    #endregion

    #region VariablesUsed
    Vector3[] roomStableForwards;

    bool[] isRedFragmentsEmbeded = new bool[8];
    bool[] isYellowFragmentsEmbeded = new bool[8];
    bool[] isBlueFragmentsEmbeded = new bool[8];
    bool[] isOrangeFragmentsEmbeded = new bool[8];
    bool[] isGreenFragmentsEmbeded = new bool[8];
    bool[] isPurpleFragmentsEmbeded = new bool[8];

    bool[] isCenterFulfilled = new bool[6];

    List<GameObject> curCarriedFragments = new List<GameObject>();
    List<int> curCarriedFragmentFaceIndexes = new List<int>();
    List<int> curCarriedFragmentIndexes = new List<int>();
    List<GameObject> curCarriedFragmentIniParents = new List<GameObject>();
    List<Vector3> curCarriedFragmentIniLocalPositions = new List<Vector3>();

    List<int> curToBeEmbededFragmentIndexes = new List<int>();
    List<Vector3> curToBeEmbededFragmentLocalPositions = new List<Vector3>();

    GameObject curPlaneEmpty;

    Vector3 curRoomStableForward;

    List<int> edgeGateLinkedToIndexes = new List<int>();

    List<int> deactivatedKeyIndexes = new List<int>();
    List<int> deactivatedLockIndexes = new List<int>();

    Vector3 curRight;
    Vector3 curUp;

    GameObject curTriggerTile;
    TileData curTriggerTileData;

    List<GameObject> carriedStrawberries = new List<GameObject>();

    List<int> deactivatedMinimapKeyIndexes = new List<int>();
    List<int> deactivatedMinimapLockIndexes = new List<int>();
    #endregion

    #region BoolVariablesUsed
    bool isOnGround;
    bool isInLiquid;
    #endregion

    void Start()
    {
        gameManager = GameObject.Find("GameManager");

        CONS = gameManager.GetComponent<Constants>();
        VARS = gameManager.GetComponent<Variables>();
        UFL = gameManager.GetComponent<UniversalFunctionsLibrary>();
        SEC = gameManager.GetComponent<ScriptsExecutionController>();

        #region ImportConstants
        faces = CONS.faces;
        faceStableUps = CONS.faceStableUps;
        faceStableRights = CONS.faceStableRights;
        roomPlanes = CONS.roomPlanes;
        camTransform = CONS.camTransform;
        catTransform = CONS.catTransform;
        catIniPositionPoint = CONS.catIniPositionPoint;
        maxEnergy = CONS.maxEnergy;
        energyCrystalEnergyRestoreAmount = CONS.energyCrystalEnergyRestoreAmount;
        fragmentDistance = CONS.fragmentDistance;
        fragmentSpeed = CONS.fragmentSpeed;
        energyFragmentSpeed = CONS.energyFragmentSpeed;
        energyFragmentBackDistance = CONS.energyFragmentBackDistance;
        absorbingEnergyFragmentWaitingTime = CONS.absorbingEnergyFragmentWaitingTime;
        energyFragments = CONS.energyFragments;
        throughEdgeGateGapTime = CONS.throughEdgeGateGapTime;
        edgeGates = CONS.edgeGates;
        savePoints = CONS.savePoints;
        storedActivatedSavePointBlock = CONS.storedActivatedSavePointBlock;
        keys = CONS.keys;
        locks = CONS.locks;
        energyFragmentMaxEnergyBonus = CONS.energyFragmentMaxEnergyBonus;
        separatedEnergyFragmentMaxEnergyBonus = CONS.separatedEnergyFragmentMaxEnergyBonus;
        keySpeed = CONS.keySpeed;
        keyDistance = CONS.keyDistance;
        connectedGateColor = CONS.connectedGateColor;
        strawberriesDistance = CONS.strawberriesDistance;
        strawberriesSpeed = CONS.strawberriesSpeed;
        strawberriesContractionMin = CONS.strawberriesContractionMin;
        strawberriesContractionSpeed = CONS.strawberriesContractionSpeed;
        energyCrystalRespawnTime = CONS.energyCrystalRespawnTime;
        minimapRoomPlanes = CONS.minimapRoomPlanes;
        minimapRedFragments = CONS.minimapRedFragments;
        minimapYellowFragments = CONS.minimapYellowFragments;
        minimapBlueFragments = CONS.minimapBlueFragments;
        minimapOrangeFragments = CONS.minimapOrangeFragments;
        minimapGreenFragments = CONS.minimapGreenFragments;
        minimapPurpleFragments = CONS.minimapPurpleFragments;
        minimapKeys = CONS.minimapKeys;
        minimapLocks = CONS.minimapLocks;
        minimapCollectibleCollectedColor = CONS.minimapCollectibleCollectedColor;
        oneColorFragmentCollectingTextEmpty = CONS.oneColorFragmentCollectingTextEmpty;
        allColorsFragmentCollectingTextEmpty = CONS.allColorsFragmentCollectingTextEmpty;
        keysAndLocksCollectingTextEmpty = CONS.keysAndLocksCollectingTextEmpty;
        oneColorFragmentCollectingTextLeft = CONS.oneColorFragmentCollectingTextLeft;
        allColorsFragmentCollectingTextLeft1 = CONS.allColorsFragmentCollectingTextLeft1;
        allColorsFragmentCollectingTextLeft2 = CONS.allColorsFragmentCollectingTextLeft2;
        keysAndLocksCollectingTextLeft1 = CONS.keysAndLocksCollectingTextLeft1;
        keysAndLocksCollectingTextLeft2 = CONS.keysAndLocksCollectingTextLeft2;
        oneColorFragmentCollectingTextRight = CONS.oneColorFragmentCollectingTextRight;
        allColorsFragmentCollectingTextRight = CONS.allColorsFragmentCollectingTextRight;
        keysAndLocksCollectingTextRight = CONS.keysAndLocksCollectingTextRight;
        TBNumberSprites = CONS.TBNumberSprites;
        #endregion

        #region ImportReferenceVariables
        roomStableForwards = VARS.roomStableForwards;
        isRedFragmentsEmbeded = VARS.isRedFragmentsEmbeded;
        isYellowFragmentsEmbeded = VARS.isYellowFragmentsEmbeded;
        isBlueFragmentsEmbeded = VARS.isBlueFragmentsEmbeded;
        isOrangeFragmentsEmbeded = VARS.isOrangeFragmentsEmbeded;
        isGreenFragmentsEmbeded = VARS.isGreenFragmentsEmbeded;
        isPurpleFragmentsEmbeded = VARS.isPurpleFragmentsEmbeded;
        isCenterFulfilled = VARS.isCenterFulfilled;
        curCarriedFragments = VARS.curCarriedFragments;
        curCarriedFragmentFaceIndexes = VARS.curCarriedFragmentFaceIndexes;
        curCarriedFragmentIndexes = VARS.curCarriedFragmentIndexes;
        curCarriedFragmentIniParents = VARS.curCarriedFragmentIniParents;
        curCarriedFragmentIniLocalPositions = VARS.curCarriedFragmentIniLocalPositions;
        curToBeEmbededFragmentIndexes = VARS.curToBeEmbededFragmentIndexes;
        curToBeEmbededFragmentLocalPositions = VARS.curToBeEmbededFragmentLocalPositions;
        edgeGateLinkedToIndexes = VARS.edgeGateLinkedToIndexes;
        deactivatedKeyIndexes = VARS.deactivatedKeyIndexes;
        deactivatedLockIndexes = VARS.deactivatedLockIndexes;
        carriedStrawberries = VARS.carriedStrawberries;
        deactivatedMinimapKeyIndexes = VARS.deactivatedMinimapKeyIndexes;
        deactivatedMinimapLockIndexes = VARS.deactivatedMinimapLockIndexes;
        #endregion

        ////loadStoredBlocks
        //storedActivatedSavePointBlock = storedActivatedSavePointBlockEmpty.transform.GetChild(0).gameObject;
    }

    void Update()
    {
        #region ImportValueVariables
        curPlaneEmpty = VARS.curPlaneEmpty;
        curRoomStableForward = VARS.curRoomStableForward;
        curRight = VARS.curRight;
        curUp = VARS.curUp;
        curTriggerTile = VARS.curTriggerTile;
        curTriggerTileData = VARS.curTriggerTileData;
        #endregion

        #region ImportBoolVariables
        isOnGround = VARS.IsOnGround;
        isInLiquid = VARS.IsInLiquid;
        #endregion

        if (VARS.IsCatTriggerMainPartExecutable)
        {
            #region EdgeGate
            if (VARS.IsEnteringAnEdgeGate)
            {
                //trigger
                if (!VARS.IsEdgeGateTriggered)
                {
                    //determinEdgeGateDirection
                    tempFloat1 = Vector3.Dot(VARS.curEdgeGate.transform.position - VARS.curRoomCenter, VARS.curUp);
                    tempFloat2 = Vector3.Dot(VARS.curEdgeGate.transform.position - VARS.curRoomCenter, VARS.curRight);
                    //upOrDown
                    if (Mathf.Abs(tempFloat1) > Mathf.Abs(tempFloat2))
                    {
                        //up
                        if (tempFloat1 > 0)
                        {
                            tempVector = VARS.curUp;
                        }
                        //down
                        else
                        {
                            tempVector = -VARS.curUp;
                        }
                    }
                    //leftOrRight
                    else
                    {
                        //left
                        if (tempFloat2 < 0)
                        {
                            tempVector = -VARS.curRight;
                        }
                        //right
                        else
                        {
                            tempVector = VARS.curRight;
                        }
                    }

                    //Debug.Log("tempVector: " + tempVector);

                    //triggerEdgeGate
                    if (Vector3.Dot(catTransform.position - VARS.curEdgeGate.transform.position, tempVector) > 0.9f /*0.95f*/)
                    {
                        //Debug.Log("enter");
                        //Debug.Log("cat: " + catTransform.position);
                        //Debug.Log("edgeGate: " + VARS.curEdgeGate.transform.position);

                        if ((tempVector == VARS.curUp && VARS.verCurSpeed > 0) ||
                            (tempVector == -VARS.curUp && VARS.verCurSpeed < 0) ||
                            (tempVector == -VARS.curRight && VARS.horCurSpeed < 0) ||
                            (tempVector == VARS.curRight && VARS.horCurSpeed > 0))
                        {
                            VARS.IsEdgeGateTriggered = true;
                        }
                    }
                }

                //ifIsGapTimeOver
                if (Time.time - throughEdgeGateTime > throughEdgeGateGapTime)
                {
                    //toNewRoom
                    if (VARS.IsEdgeGateTriggered)
                    {
                        for (int i = 0; i < edgeGates.Count; i++)
                        {
                            if (edgeGates[i] == VARS.curEdgeGate)
                            {
                                curToEdgeGate = edgeGates[edgeGateLinkedToIndexes[i]];
                            }
                        }

                        for(int i = 0; i < roomPlanes.Length; i++)
                        {
                            if (roomPlanes[i] == curToEdgeGate.transform.parent.parent.gameObject)
                            {
                                tempInt = i;

                                break;
                            }
                        }

                        catTransform.position = curToEdgeGate.transform.position - roomStableForwards[tempInt] * /*0.1f*/ 0.2f;

                        curEdgeGatesCommonLineVector = Vector3.Cross(roomStableForwards[curTriggerTileData.inRoomIndex], roomStableForwards[curToEdgeGate.GetComponent<TileData>().inRoomIndex]);
                        curEdgeGatesAngle = Vector3.Angle(roomStableForwards[curTriggerTileData.inRoomIndex], roomStableForwards[curToEdgeGate.GetComponent<TileData>().inRoomIndex]);

                        camTransform.eulerAngles += curEdgeGatesCommonLineVector * curEdgeGatesAngle;
                        //camIniEulerangles = curEdgeGatesCommonLineVector * curEdgeGatesAngle;

                        if (Vector3.Dot(curUp, curEdgeGatesCommonLineVector) == 0)
                        {
                            curUp = Vector3.Cross(curEdgeGatesCommonLineVector, curUp);
                        }
                        if (Vector3.Dot(curRight, curEdgeGatesCommonLineVector) == 0)
                        {
                            curRight = Vector3.Cross(curEdgeGatesCommonLineVector, curRight);
                        }

                        VARS.curEdgeGate = null;

                        throughEdgeGateTime = Time.time;

                        VARS.IsEnteringAnEdgeGate = false;
                        VARS.IsEdgeGateTriggered = false;
                    }
                }
            }
            #endregion

            #region SavePoint
            if (VARS.IsToActivateASavePoint)
            {
                //Debug.Log("activateASavePoint");

                //deactivateTheLastSavePoint
                //if (VARS.curActivatedSavePoint != null)
                //{
                //    VARS.curActivatedSavePoint.SetActive(true);
                //}

                savePoints[VARS.curActivatedSavePointIndex].SetActive(true);

                //determineCurActivatedSavePoint
                for (int i = 0; i < savePoints.Count; i++)
                {
                    if (savePoints[i] == curTriggerTile)
                    {
                        VARS.curActivatedSavePointIndex = i;

                        break;
                    }
                }

                //curActivatedSavePointRoomIndex
                VARS.curActivatedSavePointRoomIndex = VARS.curRoomIndex;

                if((VARS.curRoomIndex - 4) % 9 == 0)
                {
                    VARS.IsActivatingACenterSavePoint = true;
                }

                VARS.IsToDetermineCurActivatedSavePointPosition = true;

                VARS.IsToActivateASavePoint = false;
            }

            if (VARS.IsToDetermineCurActivatedSavePointPosition)
            {
                //Debug.Log("determineCurActivatedSavePointPosition");

                VARS.curActivatedSavePointPosition = savePoints[VARS.curActivatedSavePointIndex].transform.position;

                //Debug.Log(VARS.curActivatedSavePointPosition);

                VARS.IsToActivateCurSavePoint = true;

                VARS.IsToDetermineCurActivatedSavePointPosition = false;
            }

            if (VARS.IsToActivateCurSavePoint)
            {
                //Debug.Log("activateCurSavePoint");

                //activateCurSavePoint
                //storedActivatedSavePointBlock.transform.position = VARS.curActivatedSavePoint.transform.position;
                storedActivatedSavePointBlock.transform.position = VARS.curActivatedSavePointPosition;

                //tempChildToCurPlaneEmpty
                storedActivatedSavePointBlock.transform.SetParent(VARS.curPlaneEmpty.transform, true);

                //VARS.curActivatedSavePoint.SetActive(false);
                savePoints[VARS.curActivatedSavePointIndex].SetActive(false);

                //setCatIniPosition
                //VARS.catIniPosition = VARS.curActivatedSavePoint.transform.position - curRoomStableForward * 0.1f;
                //VARS.catIniPosition = VARS.curActivatedSavePointPosition - curRoomStableForward * 0.1f;
                catIniPositionPoint.transform.position = VARS.curActivatedSavePointPosition - curRoomStableForward * /*0.1f*/0.2f;

                //if ((VARS.curRoomIndex - 4) % 9 == 0)
                //{
                //    VARS.curLatestCenterSavePointPosition = catIniPositionPoint.transform.position;
                //}
                if (VARS.IsActivatingACenterSavePoint && 
                    Vector3.Magnitude(catIniPositionPoint.transform.position) > 1)
                {
                    VARS.curLatestCenterSavePointPosition = catIniPositionPoint.transform.position;

                    if (!VARS.curAccessedCenterSavePointPositions.Contains(VARS.curLatestCenterSavePointPosition))
                    {
                        VARS.curAccessedCenterSavePointPositions.Add(VARS.curLatestCenterSavePointPosition);
                    }

                    VARS.IsActivatingACenterSavePoint = false;
                }

                //Debug.Log("catIniPosition:" + VARS.catIniPosition);

                //setCatPosition
                if (VARS.horCurSpeed == 0 &&
                    VARS.verCurSpeed == 0)
                {
                    //catTransform.position = VARS.catIniPosition;
                    catIniPositionPoint.transform.position = catTransform.position;
                }

                //Debug.Log("catPosition:" + catTransform.position);

                VARS.IsToActivateCurSavePoint = false;

                //VARS.IsToWriteCatWorldData = true;
                VARS.IsToWriteCatData = true;
                VARS.IsToWriteSavePointsData = true;
            }
            #endregion

            #region Key
            //carry
            if (VARS.IsToCarryAKey)
            {
                //VARS.curCarriedKey = VARS.curTriggerTile;
                VARS.curCarriedKey = VARS.curKey;
                //Debug.Log("curCarriedKeyIsNull: " + VARS.curCarriedKey == null);
                //Debug.Log("curCarriedKeyIniParentIsNull: " + VARS.curCarriedKeyIniParent == null);
                VARS.curCarriedKeyIniParent = VARS.curCarriedKey.transform.parent.gameObject;
                VARS.curCarriedKeyIniLocalPosition = VARS.curCarriedKey.transform.localPosition;
                VARS.curCarriedKey.transform.SetParent(null, true);
                VARS.curCarriedKeyIniRoomIndex = VARS.curRoomIndex;

                VARS.curCarriedKey.GetComponent<TileData>().isNotToBeDetected = true;

                //minimapKey
                //for (int i = 0; i < minimapKeys.Count; i++)
                //{
                //    if (minimapKeys[i].activeSelf)
                //    {
                //        tempGameObject = minimapKeys[i].transform.parent.parent.gameObject;

                //        for (int j = 0; j < 54; j++)
                //        {
                //            if (tempGameObject == minimapRoomPlanes[j])
                //            {
                //                tempInt = j;
                //                break;
                //            }
                //        }

                //        if (tempInt == VARS.curCarriedKeyIniRoomIndex)
                //        {
                //            VARS.curMinimapKey = minimapKeys[i];
                //            break;
                //        }
                //    }
                //}
                //VARS.curMinimapKey.SetActive(false);
                curNearestMinimapKeyDistance = 999;
                tempVector = UFL.Vector3WorldToMinimap(VARS.curCarriedKey.transform.position);
                for (int i = 0; i < minimapKeys.Count; i++)
                {
                    tempFloat = Vector3.Distance(minimapKeys[i].transform.position, tempVector);
                    if (tempFloat < curNearestMinimapKeyDistance)
                    {
                        curNearestMinimapKeyDistance = tempFloat;
                        curNearesetMinimapKeyIndex = i;
                    }
                }
                VARS.curMinimapKey = minimapKeys[curNearesetMinimapKeyIndex];
                VARS.curMinimapKey.GetComponent<MeshRenderer>().material = minimapCollectibleCollectedColor;


                VARS.IsCarryingAKey = true;

                VARS.IsToCarryAKey = false;

                //VARS.IsToWriteCatWorldData = true;
                VARS.IsToWriteKeysAndLocksData = true;
            }
            if (VARS.IsCarryingAKey)
            {
                //follow
                tempVector = VARS.curCarriedKey.transform.position - catTransform.position - VARS.roomStableForwards[VARS.curRoomIndex] * 0.1f;
                tempFloat = Vector3.Magnitude(tempVector);
                if (tempFloat > keyDistance * 1.5f)
                {
                    VARS.curCarriedKey.transform.position += -tempVector.normalized * keySpeed * tempFloat * Time.deltaTime;
                }
                else if (tempFloat < keyDistance * 0.25f)
                {
                    VARS.curCarriedKey.transform.position += tempVector.normalized * keySpeed * tempFloat * Time.deltaTime;
                }
            }

            //unlock
            if (VARS.IsUnlocking)
            {
                VARS.IsCarryingAKey = false;

                tempVector = VARS.curUnlockingBlock.transform.position - VARS.curCarriedKey.transform.position - VARS.roomStableForwards[VARS.curRoomIndex] * 0.1f;
                tempFloat = Vector3.Magnitude(tempVector);

                if (tempFloat > 0.1f)
                {
                    VARS.curCarriedKey.transform.position += tempVector.normalized * keySpeed * (tempFloat + 1) * Time.deltaTime;
                }
                else
                {
                    //getCurNearestLock
                    for (int i = 0; i < locks.Count; i++)
                    {
                        if (locks[i] != VARS.curUnlockingBlock &&
                            locks[i].transform.parent == VARS.curUnlockingBlock.transform.parent &&
                            Vector3.Distance(locks[i].transform.position, VARS.curUnlockingBlock.transform.position) < 1.5f)
                        {
                            tempGameObject = locks[i];
                            tempInt = i;
                            break;
                        }              
                    }

                    //curCollectedPosition
                    VARS.curCollectedPosition = tempGameObject.transform.position;

                    //deactivate
                    for (int i = 0; i < keys.Count; i++)
                    {
                        if (keys[i] == VARS.curCarriedKey)
                        {
                            deactivatedKeyIndexes.Add(i);
                            break;
                        }
                    }
                    for (int i = 0; i < locks.Count; i++)
                    {
                        if (locks[i] == VARS.curUnlockingBlock)
                        {
                            deactivatedLockIndexes.Add(i);
                            break;
                        }
                    }
                    deactivatedLockIndexes.Add(tempInt);

                    //setActiveFalse
                    VARS.curUnlockingBlock.SetActive(false);
                    tempGameObject.SetActive(false);
                    VARS.curCarriedKey.SetActive(false);

                    //minimapLock
                    tempVector = UFL.Vector3WorldToMinimap(tempGameObject.transform.position);
                    curNearestMinimapLockDistance = 999;
                    for (int i = 0; i < minimapLocks.Count; i++)
                    {
                        tempFloat = Vector3.Distance(minimapLocks[i].transform.position, tempVector);
                        if (tempFloat < curNearestMinimapLockDistance)
                        {
                            curNearestMinimapLockDistance = tempFloat;
                            curNearestMinimapLockIndex = i;
                        }
                    }
                    //minimapLocks[curNearestMinimapLockIndex].GetComponent<MeshRenderer>().material = connectedGateColor;
                    minimapLocks[curNearestMinimapLockIndex].SetActive(false);

                    //minimapDeactivate
                    for (int i = 0; i < minimapKeys.Count; i++)
                    {
                        if (minimapKeys[i] == VARS.curMinimapKey)
                        {
                            deactivatedMinimapKeyIndexes.Add(i);
                            break;
                        }
                    }
                    deactivatedMinimapLockIndexes.Add(curNearestMinimapLockIndex);
                    
                    //keysAndLocksCollectingText
                    //getCurCollectedNumber
                    VARS.curKeysAndLocksCollectedNumber = deactivatedKeyIndexes.Count;
                    //setTextLeftSprite
                    keysAndLocksCollectingTextLeft1.GetComponent<SpriteRenderer>().sprite = TBNumberSprites[VARS.curKeysAndLocksCollectedNumber / 10];
                    keysAndLocksCollectingTextLeft2.GetComponent<SpriteRenderer>().sprite = TBNumberSprites[VARS.curKeysAndLocksCollectedNumber % 10];
                    //showText
                    keysAndLocksCollectingTextEmpty.transform.position = VARS.curCollectedPosition + VARS.curUp * 0.5f - VARS.curRoomStableForward * 1;
                    keysAndLocksCollectingTextLeft1.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
                    keysAndLocksCollectingTextLeft2.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
                    keysAndLocksCollectingTextRight.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
                    keysAndLocksCollectingTextEmpty.SetActive(true);
                    VARS.keysAndLocksCollectingTextActivatedStartTime = Time.time;
                    ////tempChildToCurPlane
                    //keysAndLocksCollectingTextEmpty.transform.SetParent(VARS.curPlaneEmpty.transform, true);

                    VARS.IsToDetermineGatePassabilities = true;
                    VARS.IsToChangeGatePassabilitiesAfterUnlocking = true;

                    VARS.IsUnlocking = false;

                    //VARS.IsToWriteProgressData = true;
                    //VARS.IsToWriteCatWorldData = true;
                    VARS.IsToWriteKeysAndLocksData = true;
                }
            }
            #endregion

            #region Fragments
            //checkIfCenterFulfilled
            if (!isCenterFulfilled[0] && !isYellowFragmentsEmbeded.Contains(false)) isCenterFulfilled[0] = true;
            if (!isCenterFulfilled[1] && !isPurpleFragmentsEmbeded.Contains(false)) isCenterFulfilled[1] = true;
            if (!isCenterFulfilled[2] && !isOrangeFragmentsEmbeded.Contains(false)) isCenterFulfilled[2] = true;
            if (!isCenterFulfilled[3] && !isBlueFragmentsEmbeded.Contains(false)) isCenterFulfilled[3] = true;
            if (!isCenterFulfilled[4] && !isGreenFragmentsEmbeded.Contains(false)) isCenterFulfilled[4] = true;
            if (!isCenterFulfilled[5] && !isRedFragmentsEmbeded.Contains(false)) isCenterFulfilled[5] = true;

            //toCarry(outVersion)
            if (VARS.IsToCarryAFragment)
            {
                //Debug.Log("enter");

                curCarriedFragments.Add(VARS.curToBeCarriedFragment);
                curCarriedFragmentFaceIndexes.Add(VARS.curToBeCarriedFragmentFaceIndex);
                curCarriedFragmentIndexes.Add(VARS.curToBeCarriedFragmentIndex);
                curCarriedFragmentIniParents.Add(VARS.curToBeCarriedFragment.transform.parent.gameObject);
                curCarriedFragmentIniLocalPositions.Add(VARS.curToBeCarriedFragment.transform.localPosition);
                VARS.curToBeCarriedFragment.transform.SetParent(null, true);

                VARS.curToBeCarriedFragment.GetComponent<TileData>().isNotToBeDetected = true;

                //minimap
                if (VARS.curToBeCarriedFragmentFaceIndex == 1)
                {
                    minimapYellowFragments[VARS.curToBeCarriedFragmentIndex - 1].GetComponent<MeshRenderer>().material = minimapCollectibleCollectedColor;
                }
                else if (VARS.curToBeCarriedFragmentFaceIndex == 2)
                {
                    minimapPurpleFragments[VARS.curToBeCarriedFragmentIndex - 1].GetComponent<MeshRenderer>().material = minimapCollectibleCollectedColor;
                }
                else if (VARS.curToBeCarriedFragmentFaceIndex == 3)
                {
                    minimapOrangeFragments[VARS.curToBeCarriedFragmentIndex - 1].GetComponent<MeshRenderer>().material = minimapCollectibleCollectedColor;
                }
                else if (VARS.curToBeCarriedFragmentFaceIndex == 4)
                {
                    minimapBlueFragments[VARS.curToBeCarriedFragmentIndex - 1].GetComponent<MeshRenderer>().material = minimapCollectibleCollectedColor;
                }
                else if (VARS.curToBeCarriedFragmentFaceIndex == 5)
                {
                    minimapGreenFragments[VARS.curToBeCarriedFragmentIndex - 1].GetComponent<MeshRenderer>().material = minimapCollectibleCollectedColor;
                }
                else if (VARS.curToBeCarriedFragmentFaceIndex == 6)
                {
                    minimapRedFragments[VARS.curToBeCarriedFragmentIndex - 1].GetComponent<MeshRenderer>().material = minimapCollectibleCollectedColor;
                }

                VARS.IsToCarryAFragment = false;
                VARS.IsCarryingFragments = true;
            }
            //carrying(outVersion)
            if (VARS.IsCarryingFragments)
            {
                //followAndCondense
                for (int i = 0; i < curCarriedFragments.Count; i++)
                {
                    //if (!VARS.IsEmbeddingFragments &&
                    //    curCarriedFragmentFaceIndexes[i] == VARS.curFaceIndex)
                    if (!VARS.IsEmbeddingFragments ||
                        curCarriedFragmentFaceIndexes[i] != VARS.curFaceIndex)
                    {
                        tempVector = curCarriedFragments[i].transform.position - catTransform.position - VARS.roomStableForwards[VARS.curRoomIndex] * 0.1f;
                        tempFloat = Vector3.Magnitude(tempVector);

                        if (tempFloat > 1.5 * fragmentDistance)
                        {
                            curCarriedFragments[i].transform.position += -tempVector.normalized * fragmentSpeed * tempFloat * Time.deltaTime;
                        }
                        else if (tempFloat < 0.25 * fragmentDistance)
                        {
                            curCarriedFragments[i].transform.position += tempVector.normalized * fragmentSpeed * tempFloat * Time.deltaTime;
                        }

                        curCarriedFragments[i].transform.GetChild(0).localScale = Vector3.one * 0.45f;
                        curCarriedFragments[i].transform.GetChild(1).localScale = Vector3.one * 0.45f;
                        curCarriedFragments[i].transform.GetChild(2).localScale = Vector3.one * 0.45f;
                    }
                }

                //outOfFragments
                if (curCarriedFragments.Count == 0)
                {
                    VARS.IsCarryingFragments = false;
                }

                //embed
                if (VARS.IsInCenter &&
                    !VARS.IsEmbeddingFragments &&
                    !VARS.IsCenterFulfilled &&
                    !VARS.IsAbsorbingAnEnergyFragment)
                {
                    VARS.curEmbededFragmentCount = 0;
                    for (int i = 0; i < curCarriedFragments.Count; i++)
                    {
                        if (curCarriedFragmentFaceIndexes[i] == VARS.curFaceIndex)
                        {
                            VARS.curEmbededFragmentCount++;
                        }
                    }

                    Debug.Log("curEmbededFragmentCount: " + VARS.curEmbededFragmentCount);

                    if (VARS.curEmbededFragmentCount > 0)
                    {
                        VARS.verCurSpeed = 0;
                        VARS.horCurSpeed = 0;

                        VARS.IsDeterminingToBeEmbededFragmentPositions = true;
                        VARS.IsEmbeddingFragments = true;
                    }
                }
            }
            //embedding(outVersion)
            if (VARS.IsEmbeddingFragments)
            {
                if (VARS.IsDeterminingToBeEmbededFragmentPositions)
                {
                    for (int i = 0; i < curCarriedFragments.Count; i++)
                    {
                        if (curCarriedFragmentFaceIndexes[i] == VARS.curFaceIndex)
                        {
                            tempVector1 = faceStableUps[VARS.curFaceIndex - 1];
                            tempVector2 = faceStableRights[VARS.curFaceIndex - 1];

                            switch (curCarriedFragmentIndexes[i])
                            {
                                case 1: tempVector = -tempVector1 - tempVector2; break;
                                case 2: tempVector = -tempVector1; break;
                                case 3: tempVector = -tempVector1 + tempVector2; break;
                                case 4: tempVector = -tempVector2; break;
                                case 5: tempVector = tempVector2; break;
                                case 6: tempVector = tempVector1 - tempVector2; break;
                                case 7: tempVector = tempVector1; break;
                                case 8: tempVector = tempVector1 + tempVector2; break;
                            }

                            //Debug.Log("curToBeEmbededFragmentPosition: " + (VARS.curRoomCenter + tempVector - VARS.curRoomStableForward * 0.9f));

                            //curCarriedFragments[curToBeEmbededFragmentIndexes[i]].transform.SetParent(VARS.curPlaneEmpty.transform, true);
                            curCarriedFragments[i].transform.SetParent(VARS.curPlaneEmpty.transform, true);

                            curToBeEmbededFragmentIndexes.Add(i);
                            //curToBeEmbededFragmentLocalPositions.Add(VARS.curRoomCenter + tempVector - VARS.curRoomStableForward * 0.9f);
                            curToBeEmbededFragmentLocalPositions.Add(tempVector - VARS.curRoomStableForward * 0.9f);
                        }
                    }

                    VARS.IsDeterminingToBeEmbededFragmentPositions = false;
                }

                for (int i = curToBeEmbededFragmentIndexes.Count - 1; i > -1 ; i--)
                {
                    //Debug.Log("i: " + i);
                    //Debug.Log("curToBeEmbededFragmentIndexes[i]: " + curToBeEmbededFragmentIndexes[i]);

                    //tempVector = curCarriedFragments[curToBeEmbededFragmentIndexes[i]].transform.position - curToBeEmbededFragmentLocalPositions[i];
                    tempVector = curCarriedFragments[curToBeEmbededFragmentIndexes[i]].transform.localPosition - curToBeEmbededFragmentLocalPositions[i];
                    tempFloat = Vector3.Magnitude(tempVector);
                    
                    if (tempFloat > 0.2f)
                    {
                        //curCarriedFragments[curToBeEmbededFragmentIndexes[i]].transform.position += -tempVector.normalized * fragmentSpeed * tempFloat * Time.deltaTime;
                        curCarriedFragments[curToBeEmbededFragmentIndexes[i]].transform.localPosition += -tempVector.normalized * fragmentSpeed * tempFloat * Time.deltaTime;
                    }
                    else
                    {
                        //curCarriedFragments[curToBeEmbededFragmentIndexes[i]].transform.position = curToBeEmbededFragmentLocalPositions[i];
                        curCarriedFragments[curToBeEmbededFragmentIndexes[i]].transform.localPosition = curToBeEmbededFragmentLocalPositions[i];

                        switch (VARS.curFaceIndex)
                        {
                            case 1: isYellowFragmentsEmbeded[curCarriedFragmentIndexes[curToBeEmbededFragmentIndexes[i]] - 1] = true; break;
                            case 2: isPurpleFragmentsEmbeded[curCarriedFragmentIndexes[curToBeEmbededFragmentIndexes[i]] - 1] = true; break;
                            case 3: isOrangeFragmentsEmbeded[curCarriedFragmentIndexes[curToBeEmbededFragmentIndexes[i]] - 1] = true; break;
                            case 4: isBlueFragmentsEmbeded[curCarriedFragmentIndexes[curToBeEmbededFragmentIndexes[i]] - 1] = true; break;
                            case 5: isGreenFragmentsEmbeded[curCarriedFragmentIndexes[curToBeEmbededFragmentIndexes[i]] - 1] = true; break;
                            case 6: isRedFragmentsEmbeded[curCarriedFragmentIndexes[curToBeEmbededFragmentIndexes[i]] - 1] = true; break;
                        }

                        for (int j = 0; j < curCarriedFragments[curToBeEmbededFragmentIndexes[i]].transform.childCount; j++)
                        {
                            curCarriedFragments[curToBeEmbededFragmentIndexes[i]].transform.GetChild(j).gameObject.SetActive(j > 2);
                        }

                        //curCarriedFragments[curToBeEmbededFragmentIndexes[i]].transform.SetParent(VARS.curPlaneEmpty.transform, true);

                        //curCarriedFragments.RemoveAt(curToBeEmbededFragmentIndexes[i]);
                        //curCarriedFragmentFaceIndexes.RemoveAt(curToBeEmbededFragmentIndexes[i]);
                        //curCarriedFragmentIndexes.RemoveAt(curToBeEmbededFragmentIndexes[i]);
                        //curCarriedFragmentIniParents.RemoveAt(curToBeEmbededFragmentIndexes[i]);
                        //curCarriedFragmentIniLocalPositions.RemoveAt(curToBeEmbededFragmentIndexes[i]);

                        curToBeEmbededFragmentIndexes.RemoveAt(i);
                        curToBeEmbededFragmentLocalPositions.RemoveAt(i);
                    }
                }

                if (curToBeEmbededFragmentIndexes.Count == 0)
                {
                    for (int i = curCarriedFragments.Count - 1; i > -1 ; i--)
                    {
                        if (curCarriedFragmentFaceIndexes[i] == VARS.curFaceIndex)
                        {
                            curCarriedFragments[i].transform.SetParent(VARS.curPlaneEmpty.transform, true);

                            curCarriedFragments.RemoveAt(i);
                            curCarriedFragmentFaceIndexes.RemoveAt(i);
                            curCarriedFragmentIndexes.RemoveAt(i);
                            curCarriedFragmentIniParents.RemoveAt(i);
                            curCarriedFragmentIniLocalPositions.RemoveAt(i);
                        }
                    }

                    VARS.IsEmbeddingFragments = false;

                    VARS.IsCenterFulfilled = true;

                    //if ((VARS.curFaceIndex == 1 && !isYellowFragmentsEmbeded.Contains(false)) ||
                    //    (VARS.curFaceIndex == 2 && !isPurpleFragmentsEmbeded.Contains(false)) ||
                    //    (VARS.curFaceIndex == 3 && !isOrangeFragmentsEmbeded.Contains(false)) ||
                    //    (VARS.curFaceIndex == 4 && !isBlueFragmentsEmbeded.Contains(false)) ||
                    //    (VARS.curFaceIndex == 5 && !isGreenFragmentsEmbeded.Contains(false)) ||
                    //    (VARS.curFaceIndex == 6 && !isRedFragmentsEmbeded.Contains(false)))
                    //{
                    //    isCenterFulfilled[VARS.curFaceIndex - 1] = true;

                    //    VARS.IsToActivateCenterFulfilledMasks = true;

                    //    //VARS.IsCenterFulfilled = true;
                    //}                    
                }

                //VARS.IsToWriteCatWorldData = true;
                VARS.IsToWriteFragmentsData = true;
            }
            //centerFulfilled(outVersion)
            if (VARS.IsCenterFulfilled)
            {
                Debug.Log("centerFulfilled");

                //holeBlocks[VARS.curFaceIndex - 1].transform.position = VARS.curRoomCenter - VARS.curRoomStableForward * 0.9f;
                energyFragments[VARS.curFaceIndex - 1].transform.position = VARS.curRoomCenter - VARS.curRoomStableForward * 1.1f /** 0.9f*/;

                VARS.absorbingEnergyFragmentWaitingStartTime = Time.time;
                VARS.IsEnergyFragmentBacked = false;
                VARS.IsAbsorbingAnEnergyFragment = true;

                VARS.IsCenterFulfilled = false;
            }
            //absorbingAnEnergyFragment(outVersion)
            if (VARS.IsAbsorbingAnEnergyFragment)
            {
                if (Time.time - VARS.absorbingEnergyFragmentWaitingStartTime > absorbingEnergyFragmentWaitingTime)
                {
                    tempVector = energyFragments[VARS.curFaceIndex - 1].transform.position - catTransform.position /*- VARS.curRoomStableForward * 0.1f*/;
                    tempFloat = Vector3.Magnitude(tempVector);

                    //if (!VARS.IsEnergyFragmentBacked)
                    //{
                    //    if (tempFloat < energyFragmentBackDistance)
                    //    {
                    //        energyFragments[VARS.curFaceIndex - 1].transform.position += tempVector * energyFragmentSpeed /** (2 - tempFloat)*/ * Time.deltaTime;
                    //    }
                    //    else
                    //    {
                    //        VARS.IsEnergyFragmentBacked = true;
                    //    }
                    //}
                    //else
                    //{
                        if (tempFloat > 0.1f)
                        {
                            energyFragments[VARS.curFaceIndex - 1].transform.position += -tempVector * energyFragmentSpeed /** (2 - tempFloat)*/ * Time.deltaTime;
                            //if (tempFloat < 1.5)
                            //{
                            //    energyFragments[VARS.curFaceIndex - 1].transform.localScale = Vector3.one * ((tempFloat + 0.5f) / 2);
                            //}
                        }
                        else
                        {
                            energyFragments[VARS.curFaceIndex - 1].transform.position = Vector3.zero;
                            energyFragments[VARS.curFaceIndex - 1].transform.localScale = Vector3.one;

                            //VARS.maxEnergyBonus += energyFragmentMaxEnergyBonus;
                            VARS.maxEnergyBonus += separatedEnergyFragmentMaxEnergyBonus * VARS.curEmbededFragmentCount;

                            Debug.Log("separatedEnergyFragmentMaxEnergyBonus * VARS.curEmbededFragmentCount: " + separatedEnergyFragmentMaxEnergyBonus * VARS.curEmbededFragmentCount);

                            //holeBlocks[VARS.curFaceIndex - 1].transform.position = VARS.curRoomCenter - VARS.curRoomStableForward * 0.9f;

                            VARS.IsAbsorbingAnEnergyFragment = false;
                        }
                    //}
                }
            }

            //collect(directlyWhenTouched)
            if (VARS.IsCollectingAFragment)
            {
                //curCollectedPosition
                VARS.curCollectedPosition = VARS.curToBeCarriedFragment.transform.position;

                //determinePosition
                tempVector1 = faceStableUps[VARS.curToBeCarriedFragmentFaceIndex - 1];
                tempVector2 = faceStableRights[VARS.curToBeCarriedFragmentFaceIndex - 1];
                    switch (VARS.curToBeCarriedFragmentIndex)
                    {
                        case 1: tempVector = -tempVector1 - tempVector2; break;
                        case 2: tempVector = -tempVector1; break;
                        case 3: tempVector = -tempVector1 + tempVector2; break;
                        case 4: tempVector = -tempVector2; break;
                        case 5: tempVector = tempVector2; break;
                        case 6: tempVector = tempVector1 - tempVector2; break;
                        case 7: tempVector = tempVector1; break;
                        case 8: tempVector = tempVector1 + tempVector2; break;
                    }
                VARS.curToBeCarriedFragment.transform.SetParent(roomPlanes[4 + 9 * (VARS.curToBeCarriedFragmentFaceIndex - 1)].transform.GetChild(0), true);
                VARS.curToBeCarriedFragment.transform.localPosition = (tempVector - /*VARS.curRoomStableForward*/roomStableForwards[4 + 9 * (VARS.curToBeCarriedFragmentFaceIndex - 1)] * 0.9f);

                //outlineScale
                VARS.curToBeCarriedFragment.transform.GetChild(0).localScale = Vector3.one * 0.45f;
                VARS.curToBeCarriedFragment.transform.GetChild(1).localScale = Vector3.one * 0.45f;
                VARS.curToBeCarriedFragment.transform.GetChild(2).localScale = Vector3.one * 0.45f;

                //embed
                switch (VARS.curToBeCarriedFragmentFaceIndex)
                {
                    case 1: isYellowFragmentsEmbeded[VARS.curToBeCarriedFragmentIndex - 1] = true; break;
                    case 2: isPurpleFragmentsEmbeded[VARS.curToBeCarriedFragmentIndex - 1] = true; break;
                    case 3: isOrangeFragmentsEmbeded[VARS.curToBeCarriedFragmentIndex - 1] = true; break;
                    case 4: isBlueFragmentsEmbeded[VARS.curToBeCarriedFragmentIndex - 1] = true; break;
                    case 5: isGreenFragmentsEmbeded[VARS.curToBeCarriedFragmentIndex - 1] = true; break;
                    case 6: isRedFragmentsEmbeded[VARS.curToBeCarriedFragmentIndex - 1] = true; break;
                }
                for (int j = 0; j < VARS.curToBeCarriedFragment.transform.childCount; j++)
                {
                    VARS.curToBeCarriedFragment.transform.GetChild(j).gameObject.SetActive(j > 2);
                }

                ////maxEnergyBonus
                //VARS.maxEnergyBonus += separatedEnergyFragmentMaxEnergyBonus;

                //minimap
                if (VARS.curToBeCarriedFragmentFaceIndex == 1)
                {
                    minimapYellowFragments[VARS.curToBeCarriedFragmentIndex - 1].GetComponent<MeshRenderer>().material = minimapCollectibleCollectedColor;
                }
                else if (VARS.curToBeCarriedFragmentFaceIndex == 2)
                {
                    minimapPurpleFragments[VARS.curToBeCarriedFragmentIndex - 1].GetComponent<MeshRenderer>().material = minimapCollectibleCollectedColor;
                }
                else if (VARS.curToBeCarriedFragmentFaceIndex == 3)
                {
                    minimapOrangeFragments[VARS.curToBeCarriedFragmentIndex - 1].GetComponent<MeshRenderer>().material = minimapCollectibleCollectedColor;
                }
                else if (VARS.curToBeCarriedFragmentFaceIndex == 4)
                {
                    minimapBlueFragments[VARS.curToBeCarriedFragmentIndex - 1].GetComponent<MeshRenderer>().material = minimapCollectibleCollectedColor;
                }
                else if (VARS.curToBeCarriedFragmentFaceIndex == 5)
                {
                    minimapGreenFragments[VARS.curToBeCarriedFragmentIndex - 1].GetComponent<MeshRenderer>().material = minimapCollectibleCollectedColor;
                }
                else if (VARS.curToBeCarriedFragmentFaceIndex == 6)
                {
                    minimapRedFragments[VARS.curToBeCarriedFragmentIndex - 1].GetComponent<MeshRenderer>().material = minimapCollectibleCollectedColor;
                }

                //setRoomFragmentCollected
                VARS.IsRoomFragmentCollected[VARS.curRoomIndex] = true;

                //setIsRotateEnabled
                VARS.IsRotateEnabled = true;

                //setHasCollectedFragment
                if (!VARS.HasCollectedFragment)
                {
                    VARS.HasCollectedFragment = true;
                }

                //oneColorFragmentCollectingText
                //getCurCollectedNumber
                VARS.curOneColorFragmentCollectedNumbers[VARS.curRoomIndex / 9] = 0;
                for (int i = (VARS.curRoomIndex / 9) * 9; i < (VARS.curRoomIndex / 9 + 1) * 9; i++)
                {
                    VARS.curOneColorFragmentCollectedNumbers[VARS.curRoomIndex / 9] += Convert.ToInt32(VARS.IsRoomFragmentCollected[i]);
                }
                VARS.curOneColorFragmentCollectedNumbers[VARS.curRoomIndex / 9]--;
                //setTextLeftSprite
                oneColorFragmentCollectingTextLeft.GetComponent<SpriteRenderer>().sprite = TBNumberSprites[VARS.curOneColorFragmentCollectedNumbers[VARS.curRoomIndex / 9]];
                //showText
                oneColorFragmentCollectingTextEmpty.transform.position = VARS.curCollectedPosition + VARS.curUp * 0.5f - VARS.curRoomStableForward * 1;
                oneColorFragmentCollectingTextLeft.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
                oneColorFragmentCollectingTextRight.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
                oneColorFragmentCollectingTextEmpty.SetActive(true);
                VARS.oneColorFragmentCollectingTextActivatedStartTime = Time.time;
                ////tempChildToCurPlane
                //oneColorFragmentCollectingTextEmpty.transform.SetParent(VARS.curPlaneEmpty.transform, true);

                //allColorFragmentCollectingText
                //getCurCollectedNumber
                VARS.curAllColorsFragmentCollectedNumber = 0;
                for (int i = 0; i < 54; i++)
                {
                    VARS.curAllColorsFragmentCollectedNumber += Convert.ToInt32(VARS.IsRoomFragmentCollected[i]);
                }
                VARS.curAllColorsFragmentCollectedNumber -= 6;
                //setTextLeftSprite
                allColorsFragmentCollectingTextLeft1.GetComponent<SpriteRenderer>().sprite = TBNumberSprites[VARS.curAllColorsFragmentCollectedNumber / 10];
                allColorsFragmentCollectingTextLeft2.GetComponent<SpriteRenderer>().sprite = TBNumberSprites[VARS.curAllColorsFragmentCollectedNumber % 10];
                //showText
                allColorsFragmentCollectingTextEmpty.transform.position = VARS.curCollectedPosition + VARS.curUp * 0.5f - VARS.curRoomStableForward * 1;
                allColorsFragmentCollectingTextLeft1.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
                allColorsFragmentCollectingTextLeft2.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
                allColorsFragmentCollectingTextRight.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
                allColorsFragmentCollectingTextEmpty.SetActive(true);
                VARS.allColorsFragmentCollectingTextActivatedStartTime = Time.time;
                ////tempChildToCurPlane
                //allColorsFragmentCollectingTextEmpty.transform.SetParent(VARS.curPlaneEmpty.transform, true);

                VARS.IsCollectingAFragment = false;

                //VARS.IsToWriteProgressData = true;
                //VARS.IsToWriteCatWorldData = true;
                VARS.IsToWriteFragmentsData = true;
            }
            #endregion

            #region Strawberry(outVersion)
            //lose
            if (VARS.IsToLoseCarriedStrawberries)
            {
                VARS.IsCarryingStrawberries = false;

                for (int i = 0; i < carriedStrawberries.Count; i++)
                {
                    carriedStrawberries[i].transform.position = carriedStrawberriesIniPositions[i];
                }

                carriedStrawberries.Clear();
                carriedStrawberriesIniPositions.Clear();
            }

            //get
            if (VARS.IsGettingAStrawberry)
            {
                VARS.IsCarryingStrawberries = true;

                carriedStrawberries.Add(curTriggerTile);
                carriedStrawberriesIniPositions.Add(curTriggerTile.transform.position);

                VARS.IsGettingAStrawberry = false;
            }

            //carry
            if (VARS.IsCarryingStrawberries)
            {
                for (int i = 0; i < carriedStrawberries.Count; i++)
                {
                    if (Vector3.Distance(catTransform.position, carriedStrawberries[i].transform.position) > strawberriesDistance)
                    {
                        carriedStrawberries[i].transform.position = Vector3.MoveTowards(carriedStrawberries[i].transform.position, catTransform.position, strawberriesSpeed * Time.deltaTime);
                    }
                }
            }
            //collect
            else if (VARS.IsCollectingStrawberries)
            {
                if (carriedStrawberries.Count > 0)
                {
                    if (carriedStrawberries[0].transform.localScale.magnitude > strawberriesContractionMin)
                    {
                        for (int i = 0; i < carriedStrawberries.Count; i++)
                        {
                            carriedStrawberries[i].transform.localScale -= Vector3.one * strawberriesContractionSpeed * Time.deltaTime;
                            carriedStrawberries[i].transform.position = Vector3.MoveTowards(carriedStrawberries[i].transform.position, catTransform.position, strawberriesSpeed / 6 * Time.deltaTime);
                        }
                    }
                    else
                    {
                        for (int i = 0; i < carriedStrawberries.Count; i++)
                        {
                            Destroy(carriedStrawberries[i]);
                        }

                        carriedStrawberries.Clear();
                        carriedStrawberriesIniPositions.Clear();

                        VARS.IsCollectingStrawberries = false;
                    }
                }
                else
                {
                    VARS.IsCollectingStrawberries = false;
                }
            }
            #endregion

            #region EnergyCrystal
            if (VARS.IsGettingAnEnergyCrystal)
            {
                gotEnergyCrystals.Add(curTriggerTile);
                energyCrystalGotTimes.Add(Time.time);

                //curTriggerTile.transform.localScale = Vector3.one * 0.2f;
                curTriggerTile.SetActive(false);

                //VARS.curEnergy += energyCrystalPower;
                //if (VARS.curEnergy > maxEnergy + VARS.maxEnergyBonus)
                //{
                //    VARS.curEnergy = maxEnergy + VARS.maxEnergyBonus;
                //}
                VARS.curTargetEnergy += energyCrystalEnergyRestoreAmount;

                VARS.IsGettingAnEnergyCrystal = false;
            }

            if (gotEnergyCrystals.Count > 0)
            {
                isAllGotEnergyCrystalsRespawned = true;

                for (int i = 0; i < gotEnergyCrystals.Count; i++)
                {
                    //if (gotEnergyCrystals[i].transform.localScale == Vector3.one * 0.2f)
                    //{
                    //    isAllGotEnergyCrystalsRespawned = false;

                    //    if (Time.time - energyCrystalGotTimes[i] > energyCrystalRespawnTime)
                    //    {
                    //        gotEnergyCrystals[i].transform.localScale = Vector3.one;
                    //    }
                    //}
                    if (gotEnergyCrystals[i].activeSelf == false)
                    {
                        isAllGotEnergyCrystalsRespawned = false;
                        
                        if (Time.time - energyCrystalGotTimes[i] > energyCrystalRespawnTime)
                        {
                            gotEnergyCrystals[i].SetActive(true);
                        }
                    }
                }

                if (isAllGotEnergyCrystalsRespawned)
                {
                    gotEnergyCrystals.Clear();
                    energyCrystalGotTimes.Clear();
                }
            }
            #endregion
        }

        #region OnGroundOrInLiquidReset
        if (!VARS.IsRotating &&
            !VARS.IsTwisting)
        {
            if (VARS.IsOnGround ||
                VARS.IsInLiquid)
            {
                if (VARS.IsIniRotation)
                {
                    //strawberries
                    if (VARS.IsCarryingStrawberries)
                    {
                        VARS.IsCollectingStrawberries = true;

                        VARS.IsCarryingStrawberries = false;
                    }
                }
            }
        }
        #endregion
    }
}
