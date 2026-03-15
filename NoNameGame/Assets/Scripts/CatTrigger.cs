using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder((int)ScriptsExecutionOrder.ExecutionOrder.catTrigger)]
public class CatTrigger : MonoBehaviour
{
    Constants CONS;
    Variables VARS;
    UniversalFunctionsLibrary UFL;
    ScriptsExecutionController SEC;

    GameObject gameManager;

    //strawberry
    public List<GameObject> carriedStrawberries = new List<GameObject>();
    public List<Vector3> carriedStrawberriesIniPositions = new List<Vector3>();
    //float strawberriesRotationStartTime;

    //energyCrystal
    public List<GameObject> gotEnergyCrystals = new List<GameObject>();
    public List<float> energyCrystalGotTimes = new List<float>();
    public bool isAllGotEnergyCrystalsRespawned;

    //edgeGate
    public float curNearestEdgeGateDistance;
    public int curNearestEdgeGateIndex;
    public GameObject curToEdgeGate;
    public float throughEdgeGateTime;
    //Vector3 curEdgeGatesBetweenVector;
    //public float edgeGateTransportThres;
    public Vector3 curEdgeGatesCommonLineVector;
    public float curEdgeGatesAngle;

    //savePoint
    GameObject storedSavePointBlock;
    GameObject storedActivatedSavePointBlock;

    GameObject tempGameObject;

    #region ConstantsUsed
    GameObject[] faces = new GameObject[6];

    Transform camTransform;

    Transform catTransform;

    float maxEnergy;

    float strawberriesDistance;
    float strawberriesSpeed;
    float strawberriesContractionMin;
    float strawberriesContractionSpeed;

    float energyCrystalPower;
    float energyCrystalRespawnTime;

    float throughEdgeGateGapTime;

    List<GameObject> edgeGates = new List<GameObject>();

    List<GameObject> savePoints = new List<GameObject>();

    GameObject storedActivatedSavePointBlockEmpty;
    #endregion

    #region VariablesUsed
    Vector3[] roomStableForwards;

    GameObject curPlaneEmpty;

    Vector3 curRoomStableForward;

    List<int> edgeGateLinkedToIndexes = new List<int>();

    Vector3 curRight;
    Vector3 curUp;

    GameObject curTriggerTile;
    TileData curTriggerTileData;
    #endregion

    #region BoolVariablesUsed
    bool isOnGround;
    bool isInLiquid;
    #endregion

    void Start()
    {
        gameManager = GameObject.Find("GameManager");

        CONS = gameManager.GetComponent<Constants>();
        VARS = gameManager.GetComponent<Variables>();
        UFL = gameManager.GetComponent<UniversalFunctionsLibrary>();
        SEC = gameManager.GetComponent<ScriptsExecutionController>();

        #region ImportConstants
        faces = CONS.faces;
        camTransform = CONS.camTransform;
        catTransform = CONS.catTransform;
        maxEnergy = CONS.maxEnergy;
        strawberriesDistance = CONS.strawberriesDistance;
        strawberriesSpeed = CONS.strawberriesSpeed;
        strawberriesContractionMin = CONS.strawberriesContractionMin;
        strawberriesContractionSpeed = CONS.strawberriesContractionSpeed;
        energyCrystalPower = CONS.energyCrystalPower;
        energyCrystalRespawnTime = CONS.energyCrystalRespawnTime;
        throughEdgeGateGapTime = CONS.throughEdgeGateGapTime;
        edgeGates = CONS.edgeGates;
        savePoints = CONS.savePoints;
        storedActivatedSavePointBlockEmpty = CONS.storedActivatedSavePointBlockEmpty;
        #endregion

        #region ImportReferenceVariables
        roomStableForwards = VARS.roomStableForwards;
        edgeGateLinkedToIndexes = VARS.edgeGateLinkedToIndexes;
        #endregion

        //loadStoredBlocks
        storedActivatedSavePointBlock = storedActivatedSavePointBlockEmpty.transform.GetChild(0).gameObject;
    }

    void Update()
    {
        #region ImportValueVariables
        curPlaneEmpty = VARS.curPlaneEmpty;
        curRoomStableForward = VARS.curRoomStableForward;
        curRight = VARS.curRight;
        curUp = VARS.curUp;
        curTriggerTile = VARS.curTriggerTile;
        curTriggerTileData = VARS.curTriggerTileData;
        #endregion

        #region ImportBoolVariables
        isOnGround = VARS.IsOnGround;
        isInLiquid = VARS.IsInLiquid;
        #endregion

        #region Strawberries
        if (!VARS.IsRotating && 
            !VARS.IsTwisting)
        {
            //lose
            if (VARS.IsToLoseCarriedStrawberries)
            {
                VARS.IsCarryingStrawberries = false;

                for (int i = 0; i < carriedStrawberries.Count; i++)
                {
                    carriedStrawberries[i].transform.position = carriedStrawberriesIniPositions[i];
                }

                carriedStrawberries.Clear();
                carriedStrawberriesIniPositions.Clear();
            }

            //get
            if (VARS.IsGettingAStrawberry)
            {
                VARS.IsCarryingStrawberries = true;

                carriedStrawberries.Add(curTriggerTile);
                carriedStrawberriesIniPositions.Add(curTriggerTile.transform.position);

                VARS.IsGettingAStrawberry = false;
            }

            //carry
            if (VARS.IsCarryingStrawberries)
            {
                for (int i = 0; i < carriedStrawberries.Count; i++)
                {
                    if (Vector3.Distance(catTransform.position, carriedStrawberries[i].transform.position) > strawberriesDistance)
                    {
                        carriedStrawberries[i].transform.position = Vector3.MoveTowards(carriedStrawberries[i].transform.position, catTransform.position, strawberriesSpeed * Time.deltaTime);
                    }
                }
            }
            //collect
            else if (VARS.IsCollectingStrawberries)
            {
                if (carriedStrawberries.Count > 0)
                {
                    if (carriedStrawberries[0].transform.localScale.magnitude > strawberriesContractionMin)
                    {
                        for (int i = 0; i < carriedStrawberries.Count; i++)
                        {
                            carriedStrawberries[i].transform.localScale -= Vector3.one * strawberriesContractionSpeed * Time.deltaTime;
                            carriedStrawberries[i].transform.position = Vector3.MoveTowards(carriedStrawberries[i].transform.position, catTransform.position, strawberriesSpeed / 6 * Time.deltaTime);
                        }
                    }
                    else
                    {
                        for (int i = 0; i < carriedStrawberries.Count; i++)
                        {
                            Destroy(carriedStrawberries[i]);
                        }

                        carriedStrawberries.Clear();
                        carriedStrawberriesIniPositions.Clear();

                        VARS.IsCollectingStrawberries = false;
                    }
                }
                else
                {
                    VARS.IsCollectingStrawberries = false;
                }
            }
        }
        #endregion

        #region EnergyCrystals
        if (!VARS.IsRotating && 
            !VARS.IsTwisting)
        {
            if (VARS.IsGettingAnEnergyCrystal)
            {
                gotEnergyCrystals.Add(curTriggerTile);
                energyCrystalGotTimes.Add(Time.time);

                curTriggerTile.transform.localScale = Vector3.one * 0.2f;

                VARS.curEnergy += energyCrystalPower;
                if (VARS.curEnergy > maxEnergy)
                {
                    VARS.curEnergy = maxEnergy;
                }

                VARS.IsGettingAnEnergyCrystal = false;
            }

            if (gotEnergyCrystals.Count > 0)
            {
                isAllGotEnergyCrystalsRespawned = true;

                for (int i = 0; i < gotEnergyCrystals.Count; i++)
                {
                    if (gotEnergyCrystals[i].transform.localScale == Vector3.one * 0.2f)
                    {
                        isAllGotEnergyCrystalsRespawned = false;

                        if (Time.time - energyCrystalGotTimes[i] > energyCrystalRespawnTime)
                        {
                            gotEnergyCrystals[i].transform.localScale = Vector3.one;
                        }
                    }
                }

                if (isAllGotEnergyCrystalsRespawned)
                {
                    gotEnergyCrystals.Clear();
                    energyCrystalGotTimes.Clear();
                }
            }
        }
        #endregion

        #region EdgeGate
        if (VARS.IsEnteringAnEdgeGate)
        {
            //ifIsGapTimeOver
            if (Time.time - throughEdgeGateTime > throughEdgeGateGapTime)
            {
                ////findCurNearestEdgeGate
                //curNearestEdgeGateDistance = 999;

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

                //curToEdgeGate = edgeGates[curNearestEdgeGateIndex];
                for(int i=0;i< edgeGates.Count; i++)
                {
                    if (curTriggerTile == edgeGates[i])
                    {
                        curToEdgeGate = edgeGates[edgeGateLinkedToIndexes[i]];
                    }
                }
                //curEdgeGatesBetweenVector = curToEdgeGate.transform.position - curTile.transform.position;

                //toNewRoom
                if (VARS.IsEdgeGateTriggered)
                {
                    catTransform.position = curToEdgeGate.transform.position - roomStableForwards[curToEdgeGate.GetComponent<TileData>().inRoomIndex] * 0.1f;

                    curEdgeGatesCommonLineVector = Vector3.Cross(roomStableForwards[curTriggerTileData.inRoomIndex], roomStableForwards[curToEdgeGate.GetComponent<TileData>().inRoomIndex]);
                    curEdgeGatesAngle = Vector3.Angle(roomStableForwards[curTriggerTileData.inRoomIndex], roomStableForwards[curToEdgeGate.GetComponent<TileData>().inRoomIndex]);

                    camTransform.eulerAngles += curEdgeGatesCommonLineVector * curEdgeGatesAngle;
                    //camIniEulerangles = curEdgeGatesCommonLineVector * curEdgeGatesAngle;

                    if (Vector3.Dot(curUp, curEdgeGatesCommonLineVector) == 0)
                    {
                        curUp = Vector3.Cross(curEdgeGatesCommonLineVector, curUp);
                    }
                    if (Vector3.Dot(curRight, curEdgeGatesCommonLineVector) == 0)
                    {
                        curRight = Vector3.Cross(curEdgeGatesCommonLineVector, curRight);
                    }

                    VARS.IsEdgeGateTriggered = false;

                    throughEdgeGateTime = Time.time;
                }
            }
        }
        #endregion

        #region SavePoint
        if (VARS.IsToActivateASavePoint)
        {
			//deactivateTheLastSavePoint
			//if (VARS.curActivatedSavePoint != null)
			//{
			//    VARS.curActivatedSavePoint.SetActive(true);
			//}

			savePoints[VARS.curActivatedSavePointIndex].SetActive(true);

			//determineCurActivatedSavePoint
			for (int i = 0; i < savePoints.Count; i++)
			{
				if (savePoints[i] == curTriggerTile)
				{
					VARS.curActivatedSavePointIndex = i;

					break;
				}
			}

			VARS.IsToDetermineCurActivatedSavePointPosition = true;

            VARS.IsToActivateCurSavePoint = true;

            VARS.IsToActivateASavePoint = false;
        }

        if (VARS.IsToDetermineCurActivatedSavePointPosition)
		{
			VARS.curActivatedSavePointPosition = CONS.savePoints[VARS.curActivatedSavePointIndex].transform.position;

			//Debug.Log(VARS.curActivatedSavePointPosition);

			VARS.IsToWriteCatWorldData = true;

            VARS.IsToDetermineCurActivatedSavePointPosition = false;
        }

        if (VARS.IsToActivateCurSavePoint)
        {
            //activateCurSavePoint
            //storedActivatedSavePointBlock.transform.position = VARS.curActivatedSavePoint.transform.position;
            storedActivatedSavePointBlock.transform.position = savePoints[VARS.curActivatedSavePointIndex].transform.position;

            //tempChildToCurPlaneEmpty
            storedActivatedSavePointBlock.transform.SetParent(VARS.curPlaneEmpty.transform, true);

            //VARS.curActivatedSavePoint.SetActive(false);
            savePoints[VARS.curActivatedSavePointIndex].SetActive(false);

            //setCatIniPosition
            //VARS.catIniPosition = VARS.curActivatedSavePoint.transform.position - curRoomStableForward * 0.1f;
            VARS.catIniPosition = VARS.curActivatedSavePointPosition - curRoomStableForward * 0.1f;

            //Debug.Log("catIniPosition:" + VARS.catIniPosition);

            //setCatPosition
            if (VARS.horCurSpeed == 0 &&
                VARS.verCurSpeed == 0)
                catTransform.position = VARS.catIniPosition;

            //Debug.Log("catPosition:" + catTransform.position);

            VARS.IsToActivateCurSavePoint = false;
        }
        #endregion

        #region OnGroundOrInLiquidReset
        if (!VARS.IsRotating && 
            !VARS.IsTwisting)
        {
            if (VARS.IsOnGround ||
                VARS.IsInLiquid)
            {
                if (VARS.IsIniRotation)
                {
                    //strawberries
                    if (VARS.IsCarryingStrawberries)
                    {
                        VARS.IsCollectingStrawberries = true;

                        VARS.IsCarryingStrawberries = false;
                    }
                }
            }
        }
        #endregion
    }
}
