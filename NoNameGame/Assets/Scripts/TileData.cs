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

    //coordinates(~?)
    Vector3 curPosition;
    Vector3 curCoordinates;
    public float offsetThres = 0.2f;

    //0-not,
    //1-strawberry, 2-energyCrystal,
    //3-gate, 4-edgeGate, 5-edgeGateTrigger,
    //6-savePoint, 7-activatedSavePoint,
    //8-centerBlock
    public int triggerTypeIndex;

    //0-solid, 1-liquid, 2-gas
    public int stateOfMatterIndex;

    public bool isMovable;

    public bool isPositionDetermined;

    public bool isAffectedByGravity;

    public bool isPlatform;

    public bool isNotClimbable;

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
    public float conductivity;

    //default: 0
    public float toxicity;

    Vector3 tempVector;

    //blockTypeIndex:
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
    public int blockTypeIndex;

    #region ConstantsUsed

    #endregion

    #region VariablesUsed

    #endregion

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

    private void Update()
    {
        #region ImportValueVariables

        #endregion
    }
}
