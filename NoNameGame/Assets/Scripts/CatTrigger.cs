using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder((int)ScriptsExecutionOrder.ExecutionOrder.catTrigger)]
public class CatTrigger : MonoBehaviour
{
    Constants CONS;
    Variables VARS;
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

    #region ConstantsUsed
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
    #endregion

    #region VariablesUsed
    GameObject curTriggerTile;
    TileData curTriggerTileData;

    Vector3[] roomStableForwards;

    Vector3 curRight;
    Vector3 curUp;

    bool isOnGround;
    bool isInLiquid;
    #endregion

    void Start()
    {
        gameManager = GameObject.Find("GameManager");

        CONS = gameManager.GetComponent<Constants>();
        VARS = gameManager.GetComponent<Variables>();
        SEC = gameManager.GetComponent<ScriptsExecutionController>();

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
    }

    void Update()
    {
        curTriggerTile = VARS.curTriggerTile;
        curTriggerTileData = VARS.curTriggerTileData;
        roomStableForwards = VARS.roomStableForwards;
        curRight = VARS.curRight;
        curUp = VARS.curUp;
        isOnGround = VARS.isOnGround;
        isInLiquid = VARS.isInLiquid;

        #region Strawberries
        if (!VARS.isRotating && 
            !VARS.isTwisting)
        {
            //lose
            if (VARS.isToLoseCarriedStrawberries)
            {
                VARS.isCarryingStrawberries = false;

                for (int i = 0; i < carriedStrawberries.Count; i++)
                {
                    carriedStrawberries[i].transform.position = carriedStrawberriesIniPositions[i];
                }

                carriedStrawberries.Clear();
                carriedStrawberriesIniPositions.Clear();
            }

            //get
            if (VARS.isGettingAStrawberry)
            {
                VARS.isCarryingStrawberries = true;

                carriedStrawberries.Add(curTriggerTile);
                carriedStrawberriesIniPositions.Add(curTriggerTile.transform.position);

                VARS.isGettingAStrawberry = false;
            }

            //carry
            if (VARS.isCarryingStrawberries)
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
            else if (VARS.isCollectingStrawberries)
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

                        VARS.isCollectingStrawberries = false;
                    }
                }
                else
                {
                    VARS.isCollectingStrawberries = false;
                }
            }
        }
        #endregion

        #region EnergyCrystals
        if (!VARS.isRotating && 
            !VARS.isTwisting)
        {
            if (VARS.isGettingAnEnergyCrystal)
            {
                gotEnergyCrystals.Add(curTriggerTile);
                energyCrystalGotTimes.Add(Time.time);

                curTriggerTile.transform.localScale = Vector3.one * 0.2f;

                VARS.curEnergy += energyCrystalPower;
                if (VARS.curEnergy > maxEnergy)
                {
                    VARS.curEnergy = maxEnergy;
                }

                VARS.isGettingAnEnergyCrystal = false;
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
        if (VARS.isEnteringAnEdgeGate)
        {
            //ifIsGapTimeOver
            if (Time.time - throughEdgeGateTime > throughEdgeGateGapTime)
            {
                //findCurNearestEdgeGate
                curNearestEdgeGateDistance = 999;

                for (int i = 0; i < edgeGates.Count; i++)
                {
                    if (edgeGates[i].transform.parent != curTriggerTile.transform.parent)
                    {
                        if (Vector3.Distance(edgeGates[i].transform.position, curTriggerTile.transform.position) < curNearestEdgeGateDistance)
                        {
                            curNearestEdgeGateDistance = Vector3.Distance(edgeGates[i].transform.position, curTriggerTile.transform.position);
                            curNearestEdgeGateIndex = i;
                        }
                    }
                }

                curToEdgeGate = edgeGates[curNearestEdgeGateIndex];
                //curEdgeGatesBetweenVector = curToEdgeGate.transform.position - curTile.transform.position;

                //toNewRoom
                if (VARS.isEdgeGateTriggered)
                {
                    Debug.Log("enter");

                    print(curToEdgeGate.transform.position);

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

                    VARS.isEdgeGateTriggered = false;

                    throughEdgeGateTime = Time.time;
                }
            }
        }
        #endregion

        #region OnGroundOrInLiquidReset
        if (!VARS.isRotating && 
            !VARS.isTwisting)
        {
            if (VARS.isOnGround ||
                VARS.isInLiquid)
            {
                if (VARS.isIniRotation)
                {
                    //strawberries
                    if (VARS.isCarryingStrawberries)
                    {
                        VARS.isCollectingStrawberries = true;

                        VARS.isCarryingStrawberries = false;
                    }
                }
            }
        }
        #endregion
    }
}
