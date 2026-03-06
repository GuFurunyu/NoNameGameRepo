using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[DefaultExecutionOrder((int)ScriptsExecutionOrder.ExecutionOrder.universalFunctionsLibrary)]
public class UniversalFunctionsLibrary : MonoBehaviour
{
    Constants CONS;
    Variables VARS;

    GameObject gameManager;

    Vector3 tempVector;

    #region ConstantsUsed
    float gridBreadth;
    int roomCoordBreadth;

    float inRoomMaxForwardDistance;

    GameObject[] roomPlanes = new GameObject[54];

    Transform camTransform;
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
        inRoomMaxForwardDistance= CONS.inRoomMaxForwardDistance;
        roomPlanes = CONS.roomPlanes;
        camTransform = CONS.camTransform;

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
    #endregion

    #region CameraManager
    public void SetCameraEulerangles(Vector3 targetEulerangles)
    {
        camTransform.eulerAngles = targetEulerangles;
    }

    public void CameraRotate(float rotationStep)
    {
        camTransform.Rotate(0, 0, rotationStep);
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
