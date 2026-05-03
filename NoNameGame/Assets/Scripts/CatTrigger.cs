using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using UnityEngine;

[DefaultExecutionOrder((int)ScriptsExecutionOrder.ExecutionOrder.catTrigger)]
public class CatTrigger : MonoBehaviour
{
    Constants CONS;
    Variables VARS;
    UniversalFunctionsLibrary UFL;
    ScriptsExecutionController SEC;

    GameObject gameManager;

    //strawberry
    public List<Vector3> carriedStrawberriesIniPositions = new List<Vector3>();
    //float strawberriesRotationStartTime;

    //energyCrystal
    public List<GameObject> gotEnergyCrystals = new List<GameObject>();
    public List<float> energyCrystalGotTimes = new List<float>();
    public bool isAllGotEnergyCrystalsRespawned;

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

    //minimapLock
    float curNearestMinimapLockDistance;
    int curNearestMinimapLockIndex;

    int tempInt;
    float tempFloat;
    float tempFloat1;
    float tempFloat2;
    Vector3 tempVector;
    Transform tempTransform;
    GameObject tempGameObject;

    #region ConstantsUsed
    GameObject[] faces = new GameObject[6];

    Transform camTransform;

    Transform catTransform;

    GameObject catIniPositionPoint;

    float maxEnergy;

    float fragmentDistance;
    float fragmentSpeed;
    float energyFragmentSpeed;
    float energyFragmentBackDistance;
    float absorbingEnergyFragmentWaitingTime;

    GameObject[] energyFragments = new GameObject[6];
    GameObject[] holeBlocks = new GameObject[6];

    float throughEdgeGateGapTime;

    List<GameObject> edgeGates = new List<GameObject>();

    List<GameObject> savePoints = new List<GameObject>();

    GameObject storedActivatedSavePointBlock;

    //GameObject storedActivatedSavePointBlockEmpty;

    List<GameObject> keys = new List<GameObject>();
    List<GameObject> locks = new List<GameObject>();

    float energyFragmentMaxEnergyBonus;

    float keySpeed;
    float keyDistance;

    Material connectedGateColor;

    float strawberriesDistance;
    float strawberriesSpeed;
    float strawberriesContractionMin;
    float strawberriesContractionSpeed;

    float energyCrystalPower;
    float energyCrystalRespawnTime;

    GameObject[] minimapRoomPlanes = new GameObject[54];

    List<GameObject> minimapKeys = new List<GameObject>();
    List<GameObject> minimapLocks = new List<GameObject>();
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
    List<Vector3> curToBeEmbededFragmentPositions = new List<Vector3>();

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
        camTransform = CONS.camTransform;
        catTransform = CONS.catTransform;
        catIniPositionPoint = CONS.catIniPositionPoint;
        maxEnergy = CONS.maxEnergy;
        fragmentDistance = CONS.fragmentDistance;
        fragmentSpeed = CONS.fragmentSpeed;
        energyFragmentSpeed = CONS.energyFragmentSpeed;
        energyFragmentBackDistance = CONS.energyFragmentBackDistance;
        absorbingEnergyFragmentWaitingTime = CONS.absorbingEnergyFragmentWaitingTime;
        energyFragments = CONS.energyFragments;
        holeBlocks = CONS.holeBlocks;
        throughEdgeGateGapTime = CONS.throughEdgeGateGapTime;
        edgeGates = CONS.edgeGates;
        savePoints = CONS.savePoints;
        storedActivatedSavePointBlock = CONS.storedActivatedSavePointBlock;
        keys = CONS.keys;
        locks = CONS.locks;
        energyFragmentMaxEnergyBonus = CONS.energyFragmentMaxEnergyBonus;
        keySpeed = CONS.keySpeed;
        keyDistance = CONS.keyDistance;
        connectedGateColor = CONS.connectedGateColor;
        strawberriesDistance = CONS.strawberriesDistance;
        strawberriesSpeed = CONS.strawberriesSpeed;
        strawberriesContractionMin = CONS.strawberriesContractionMin;
        strawberriesContractionSpeed = CONS.strawberriesContractionSpeed;
        energyCrystalPower = CONS.energyCrystalPower;
        energyCrystalRespawnTime = CONS.energyCrystalRespawnTime;
        minimapRoomPlanes = CONS.minimapRoomPlanes;
        minimapKeys = CONS.minimapKeys;
        minimapLocks = CONS.minimapLocks;
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
        curToBeEmbededFragmentPositions = VARS.curToBeEmbededFragmentPositions;
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
                    if (Vector3.Dot(catTransform.position - VARS.curEdgeGate.transform.position, tempVector) > 0.9f)
                    {
                        //Debug.Log("enter");
                        //Debug.Log("cat: " + catTransform.position);
                        //Debug.Log("edgeGate: " + VARS.curEdgeGate.transform.position);

                        VARS.IsEdgeGateTriggered = true;
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

                        catTransform.position = curToEdgeGate.transform.position - roomStableForwards[curToEdgeGate.GetComponent<TileData>().inRoomIndex] * 0.1f;

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

                VARS.IsToDetermineCurActivatedSavePointPosition = true;

                VARS.IsToActivateCurSavePoint = true;

                VARS.IsToActivateASavePoint = false;
            }

            if (VARS.IsToDetermineCurActivatedSavePointPosition)
            {
                VARS.curActivatedSavePointPosition = savePoints[VARS.curActivatedSavePointIndex].transform.position;

                //Debug.Log(VARS.curActivatedSavePointPosition);

                VARS.IsToDetermineCurActivatedSavePointPosition = false;
            }

            if (VARS.IsToActivateCurSavePoint)
            {
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
                catIniPositionPoint.transform.position = VARS.curActivatedSavePointPosition - curRoomStableForward * 0.1f;

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

                VARS.IsToWriteCatWorldData = true;
            }
            #endregion

            #region Key
            //carry
            if (VARS.IsToCarryAKey)
            {
                //VARS.curCarriedKey = VARS.curTriggerTile;
                VARS.curCarriedKey = VARS.curKey;
                VARS.curCarriedKeyIniParent = VARS.curCarriedKey.transform.parent.gameObject;
                VARS.curCarriedKeyIniLocalPosition = VARS.curCarriedKey.transform.localPosition;
                VARS.curCarriedKey.transform.SetParent(null, true);
                VARS.curCarriedKeyIniRoomIndex = VARS.curRoomIndex;

                //minimapKey
                for (int i = 0; i < minimapKeys.Count; i++)
                {
                    if (minimapKeys[i].activeSelf)
                    {
                        tempGameObject = minimapKeys[i].transform.parent.parent.gameObject;

                        for (int j = 0; j < 54; j++)
                        {
                            if (tempGameObject == minimapRoomPlanes[j])
                            {
                                tempInt = j;
                                break;
                            }
                        }

                        if (tempInt == VARS.curCarriedKeyIniRoomIndex)
                        {
                            VARS.curMinimapKey = minimapKeys[i];
                            break;
                        }
                    }
                }
                VARS.curMinimapKey.SetActive(false);

                VARS.IsCarryingAKey = true;

                VARS.IsToCarryAKey = false;

                VARS.IsToWriteCatWorldData = true;
            }
            if (VARS.IsCarryingAKey)
            {
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
                    minimapLocks[curNearestMinimapLockIndex].GetComponent<MeshRenderer>().material = connectedGateColor;

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

                    VARS.IsUnlocking = false;

                    VARS.IsToWriteCatWorldData = true;
                }
            }
            #endregion

            #region Fragments
            //toCarry
            if (VARS.IsToCarryAFragment)
            {
                //Debug.Log("enter");

                curCarriedFragments.Add(VARS.curToBeCarriedFragment);
                curCarriedFragmentFaceIndexes.Add(VARS.curToBeCarriedFragmentFaceIndex);
                curCarriedFragmentIndexes.Add(VARS.curToBeCarriedFragmentIndex);
                curCarriedFragmentIniParents.Add(VARS.curToBeCarriedFragment.transform.parent.gameObject);
                curCarriedFragmentIniLocalPositions.Add(VARS.curToBeCarriedFragment.transform.localPosition);
                VARS.curToBeCarriedFragment.transform.SetParent(null, true);

                VARS.IsToCarryAFragment = false;
                VARS.IsCarryingFragments = true;
            }
            //carrying
            if (VARS.IsCarryingFragments)
            {
                //follow
                for (int i = 0; i < curCarriedFragments.Count; i++)
                {
                    if (!VARS.IsEmbeddingFragments &&
                        curCarriedFragmentFaceIndexes[i] == VARS.curFaceIndex)
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
                    }
                }

                //embed
                if (VARS.IsInCenter &&
                    !VARS.IsEmbeddingFragments)
                {
                    VARS.IsDeterminingToBeEmbededFragmentPositions = true;
                    VARS.IsEmbeddingFragments = true;
                }

                //outOfFragments
                if (curCarriedFragments.Count == 0)
                {
                    VARS.IsCarryingFragments = false;
                }
            }
            //embedding
            if (VARS.IsEmbeddingFragments)
            {
                if (VARS.IsDeterminingToBeEmbededFragmentPositions)
                {
                    for (int i = 0; i < curCarriedFragments.Count; i++)
                    {
                        if (curCarriedFragmentFaceIndexes[i] == VARS.curFaceIndex)
                        {
                            switch (curCarriedFragmentIndexes[i])
                            {
                                case 1: tempVector = -VARS.curRoomStableUp - VARS.curRoomStableRight; break;
                                case 2: tempVector = -VARS.curRoomStableUp; break;
                                case 3: tempVector = -VARS.curRoomStableUp + VARS.curRoomStableRight; break;
                                case 4: tempVector = -VARS.curRoomStableRight; break;
                                case 5: tempVector = VARS.curRoomStableRight; break;
                                case 6: tempVector = VARS.curRoomStableUp - VARS.curRoomStableRight; break;
                                case 7: tempVector = VARS.curRoomStableUp; break;
                                case 8: tempVector = VARS.curRoomStableUp + VARS.curRoomStableRight; break;
                            }

                            //Debug.Log("curToBeEmbededFragmentPosition: " + (VARS.curRoomCenter + tempVector - VARS.curRoomStableForward * 0.9f));

                            curToBeEmbededFragmentIndexes.Add(i);
                            curToBeEmbededFragmentPositions.Add(VARS.curRoomCenter + tempVector - VARS.curRoomStableForward * 0.9f);
                        }
                    }

                    VARS.IsDeterminingToBeEmbededFragmentPositions = false;
                }

                for (int i = curToBeEmbededFragmentIndexes.Count - 1; i > -1 ; i--)
                {
                    //Debug.Log("i: " + i);
                    //Debug.Log("curToBeEmbededFragmentIndexes[i]: " + curToBeEmbededFragmentIndexes[i]);

                    tempVector = curCarriedFragments[curToBeEmbededFragmentIndexes[i]].transform.position - curToBeEmbededFragmentPositions[i];
                    tempFloat = Vector3.Magnitude(tempVector);
                    
                    if (tempFloat > 0.2f)
                    {
                        curCarriedFragments[curToBeEmbededFragmentIndexes[i]].transform.position += -tempVector.normalized * fragmentSpeed * tempFloat * Time.deltaTime;
                    }
                    else
                    {
                        curCarriedFragments[curToBeEmbededFragmentIndexes[i]].transform.position = curToBeEmbededFragmentPositions[i];

                        switch (VARS.curFaceIndex)
                        {
                            case 1: isYellowFragmentsEmbeded[curCarriedFragmentIndexes[curToBeEmbededFragmentIndexes[i]] - 1] = true; break;
                            case 2: isPurpleFragmentsEmbeded[curCarriedFragmentIndexes[curToBeEmbededFragmentIndexes[i]] - 1] = true; break;
                            case 3: isOrangeFragmentsEmbeded[curCarriedFragmentIndexes[curToBeEmbededFragmentIndexes[i]] - 1] = true; break;
                            case 4: isBlueFragmentsEmbeded[curCarriedFragmentIndexes[curToBeEmbededFragmentIndexes[i]] - 1] = true; break;
                            case 5: isGreenFragmentsEmbeded[curCarriedFragmentIndexes[curToBeEmbededFragmentIndexes[i]] - 1] = true; break;
                            case 6: isRedFragmentsEmbeded[curCarriedFragmentIndexes[curToBeEmbededFragmentIndexes[i]] - 1] = true; break;
                        }

                        for (int j = 0; j < 9; j++)
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
                        curToBeEmbededFragmentPositions.RemoveAt(i);
                    }
                }

                if (curToBeEmbededFragmentIndexes.Count == 0)
                {
                    for (int i = 0; i < curCarriedFragments.Count; i++)
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

                    if ((VARS.curFaceIndex == 1 && !isYellowFragmentsEmbeded.Contains(false)) ||
                        (VARS.curFaceIndex == 2 && !isPurpleFragmentsEmbeded.Contains(false)) ||
                        (VARS.curFaceIndex == 3 && !isOrangeFragmentsEmbeded.Contains(false)) ||
                        (VARS.curFaceIndex == 4 && !isBlueFragmentsEmbeded.Contains(false)) ||
                        (VARS.curFaceIndex == 5 && !isGreenFragmentsEmbeded.Contains(false)) ||
                        (VARS.curFaceIndex == 6 && !isRedFragmentsEmbeded.Contains(false)))
                    {
                        isCenterFulfilled[VARS.curFaceIndex - 1] = true;

                        VARS.IsCenterFulfilled = true;
                    }
                }

                VARS.IsToWriteCatWorldData = true;
            }
            //centerFulfilled
            if (VARS.IsCenterFulfilled)
            {
                Debug.Log("centerFulfilled");

                holeBlocks[VARS.curFaceIndex - 1].transform.position = VARS.curRoomCenter - VARS.curRoomStableForward * 0.9f;
                energyFragments[VARS.curFaceIndex - 1].transform.position = VARS.curRoomCenter - VARS.curRoomStableForward /** 0.9f*/;

                VARS.absorbingEnergyFragmentWaitingStartTime = Time.time;
                VARS.IsEnergyFragmentBacked = false;
                VARS.IsAbsorbingAnEnergyFragment = true;

                VARS.IsCenterFulfilled = false;
            }
            //absorbingAnEnergyFragment
            if (VARS.IsAbsorbingAnEnergyFragment)
            {
                if (Time.time - VARS.absorbingEnergyFragmentWaitingStartTime > absorbingEnergyFragmentWaitingTime)
                {
                    tempVector = energyFragments[VARS.curFaceIndex - 1].transform.position - catTransform.position /*- VARS.curRoomStableForward * 0.1f*/;
                    tempFloat = Vector3.Magnitude(tempVector);

                    if (!VARS.IsEnergyFragmentBacked)
                    {
                        if (tempFloat < energyFragmentBackDistance)
                        {
                            energyFragments[VARS.curFaceIndex - 1].transform.position += tempVector * energyFragmentSpeed * (2 - tempFloat) * Time.deltaTime;
                        }
                        else
                        {
                            VARS.IsEnergyFragmentBacked = true;
                        }
                    }
                    else
                    {
                        if (tempFloat > 0.1f)
                        {
                            energyFragments[VARS.curFaceIndex - 1].transform.position += -tempVector * energyFragmentSpeed * (2 - tempFloat) * Time.deltaTime;
                            //if (tempFloat < 1.5)
                            //{
                            //    energyFragments[VARS.curFaceIndex - 1].transform.localScale = Vector3.one * ((tempFloat + 0.5f) / 2);
                            //}
                        }
                        else
                        {
                            energyFragments[VARS.curFaceIndex - 1].transform.position = Vector3.zero;
                            energyFragments[VARS.curFaceIndex - 1].transform.localScale = Vector3.one;

                            VARS.maxEnergyBonus += energyFragmentMaxEnergyBonus;

                            //holeBlocks[VARS.curFaceIndex - 1].transform.position = VARS.curRoomCenter - VARS.curRoomStableForward * 0.9f;

                            VARS.IsAbsorbingAnEnergyFragment = false;
                        }
                    }
                }
            }
            #endregion

            #region Strawberry
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

                curTriggerTile.transform.localScale = Vector3.one * 0.2f;

                VARS.curEnergy += energyCrystalPower;
                if (VARS.curEnergy > maxEnergy + VARS.maxEnergyBonus)
                {
                    VARS.curEnergy = maxEnergy + VARS.maxEnergyBonus;
                }

                VARS.IsGettingAnEnergyCrystal = false;
            }

            if (gotEnergyCrystals.Count > 0)
            {
                isAllGotEnergyCrystalsRespawned = true;

                for (int i = 0; i < gotEnergyCrystals.Count; i++)
                {
                    if (gotEnergyCrystals[i].transform.localScale == Vector3.one * 0.2f)
                    {
                        isAllGotEnergyCrystalsRespawned = false;

                        if (Time.time - energyCrystalGotTimes[i] > energyCrystalRespawnTime)
                        {
                            gotEnergyCrystals[i].transform.localScale = Vector3.one;
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
