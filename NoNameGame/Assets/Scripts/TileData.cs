using UnityEngine;

[DefaultExecutionOrder((int)ScriptsExecutionOrder.ExecutionOrder.tileData)]
public class TileData : MonoBehaviour
{
    Constants CONS;
    Variables VARS;
    UniversalFunctionsLibrary UFL;
    ScriptsExecutionController SEC;

    GameObject gameManager;

    public int inRoomIndex;

    Transform thisTransform;

    int curRoomIndex;

    public bool isNotToBeInCurBlocks;

    public int fragmentIndex;

    //coordinates(~?)
    Vector3 curPosition;
    Vector3 curCoordinates;
    public float offsetThres = 0.2f;

    ////0-not,
    ////1-strawberry, 2-energyCrystal,
    ////3-gate, 4-edgeGate, 5-edgeGateTrigger,
    ////6-savePoint, 7-activatedSavePoint,
    ////8-centerBlock
    ////public int triggerTypeIndex;
    //[SerializeField] private int _triggerTypeIndex;
    //public int triggerTypeIndex { get { return _triggerTypeIndex; } set { _triggerTypeIndex = value; } }


    //0-trigger, 1-solid, 2-liquid, 3-gas, 4-mist
    //public int stateOfMatterIndex;
    [SerializeField] private int _stateOfMatterIndex;
    public int stateOfMatterIndex { get { return _stateOfMatterIndex; } set { _stateOfMatterIndex = value; } }

    public bool isMovable;

    public Vector3 iniLocalPosition;

    public int continuousHorMovingTimes;

    public bool isPositionDetermined;

    public bool isAffectedByGravity;

    public bool isPlatform;

    public bool isNotClimbable;

    public bool isFragile;

    //default: 0
    public int railBlockIndex;

    //default: 0
    public int railBlockMoveStringStartIndex;

    public bool isLoop;

    //0-not...
    public Vector3 sharpDirVector;

    //default: 60(~?)
    public float toughness = 60;

    //default: 0
    public float elasticity;

    //default: 0(~?)
    public float tackiness = 0;

    //default: 1
    public float friction = 1;

    //default: 1(~?)
    public float fluidDrag = 1;

    //default: 1(~?)
    public float mass = 1;

    //default: 20(celsius)
    public float temperature = 20;

    //default: 0
    public float electricity;

    //default: 0
    public float toxicity;

    //blockTypeIndex:
    //way1:
    //  1-NormalBlocks
    //      100-(White)Block, 110-RedBlock, 120-YellowBlock, 130-BlueBlock, 140-OrangeBlock, 150-GreenBlock, 160-PurpleBlock
    //  2-SpecialSolidBlocks
    //      2010-VoidBlock(~Spike), 2011-UpSpikeBlock, 2012-DownSpikeBlock, 2013-LeftSpikeBlock, 2014-RightSpikeBlock
    //      2020-IceBlock(~?), 2021-BreakableIceBlock, 2030-HoneyBlock, 2040-ElasticBlock, 2050-SandBlock, 2060-PlatFormBlock, 2070-ElectricMistCenterBlock
    //  3-LiquidBlocks
    //      310-WaterBlock, 320-AcidBlock
    //  4-GasBlocks
    //      410-VaporBlock, 420-GasBlock
    //  5-MistBlocks(~?)(~ElectricMist)
    //      510-ElectricMistBlock
    //  6-TriggerBlocks
    //      610-GateBlock, 620-EdgeGateBlock, 630-EdgeGateTriggerBlock,
    //      640-StrawberryBlock, 650-EnergyCrystalBlock,
    //      660-SavePointBlock, 670-ActivatedSavePointBlock,
    //      680-CenterBlock
    //way2(x-x-xx(region-stateOfMatterIndex-mainType)):
    //  1-RedRegionBlocks
    //      1001-RedFragmentBlock
    //      1101-RedBlock, 1102-DarkRedBlock, 1103-CoalBlock
    //      1301-VaporBlock
    //  2-YellowRegionBlocks
    //      2001-YellowFragmentBlock
    //      2101-YellowBlock, 2102-DarkYellow, 2103-SandBlock
    //  3-BlueRegionBlocks
    //      3001-BlueFragmentBlock
    //      3101-BlueBlock, 3102-DarkBlueBlock, 3103-IceBlock, 3104-BreakableIceBlock
    //      3201-WaterBlock
    //  4-OrangeRegionBlocks
    //      4001-OrangeFragmentBlock
    //      4101-OrangeBlock, 4102-DarkOrangeBlock, 4103-FragileRustBlock, 4104-RailRustBlock
    //  5-GreenRegionBlocks
    //      5001-GreenFragmentBlock
    //      5101-GreenBlock, 5102-DarkGreenBlock, 5103-ElasticBlock
    //      5201-AcidBlock
    //      5301-GasBlock
    //  6-PurpleRegionBlocks
    //      6001-PurpleFragmentBlock
    //      6101-PurpleBlock, 6102-DarkPurpleBlock, 6103-ElectricMistCenterBlock
    //      6401-ElectricMistBlock, 6402-LightElectricMistBlock
    //  7-UniversalBlocks
    //      7001-GateBlock, 7002-EdgeGateBlock, 7003-EdgeGateTriggerBlock, 7004-SavePointBlock, 7005-ActivatedSavePointBlock, 7006-CenterBlock, 7007-KeyBlock
    //      7008-StrawberryBlock, 7009-EnergyCrystalBlock,
    //      7010-VoidBlock, 7011-EnergyFragmentBlock
    //      7101-(White)Block, 7102-Dark(White)Block, 7103-PlatformBlock, 7104-LockedBlock
    //public int blockTypeIndex;
    [SerializeField] int _blockTypeIndex;
    public int blockTypeIndex { get { return _blockTypeIndex; } set { _blockTypeIndex = value; } }

    #region ConstantsUsed

    #endregion

    #region VariablesUsed

    #endregion

    //private void Awake()
    //{
    //    if (isMovable)
    //        iniLocalPosition = this.transform.localPosition;
    //}

    private void Start()
    {
        gameManager = GameObject.Find("GameManager");

        CONS = gameManager.GetComponent<Constants>();
        VARS = gameManager.GetComponent<Variables>();
        UFL = gameManager.GetComponent<UniversalFunctionsLibrary>();
        SEC = gameManager.GetComponent<ScriptsExecutionController>();

        thisTransform = this.transform;

        #region ImportConstants
        #endregion

        #region ImportReferenceVariables
        #endregion

        for (int i = 0; i < VARS.roomCenters.Length; i++)
        {
            if (UFL.IsInRoom(i, thisTransform.position))
            {
                inRoomIndex = i;

                break;
            }
        }
    }
}