using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder((int)ScriptsExecutionOrder.ExecutionOrder.variables)]
public class Variables : MonoBehaviour
{
    #region ScriptsExecutionController
    [Header("- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -" +
        "  \nSCRIPTSEXECUTIONCONTROLLER\n --- ")]
    //scriptExecutionParts:
    //    ~reset~main

    //roomsManager
    public bool isRoomsManagerResetting;
    public bool isRoomsManagerMainActivated;
    #endregion

    #region DataManager
    [Header("- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -" +
        "  \nDATAMANAGER\n --- ")]
    //keyCodes
    public bool isToWriteKeyCodesData;
    #endregion

    #region RoomsManager
    [Header("- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -" +
        "  \nROOMSMANAGER\n --- ")]
    //roomsInfo
    public Vector3[] roomCenters = new Vector3[54];
    public Vector3[] roomStableForwards = new Vector3[54];
    public Vector3[] roomStableUps = new Vector3[54];
    public Vector3[] roomStableRights = new Vector3[54];
    //public List<Vector3> roomOPoints = new List<Vector3>();

    //~?
    //bool isRoomDetermined;

    //isInNewRoom
    public bool isIntoNewRoom = true;
    public bool isInNewRoom;

    //isInNewRoomResetOver
    public bool isInNewRoomCurRoomManagerResetOver;
    public bool isInNewRoomCameraManagerResetOver;
    public bool isInNewRoomCatRotateResetOver;
    public bool isInNewRoomBlocksManagerResetOver;
    public bool isInNewRoomAllResetOver;

    //curRoomIndex
    public int curRoomIndex;

    //twist
    public bool isTwisting;
    public int twistingFaceIndex;
    public bool isClockwiseTwisting;
    #endregion

    #region CurRoomManager
    [Header("- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -" +
        "  \nCURROOMMANAGER\n --- ")]
    //curRoomInfo
    public Vector3 curRoomCenter;
    public Vector3 curRoomStableForward;
    public Vector3 curRoomStableUp;
    public Vector3 curRoomStableRight;
    //Vector3 curRoomOPoint;

    //ucrDirections
    public Vector3 curUp = Vector3.zero;
    public Vector3 curRight = Vector3.zero;

    //public Vector3 planeForward;
    //public Vector3 planeUp;
    //public Vector3 planeRight;
    //public Vector3 iniUp;
    //public Vector3 iniRight;

    //gravity
    public float curRoomGravity;
    #endregion

    #region TileData
    [Header("- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -" +
        "  \nTILEDATA\n --- ")]

    #endregion

    #region CameraManager
    [Header("- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -" +
        "  \nCAMERAMANAGER\n --- ")]
    //iniEulerAngles
    public Vector3 camIniEulerangles;
    #endregion

    #region Cat
    [Header("- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -" +
        "  \nCAT\n --- ")]
    //position
    public Vector3 curCatPosition;
    #endregion

    #region Control
    [Header("- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -" +
        "  \nCONTROL\n --- ")]
    //keyCodes
    public KeyCode upKeyCode = KeyCode.W;
    public KeyCode downKeyCode = KeyCode.S;
    public KeyCode leftKeyCode = KeyCode.A;
    public KeyCode rightKeyCode = KeyCode.D;
    public KeyCode jumpKeyCode = KeyCode.K;
    public KeyCode dashKeyCode = KeyCode.L;
    public KeyCode confirmKeyCode = KeyCode.Space;
    public KeyCode backKeyCode = KeyCode.Escape;

    public bool isKeyCodeChanged = true;

    //isInputtingKey
    public bool isInputtingLeftKey;
    public bool isInputtingRightKey;
    public bool isInputtingUpKey;
    public bool isInputtingDownKey;
    public bool isInputtingJumpKey;
    public bool isInputtingDashKey;
    public bool isInputtingConfirmKey;
    public bool isInputtingBackKey;

    //isKeyDown
    public bool isUpKeyDown;
    public bool isDownKeyDown;
    public bool isLeftKeyDown;
    public bool isRightKeyDown;
    public bool isJumpKeyDown;
    public bool isDashKeyDown;
    public bool isConfirmKeyDown;
    public bool isBackKeyDown;

    ////lastHotDirectionInput
    //public KeyCode lastHorDirectionInput;

    //isInputting
    public bool isHorInputting;
    //bool isVerInputting;
    #endregion

    #region CatCollision
    [Header("- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -" +
        "  \nCATCOLLISION\n --- ")]
    //solidCollisionState
    public bool isLeftBlocked;
    public bool isRightBlocked;
    public bool isOnGround;
    public bool isToCeiling;
    public bool leftBlockDetected;
    public bool rightBlockDetected;
    public bool groundDetected;
    public bool ceilingDetected;

    //fluidCollisionState
    public bool isInLiquid;
    public bool isInGas;
    public bool isInMist;
    public bool liquidDetected;
    public bool gasDetected;
    public bool mistDetected;

    //attachWall
    public bool isAttachWall;

    //solidTileData
    public TileData curUpTileData;
    public TileData curDownTileData;
    public TileData curLeftTileData;
    public TileData curRightTileData;
    public Vector3 curTilePosition;
    //Vector3 curTileType;
    //float curTileX;
    //float curTileY;

    //fluidTileData
    public TileData curLiquidTileData;
    public TileData curGasTileData;
    public TileData curMistTileData;

    //~?
    public float buoyancyDistanceFixFloat;

    //triggerTile
    public GameObject curTriggerTile;
    public TileData curTriggerTileData;
    #endregion

    #region CatMove
    [Header("- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -" +
        "  \nCATMOVE\n --- ")]
    //curSpeed
    public float horCurSpeed;
    public float verCurSpeed;

    public bool isHighJumping;

    //curFacingDirection
    //1-left, 2-right
    public float curFacingDirectionIndex;

    //isStill
    public bool isCatStill;
    #endregion

    #region CatRotate
    [Header("- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -" +
        "  \nCATROTATE\n --- ")]
    //rotationNum
    public int rotationRestNum;

    //isRotating
    public bool isRotating;

    //isIniRotation
    public bool isIniRotation;
    public float outIniRotationStartTime;
    #endregion

    #region CatEnergy
    [Header("- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -" +
        "  \nCATENERGY\n --- ")]
    //energy
    public float curEnergy;
    #endregion

    #region CatTrigger
    [Header("- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -" +
        "  \nCATTRIGGER\n --- ")]
    //strawberry
    public bool isCarryingStrawberries;
    public bool isCollectingStrawberries;
    public bool isGettingAStrawberry;
    public bool isToLoseCarriedStrawberries;

    //energyCrystal
    public bool isGettingAnEnergyCrystal;

    //edgeGate
    public bool isEnteringAnEdgeGate;
    public bool isEdgeGateTriggered;
    #endregion

    #region CatAppearance
    [Header("- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -" +
        "  \nCATAPPEARANCE\n --- ")]
    //contraction
    public bool isContracting;

    //color
    public bool isInFadedColor;
    #endregion

    #region CatDeath
    [Header("- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -" +
        "  \nCATDEATH\n --- ")]
    //toDie
    public bool isToDie;
    #endregion

    #region BlocksManager
    [Header("- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -" +
        "  \nBLOCKSMANAGER\n --- ")]
    //curPlaneEmpty
    //~?
    public GameObject curPlaneEmpty;

    //gravity
    //~?
    public float curGravity;

    //~?
    //int[] curGoableBlockTypeIndexes;

    //fluidContinuosnessOptimization
    public bool isFluidContinuousnessOptimizationActivated = true;
    #endregion

    #region OptionsManager
    [Header("- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -" +
        "  \nOPTIONSMANAGER\n --- ")]
    //isOptionPanelActivated
    public bool isOptionPanelActivated;

    //curKeyCodes
    public List<KeyCode> curKeyCodes = new List<KeyCode>();
    #endregion

    #region ConstantsUsed
    Transform catTransform;

    float catBreadth;
    #endregion

    void Start()
    {
    }
}
