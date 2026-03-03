using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder((int)ScriptsExecutionOrder.ExecutionOrder.blocksManager)]
public class BlocksManager : MonoBehaviour
{
    Constants CONS;
    Variables VARS;
    ScriptsExecutionController SEC;

    GameObject gameManager;

    //storedBlock
    GameObject[] storedWaterBlocks = new GameObject[512];
    GameObject[] storedAcidBlocks = new GameObject[512];
    GameObject[] storedVaporBlocks = new GameObject[512];
    GameObject[] storedGasBlocks = new GameObject[512];
    GameObject[] storedElectricMistBlocks = new GameObject[512];
    GameObject[] storedLightElectricMistBlocks = new GameObject[512];
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
    public List<GameObject> curBlocks = new List<GameObject>();
    List<TileData> curBlockTileDatas = new List<TileData>();
    //public List<int> curBlockStateOfMatterIndexes = new List<int>();
    GameObject curBlock;
    TileData curBlockTileData;
    //int curBlockStateOfMatterIndex;
    //public int curBlockIndex;

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

    bool tempBool;
    int tempInt;
    float tempFloat;
    Vector3 tempVector;
    Transform tempTransform;
    GameObject tempGameObject;
    TileData tempTileData;
    //BlockInfoInMatrix tempBlockInfoInMatrix;

    #region ConstantsUsed
    int roomCoordBreadth;

    GameObject storedWaterBlocksEmpty;
    GameObject storedAcidBlocksEmpty;
    GameObject storedVaporBlocksEmpty;
    GameObject storedGasBlocksEmpty;
    GameObject storedElectricMistBlocksEmpty;
    GameObject storedLightElectricMistBlocksEmpty;

    float blocksManagerFixedDeltaTime;
    #endregion

    #region VariablesUsed
    //Vector3 planeUp;
    //Vector3 planeRight;
    Vector3 curRoomStableUp;
    Vector3 curRoomStableRight;
    Vector3 curUp;
    Vector3 curRight;
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
        SEC = gameManager.GetComponent<ScriptsExecutionController>();

        roomCoordBreadth = CONS.roomCoordBreadth;
        storedWaterBlocksEmpty = CONS.storedWaterBlocksEmpty;
        storedAcidBlocksEmpty = CONS.storedAcidBlocksEmpty;
        storedVaporBlocksEmpty = CONS.storedVaporBlocksEmpty;
        storedGasBlocksEmpty = CONS.storedGasBlocksEmpty;
        storedElectricMistBlocksEmpty = CONS.storedElectricMistBlocksEmpty;
        storedLightElectricMistBlocksEmpty = CONS.storedLightElectricMistBlocksEmpty;

        blocksManagerFixedDeltaTime = CONS.blocksManagerFixedDeltaTime;

        #region loadStoredBlocks
        for (int i = 0; i < 512; i++)
        {
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

        Time.fixedDeltaTime = blocksManagerFixedDeltaTime;
    }

    //void Update()
    void FixedUpdate()
    {
        //planeUp = VARS.planeUp;
        //planeRight = VARS.planeRight;
        curRoomStableUp = VARS.curRoomStableUp;
        curRoomStableRight = VARS.curRoomStableRight;
        curUp = VARS.curUp;
        curRight = VARS.curRight;

        if (VARS.isInNewRoom)
        {
            VARS.curPlaneEmpty = CONS.roomPlanes[VARS.curRoomIndex].transform.GetChild(0).gameObject;

            curBlocks.Clear();
            curBlockTileDatas.Clear();
            curCoordVectors.Clear();

            ////blockInfoMatrix
            //blockInfoMatrixList.Clear();

            //formListsOfBlocks
            for (int i = 0; i < VARS.curPlaneEmpty.transform.childCount; i++)
            {
                tempTransform = VARS.curPlaneEmpty.transform.GetChild(i);

                if (tempTransform.GetComponent<TileData>() != null &&
                    tempTransform.gameObject.activeSelf)
                {
                    if (tempTransform.GetComponent<TileData>().blockTypeIndex != 630)
                    {
                        curBlocks.Add(tempTransform.gameObject);
                        curBlockTileDatas.Add(tempTransform.GetComponent<TileData>());
                        //curBlockStateOfMatterIndexes.Add(tempTransform.GetComponent<TileData>().stateOfMatterIndex);

                        curCoordVector = tempTransform.localPosition;
                        curCoordVectors.Add(curCoordVector);
                    }
                }
            }

            VARS.isInNewRoomBlocksManagerResetOver = true;
        }

        if (VARS.isInNewRoomAllResetOver)
        {
            ////sortCurBlocks
            //SortCurBlocks();

            curLiquidMaxHeight = 999;
            curGasMinHeight = 999;
            curLiquidMinHeight = 999;
            curGasMaxHeight = 999;

            //getCurFluidMaxAndMinHeight
            for (int i = 0; i < curBlocks.Count; i++)
            {
                curBlock = curBlocks[i];
                curBlockTileData = curBlockTileDatas[i];

                if (curBlockTileData.stateOfMatterIndex == 1)
                {
                    tempFloat = Vector3.Dot(curBlock.transform.localPosition, curUp);

                    if (curLiquidMaxHeight == 999)
                    {
                        curLiquidMaxHeight = tempFloat;
                    }
                    else if (tempFloat > curLiquidMaxHeight)
                    {
                        curLiquidMaxHeight = tempFloat;
                    }

                    if (curLiquidMinHeight == 999)
                    {
                        curLiquidMinHeight = tempFloat;
                    }
                    else if (tempFloat < curLiquidMinHeight)
                    {
                        curLiquidMinHeight = tempFloat;
                    }
                }

                if (curBlockTileData.stateOfMatterIndex == 2)
                {
                    tempFloat = Vector3.Dot(curBlock.transform.localPosition, curUp);

                    if (curGasMinHeight == 999)
                    {
                        curGasMinHeight = tempFloat;
                    }
                    else if (tempFloat < curGasMinHeight)
                    {
                        curGasMinHeight = tempFloat;
                    }

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
                if (curBlockTileData.blockTypeIndex == 2070)
                {
                    curElectricMistCenterBlock = curBlock;
                    curElectricMistCenterBlockCoordVector = curElectricMistCenterBlock.transform.localPosition;
                }
            }

            #region BlocksMove
            for (int i = 0; i < curBlocks.Count; i++)
            {
                curBlock = curBlocks[i];
                curBlockTileData = curBlockTileDatas[i];
                //curBlockStateOfMatterIndex = curBlockStateOfMatterIndexes[i];

                #region Solid
                if (curBlockTileData.stateOfMatterIndex == 0)
                {
                    //affectedByGravitySolid
                    if (curBlockTileData.isAffectedByGravity)
                    {
                        curCoordVector = curCoordVectors[i];

                        curDownBlockTypeIndex = GetNearBlockTypeIndex(2);

                        if (curDownBlockTypeIndex == 0)
                        {
                            CurBlockMove(i, 2, false);
                        }
                    }
                }
                #endregion

                #region Liquid
                else if (curBlockTileData.stateOfMatterIndex == 1)
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
                }
                #endregion

                #region Gas
                else if (curBlockTileData.stateOfMatterIndex == 2)
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
                }
                #endregion

                #region Mist
                else if (curBlockTileData.stateOfMatterIndex == 3)
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
                }
                #endregion
            }
            #endregion

            #region FluidContinuousnessOptimization
            if (VARS.isFluidContinuousnessOptimizationActivated)
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

    void CurBlockMove(int curBlockIndex, int dirIndex, bool isFluid = true)
    {
        if (VARS.isFluidContinuousnessOptimizationActivated &&
            isFluid)
        {
            //curMovedBlockIndexes.Add(curBlockIndex);
            curMovedBlockCoordVectors.Add(curCoordVector);
            //curMovedBlockTypeIndexes.Add(curBlocksMatrix[Mathf.RoundToInt(curCoordVector.x), Mathf.RoundToInt(curCoordVector.y)]);
            curMovedBlockTypeIndexes.Add(curBlockTileDatas[curBlockIndex].blockTypeIndex);
        }

        if (dirIndex == 1)
        {
            curBlock.transform.position += curUp;
            curCoordVectors[curBlockIndex] += curUp;
        }
        else if (dirIndex == 2)
        {
            curBlock.transform.position -= curUp;
            curCoordVectors[curBlockIndex] -= curUp;
        }
        else if (dirIndex == 3)
        {
            curBlock.transform.position -= curRight;
            curCoordVectors[curBlockIndex] -= curRight;
        }
        else if (dirIndex == 4)
        {
            curBlock.transform.position += curRight;
            curCoordVectors[curBlockIndex] += curRight;
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
            case 310:
                tempGameObject = storedWaterBlocks[curStoredWaterBlockIndex++];
                break;
            case 320:
                tempGameObject = storedAcidBlocks[curStoredAcidBlockIndex++];
                break;
            case 410:
                tempGameObject = storedVaporBlocks[curStoredVaporBlockIndex++];
                break;
            case 420:
                tempGameObject = storedGasBlocks[curStoredGasBlockIndex++];
                break;
            case 510:
                tempGameObject=storedElectricMistBlocks[curStoredElectricMistBlockIndex++];
                break;
            case 520:
                tempGameObject = storedLightElectricMistBlocks[curStoredLightElectricMistBlockIndex++];
                break;
        }

        //curRoomPosition = curCoordVector -
        //    new Vector3(Mathf.Abs(curRoomStableUp.x + curRoomStableRight.x), Mathf.Abs(curRoomStableUp.y + curRoomStableRight.y), Mathf.Abs(curRoomStableUp.x + curRoomStableRight.x))
        //    * roomCoordBreadth / 2;
        //tempVector = curRoomPosition + VARS.roomCenters[VARS.curRoomIndex];

        tempVector = curCoordVector + VARS.roomCenters[VARS.curRoomIndex];

        tempGameObject.transform.position = tempVector;

        curSpawnedBlocks.Add(tempGameObject);
    }
}