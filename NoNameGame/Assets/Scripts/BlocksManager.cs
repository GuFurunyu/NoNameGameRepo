using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

[DefaultExecutionOrder((int)ScriptsExecutionOrder.ExecutionOrder.blocksManager)]
public class BlocksManager : MonoBehaviour
{
    Constants CONS;
    Variables VARS;
    UniversalFunctionsLibrary UFL;
    ScriptsExecutionController SEC;

    GameObject gameManager;

    //lastUpdateTime
    float lastUpdateTime;

    //storedBlock
    GameObject[] storedSandBlocks = new GameObject[512];
    GameObject[] storedWaterBlocks = new GameObject[512];
    GameObject[] storedAcidBlocks = new GameObject[512];
    GameObject[] storedVaporBlocks = new GameObject[512];
    GameObject[] storedGasBlocks = new GameObject[512];
    GameObject[] storedElectricMistBlocks = new GameObject[512];
    GameObject[] storedLightElectricMistBlocks = new GameObject[512];
    int curStoredSandBlockIndex;
    int curStoredWaterBlockIndex;
    int curStoredAcidBlockIndex;
    int curStoredVaporBlockIndex;
    int curStoredGasBlockIndex;
    int curStoredElectricMistBlockIndex;
    int curStoredLightElectricMistBlockIndex;

    ////matrix
    ////[x][y][z](~coord)
    //public int[,] curBlocksMatrix;
    //public int[,] curBlockIndexesMatrix;
    int curBlockTypeIndex;
    int curUpBlockTypeIndex;
    int curDownBlockTypeIndex;
    int curLeftBlockTypeIndex;
    int curRightBlockTypeIndex;
    //public int curUpBlockIndex;
    //public int curDownBlockIndex;
    //public int curLeftBlockIndex;
    //public int curRightBlockIndex;

    //curBlock
    //HR: toBeMadeDynamic
    List<TileData> curBlockTileDatas = new List<TileData>();
    //public List<int> curBlockStateOfMatterIndexes = new List<int>();
    GameObject curBlock;
    TileData curBlockTileData;
    //int curBlockStateOfMatterIndex;
    //public int curBlockIndex;
    float curBlockLastUpdateTime;

    ////coordVectorAndPosition
    List<Vector3> curCoordVectors = new List<Vector3>();
    Vector3 curCoordVector;
    Vector3 nearCoordVector;

    //fluidHeight
    public float curLiquidMaxHeight;
    public float curGasMinHeight;
    public float curLiquidMinHeight;
    public float curGasMaxHeight;

    //fluidContinuousnessOptimization
    //public List<int> curMovedBlockIndexes = new List<int>();
    List<Vector3> curMovedBlockCoordVectors = new List<Vector3>();
    List<int> curMovedBlockTypeIndexes = new List<int>();
    List<GameObject> curSpawnedBlocks = new List<GameObject>();
    bool isOccupied;

    //electricMistCenter
    GameObject curElectricMistCenterBlock;
    Vector3 curElectricMistCenterBlockCoordVector;
    float[] curElectricMistBlockDistances = new float[4];
    int curElectricMistBlockGoingDirIndex;

    //isCurMistBlockMoved
    bool isCurMistBlockMoved;

    ////blockInfoInMatrix1
    //struct BlockInfoInMatrix
    //{
    //    int blockIndex;
    //    Vector3 coordVector;
    //    int blockTypeIndex;
    //}
    //BlockInfoInMatrix defaultBlockInfoInMatrix;

    ////blockInfoInMatrix2
    //List<BlockInfoInMatrix> blockInfoMatrixList = new List<BlockInfoInMatrix>();
    //BlockInfoInMatrix curBlockInfoInMatrix;
    //Vector3 curCoordVector;
    //Vector3 nearCoordVector;
    //Vector3 curRoomPosition;

    ////blockInfoInMatrix3
    //BlockInfoInMatrix curUpBlockInfoInMatrix;
    //BlockInfoInMatrix curDownBlockInfoInMatrix;
    //BlockInfoInMatrix curLeftBlockInfoInMatrix;
    //BlockInfoInMatrix curRightBlockInfoInMatrix;

    //gates
    float curNearestGateDistance;
    float curGateNearestLockDistance;

    //edgeGates
    float curNearestEdgeGateDistance;
    int curNearestEdgeGateIndex;
    float curEdgeGateNearestLockDistance;

    //railBlocks
    int curRailBlockMoveStringIndex;
    int curCarryCatRailBlockIndex = -1;

    bool tempBool;
    int tempInt;
    float tempFloat;
    char tempChar;
    string tempString;
    Vector3 tempVector;
    Vector3 tempVector1;
    Vector3 tempVector2;
    Transform tempTransform;
    GameObject tempGameObject;
    TileData tempTileData;
    //BlockInfoInMatrix tempBlockInfoInMatrix;

    #region ConstantsUsed
    float gridBreadth;
    int roomCoordBreadth;

    List<GameObject> gates = new List<GameObject>();

    List<GameObject> edgeGates = new List<GameObject>();

    List<GameObject> locks = new List<GameObject>();

    Transform catTransform;

    GameObject storedSandBlocksEmpty;
    GameObject storedWaterBlocksEmpty;
    GameObject storedAcidBlocksEmpty;
    GameObject storedVaporBlocksEmpty;
    GameObject storedGasBlocksEmpty;
    GameObject storedElectricMistBlocksEmpty;
    GameObject storedLightElectricMistBlocksEmpty;

    float liquidFixedUpdateTime;
    float gasFixedUpdateTime;
    float mistFixedUpdateTime;
    float sandFixedUpdateTime;
    float railBlockFixedUpdateTime;

    float unlockDistance;

    float fragileRustBlockToBeBrokenTime;
    float fragileRustBlockRespawnTime;

    List<string> railBlockMoveStrings = new List<string>();
    #endregion

    #region VariablesUsed
    List<int> edgeGateLinkedToIndexes = new List<int>();

    Vector3 curRoomStableUp;
    Vector3 curRoomStableRight;
    Vector3 curUp;
    Vector3 curRight;

    List<GameObject> curBlocks = new List<GameObject>();

    List<float> curBlockLastUpdateTimes = new List<float>();

    List<int> curRoomBlockStateOfMatterIndexes = new List<int>();
    List<int> curRoomBlockTypeIndexes = new List<int>();

    List<GameObject> curToBeBrokenFragileRustBlocks = new List<GameObject>();
    List<float> curFragileRustBlockToBeBrokenStartTimes = new List<float>();
    List<GameObject> curBrokenFragileRustBlocks = new List<GameObject>();
    List<float> curFragileRustBlockBrokenTimes = new List<float>();

    List<GameObject> curRailBlocks = new List<GameObject>();
    List<Vector3> curRailBlockInitialPositions = new List<Vector3>();
    #endregion

    //toDoLater:
    //multipleTypesOfLiquid
    //multiplePartsOfLiquid
    //roomTransition

    void Start()
    {
        gameManager = GameObject.Find("GameManager");

        CONS = gameManager.GetComponent<Constants>();
        VARS = gameManager.GetComponent<Variables>();
        UFL = gameManager.GetComponent<UniversalFunctionsLibrary>();
        SEC = gameManager.GetComponent<ScriptsExecutionController>();

        #region ImportConstants
        gridBreadth = CONS.gridBreadth;
        roomCoordBreadth = CONS.roomCoordBreadth;
        gates = CONS.gates;
        edgeGates = CONS.edgeGates;
        locks = CONS.locks;
        catTransform = CONS.catTransform;
        storedSandBlocksEmpty = CONS.storedSandBlocksEmpty;
        storedWaterBlocksEmpty = CONS.storedWaterBlocksEmpty;
        storedAcidBlocksEmpty = CONS.storedAcidBlocksEmpty;
        storedVaporBlocksEmpty = CONS.storedVaporBlocksEmpty;
        storedGasBlocksEmpty = CONS.storedGasBlocksEmpty;
        storedElectricMistBlocksEmpty = CONS.storedElectricMistBlocksEmpty;
        storedLightElectricMistBlocksEmpty = CONS.storedLightElectricMistBlocksEmpty;
        liquidFixedUpdateTime = CONS.liquidFixedUpdateTime;
        gasFixedUpdateTime = CONS.gasFixedUpdateTime;
        mistFixedUpdateTime = CONS.mistFixedUpdateTime;
        sandFixedUpdateTime = CONS.sandFixedUpdateTime;
        railBlockFixedUpdateTime = CONS.railBlockFixedUpdateTime;
        unlockDistance = CONS.unlockDistance;
        fragileRustBlockToBeBrokenTime = CONS.fragileRustBlockToBeBrokenTime;
        fragileRustBlockRespawnTime = CONS.fragileRustBlockRespawnTime;
        railBlockMoveStrings = CONS.railBlockMoveStrings;
        #endregion

        #region ImportReferenceVariables
        edgeGateLinkedToIndexes = VARS.edgeGateLinkedToIndexes;
        curBlocks = VARS.curBlocks;
        curBlockLastUpdateTimes = VARS.curBlockLastUpdateTimes;
        curRoomBlockStateOfMatterIndexes = VARS.curRoomBlockStateOfMatterIndexes;
        curRoomBlockTypeIndexes = VARS.curRoomBlockTypeIndexes;
        curToBeBrokenFragileRustBlocks = VARS.curToBeBrokenFragileRustBlocks;
        curFragileRustBlockToBeBrokenStartTimes = VARS.curFragileRustBlockToBeBrokenStartTimes;
        curBrokenFragileRustBlocks = VARS.curBrokenFragileRustBlocks;
        curFragileRustBlockBrokenTimes = VARS.curFragileRustBlockBrokenTimes;
        curRailBlocks = VARS.curRailBlocks;
        curRailBlockInitialPositions = VARS.curRailBlockInitialPositions;
        #endregion

        #region loadStoredBlocks
        for (int i = 0; i < 512; i++)
        {
            storedSandBlocks[i] = storedSandBlocksEmpty.transform.GetChild(i).gameObject;
            storedWaterBlocks[i] = storedWaterBlocksEmpty.transform.GetChild(i).gameObject;
            storedAcidBlocks[i] = storedAcidBlocksEmpty.transform.GetChild(i).gameObject;
            storedVaporBlocks[i] = storedVaporBlocksEmpty.transform.GetChild(i).gameObject;
            storedGasBlocks[i] = storedGasBlocksEmpty.transform.GetChild(i).gameObject;
            storedElectricMistBlocks[i] = storedElectricMistBlocksEmpty.transform.GetChild(i).gameObject;
            storedLightElectricMistBlocks[i] = storedLightElectricMistBlocksEmpty.transform.GetChild(i).gameObject;
        }
        #endregion

        //curBlocksMatrix = new int[roomCoordBreadth + 1, roomCoordBreadth + 1];
        //curBlockIndexesMatrix = new int[roomCoordBreadth + 1, roomCoordBreadth + 1];

        //defaultBlockInfoInMatrix.blockIndex = 0;
        //defaultBlockInfoInMatrix.coordVector = Vector3.zero;
        //defaultBlockInfoInMatrix.blockTypeIndex = 0;

        //for (int i = 0; i < roomCoordBreadth; i++)
        //{
        //    for (int j = 0; j < roomCoordBreadth; j++)
        //    {
        //        print(/*"(" + i + ", " + j + "): " + */curBlocksMatrix[i, j]);
        //    }
        //}

        lastUpdateTime = Time.time;
    }

    void Update()
    //void FixedUpdate()
    {
        #region ImportValueVariables
        curRoomStableUp = VARS.curRoomStableUp;
        curRoomStableRight = VARS.curRoomStableRight;
        curUp = VARS.curUp;
        curRight = VARS.curRight;
        #endregion

        ////customFixedUpdateInUpdate
        //if (Time.time - lastUpdateTime < (VARS.blocksManagerFixedDeltaTime * 0.9f))
        //{
        //    return;
        //}
        //else
        //{
        //    lastUpdateTime = Time.time;
        //}

        #region InNewRoomReset
        if (!VARS.IsInNewRoomBlocksManagerResetOver)
        {
            if (VARS.curPlaneEmpty == null)
                VARS.curPlaneEmpty = CONS.roomPlanes[VARS.curRoomIndex].transform.GetChild(0).gameObject;

            curBlocks.Clear();
            curBlockTileDatas.Clear();
            curCoordVectors.Clear();

            curBlockLastUpdateTimes.Clear();

            curRoomBlockStateOfMatterIndexes.Clear();
            curRoomBlockTypeIndexes.Clear();

            //railBlocks
            for (int i = 0; i < curRailBlocks.Count; i++)
            {
                curRailBlocks[i].transform.position = curRailBlockInitialPositions[i];
            }
            curRailBlocks.Clear();
            curRailBlockInitialPositions.Clear();
            curRailBlockMoveStringIndex = -1;

            ////blockInfoMatrix
            //blockInfoMatrixList.Clear();

            //formListsOfBlocks
            for (int i = 0; i < VARS.curPlaneEmpty.transform.childCount; i++)
            {
                tempTransform = VARS.curPlaneEmpty.transform.GetChild(i);

                if (tempTransform.GetComponent<TileData>() != null &&
                    tempTransform.gameObject.activeSelf)
                {
                    //notCountEdgeGateTriggers
                    if (tempTransform.GetComponent<TileData>().blockTypeIndex != 7003)
                    {
                        tempTileData = tempTransform.GetComponent<TileData>();

                        curBlocks.Add(tempTransform.gameObject);
                        curBlockTileDatas.Add(tempTileData);
                        //curBlockStateOfMatterIndexes.Add(tempTransform.GetComponent<TileData>().stateOfMatterIndex);

                        curBlockLastUpdateTimes.Add(Time.time);
                        VARS.railBlocksLastUpdateTime = Time.time;

                        //curCoordVector = tempTransform.localPosition;
                        curCoordVector = tempTransform.position - VARS.curPlaneEmpty.transform.position;
                        curCoordVectors.Add(curCoordVector);

                        //getCurRoomBlockStateOfMatterIndexesAndTypeIndexes
                        if (!curRoomBlockStateOfMatterIndexes.Contains(tempTileData.stateOfMatterIndex))
                        {
                            curRoomBlockStateOfMatterIndexes.Add(tempTileData.stateOfMatterIndex);
                        }
                        if (!curRoomBlockTypeIndexes.Contains(tempTileData.blockTypeIndex))
                        {
                            curRoomBlockTypeIndexes.Add(tempTileData.blockTypeIndex);
                        }

                        //getRailBlocks
                        if (tempTileData.railBlockIndex > 0)
                        {
                            curRailBlocks.Add(tempTransform.gameObject);
                            curRailBlockInitialPositions.Add(tempTransform.position);
                        }
                    }
                }
            }

            //shaffleCurBlocks
            ShaffleCurBlocks();

            ////deactivateOutlineSquaresHidenInSurroundingBlocks
            //DeactivateOutlineSquaresHidenInSurroundingBlocks();

            //lockNotConnectedGates
            for (int i = 0; i < gates.Count; i++)
            {
                if (gates[i].transform.parent != VARS.curPlaneEmpty.transform)
                    continue;

                tempTransform = gates[i].transform;

                //findCurNearestGate
                curNearestGateDistance = 999;
                for (int j = 0; j < gates.Count; j++)
                {
                    if (gates[j].transform.parent != tempTransform.parent)
                    {
                        if (Vector3.Distance(gates[j].transform.position, tempTransform.position) < curNearestGateDistance)
                        {
                            curNearestGateDistance = Vector3.Distance(gates[j].transform.position, tempTransform.position);
                        }
                    }
                }

                //linkConnectedGates
                if (curNearestGateDistance < 6 * gridBreadth)
                {
                    //Debug.Log("enter1");
                    //tempTransform.GetComponent<TileData>().triggerTypeIndex = 3;
                    //toTrigger
                    tempTransform.GetComponent<TileData>().stateOfMatterIndex = 0;
                    for (int k = 0; k < tempTransform.childCount; k++)
                    {
                        tempTransform.GetChild(k).gameObject.SetActive(false);
                    }
                }
                //lockNotConnectedGates
                else
                {
                    //Debug.Log("enter2");
                    //tempTransform.GetComponent<TileData>().triggerTypeIndex = 0;
                    //toSolid
                    tempTransform.GetComponent<TileData>().stateOfMatterIndex = 1;
                    for (int k = 0; k < tempTransform.childCount; k++)
                    {
                        tempTransform.GetChild(k).gameObject.SetActive(true);
                    }
                }

                //findCurNearestLock
                curGateNearestLockDistance = 999;
                for (int j = 0; j < locks.Count; j++)
                {
                    if (locks[j].transform.parent != tempTransform.parent &&
                        locks[j].activeSelf)
                    {
                        if (Vector3.Distance(locks[j].transform.position, tempTransform.position) < curGateNearestLockDistance)
                        {
                            curGateNearestLockDistance = Vector3.Distance(locks[j].transform.position, tempTransform.position);
                        }
                    }
                }

                //lockNotConnectedGates
                if (curGateNearestLockDistance < 6 * gridBreadth)
                {
                    //toSolid
                    tempTransform.GetComponent<TileData>().stateOfMatterIndex = 1;
                    for (int k = 0; k < tempTransform.childCount; k++)
                    {
                        tempTransform.GetChild(k).gameObject.SetActive(true);
                    }
                }
            }


            //initializeEdgeGateLinkedToIndexes
            edgeGateLinkedToIndexes.Clear();
            for (int i = 0; i < edgeGates.Count; i++)
            {
                edgeGateLinkedToIndexes.Add(-1);
            }

            //determineEdgeGatePassabilities
            for (int i = 0; i < edgeGates.Count; i++)
            {
                if (edgeGates[i].transform.parent != VARS.curPlaneEmpty.transform)
                    continue;

                tempTransform = edgeGates[i].transform;

                //for (int i = 0; i < edgeGates.Count; i++)
                //{
                //    if (edgeGates[i].transform.parent != curTriggerTile.transform.parent)
                //    {
                //        if (Vector3.Distance(edgeGates[i].transform.position, curTriggerTile.transform.position) < curNearestEdgeGateDistance)
                //        {
                //            curNearestEdgeGateDistance = Vector3.Distance(edgeGates[i].transform.position, curTriggerTile.transform.position);
                //            curNearestEdgeGateIndex = i;
                //        }
                //    }
                //}

                //findCurNearestEdgeGate
                curNearestEdgeGateDistance = 999;
                for (int j = 0; j < edgeGates.Count; j++)
                {
                    if (edgeGates[j].transform.parent != tempTransform.parent)
                    {
                        if (Vector3.Distance(edgeGates[j].transform.position, tempTransform.position) < curNearestEdgeGateDistance)
                        {
                            curNearestEdgeGateDistance = Vector3.Distance(edgeGates[j].transform.position, tempTransform.position);
                            curNearestEdgeGateIndex = j;
                        }
                    }
                }

                //linkConnectedEdgeGates
                if (curNearestEdgeGateDistance < 6 * gridBreadth)
                {
                    //tempTransform.GetComponent<TileData>().triggerTypeIndex = 4;
                    //toTrigger
                    tempTransform.GetComponent<TileData>().stateOfMatterIndex = 0;
                    edgeGateLinkedToIndexes[i] = curNearestEdgeGateIndex;
                    for (int k = 0; k < tempTransform.childCount; k++)
                    {
                        tempTransform.GetChild(k).gameObject.SetActive(false);
                    }
                }
                //lockNotConnectedEdgeGates
                else
                {
                    //Debug.Log("enter3");
                    //tempTransform.GetComponent<TileData>().triggerTypeIndex = 0;
                    //toSolid
                    tempTransform.GetComponent<TileData>().stateOfMatterIndex = 1;
                    edgeGateLinkedToIndexes[i] = -1;
                    for (int k = 0; k < tempTransform.childCount; k++)
                    {
                        tempTransform.GetChild(k).gameObject.SetActive(true);
                    }
                }

                //findCurNearestLock
                curEdgeGateNearestLockDistance = 999;
                for (int j = 0; j < locks.Count; j++)
                {
                    if (locks[j].transform.parent != tempTransform.parent &&
                        locks[j].activeSelf)
                    {
                        if (Vector3.Distance(locks[j].transform.position, tempTransform.position) < curEdgeGateNearestLockDistance)
                        {
                            curEdgeGateNearestLockDistance = Vector3.Distance(locks[j].transform.position, tempTransform.position);
                        }
                    }
                }

                //lockNotConnectedGates
                if (curEdgeGateNearestLockDistance < 6 * gridBreadth)
                {
                    //Debug.Log("enter4");
                    //toSolid
                    tempTransform.GetComponent<TileData>().stateOfMatterIndex = 1;
                    for (int k = 0; k < tempTransform.childCount; k++)
                    {
                        tempTransform.GetChild(k).gameObject.SetActive(true);
                    }
                }
            }

            VARS.IsInNewRoomBlocksManagerResetOver = true;
        }
        #endregion

        if (VARS.IsBlocksManagerMainPartExecutable)
        {
            ////sortCurBlocks
            //SortCurBlocks();

            #region getCurFluidMaxAndMinHeight
            curLiquidMaxHeight = 999;
            curGasMinHeight = 999;
            curLiquidMinHeight = 999;
            curGasMaxHeight = 999;

            for (int i = 0; i < curBlocks.Count; i++)
            {
                curBlock = curBlocks[i];
                curBlockTileData = curBlockTileDatas[i];

                //liquidHeight
                if (curBlockTileData.stateOfMatterIndex == 2)
                {
                    tempFloat = Vector3.Dot(curBlock.transform.localPosition, curUp);

                    //curLiquidMaxHeight
                    if (curLiquidMaxHeight == 999)
                    {
                        curLiquidMaxHeight = tempFloat;
                    }
                    else if (tempFloat > curLiquidMaxHeight)
                    {
                        curLiquidMaxHeight = tempFloat;
                    }

                    //curLiquidMinHeight
                    if (curLiquidMinHeight == 999)
                    {
                        curLiquidMinHeight = tempFloat;
                    }
                    else if (tempFloat < curLiquidMinHeight)
                    {
                        curLiquidMinHeight = tempFloat;
                    }
                }

                //gasHeight
                if (curBlockTileData.stateOfMatterIndex == 3)
                {
                    tempFloat = Vector3.Dot(curBlock.transform.localPosition, curUp);

                    //curGasMinHeight
                    if (curGasMinHeight == 999)
                    {
                        curGasMinHeight = tempFloat;
                    }
                    else if (tempFloat < curGasMinHeight)
                    {
                        curGasMinHeight = tempFloat;
                    }

                    //curGasMaxHeight
                    if (curGasMaxHeight == 999)
                    {
                        curGasMaxHeight = tempFloat;
                    }
                    else if (tempFloat > curGasMaxHeight)
                    {
                        curGasMaxHeight = tempFloat;
                    }
                }

                //getCurElectricMistCenterBlock
                if (curBlockTileData.blockTypeIndex == 6103)
                {
                    curElectricMistCenterBlock = curBlock;
                    curElectricMistCenterBlockCoordVector = curElectricMistCenterBlock.transform.localPosition;
                }
            }
            #endregion

            #region BlocksTraverse
            //railBlock
            if (Time.time - VARS.railBlocksLastUpdateTime > railBlockFixedUpdateTime)
            {
                curRailBlockMoveStringIndex++;
                VARS.IsCatBeingMovedByRailBlock = VARS.IsCatMovedByRailBlock;
                VARS.IsCatMovedByRailBlock = false;

                VARS.railBlocksLastUpdateTime = Time.time;
            }

            for (int i = 0; i < curBlocks.Count; i++)
            {                
                curBlock = curBlocks[i];
                curBlockTileData = curBlockTileDatas[i];
                //curBlockStateOfMatterIndex = curBlockStateOfMatterIndexes[i];
                curBlockLastUpdateTime = curBlockLastUpdateTimes[i];

                #region BlocksMove
                #region Solid
                if (curBlockTileData.stateOfMatterIndex == 1)
                {
                    #region Sand
                    //sand(affectedByGravitySolid(~?))
                    if (curBlockTileData.isAffectedByGravity)
                    {
                        if (Time.time - curBlockLastUpdateTime > sandFixedUpdateTime)
                        {
                            curCoordVector = curCoordVectors[i];

                            curDownBlockTypeIndex = GetNearBlockTypeIndex(2);

                            if (curDownBlockTypeIndex == 0)
                            {
                                CurBlockMove(i, 2, true);
                            }

                            curBlockLastUpdateTimes[i] = Time.time;
                        }
                    }
                    #endregion

                    #region RailBlock
                    if (curBlockTileData.railBlockIndex > 0)
                    {
                        if (Time.time - curBlockLastUpdateTime > railBlockFixedUpdateTime)
                        {
                            tempString = railBlockMoveStrings[curBlockTileData.railBlockIndex - 1];
                            tempInt = curRailBlockMoveStringIndex + curBlockTileData.railBlockMoveStringStartIndex;

                            //forwardMove
                            if (tempInt % tempString.Length != 0 ||
                                tempInt == 0)
                            {
                                tempInt = tempInt % tempString.Length;
                                tempChar = tempString[tempInt];

                                switch (tempChar)
                                {
                                    case 'u':
                                        tempVector = VARS.roomStableUps[VARS.curRoomIndex];
                                        tempVector1 = VARS.roomStableRights[VARS.curRoomIndex];
                                        tempInt = 1;
                                        break;
                                    case 'd':
                                        tempVector = -VARS.roomStableUps[VARS.curRoomIndex];
                                        tempVector1 = VARS.roomStableRights[VARS.curRoomIndex];
                                        tempInt = 2;
                                        break;
                                    case 'l':
                                        tempVector = -VARS.roomStableRights[VARS.curRoomIndex];
                                        tempVector1 = VARS.roomStableUps[VARS.curRoomIndex];
                                        tempInt = 3;
                                        break;
                                    case 'r':
                                        tempVector = VARS.roomStableRights[VARS.curRoomIndex];
                                        tempVector1 = VARS.roomStableUps[VARS.curRoomIndex];
                                        tempInt = 4;
                                        break;
                                }

                                //if (i == curCarryCatRailBlockIndex)
                                //{
                                //    UFL.AddCatPosition(tempVector);

                                //    VARS.IsCatMovedByRailBlock = true;

                                //    if (Vector3.Dot(tempVector, curRight) == 0)
                                //    {
                                //        VARS.verCurSpeed = 0;
                                //    }
                                //    else if (Vector3.Dot(tempVector, curUp) == 0)
                                //    {
                                //        VARS.horCurSpeed = 0;
                                //    }
                                //}

                                //moveCat
                                tempVector2 = catTransform.position - curBlock.transform.position;
                                tempFloat = Vector3.Dot(tempVector2, tempVector);
                                if (!VARS.IsCatMovedByRailBlock &&
                                    ((VARS.IsOnOrToARailBlock &&
                                    curBlock == VARS.curOnOrToRailBlock) ||
                                    (Mathf.Abs(Vector3.Dot(tempVector2, tempVector1)) < gridBreadth &&
                                    tempFloat < gridBreadth * 1.5 &&
                                    tempFloat > gridBreadth * -0.2f)))
                                {
                                    if (Mathf.Abs(Vector3.Dot(tempVector2, tempVector1)) < gridBreadth &&
                                    tempFloat < gridBreadth * 1.5 &&
                                    tempFloat > gridBreadth * -0.2f)
                                    {
                                        //Debug.Log("enter2");
                                        //Debug.Log("tempFloat:" + tempFloat);
                                        //Debug.Log("move:" + tempVector * (2 - tempFloat));

                                        //Debug.Log("catPosition1:" + catTransform.position);
                                        UFL.AddCatPosition(tempVector * (2 - tempFloat + 0.001f));
                                        //Debug.Log("catPosition2:" + catTransform.position);
                                    }
                                    else if (VARS.IsOnOrToARailBlock &&
                                        curBlock == VARS.curOnOrToRailBlock)
                                    {
                                        UFL.AddCatPosition(tempVector);
                                    }
                                    VARS.curCatMovedByRailBlockVector = tempVector;

                                    VARS.IsCatMovedByRailBlock = true;

                                    curCarryCatRailBlockIndex = i;

                                    //if (Vector3.Dot(tempVector, curRight) == 0)
                                    //{
                                    //    VARS.verCurSpeed = 0;
                                    //}
                                    //else if (Vector3.Dot(tempVector, curUp) == 0)
                                    //{
                                    //    VARS.horCurSpeed = 0;
                                    //}

                                    CurBlockMove(i, tempInt, false, true);

                                    //avoidCatIntoWall
                                    for (int j = 0; j < curBlocks.Count; j++)
                                    {
                                        //notRailBlock
                                        if (curBlockTileDatas[j].railBlockIndex == 0)
                                        {
                                            tempFloat = Mathf.Abs(Vector3.Dot(catTransform.position - curBlocks[j].transform.position, tempVector));
                                            if (tempFloat < gridBreadth)
                                            {
                                                UFL.AddCatPosition(-tempVector * (gridBreadth - tempFloat));

                                                curCarryCatRailBlockIndex = -1;

                                                break;
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    CurBlockMove(i, tempInt, false, true);
                                }
                            }
                            //back
                            else
                            {
                                if (!curBlockTileData.isLoop)
                                {
                                    VARS.curOnOrToRailBlock = null;
                                    VARS.IsOnOrToARailBlock = false;

                                    tempInt = tempString.Length;

                                    tempChar = tempString[tempInt - 1];

                                    //catOffset
                                    if (i == curCarryCatRailBlockIndex)
                                    {
                                        switch (tempChar)
                                        {
                                            case 'u':
                                                tempVector = VARS.roomStableUps[VARS.curRoomIndex];
                                                break;
                                            case 'd':
                                                tempVector = -VARS.roomStableUps[VARS.curRoomIndex];
                                                break;
                                            case 'l':
                                                tempVector = -VARS.roomStableRights[VARS.curRoomIndex];
                                                break;
                                            case 'r':
                                                tempVector = VARS.roomStableRights[VARS.curRoomIndex];
                                                break;
                                        }
                                        UFL.AddCatPosition(tempVector * 0.02f);

                                        curCarryCatRailBlockIndex = -1;
                                    }

                                    while (tempInt > 0)
                                    {
                                        tempInt--;

                                        tempChar = tempString[tempInt];

                                        switch (tempChar)
                                        {
                                            case 'u':
                                                CurBlockMove(i, 2, false, true);
                                                tempVector = VARS.roomStableUps[VARS.curRoomIndex];
                                                break;
                                            case 'd':
                                                CurBlockMove(i, 1, false, true);
                                                tempVector = -VARS.roomStableUps[VARS.curRoomIndex];
                                                break;
                                            case 'l':
                                                CurBlockMove(i, 4, false, true);
                                                tempVector = -VARS.roomStableRights[VARS.curRoomIndex];
                                                break;
                                            case 'r':
                                                CurBlockMove(i, 3, false, true);
                                                tempVector = VARS.roomStableRights[VARS.curRoomIndex];
                                                break;
                                        }
                                    }
                                }
                            }

                            //makeHiddenRailBlocksVisible
                            if (curBlock.GetComponent<MeshRenderer>().enabled == false)
                            {
                                curBlock.GetComponent<MeshRenderer>().enabled = true;
                                curBlock.transform.localScale = Vector3.one;
                            }

                            //hideRailBlocksOutOfRoomBoundary
                            tempVector = curBlock.transform.position - VARS.roomCenters[VARS.curRoomIndex];
                            if (Mathf.Abs(Vector3.Dot(tempVector, curUp)) > (roomCoordBreadth / 2) * gridBreadth ||
                                Mathf.Abs(Vector3.Dot(tempVector, curRight)) > (roomCoordBreadth / 2) * gridBreadth)
                            {
                                curBlock.GetComponent<MeshRenderer>().enabled = false;
                                curBlock.transform.localScale = Vector3.one * 0.01f;
                            }

                            curBlockLastUpdateTimes[i] = Time.time;
                        }
                    }
                    #endregion
                }
                #endregion

                #region Liquid
                else if (curBlockTileData.stateOfMatterIndex == 2)
                {
                    if (Time.time - curBlockLastUpdateTime > liquidFixedUpdateTime)
                    {
                        curCoordVector = curCoordVectors[i];

                        curUpBlockTypeIndex = GetNearBlockTypeIndex(1);
                        curDownBlockTypeIndex = GetNearBlockTypeIndex(2);
                        curLeftBlockTypeIndex = GetNearBlockTypeIndex(3);
                        curRightBlockTypeIndex = GetNearBlockTypeIndex(4);

                        if (curDownBlockTypeIndex != 0)
                        {
                            if (curUpBlockTypeIndex != 0 || Vector3.Dot(curBlock.transform.localPosition, curUp) >= curLiquidMaxHeight - 1 ||
                                /*(Vector3.Dot(curBlock.transform.localPosition, curUp) - curLiquidMaxHeight) * (curUp.x + curUp.y + curUp.z) <= 0 ||*/
                                curLeftBlockTypeIndex == 0 || curRightBlockTypeIndex == 0 /*&&
                            Vector3.Dot(curBlock.transform.localPosition, curUp) != curLiquidMaxHeight - 2 &&
                            Vector3.Dot(curBlock.transform.localPosition, curUp) != curLiquidMaxHeight - 1*/)
                            {
                                CurFluidBlockMoveLeftOrRight(i);
                            }
                            else
                            {
                                tempFloat = Random.value;

                                //moveUp
                                if (tempFloat < (curLiquidMaxHeight - Vector3.Dot(curBlock.transform.localPosition, curUp)) / (curLiquidMaxHeight - curLiquidMinHeight))
                                {
                                    //Debug.Log(tempFloat + " " + (curLiquidMaxHeight - Vector3.Dot(curBlock.transform.localPosition, curUp)) / (curLiquidMaxHeight - curLiquidMinHeight));

                                    CurBlockMove(i, 1);
                                }
                                else
                                {
                                    CurFluidBlockMoveLeftOrRight(i);
                                }
                            }
                        }
                        //moveDown
                        else
                        {
                            CurBlockMove(i, 2);
                        }

                        curBlockLastUpdateTimes[i] = Time.time;
                    }
                }
                #endregion

                #region Gas
                else if (curBlockTileData.stateOfMatterIndex == 3)
                {
                    if (Time.time - curBlockLastUpdateTime > gasFixedUpdateTime)
                    {
                        curCoordVector = curCoordVectors[i];

                        curUpBlockTypeIndex = GetNearBlockTypeIndex(1);
                        curDownBlockTypeIndex = GetNearBlockTypeIndex(2);
                        curLeftBlockTypeIndex = GetNearBlockTypeIndex(3);
                        curRightBlockTypeIndex = GetNearBlockTypeIndex(4);

                        if (curUpBlockTypeIndex != 0)
                        {
                            if (curDownBlockTypeIndex != 0 || Vector3.Dot(curBlock.transform.localPosition, curUp) <= curGasMinHeight + 1 ||
                                /*(Vector3.Dot(curBlock.transform.localPosition, curUp) - curGasMinHeight) * (curUp.x + curUp.y + curUp.z) <= 0 ||*/
                                curLeftBlockTypeIndex == 0 || curRightBlockTypeIndex == 0 /*&&
                            Vector3.Dot(curBlock.transform.localPosition, curUp) != curGasMinHeight + 2 &&
                            Vector3.Dot(curBlock.transform.localPosition, curUp) != curGasMinHeight + 1*/)
                            {
                                CurFluidBlockMoveLeftOrRight(i);
                            }
                            else
                            {
                                tempFloat = Random.value;

                                //moveDown
                                if (tempFloat < (Vector3.Dot(curBlock.transform.localPosition, curUp) - curGasMinHeight) / (curGasMaxHeight - curGasMinHeight))
                                {
                                    CurBlockMove(i, 2);
                                }
                                else
                                {
                                    CurFluidBlockMoveLeftOrRight(i);
                                }
                            }
                        }
                        //moveUp
                        else
                        {
                            CurBlockMove(i, 1);
                        }

                        curBlockLastUpdateTimes[i] = Time.time;
                    }
                }
                #endregion

                #region Mist
                else if (curBlockTileData.stateOfMatterIndex == 4)
                {
                    if (Time.time - curBlockLastUpdateTime > mistFixedUpdateTime)
                    {
                        curCoordVector = curCoordVectors[i];

                        curUpBlockTypeIndex = GetNearBlockTypeIndex(1);
                        curDownBlockTypeIndex = GetNearBlockTypeIndex(2);
                        curLeftBlockTypeIndex = GetNearBlockTypeIndex(3);
                        curRightBlockTypeIndex = GetNearBlockTypeIndex(4);

                        //verticalOrHorizontal
                        //upOrDown, leftOrRight

                        //if(curUpBlockTypeIndex==0 ||
                        //    curDownBlockTypeIndex==0 ||
                        //    curLeftBlockTypeIndex==0 ||
                        //    curRightBlockTypeIndex==0)
                        //{
                        //    isCurMistBlockMoved = false;

                        //    while (!isCurMistBlockMoved)
                        //    {
                        //        tempFloat = Random.value;

                        //        if (tempFloat < 0.25f)
                        //        {
                        //            if (curUpBlockTypeIndex == 0)
                        //            {
                        //                if (Vector3.Dot(curCoordVector - curElectricMistCenterBlockCoordVector, curUp) < 7)
                        //                    CurBlockMove(i, 1);

                        //                isCurMistBlockMoved = true;
                        //            }
                        //        }
                        //        else if (tempFloat >= 0.25f &&
                        //            tempFloat < 0.5f)
                        //        {
                        //            if (curDownBlockTypeIndex == 0)
                        //            {
                        //                if (Vector3.Dot(curCoordVector - curElectricMistCenterBlockCoordVector, curUp) > -7)
                        //                    CurBlockMove(i, 2);

                        //                isCurMistBlockMoved = true;
                        //            }
                        //        }
                        //        else if (tempFloat >= 0.5f &&
                        //            tempFloat < 0.75f)
                        //        {
                        //            if (curLeftBlockTypeIndex == 0)
                        //            {
                        //                if (Vector3.Dot(curCoordVector - curElectricMistCenterBlockCoordVector, curRight) > -7)
                        //                    CurBlockMove(i, 3);

                        //                isCurMistBlockMoved = true;
                        //            }
                        //        }
                        //        else if (tempFloat >= 0.75f)
                        //        {
                        //            if (curRightBlockTypeIndex == 0)
                        //            {
                        //                if (Vector3.Dot(curCoordVector - curElectricMistCenterBlockCoordVector, curRight) < 7)
                        //                    CurBlockMove(i, 4);

                        //                isCurMistBlockMoved = true;
                        //            }
                        //        }
                        //    }
                        //}

                        if (curUpBlockTypeIndex == 0 ||
                            curDownBlockTypeIndex == 0 ||
                            curLeftBlockTypeIndex == 0 ||
                            curRightBlockTypeIndex == 0)
                        {
                            curElectricMistBlockDistances[0] = Vector3.Dot(curCoordVector - curElectricMistCenterBlockCoordVector, -curUp);
                            curElectricMistBlockDistances[1] = Vector3.Dot(curCoordVector - curElectricMistCenterBlockCoordVector, curUp);
                            curElectricMistBlockDistances[2] = Vector3.Dot(curCoordVector - curElectricMistCenterBlockCoordVector, curRight);
                            curElectricMistBlockDistances[3] = Vector3.Dot(curCoordVector - curElectricMistCenterBlockCoordVector, -curRight);

                            isCurMistBlockMoved = false;

                            while (!isCurMistBlockMoved)
                            {
                                tempFloat = Mathf.Max(curElectricMistBlockDistances[0], curElectricMistBlockDistances[1], curElectricMistBlockDistances[2], curElectricMistBlockDistances[3]);

                                //getCurElectricMistBlockGoingIndex
                                if (tempFloat == curElectricMistBlockDistances[0])
                                {
                                    curElectricMistBlockGoingDirIndex = 1;

                                    curElectricMistBlockDistances[0] = -999;
                                }
                                else if (tempFloat == curElectricMistBlockDistances[1])
                                {
                                    curElectricMistBlockGoingDirIndex = 2;

                                    curElectricMistBlockDistances[1] = -999;
                                }
                                else if (tempFloat == curElectricMistBlockDistances[2])
                                {
                                    curElectricMistBlockGoingDirIndex = 3;

                                    curElectricMistBlockDistances[2] = -999;
                                }
                                else if (tempFloat == curElectricMistBlockDistances[3])
                                {
                                    curElectricMistBlockGoingDirIndex = 4;

                                    curElectricMistBlockDistances[3] = -999;
                                }

                                //tryMove
                                if (curElectricMistBlockGoingDirIndex == 1)
                                {
                                    if (curUpBlockTypeIndex == 0)
                                    {
                                        CurBlockMove(i, 1);

                                        isCurMistBlockMoved = true;
                                    }
                                }
                                else if (curElectricMistBlockGoingDirIndex == 2)
                                {
                                    if (curDownBlockTypeIndex == 0)
                                    {
                                        CurBlockMove(i, 2);

                                        isCurMistBlockMoved = true;
                                    }
                                }
                                else if (curElectricMistBlockGoingDirIndex == 3)
                                {
                                    if (curLeftBlockTypeIndex == 0)
                                    {
                                        CurBlockMove(i, 3);

                                        isCurMistBlockMoved = true;
                                    }
                                }
                                else if (curElectricMistBlockGoingDirIndex == 4)
                                {
                                    if (curRightBlockTypeIndex == 0)
                                    {
                                        CurBlockMove(i, 4);

                                        isCurMistBlockMoved = true;
                                    }
                                }
                            }
                        }

                        curBlockLastUpdateTimes[i] = Time.time;
                    }
                }
                #endregion
                #endregion

                #region Locks
                if (curBlockTileData.blockTypeIndex == 7104)
                {
                    if (Vector3.Distance(curBlock.transform.position,catTransform.position) < unlockDistance &&
                        VARS.IsCarryingAKey)
                    {
                        VARS.curUnlockingBlock = curBlock;

                        VARS.IsUnlocking = true;
                    }
                }
                #endregion
            }

            #region FluidContinuousnessOptimization
            if (VARS.IsFluidContinuousnessOptimizationActivated)
            {
                //filterOutTheHolesCausedByTheBlocksMoved
                for (int i = curMovedBlockCoordVectors.Count - 1; i >= 0; i--)
                {
                    curCoordVector = curMovedBlockCoordVectors[i];

                    isOccupied = false;

                    for (int j = 0; j < curCoordVectors.Count; j++)
                    {
                        if (Mathf.RoundToInt(curCoordVectors[j].x) == Mathf.RoundToInt(curCoordVector.x) &&
                            Mathf.RoundToInt(curCoordVectors[j].y) == Mathf.RoundToInt(curCoordVector.y) &&
                            Mathf.RoundToInt(curCoordVectors[j].z) == Mathf.RoundToInt(curCoordVector.z))
                        {
                            isOccupied = true;

                            break;
                        }
                    }

                    if (isOccupied)
                    {
                        //curMovedBlockIndexes.RemoveAt(i);
                        curMovedBlockCoordVectors.RemoveAt(i);
                        curMovedBlockTypeIndexes.RemoveAt(i);
                    }
                }
                //clearTheCurSpawnedBlocks
                for (int i = 0; i < curSpawnedBlocks.Count; i++)
                {
                    //Destroy(curSpawnedBlocks[i]);

                    curSpawnedBlocks[i].transform.position = Vector3.zero;

                    curSpawnedBlocks[i].SetActive(false);

                    curStoredSandBlockIndex = 0;
                    curStoredWaterBlockIndex = 0;
                    curStoredAcidBlockIndex = 0;
                    curStoredVaporBlockIndex = 0;
                    curStoredGasBlockIndex = 0;
                    curStoredElectricMistBlockIndex = 0;
                    curStoredLightElectricMistBlockIndex = 0;
                }
                curSpawnedBlocks.Clear();
                //spawnTheSameBlocksInTheMovedOutPositionsToMakeTheEffectMoreContinuous
                for (int i = 0; i < curMovedBlockCoordVectors.Count; i++)
                {
                    SpawnBlockByTypeIndex(curMovedBlockTypeIndexes[i], curMovedBlockCoordVectors[i]);
                }
                //clearTheListsOfTheCurMovedBlocks
                //curMovedBlockIndexes.Clear();
                curMovedBlockCoordVectors.Clear();
                curMovedBlockTypeIndexes.Clear();
            }
            #endregion

            #endregion

            #region FragileBlocks
            //toBeBrokenToBeBroken
            if (curToBeBrokenFragileRustBlocks.Count > 0)
            {
                for (int i = curToBeBrokenFragileRustBlocks.Count - 1; i >= 0; i--)
                {
                    //break
                    if (Time.time - curFragileRustBlockToBeBrokenStartTimes[i] > fragileRustBlockToBeBrokenTime)
                    {
                        curToBeBrokenFragileRustBlocks[i].SetActive(false);

                        curBrokenFragileRustBlocks.Add(curToBeBrokenFragileRustBlocks[i]);
                        curFragileRustBlockBrokenTimes.Add(Time.time);

                        curToBeBrokenFragileRustBlocks.RemoveAt(i);
                        curFragileRustBlockToBeBrokenStartTimes.RemoveAt(i);

                        UFL.ClearCurCollisionTileDatas();

                        //VARS.IsInNewRoomBlocksManagerResetOver = false;
                    }
                }
            }

            //brokenToRespawn
            if (curBrokenFragileRustBlocks.Count > 0)
            {
                for (int i = curBrokenFragileRustBlocks.Count - 1; i >= 0; i--)
                {
                    if (Time.time - curFragileRustBlockBrokenTimes[i] > fragileRustBlockRespawnTime)
                    {
                        //ifCatInPutOff
                        tempVector = catTransform.position - curBrokenFragileRustBlocks[i].transform.position;
                        if (Mathf.Abs(Vector3.Dot(tempVector, curUp)) < gridBreadth &&
                            Mathf.Abs(Vector3.Dot(tempVector, curRight)) < gridBreadth)
                        {
                            curFragileRustBlockBrokenTimes[i] = Time.time;
                        }
                        else
                        {
                            curBrokenFragileRustBlocks[i].SetActive(true);

                            curBrokenFragileRustBlocks.RemoveAt(i);
                            curFragileRustBlockBrokenTimes.RemoveAt(i);

                            //VARS.IsInNewRoomBlocksManagerResetOver = false;
                        }
                    }
                }
            }
            #endregion
        }
    }

    //fromBottomToTop
    //bubbleSort
    void SortCurBlocks()
    {
        for (int i = 0; i < curBlocks.Count - 1; i++)
        {
            //isAlreadySorted
            tempBool = true;

            for (int j = curBlocks.Count - 1; j > i; j--)
            {
                if (Vector3.Dot(curBlocks[j].transform.localPosition, curUp) < Vector3.Dot(curBlocks[j - 1].transform.localPosition, curUp))
                {
                    //curBlocks
                    tempGameObject = curBlocks[j];
                    curBlocks[j] = curBlocks[j - 1];
                    curBlocks[j - 1] = tempGameObject;
                    //curBlockTileDatas
                    tempTileData = curBlockTileDatas[j];
                    curBlockTileDatas[j] = curBlockTileDatas[j - 1];
                    curBlockTileDatas[j - 1] = tempTileData;
                    //curCoordVectors
                    tempVector = curCoordVectors[j];
                    curCoordVectors[j] = curCoordVectors[j - 1];
                    curCoordVectors[j - 1] = tempVector;

                    ////blockInfoMatrixList
                    //tempBlockInfoInMatrix = blockInfoMatrixList[j];
                    //blockInfoMatrixList[j] = blockInfoMatrixList[j - 1];
                    //blockInfoMatrixList[j - 1] = tempBlockInfoInMatrix;

                    tempBool = false;
                }
            }

            if (tempBool)
            {
                //for (int k = 0; k < blockInfoMatrixList.Count; k++)
                //{
                //    print(blockInfoMatrixList[k].coordVector);
                //}

                break;
            }
        }
    }

    void ShaffleCurBlocks()
    {
        int n = curBlocks.Count;
        for (int i = n - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);

            //curBlocks
            tempGameObject = curBlocks[i];
            curBlocks[i] = curBlocks[j];
            curBlocks[j] = tempGameObject;

            //curBlockTileDatas
            tempTileData = curBlockTileDatas[i];
            curBlockTileDatas[i] = curBlockTileDatas[j];
            curBlockTileDatas[j] = tempTileData;

            //curCoordVectors
            tempVector = curCoordVectors[i];
            curCoordVectors[i] = curCoordVectors[j];
            curCoordVectors[j] = tempVector;

            //curBlockLastUpdateTimes
            tempFloat = curBlockLastUpdateTimes[i];
            curBlockLastUpdateTimes[i] = curBlockLastUpdateTimes[j];
            curBlockLastUpdateTimes[j] = tempFloat;
        }
    }

    //notApplied
    void DeactivateOutlineSquaresHidenInSurroundingBlocks()
    {
        int[] hollowOrMovableTypeIndexes = { 0, 2050, 310, 320, 410, 420, 510, 520 };

        for (int i = 0; i < curBlocks.Count; i++)
        {
            curCoordVector = curCoordVectors[i];

            int upType = GetNearBlockTypeIndex(1);
            int downType = GetNearBlockTypeIndex(2);
            int leftType = GetNearBlockTypeIndex(3);
            int rightType = GetNearBlockTypeIndex(4);

            bool upOk = System.Array.IndexOf(hollowOrMovableTypeIndexes, upType) == -1;
            bool downOk = System.Array.IndexOf(hollowOrMovableTypeIndexes, downType) == -1;
            bool leftOk = System.Array.IndexOf(hollowOrMovableTypeIndexes, leftType) == -1;
            bool rightOk = System.Array.IndexOf(hollowOrMovableTypeIndexes, rightType) == -1;

            if (upOk && downOk && leftOk && rightOk)
            {
                Transform blockTrans = curBlocks[i].transform;
                for (int c = 0; c < blockTrans.childCount; c++)
                {
                    blockTrans.GetChild(c).gameObject.SetActive(false);
                }
            }
        }
    }

    int GetNearBlockTypeIndex(int dirIndex)
    {
        switch (dirIndex)
        {
            case 1:
                nearCoordVector = curCoordVector + curUp;
                break;
            case 2:
                nearCoordVector = curCoordVector - curUp;
                break;
            case 3:
                nearCoordVector = curCoordVector - curRight;
                break;
            case 4:
                nearCoordVector = curCoordVector + curRight;
                break;
        }

        for (int i = 0; i < curCoordVectors.Count; i++)
        {
            if (Mathf.RoundToInt(curCoordVectors[i].x) == Mathf.RoundToInt(nearCoordVector.x) &&
                Mathf.RoundToInt(curCoordVectors[i].y) == Mathf.RoundToInt(nearCoordVector.y) &&
                Mathf.RoundToInt(curCoordVectors[i].z) == Mathf.RoundToInt(nearCoordVector.z))
            {
                //if (dirIndex == 2)
                //    Debug.Log(curBlockTileDatas[i].blockTypeIndex);

                return curBlockTileDatas[i].blockTypeIndex;
            }
        }

        //if(dirIndex==2)
        //    Debug.Log(0);

        return 0;
    }

    void CurBlockMove(int curBlockIndex, int dirIndex, bool isFluid = true, bool isStableDir = false)
    {
        Vector3 upVector;
        Vector3 rightVector;

        if (VARS.IsFluidContinuousnessOptimizationActivated &&
            isFluid)
        {
            //curMovedBlockIndexes.Add(curBlockIndex);
            curMovedBlockCoordVectors.Add(curCoordVector);
            //curMovedBlockTypeIndexes.Add(curBlocksMatrix[Mathf.RoundToInt(curCoordVector.x), Mathf.RoundToInt(curCoordVector.y)]);
            curMovedBlockTypeIndexes.Add(curBlockTileDatas[curBlockIndex].blockTypeIndex);
        }

        //ifIsStableDir
        if (isStableDir)
        {
            upVector = VARS.roomStableUps[VARS.curRoomIndex];
            rightVector = VARS.roomStableRights[VARS.curRoomIndex];
        }
        else
        {
            upVector = curUp;
            rightVector = curRight;
        }

        if (dirIndex == 1)
        {
            curBlock.transform.position += upVector;
            curCoordVectors[curBlockIndex] += upVector;
        }
        else if (dirIndex == 2)
        {
            curBlock.transform.position -= upVector;
            curCoordVectors[curBlockIndex] -= upVector;
        }
        else if (dirIndex == 3)
        {
            curBlock.transform.position -= rightVector;
            curCoordVectors[curBlockIndex] -= rightVector;
        }
        else if (dirIndex == 4)
        {
            curBlock.transform.position += rightVector;
            curCoordVectors[curBlockIndex] += rightVector;
        }
    }

    void CurFluidBlockMoveLeftOrRight(int curBlockIndex)
    {
        if (curLeftBlockTypeIndex == 0 && curRightBlockTypeIndex == 0)
        {
            tempFloat = Random.value;

            //moveLeft
            if (tempFloat < 0.5f)
            {
                if (curLeftBlockTypeIndex == 0)
                {
                    CurBlockMove(curBlockIndex, 3);
                }
            }
            //moveRight
            else
            {
                if (curRightBlockTypeIndex == 0)
                {
                    CurBlockMove(curBlockIndex, 4);
                }
            }
        }
        else
        {
            //moveLeft
            if (curLeftBlockTypeIndex == 0 && curRightBlockTypeIndex != 0)
            {
                CurBlockMove(curBlockIndex, 3);
            }
            //moveRight
            else if (curRightBlockTypeIndex == 0 && curLeftBlockTypeIndex != 0)
            {
                CurBlockMove(curBlockIndex, 4);
            }
        }
    }

    void SpawnBlockByTypeIndex(int blockTypeIndex, Vector3 curCoordVector)
    {
        switch (blockTypeIndex)
        {
            case 2103:
                tempGameObject = storedSandBlocks[curStoredSandBlockIndex++];
                break;
            case 3201:
                tempGameObject = storedWaterBlocks[curStoredWaterBlockIndex++];
                break;
            case 5201:
                tempGameObject = storedAcidBlocks[curStoredAcidBlockIndex++];
                break;
            case 1301:
                tempGameObject = storedVaporBlocks[curStoredVaporBlockIndex++];
                break;
            case 5301:
                tempGameObject = storedGasBlocks[curStoredGasBlockIndex++];
                break;
            case 6401:
                tempGameObject=storedElectricMistBlocks[curStoredElectricMistBlockIndex++];
                break;
            case 6402:
                tempGameObject = storedLightElectricMistBlocks[curStoredLightElectricMistBlockIndex++];
                break;
        }

        //curRoomPosition = curCoordVector -
        //    new Vector3(Mathf.Abs(curRoomStableUp.x + curRoomStableRight.x), Mathf.Abs(curRoomStableUp.y + curRoomStableRight.y), Mathf.Abs(curRoomStableUp.x + curRoomStableRight.x))
        //    * roomCoordBreadth / 2;
        //tempVector = curRoomPosition + VARS.roomCenters[VARS.curRoomIndex];

        tempVector = curCoordVector + VARS.roomCenters[VARS.curRoomIndex];

        tempGameObject.transform.position = tempVector;

        tempGameObject.SetActive(true);

        curSpawnedBlocks.Add(tempGameObject);
    }
}