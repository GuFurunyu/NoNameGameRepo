using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Video;

[DefaultExecutionOrder((int)ScriptsExecutionOrder.ExecutionOrder.universalFunctionsLibrary)]
public class UniversalFunctionsLibrary : MonoBehaviour
{
    Constants CONS;
    Variables VARS;

    GameObject gameManager;

    bool hasGotCurTriggerBlock;
    bool hasGotCurNearestUpBlock;
    bool hasGotCurNearestDownBlock;
    bool hasGotCurNearestLeftBlock;
    bool hasGotCurNearestRightBlock;
    bool hasGotCurNearestLiquidBlock;
    bool hasGotCurGasBlock;
    bool hasGotCurMistBlock;

    float curUpBlockDistance;
    float curDownBlockDistance;
    float curLeftBlockDistance;
    float curRightBlockDistance;

    float curUpBlockHorDistance;
    float curDownBlockHorDistance;
    float curLeftBlockVerDistance;
    float curRightBlockVerDistance;

    bool isFullyDrown;

    float curNearestLiquidBlockDistance;
    float curNearestLiquidBlockVerticalDistance;

    float curRotatingDirectionNearestMinimapRotationCameraPointDistance;
    float curRestrictDirectiongNearestMinimapRotationCameraPointDistance;
    float curForwardNearestMinimapRotationCameraPointDistance;
    //float curForwardNearestMinimapRotationCameraPointIndex;
    //float curRotatingDirectionNearestMinimapRotationCameraPointIndex;


    int tempInt;
    float tempFloat;
    float tempFloat1;
    float tempFloat2;
    float tempFloat3;
    float tempFloat4;
    float[] tempFloats = new float[3];
    Vector3 tempVector;
    Vector3 tempVector1;
    Vector3 tempVector2;
    Vector3 tempVector3;
    Quaternion tempQuaternion;
    GameObject tempGameObject;

    #region ConstantsUsed
    float gridBreadth;
    int roomCoordBreadth;
    int minimapRoomCoordBreadth;

    float inRoomMaxForwardDistance;

    Vector3[] faceStableForwards = new Vector3[6];
    Vector3[] faceStableUps = new Vector3[6];
    Vector3[] faceStableRights = new Vector3[6];

    GameObject[] roomPlanes = new GameObject[54];

    GameObject[] twistingCenters = new GameObject[6];

    List<GameObject> edgeGates = new List<GameObject>();
    List<GameObject> edgeGateTriggers = new List<GameObject>();

    Camera cam;
    Transform camTransform;

    float camNormalSize;
    float camMinimapSize;

    float camMinimapDistanceToCubeCore;

    GameObject cat;
    Transform catTransform;

    float stuckWarmupTime;
    float sandStuckWarmupTime;

    float horMaxSpeed;
    float verMaxSpeed;

    float temperatureTransferSpeed;
    float electricityTransferSpeed;
    float toxicityTransferSpeed;

    float intoVoidEnergyLost;

    float intoVoidGapTime;
    float intoVoidWarmupTime;

    float activateSavePointGapTime;

    GameObject[] minimapFaces = new GameObject[6];
    GameObject[] minimapRoomPlanes = new GameObject[54];
    GameObject[] minimapTwistingCenters = new GameObject[6];
    GameObject[] minimapRotationCameraPoints = new GameObject[26];
    //Vector3[] minimapRotationCameraPointStableUps = new Vector3[26];
    //Vector3[] minimapRotationCameraPointStableRights = new Vector3[26];
    //GameObject[] minimapRotationCameraUpPoints = new GameObject[26];
    //GameObject[] minimapRotationCameraDownPoints = new GameObject[26];
    //GameObject[] minimapRotationCameraLeftPoints = new GameObject[26];
    //GameObject[] minimapRotationCameraRightPoints = new GameObject[26];

    GameObject[] minimapCenterTriangleEmpties = new GameObject[6];
    #endregion

    #region VariablesUsed
    Vector3[] roomCenters = new Vector3[54];
    Vector3[] roomStableForwards = new Vector3[54];
    Vector3[] roomStableUps = new Vector3[54];
    Vector3[] roomStableRights = new Vector3[54];

    bool[] isRedFragmentsEmbeded = new bool[9];
    bool[] isYellowFragmentsEmbeded = new bool[9];
    bool[] isBlueFragmentsEmbeded = new bool[9];
    bool[] isOrangeFragmentsEmbeded = new bool[9];
    bool[] isGreenFragmentsEmbeded = new bool[9];
    bool[] isPurpleFragmentsEmbeded = new bool[9];

    List<GameObject> curBlocks = new List<GameObject>();
    List<TileData> curBlockTileDatas = new List<TileData>();
    
    //storedBlocks
    GameObject[] storedSandBlocks = new GameObject[512];
    GameObject[] storedWaterBlocks = new GameObject[512];
    GameObject[] storedAcidBlocks = new GameObject[512];
    GameObject[] storedVaporBlocks = new GameObject[512];
    GameObject[] storedGasBlocks = new GameObject[512];
    GameObject[] storedElectricMistBlocks = new GameObject[512];
    GameObject[] storedLightElectricMistBlocks = new GameObject[512];

    List<GameObject> curToBeBrokenFragileRustBlocks = new List<GameObject>();
    List<float> curFragileRustBlockToBeBrokenStartTimes = new List<float>();
    #endregion

    private void Start()
    {
        gameManager = GameObject.Find("GameManager");

        CONS = gameManager.GetComponent<Constants>();
        VARS = gameManager.GetComponent<Variables>();

        #region ImportConstants
        gridBreadth = CONS.gridBreadth;
        roomCoordBreadth = CONS.roomCoordBreadth;
        minimapRoomCoordBreadth = CONS.minimapRoomCoordBreadth;
        inRoomMaxForwardDistance = CONS.inRoomMaxForwardDistance;
        faceStableForwards = CONS.faceStableForwards;
        faceStableUps = CONS.faceStableUps;
        faceStableRights = CONS.faceStableRights;
        roomPlanes = CONS.roomPlanes;
        twistingCenters = CONS.twistingCenters;
        edgeGates = CONS.edgeGates;
        edgeGateTriggers = CONS.edgeGateTriggers;
        cam = CONS.cam;
        camTransform = CONS.camTransform;
        camNormalSize = CONS.camNormalSize;
        camMinimapSize = CONS.camMinimapSize;
        camMinimapDistanceToCubeCore = CONS.camMinimapDistanceToCubeCore;
        cat = CONS.cat;
        catTransform = CONS.catTransform;
        stuckWarmupTime = CONS.stuckWarmupTime;
        sandStuckWarmupTime = CONS.sandStuckWarmupTime;
        horMaxSpeed = CONS.horMaxSpeed;
        verMaxSpeed = CONS.verMaxSpeed;
        temperatureTransferSpeed = CONS.temperatureTransferSpeed;
        electricityTransferSpeed = CONS.electricityTransferSpeed;
        toxicityTransferSpeed = CONS.toxicityTransferSpeed;
        intoVoidEnergyLost = CONS.intoVoidEnergyLost;
        intoVoidGapTime = CONS.intoVoidGapTime;
        intoVoidWarmupTime = CONS.intoVoidWarmupTime;
        activateSavePointGapTime = CONS.activateSavePointGapTime;
        minimapFaces = CONS.minimapFaces;
        minimapRoomPlanes = CONS.minimapRoomPlanes;
        minimapTwistingCenters = CONS.minimapTwistingCenters;
        minimapRotationCameraPoints = CONS.minimapRotationCameraPoints;
        minimapCenterTriangleEmpties = CONS.minimapCenterTriangleEmpties;
        #endregion

        #region ImportReferenceVariables
        roomCenters = VARS.roomCenters;
        roomStableForwards = VARS.roomStableForwards;
        roomStableUps = VARS.roomStableUps;
        roomStableRights = VARS.roomStableRights;
        isRedFragmentsEmbeded = VARS.isRedFragmentsEmbeded;
        isYellowFragmentsEmbeded = VARS.isYellowFragmentsEmbeded;
        isBlueFragmentsEmbeded = VARS.isBlueFragmentsEmbeded;
        isOrangeFragmentsEmbeded = VARS.isOrangeFragmentsEmbeded;
        isGreenFragmentsEmbeded = VARS.isGreenFragmentsEmbeded;
        isPurpleFragmentsEmbeded = VARS.isPurpleFragmentsEmbeded;
        curBlocks = VARS.curBlocks;
        curBlockTileDatas = VARS.curBlockTileDatas;
        storedSandBlocks = VARS.storedSandBlocks;
        storedWaterBlocks = VARS.storedWaterBlocks;
        storedAcidBlocks = VARS.storedAcidBlocks;
        storedVaporBlocks = VARS.storedVaporBlocks;
        storedGasBlocks = VARS.storedGasBlocks;
        storedElectricMistBlocks = VARS.storedElectricMistBlocks;
        storedLightElectricMistBlocks = VARS.storedLightElectricMistBlocks;
        curToBeBrokenFragileRustBlocks = VARS.curToBeBrokenFragileRustBlocks;
        curFragileRustBlockToBeBrokenStartTimes = VARS.curFragileRustBlockToBeBrokenStartTimes;
        #endregion
    }

    private void Update()
    {
        #region ImportValueVariables
        #endregion
    }

    #region Debug
    [Conditional("UNITY_EDITOR")]
    public void DebugLog()
    {
        UnityEngine.Debug.Log("debug " + VARS.debugCount++);
    }

    [Conditional("UNITY_EDITOR")]
    public void DebugLog(string s)
    {
        UnityEngine.Debug.Log("debug " + s);
    }
    #endregion

    #region Universal
    public bool EqualToZero(float f)
    {
        return Mathf.Abs(f) < 1e-8f;
    }

    public Vector3 Vector3Abs(Vector3 vector)
    {
        return new Vector3(Mathf.Abs(vector.x), Mathf.Abs(vector.y), Mathf.Abs(vector.z));
    }

    public Vector3 Vector3RoundToInt(Vector3 vector)
    {
        return new Vector3(Mathf.RoundToInt(vector.x), Mathf.RoundToInt(vector.y), Mathf.RoundToInt(vector.z));
    }

    public Vector3 Vector3WorldToMinimap(Vector3 vector)
    {
        tempVector = vector;
        //frontal
        if (Mathf.Abs(tempVector.z) > Mathf.Abs(tempVector.x) && Mathf.Abs(tempVector.z) > Mathf.Abs(tempVector.y))
        {
            if (tempVector.x > roomCoordBreadth / 2)
            {
                tempVector.x -= roomCoordBreadth + 1;
                tempVector.x = (tempVector.x / roomCoordBreadth) * minimapRoomCoordBreadth;
                tempVector.x += minimapRoomCoordBreadth + 1;
            }
            else if (tempVector.x < -roomCoordBreadth / 2)
            {
                tempVector.x += roomCoordBreadth + 1;
                tempVector.x = (tempVector.x / roomCoordBreadth) * minimapRoomCoordBreadth;
                tempVector.x -= minimapRoomCoordBreadth + 1;
            }
            else
            {
                tempVector.x = (tempVector.x / roomCoordBreadth) * minimapRoomCoordBreadth;
            }
            if (tempVector.y > roomCoordBreadth / 2)
            {
                tempVector.y -= roomCoordBreadth + 1;
                tempVector.y = (tempVector.y / roomCoordBreadth) * minimapRoomCoordBreadth;
                tempVector.y += minimapRoomCoordBreadth + 1;
            }
            else if (tempVector.y < -roomCoordBreadth / 2)
            {
                tempVector.y += roomCoordBreadth + 1;
                tempVector.y = (tempVector.y / roomCoordBreadth) * minimapRoomCoordBreadth;
                tempVector.y -= minimapRoomCoordBreadth + 1;
            }
            else
            {
                tempVector.y = (tempVector.y / roomCoordBreadth) * minimapRoomCoordBreadth;
            }
        }
        //profile
        else if (Mathf.Abs(tempVector.x) > Mathf.Abs(tempVector.y) && Mathf.Abs(tempVector.x) > Mathf.Abs(tempVector.z))
        {
            if (tempVector.y > roomCoordBreadth / 2)
            {
                tempVector.y -= roomCoordBreadth + 1;
                tempVector.y = (tempVector.y / roomCoordBreadth) * minimapRoomCoordBreadth;
                tempVector.y += minimapRoomCoordBreadth + 1;
            }
            else if (tempVector.y < -roomCoordBreadth / 2)
            {
                tempVector.y += roomCoordBreadth + 1;
                tempVector.y = (tempVector.y / roomCoordBreadth) * minimapRoomCoordBreadth;
                tempVector.y -= minimapRoomCoordBreadth + 1;
            }
            else
            {
                tempVector.y = (tempVector.y / roomCoordBreadth) * minimapRoomCoordBreadth;
            }
            if (tempVector.z > roomCoordBreadth / 2)
            {
                tempVector.z -= roomCoordBreadth + 1;
                tempVector.z = (tempVector.z / roomCoordBreadth) * minimapRoomCoordBreadth;
                tempVector.z += minimapRoomCoordBreadth + 1;
            }
            else if (tempVector.z < -roomCoordBreadth / 2)
            {
                tempVector.z += roomCoordBreadth + 1;
                tempVector.z = (tempVector.z / roomCoordBreadth) * minimapRoomCoordBreadth;
                tempVector.z -= minimapRoomCoordBreadth + 1;
            }
            else
            {
                tempVector.z = (tempVector.z / roomCoordBreadth) * minimapRoomCoordBreadth;
            }
        }
        //horizontal
        else
        {
            if (tempVector.x > roomCoordBreadth / 2)
            {
                tempVector.x -= roomCoordBreadth + 1;
                tempVector.x = (tempVector.x / roomCoordBreadth) * minimapRoomCoordBreadth;
                tempVector.x += minimapRoomCoordBreadth + 1;
            }
            else if (tempVector.x < -roomCoordBreadth / 2)
            {
                tempVector.x += roomCoordBreadth + 1;
                tempVector.x = (tempVector.x / roomCoordBreadth) * minimapRoomCoordBreadth;
                tempVector.x -= minimapRoomCoordBreadth + 1;
            }
            else
            {
                tempVector.x = (tempVector.x / roomCoordBreadth) * minimapRoomCoordBreadth;
            }
            if (tempVector.z > roomCoordBreadth / 2)
            {
                tempVector.z -= roomCoordBreadth + 1;
                tempVector.z = (tempVector.z / roomCoordBreadth) * minimapRoomCoordBreadth;
                tempVector.z += minimapRoomCoordBreadth + 1;
            }
            else if (tempVector.z < -roomCoordBreadth / 2)
            {
                tempVector.z += roomCoordBreadth + 1;
                tempVector.z = (tempVector.z / roomCoordBreadth) * minimapRoomCoordBreadth;
                tempVector.z -= minimapRoomCoordBreadth + 1;
            }
            else
            {
                tempVector.z = (tempVector.z / roomCoordBreadth) * minimapRoomCoordBreadth;
            }
        }

        return tempVector;
    }
    #endregion

    #region RoomsManager
    public bool IsInRoom(int roomIndex, Vector3 position)
    {
        tempVector = position - roomCenters[roomIndex];

        //ifIsInThePlane
        if (Mathf.Abs(Vector3.Dot(tempVector, roomStableForwards[roomIndex])) <= inRoomMaxForwardDistance)
        {
            //ifIsInsideTheBoundary
            if (Mathf.Abs(Vector3.Dot(tempVector, roomStableUps[roomIndex])) <= (roomCoordBreadth / 2 + 1) * gridBreadth &&
                Mathf.Abs(Vector3.Dot(tempVector, roomStableRights[roomIndex])) <= (roomCoordBreadth / 2 + 1) * gridBreadth)
            {
                return true;
            }
        }

        return false;
    }

    public void HideAllPlanes()
    {
        for (int i = 0; i < 54; i++)
        {
            roomPlanes[i].SetActive(false);
        }
    }

    public void HideOtherPlanes()
    {
        for (int i = 0; i < 54; i++)
        {
            if (i != VARS.curRoomIndex)
            {
                roomPlanes[i].SetActive(false);
            }
            else
            {
                roomPlanes[i].SetActive(true);
            }
        }
    }

    public bool IsPlaneInTheFace(int planeIndex, int faceIndex)
    {
        tempVector = roomCenters[planeIndex] - twistingCenters[faceIndex - 1].transform.position;
        tempFloat = Mathf.Abs(Vector3.Dot(tempVector, faceStableForwards[faceIndex - 1]));

        if (tempFloat <= (roomCoordBreadth / 2 + 2) * gridBreadth &&
            tempFloat > 3 * gridBreadth)
        {
            return true;
        }

        return false;
    }

    public bool IsPlaneSurroundingTheFace(int planeIndex, int faceIndex)
    {
        tempVector = roomCenters[planeIndex] - twistingCenters[faceIndex - 1].transform.position;
        tempFloat = Mathf.Abs(Vector3.Dot(tempVector, faceStableForwards[faceIndex - 1]));

        if (tempFloat <= 3 * gridBreadth)
        {
            return true;
        }

        return false;
    }

    public bool IsRoomExplored(int roomIndex)
    {
        return VARS.IsRoomExplored[roomIndex];
    }

    public void SetMinimapRoomPlanesByRoomPlanes()
    {
        for (int i = 0; i < 54; i++)
        {
            //position
            tempVector = roomPlanes[i].transform.position;
            tempFloats[0] = tempVector.x;
            tempFloats[1] = tempVector.y;
            tempFloats[2] = tempVector.z;

            for (int j = 0; j < 3; j++)
            {
                if (tempFloats[j] == (roomCoordBreadth + 1) * gridBreadth ||
                    tempFloats[j] == -(roomCoordBreadth + 1) * gridBreadth)
                {
                    tempFloats[j] *= ((minimapRoomCoordBreadth + 1) * gridBreadth) / ((roomCoordBreadth + 1) * gridBreadth);
                }
                else if (tempFloats[j] == (roomCoordBreadth * 1.5f + 2) * gridBreadth ||
                    tempFloats[j] == -(roomCoordBreadth * 1.5f + 2) * gridBreadth)
                {
                    tempFloats[j] *= ((minimapRoomCoordBreadth * 1.5f + 2) * gridBreadth) / ((roomCoordBreadth * 1.5f + 2) * gridBreadth);
                }
            }

            tempVector = new Vector3(tempFloats[0], tempFloats[1], tempFloats[2]);
            minimapRoomPlanes[i].transform.position = Vector3RoundToInt(tempVector);

            //eulerangles
            minimapRoomPlanes[i].transform.eulerAngles = roomPlanes[i].transform.eulerAngles;
        }
    }

    public bool IsMinimapPlaneInTheFace(int planeIndex, int faceIndex)
    {
        tempVector = minimapRoomPlanes[planeIndex].transform.position - minimapTwistingCenters[faceIndex - 1].transform.position;
        tempFloat = Mathf.Abs(Vector3.Dot(tempVector, faceStableForwards[faceIndex - 1]));

        if (tempFloat <= (minimapRoomCoordBreadth / 2 + 2) * gridBreadth &&
            tempFloat > 3 * gridBreadth)
        {
            return true;
        }

        return false;
    }

    public void HideAllMinimapPlanes()
    {
        for (int i = 0; i < 54; i++)
        {
            minimapRoomPlanes[i].SetActive(false);
        }
    }

    public void ActivateMinimapPlanes()
    {
        for (int i = 0; i < 54; i++)
        {
            minimapRoomPlanes[i].SetActive(IsRoomExplored(i));
        }
    }

    public void IntoMinimap()
    {
        roomPlanes[VARS.curRoomIndex].SetActive(false);
        for (int i = 0; i < 54; i++)
        {
            minimapRoomPlanes[i].SetActive(IsRoomExplored(i));
            //minimapRoomPlanes[i].SetActive(true);

            //setCurRoomMinimapPlaneWhite
            if (i == VARS.curRoomIndex)
            {
                //VARS.curMinimapRoomPlaneColor = minimapRoomPlanes[i].GetComponent<MeshRenderer>().material.GetColor("_MainColor");
                //minimapRoomPlanes[i].GetComponent<MeshRenderer>().material.SetColor("_MainColor", Color.white);
                minimapRoomPlanes[i].transform.GetChild(2).gameObject.SetActive(true);
                minimapRoomPlanes[i].transform.GetChild(3).gameObject.SetActive(true);
                minimapRoomPlanes[i].transform.GetChild(4).gameObject.SetActive(true);
                minimapRoomPlanes[i].GetComponent<MeshRenderer>().material.SetFloat("_OutlineWidth", 0.5f);

                if ((i - 4) % 9 == 0)
                {
                    //tempGameObject = minimapCenterTriangleEmpties[(i - 4) / 9];
                    //tempGameObject.transform.GetChild(0).gameObject.SetActive(false);
                    //tempGameObject.transform.GetChild(1).gameObject.SetActive(true);
                }
            }
            else if ((i - 4) % 9 == 0)
            {
                //tempGameObject = minimapCenterTriangleEmpties[(i - 4) / 9];
                //tempGameObject.SetActive(IsRoomExplored(i));
            }

            //roomPlanes[i].SetActive(false);
        }

        //cat.GetComponent<MeshRenderer>().enabled = false;

        //for (int i = 0; i < cat.transform.childCount; i++)
        //{
        //    cat.transform.GetChild(i).gameObject.SetActive(false);
        //}

        //if (VARS.IsCarryingAKey)
        //{
        //    VARS.curCarriedKey.SetActive(false);
        //}

        //if (VARS.IsCarryingFragments)
        //{
        //    for(int i=0;i<VARS.curCarriedFragments.Count; i++)
        //    {
        //        VARS.curCarriedFragments[i].SetActive(false);
        //    }
        //}

        //deactivateCurStoredBlocks
        if (VARS.curStoredSandBlockIndex > 0)
        {
            for (int i = 0; i < VARS.curStoredSandBlockIndex + 1; i++)
            {
                storedSandBlocks[i].SetActive(false);
            }
            VARS.curStoredSandBlockIndex = 0;
        }
        if (VARS.curStoredWaterBlockIndex > 0)
        {
            for (int i = 0; i < VARS.curStoredWaterBlockIndex + 1; i++)
            {
                storedWaterBlocks[i].SetActive(false);
            }
            VARS.curStoredWaterBlockIndex = 0;
        }
        if (VARS.curStoredAcidBlockIndex > 0)
        {
            for (int i = 0; i < VARS.curStoredAcidBlockIndex + 1; i++)
            {
                storedAcidBlocks[i].SetActive(false);
            }
            VARS.curStoredAcidBlockIndex = 0;
        }
        if (VARS.curStoredVaporBlockIndex > 0)
        {
            for (int i = 0; i < VARS.curStoredVaporBlockIndex + 1; i++)
            {
                storedVaporBlocks[i].SetActive(false);
            }
            VARS.curStoredVaporBlockIndex = 0;
        }
        if (VARS.curStoredGasBlockIndex > 0)
        {
            for (int i = 0; i < VARS.curStoredGasBlockIndex + 1; i++)
            {
                storedGasBlocks[i].SetActive(false);
            }
            VARS.curStoredGasBlockIndex = 0;
        }
        if (VARS.curStoredElectricMistBlockIndex > 0)
        {
            for (int i = 0; i < VARS.curStoredElectricMistBlockIndex + 1; i++)
            {
                storedElectricMistBlocks[i].SetActive(false);
            }
            VARS.curStoredElectricMistBlockIndex = 0;
        }
        if (VARS.curStoredLightElectricMistBlockIndex > 0)
        {
            for (int i = 0; i < VARS.curStoredLightElectricMistBlockIndex + 1; i++)
            {
                storedLightElectricMistBlocks[i].SetActive(false);
            }
            VARS.curStoredLightElectricMistBlockIndex = 0;
        }

        //camPosition
        SetCameraPosition(-VARS.curRoomStableForward * camMinimapDistanceToCubeCore);

        //camEulerangles
        VARS.camEuleranglesBeforeIntoMinimap = camTransform.eulerAngles;

        //camSize
        SetCameraSize(camMinimapSize);
    }

    public void OutOfMinimap()
    {
        roomPlanes[VARS.curRoomIndex].SetActive(true);
        for (int i = 0; i < 54; i++)
        {
            minimapRoomPlanes[i].SetActive(false);

            //resetMinimapPlaneColor
            if (i == VARS.curRoomIndex)
            {
                //minimapRoomPlanes[i].GetComponent<MeshRenderer>().material.SetColor("_MainColor", VARS.curMinimapRoomPlaneColor);
                minimapRoomPlanes[i].transform.GetChild(2).gameObject.SetActive(false);
                minimapRoomPlanes[i].transform.GetChild(3).gameObject.SetActive(false);
                minimapRoomPlanes[i].transform.GetChild(4).gameObject.SetActive(false);
                minimapRoomPlanes[i].GetComponent<MeshRenderer>().material.SetFloat("_OutlineWidth", 0.01f);

                if ((i - 4) % 9 == 0)
                {
                    //tempGameObject = minimapCenterTriangleEmpties[(i - 4) / 9];
                    //tempGameObject.transform.GetChild(0).gameObject.SetActive(true);
                    //tempGameObject.transform.GetChild(1).gameObject.SetActive(false);
                }
            }

            //roomPlanes[i].SetActive(true);
        }

        //cat.GetComponent<MeshRenderer>().enabled = true;

        //for(int i=0;i<cat.transform.childCount; i++)
        //{
        //    cat.transform.GetChild(i).gameObject.SetActive(true);
        //}

        //if (VARS.IsCarryingAKey)
        //{
        //    VARS.curCarriedKey.SetActive(true);
        //}

        //if (VARS.IsCarryingFragments)
        //{
        //    for (int i = 0; i < VARS.curCarriedFragments.Count; i++)
        //    {
        //        VARS.curCarriedFragments[i].SetActive(true);
        //    }
        //}

        //reactivateCurStoredBlocks
        if (VARS.curStoredSandBlockIndex > 0)
        {
            for (int i = 0; i < VARS.curStoredSandBlockIndex + 1; i++)
            {
                storedSandBlocks[i].SetActive(true);
            }
            VARS.curStoredSandBlockIndex = 0;
        }
        if (VARS.curStoredWaterBlockIndex > 0)
        {
            for (int i = 0; i < VARS.curStoredWaterBlockIndex + 1; i++)
            {
                storedWaterBlocks[i].SetActive(true);
            }
            VARS.curStoredWaterBlockIndex = 0;
        }
        if (VARS.curStoredAcidBlockIndex > 0)
        {
            for (int i = 0; i < VARS.curStoredAcidBlockIndex + 1; i++)
            {
                storedAcidBlocks[i].SetActive(true);
            }
            VARS.curStoredAcidBlockIndex = 0;
        }
        if (VARS.curStoredVaporBlockIndex > 0)
        {
            for (int i = 0; i < VARS.curStoredVaporBlockIndex + 1; i++)
            {
                storedVaporBlocks[i].SetActive(true);
            }
            VARS.curStoredVaporBlockIndex = 0;
        }
        if (VARS.curStoredGasBlockIndex > 0)
        {
            for (int i = 0; i < VARS.curStoredGasBlockIndex + 1; i++)
            {
                storedGasBlocks[i].SetActive(true);
            }
            VARS.curStoredGasBlockIndex = 0;
        }
        if (VARS.curStoredElectricMistBlockIndex > 0)
        {
            for (int i = 0; i < VARS.curStoredElectricMistBlockIndex + 1; i++)
            {
                storedElectricMistBlocks[i].SetActive(true);
            }
            VARS.curStoredElectricMistBlockIndex = 0;
        }
        if (VARS.curStoredLightElectricMistBlockIndex > 0)
        {
            for (int i = 0; i < VARS.curStoredLightElectricMistBlockIndex + 1; i++)
            {
                storedLightElectricMistBlocks[i].SetActive(true);
            }
            VARS.curStoredLightElectricMistBlockIndex = 0;
        }

        //camPosition
        SetCameraPosition(VARS.curRoomCenter - VARS.curRoomStableForward * 7);

        //camEulerangles
        SetCameraEulerangles(VARS.camEuleranglesBeforeIntoMinimap);

        //camSize
        SetCameraSize(camNormalSize);

        VARS.IsJustOutOfMinimap = true;
    }
    #endregion

    #region TileData
    //public int GetBlockStateOfMatterIndex(GameObject block)
    //{
    //    if (block.GetComponent<TileData>() != null)
    //    {
    //        return block.GetComponent<TileData>().stateOfMatterIndex;
    //    }

    //    return 0;
    //}

    //public int GetBlocksStateOfMatterIndex(TileData tileData)
    //{
    //    return tileData.stateOfMatterIndex;
    //}
    #endregion

    #region CameraManager
    public void SetCameraPosition(Vector3 position)
    {
        camTransform.position = position;
    }

    public void CameraMove(Vector3 movingVector)
    {
        camTransform.position += movingVector;
    }

    public void SetCameraEulerangles(Vector3 targetEulerangles)
    {
        camTransform.eulerAngles = targetEulerangles;
    }

    public void CameraRotate(float rotationStep)
    {
        camTransform.Rotate(0, 0, rotationStep);
    }

    //dirIndex:
    //1-up, 2-down, 3-left, 4-right
    public void MinimapCameraRotate(int dirIndex, float rotationMovingStep)
    {
        tempVector = (VARS.curToMinimapRotationCameraPoint.transform.position - VARS.curMinimapRotationCameraPoint.transform.position).normalized;

        CameraMove(tempVector * rotationMovingStep);

        //tempVector = camTransform.eulerAngles;

        //camTransform.LookAt(Vector3.zero);

        //SetCameraEulerangles(new Vector3(camTransform.eulerAngles.x, camTransform.eulerAngles.y, tempVector.z));

        //// 保存LookAt前的z轴旋转
        //float z = camTransform.eulerAngles.z;
        ////// LookAt会重置rotation
        ////camTransform.LookAt(Vector3.zero);
        ////// 用四元数叠加z轴旋转
        ////camTransform.rotation = Quaternion.Euler(camTransform.eulerAngles.x, camTransform.eulerAngles.y, z);

        //Quaternion lookAtRotation = Quaternion.LookRotation(Vector3.zero - camTransform.position, Vector3.up);
        //Quaternion zRotation = Quaternion.AngleAxis(z, Vector3.forward);
        //camTransform.rotation = lookAtRotation * zRotation;

        camTransform.LookAt(Vector3.zero,camTransform.up);
    }

    public void GetCurToMinimapRotationCameraPoint(int dirIndex)
    {
        //getCurIndexAndCurPoint
        if (VARS.IsMinimapRotationCameraPointIndexNotInitialized)
        {
            VARS.curMinimapRotationCameraPointIndex = VARS.curFaceIndex - 1;

            VARS.IsMinimapRotationCameraPointIndexNotInitialized = false;
        }
        VARS.curMinimapRotationCameraPoint = minimapRotationCameraPoints[VARS.curMinimapRotationCameraPointIndex];
        //Debug.Log("curIndex: " + VARS.curMinimapRotationCameraPointIndex);

        //switch (dirIndex)
        //{
        //    case 1:
        //        VARS.curToMinimapRotationCameraPoint = minimapRotationCameraDownPoints[VARS.curMinimapRotationCameraPointIndex];
        //        break;
        //    case 2:
        //        VARS.curToMinimapRotationCameraPoint = minimapRotationCameraUpPoints[VARS.curMinimapRotationCameraPointIndex];
        //        break;
        //    case 3:
        //        VARS.curToMinimapRotationCameraPoint = minimapRotationCameraRightPoints[VARS.curMinimapRotationCameraPointIndex];
        //        break;
        //    case 4:
        //        VARS.curToMinimapRotationCameraPoint = minimapRotationCameraLeftPoints[VARS.curMinimapRotationCameraPointIndex];
        //        break;
        //}

        //Debug.Log("camUp: " + camTransform.up);
        //Debug.Log("camDown: " + -camTransform.up);
        //Debug.Log("camLeft: " + -camTransform.right);
        //Debug.Log("camRight: " + camTransform.right);

        ////getCurToIndexAndCurToPoint
        //switch (dirIndex)
        //{
        //    case 1:
        //        //tempVector = -minimapRotationCameraPointStableUps[VARS.curMinimapRotationCameraPointIndex];
        //        tempVector = -camTransform.up;
        //        //tempQuaternion = camTransform.rotation * Quaternion.AngleAxis(camTransform.eulerAngles.z, camTransform.forward);
        //        //tempVector = tempQuaternion.eulerAngles;
        //        break;
        //    case 2:
        //        //tempVector = minimapRotationCameraPointStableUps[VARS.curMinimapRotationCameraPointIndex];
        //        tempVector = camTransform.up;
        //        break;
        //    case 3:
        //        //tempVector = minimapRotationCameraPointStableRights[VARS.curMinimapRotationCameraPointIndex];
        //        tempVector = camTransform.right;
        //        break;
        //    case 4:
        //        //tempVector = -minimapRotationCameraPointStableRights[VARS.curMinimapRotationCameraPointIndex];
        //        tempVector = -camTransform.right;
        //        break;
        //}

        //getCurToIndexAndCurToPoint
        switch (dirIndex)
        {
            case 1:
                //tempVector = -minimapRotationCameraPointStableUps[VARS.curMinimapRotationCameraPointIndex];
                tempVector1 = -camTransform.up;
                tempVector2 = camTransform.right;
                //tempQuaternion = camTransform.rotation * Quaternion.AngleAxis(camTransform.eulerAngles.z, camTransform.forward);
                //tempVector = tempQuaternion.eulerAngles;
                break;
            case 2:
                //tempVector = minimapRotationCameraPointStableUps[VARS.curMinimapRotationCameraPointIndex];
                tempVector1 = camTransform.up;
                tempVector2 = camTransform.right;
                break;
            case 3:
                //tempVector = minimapRotationCameraPointStableRights[VARS.curMinimapRotationCameraPointIndex];
                tempVector1 = camTransform.right;
                tempVector2 = camTransform.up;
                break;
            case 4:
                //tempVector = -minimapRotationCameraPointStableRights[VARS.curMinimapRotationCameraPointIndex];
                tempVector1 = -camTransform.right;
                tempVector2 = camTransform.up;
                break;
        }

        tempVector3 = camTransform.forward;

        ////Debug.Log("tempVector: " + tempVector);
        //for (int i = 0; i < 26; i++)
        //{
        //    //Debug.Log("enter1");

        //    //if (i == 4)
        //    //{
        //    //    Debug.Log(Vector3.Angle(minimapRotationCameraPoints[i].transform.position - VARS.curMinimapRotationCameraPoint.transform.position, tempVector));
        //    //    Debug.Log(Vector3.Dot(minimapRotationCameraPoints[i].transform.position - VARS.curMinimapRotationCameraPoint.transform.position, tempVector));
        //    //}

        //    //ifIsNearlyOnTheLine
        //    if (Vector3.Angle(minimapRotationCameraPoints[i].transform.position - VARS.curMinimapRotationCameraPoint.transform.position, tempVector) < /*30*/ 31)
        //    {
        //        UnityEngine.Debug.Log("enter1");
        //        //Debug.Log("enter2: " + i);

        //        //Debug.Log("dot: " + Vector3.Dot(minimapRotationCameraPoints[i].transform.position - VARS.curMinimapRotationCameraPoint.transform.position, tempVector));

        //        //ifIsTheRightDirection
        //        if (Vector3.Dot(minimapRotationCameraPoints[i].transform.position - VARS.curMinimapRotationCameraPoint.transform.position, tempVector) > 0)
        //        {
        //            UnityEngine.Debug.Log("enter2");
        //            //Debug.Log("enter3");

        //            //Debug.Log("curToIndex: " + i);

        //            //Debug.Log(VARS.curToMinimapRotationCameraPoint.transform.position - VARS.curMinimapRotationCameraPoint.transform.position);

        //            VARS.curToMinimapRotationCameraPointIndex = i;
        //            VARS.curToMinimapRotationCameraPoint = minimapRotationCameraPoints[i];
        //            break;
        //        }
        //    }
        //}

        //curRotatingDirectionNearestMinimapRotationCameraPointDistance = 999;
        //curRestrictDirectiongNearestMinimapRotationCameraPointDistance = 999;
        //curForwardNearestMinimapRotationCameraPointDistance = 999;

        //UnityEngine.Debug.Log(tempVector1);
        //UnityEngine.Debug.Log(tempVector2);
        //UnityEngine.Debug.Log(tempVector3);

        for (int i = 0; i < 26; i++)
        {
            tempVector = minimapRotationCameraPoints[i].transform.position - VARS.curMinimapRotationCameraPoint.transform.position;

            tempFloat1 = Vector3.Dot(tempVector, tempVector1);
            tempFloat2 = Mathf.Abs(Vector3.Dot(tempVector, tempVector2));
            tempFloat3 = Vector3.Dot(tempVector, tempVector3);


            if (tempFloat1 > 0 && tempFloat1 < 21)
            {
                //UnityEngine.Debug.Log("enter1");

                if (tempFloat2 < 6.75)
                {
                    //UnityEngine.Debug.Log("enter2");

                    //UnityEngine.Debug.Log(tempFloat3);

                    if (tempFloat3 < 12)
                    {
                        //UnityEngine.Debug.Log("enter3");

                        VARS.curToMinimapRotationCameraPointIndex = i;
                        VARS.curToMinimapRotationCameraPoint = minimapRotationCameraPoints[i];

                        //UnityEngine.Debug.Log(VARS.curMinimapRotationCameraPoint.transform.position + "; " + minimapRotationCameraPoints[i].transform.position + ";" + tempVector);
                        //UnityEngine.Debug.Log("1 " + tempFloat1);
                        //UnityEngine.Debug.Log("2 " + tempFloat2);
                        //UnityEngine.Debug.Log("3 " + tempFloat3);

                        break;
                    }
                }
            }
        }
    }

    public void SetCameraSize(float size)
    {
        cam.orthographicSize = size;
    }

    public void AddCameraSize(float size)
    {
        cam.orthographicSize += size;
    }
    #endregion

    #region CatCollision
    public void GetCatCollisionInfo()
    {
        hasGotCurTriggerBlock = false;
        hasGotCurNearestUpBlock = false;
        hasGotCurNearestDownBlock = false;
        hasGotCurNearestLeftBlock = false;
        hasGotCurNearestRightBlock = false;
        hasGotCurNearestLiquidBlock = false;
        hasGotCurGasBlock = false;
        hasGotCurMistBlock = false;

        VARS.curTriggerTile = null;
        VARS.curTriggerTileData = null;
        VARS.curUpTile = null;
        VARS.curUpTileData = null;
        VARS.curDownTile = null;
        VARS.curDownTileData = null;
        VARS.curLeftTile = null;
        VARS.curLeftTileData = null;
        VARS.curRightTile = null;
        VARS.curRightTileData = null;
        VARS.curLiquidTileData = null;
        VARS.curGasTileData = null;
        VARS.curMistTileData = null;

        curUpBlockDistance = 999;
        curDownBlockDistance = 999;
        curLeftBlockDistance = 999;
        curRightBlockDistance = 999;

        curNearestLiquidBlockDistance = 999;

        isFullyDrown = false;

        VARS.IsTouchingAfflictingBlocks = false;

        for (int i = 0; i < curBlocks.Count; i++)
        {
            if (curBlocks[i].activeSelf == false ||
                curBlockTileDatas[i].isNotToBeDetected == true)
                continue;

            tempVector = catTransform.position - curBlocks[i].transform.position;

            //trigger
            if (curBlockTileDatas[i].stateOfMatterIndex == 0)
            {
                if (!hasGotCurTriggerBlock)
                {
                    if (Mathf.Abs(Vector3.Dot(tempVector, VARS.curUp)) < gridBreadth - 0.025f &&
                        Mathf.Abs(Vector3.Dot(tempVector, VARS.curRight)) < gridBreadth - 0.025f)
                    {
                        if (!(VARS.IsCarryingAKey && curBlockTileDatas[i].blockTypeIndex == 7007) &&
                            !(VARS.IsCarryingStrawberries && VARS.carriedStrawberries.Contains(curBlocks[i])))
                        {
                            VARS.curTriggerTile = curBlocks[i];
                            VARS.curTriggerTileData = curBlockTileDatas[i];

                            hasGotCurTriggerBlock = true;
                        }
                    }
                }
            }
            
            //solid
            if (curBlockTileDatas[i].stateOfMatterIndex == 1)
            {
                //ifStuckDie
                if (!curBlockTileDatas[i].isPlatform &&
                    !curBlockTileDatas[i].isFragile &&
                    curBlockTileDatas[i].railBlockIndex == 0)
                {
                    if (Mathf.Abs(Vector3.Dot(tempVector, VARS.curUp)) < gridBreadth / 8 &&
                            Mathf.Abs(Vector3.Dot(tempVector, VARS.curRight)) < gridBreadth / 8)
                    {
                        if (curBlockTileDatas[i].blockTypeIndex != 2103)
                        {
                            if (Mathf.Abs(VARS.stuckStartTime - 0) < 1e-6)
                            {
                                VARS.stuckStartTime = Time.time;
                            }
                            else if (Time.time - VARS.stuckStartTime > stuckWarmupTime)
                            {
                                VARS.stuckStartTime = 0;

                                VARS.IsToDie = true;

                                break;
                            }
                        }
                        else
                        {
                            if (!VARS.IsJustByGate)
                            {
                                if (Mathf.Abs(VARS.sandStuckStartTime - 0) < 1e-6)
                                {
                                    VARS.sandStuckStartTime = Time.time;
                                }
                                else if (Time.time - VARS.sandStuckStartTime > sandStuckWarmupTime)
                                {
                                    VARS.sandStuckStartTime = 0;

                                    VARS.IsToDie = true;

                                    break;
                                }
                            }
                        }
                    }
                }

                //up
                if ((!hasGotCurNearestUpBlock &&
                !curBlockTileDatas[i].isPlatform) ||
                curBlockTileDatas[i].isFragile)
                {
                    tempFloat1 = Mathf.Abs(Vector3.Dot(tempVector, VARS.curRight));

                    if (tempFloat1 < gridBreadth - 0.025f)
                    {
                        tempFloat = Vector3.Dot(tempVector, -VARS.curUp);

                        if (tempFloat > 0.5f &&
                            tempFloat < gridBreadth + 0.1f /*0.01f*/ &&
                            tempFloat > tempFloat1)
                        {
                            if (tempFloat < curUpBlockDistance)
                            {
                                curUpBlockDistance = tempFloat;

                                curUpBlockHorDistance = tempFloat1;

                                if (/*VARS.IsLeftBlocked || VARS.IsRightBlocked*/
                                    //VARS.IsToCeiling &&
                                    !VARS.IsLeftBlocked && !VARS.IsRightBlocked &&
                                    curUpBlockHorDistance > /*0.9f*/ 0.8f &&
                                    !VARS.IsMovingInAttachingCeiling &&
                                    Mathf.Abs(VARS.horCurSpeed) < 1)
                                {
                                    if (VARS.IsHighJumping ||
                                        VARS.IsInputtingJumpKey ||
                                        VARS.IsInputtingUpKey /*||
                                        VARS.IsInputtingDownKey*/)
                                        tempFloat4 = (0.05f - (Mathf.Abs(VARS.horCurSpeed) / horMaxSpeed) / 100) /** 16*/ /** 12*/ * 8 /** 4*/ /** 2*/ /** 0.5f*/;
                                    else
                                        tempFloat4 = 0;
                                }
                                else
                                {
                                    tempFloat4 = 0;
                                }

                                if (curUpBlockDistance < gridBreadth + 0.025f - tempFloat4)
                                {
                                    //DebugLog("enter");

                                    if (curBlockTileDatas[i].temperature != 0 ||
                                        curBlockTileDatas[i].electricity != 0 ||
                                        curBlockTileDatas[i].toxicity != 0)
                                    {
                                        if (VARS.IsAttachCeiling)
                                        {
                                            CurTileTransferAffliction(curBlockTileDatas[i]);
                                        }
                                        else
                                        {
                                            CurTileTransferAffliction(curBlockTileDatas[i], 0.5f);
                                        }

                                        VARS.IsTouchingAfflictingBlocks = true;
                                    }

                                    //UnityEngine.Debug.Log("ceilingDetected");

                                    VARS.curUpTile = curBlocks[i];
                                    VARS.curUpTileData = curBlockTileDatas[i];
                                    VARS.IsCeilingDetected = true;

                                    hasGotCurNearestUpBlock = true;
                                }
                            }
                            if (curBlockTileDatas[i].isFragile &&
                                VARS.IsAttachCeiling &&
                                tempFloat1 < 0.9f)
                            {
                                BreakCurTile(curBlocks[i], curToBeBrokenFragileRustBlocks, curFragileRustBlockToBeBrokenStartTimes);
                            }
                        }
                    }
                }
                //down
                if ((!hasGotCurNearestDownBlock &&
                    !(curBlockTileDatas[i].isPlatform && VARS.IsInputtingDownKey))||
                    curBlockTileDatas[i].isFragile)
                {
                    tempFloat1 = Mathf.Abs(Vector3.Dot(tempVector, VARS.curRight));

                    if (tempFloat1 < gridBreadth - 0.025f)
                    {
                        tempFloat = Vector3.Dot(tempVector, VARS.curUp);

                        if (tempFloat > 0.5f &&
                            tempFloat < gridBreadth + 0.1f /*0.01f*/ &&
                            tempFloat > tempFloat1)
                        {
                            if (tempFloat < curDownBlockDistance)
                            {
                                curDownBlockDistance = tempFloat;

                                curDownBlockHorDistance = tempFloat1;

                                if (/*VARS.IsLeftBlocked || VARS.IsRightBlocked*/
                                    //VARS.IsOnGround &&
                                    !VARS.IsLeftBlocked && !VARS.IsRightBlocked &&
                                    curDownBlockHorDistance > /*0.9f*/ 0.8f &&
                                    Mathf.Abs(VARS.horCurSpeed) < 1)
                                {
                                    if (/*VARS.IsHighJumping ||
                                        VARS.IsInputtingJumpKey ||
                                        VARS.IsInputtingUpKey ||*/
                                        VARS.IsInputtingDownKey
                                        /*VARS.IsDownKeyDown*/)
                                        tempFloat4 = (0.05f - (Mathf.Abs(VARS.horCurSpeed) / horMaxSpeed) / 100) /** 16*/ * 12 /** 8*/ /** 4*/ /** 2*/ /** 0.5f*/;
                                    else
                                        tempFloat4 = 0;
                                }
                                else
                                {
                                    tempFloat4 = 0;
                                }

                                if (curDownBlockDistance < gridBreadth + 0.025f - tempFloat4)
                                {
                                    if (curBlockTileDatas[i].temperature != 0 ||
                                        curBlockTileDatas[i].electricity != 0 ||
                                        curBlockTileDatas[i].toxicity != 0)
                                    {
                                        if (!VARS.IsAttachCeiling)
                                        {
                                            CurTileTransferAffliction(curBlockTileDatas[i]);
                                        }
                                        else
                                        {
                                            CurTileTransferAffliction(curBlockTileDatas[i], 0.5f);
                                        }

                                        VARS.IsTouchingAfflictingBlocks = true;
                                    }
                                    
                                    if (curBlockTileDatas[i].isPlatform)
                                    {
                                        VARS.IsHighJumping = false;
                                    }

                                    VARS.curDownTile = curBlocks[i];
                                    VARS.curDownTileData = curBlockTileDatas[i];
                                    VARS.IsGroundDetected = true;

                                    hasGotCurNearestDownBlock = true;
                                }
                            }
                            if (curBlockTileDatas[i].isFragile &&
                                tempFloat1 < 0.9f)
                            {
                                BreakCurTile(curBlocks[i], curToBeBrokenFragileRustBlocks, curFragileRustBlockToBeBrokenStartTimes);
                            }
                        }
                    }
                }
                //left
                if ((!hasGotCurNearestLeftBlock &&
                    !curBlockTileDatas[i].isPlatform) ||
                    curBlockTileDatas[i].isFragile)
                {
                    tempFloat1 = Mathf.Abs(Vector3.Dot(tempVector, VARS.curUp));

                    if (tempFloat1 < gridBreadth - 0.025f)
                    {
                        tempFloat = Vector3.Dot(tempVector, VARS.curRight);

                        if (tempFloat > 0.5f &&
                            tempFloat < gridBreadth + 0.1f /*0.01f*/ &&
                            tempFloat > tempFloat1)
                        {
                            if (tempFloat < curLeftBlockDistance)
                            {
                                curLeftBlockDistance = tempFloat;

                                curLeftBlockVerDistance = tempFloat1;

                                if (/*VARS.IsOnGround || VARS.IsToCeiling*/
                                    //VARS.IsLeftBlocked &&
                                    !VARS.IsOnGround && !VARS.IsToCeiling &&
                                    curLeftBlockVerDistance > /*0.9f*/ 0.8f &&
                                    !VARS.IsAttachWall &&
                                    !VARS.IsClimbing &&
                                    Mathf.Abs(VARS.verCurSpeed) < 6)
                                {
                                    if (VARS.IsInputtingLeftKey /*|| VARS.IsInputtingRightKey*/ &&
                                        VARS.IsInputtingAcceKey)
                                        tempFloat4 = (0.05f - (Mathf.Abs(VARS.verCurSpeed) / verMaxSpeed) / 100) /** 8*/ /** 4*/ /** 2*/ * 0.5f /** 0.25f*/;
                                    else
                                        tempFloat4 = 0;
                                }
                                else
                                {
                                    tempFloat4 = 0;
                                }

                                if (curLeftBlockDistance < gridBreadth + 0.025f - tempFloat4)
                                {
                                    if (curBlockTileDatas[i].temperature != 0 ||
                                        curBlockTileDatas[i].electricity != 0 ||
                                        curBlockTileDatas[i].toxicity != 0)
                                    {
                                        if (VARS.IsAttachWall && VARS.curFacingDirectionIndex == 1)
                                        {
                                            CurTileTransferAffliction(curBlockTileDatas[i]);
                                        }
                                        else
                                        {
                                            CurTileTransferAffliction(curBlockTileDatas[i], 0.5f);
                                        }

                                        VARS.IsTouchingAfflictingBlocks = true;
                                    }

                                    VARS.curLeftTile = curBlocks[i];
                                    VARS.curLeftTileData = curBlockTileDatas[i];
                                    VARS.IsLeftBlockDetected = true;

                                    hasGotCurNearestLeftBlock = true;
                                }
                            }
                            if (curBlockTileDatas[i].isFragile &&
                                (VARS.IsAttachWall || VARS.IsInAcce) &&
                                VARS.curFacingDirectionIndex == 1 &&
                                tempFloat1 < 0.9f)
                            {
                                BreakCurTile(curBlocks[i], curToBeBrokenFragileRustBlocks, curFragileRustBlockToBeBrokenStartTimes);
                            }
                        }
                    }
                }
                //right
                if ((!hasGotCurNearestRightBlock
                    && !curBlockTileDatas[i].isPlatform) ||
                    curBlockTileDatas[i].isFragile)
                {
                    tempFloat1 = Mathf.Abs(Vector3.Dot(tempVector, VARS.curUp));

                    if (tempFloat1 < gridBreadth - 0.025f)
                    {
                        tempFloat = Vector3.Dot(tempVector, -VARS.curRight);

                        if (tempFloat > 0.5f &&
                            tempFloat < gridBreadth + 0.1f /*0.01f*/ &&
                            tempFloat > tempFloat1)
                        {
                            if (tempFloat < curRightBlockDistance)
                            {
                                curRightBlockDistance = tempFloat;

                                curRightBlockVerDistance = tempFloat1;

                                if (/*VARS.IsOnGround || VARS.IsToCeiling*/
                                    //VARS.IsRightBlocked &&
                                    !VARS.IsOnGround && !VARS.IsToCeiling &&
                                    curRightBlockVerDistance > /*0.9f*/ 0.8f &&
                                    !VARS.IsAttachWall &&
                                    !VARS.IsClimbing &&
                                    Mathf.Abs(VARS.verCurSpeed) < 6)
                                {
                                    if (/*VARS.IsInputtingLeftKey ||*/ VARS.IsInputtingRightKey &&
                                        VARS.IsInputtingAcceKey)
                                        tempFloat4 = (0.05f - (Mathf.Abs(VARS.verCurSpeed) / verMaxSpeed) / 100) /** 8*/ /** 4*/ /** 2*/ * 0.5f /** 0.25f*/;
                                else
                                        tempFloat4 = 0;
                                }
                                else
                                {
                                    tempFloat4 = 0;
                                }

                                if (curRightBlockDistance < gridBreadth + 0.025f - tempFloat4)
                                {
                                    if (curBlockTileDatas[i].temperature != 0 ||
                                        curBlockTileDatas[i].electricity != 0 ||
                                        curBlockTileDatas[i].toxicity != 0)
                                    {
                                        if (VARS.IsAttachWall && VARS.curFacingDirectionIndex == 2)
                                        {
                                            CurTileTransferAffliction(curBlockTileDatas[i]);
                                        }
                                        else
                                        {
                                            CurTileTransferAffliction(curBlockTileDatas[i], 0.5f);
                                        }

                                        VARS.IsTouchingAfflictingBlocks = true;
                                    }

                                    VARS.curRightTile = curBlocks[i];
                                    VARS.curRightTileData = curBlockTileDatas[i];
                                    VARS.IsRightBlockDetected = true;

                                    hasGotCurNearestRightBlock = true;
                                }
                            }
                            if (curBlockTileDatas[i].isFragile &&
                                (VARS.IsAttachWall || VARS.IsInAcce) &&
                                VARS.curFacingDirectionIndex == 2 &&
                                tempFloat1 < 0.9f)
                            {
                                BreakCurTile(curBlocks[i], curToBeBrokenFragileRustBlocks, curFragileRustBlockToBeBrokenStartTimes);
                            }
                        }
                    }
                }
            }

            //liquid
            if (curBlockTileDatas[i].stateOfMatterIndex == 2)
            {
                //if (!isFullyDrown)
                //{
                //    tempFloat1 = Mathf.Abs(Vector3.Dot(tempVector, VARS.curRight));
                //    if (tempFloat1 < gridBreadth - 0.025f)
                //    {
                //        tempFloat = Vector3.Dot(tempVector, -VARS.curUp);

                //        if (tempFloat > 0 &&
                //            tempFloat < gridBreadth + 0.025f &&
                //            tempFloat > tempFloat1)
                //        {
                //            VARS.buoyancyDistanceFixFloat = 0;

                //            isFullyDrown = true;
                //        }
                //    }

                //}

                if (!hasGotCurNearestLiquidBlock)
                {
                    tempFloat = Vector3.Magnitude(tempVector);
                    //ifIsFullyDrown
                    if (Vector3.Dot(tempVector, VARS.curUp) < 0 &&
                        Mathf.Abs(Vector3.Dot(tempVector, VARS.curUp)) < gridBreadth - 0.025f &&
                        Mathf.Abs(Vector3.Dot(tempVector, VARS.curRight)) < gridBreadth - 0.025f)
                    {
                        //UnityEngine.Debug.Log("isFullyDrown");

                        isFullyDrown = true;
                    }
                    if (tempFloat < curNearestLiquidBlockDistance &&
                        Mathf.Abs(Vector3.Dot(tempVector, VARS.curUp)) < gridBreadth - 0.025f &&
                        Mathf.Abs(Vector3.Dot(tempVector, VARS.curRight)) < gridBreadth - 0.025f)
                    {
                        if (curBlockTileDatas[i].temperature != 0 ||
                            curBlockTileDatas[i].electricity != 0 ||
                            curBlockTileDatas[i].toxicity != 0)
                        {
                            CurTileTransferAffliction(curBlockTileDatas[i]);

                            VARS.IsTouchingAfflictingBlocks = true;
                        }

                        if (isFullyDrown)
                        {
                            VARS.buoyancyDistanceFixFloat = 0;
                        }
                        else
                        {
                            VARS.buoyancyDistanceFixFloat = Vector3.Dot(tempVector, VARS.curUp);
                        }

                        curNearestLiquidBlockDistance = tempFloat;

                        VARS.curLiquidTileData = curBlockTileDatas[i];
                        VARS.IsLiquidDetected = true;

                        //if (!isFullyDrown)
                        //{
                        //    tempFloat = Vector3.Dot(tempVector, VARS.curUp);
                        //    if (tempFloat < 0)
                        //    {
                        //        VARS.buoyancyDistanceFixFloat = 0;
                        //    }
                        //    else
                        //    {
                        //        VARS.buoyancyDistanceFixFloat = 1 - tempFloat;
                        //    }
                        //}

                        //hasGotCurLiquidBlock = true;
                    }
                }
            }

            //gas
            if (curBlockTileDatas[i].stateOfMatterIndex == 3)
            {
                if (!hasGotCurGasBlock)
                {
                    if (Mathf.Abs(Vector3.Dot(tempVector, VARS.curUp)) < gridBreadth - 0.025f &&
                        Mathf.Abs(Vector3.Dot(tempVector, VARS.curRight)) < gridBreadth - 0.025f)
                    {
                        if (curBlockTileDatas[i].temperature != 0 ||
                            curBlockTileDatas[i].electricity != 0 ||
                            curBlockTileDatas[i].toxicity != 0)
                        {
                            CurTileTransferAffliction(curBlockTileDatas[i]);

                            VARS.IsTouchingAfflictingBlocks = true;
                        }

                        VARS.curGasTileData = curBlockTileDatas[i];
                        VARS.IsGasDetected = true;

                        hasGotCurGasBlock = true;
                    }
                }
            }

            //mist
            if (curBlockTileDatas[i].stateOfMatterIndex == 4)
            {
                if (!hasGotCurMistBlock)
                {
                    if (Mathf.Abs(Vector3.Dot(tempVector, VARS.curUp)) < gridBreadth - 0.025f &&
                        Mathf.Abs(Vector3.Dot(tempVector, VARS.curRight)) < gridBreadth - 0.025f)
                    {
                        if (curBlockTileDatas[i].temperature != 0 ||
                            curBlockTileDatas[i].electricity != 0 ||
                            curBlockTileDatas[i].toxicity != 0)
                        {
                            CurTileTransferAffliction(curBlockTileDatas[i]);

                            VARS.IsTouchingAfflictingBlocks = true;
                        }

                        VARS.curMistTileData = curBlockTileDatas[i];
                        VARS.IsMistDetected = true;

                        hasGotCurMistBlock = true;
                    }
                }
            }
        }

        if (VARS.IsCeilingDetected)
        {
            VARS.IsCeilingDetected = false;
            VARS.IsToCeiling = true;
        }
        else
        {
            VARS.IsToCeiling = false;
        }
        if (VARS.IsGroundDetected)
        {
            VARS.IsGroundDetected = false;
            VARS.IsOnGround = true;
        }
        else
        {
            VARS.IsOnGround = false;
        }
        if (VARS.IsLeftBlockDetected)
        {
            VARS.IsLeftBlockDetected = false;
            VARS.IsLeftBlocked = true;
        }
        else
        {
            VARS.IsLeftBlocked = false;
        }
        if (VARS.IsRightBlockDetected)
        {
            VARS.IsRightBlockDetected = false;
            VARS.IsRightBlocked = true;
        }
        else
        {
            VARS.IsRightBlocked = false;
        }
        if (VARS.IsLiquidDetected)
        {
            VARS.IsLiquidDetected = false;
            VARS.IsInLiquid = true;
        }
        else
        {
            VARS.IsInLiquid = false;
        }
        if (VARS.IsGasDetected)
        {
            VARS.IsGasDetected = false;
            VARS.IsInGas = true;
        }
        else
        {
            VARS.IsInGas = false;
        }
        if (VARS.IsMistDetected)
        {
            VARS.IsMistDetected = false;
            VARS.IsInMist = true;
        }
        else
        {
            VARS.IsInMist = false;
        }

        ////onOrToCurTile
        //if (VARS.curUpTile != null &&
        //    VARS.curUpTile == VARS.curAttachedCeilingTile)
        //{
        //    //fragile
        //    if (VARS.curUpTileData.isFragile)
        //    {
        //        BreakCurTile(VARS.curUpTile,curToBeBrokenFragileRustBlocks, curFragileRustBlockToBeBrokenStartTimes);
        //    }

        //    //railBlock
        //    if (VARS.curUpTileData.railBlockIndex > 0)
        //    {
        //        VARS.curOnOrToRailBlock = VARS.curUpTile;
        //        VARS.IsOnOrToARailBlock = true;
        //    }
        //}
        //if (VARS.curDownTile != null)
        //{
        //    //fragile
        //    if (VARS.curDownTileData.isFragile)
        //    {
        //        BreakCurTile(VARS.curDownTile, curToBeBrokenFragileRustBlocks, curFragileRustBlockToBeBrokenStartTimes);
        //    }

        //    //railBlock
        //    if (VARS.curDownTileData.railBlockIndex > 0)
        //    {
        //        VARS.curOnOrToRailBlock = VARS.curDownTile;
        //        VARS.IsOnOrToARailBlock = true;
        //    }
        //}
        //if (VARS.curLeftTile!=null &&
        //    VARS.curLeftTile==VARS.curAttachedWallTile)
        //{
        //    //fragile
        //    if (VARS.curLeftTileData.isFragile)
        //    {
        //        BreakCurTile(VARS.curLeftTile, curToBeBrokenFragileRustBlocks, curFragileRustBlockToBeBrokenStartTimes);
        //    }

        //    //railBlock
        //    if (VARS.curLeftTileData.railBlockIndex > 0)
        //    {
        //        VARS.curOnOrToRailBlock = VARS.curLeftTile;
        //        VARS.IsOnOrToARailBlock = true;
        //    }
        //}
        //if (VARS.curRightTile != null &&
        //    VARS.curRightTile == VARS.curAttachedWallTile)
        //{
        //    //fragile
        //    if (VARS.curRightTileData.isFragile)
        //    {
        //        BreakCurTile(VARS.curRightTile, curToBeBrokenFragileRustBlocks, curFragileRustBlockToBeBrokenStartTimes);
        //    }

        //    //railBlock
        //    if (VARS.curRightTileData.railBlockIndex > 0)
        //    {
        //        VARS.curOnOrToRailBlock = VARS.curRightTile;
        //        VARS.IsOnOrToARailBlock = true;
        //    }
        //}

        VARS.curUpBlockDistance = curUpBlockDistance;
        VARS.curDownBlockDistance = curDownBlockDistance;
        VARS.curLeftBlockDistance = curLeftBlockDistance;
        VARS.curRightBlockDistance = curRightBlockDistance;
    }

    public void CurTileTransferAffliction(TileData curTileData, float speedFix = 1)
    {
        if (curTileData != null)
        {
            //temperature
            if (!(EqualToZero(curTileData.temperature) &&
                EqualToZero(VARS.catCurTemperature)))
            {
                //UFL.AddCatCurTemperature((curTileData.temperature - VARS.catCurTemperature) * temperatureTransferSpeed * Time.deltaTime);
                VARS.catCurTemperature += (curTileData.temperature - VARS.catCurTemperature) * temperatureTransferSpeed * speedFix * Time.deltaTime;
            }
            //electricity
            if (!(EqualToZero(curTileData.electricity) &&
                EqualToZero(VARS.catCurElectricity)))
            {
                //UFL.AddCatCurElectricity((curTileData.electricity - VARS.catCurElectricity) * electricityTransferSpeed * Time.deltaTime);
                VARS.catCurElectricity += (curTileData.electricity - VARS.catCurElectricity) * electricityTransferSpeed * speedFix * Time.deltaTime;
            }
            //toxicity
            if (!(EqualToZero(curTileData.toxicity) &&
                EqualToZero(VARS.catCurToxicity)))
            {
                //UFL.AddCatCurToxicity((curTileData.toxicity - VARS.catCurToxicity) * toxicityTransferSpeed * Time.deltaTime);
                VARS.catCurToxicity += (curTileData.toxicity - VARS.catCurToxicity) * toxicityTransferSpeed * speedFix * Time.deltaTime;
            }
        }
    }

    public void DissipateAffliction()
    {
        if (!VARS.IsTouchingAfflictingBlocks)
        {
            //temperature
            if (VARS.catCurTemperature != 0)
            {
                //UFL.AddCatCurTemperature(-VARS.catCurTemperature * temperatureTransferSpeed * Time.deltaTime);
                VARS.catCurTemperature += -VARS.catCurTemperature * temperatureTransferSpeed * Time.deltaTime;

            }

            //electricity
            if (VARS.catCurElectricity != 0)
            {
                //UFL.AddCatCurElectricity(-VARS.catCurElectricity * electricityTransferSpeed * Time.deltaTime);
                VARS.catCurElectricity += -VARS.catCurElectricity * electricityTransferSpeed * Time.deltaTime;
            }

            //toxicity
            if (VARS.catCurToxicity != 0)
            {
                //UFL.AddCatCurToxicity(-VARS.catCurToxicity * toxicityTransferSpeed * Time.deltaTime);
                VARS.catCurToxicity += -VARS.catCurToxicity * toxicityTransferSpeed * Time.deltaTime;
            }
        }
    }

    public void TransferAffliction()
    {
        TileData curTileData;

        if (!VARS.IsInLiquid &&
            !VARS.IsInGas &&
            !VARS.IsInMist)
        {
            if (VARS.IsToCeiling ||
                VARS.IsOnGround ||
                VARS.IsLeftBlocked ||
                VARS.IsRightBlocked)
            {
                for (int i = 0; i < 4; i++)
                {
                    switch (i)
                    {
                        case 0: curTileData = VARS.curUpTileData; break;
                        case 1: curTileData = VARS.curDownTileData; break;
                        case 2: curTileData = VARS.curLeftTileData; break;
                        case 3: curTileData = VARS.curRightTileData; break;
                        default: curTileData = null; break;
                    }

                    if (curTileData != null)
                    {
                        //temperature
                        if (!(EqualToZero(curTileData.temperature) &&
                            EqualToZero(VARS.catCurTemperature)))
                        {
                            //UFL.AddCatCurTemperature((curTileData.temperature - VARS.catCurTemperature) * temperatureTransferSpeed * Time.deltaTime);
                            VARS.catCurTemperature += (curTileData.temperature - VARS.catCurTemperature) * temperatureTransferSpeed * Time.deltaTime;
                        }
                        //electricity
                        if (!(EqualToZero(curTileData.electricity) &&
                            EqualToZero(VARS.catCurElectricity)))
                        {
                            //UFL.AddCatCurElectricity((curTileData.electricity - VARS.catCurElectricity) * electricityTransferSpeed * Time.deltaTime);
                            VARS.catCurElectricity += (curTileData.electricity - VARS.catCurElectricity) * electricityTransferSpeed * Time.deltaTime;
                        }
                        //toxicity
                        if (!(EqualToZero(curTileData.toxicity) &&
                            EqualToZero(VARS.catCurToxicity)))
                        {
                            //UFL.AddCatCurToxicity((curTileData.toxicity - VARS.catCurToxicity) * toxicityTransferSpeed * Time.deltaTime);
                            VARS.catCurToxicity += (curTileData.toxicity - VARS.catCurToxicity) * toxicityTransferSpeed * Time.deltaTime;
                        }
                    }
                }
            }
            else
            {
                //temperature
                if (VARS.catCurTemperature != 0)
                {
                    //UFL.AddCatCurTemperature(-VARS.catCurTemperature * temperatureTransferSpeed * Time.deltaTime);
                    VARS.catCurTemperature += -VARS.catCurTemperature * temperatureTransferSpeed * Time.deltaTime;

                }

                //electricity
                if (VARS.catCurElectricity != 0)
                {
                    //UFL.AddCatCurElectricity(-VARS.catCurElectricity * electricityTransferSpeed * Time.deltaTime);
                    VARS.catCurElectricity += -VARS.catCurElectricity * electricityTransferSpeed * Time.deltaTime;
                }

                //toxicity
                if (VARS.catCurToxicity != 0)
                {
                    //UFL.AddCatCurToxicity(-VARS.catCurToxicity * toxicityTransferSpeed * Time.deltaTime);
                    VARS.catCurToxicity += -VARS.catCurToxicity * toxicityTransferSpeed * Time.deltaTime;
                }
            }
        }
        else
        {
            for (int i = 0; i < 3; i++)
            {
                switch (i)
                {
                    case 0: curTileData = VARS.curLiquidTileData; break;
                    case 1: curTileData = VARS.curGasTileData; break;
                    case 2: curTileData = VARS.curMistTileData; break;
                    default: curTileData = null; break;
                }

                if (curTileData != null)
                {
                    //temperature
                    if (!(EqualToZero(curTileData.temperature) &&
                        EqualToZero(VARS.catCurTemperature)))
                    {
                        //UFL.AddCatCurTemperature((curTileData.temperature - VARS.catCurTemperature) * temperatureTransferSpeed * Time.deltaTime);
                        VARS.catCurTemperature += (curTileData.temperature - VARS.catCurTemperature) * temperatureTransferSpeed * Time.deltaTime;
                    }
                    //electricity
                    if (!(EqualToZero(curTileData.electricity) &&
                        EqualToZero(VARS.catCurElectricity)))
                    {
                        //UFL.AddCatCurElectricity((curTileData.electricity - VARS.catCurElectricity) * electricityTransferSpeed * Time.deltaTime);
                        VARS.catCurElectricity += (curTileData.electricity - VARS.catCurElectricity) * electricityTransferSpeed * Time.deltaTime;
                    }
                    //toxicity
                    if (!(EqualToZero(curTileData.toxicity) &&
                        EqualToZero(VARS.catCurToxicity)))
                    {
                        //UFL.AddCatCurToxicity((curTileData.toxicity - VARS.catCurToxicity) * toxicityTransferSpeed * Time.deltaTime);
                        VARS.catCurToxicity += (curTileData.toxicity - VARS.catCurToxicity) * toxicityTransferSpeed * Time.deltaTime;
                    }
                }
            }
        }
    }

    public void CollidTriggers()
    {
        //DebugLog("collidTriggers");

        if (VARS.curTriggerTile != null)
        {
            //gate
            if (/*curTileData.triggerTypeIndex == 3*/
                VARS.curTriggerTileData.blockTypeIndex == 7001)
            {
                VARS.lastJustInGateTime = Time.time;

                VARS.IsJustByGate = true;
            }

            //edgeGate(enter)
            else if (/*VARS.curTriggerTileData.triggerTypeIndex == 4*/
                VARS.curTriggerTileData.blockTypeIndex == 7002)
            {
                VARS.curEdgeGate = VARS.curTriggerTile;

                VARS.IsEnteringAnEdgeGate = true;

                VARS.lastJustInGateTime = Time.time;

                VARS.IsJustByGate = true;
            }

            ////edgeGateTrigger(triggerEdgeGate)
            //else if (/*VARS.curTriggerTileData.triggerTypeIndex == 5*/
            //    VARS.curTriggerTileData.blockTypeIndex == 7003 &&
            //    UFL.IsCatInEdgeGateTrigger())
            //{
            //    VARS.IsEdgeGateTriggered = true;
            //}

            //activateSavePoint(notActiavted)
            //savePoint
            else if (/*VARS.curTriggerTileData.triggerTypeIndex == 6*/
                VARS.curTriggerTileData.blockTypeIndex == 7004)
            {
                //DebugLog("savePoint");

                //if (IsCatInSavePointBlock())
                //{
                //    VARS.IsToActivateASavePoint = true;
                //}

                if (Time.time - VARS.lastActivatedSavePointTime > activateSavePointGapTime)
                {
                    UnityEngine.Debug.Log("touchedSavePoint");

                    VARS.lastActivatedSavePointTime = Time.time;

                    VARS.IsToActivateASavePoint = true;
                }
            }

            //activatedSavePoint(~~?)
            //activatedSavePoint
            else if (/*VARS.curTriggerTileData.triggerTypeIndex == 7*/
                VARS.curTriggerTileData.blockTypeIndex == 7005)
            {

            }

            //center(in)
            else if (/*VARS.curTriggerTileData.triggerTypeIndex == 8*/
                VARS.curTriggerTileData.blockTypeIndex == 7006)
            {
                VARS.IsInCenter = true;
            }

            //key
            else if (VARS.curTriggerTileData.blockTypeIndex == 7007)
            {
                //DebugLog("key");

                if (!VARS.IsToCarryAKey &&
                    !VARS.IsCarryingAKey &&
                    !VARS.IsUnlocking)
                {
                    VARS.curKey = VARS.curTriggerTile;

                    VARS.IsToCarryAKey = true;
                }
            }

            //strawberry(get)
            else if (/*VARS.curTriggerTileData.triggerTypeIndex == 1*/
                VARS.curTriggerTileData.blockTypeIndex == 7008)
            {
                //isCarryingStrawberries = true;

                //carriedStrawberries.Add(curTile);
                //carriedStrawberriesIniPositions.Add(curTile.transform.position);

                VARS.IsGettingAStrawberry = true;
            }

            //energyCrystal(get)
            else if (/*VARS.curTriggerTileData.triggerTypeIndex == 2*/
                VARS.curTriggerTileData.blockTypeIndex == 7009)
            {
                //if (VARS.curTriggerTile.transform.localScale != Vector3.one * 0.2f)
                //if (Vector3.Distance(VARS.curTriggerTile.transform.localScale, Vector3.one * 0.2f) < 0.1f)
                //{
                //    VARS.IsGettingAnEnergyCrystal = true;
                //}
                if (VARS.curTriggerTile.activeSelf == true)//~?
                {
                    VARS.IsGettingAnEnergyCrystal = true;
                }
            }

            //void
            else if (VARS.curTriggerTileData.blockTypeIndex == 7010)
            {
                //if (IsCatInVoidBlock())
                //{
                //    VARS.IsToDie = true;
                //}

                //DebugLog("intoVoid");

                //VARS.IsToDie = true;

                if (Time.time - VARS.lastTouchedVoidTime > intoVoidWarmupTime)
                {
                    if (Time.time - VARS.lastIntoVoidTime > intoVoidGapTime)
                    {
                        tempVector = VARS.curTriggerTile.transform.position - catTransform.position;
                        tempFloat1 = Mathf.Abs(Vector3.Dot(tempVector, VARS.curUp));
                        tempFloat2 = Mathf.Abs(Vector3.Dot(tempVector, VARS.curRight));

                        if (tempFloat1 < 0.9f && tempFloat2 < 0.9f)
                        {
                            VARS.curTargetEnergy -= intoVoidEnergyLost;

                            VARS.IsJustIntoVoid = true;

                            VARS.lastIntoVoidTime = Time.time;
                        }
                    }

                    VARS.lastTouchedVoidTime = 0;
                }

                if (VARS.lastTouchedVoidTime == 0)
                {
                    VARS.lastTouchedVoidTime = Time.time;
                }
            }

            //redFragment
            else if (VARS.curTriggerTileData.blockTypeIndex == 1001)
            {
                if (!VARS.IsToCarryAFragment &&
                    !VARS.IsEmbeddingFragments &&
                    !isRedFragmentsEmbeded[VARS.curTriggerTileData.fragmentIndex - 1] &&
                    VARS.curTriggerTile.transform.GetChild(0).gameObject.activeSelf &&
                    !VARS.curCarriedFragments.Contains(VARS.curTriggerTile))
                {
                    VARS.curToBeCarriedFragment = VARS.curTriggerTile;
                    VARS.curToBeCarriedFragmentFaceIndex = 6;
                    VARS.curToBeCarriedFragmentIndex = VARS.curTriggerTileData.fragmentIndex;
                    VARS.IsToCarryAFragment = true;
                }
            }

            //yellowFragment
            else if (VARS.curTriggerTileData.blockTypeIndex == 2001)
            {
                if (!VARS.IsToCarryAFragment &&
                    !VARS.IsEmbeddingFragments &&
                    !isYellowFragmentsEmbeded[VARS.curTriggerTileData.fragmentIndex - 1] &&
                    VARS.curTriggerTile.transform.GetChild(0).gameObject.activeSelf &&
                    !VARS.curCarriedFragments.Contains(VARS.curTriggerTile))
                {
                    VARS.curToBeCarriedFragment = VARS.curTriggerTile;
                    VARS.curToBeCarriedFragmentFaceIndex = 1;
                    VARS.curToBeCarriedFragmentIndex = VARS.curTriggerTileData.fragmentIndex;
                    VARS.IsToCarryAFragment = true;
                }
            }

            //blueFragment
            else if (VARS.curTriggerTileData.blockTypeIndex == 3001)
            {
                if (!VARS.IsToCarryAFragment &&
                    !VARS.IsEmbeddingFragments &&
                    !isBlueFragmentsEmbeded[VARS.curTriggerTileData.fragmentIndex - 1] &&
                    VARS.curTriggerTile.transform.GetChild(0).gameObject.activeSelf &&
                    !VARS.curCarriedFragments.Contains(VARS.curTriggerTile))
                {
                    VARS.curToBeCarriedFragment = VARS.curTriggerTile;
                    VARS.curToBeCarriedFragmentFaceIndex = 4;
                    VARS.curToBeCarriedFragmentIndex = VARS.curTriggerTileData.fragmentIndex;
                    VARS.IsToCarryAFragment = true;
                }
            }

            //orangeFragment
            else if (VARS.curTriggerTileData.blockTypeIndex == 4001)
            {
                if (!VARS.IsToCarryAFragment &&
                    !VARS.IsEmbeddingFragments &&
                    !isOrangeFragmentsEmbeded[VARS.curTriggerTileData.fragmentIndex - 1] &&
                    VARS.curTriggerTile.transform.GetChild(0).gameObject.activeSelf &&
                    !VARS.curCarriedFragments.Contains(VARS.curTriggerTile))
                {
                    VARS.curToBeCarriedFragment = VARS.curTriggerTile;
                    VARS.curToBeCarriedFragmentFaceIndex = 3;
                    VARS.curToBeCarriedFragmentIndex = VARS.curTriggerTileData.fragmentIndex;
                    VARS.IsToCarryAFragment = true;
                }
            }

            //greenFragment
            else if (VARS.curTriggerTileData.blockTypeIndex == 5001)
            {
                if (!VARS.IsToCarryAFragment &&
                    !VARS.IsEmbeddingFragments &&
                    !isGreenFragmentsEmbeded[VARS.curTriggerTileData.fragmentIndex - 1] &&
                    VARS.curTriggerTile.transform.GetChild(0).gameObject.activeSelf &&
                    !VARS.curCarriedFragments.Contains(VARS.curTriggerTile))
                {
                    VARS.curToBeCarriedFragment = VARS.curTriggerTile;
                    VARS.curToBeCarriedFragmentFaceIndex = 5;
                    VARS.curToBeCarriedFragmentIndex = VARS.curTriggerTileData.fragmentIndex;
                    VARS.IsToCarryAFragment = true;
                }
            }

            //purpleFragment
            else if (VARS.curTriggerTileData.blockTypeIndex == 6001)
            {
                if (!VARS.IsToCarryAFragment &&
                    !VARS.IsEmbeddingFragments &&
                    !isPurpleFragmentsEmbeded[VARS.curTriggerTileData.fragmentIndex - 1] &&
                    VARS.curTriggerTile.transform.GetChild(0).gameObject.activeSelf &&
                    !VARS.curCarriedFragments.Contains(VARS.curTriggerTile))
                {
                    VARS.curToBeCarriedFragment = VARS.curTriggerTile;
                    VARS.curToBeCarriedFragmentFaceIndex = 2;
                    VARS.curToBeCarriedFragmentIndex = VARS.curTriggerTileData.fragmentIndex;
                    VARS.IsToCarryAFragment = true;
                }
            }

            //center(out)
            if (/*VARS.curTriggerTileData.triggerTypeIndex != 8*/
                VARS.curTriggerTileData.blockTypeIndex != 7006)
            {
                VARS.IsInCenter = false;
            }
        }
        else
        {
            VARS.IsInCenter = false;
        }
    }

    void BreakCurTile(GameObject curTile, List<GameObject> intoGameObjectList, List<float> intoTimeList)
    {
        intoGameObjectList.Add(curTile);
        intoTimeList.Add(Time.time);
    }

    public void ClearCurCollisionTileDatas()
    {
        VARS.curUpTileData = null;
        VARS.curDownTileData = null;
        VARS.curLeftTileData = null;
        VARS.curRightTileData = null;
        VARS.curLiquidTileData = null;
        VARS.curGasTileData = null;
        VARS.curMistTileData = null;
    }
    #endregion

    #region CatMove
    //public void SetHorCurSpeed(float value)
    //{
    //    //Debug.Log("setHorCurSpeed: " + value);

    //    VARS.horCurSpeed = value;
    //}

    //public void AddHorCurSpeed(float value)
    //{
    //    //Debug.Log("addHorCurSpeed: " + value);

    //    VARS.horCurSpeed += value;
    //}

    //public void SetVerCurSpeed(float value)
    //{
    //    //Debug.Log("setVerCurSpeed: " + value);

    //    VARS.verCurSpeed = value;
    //}

    //public void AddVerCurSpeed(float value)
    //{
    //    //Debug.Log("addVerCurSpeed: " + value);

    //    VARS.verCurSpeed += value;
    //}

    public void SetCatPosition(Vector3 position)
    {
        catTransform.position = position;
    }

    public void AddCatPosition(Vector3 offset)
    {
        catTransform.position += offset;
    }
    #endregion

    #region CatState
    ////temperature
    //public void SetCatCurTemperature(float value)
    //{
    //    VARS.catCurTemperature = value;
    //}
    //public void AddCatCurTemperature(float value)
    //{
    //    VARS.catCurTemperature += value;
    //}

    ////electricity
    //public void SetCatCurElectricity(float value)
    //{
    //    VARS.catCurElectricity = value;
    //}
    //public void AddCatCurElectricity(float value)
    //{
    //    VARS.catCurElectricity += value;
    //}

    ////toxicity
    //public void SetCatCurToxicity(float value)
    //{
    //    VARS.catCurToxicity = value;
    //}
    //public void AddCatCurToxicity(float value)
    //{
    //    VARS.catCurToxicity += value;
    //}
    #endregion

    #region CatEnergy
    //public void SetCurTargetEnergy(float value)
    //{
    //    VARS.curTargetEnergy = value;
    //}

    //public void AddCurTargetEnergy(float value)
    //{
    //    VARS.curTargetEnergy += value;
    //}

    //public void SetCurEnergy(float value)
    //{
    //    VARS.curEnergy = value;
    //}

    //public void AddCurEnergy(float value)
    //{
    //    VARS.curEnergy += value;
    //}
    #endregion

    #region BlocksManager
    public bool IsCatInBlock(int curBlockIndex)
    {
        tempVector = catTransform.position - curBlocks[curBlockIndex].transform.position;

        if (Mathf.Abs(Vector3.Dot(tempVector, VARS.curUp)) < gridBreadth - 0.01f &&
            Mathf.Abs(Vector3.Dot(tempVector, VARS.curRight)) < gridBreadth - 0.01f)
        {
            return true;
        }

        return false;
    }

    public int CatInBlockIndex()
    {
        for (int i = 0; i < curBlocks.Count; i++)
        {
            if (IsCatInBlock(i))
            {
                return i;
            }
        }

        return -1;
    }

    public bool IsCatInVoidBlock()
    {
        tempInt = CatInBlockIndex();
        if (tempInt >= 0)
        {
            if (curBlocks[tempInt].GetComponent<TileData>().blockTypeIndex == 7010)
            {
                return true;
            }
        }

        return false;
    }

    public bool IsCatInSavePointBlock()
    {
        tempInt = CatInBlockIndex();
        if (tempInt >= 0)
        {
            if (curBlocks[tempInt].GetComponent<TileData>().blockTypeIndex == 7004)
            {
                return true;
            }
        }

        return false;
    }

    public bool IsCatInEdgeGateTrigger()
    {
        for (int i = 0; i < edgeGateTriggers.Count; i++)
        {
            tempVector = catTransform.position - edgeGateTriggers[i].transform.position;

            if (Mathf.Abs(Vector3.Dot(tempVector, VARS.curUp)) < gridBreadth - 0.1f &&
                Mathf.Abs(Vector3.Dot(tempVector, VARS.curRight)) < gridBreadth - 0.1f)
            {
                return true;
            }

        }

        return false;
    }

    public bool IsCatInEdgeGate()
    {
        for (int i = 0; i < edgeGates.Count; i++)
        {
            tempVector = catTransform.position - edgeGates[i].transform.position;

            if (Mathf.Abs(Vector3.Dot(tempVector, VARS.curUp)) < gridBreadth /*- 0.1f*/ &&
                Mathf.Abs(Vector3.Dot(tempVector, VARS.curRight)) < gridBreadth /*- 0.1f*/)
            {
                VARS.curEdgeGate = edgeGates[i];

                return true;
            }

        }

        return false;
    }
    #endregion
}
