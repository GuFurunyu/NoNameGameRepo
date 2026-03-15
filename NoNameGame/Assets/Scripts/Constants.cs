using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder((int)ScriptsExecutionOrder.ExecutionOrder.constants)]
public class Constants : MonoBehaviour
{
    #region Default
    [Header("- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -" +
        "  \nDEFAULT\n --- ")]
    //vector3
    public Vector3 defaultVector = new Vector3(999, 999, 999);
    #endregion

    #region ScriptsExecutionController
    [Header("- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -" +
        "  \nSCRIPTSEXECUTIONCONTROLLER\n --- ")]
    #endregion

    #region DataManager
    [Header("- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -" +
        "  \nDATAMANAGER\n --- ")]
    #endregion

    #region RoomsManager
    [Header("- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -" +
        "  \nROOMSMANAGER\n --- ")]
    //breadth
    public float gridBreadth;
    public int roomCoordBreadth;
    public int miniMapRoomCoordBreadth;

    //inRoomMaxForwardDistance
    public float inRoomMaxForwardDistance;

    //facesInfo
    //1-front, 2-back, 3-left, 4-right, 5-top, 6-bottom
    //1-yellow, 2-purple, 3-orange, 4-blue, 5-green, 6-red
    //gravityDefaultValue: 1
    public GameObject[] faces = new GameObject[6];
    public Vector3[] faceStableForwards = new Vector3[6];
    public Vector3[] faceStableUps = new Vector3[6];
    public Vector3[] faceStableRights = new Vector3[6];
    //public Vector3[] faceOPoints = new Vector3[6];
    public float[] roomGravities = new float[54];

    //roomPlanes
    public GameObject[] roomPlanes = new GameObject[54];

    //~?
    //Vector3 outerGravityVector = new Vector3(1, -1, 1);

    //gates
    public List<GameObject> gates = new List<GameObject>();

    //edgeGates
    public List<GameObject> edgeGates = new List<GameObject>();

    //twist
    public GameObject[] twistingCenters = new GameObject[6];
    public float twistSpeed;
    #endregion

    #region CurRoomManager
    [Header("- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -" +
        "  \nCURROOMMANAGER\n --- ")]
    #endregion

    #region TileData
    [Header("- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -" +
        "  \nTILEDATA\n --- ")]

    #endregion

    #region CameraManager
    [Header("- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -" +
        "  \nCAMERAMANAGER\n --- ")]
    //objectAndTransform
    public Camera cam;
    public Transform camTransform;

    //initialSightMasks
    public GameObject initialSightMasksEmpty;
    public float initialSightMasksSpeed;
    public float initialSightMasksMaxDistance;

    //zoomSpeed
    public float zoomSpeed;

    //zoomInAutoTriggerTime
    public float zoomInAutoTriggerTime;

    //size
    public float camNormalSize;
    public float camZoomedOutMaxSize;
    public float camMiniMapSize;

    //distance
    public float camMiniMapDistanceToCubeCore;
    #endregion

    #region Cat
    [Header("- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -" +
        "  \nCAT\n --- ")]
    //objectAndTransform
    public GameObject cat;
    public Transform catTransform;

    //catInfo
    public float catBreadth;
    #endregion

    #region Control
    [Header("- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -" +
        "  \nCONTROL\n --- ")]
    #endregion

    #region CatCollision
    [Header("- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -" +
        "  \nCATCOLLISION\n --- ")]
    //rayDistance
    public float rayDistance;
    public float longRayDistance;
    public float longLongRayDistance;
    public float longLongLongRayDistance;
    public float shortRayDistance;

    //~?
    [HideInInspector]
    public float positionFixOffset;
    #endregion

    #region CatMove
    [Header("- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -" +
        "  \nCATMOVE\n --- ")]
    //horSpeed
    public float horAcce;
    public float horReverseAcce;
    public float horMaxSpeed;
    public float horStopThres;
    public float horWallJumpBonusSpeed;
    public float horToCeilingMaxSpeed;

    //verSpeed
    public float verIniSpeed;
    public float verAcce;
    public float gravityAcce;
    public float climbSpeed;
    public float verMaxSpeed;

    //jumpPreInput
    public float jumpPreInputThres;

    //jumpPostInput
    public float jumpPostInputTres;

    //wallJump
    public float wallJumpPreInputThres;
    public float wallJumpPostInputThres;

    //dash
    public float dashIniSpeed;
    public float dashTime;
    #endregion

    #region CatRotate
    [Header("- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -" +
        "  \nCATROTATE\n --- ")]
    //rotationNum
    public int rotationMaxNum;
    public float rotationNumRestoreThres;

    //rotationProcess
    public float rotationSpeed;
    public float rotationStep;
    public float rotationEndThres;
    public Vector3 leftRotationVector = Vector3.back;
    public Vector3 rightRotationVector = Vector3.forward;

    //iniRotation
    public float returnIniRotationTime;
    #endregion

    #region CatState
    [Header("- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -" +
        "  \nCATSTATE\n --- ")]
    //temperature
    public float intoHotStateTemperature;
    public float intoColdStateTemperature;
    public float temperatureTransferSpeed;
    public float temperatureSetToZeroThres;

    //electricity
    public float intoElectricStateElectricity;
    public float electricityTransferSpeed;
    public float electricitySetToZeroThres;

    //toxic
    public float intoToxicStateToxicity;
    public float toxicityTransferSpeed;
    public float toxicitySetToZeroThres;
    #endregion

    #region CatEnergy
    [Header("- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -" +
        "  \nCATENERGY\n --- ")]
    //maxEnergy
    public float maxEnergy;

    //energyCost
    public float jumpEnergyCost;
    public float dashEnergyCost;
    public float rotationEnergyCost;

    //energyDecreaseSpeed1
    public float jumpEnergyDecreaseSpeedFixParameter;
    public float attachWallEnergyDecreaseSpeed;
    public float climbEnergyDecreaseSpeed;
    public float attachCeilingEnergyDecreaseSpeed;
    public float inAcceEnergyDecreaseSpeed;

    //energyDecreaseSpeed2
    public float inHotStateEnergyDecreaseSpeed;
    public float inColdStateEnergyDecreaseSpeed;
    public float inElectricStateEnergyDecreaseSpeed;
    public float inToxicStateEnergyDecreaseSpeed;

    //energyRestoreAmount
    public float elasticEnergyRestoreAmount;

    //energyRestoreSpeed
    public float onGroundEnergyRestoreSpeed;
    public float inLiquidEnergyRestoreSpeed;

    //curEnergeChangeToTargetEnergySpeed
    public float curEnergyChangeToTargetEnergySpeed;
    #endregion

    #region CatTrigger
    [Header("- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -" +
        "  \nCATTRIGGER\n --- ")]
    //strawberry
    public float strawberriesDistance;
    public float strawberriesSpeed;
    //public float strawberriesRotationSpeed;
    //public float strawberriesRotationTime;
    public float strawberriesContractionSpeed;
    public float strawberriesContractionMin;

    //energyCrystal
    public float energyCrystalPower;
    public float energyCrystalRespawnTime;

    //edgeGate
    public float throughEdgeGateGapTime;

    //savePoints
    public List<GameObject> savePoints = new List<GameObject>();

    //storedBlocks
    public GameObject storedActivatedSavePointBlockEmpty;
    #endregion

    #region CatAppearance
    [Header("- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -" +
        "  \nCATAPPEARANCE\n --- ")]
    //color
    public Material normalColor;
    public Material fadedColor;

    //energyBar
    public GameObject catEnergyBar;
    public GameObject catEnergyBarMask;

    //energyMask
    public GameObject catEnergyMask;

    //outlines
    public GameObject innerOutlinesEmpty;
    //public GameObject outerOutlinesEmpty;
    //public GameObject outerGrayOutlinesEmpty;

    //eyes
    public GameObject catLeftEye;
    public GameObject catRightEye;
    #endregion

    #region CatDeath
    [Header("- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -" +
        "  \nCATDEATH\n --- ")]
    //storedVoidBlocks
    public GameObject storedVoidBlocksEmpty;
    #endregion

    #region BlocksManager
    [Header("- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -" +
        "  \nBLOCKSMANAGER\n --- ")]
    //fixedDeltaTime
    public float blocksManagerFixedDeltaTime;

    //storedBlocks
    public GameObject storedSandBlocksEmpty;
    public GameObject storedWaterBlocksEmpty;
    public GameObject storedAcidBlocksEmpty;
    public GameObject storedVaporBlocksEmpty;
    public GameObject storedGasBlocksEmpty;
    public GameObject storedElectricMistBlocksEmpty;
    public GameObject storedLightElectricMistBlocksEmpty;

    //fragileBlocks
    public float fragileRustBlockToBeBrokenTime;
    public float fragileRustBlockRespawnTime;
    #endregion

    #region MiniMapManager
    [Header("- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -" +
        "  \nMINIMAPMANAGER\n --- ")]
    //faces
    public GameObject[] miniMapFaces = new GameObject[6];

    //roomPlanes
    public GameObject[] miniMapRoomPlanes = new GameObject[54];

    //twistingCenters
    public GameObject[] miniMapTwistingCenters = new GameObject[6];

    //rotationMovingSpeed
    public float miniMapRotationMovingSpeed;

    //rotationCameraPoints
    public GameObject[] miniMapRotationCameraPoints = new GameObject[26];

    //outVersion
    //public Vector3[] miniMapRotationCameraPointStableUps = new Vector3[26];
    //public Vector3[] miniMapRotationCameraPointStableRights = new Vector3[26];
    //public GameObject[] miniMapRotationCameraUpPoints = new GameObject[26];
    //public GameObject[] miniMapRotationCameraDownPoints = new GameObject[26];
    //public GameObject[] miniMapRotationCameraLeftPoints = new GameObject[26];
    //public GameObject[] miniMapRotationCameraRightPoints = new GameObject[26];
    #endregion

    #region OptionsManager
    [Header("- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -" +
        "  \nOPTIONSMANAGER\n --- ")]
    //optionsPanel
    public GameObject optionsPanel;

    //options
    public GameObject optionsEmpty;
    public List<GameObject> optionEmpties = new List<GameObject>();

    //keySetSub
    public GameObject keySetSubEmpty;
    public List<GameObject> keySetSubEmpties = new List<GameObject>();

    //keyCodes
    public List<KeyCode> keyCodes = new List<KeyCode>();

    //keySprites
    public List<Sprite> keySprites = new List<Sprite>();
    public List<Sprite> keyChosenSprites = new List<Sprite>();
    public enum keyIndexes
    {
        keySpace,
        key0, key1, key2, key3, key4, key5, key6, key7, key8, key9,
        keyA, keyB, keyC, keyD, keyE, keyF, keyG, keyH, keyI, keyJ, keyK, keyL, keyM, keyN, keyO,
        keyP, keyQ, keyR, keyS, keyT, keyU, keyV, keyW, keyX, keyY, keyZ,
        keyUpArrow, keyDownArrow, keyLeftArrow, keyRightArrow
    }
    #endregion

    void Start()
    {
        //H:~presetable?
        //camTransform = cam.transform;
        //catTransform = cat.transform;

        positionFixOffset = gridBreadth + rayDistance - catBreadth / 4;
    }

    void test()
    {

    }
}
