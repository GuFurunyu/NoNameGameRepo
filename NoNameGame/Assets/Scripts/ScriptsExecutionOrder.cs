using UnityEngine;

[DefaultExecutionOrder((int)ScriptsExecutionOrder.ExecutionOrder.scriptsExecutionOrder)]
public class ScriptsExecutionOrder : MonoBehaviour
{
    #region OutVersionExecutionOrder
    //public class ExecutionOrder
    //{
    //    public const int scriptsInteractionCenter = 51;
    //    public const int constants = 52;
    //    public const int variables = 53;
    //    public const int roomsManager = 54;
    //    public const int curRoomManager = 55;
    //    public const int tileData = 56;
    //    public const int cameraManager = 57;
    //    public const int catControl = 58;
    //    public const int catCollision = 59;
    //    public const int catMove = 60;
    //    public const int catRotate = 61;
    //    public const int catEnergy = 62;
    //    public const int catTrigger = 63;
    //    public const int catAppearance = 64;
    //    public const int catDeath = 65;
    //    public const int blocksManager = 66;
    //}
    #endregion

    public enum ExecutionOrder
    {
        scriptsExecutionOrder,
        constants,
        variables,
        universalFunctionsLibrary,
        scriptsExecutionController,
        dataManager,
        roomsManager,
        curRoomManager,
        tileData,
        cameraManager,
        control,
        catCollision,
        catMove,
        catRotate,
        catEnergy,
        catState,
        catTrigger,
        catAppearance,
        catDeath,
        blocksManager,//fixedUpdate
        miniMapManager,
        optionsManager,
        screenPostProcessor,
        audioManager,
        test
    }

    //private void Start()
    //{
    //    //CAUTION
    //    //filterOutIgnorableWarnings
    //    Application.logMessageReceived += (condition, stackTrace, type) =>
    //    {
    //        if (condition.Contains("_unity_self")/* && type == LogType.Warning*/)
    //        {
    //            Debug.Log("enter");

    //            return;
    //        }

    //        Debug.LogFormat("{0}: {1}", type, condition);
    //    };
    //}
}
