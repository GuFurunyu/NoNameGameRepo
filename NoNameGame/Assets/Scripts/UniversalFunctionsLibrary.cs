using System.Collections.Generic;
using System.Diagnostics;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

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

    bool isUpLiquid;

    float curNearestLiquidBlockDistance;

    int tempInt;
    float tempFloat;
    float tempFloat1;
    float tempFloat2;
    float[] tempFloats = new float[3];
    Vector3 tempVector;
    Vector3 tempVector1;
    Vector3 tempVector2;
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

    float temperatureTransferSpeed;
    float electricityTransferSpeed;
    float toxicityTransferSpeed;

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
        temperatureTransferSpeed = CONS.temperatureTransferSpeed;
        electricityTransferSpeed = CONS.electricityTransferSpeed;
        toxicityTransferSpeed = CONS.toxicityTransferSpeed;
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
                VARS.curMinimapRoomPlaneColor = minimapRoomPlanes[i].GetComponent<MeshRenderer>().material.GetColor("_MainColor");
                minimapRoomPlanes[i].GetComponent<MeshRenderer>().material.SetColor("_MainColor", Color.white);

                if ((i - 4) % 9 == 0)
                {
                    tempGameObject = minimapCenterTriangleEmpties[(i - 4) / 9];
                    tempGameObject.transform.GetChild(0).gameObject.SetActive(false);
                    tempGameObject.transform.GetChild(1).gameObject.SetActive(true);
                }
            }
            else if ((i - 4) % 9 == 0)
            {
                tempGameObject = minimapCenterTriangleEmpties[(i - 4) / 9];
                tempGameObject.SetActive(IsRoomExplored(i));
            }

            //roomPlanes[i].SetActive(false);
        }

        cat.GetComponent<MeshRenderer>().enabled = false;

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
                minimapRoomPlanes[i].GetComponent<MeshRenderer>().material.SetColor("_MainColor", VARS.curMinimapRoomPlaneColor);

                if ((i - 4) % 9 == 0)
                {
                    tempGameObject = minimapCenterTriangleEmpties[(i - 4) / 9];
                    tempGameObject.transform.GetChild(0).gameObject.SetActive(true);
                    tempGameObject.transform.GetChild(1).gameObject.SetActive(false);
                }
            }

            //roomPlanes[i].SetActive(true);
        }

        cat.GetComponent<MeshRenderer>().enabled = true;

        //camPosition
        SetCameraPosition(VARS.curRoomCenter - VARS.curRoomStableForward * 7);

        //camEulerangles
        SetCameraEulerangles(VARS.camEuleranglesBeforeIntoMinimap);

        //camSize
        SetCameraSize(camNormalSize);
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

        //getCurToIndexAndCurToPoint
        switch (dirIndex)
        {
            case 1:
                //tempVector = -minimapRotationCameraPointStableUps[VARS.curMinimapRotationCameraPointIndex];
                tempVector = -camTransform.up;
                //tempQuaternion = camTransform.rotation * Quaternion.AngleAxis(camTransform.eulerAngles.z, camTransform.forward);
                //tempVector = tempQuaternion.eulerAngles;
                break;
            case 2:
                //tempVector = minimapRotationCameraPointStableUps[VARS.curMinimapRotationCameraPointIndex];
                tempVector = camTransform.up;
                break;
            case 3:
                //tempVector = minimapRotationCameraPointStableRights[VARS.curMinimapRotationCameraPointIndex];
                tempVector = camTransform.right;
                break;
            case 4:
                //tempVector = -minimapRotationCameraPointStableRights[VARS.curMinimapRotationCameraPointIndex];
                tempVector = -camTransform.right;
                break;
        }
        //Debug.Log("tempVector: " + tempVector);
        for (int i = 0; i < 26; i++)
        {
            //Debug.Log("enter1");

            //if (i == 4)
            //{
            //    Debug.Log(Vector3.Angle(minimapRotationCameraPoints[i].transform.position - VARS.curMinimapRotationCameraPoint.transform.position, tempVector));
            //    Debug.Log(Vector3.Dot(minimapRotationCameraPoints[i].transform.position - VARS.curMinimapRotationCameraPoint.transform.position, tempVector));
            //}

            //ifIsNearlyOnTheLine
            if (Vector3.Angle(minimapRotationCameraPoints[i].transform.position - VARS.curMinimapRotationCameraPoint.transform.position, tempVector) < 30)
            {
                //Debug.Log("enter2: " + i);

                //Debug.Log("dot: " + Vector3.Dot(minimapRotationCameraPoints[i].transform.position - VARS.curMinimapRotationCameraPoint.transform.position, tempVector));

                //ifIsTheRightDirection
                if (Vector3.Dot(minimapRotationCameraPoints[i].transform.position - VARS.curMinimapRotationCameraPoint.transform.position, tempVector) > 0)
                {
                    //Debug.Log("enter3");

                    //Debug.Log("curToIndex: " + i);

                    //Debug.Log(VARS.curToMinimapRotationCameraPoint.transform.position - VARS.curMinimapRotationCameraPoint.transform.position);

                    VARS.curToMinimapRotationCameraPointIndex = i;
                    VARS.curToMinimapRotationCameraPoint = minimapRotationCameraPoints[i];
                    break;
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

        isUpLiquid = false;

        for (int i = 0; i < curBlocks.Count; i++)
        {
            if (curBlocks[i].activeSelf == false)
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
                            tempFloat < gridBreadth + 0.1f &&
                            tempFloat > tempFloat1)
                        {
                            if (tempFloat < curUpBlockDistance)
                            {
                                curUpBlockDistance = tempFloat;

                                if (curUpBlockDistance < gridBreadth + 0.025f)
                                {
                                    //DebugLog("enter");

                                    VARS.curUpTile = curBlocks[i];
                                    VARS.curUpTileData = curBlockTileDatas[i];
                                    VARS.IsCeilingDetected = true;

                                    hasGotCurNearestUpBlock = true;
                                }
                            }
                            if (curBlockTileDatas[i].isFragile &&
                                VARS.IsAttachCeiling)
                            {
                                BreakCurTile(curBlocks[i], curToBeBrokenFragileRustBlocks, curFragileRustBlockToBeBrokenStartTimes);
                            }
                        }
                    }
                }
                //down
                if (!hasGotCurNearestDownBlock ||
                    curBlockTileDatas[i].isFragile)
                {
                    tempFloat1 = Mathf.Abs(Vector3.Dot(tempVector, VARS.curRight));

                    if (tempFloat1 < gridBreadth - 0.025f)
                    {
                        tempFloat = Vector3.Dot(tempVector, VARS.curUp);

                        if (tempFloat > 0.5f &&
                            tempFloat < gridBreadth + 0.1f &&
                            tempFloat > tempFloat1)
                        {
                            if (tempFloat < curDownBlockDistance)
                            {
                                curDownBlockDistance = tempFloat;

                                if (curDownBlockDistance < gridBreadth + 0.025f)
                                {
                                    VARS.curDownTile = curBlocks[i];
                                    VARS.curDownTileData = curBlockTileDatas[i];
                                    VARS.IsGroundDetected = true;

                                    hasGotCurNearestDownBlock = true;
                                }
                            }
                            if (curBlockTileDatas[i].isFragile)
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
                            tempFloat < gridBreadth + 0.1f &&
                            tempFloat > tempFloat1)
                        {
                            if (tempFloat < curLeftBlockDistance)
                            {
                                curLeftBlockDistance = tempFloat;

                                if (curLeftBlockDistance < gridBreadth + 0.025f)
                                {
                                    VARS.curLeftTile = curBlocks[i];
                                    VARS.curLeftTileData = curBlockTileDatas[i];
                                    VARS.IsLeftBlockDetected = true;

                                    hasGotCurNearestLeftBlock = true;
                                }
                            }
                            if (curBlockTileDatas[i].isFragile &&
                                VARS.IsAttachWall &&
                                VARS.curFacingDirectionIndex == 1)
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
                            tempFloat < gridBreadth + 0.1f &&
                            tempFloat > tempFloat1)
                        {
                            if (tempFloat < curRightBlockDistance)
                            {
                                curRightBlockDistance = tempFloat;

                                if (curRightBlockDistance < gridBreadth + 0.025f)
                                {
                                    VARS.curRightTile = curBlocks[i];
                                    VARS.curRightTileData = curBlockTileDatas[i];
                                    VARS.IsRightBlockDetected = true;

                                    hasGotCurNearestRightBlock = true;
                                }
                            }
                            if (curBlockTileDatas[i].isFragile &&
                                VARS.IsAttachWall &&
                                VARS.curFacingDirectionIndex == 2)
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
                if (!isUpLiquid)
                {
                    tempFloat1 = Mathf.Abs(Vector3.Dot(tempVector, VARS.curRight));
                    if (tempFloat1 < gridBreadth - 0.025f)
                    {
                        tempFloat = Vector3.Dot(tempVector, -VARS.curUp);

                        if (tempFloat > 0 &&
                            tempFloat < gridBreadth + 0.025f &&
                            tempFloat > tempFloat1)
                        {
                            VARS.buoyancyDistanceFixFloat = 0;

                            isUpLiquid = true;
                        }
                    }

                }

                if (!hasGotCurNearestLiquidBlock)
                {
                    tempFloat = Vector3.Magnitude(tempVector);
                    if (tempFloat < curNearestLiquidBlockDistance &&
                        Mathf.Abs(Vector3.Dot(tempVector, VARS.curUp)) < gridBreadth - 0.025f &&
                        Mathf.Abs(Vector3.Dot(tempVector, VARS.curRight)) < gridBreadth - 0.025f)
                    {
                        curNearestLiquidBlockDistance = tempFloat;

                        VARS.curLiquidTileData = curBlockTileDatas[i];
                        VARS.IsLiquidDetected = true;

                        if (!isUpLiquid)
                        {
                            tempFloat = Vector3.Dot(tempVector, VARS.curUp);
                            if (tempFloat < 0)
                            {
                                VARS.buoyancyDistanceFixFloat = 0;
                            }
                            else
                            {
                                VARS.buoyancyDistanceFixFloat = 1 - tempFloat;
                            }
                        }

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

            }

            //edgeGate(enter)
            else if (/*VARS.curTriggerTileData.triggerTypeIndex == 4*/
                VARS.curTriggerTileData.blockTypeIndex == 7002)
            {
                VARS.curEdgeGate = VARS.curTriggerTile;

                VARS.IsEnteringAnEdgeGate = true;
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

                if (!VARS.IsCarryingAKey &&
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
                if (VARS.curTriggerTile.transform.localScale != Vector3.one * 0.2f)
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

                //DebugLog("enterVoid");

                VARS.IsToDie = true;
            }

            //redFragment
            else if (VARS.curTriggerTileData.blockTypeIndex == 1001)
            {
                if (!VARS.IsToCarryAFragment &&
                    !VARS.IsEmbeddingFragments &&
                    !isRedFragmentsEmbeded[VARS.curTriggerTileData.fragmentIndex - 1] &&
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
