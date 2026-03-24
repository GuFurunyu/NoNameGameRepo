using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
//using ECB = Variables.ExecutionControlBool;

[DefaultExecutionOrder((int)ScriptsExecutionOrder.ExecutionOrder.variables)]
public class Variables : MonoBehaviour
{
    #region ScriptsExecutionController
    //[Header("- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -" +
    //    "  \nSCRIPTSEXECUTIONCONTROLLER\n --- ")]
    ////scriptExecutionParts:
    ////    ~reset~main

    ////roomsManager
    //public bool isRoomsManagerResetting;
    //public bool isRoomsManagerMainActivated;
    #endregion

    #region DataManager
    [Header("- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -" +
        "  \nDATAMANAGER\n --- ")]
    //keyCodesData
    [SerializeField] private bool _isToWriteKeyCodesData;
    public bool IsToWriteKeyCodesData { get { return _isToWriteKeyCodesData; } set { _isToWriteKeyCodesData = value; } }


    //worldData
    [SerializeField] private bool _isToWriteWorldData;
    public bool IsToWriteWorldData { get { return _isToWriteWorldData; } set { _isToWriteWorldData = value; } }


    //catWorldData
    [SerializeField] private bool _isToWriteCatWorldData;
    public bool IsToWriteCatWorldData { get { return _isToWriteCatWorldData; } set { _isToWriteCatWorldData = value; } }

    #endregion

    #region RoomsManager
    [Header("- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -" +
        "  \nROOMSMANAGER\n --- ")]
    //~?
    //bool isRoomDetermined;

    //isInNewRoom
    [SerializeField] private bool _isIntoNewRoom = true;
    public bool IsIntoNewRoom
    {
        get { return _isIntoNewRoom; }
        set
        {
            _isIntoNewRoom = value;

            if (value == true)
            {
                IsIntoNewRoom = false;

                IsInNewRoom = true;

                IsInNewRoomCurRoomManagerResetOver = false;
                IsInNewRoomCameraManagerResetOver = false;
                IsInNewRoomCatRotateResetOver = false;
                IsInNewRoomBlocksManagerResetOver = false;
            }
        }
    }

    [SerializeField] private bool _isInNewRoom = true; 
    public bool IsInNewRoom { get { return _isInNewRoom; } set { _isInNewRoom = value; } }


    //isInNewRoomResetOver
    [SerializeField] private bool _isInNewRoomCurRoomManagerResetOver;
    public bool IsInNewRoomCurRoomManagerResetOver
    {
        get { return _isInNewRoomCurRoomManagerResetOver; }
        set
        {
            _isInNewRoomCurRoomManagerResetOver = value;
        }
    }

    [SerializeField] private bool _isInNewRoomCameraManagerResetOver;
    public bool IsInNewRoomCameraManagerResetOver
    {
        get { return _isInNewRoomCameraManagerResetOver; }
        set
        {
            _isInNewRoomCameraManagerResetOver = value;
        }
    }

    [SerializeField] private bool _isInNewRoomCatRotateResetOver;
    public bool IsInNewRoomCatRotateResetOver
    {
        get { return _isInNewRoomCatRotateResetOver; }
        set
        {
            _isInNewRoomCatRotateResetOver = value;
        }
    }

    [SerializeField] private bool _isInNewRoomBlocksManagerResetOver;
    public bool IsInNewRoomBlocksManagerResetOver
    {
        get { return _isInNewRoomBlocksManagerResetOver; }
        set
        {
            _isInNewRoomBlocksManagerResetOver = value;
        }
    }

    [SerializeField] private bool _isInNewRoomAllResetOver;
    public bool IsInNewRoomAllResetOver
    {
        get
        {
            IsInNewRoomAllResetOver =
                IsInNewRoomCurRoomManagerResetOver &&
                IsInNewRoomCameraManagerResetOver &&
                IsInNewRoomCatRotateResetOver &&
                IsInNewRoomBlocksManagerResetOver;

            return _isInNewRoomAllResetOver;
        }
        set
        {
            if (value == true) IsInNewRoom = false;
            _isInNewRoomAllResetOver = value;
        }
    }


    //roomsInfo
    public Vector3[] roomCenters = new Vector3[54];
    public Vector3[] roomStableForwards = new Vector3[54];
    public Vector3[] roomStableUps = new Vector3[54];
    public Vector3[] roomStableRights = new Vector3[54];
    //public List<Vector3> roomOPoints = new List<Vector3>();

    //edgeGatesLinkedToIndexes
    public List<int> edgeGateLinkedToIndexes = new List<int>();

    //isRoomExploredList
    [SerializeField] private bool[] _isRoomExplored = new bool[54];
    public bool[] IsRoomExplored { get { return _isRoomExplored; } set { _isRoomExplored = value; } }

    //center
    [SerializeField] private bool _isInCenter;
    public bool IsInCenter { get { return _isInCenter; } set { _isInCenter = value; } }

    //twist
    [SerializeField] private bool _isTwisting;
    public bool IsTwisting { get { return _isTwisting; } set { _isTwisting = value; } }

    public int twistingFaceIndex;
    [SerializeField] private bool _isClockwiseTwisting;
    public bool IsClockwiseTwisting { get { return _isClockwiseTwisting; } set { _isClockwiseTwisting = value; } }


    //inRoomsManager?
    //[SerializeField] private bool _isIntoMiniMap;
    //public bool IsIntoMiniMap { get { return _isIntoMiniMap; } set { _isIntoMiniMap = value; } }

    [SerializeField] private bool _isInMiniMap;
    public bool IsInMiniMap { get { return _isInMiniMap; } set { _isInMiniMap = value; } }

    [SerializeField] private bool _isMiniMapRotating;
    public bool IsMiniMapRotating { get { return _isMiniMapRotating; } set { _isMiniMapRotating = value; } }

    public Color curMiniMapRoomPlaneColor;

    #endregion

    #region CurRoomManager
    [Header("- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -" +
        "  \nCURROOMMANAGER\n --- ")]
    //curFaceIndex
    public int curFaceIndex;

    //curRoomIndex
    public int curRoomIndex;

    //curPlaneEmpty
    //~?
    public GameObject curPlaneEmpty;

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
    //[Header("- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -" +
    //    "  \nTILEDATA\n --- ")]

    #endregion

    #region CameraManager
    [Header("- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -" +
        "  \nCAMERAMANAGER\n --- ")]
    //iniEulerAngles
    public Vector3 camIniEulerangles;

    //initializeSight
    //public bool isToInitializeSight;
    //public bool isInitializingSight;
    [SerializeField] private bool _isToInitializeSight = true;
    public bool IsToInitializeSight { get { return _isToInitializeSight; } set { _isToInitializeSight = value; } }

    [SerializeField] private bool _isInitializingSight;
    public bool IsInitializingSight { get { return _isInitializingSight; } set { _isInitializingSight = value; } }

    //zoom
    [SerializeField] private bool _isToZoomOut;
    public bool IsToZoomOut { get { return _isToZoomOut; } set { _isToZoomOut = value; } }

    [SerializeField] private bool _isZoomingOut;
    public bool IsZoomingOut { get { return _isZoomingOut; } set { _isZoomingOut = value; } }

    [SerializeField] private bool _isZoomedOut;
    public bool IsZoomedOut { get { return _isZoomedOut; } set { _isZoomedOut = value; } }

    [SerializeField] private bool _isToZoomIn;
    public bool IsToZoomIn { get { return _isToZoomIn; } set { _isToZoomIn = value; } }

    [SerializeField] private bool _isZoomingIn;
    public bool IsZoomingIn { get { return _isZoomingIn; } set { _isZoomingIn = value; } }


    public Vector3 camEuleranglesBeforeIntoMiniMap;
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
    //public KeyCode dashKeyCode = KeyCode.L;
    public KeyCode acceKeyCode = KeyCode.L;
    public KeyCode grabKeyCode = KeyCode.J;
    public KeyCode confirmKeyCode = KeyCode.Space;
    public KeyCode backKeyCode = KeyCode.Escape;

    [SerializeField] private bool _isKeyCodeChanged = true;
    public bool IsKeyCodeChanged { get { return _isKeyCodeChanged; } set { _isKeyCodeChanged = value; } }


    //isInputtingKey
    [SerializeField] private bool _isInputtingUpKey;
    public bool IsInputtingUpKey { get { return _isInputtingUpKey; } set { _isInputtingUpKey = value; } }

    [SerializeField] private bool _isInputtingDownKey;
    public bool IsInputtingDownKey { get { return _isInputtingDownKey; } set { _isInputtingDownKey = value; } }

    [SerializeField] private bool _isInputtingLeftKey;
    public bool IsInputtingLeftKey { get { return _isInputtingLeftKey; } set { _isInputtingLeftKey = value; } }

    [SerializeField] private bool _isInputtingRightKey;
    public bool IsInputtingRightKey { get { return _isInputtingRightKey; } set { _isInputtingRightKey = value; } }

    [SerializeField] private bool _isInputtingJumpKey;
    public bool IsInputtingJumpKey { get { return _isInputtingJumpKey; } set { _isInputtingJumpKey = value; } }

    [SerializeField] private bool _isInputtingDashKey;
    public bool IsInputtingDashKey { get { return _isInputtingDashKey; } set { _isInputtingDashKey = value; } }

    [SerializeField] private bool _isInputtingAcceKey;
    public bool IsInputtingAcceKey { get { return _isInputtingAcceKey; } set { _isInputtingAcceKey = value; } }

    [SerializeField] private bool _isInputtingGrabKey;
    public bool IsInputtingGrabKey { get { return _isInputtingGrabKey; } set { _isInputtingGrabKey = value; } }

    [SerializeField] private bool _isInputtingConfirmKey;
    public bool IsInputtingConfirmKey { get { return _isInputtingConfirmKey; } set { _isInputtingConfirmKey = value; } }

    [SerializeField] private bool _isInputtingBackKey;
    public bool IsInputtingBackKey { get { return _isInputtingBackKey; } set { _isInputtingBackKey = value; } }


    //isKeyDown
    [SerializeField] private bool _isUpKeyDown;
    public bool IsUpKeyDown { get { return _isUpKeyDown; } set { _isUpKeyDown = value; } }

    [SerializeField] private bool _isDownKeyDown;
    public bool IsDownKeyDown { get { return _isDownKeyDown; } set { _isDownKeyDown = value; } }

    [SerializeField] private bool _isLeftKeyDown;
    public bool IsLeftKeyDown { get { return _isLeftKeyDown; } set { _isLeftKeyDown = value; } }

    [SerializeField] private bool _isRightKeyDown;
    public bool IsRightKeyDown { get { return _isRightKeyDown; } set { _isRightKeyDown = value; } }

    [SerializeField] private bool _isJumpKeyDown;
    public bool IsJumpKeyDown { get { return _isJumpKeyDown; } set { _isJumpKeyDown = value; } }

    [SerializeField] private bool _isDashKeyDown;
    public bool IsDashKeyDown { get { return _isDashKeyDown; } set { _isDashKeyDown = value; } }

    [SerializeField] private bool _isAcceKeyDown;
    public bool IsAcceKeyDown { get { return _isAcceKeyDown; } set { _isAcceKeyDown = value; } }

    [SerializeField] private bool _isGrabKeyDown;
    public bool IsGrabKeyDown { get { return _isGrabKeyDown; } set { _isGrabKeyDown = value; } }

    [SerializeField] private bool _isConfirmKeyDown;
    public bool IsConfirmKeyDown { get { return _isConfirmKeyDown; } set { _isConfirmKeyDown = value; } }

    [SerializeField] private bool _isBackKeyDown;
    public bool IsBackKeyDown { get { return _isBackKeyDown; } set { _isBackKeyDown = value; } }

    //isKeyUp
    [SerializeField] private bool _isUpKeyUp;
    public bool IsUpKeyUp { get { return _isUpKeyUp; } set { _isUpKeyUp = value; } }

    [SerializeField] private bool _isDownKeyUp;
    public bool IsDownKeyUp { get { return _isDownKeyUp; } set { _isDownKeyUp = value; } }

    [SerializeField] private bool _isLeftKeyUp;
    public bool IsLeftKeyUp { get { return _isLeftKeyUp; } set { _isLeftKeyUp = value; } }

    [SerializeField] private bool _isRightKeyUp;
    public bool IsRightKeyUp { get { return _isRightKeyUp; } set { _isRightKeyUp = value; } }

    [SerializeField] private bool _isJumpKeyUp;
    public bool IsJumpKeyUp { get { return _isJumpKeyUp; } set { _isJumpKeyUp = value; } }

    [SerializeField] private bool _isDashKeyUp;
    public bool IsDashKeyUp { get { return _isDashKeyUp; } set { _isDashKeyUp = value; } }

    [SerializeField] private bool _isAcceKeyUp;
    public bool IsAcceKeyUp { get { return _isAcceKeyUp; } set { _isAcceKeyUp = value; } }

    [SerializeField] private bool _isGrabKeyUp;
    public bool IsGrabKeyUp { get { return _isGrabKeyUp; } set { _isGrabKeyUp = value; } }

    [SerializeField] private bool _isConfirmKeyUp;
    public bool IsConfirmKeyUp { get { return _isConfirmKeyUp; } set { _isConfirmKeyUp = value; } }

    [SerializeField] private bool _isBackKeyUp;
    public bool IsBackKeyUp { get { return _isBackKeyUp; } set { _isBackKeyUp = value; } }

    ////lastHotDirectionInput
    //public KeyCode lastHorDirectionInput;

    //isInputting
    [SerializeField] private bool _isHorInputting;
    public bool IsHorInputting { get { return _isHorInputting; } set { _isHorInputting = value; } }

    //bool isVerInputting;
    #endregion

    #region CatCollision
    [Header("- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -" +
        "  \nCATCOLLISION\n --- ")]
    //executability
    [SerializeField] private bool _isCatCollisionMainPartExecutable;
    public bool IsCatCollisionMainPartExecutable { get { return _isCatCollisionMainPartExecutable; } set { _isCatCollisionMainPartExecutable = value; } }

    //notTouchingAnything
    [SerializeField] private bool _isNotTouchingAnything;
    public bool IsNotTouchingAnything
    {
        get
        {
            IsNotTouchingAnything =
                !IsOnGround &&
                !IsToCeiling &&
                !IsLeftBlocked &&
                !IsRightBlocked &&
                !IsInLiquid &&
                !IsInGas &&
                !IsInMist;

            return _isNotTouchingAnything;
        }
        set { _isNotTouchingAnything = value; }
    }

    //solidCollisionState
    [SerializeField] private bool _isOnGround;
    public bool IsOnGround { get { return _isOnGround; } set { _isOnGround = value; } }

    [SerializeField] private bool _isToCeiling;
    public bool IsToCeiling { get { return _isToCeiling; } set { _isToCeiling = value; } }

    [SerializeField] private bool _isLeftBlocked;
    public bool IsLeftBlocked { get { return _isLeftBlocked; } set { _isLeftBlocked = value; } }

    [SerializeField] private bool _isRightBlocked;
    public bool IsRightBlocked { get { return _isRightBlocked; } set { _isRightBlocked = value; } }

    [SerializeField] private bool _isGroundDetected;
    public bool IsGroundDetected { get { return _isGroundDetected; } set { _isGroundDetected = value; } }

    [SerializeField] private bool _isCeilingDetected;
    public bool IsCeilingDetected { get { return _isCeilingDetected; } set { _isCeilingDetected = value; } }

    [SerializeField] private bool _isLeftBlockDetected;
    public bool IsLeftBlockDetected { get { return _isLeftBlockDetected; } set { _isLeftBlockDetected = value; } }

    [SerializeField] private bool _isRightBlockDetected;
    public bool IsRightBlockDetected { get { return _isRightBlockDetected; } set { _isRightBlockDetected = value; } }


    //fluidCollisionState
    [SerializeField] private bool _isInLiquid;
    public bool IsInLiquid { get { return _isInLiquid; } set { _isInLiquid = value; } }

    [SerializeField] private bool _isInGas;
    public bool IsInGas { get { return _isInGas; } set { _isInGas = value; } }

    [SerializeField] private bool _isInMist;
    public bool IsInMist { get { return _isInMist; } set { _isInMist = value; } }

    [SerializeField] private bool _isLiquidDetected;
    public bool IsLiquidDetected { get { return _isLiquidDetected; } set { _isLiquidDetected = value; } }

    [SerializeField] private bool _isGasDetected;
    public bool IsGasDetected { get { return _isGasDetected; } set { _isGasDetected = value; } }

    [SerializeField] private bool _isMistDetected;
    public bool IsMistDetected { get { return _isMistDetected; } set { _isMistDetected = value; } }


    //attachWall
    [SerializeField] private bool _isAttachWall;
    public bool IsAttachWall { get { return _isAttachWall; } set { _isAttachWall = value; } }

    //attachCeiling
    [SerializeField] private bool _isAttachCeiling;
    public bool IsAttachCeiling { get { return _isAttachCeiling; } set { _isAttachCeiling = value; } }

    //solidTile
    public GameObject curUpTile;
    public GameObject curDownTile;
    public GameObject curLeftTile;
    public GameObject curRightTile;
    public GameObject curAttachedWallTile;
    public GameObject curAttachedCeilingTile;

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

    //railBlock
    [SerializeField] private bool _isOnOrToARailBlock;
    public bool IsOnOrToARailBlock
    {
        get
        {
            if(IsNotTouchingAnything)
                _isOnOrToARailBlock = false;

            return _isOnOrToARailBlock;
        }
        set { _isOnOrToARailBlock = value; }
    }

    public GameObject curOnOrToRailBlock;

    //triggerTile
    public GameObject curTriggerTile;
    public TileData curTriggerTileData;
    #endregion

    #region CatMove
    [Header("- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -" +
        "  \nCATMOVE\n --- ")]
    //executability
    [SerializeField] private bool _isCatMoveMainPartExecutable;
    public bool IsCatMoveMainPartExecutable { get { return _isCatMoveMainPartExecutable; } set { _isCatMoveMainPartExecutable = value; } }

    //curSpeed
    //public float horCurSpeed;
    //public float verCurSpeed;
    [SerializeField] private float _horCurSpeed;
    public float horCurSpeed { get { return _horCurSpeed; } set { _horCurSpeed = value; } }

    [SerializeField] private float _verCurSpeed;
    public float verCurSpeed { get { return _verCurSpeed; } set { _verCurSpeed = value; } }

    [SerializeField] private bool _isHighJumping;
    public bool IsHighJumping { get { return _isHighJumping; } set { _isHighJumping = value; } }

    //isAcced
    [SerializeField] private bool _isInAcce;
    public bool IsInAcce { get { return _isInAcce; } set { _isInAcce = value; } }


    //curFacingDirection
    //1-left, 2-right
    public float curFacingDirectionIndex;

    //isStill
    [SerializeField] private bool _isCatStill;
    public bool IsCatStill { get { return _isCatStill; } set { _isCatStill = value; } }

    [SerializeField] private bool _isDashEnabled;
    public bool IsDashEnabled { get { return _isDashEnabled; } set { _isDashEnabled = value; } }

    #endregion

    #region CatRotate
    [Header("- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -" +
        "  \nCATROTATE\n --- ")]
    //executability
    [SerializeField] private bool _isCatRotateMainPartExecutable;
    public bool IsCatRotateMainPartExecutable { get { return _isCatRotateMainPartExecutable; } set { _isCatRotateMainPartExecutable = value; } }

    //isRotating
    [SerializeField] private bool _isRotating;
    public bool IsRotating { get { return _isRotating; } set { _isRotating = value; } }


    //isIniRotation
    [SerializeField] private bool _isIniRotation;
    public bool IsIniRotation { get { return _isIniRotation; } set { _isIniRotation = value; } }

    public float outIniRotationStartTime;

    //rotationNum
    public int rotationRestNum;

    [SerializeField] private bool _isRotateEnabled;
    public bool IsRotateEnabled { get { return _isRotateEnabled; } set { _isRotateEnabled = value; } }

    #endregion

    #region CatState
    [Header("- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -" +
        "  \nCATSTATE\n --- ")]
    //afflicted
    [SerializeField] private bool _isAfflicted;
    public bool IsAfflicted
    {
        get
        {
            IsAfflicted =
                IsInHotState ||
                IsInColdState ||
                IsInElectricState ||
                IsInToxicState;

            return _isAfflicted;
        }
        set
        {
            if (value == true) IsInNormalColor = false;

            _isAfflicted = value;
        }
    }

    //temperature
    //public float catCurTemperature;
    [SerializeField] private float _catCurTemperature;
    public float catCurTemperature { get { return _catCurTemperature; } set { _catCurTemperature = value; } }

    [SerializeField] private bool _isInHotState;
    public bool IsInHotState
    {
        get { return _isInHotState; }
        set
        {
            _isInHotState = value;
        }
    }

    [SerializeField] private bool _isInColdState;
    public bool IsInColdState
    {
        get { return _isInColdState; }
        set
        {
            _isInColdState = value;
        }
    }


    //electricity
    //public float catCurElectricity;
    [SerializeField] private float _catCurElectricity;
    public float catCurElectricity { get { return _catCurElectricity; } set { _catCurElectricity = value; } }

    [SerializeField] private bool _isInElectricState;
    public bool IsInElectricState
    {
        get { return _isInElectricState; }
        set
        {
            _isInElectricState = value;
        }
    }


    //toxicity
    //public float catCurToxicity;
    [SerializeField] private float _catCurToxicity;
    public float catCurToxicity { get { return _catCurToxicity; } set { _catCurToxicity = value; } }

    [SerializeField] private bool _isInToxicState;
    public bool IsInToxicState
    {
        get { return _isInToxicState; }
        set
        {
            _isInToxicState = value;
        }
    }

    #endregion

    #region CatEnergy
    [Header("- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -" +
        "  \nCATENERGY\n --- ")]
    //executability
    [SerializeField] private bool _isCatEnergyResetExecutable;
    public bool IsCatEnergyResetExecutable { get { return _isCatEnergyResetExecutable; } set { _isCatEnergyResetExecutable = value; } }

    //curEnergy
    //public float curEnergy;
    [SerializeField] private float _curEnergy;
    public float curEnergy { get { return _curEnergy; } set { _curEnergy = value; } }

    //curTargetEnergy
    //public float curTargetEnergy;
    [SerializeField] private float _curTargetEnergy;
    public float curTargetEnergy { get { return _curTargetEnergy; } set { _curTargetEnergy = value; } }

    #endregion

    #region CatTrigger
    [Header("- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -" +
        "  \nCATTRIGGER\n --- ")]
    //executability
    [SerializeField] private bool _isCatTriggerMainPartExecutable;
    public bool IsCatTriggerMainPartExecutable { get { return _isCatTriggerMainPartExecutable; } set { _isCatTriggerMainPartExecutable = value; } }

    //strawberry
    [SerializeField] private bool _isCarryingStrawberries;
    public bool IsCarryingStrawberries { get { return _isCarryingStrawberries; } set { _isCarryingStrawberries = value; } }

    [SerializeField] private bool _isCollectingStrawberries;
    public bool IsCollectingStrawberries { get { return _isCollectingStrawberries; } set { _isCollectingStrawberries = value; } }

    [SerializeField] private bool _isGettingAStrawberry;
    public bool IsGettingAStrawberry { get { return _isGettingAStrawberry; } set { _isGettingAStrawberry = value; } }

    [SerializeField] private bool _isToLoseCarriedStrawberries;
    public bool IsToLoseCarriedStrawberries { get { return _isToLoseCarriedStrawberries; } set { _isToLoseCarriedStrawberries = value; } }


    //energyCrystal
    [SerializeField] private bool _isGettingAnEnergyCrystal;
    public bool IsGettingAnEnergyCrystal { get { return _isGettingAnEnergyCrystal; } set { _isGettingAnEnergyCrystal = value; } }


    //edgeGate
    [SerializeField] private bool _isEnteringAnEdgeGate;
    public bool IsEnteringAnEdgeGate { get { return _isEnteringAnEdgeGate; } set { _isEnteringAnEdgeGate = value; } }

    [SerializeField] private bool _isEdgeGateTriggered;
    public bool IsEdgeGateTriggered { get { return _isEdgeGateTriggered; } set { _isEdgeGateTriggered = value; } }


    //savePoint
    [SerializeField] private bool _isToActivateASavePoint;
    public bool IsToActivateASavePoint { get { return _isToActivateASavePoint; } set { _isToActivateASavePoint = value; } }

    [SerializeField] private bool _isToDetermineCurActivatedSavePointPosition;
    public bool IsToDetermineCurActivatedSavePointPosition
    { get { return _isToDetermineCurActivatedSavePointPosition; } set { _isToDetermineCurActivatedSavePointPosition = value; } }

    [SerializeField] private bool _isToActivateCurSavePoint;
    public bool IsToActivateCurSavePoint { get { return _isToActivateCurSavePoint; } set { _isToActivateCurSavePoint = value; } }

    //public GameObject curActivatedSavePoint;//~?
    public int curActivatedSavePointIndex;
    public Vector3 curActivatedSavePointPosition;

    #endregion

    #region CatAppearance
    [Header("- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -" +
        "  \nCATAPPEARANCE\n --- ")]
    //contraction
    [SerializeField] private bool _isContracting;
    public bool IsContracting { get { return _isContracting; } set { _isContracting = value; } }

    //color
    [SerializeField] private bool _isInNormalColor;
    public bool IsInNormalColor
    {
        get
        {
            IsInNormalColor =
                !IsAfflicted &&
                !IsInFadedColor;

            return _isInNormalColor;
        }
        set { _isInNormalColor = value; }
    }

    [SerializeField] private bool _isInFadedColor;
    public bool IsInFadedColor
    {
        get { return _isInFadedColor; }
        set
        {
            if (value == true) IsInNormalColor = false;

            _isInFadedColor = value;
        }
    }

    #endregion

    #region CatDeath
    [Header("- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -" +
        "  \nCATDEATH\n --- ")]
    //toDie
    [SerializeField] private bool _isToDie;
    public bool IsToDie { get { return _isToDie; } set { _isToDie = value; } }

    //catIniPosition
    public Vector3 catIniPosition;
    #endregion

    #region BlocksManager
    [Header("- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -" +
        "  \nBLOCKSMANAGER\n --- ")]
    //executability
    [SerializeField] private bool _isBlocksManagerBlocksMoveExecutable;
    public bool IsBlocksManagerBlocksMoveExecutable { get { return _isBlocksManagerBlocksMoveExecutable; } set { _isBlocksManagerBlocksMoveExecutable = value; } }

    //fixedDeltaTime
    public float blocksManagerFixedDeltaTime;

    public List<int> curRoomBlockStateOfMatterIndexes;
    public List<int> curRoomBlockTypeIndexes;

    //gravity
    //~?
    public float curGravity;

    //~?
    //int[] curGoableBlockTypeIndexes;

    //fluidContinuosnessOptimization
    [SerializeField] private bool _isFluidContinuousnessOptimizationActivated = true;
    public bool IsFluidContinuousnessOptimizationActivated 
    { get { return _isFluidContinuousnessOptimizationActivated; } set { _isFluidContinuousnessOptimizationActivated = value; } }

    //fragileBlocks
    public List<GameObject> curToBeBrokenFragileRustBlocks = new List<GameObject>();
    public List<float> curFragileRustBlockToBeBrokenStartTimes = new List<float>();
    public List<GameObject> curBrokenFragileRustBlocks = new List<GameObject>();
    public List<float> curFragileRustBlockBrokenTimes = new List<float>();

    //railBlocks
    public List<GameObject> curRailBlocks = new List<GameObject>();
    public List<Vector3> curRailBlockInitialPositions = new List<Vector3>();
    #endregion

    #region MiniMapManager
    [Header("- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -" +
        "  \nMINIMAPMANAGER\n --- ")]

    public GameObject curMiniMapRotationCameraPoint;
    public GameObject curToMiniMapRotationCameraPoint;

    [SerializeField] private bool _isMiniMapRotationCameraPositionIndexNotInitialized;
    public bool IsMiniMapRotationCameraPointIndexNotInitialized
    { get { return _isMiniMapRotationCameraPositionIndexNotInitialized; } set { _isMiniMapRotationCameraPositionIndexNotInitialized = value; } }

    public int curMiniMapRotationCameraPointIndex;
    public int curToMiniMapRotationCameraPointIndex;
    #endregion

    #region OptionsManager
    [Header("- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -" +
        "  \nOPTIONSMANAGER\n --- ")]
    //executability
    [SerializeField] private bool _isOptionsManagerActivationExecutable;
    public bool IsOptionsManagerActivationExecutable { get { return _isOptionsManagerActivationExecutable; } set { _isOptionsManagerActivationExecutable = value; } }

    //curKeyCodes
    public List<KeyCode> curKeyCodes = new List<KeyCode>();

    //isOptionPanelActivated
    [SerializeField] private bool _isOptionPanelActivated;
    public bool IsOptionPanelActivated
    { get { return _isOptionPanelActivated; } set { _isOptionPanelActivated = value; } }

    #endregion

    void Start()
    {
    }
}
