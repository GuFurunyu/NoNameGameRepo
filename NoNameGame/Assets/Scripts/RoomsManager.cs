using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

[DefaultExecutionOrder((int)ScriptsExecutionOrder.ExecutionOrder.roomsManager)]
public class RoomsManager : MonoBehaviour
{
    Constants CONS;
    Variables VARS;
    UniversalFunctionsLibrary UFL;
    ScriptsExecutionController SEC;

    GameObject gameManager;

    bool isInAnotherRoom;

    bool isTwistingPresetOver;

    GameObject curTwistingCenter;
    Vector3 curTwistingCenterPosition;

    Vector3 curFaceStableForward;
    Vector3 curFaceStableUp;
    Vector3 curFaceStableRight;

    List<GameObject> curRelatedRoomPlanes = new List<GameObject>();
    List<int> curRelatedRoomPlaneIndexes = new List<int>();

    float twistingAccumulatedDegree;
    Vector3 twistingTargetEulerangles;

    //gates
    float curNearestGateDistance;
    float curGateNearestLockDistance;

    //edgeGates
    float curNearestEdgeGateDistance;
    int curNearestEdgeGateIndex;
    float curEdgeGateNearestLockDistance;

    //minimapGates
    float curNearestMinimapGateDistance;
    int curNearestMinimapGateIndex;

    int tempInt;
    float tempFloat;
    float tempFloat1;
    float tempFloat2;
    float tempFloat3;
    float tempFloat4;
    Vector3 tempVector;
    GameObject tempGameObject;
    Transform tempTransform;

    #region ConstantsUsed
    float gridBreadth;
    int roomCoordBreadth;

    float inRoomMaxForwardDistance;

    GameObject[] faces = new GameObject[6];
    Vector3[] faceStableForwards = new Vector3[6];
    Vector3[] faceStableUps = new Vector3[6];
    Vector3[] faceStableRights = new Vector3[6];

    GameObject[] roomPlanes = new GameObject[54];

    float justInGateOverTime;

    GameObject[] twistingCenters = new GameObject[6];
    Vector3[] twistingCenterClockwiseVectors = new Vector3[6];
    float twistSpeed;

    //gates
    List<GameObject> gates = new List<GameObject>();

    //edgeGates
    List<GameObject> edgeGates = new List<GameObject>();
    List<GameObject> edgeGateTriggers = new List<GameObject>();

    //gateColor
    Material connectedGateColor;
    Material unconnectedGateColor;

    List<GameObject> locks = new List<GameObject>();

    List<GameObject> savePoints = new List<GameObject>();

    List<GameObject> minimapGates = new List<GameObject>();

    float minimapRotationMovingSpeed;

    Transform camTransform;

    float camMinimapDistanceToCubeCore;

    Transform catTransform;

    GameObject catIniPositionPoint;

    GameObject storedSandBlocksEmpty;
    GameObject storedWaterBlocksEmpty;
    GameObject storedAcidBlocksEmpty;
    GameObject storedVaporBlocksEmpty;
    GameObject storedGasBlocksEmpty;
    GameObject storedElectricMistBlocksEmpty;
    GameObject storedLightElectricMistBlocksEmpty;
    #endregion

    #region VariablesUsed
    GameObject curPlaneEmpty;

    //int[] faceDirectionIndexes = new int[6];

    Vector3[] roomCenters = new Vector3[54];
    Vector3[] roomStableForwards = new Vector3[54];
    Vector3[] roomStableUps = new Vector3[54];
    Vector3[] roomStableRights = new Vector3[54];

    List<int> edgeGateLinkedToIndexes = new List<int>();

    List<int> deactivatedLockIndexes = new List<int>();

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
        gridBreadth = CONS.gridBreadth;
        roomCoordBreadth = CONS.roomCoordBreadth;
        inRoomMaxForwardDistance = CONS.inRoomMaxForwardDistance;
        faces = CONS.faces;
        faceStableForwards = CONS.faceStableForwards;
        faceStableUps = CONS.faceStableUps;
        faceStableRights = CONS.faceStableRights;
        roomPlanes = CONS.roomPlanes;
        justInGateOverTime = CONS.justInGateOverTime;
        twistingCenters = CONS.twistingCenters;
        twistingCenterClockwiseVectors = CONS.twistingCenterClockwiseVectors;
        twistSpeed = CONS.twistSpeed;
        gates = CONS.gates;
        edgeGates = CONS.edgeGates;
        edgeGateTriggers = CONS.edgeGateTriggers;
        connectedGateColor = CONS.connectedGateColor;
        unconnectedGateColor = CONS.unconnectedGateColor;
        locks = CONS.locks;
        savePoints = CONS.savePoints;
        minimapGates = CONS.minimapGates;
        minimapRotationMovingSpeed = CONS.minimapRotationMovingSpeed;
        camTransform = CONS.camTransform;
        camMinimapDistanceToCubeCore = CONS.camMinimapDistanceToCubeCore;
        catTransform = CONS.catTransform;
        catIniPositionPoint = CONS.catIniPositionPoint;
        storedSandBlocksEmpty = CONS.storedSandBlocksEmpty;
        storedWaterBlocksEmpty = CONS.storedWaterBlocksEmpty;
        storedAcidBlocksEmpty = CONS.storedAcidBlocksEmpty;
        storedVaporBlocksEmpty = CONS.storedVaporBlocksEmpty;
        storedGasBlocksEmpty = CONS.storedGasBlocksEmpty;
        storedElectricMistBlocksEmpty = CONS.storedElectricMistBlocksEmpty;
        storedLightElectricMistBlocksEmpty = CONS.storedLightElectricMistBlocksEmpty;
        #endregion
        catTransform = CONS.catTransform;

        #region ImportReferenceVariables
        roomCenters = VARS.roomCenters;
        roomStableForwards = VARS.roomStableForwards;
        roomStableUps = VARS.roomStableUps;
        roomStableRights = VARS.roomStableRights;
        edgeGateLinkedToIndexes = VARS.edgeGateLinkedToIndexes;
        deactivatedLockIndexes = VARS.deactivatedLockIndexes;
        storedSandBlocks = VARS.storedSandBlocks;
        storedWaterBlocks = VARS.storedWaterBlocks;
        storedAcidBlocks = VARS.storedAcidBlocks;
        storedVaporBlocks = VARS.storedVaporBlocks;
        storedGasBlocks = VARS.storedGasBlocks;
        storedElectricMistBlocks = VARS.storedElectricMistBlocks;
        storedLightElectricMistBlocks = VARS.storedLightElectricMistBlocks;
        #endregion        
    }

    void Update()
    {
        #region ImportValueVariables
        curPlaneEmpty = VARS.curPlaneEmpty;
        #endregion

        //Debug.Log(roomPlanes[19].transform.forward);
        //Debug.Log(roomPlanes[19].transform.up);
        //Debug.Log(roomPlanes[19].transform.right);

        ////edgeGateTriggerRefresh
        //for (int i = 0; i < edgeGateTriggers.Count; i++)
        //{
        //    edgeGateTriggers[i].transform.position = edgeGateTriggers[i].transform.parent.position;
        //}

        #region IfIsInNewRoom
        //justByGate
        if (VARS.IsJustByGate &&
            Time.time - VARS.lastJustInGateTime > justInGateOverTime)
        {
            VARS.IsJustByGate = false;
        }

        if (UFL.IsInRoom(VARS.curRoomIndex, catTransform.position))
        {
        }
        else
        {
            ////mustBeViable(~byGates)
            //if (VARS.IsRoomTransferViable)
            //{

            isInAnotherRoom = false;

            for (int i = 0; i < roomCenters.Length; i++)
            {
                if (UFL.IsInRoom(i, catTransform.position))
                {
                    VARS.curRoomIndex = i;
                    //isInAnotherRoom = true;
                    VARS.IsIntoNewRoom = true;
                    break;
                }
            }

            //VARS.IsJustStartedTheGame = false;
            //VARS.IsJustByGate = false;
            //VARS.IsJustDied = false;

            //if (isInAnotherRoom)
            //{
            //    VARS.IsIntoNewRoom = true;
            //}
            //else
            //{
            //    VARS.IsToDie = true;
            //}
            //}
            ////elseDie
            //else
            //{
            //    VARS.IsToDie = true;
            //}
        }

        //if (VARS.IsIntoNewRoom)
        //{
        //    IntoNewRoom();
        //}
        #endregion

        #region InNewRoomReset(OutVersion)
        //if (VARS.IsInNewRoomAllResetOver)
        //{
        //    if (!VARS.IsInNewRoomCurRoomManagerResetOver ||
        //        !VARS.IsInNewRoomCameraManagerResetOver ||
        //        !VARS.IsInNewRoomCatRotateResetOver ||
        //        !VARS.IsInNewRoomBlocksManagerResetOver)
        //    {
        //        VARS.IsInNewRoomAllResetOver = false;
        //    }
        //}
        //if (!VARS.IsInNewRoomAllResetOver)
        //{
        //    if (VARS.IsInNewRoomCurRoomManagerResetOver &&
        //        VARS.IsInNewRoomCameraManagerResetOver &&
        //        VARS.IsInNewRoomCatRotateResetOver &&
        //        VARS.IsInNewRoomBlocksManagerResetOver)
        //    {
        //        //hideOtherPlanes
        //        if (!VARS.IsZoomedOut)
        //            //UFL.HideOtherPlanes();

        //        VARS.IsInNewRoom = false;

        //        VARS.IsInNewRoomAllResetOver = true;
        //    }
        //}
        #endregion

        if (VARS.IsInNewRoomAllResetOver)
        {
            //hideOtherPlanes
            if (!VARS.IsZoomedOut &&
                !VARS.IsInMinimap)
                UFL.HideOtherPlanes();

            #region Twist
            //control
            if (!VARS.IsTwisting)
            {
                if (VARS.IsInCenter &&
                    UFL.IsInRoom(VARS.curRoomIndex,VARS.curLatestCenterSavePointPosition))
                {
                    if (VARS.IsInputtingDownKey)
                    {
                        if (VARS.IsLeftKeyDown ||
                            VARS.IsRightKeyDown)
                        {
                            //getTwistingFaceIndex
                            tempGameObject = curPlaneEmpty.transform.parent.parent.gameObject;
                            for (int i = 0; i < 6; i++)
                            {
                                if (tempGameObject == faces[i])
                                {
                                    VARS.twistingFaceIndex = i + 1;
                                    break;
                                }
                            }

                            //determineTwistingDirection
                            if (VARS.IsLeftKeyDown)
                            {
                                VARS.IsClockwiseTwisting = true;
                            }
                            else if (VARS.IsRightKeyDown)
                            {
                                VARS.IsClockwiseTwisting = false;
                            }

                            VARS.IsTwisting = true;
                        }
                    }
                }
            }

            //process
            else
            {
                if (!isTwistingPresetOver)
                {
                    tempInt = VARS.twistingFaceIndex;

                    curTwistingCenter = twistingCenters[tempInt - 1];
                    curTwistingCenterPosition = curTwistingCenter.transform.position;

                    curFaceStableForward = faceStableForwards[tempInt - 1];
                    curFaceStableUp = faceStableUps[tempInt - 1];
                    curFaceStableRight = faceStableRights[tempInt - 1];

                    //getRelatedRoomPlanes
                    for (int i = 0; i < roomPlanes.Length; i++)
                    {
                        tempVector = roomCenters[i] - curTwistingCenterPosition;

                        //getRoomPlanesInTheFace
                        if (/*Mathf.Abs(Vector3.Dot(tempVector, curFaceStableForward)) <= (roomCoordBreadth / 2 + 2) * gridBreadth*/
                            UFL.IsPlaneInTheFace(i, tempInt))
                        {
                            curRelatedRoomPlanes.Add(roomPlanes[i]);
                            curRelatedRoomPlaneIndexes.Add(i);
                        }

                        //getRoomPlanesSurroundingTheFace
                        if (/*Mathf.Abs(Vector3.SignedAngle(tempVector, curFaceStableUp, curFaceStableForward)) <= 6 &&
                            Mathf.Abs(Vector3.SignedAngle(tempVector, curFaceStableRight, curFaceStableForward)) <= 6*/
                            UFL.IsPlaneSurroundingTheFace(i, tempInt))
                        {
                            curRelatedRoomPlanes.Add(roomPlanes[i]);
                            curRelatedRoomPlaneIndexes.Add(i);
                        }
                    }

                    //roomPlanesTempChildToCurTwistingCenter
                    for (int i = 0; i < curRelatedRoomPlanes.Count; i++)
                    {
                        curRelatedRoomPlanes[i].transform.SetParent(curTwistingCenter.transform, true);
                    }

                    //storedBlocksTempChildToCurTwistingCenter
                    if (VARS.curStoredSandBlockIndex > 0)
                    {
                        for (int i = 0; i < VARS.curStoredSandBlockIndex + 1; i++)
                        {
                            storedSandBlocks[i].transform.SetParent(curTwistingCenter.transform, true);
                        }
                    }
                    if (VARS.curStoredWaterBlockIndex > 0)
                    {
                        for (int i = 0; i < VARS.curStoredWaterBlockIndex + 1; i++)
                        {
                            storedWaterBlocks[i].transform.SetParent(curTwistingCenter.transform, true);
                        }
                    }
                    if (VARS.curStoredAcidBlockIndex > 0)
                    {
                        for (int i = 0; i < VARS.curStoredAcidBlockIndex + 1; i++)
                        {
                            storedAcidBlocks[i].transform.SetParent(curTwistingCenter.transform, true);
                        }
                    }
                    if (VARS.curStoredVaporBlockIndex > 0)
                    {
                        for (int i = 0; i < VARS.curStoredVaporBlockIndex + 1; i++)
                        {
                            storedVaporBlocks[i].transform.SetParent(curTwistingCenter.transform, true);
                        }
                    }
                    if (VARS.curStoredGasBlockIndex > 0)
                    {
                        for (int i = 0; i < VARS.curStoredGasBlockIndex + 1; i++)
                        {
                            storedGasBlocks[i].transform.SetParent(curTwistingCenter.transform, true);
                        }
                    }
                    if (VARS.curStoredElectricMistBlockIndex > 0)
                    {
                        for (int i = 0; i < VARS.curStoredElectricMistBlockIndex + 1; i++)
                        {
                            storedElectricMistBlocks[i].transform.SetParent(curTwistingCenter.transform, true);
                        }
                    }
                    if (VARS.curStoredLightElectricMistBlockIndex > 0)
                    {
                        for (int i = 0; i < VARS.curStoredLightElectricMistBlockIndex + 1; i++)
                        {
                            storedLightElectricMistBlocks[i].transform.SetParent(curTwistingCenter.transform, true);
                        }
                    }

                    //catAndCamTempChildToCurTwistingCenter
                    //camTransform.SetParent(curTwistingCenter.transform, true);
                    catTransform.SetParent(curTwistingCenter.transform, true);
                    //catIniPositionPoint.transform.SetParent(curTwistingCenter.transform, true);

                    //setTargetEulerangles
                    if (VARS.IsClockwiseTwisting)
                    {
                        //twistingTargetEulerangles = curTwistingCenter.transform.eulerAngles + twistingCenterClockwiseVectors[VARS.curFaceIndex - 1];

                        //if (Mathf.Min(Mathf.Abs(Vector3.Dot(curTwistingCenter.transform.eulerAngles, twistingCenterClockwiseVectors[VARS.curFaceIndex - 1])),
                        //       Mathf.Abs(Vector3.Dot(curTwistingCenter.transform.eulerAngles - Vector3.one * 360, twistingCenterClockwiseVectors[VARS.curFaceIndex - 1]))) < 1000)
                        //{
                        //    //twistingTargetEulerangles = curTwistingCenter.transform.eulerAngles - twistingCenterClockwiseVectors[VARS.curFaceIndex - 1];
                        //    twistingTargetEulerangles *= -1;
                        //}
                        //else
                        //{
                        //    //twistingTargetEulerangles = curTwistingCenter.transform.eulerAngles + twistingCenterClockwiseVectors[VARS.curFaceIndex - 1];
                        //}

                        Quaternion curRotation = curTwistingCenter.transform.rotation;
                        Quaternion deltaRotation = Quaternion.Euler(twistingCenterClockwiseVectors[VARS.curFaceIndex - 1]);
                        Quaternion newRotation = curRotation * deltaRotation;
                        twistingTargetEulerangles = newRotation.eulerAngles;

                        Debug.Log("enter" + Mathf.Min(Mathf.Abs(Vector3.Dot(curTwistingCenter.transform.eulerAngles, twistingCenterClockwiseVectors[VARS.curFaceIndex - 1])),
                               Mathf.Abs(Vector3.Dot(curTwistingCenter.transform.eulerAngles - Vector3.one * 360, twistingCenterClockwiseVectors[VARS.curFaceIndex - 1]))));
                    }
                    else
                    {
                        //twistingTargetEulerangles = curTwistingCenter.transform.eulerAngles + -twistingCenterClockwiseVectors[VARS.curFaceIndex - 1];

                        //if (Mathf.Min(Mathf.Abs(Vector3.Dot(curTwistingCenter.transform.eulerAngles, twistingCenterClockwiseVectors[VARS.curFaceIndex - 1])),
                        //       Mathf.Abs(Vector3.Dot(curTwistingCenter.transform.eulerAngles - Vector3.one * 360, twistingCenterClockwiseVectors[VARS.curFaceIndex - 1]))) < 1000)
                        //{
                        //    //twistingTargetEulerangles = curTwistingCenter.transform.eulerAngles + twistingCenterClockwiseVectors[VARS.curFaceIndex - 1];
                        //    twistingTargetEulerangles *= -1;
                        //}
                        //else
                        //{
                        //    //twistingTargetEulerangles = curTwistingCenter.transform.eulerAngles - twistingCenterClockwiseVectors[VARS.curFaceIndex - 1];
                        //}

                        Quaternion curRotation = curTwistingCenter.transform.rotation;
                        Quaternion deltaRotation = Quaternion.Euler(-twistingCenterClockwiseVectors[VARS.curFaceIndex - 1]);
                        Quaternion newRotation = curRotation * deltaRotation;
                        twistingTargetEulerangles = newRotation.eulerAngles;

                        //Debug.Log("enter" + Mathf.Min(Mathf.Abs(Vector3.Dot(curTwistingCenter.transform.eulerAngles, twistingCenterClockwiseVectors[VARS.curFaceIndex - 1])),
                        //       Mathf.Abs(Vector3.Dot(curTwistingCenter.transform.eulerAngles - Vector3.one * 360, twistingCenterClockwiseVectors[VARS.curFaceIndex - 1]))));
                    }

                    //Debug.Log("curTwistingCenter.transform.eulerAngles: " + curTwistingCenter.transform.eulerAngles);
                    //Debug.Log("twistingCenterClockwiseVectors[VARS.curFaceIndex - 1]: " + twistingCenterClockwiseVectors[VARS.curFaceIndex - 1]);
                    //Debug.Log("twistingTargetEulerangles: " + twistingTargetEulerangles);

                    isTwistingPresetOver = true;
                }

                //twist
                if (twistingAccumulatedDegree < 90)
                {
                    if (Input.GetKey(VARS.acceKeyCode))
                    {
                        twistingAccumulatedDegree += twistSpeed * 1.5f * Time.deltaTime;
                    }
                    else
                    {
                        twistingAccumulatedDegree += twistSpeed * Time.deltaTime;
                    }
                    if (VARS.IsClockwiseTwisting)
                    {
                        if (Input.GetKey(VARS.acceKeyCode))
                        {
                            curTwistingCenter.transform.Rotate(curFaceStableForward * twistSpeed * 1.5f * Time.deltaTime);
                        }
                        else
                        {
                            curTwistingCenter.transform.Rotate(curFaceStableForward * twistSpeed * Time.deltaTime);
                        }
                    }
                    else
                    {
                        if (Input.GetKey(VARS.acceKeyCode))
                        {
                            curTwistingCenter.transform.Rotate(-curFaceStableForward * twistSpeed * 1.5f * Time.deltaTime);
                        }
                        else
                        {
                            curTwistingCenter.transform.Rotate(-curFaceStableForward * twistSpeed * Time.deltaTime);
                        }
                    }
                }
                else
                {
                    //setPositionsAndEulerangles(~?)
                    curTwistingCenter.transform.eulerAngles = twistingTargetEulerangles;

                    //resetRoomPlanesParents
                    ResetCurRelatedPlanes();
                    curRelatedRoomPlanes.Clear();
                    curRelatedRoomPlaneIndexes.Clear();

                    //freeStoredBlocks
                    if (VARS.curStoredSandBlockIndex > 0)
                    {
                        for (int i = 0; i < VARS.curStoredSandBlockIndex + 1; i++)
                        {
                            storedSandBlocks[i].transform.SetParent(storedSandBlocksEmpty.transform, true);
                        }
                    }
                    if (VARS.curStoredWaterBlockIndex > 0)
                    {
                        for (int i = 0; i < VARS.curStoredWaterBlockIndex + 1; i++)
                        {
                            storedWaterBlocks[i].transform.SetParent(storedWaterBlocksEmpty.transform, true);
                        }
                    }
                    if (VARS.curStoredAcidBlockIndex > 0)
                    {
                        for (int i = 0; i < VARS.curStoredAcidBlockIndex + 1; i++)
                        {
                            storedAcidBlocks[i].transform.SetParent(storedAcidBlocksEmpty.transform, true);
                        }
                    }
                    if (VARS.curStoredVaporBlockIndex > 0)
                    {
                        for (int i = 0; i < VARS.curStoredVaporBlockIndex + 1; i++)
                        {
                            storedVaporBlocks[i].transform.SetParent(storedVaporBlocksEmpty.transform, true);
                        }
                    }
                    if (VARS.curStoredGasBlockIndex > 0)
                    {
                        for (int i = 0; i < VARS.curStoredGasBlockIndex + 1; i++)
                        {
                            storedGasBlocks[i].transform.SetParent(storedGasBlocksEmpty.transform, true);
                        }
                    }
                    if (VARS.curStoredElectricMistBlockIndex > 0)
                    {
                        for (int i = 0; i < VARS.curStoredElectricMistBlockIndex + 1; i++)
                        {
                            storedElectricMistBlocks[i].transform.SetParent(storedElectricMistBlocksEmpty.transform, true);
                        }
                    }
                    if (VARS.curStoredLightElectricMistBlockIndex > 0)
                    {
                        for (int i = 0; i < VARS.curStoredLightElectricMistBlockIndex + 1; i++)
                        {
                            storedLightElectricMistBlocks[i].transform.SetParent(storedLightElectricMistBlocksEmpty.transform, true);
                        }
                    }

                    //freeCatAndCam
                    //camTransform.SetParent(null);
                    catTransform.SetParent(null);
                    //catIniPositionPoint.transform.SetParent(null);

                    //catIniPositionPoint
                    catIniPositionPoint.transform.position = 
                        savePoints[VARS.curActivatedSavePointIndex].transform.position - VARS.roomStableForwards[VARS.curActivatedSavePointRoomIndex] * 0.1f;

                    //resetCatEulerangles
                    catTransform.eulerAngles = Vector3.zero;

                    //setMinimapRoomPlanes
                    UFL.SetMinimapRoomPlanesByRoomPlanes();

                    //faceDirectionIndexes
                    if (!VARS.IsClockwiseTwisting)
                    {
                        VARS.faceDirectionIndexes[VARS.curFaceIndex - 1] = (VARS.faceDirectionIndexes[VARS.curFaceIndex - 1] + 1) % 4;
                    }
                    else
                    {
                        VARS.faceDirectionIndexes[VARS.curFaceIndex - 1] = (VARS.faceDirectionIndexes[VARS.curFaceIndex - 1] + 3) % 4;
                    }

                    VARS.IsToDetermineGatePassabilities = true;

                    isTwistingPresetOver = false;

                    twistingAccumulatedDegree = 0;

                    VARS.IsIntoNewRoom = true;

                    VARS.IsNotToResetMovableBlockPositions = true;

                    VARS.IsToDetermineCurActivatedSavePointPosition = true;

                    VARS.IsTwisting = false;

                    VARS.IsToWriteWorldData = true;
                }
            }
            #endregion

            if (VARS.IsToDetermineGatePassabilities)
            {
                #region DetermineGatePassabilities
                //lockNotConnectedGates
                for (int i = 0; i < gates.Count; i++)
                {
                    //if (gates[i].transform.parent != VARS.curPlaneEmpty.transform)
                    //    continue;

                    tempTransform = gates[i].transform;

                    //findCurNearestGate
                    curNearestGateDistance = 999;
                    for (int j = 0; j < gates.Count; j++)
                    {
                        if (gates[j].transform.parent != tempTransform.parent)
                        {
                            if (Vector3.Distance(gates[j].transform.position, tempTransform.position) < curNearestGateDistance)
                            {
                                curNearestGateDistance = Vector3.Distance(gates[j].transform.position, tempTransform.position);
                            }
                        }
                    }

                    //linkConnectedGates
                    if (curNearestGateDistance < 6 * gridBreadth)
                    {
                        //Debug.Log("enter1");
                        //tempTransform.GetComponent<TileData>().triggerTypeIndex = 3;
                        //toTrigger
                        tempTransform.GetComponent<TileData>().stateOfMatterIndex = 0;
                        for (int k = 0; k < tempTransform.childCount; k++)
                        {
                            tempTransform.GetChild(k).gameObject.SetActive(false);
                        }

                        tempTransform.GetComponent<MeshRenderer>().material = connectedGateColor;

                        //minimapGate
                        tempVector = UFL.Vector3WorldToMinimap(tempTransform.position);
                        curNearestMinimapGateDistance = 999;
                        for (int j = 0; j < minimapGates.Count; j++)
                        {
                            tempFloat = Vector3.Distance(minimapGates[j].transform.position, tempVector);
                            if (tempFloat < curNearestMinimapGateDistance)
                            {
                                curNearestMinimapGateDistance = tempFloat;
                                curNearestMinimapGateIndex = j;
                            }
                        }
                        minimapGates[curNearestMinimapGateIndex].GetComponent<MeshRenderer>().material = connectedGateColor;
                    }
                    //lockNotConnectedGates
                    else
                    {
                        //Debug.Log("enter2");
                        //tempTransform.GetComponent<TileData>().triggerTypeIndex = 0;
                        //toSolid
                        tempTransform.GetComponent<TileData>().stateOfMatterIndex = 1;
                        for (int k = 0; k < tempTransform.childCount; k++)
                        {
                            tempTransform.GetChild(k).gameObject.SetActive(true);
                        }

                        tempTransform.GetComponent<MeshRenderer>().material = unconnectedGateColor;

                        //minimapGate
                        tempVector = UFL.Vector3WorldToMinimap(tempTransform.position);
                        curNearestMinimapGateDistance = 999;
                        for (int j = 0; j < minimapGates.Count; j++)
                        {
                            tempFloat = Vector3.Distance(minimapGates[j].transform.position, tempVector);
                            if (tempFloat < curNearestMinimapGateDistance)
                            {
                                curNearestMinimapGateDistance = tempFloat;
                                curNearestMinimapGateIndex = j;
                            }
                        }
                        minimapGates[curNearestMinimapGateIndex].GetComponent<MeshRenderer>().material = unconnectedGateColor;
                    }

                    //findCurNearestLock
                    curGateNearestLockDistance = 999;
                    for (int j = 0; j < locks.Count; j++)
                    {
                        if (locks[j].transform.parent != tempTransform.parent &&
                            !deactivatedLockIndexes.Contains(j))
                        {
                            if (Vector3.Distance(locks[j].transform.position, tempTransform.position) < curGateNearestLockDistance)
                            {
                                curGateNearestLockDistance = Vector3.Distance(locks[j].transform.position, tempTransform.position);
                            }
                        }
                    }

                    //lockNotConnectedGates
                    if (curGateNearestLockDistance < 6 * gridBreadth)
                    {
                        //Debug.Log("enter");

                        //toSolid
                        tempTransform.GetComponent<TileData>().stateOfMatterIndex = 1;
                        for (int k = 0; k < tempTransform.childCount; k++)
                        {
                            tempTransform.GetChild(k).gameObject.SetActive(true);
                        }

                        tempTransform.GetComponent<MeshRenderer>().material = unconnectedGateColor;

                        //minimapGate
                        tempVector = UFL.Vector3WorldToMinimap(tempTransform.position);
                        curNearestMinimapGateDistance = 999;
                        for (int j = 0; j < minimapGates.Count; j++)
                        {
                            tempFloat = Vector3.Distance(minimapGates[j].transform.position, tempVector);
                            if (tempFloat < curNearestMinimapGateDistance)
                            {
                                curNearestMinimapGateDistance = tempFloat;
                                curNearestMinimapGateIndex = j;
                            }
                        }
                        minimapGates[curNearestMinimapGateIndex].GetComponent<MeshRenderer>().material = unconnectedGateColor;
                    }
                }


                //initializeEdgeGateLinkedToIndexes
                edgeGateLinkedToIndexes.Clear();
                for (int i = 0; i < edgeGates.Count; i++)
                {
                    edgeGateLinkedToIndexes.Add(-1);
                }

                //determineEdgeGatePassabilities
                for (int i = 0; i < edgeGates.Count; i++)
                {
                    //if (edgeGates[i].transform.parent != VARS.curPlaneEmpty.transform)
                    //    continue;

                    tempTransform = edgeGates[i].transform;

                    //for (int i = 0; i < edgeGates.Count; i++)
                    //{
                    //    if (edgeGates[i].transform.parent != curTriggerTile.transform.parent)
                    //    {
                    //        if (Vector3.Distance(edgeGates[i].transform.position, curTriggerTile.transform.position) < curNearestEdgeGateDistance)
                    //        {
                    //            curNearestEdgeGateDistance = Vector3.Distance(edgeGates[i].transform.position, curTriggerTile.transform.position);
                    //            curNearestEdgeGateIndex = i;
                    //        }
                    //    }
                    //}

                    //findCurNearestEdgeGate
                    curNearestEdgeGateDistance = 999;
                    for (int j = 0; j < edgeGates.Count; j++)
                    {
                        if (edgeGates[j].transform.parent != tempTransform.parent)
                        {
                            if (Vector3.Distance(edgeGates[j].transform.position, tempTransform.position) < curNearestEdgeGateDistance)
                            {
                                curNearestEdgeGateDistance = Vector3.Distance(edgeGates[j].transform.position, tempTransform.position);
                                curNearestEdgeGateIndex = j;
                            }
                        }
                    }

                    //linkConnectedEdgeGates
                    if (curNearestEdgeGateDistance < 6 * gridBreadth)
                    {
                        //tempTransform.GetComponent<TileData>().triggerTypeIndex = 4;
                        //toTrigger
                        tempTransform.GetComponent<TileData>().stateOfMatterIndex = 0;
                        edgeGateLinkedToIndexes[i] = curNearestEdgeGateIndex;
                        for (int k = 0; k < tempTransform.childCount; k++)
                        {
                            tempTransform.GetChild(k).gameObject.SetActive(false);
                        }

                        tempTransform.GetComponent<MeshRenderer>().material = connectedGateColor;

                        //minimapGate
                        tempVector = UFL.Vector3WorldToMinimap(tempTransform.position);
                        curNearestMinimapGateDistance = 999;
                        for (int j = 0; j < minimapGates.Count; j++)
                        {
                            tempFloat = Vector3.Distance(minimapGates[j].transform.position, tempVector);
                            if (tempFloat < curNearestMinimapGateDistance)
                            {
                                curNearestMinimapGateDistance = tempFloat;
                                curNearestMinimapGateIndex = j;
                            }
                        }
                        minimapGates[curNearestMinimapGateIndex].GetComponent<MeshRenderer>().material = connectedGateColor;
                    }
                    //lockNotConnectedEdgeGates
                    else
                    {
                        //Debug.Log("enter3");
                        //tempTransform.GetComponent<TileData>().triggerTypeIndex = 0;
                        //toSolid
                        tempTransform.GetComponent<TileData>().stateOfMatterIndex = 1;
                        edgeGateLinkedToIndexes[i] = -1;
                        for (int k = 0; k < tempTransform.childCount; k++)
                        {
                            tempTransform.GetChild(k).gameObject.SetActive(true);
                        }

                        tempTransform.GetComponent<MeshRenderer>().material = unconnectedGateColor;

                        //minimapGate
                        tempVector = UFL.Vector3WorldToMinimap(tempTransform.position);
                        curNearestMinimapGateDistance = 999;
                        for (int j = 0; j < minimapGates.Count; j++)
                        {
                            tempFloat = Vector3.Distance(minimapGates[j].transform.position, tempVector);
                            if (tempFloat < curNearestMinimapGateDistance)
                            {
                                curNearestMinimapGateDistance = tempFloat;
                                curNearestMinimapGateIndex = j;
                            }
                        }
                        minimapGates[curNearestMinimapGateIndex].GetComponent<MeshRenderer>().material = unconnectedGateColor;
                    }

                    //findCurNearestLock
                    curEdgeGateNearestLockDistance = 999;
                    for (int j = 0; j < locks.Count; j++)
                    {
                        if (locks[j].transform.parent != tempTransform.parent &&
                            !deactivatedLockIndexes.Contains(j))
                        {
                            if (Vector3.Distance(locks[j].transform.position, tempTransform.position) < curEdgeGateNearestLockDistance)
                            {
                                curEdgeGateNearestLockDistance = Vector3.Distance(locks[j].transform.position, tempTransform.position);
                            }
                        }
                    }

                    //lockNotConnectedGates
                    if (curEdgeGateNearestLockDistance < 6 * gridBreadth)
                    {
                        //Debug.Log("enter4");
                        //toSolid
                        tempTransform.GetComponent<TileData>().stateOfMatterIndex = 1;
                        for (int k = 0; k < tempTransform.childCount; k++)
                        {
                            tempTransform.GetChild(k).gameObject.SetActive(true);
                        }

                        tempTransform.GetComponent<MeshRenderer>().material = unconnectedGateColor;

                        //minimapGate
                        tempVector = UFL.Vector3WorldToMinimap(tempTransform.position);
                        curNearestMinimapGateDistance = 999;
                        for (int j = 0; j < minimapGates.Count; j++)
                        {
                            tempFloat = Vector3.Distance(minimapGates[j].transform.position, tempVector);
                            if (tempFloat < curNearestMinimapGateDistance)
                            {
                                curNearestMinimapGateDistance = tempFloat;
                                curNearestMinimapGateIndex = j;
                            }
                        }
                        minimapGates[curNearestMinimapGateIndex].GetComponent<MeshRenderer>().material = unconnectedGateColor;
                    }
                }
                #endregion

                VARS.IsToDetermineGatePassabilities = false;
            }
        }
    }

    //void IntoNewRoom()
    //{
    //    VARS.IsIntoNewRoom = false;

    //    VARS.IsInNewRoom = true;

    //    VARS.IsInNewRoomCurRoomManagerResetOver = false;
    //    VARS.IsInNewRoomCameraManagerResetOver = false;
    //    VARS.IsInNewRoomCatRotateResetOver = false;
    //    VARS.IsInNewRoomBlocksManagerResetOver = false;
    //}

    void ResetCurRelatedPlanes()
    {
        for(int i = 0; i < curRelatedRoomPlanes.Count; i++)
        {
            tempFloat1 = curRelatedRoomPlanes[i].transform.position.x;
            tempFloat2 = curRelatedRoomPlanes[i].transform.position.y;
            tempFloat3 = curRelatedRoomPlanes[i].transform.position.z;
            tempFloat4 = Mathf.Max(Mathf.Abs(tempFloat1), Mathf.Abs(tempFloat2), Mathf.Abs(tempFloat3));

            //frontalFace
            if (tempFloat4 == Mathf.Abs(tempFloat3))
            {
                //frontFace
                if (tempFloat3 < 0)
                {
                    ResetCurRelatedPlane(curRelatedRoomPlaneIndexes[i], 1);
                }
                //backFace
                else
                {
                    ResetCurRelatedPlane(curRelatedRoomPlaneIndexes[i], 2);
                }
            }
            //profileFace
            else if (tempFloat4 == Mathf.Abs(tempFloat1))
            {
                //leftFace
                if(tempFloat1 < 0)
                {
                    ResetCurRelatedPlane(curRelatedRoomPlaneIndexes[i], 3);
                }
                //rightFace
                else
                {
                    ResetCurRelatedPlane(curRelatedRoomPlaneIndexes[i], 4);
                }
            }
            //horizontalFace
            else
            {
                //topFace
                if (tempFloat2 > 0)
                {
                    ResetCurRelatedPlane(curRelatedRoomPlaneIndexes[i], 5);
                }
                //bottomFace
                else
                {
                    ResetCurRelatedPlane(curRelatedRoomPlaneIndexes[i], 6);
                }
            }
        }
    }

    void ResetCurRelatedPlane(int roomIndex, int faceIndex)
    {
        roomPlanes[roomIndex].transform.SetParent(faces[faceIndex - 1].transform, true);

        roomPlanes[roomIndex].transform.position = new Vector3
            (Mathf.Round(tempFloat1), Mathf.Round(tempFloat2), Mathf.Round(tempFloat3));

        roomCenters[roomIndex] = roomPlanes[roomIndex].transform.position;

        roomStableForwards[roomIndex] = faceStableForwards[faceIndex - 1];
        roomStableUps[roomIndex] = faceStableUps[faceIndex - 1];
        roomStableRights[roomIndex] = faceStableRights[faceIndex - 1];
    }
}
