using UnityEngine;

[DefaultExecutionOrder((int)ScriptsExecutionOrder.ExecutionOrder.cameraManager)]
public class CameraManager : MonoBehaviour
{
    Constants CONS;
    Variables VARS;
    UniversalFunctionsLibrary UFL;
    ScriptsExecutionController SEC;

    GameObject gameManager;

    float tempFloat;

    #region ConstantsUsed
    Transform camTransform;
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

        camTransform = CONS.camTransform;
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
            camTransform.position = VARS.roomCenters[VARS.curRoomIndex] - VARS.curRoomStableForward * 8;

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

            VARS.isInNewRoomCameraManagerResetOver = true;
        }
    }
}
