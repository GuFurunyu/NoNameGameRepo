using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

[DefaultExecutionOrder((int)ScriptsExecutionOrder.ExecutionOrder.roomsManager)]
public class RoomsManager : MonoBehaviour
{
    Constants CONS;
    Variables VARS;
    ScriptsExecutionController SEC;

    GameObject gameManager;

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

    float tempFloat1;
    float tempFloat2;
    float tempFloat3;
    float tempFloat4;
    Vector3 tempVector;

    #region ConstantsUsed
    float gridBreadth;
    int roomCoordBreadth;

    float inRoomMaxForwardDistance;

    GameObject[] faces = new GameObject[6];
    Vector3[] faceStableForwards = new Vector3[6];
    Vector3[] faceStableUps = new Vector3[6];
    Vector3[] faceStableRights = new Vector3[6];

    GameObject[] roomPlanes = new GameObject[54];

    GameObject[] twistingCenters = new GameObject[6];

    float twistSpeed;

    Transform camTransform;

    Transform catTransform;
    #endregion

    #region VariablesUsed
    Vector3[] roomCenters = new Vector3[54];
    Vector3[] roomStableForwards = new Vector3[54];
    Vector3[] roomStableUps = new Vector3[54];
    Vector3[] roomStableRights = new Vector3[54];
    #endregion

    void Start()
    {
        gameManager = GameObject.Find("GameManager");

        CONS = gameManager.GetComponent<Constants>();
        VARS = gameManager.GetComponent<Variables>();
        SEC = gameManager.GetComponent<ScriptsExecutionController>();

        gridBreadth = CONS.gridBreadth;
        roomCoordBreadth = CONS.roomCoordBreadth;
        inRoomMaxForwardDistance = CONS.inRoomMaxForwardDistance;
        faces = CONS.faces;
        faceStableForwards = CONS.faceStableForwards;
        faceStableUps = CONS.faceStableUps;
        faceStableRights = CONS.faceStableRights;
        roomPlanes = CONS.roomPlanes;
        twistingCenters = CONS.twistingCenters;
        twistSpeed = CONS.twistSpeed;
        camTransform = CONS.camTransform;
        catTransform = CONS.catTransform;
    }

    void Update()
    {
        roomCenters = VARS.roomCenters;
        roomStableForwards = VARS.roomStableForwards;
        roomStableUps = VARS.roomStableUps;
        roomStableRights = VARS.roomStableRights;

        #region IfIsInNewRoom
        if (IsInRoom(VARS.curRoomIndex, catTransform.position))
        {
        }
        else
        {
            for (int i = 0; i < roomCenters.Length; i++)
            {
                if (IsInRoom(i, catTransform.position))
                {
                    VARS.curRoomIndex = i;
                    break;
                }
            }

            VARS.isIntoNewRoom = true;
        }

        if (VARS.isIntoNewRoom)
        {
            IntoNewRoom();
        }
        #endregion

        #region InNewRoomReset
        if (VARS.isInNewRoomCurRoomManagerResetOver &&
            VARS.isInNewRoomCameraManagerResetOver &&
            VARS.isInNewRoomCatRotateResetOver &&
            VARS.isInNewRoomBlocksManagerResetOver)
        {
            VARS.isInNewRoom = false;

            VARS.isInNewRoomAllResetOver = true;
        }
        #endregion

        #region Twist
        if (VARS.isTwisting)
        {
            if (!isTwistingPresetOver)
            {
                curTwistingCenter = twistingCenters[VARS.twistingFaceIndex - 1];
                curTwistingCenterPosition = curTwistingCenter.transform.position;

                curFaceStableForward = faceStableForwards[VARS.twistingFaceIndex - 1];
                curFaceStableUp = faceStableUps[VARS.twistingFaceIndex - 1];
                curFaceStableRight = faceStableRights[VARS.twistingFaceIndex - 1];

                //getRelatedRoomPlanes
                for (int i = 0; i < roomPlanes.Length; i++)
                {
                    tempVector = roomCenters[i] - curTwistingCenterPosition;

                    //getRoomPlanesInTheFace
                    if (Mathf.Abs(Vector3.Dot(tempVector, curFaceStableForward)) <= (roomCoordBreadth / 2 + 2) * gridBreadth)
                    {
                        curRelatedRoomPlanes.Add(roomPlanes[i]);
                        curRelatedRoomPlaneIndexes.Add(i);
                    }

                    //getRoomPlanesSurroundingTheFace
                    if (Mathf.Abs(Vector3.SignedAngle(tempVector, curFaceStableUp, curFaceStableForward)) <= 6 &&
                        Mathf.Abs(Vector3.SignedAngle(tempVector, curFaceStableRight, curFaceStableForward)) <= 6)
                    {
                        curRelatedRoomPlanes.Add(roomPlanes[i]);
                        curRelatedRoomPlaneIndexes.Add(i);
                    }
                }

                //roomPlanesTempChildToCurTwistingCenter
                for(int i=0; i < curRelatedRoomPlanes.Count; i++)
                {
                    curRelatedRoomPlanes[i].transform.SetParent(curTwistingCenter.transform, true);
                }

                //catAndCamTempChildToCurTwistingCenter
                camTransform.SetParent(curTwistingCenter.transform, true);
                catTransform.SetParent(curTwistingCenter.transform, true);

                //setTargetEulerangles
                if (VARS.isClockwiseTwisting)
                {
                    twistingTargetEulerangles = curTwistingCenter.transform.eulerAngles + new Vector3(0, 0, -90);
                }
                else
                {
                    twistingTargetEulerangles = curTwistingCenter.transform.eulerAngles + new Vector3(0, 0, 90);
                }

                isTwistingPresetOver = true;
            }

            //twist
            if (twistingAccumulatedDegree < 90)
            {
                twistingAccumulatedDegree += twistSpeed * Time.deltaTime;
                if (VARS.isClockwiseTwisting)
                {
                    curTwistingCenter.transform.Rotate(curFaceStableForward * twistSpeed * Time.deltaTime);
                }
                else
                {
                    curTwistingCenter.transform.Rotate(-curFaceStableForward * twistSpeed * Time.deltaTime);
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

                //freeCatAndCam
                camTransform.SetParent(null);
                catTransform.SetParent(null);

                isTwistingPresetOver = false;

                twistingAccumulatedDegree = 0;

                VARS.isTwisting = false;
            }
        }
        #endregion
    }

    public bool IsInRoom(int roomIndex, Vector3 position)
    {
        tempVector = position - roomCenters[roomIndex];

        //ifIsInThePlane
        if (Mathf.Abs(Vector3.Dot(tempVector, roomStableForwards[roomIndex])) <= inRoomMaxForwardDistance)
        {
            //ifIsInsideTheBoundary
            if (Mathf.Abs(Vector3.Dot(tempVector, roomStableUps[roomIndex])) <= (roomCoordBreadth / 2 + 1) * gridBreadth &&
                Mathf.Abs(Vector3.Dot(tempVector, roomStableRights[roomIndex])) <= (roomCoordBreadth / 2 + 1)* gridBreadth)
            {
                return true;
            }
        }

        return false;
    }

    void IntoNewRoom()
    {
        VARS.isIntoNewRoom = false;

        VARS.isInNewRoom = true;

        VARS.isInNewRoomCurRoomManagerResetOver = false;
        VARS.isInNewRoomCameraManagerResetOver = false;
        VARS.isInNewRoomCatRotateResetOver = false;
        VARS.isInNewRoomBlocksManagerResetOver = false;
        VARS.isInNewRoomAllResetOver = false;
    }

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
                if (tempFloat2 < 0)
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
