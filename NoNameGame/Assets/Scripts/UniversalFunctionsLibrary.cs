using System.Collections;
using System.Collections.Generic;
using System.IO;
using JetBrains.Annotations;
using UnityEditor.SceneTemplate;
using UnityEngine;

[DefaultExecutionOrder((int)ScriptsExecutionOrder.ExecutionOrder.universalFunctionsLibrary)]
public class UniversalFunctionsLibrary : MonoBehaviour
{
    Constants CONS;
    Variables VARS;

    GameObject gameManager;

    float tempFloat;
    float[] tempFloats = new float[3];
    Vector3 tempVector;
    Quaternion tempQuaternion;

    #region ConstantsUsed
    float gridBreadth;
    int roomCoordBreadth;
    int miniMapRoomCoordBreadth;

    float inRoomMaxForwardDistance;

    Vector3[] faceStableForwards = new Vector3[6];
    Vector3[] faceStableUps = new Vector3[6];
    Vector3[] faceStableRights = new Vector3[6];

    GameObject[] roomPlanes = new GameObject[54];

    GameObject[] twistingCenters = new GameObject[6];

    GameObject[] miniMapFaces = new GameObject[6];
    GameObject[] miniMapRoomPlanes = new GameObject[54];
    GameObject[] miniMapTwistingCenters = new GameObject[6];
    GameObject[] miniMapRotationCameraPoints = new GameObject[26];
    //Vector3[] miniMapRotationCameraPointStableUps = new Vector3[26];
    //Vector3[] miniMapRotationCameraPointStableRights = new Vector3[26];
    GameObject[] miniMapRotationCameraUpPoints = new GameObject[26];
    GameObject[] miniMapRotationCameraDownPoints = new GameObject[26];
    GameObject[] miniMapRotationCameraLeftPoints = new GameObject[26];
    GameObject[] miniMapRotationCameraRightPoints = new GameObject[26];

    Camera cam;
    Transform camTransform;

    float camNormalSize;
    float camMiniMapSize;

    float camMiniMapDistanceToCubeCore;

    GameObject cat;
    #endregion

    #region VariablesUsed
    Vector3[] roomCenters = new Vector3[54];
    Vector3[] roomStableForwards = new Vector3[54];
    Vector3[] roomStableUps = new Vector3[54];
    Vector3[] roomStableRights = new Vector3[54];
    #endregion

    private void Start()
    {
        gameManager = GameObject.Find("GameManager");

        CONS = gameManager.GetComponent<Constants>();
        VARS = gameManager.GetComponent<Variables>();

        gridBreadth = CONS.gridBreadth;
        roomCoordBreadth = CONS.roomCoordBreadth;
        miniMapRoomCoordBreadth = CONS.miniMapRoomCoordBreadth;
        inRoomMaxForwardDistance= CONS.inRoomMaxForwardDistance;
        faceStableForwards = CONS.faceStableForwards;
        faceStableUps = CONS.faceStableUps;
        faceStableRights = CONS.faceStableRights;
        roomPlanes = CONS.roomPlanes;
        twistingCenters = CONS.twistingCenters;
        miniMapFaces = CONS.miniMapFaces;
        miniMapRoomPlanes = CONS.miniMapRoomPlanes;
        miniMapTwistingCenters = CONS.miniMapTwistingCenters;
        miniMapRotationCameraPoints = CONS.miniMapRotationCameraPoints;
        //miniMapRotationCameraPointStableUps = CONS.miniMapRotationCameraPointStableUps;
        //miniMapRotationCameraPointStableRights = CONS.miniMapRotationCameraPointStableRights;
        miniMapRotationCameraUpPoints = CONS.miniMapRotationCameraUpPoints;
        miniMapRotationCameraDownPoints = CONS.miniMapRotationCameraDownPoints;
        miniMapRotationCameraLeftPoints = CONS.miniMapRotationCameraLeftPoints;
        miniMapRotationCameraRightPoints = CONS.miniMapRotationCameraRightPoints;
        cam = CONS.cam;
        camTransform = CONS.camTransform;
        camNormalSize = CONS.camNormalSize;
        camMiniMapSize = CONS.camMiniMapSize;
        camMiniMapDistanceToCubeCore = CONS.camMiniMapDistanceToCubeCore;
        cat = CONS.cat;

        roomCenters = VARS.roomCenters;
        roomStableForwards = VARS.roomStableForwards;
        roomStableUps = VARS.roomStableUps;
        roomStableRights = VARS.roomStableRights;
    }

    //private void Update()
    //{
    //}

    #region Universal
    public Vector3 Vector3Abs(Vector3 vector)
    {
        return new Vector3(Mathf.Abs(vector.x), Mathf.Abs(vector.y), Mathf.Abs(vector.z));
    }

    public Vector3 Vector3RoundToInt(Vector3 vector)
    {
        return new Vector3(Mathf.RoundToInt(vector.x), Mathf.RoundToInt(vector.y), Mathf.RoundToInt(vector.z));
    }
    #endregion

    //#region DataManager
    //#endregion

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

    public void SetMiniMapRoomPlanesByRoomPlanes()
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
                    tempFloats[j] *= ((miniMapRoomCoordBreadth + 1) * gridBreadth) / ((roomCoordBreadth + 1) * gridBreadth);
                }
                else if (tempFloats[j] == (roomCoordBreadth * 1.5f + 2) * gridBreadth ||
                    tempFloats[j] == -(roomCoordBreadth * 1.5f + 2) * gridBreadth)
                {
                    tempFloats[j] *= ((miniMapRoomCoordBreadth * 1.5f + 2) * gridBreadth) / ((roomCoordBreadth * 1.5f + 2) * gridBreadth);
                }
            }

            tempVector = new Vector3(tempFloats[0], tempFloats[1], tempFloats[2]);
            miniMapRoomPlanes[i].transform.position = Vector3RoundToInt(tempVector);

            //eulerangles
            miniMapRoomPlanes[i].transform.eulerAngles = roomPlanes[i].transform.eulerAngles;
        }
    }

    public bool IsMiniMapPlaneInTheFace(int planeIndex, int faceIndex)
    {
        tempVector = miniMapRoomPlanes[planeIndex].transform.position - miniMapTwistingCenters[faceIndex - 1].transform.position;
        tempFloat = Mathf.Abs(Vector3.Dot(tempVector, faceStableForwards[faceIndex - 1]));

        if (tempFloat <= (miniMapRoomCoordBreadth / 2 + 2) * gridBreadth &&
            tempFloat > 3 * gridBreadth)
        {
            return true;
        }

        return false;
    }

    public void HideAllMiniMapPlanes()
    {
        for (int i = 0; i < 54; i++)
        {
            miniMapRoomPlanes[i].SetActive(false);
        }
    }

    public void ActivateMiniMapPlanes()
    {
        for (int i = 0; i < 54; i++)
        {
            miniMapRoomPlanes[i].SetActive(IsRoomExplored(i));
        }
    }

    public void IntoMiniMap()
    {
        for (int i = 0; i < 54; i++)
        {
            miniMapRoomPlanes[i].SetActive(IsRoomExplored(i));
            //miniMapRoomPlanes[i].SetActive(true);

            if (i == VARS.curRoomIndex)
            {
                VARS.curMiniMapRoomPlaneColor = miniMapRoomPlanes[i].GetComponent<MeshRenderer>().material.GetColor("_MainColor");
                miniMapRoomPlanes[i].GetComponent<MeshRenderer>().material.SetColor("_MainColor", Color.white);
            }

            roomPlanes[i].SetActive(false);
        }

        cat.GetComponent<MeshRenderer>().enabled = false;

        //camPosition
        SetCameraPosition(-VARS.curRoomStableForward * camMiniMapDistanceToCubeCore);

        //camEulerangles
        VARS.camEuleranglesBeforeIntoMiniMap = camTransform.eulerAngles;

        //camSize
        SetCameraSize(camMiniMapSize);
    }

    public void OutOfMiniMap()
    {
        for (int i = 0; i < 54; i++)
        {
            miniMapRoomPlanes[i].SetActive(false);

            if (i == VARS.curRoomIndex)
            {
                miniMapRoomPlanes[i].GetComponent<MeshRenderer>().material.SetColor("_MainColor", VARS.curMiniMapRoomPlaneColor);

                roomPlanes[i].SetActive(true);
            }
        }

        cat.GetComponent<MeshRenderer>().enabled = true;

        //camPosition
        SetCameraPosition(VARS.curRoomCenter - VARS.curRoomStableForward * 7);

        //camEulerangles
        SetCameraEulerangles(VARS.camEuleranglesBeforeIntoMiniMap);

        //camSize
        SetCameraSize(camNormalSize);
    }
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
    public void MiniMapCameraRotate(int dirIndex, float rotationMovingStep)
    {
        tempVector = (VARS.curToMiniMapRotationCameraPoint.transform.position - VARS.curMiniMapRotationCameraPoint.transform.position).normalized;

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

    public void GetCurToMiniMapRotationCameraPoint(int dirIndex)
    {
        //getCurIndexAndCurPoint
        if (VARS.IsMiniMapRotationCameraPointIndexNotInitialized)
        {
            VARS.curMiniMapRotationCameraPointIndex = VARS.curFaceIndex - 1;

            VARS.IsMiniMapRotationCameraPointIndexNotInitialized = false;
        }
        VARS.curMiniMapRotationCameraPoint = miniMapRotationCameraPoints[VARS.curMiniMapRotationCameraPointIndex];
        Debug.Log("curIndex: " + VARS.curMiniMapRotationCameraPointIndex);

        //switch (dirIndex)
        //{
        //    case 1:
        //        VARS.curToMiniMapRotationCameraPoint = miniMapRotationCameraDownPoints[VARS.curMiniMapRotationCameraPointIndex];
        //        break;
        //    case 2:
        //        VARS.curToMiniMapRotationCameraPoint = miniMapRotationCameraUpPoints[VARS.curMiniMapRotationCameraPointIndex];
        //        break;
        //    case 3:
        //        VARS.curToMiniMapRotationCameraPoint = miniMapRotationCameraRightPoints[VARS.curMiniMapRotationCameraPointIndex];
        //        break;
        //    case 4:
        //        VARS.curToMiniMapRotationCameraPoint = miniMapRotationCameraLeftPoints[VARS.curMiniMapRotationCameraPointIndex];
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
                //tempVector = -miniMapRotationCameraPointStableUps[VARS.curMiniMapRotationCameraPointIndex];
                tempVector = -camTransform.up;
                //tempQuaternion = camTransform.rotation * Quaternion.AngleAxis(camTransform.eulerAngles.z, camTransform.forward);
                //tempVector = tempQuaternion.eulerAngles;
                break;
            case 2:
                //tempVector = miniMapRotationCameraPointStableUps[VARS.curMiniMapRotationCameraPointIndex];
                tempVector = camTransform.up;
                break;
            case 3:
                //tempVector = miniMapRotationCameraPointStableRights[VARS.curMiniMapRotationCameraPointIndex];
                tempVector = camTransform.right;
                break;
            case 4:
                //tempVector = -miniMapRotationCameraPointStableRights[VARS.curMiniMapRotationCameraPointIndex];
                tempVector = -camTransform.right;
                break;
        }
        Debug.Log("tempVector: " + tempVector);
        for (int i = 0; i < 26; i++)
        {
            //Debug.Log("enter1");

            //if (i == 4)
            //{
            //    Debug.Log(Vector3.Angle(miniMapRotationCameraPoints[i].transform.position - VARS.curMiniMapRotationCameraPoint.transform.position, tempVector));
            //    Debug.Log(Vector3.Dot(miniMapRotationCameraPoints[i].transform.position - VARS.curMiniMapRotationCameraPoint.transform.position, tempVector));
            //}

            //ifIsNearlyOnTheLine
            if (Vector3.Angle(miniMapRotationCameraPoints[i].transform.position - VARS.curMiniMapRotationCameraPoint.transform.position, tempVector) < 30)
            {
                Debug.Log("enter2: " + i);

                Debug.Log("dot: " + Vector3.Dot(miniMapRotationCameraPoints[i].transform.position - VARS.curMiniMapRotationCameraPoint.transform.position, tempVector));

                //ifIsTheRightDirection
                if (Vector3.Dot(miniMapRotationCameraPoints[i].transform.position - VARS.curMiniMapRotationCameraPoint.transform.position, tempVector) > 0)
                {
                    Debug.Log("enter3");

                    Debug.Log("curToIndex: " + i);

                    //Debug.Log(VARS.curToMiniMapRotationCameraPoint.transform.position - VARS.curMiniMapRotationCameraPoint.transform.position);

                    VARS.curToMiniMapRotationCameraPointIndex = i;
                    VARS.curToMiniMapRotationCameraPoint = miniMapRotationCameraPoints[i];
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

    #region CatMove
    public void SetHorCurSpeed(float value)
    {
        //Debug.Log("setHorCurSpeed: " + value);

        VARS.horCurSpeed = value;
    }

    public void AddHorCurSpeed(float value)
    {
        //Debug.Log("addHorCurSpeed: " + value);

        VARS.horCurSpeed += value;
    }

    public void SetVerCurSpeed(float value)
    {
        //Debug.Log("setVerCurSpeed: " + value);

        VARS.verCurSpeed = value;
    }

    public void AddVerCurSpeed(float value)
    {
        //Debug.Log("addVerCurSpeed: " + value);

        VARS.verCurSpeed += value;
    }
    #endregion

    #region CatEnergy
    public void SetCurEnergy(float value)
    {
        VARS.curEnergy = value;
    }

    public void AddCurEnergy(float value)
    {
        VARS.curEnergy += value;
    }
    #endregion
}
