using UnityEngine;

[DefaultExecutionOrder((int)ScriptsExecutionOrder.ExecutionOrder.cameraManager)]
public class CameraManager : MonoBehaviour
{
    Constants CONS;
    Variables VARS;
    UniversalFunctionsLibrary UFL;
    ScriptsExecutionController SEC;

    GameObject gameManager;

    GameObject[] initialSightMasks = new GameObject[4];

    float accumulatedInitialSightMasksDistance;

    float tempFloat;

    #region ConstantsUsed
    float gridBreadth;
    int roomCoordBreadth;

    Transform camTransform;

    GameObject initialSightMasksEmpty;
    float initialSightMasksSpeed;
    float initialSightMasksMaxDistance;

    Transform catTransform;
    #endregion

    #region VariablesUsed
    //Vector3 planeForward;
    //Vector3 planeUp;
    //Vector3 planeRight;
    //Vector3 iniRight;
    #endregion

    void Start()
    {
        gameManager = GameObject.Find("GameManager");

        CONS = gameManager.GetComponent<Constants>();
        VARS = gameManager.GetComponent<Variables>();
        UFL = gameManager.GetComponent<UniversalFunctionsLibrary>();
        SEC = gameManager.GetComponent<ScriptsExecutionController>();

        gridBreadth = CONS.gridBreadth;
        roomCoordBreadth = CONS.roomCoordBreadth;
        camTransform = CONS.camTransform;
        initialSightMasksEmpty = CONS.initialSightMasksEmpty;
        initialSightMasksSpeed = CONS.initialSightMasksSpeed;
        initialSightMasksMaxDistance = CONS.initialSightMasksMaxDistance;
        catTransform = CONS.catTransform;

        //getInitialSightMasks
        for (int i = 0; i < 4; i++)
        {
            initialSightMasks[i] = initialSightMasksEmpty.transform.GetChild(i).gameObject;
        }
    }

    void Update()
    {
        //planeForward = VARS.planeForward;
        //planeUp = VARS.planeUp;
        //planeRight = VARS.planeRight;
        //iniRight = VARS.iniRight;

        if (VARS.isInNewRoom)
        {
            //position
            camTransform.position = VARS.roomCenters[VARS.curRoomIndex] - VARS.curRoomStableForward * 7;

            //tempFloat = camTransform.eulerAngles.z;
            //camTransform.LookAt(CONS.roomPlanes[VARS.curRoomIndex].transform.position);
            //camIniEulerangles = camTransform.eulerAngles;
            //camTransform.eulerAngles = new Vector3(camTransform.eulerAngles.x, camTransform.eulerAngles.y, tempFloat);
            //camIniEulerangles += Vector3.forward * Vector3.SignedAngle(planeRight, iniRight, planeForward);

            //eulerangles
            tempFloat = camTransform.eulerAngles.z;
            camTransform.LookAt(CONS.roomPlanes[VARS.curRoomIndex].transform.position);
            VARS.camIniEulerangles = camTransform.eulerAngles;
            VARS.camIniEulerangles += Vector3.forward * Vector3.SignedAngle(camTransform.right, VARS.curRoomStableRight, VARS.curRoomStableForward);
            camTransform.eulerAngles = new Vector3(camTransform.eulerAngles.x, camTransform.eulerAngles.y, tempFloat);

            VARS.curUp = new Vector3
                (Mathf.RoundToInt(camTransform.up.x), Mathf.RoundToInt(camTransform.up.y), Mathf.RoundToInt(camTransform.up.z));
            VARS.curRight = new Vector3
                (Mathf.RoundToInt(camTransform.right.x), Mathf.RoundToInt(camTransform.right.y), Mathf.RoundToInt(camTransform.right.z));

            if (VARS.isToInitializeSight)
            {
                for (int i = 0; i < 4; i++)
                {
                    initialSightMasks[i].SetActive(true);
                    initialSightMasks[i].transform.position = catTransform.position - VARS.curRoomStableForward * 4;
                }

                initialSightMasks[0].transform.position += VARS.curRoomStableUp * roomCoordBreadth * gridBreadth;
                initialSightMasks[1].transform.position -= VARS.curRoomStableUp * roomCoordBreadth * gridBreadth;
                initialSightMasks[2].transform.position -= VARS.curRoomStableRight * roomCoordBreadth * gridBreadth;
                initialSightMasks[3].transform.position += VARS.curRoomStableRight * roomCoordBreadth * gridBreadth;

                accumulatedInitialSightMasksDistance = 0;

                VARS.isInitializingSight = true;

                VARS.isToInitializeSight = false;
            }

            VARS.isInNewRoomCameraManagerResetOver = true;
        }

        //inCurRoomManager?
        //sightExpand
        if (VARS.isInNewRoomAllResetOver)
        {
            if (VARS.isInitializingSight)
            {
                if (accumulatedInitialSightMasksDistance < initialSightMasksMaxDistance)
                {
                    initialSightMasks[0].transform.position += VARS.curRoomStableUp * initialSightMasksSpeed * Time.deltaTime;
                    initialSightMasks[1].transform.position -= VARS.curRoomStableUp * initialSightMasksSpeed * Time.deltaTime;
                    initialSightMasks[2].transform.position -= VARS.curRoomStableRight * initialSightMasksSpeed * Time.deltaTime;
                    initialSightMasks[3].transform.position += VARS.curRoomStableRight * initialSightMasksSpeed * Time.deltaTime;

                    accumulatedInitialSightMasksDistance += initialSightMasksSpeed * Time.deltaTime;
                }
                else
                {
                    for (int i = 0; i < 4; i++)
                    {
                        initialSightMasks[i].SetActive(false);
                    }

                    VARS.isInitializingSight = false;
                }
            }
        }
    }
}
