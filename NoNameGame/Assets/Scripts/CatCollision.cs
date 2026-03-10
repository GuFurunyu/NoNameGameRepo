using UnityEngine;

[DefaultExecutionOrder((int)ScriptsExecutionOrder.ExecutionOrder.catCollision)]
public class CatCollision : MonoBehaviour
{
    Constants CONS;
    Variables VARS;
    UniversalFunctionsLibrary UFL;
    ScriptsExecutionController SEC;

    GameObject gameManager;

    //rayStartPoint
    Vector3 leftTopPoint;
    Vector3 leftBottomPoint;
    Vector3 rightTopPoint;
    Vector3 rightBottomPoint;
    Vector3 topLeftPoint;
    Vector3 topRightPoint;
    Vector3 bottomLeftPoint;
    Vector3 bottomRightPoint;
    //Vector3 liquidCenterPoint;
    //Vector3 gasCenterPoint;
    Vector3 topPoint;
    Vector3 bottomPoint;
    Vector3 leftPoint;
    Vector3 rightPoint;
    //Vector3 topOutPoint;

    //solidRay
    Ray leftRay1;
    Ray leftRay2;
    Ray rightRay1;
    Ray rightRay2;
    Ray upRay1;
    Ray upRay2;
    Ray downRay1;
    Ray downRay2;

    //fluidRay
    //Ray liquidCenterRay;
    //Ray gasCenterRay;
    Ray liquidDownRay;
    Ray liquidUpRay;
    Ray gasUpRay;
    Ray gasDownRay;
    Ray mistUpRay;
    Ray mistDownRay;
    Ray mistLeftRay;
    Ray mistRightRay;

    //solidHit
    RaycastHit leftHit1;
    RaycastHit leftHit2;
    RaycastHit rightHit1;
    RaycastHit rightHit2;
    RaycastHit upHit1;
    RaycastHit upHit2;
    RaycastHit downHit1;
    RaycastHit downHit2;

    //fluidHit
    //RaycastHit liquidCenterHit;
    //RaycastHit gasCenterHit;
    RaycastHit liquidDownHit;
    RaycastHit liquidUpHit;
    RaycastHit gasUpHit;
    RaycastHit gasDownHit;
    RaycastHit mistUpHit;
    RaycastHit mistDownHit;
    RaycastHit mistLeftHit;
    RaycastHit mistRightHit;

    //curTile
    GameObject curTile;
    TileData curTileData;

    //breaking
    float breakingPower;
    //bool isBreaking;

    int tempInt;
    float tempFloat;
    Vector3 tempVector;
    RaycastHit tempHit;

    #region ConstantsUsed
    Transform catTransform;

    float catBreadth;

    float rayDistance;
    float longRayDistance;
    float longLongRayDistance;
    float longLongLongRayDistance;
    float shortRayDistance;

    float positionFixOffset;

    float verMaxSpeed;

    float elasticEnergyRestoreAmount;
    #endregion

    #region VariablesUsed
    Vector3 curRight;
    Vector3 curUp;

    //float horCurSpeed;
    //float verCurSpeed;

    //float curEnergy;
    #endregion

    void Start()
    {
        gameManager = GameObject.Find("GameManager");

        CONS = gameManager.GetComponent<Constants>();
        VARS = gameManager.GetComponent<Variables>();
        UFL = gameManager.GetComponent<UniversalFunctionsLibrary>();
        SEC = gameManager.GetComponent<ScriptsExecutionController>();

        #region ImportConstants
        catTransform = CONS.catTransform;
        catBreadth = CONS.catBreadth;
        rayDistance = CONS.rayDistance;
        longRayDistance = CONS.longRayDistance;
        longLongRayDistance = CONS.longLongRayDistance;
        longLongLongRayDistance = CONS.longLongLongRayDistance;
        shortRayDistance = CONS.shortRayDistance;
        positionFixOffset = CONS.positionFixOffset;
        verMaxSpeed = CONS.verMaxSpeed;
        elasticEnergyRestoreAmount = CONS.elasticEnergyRestoreAmount;
        #endregion

        #region ImportReferenceVariables
        #endregion
    }

    void Update()
    {
        #region ImportValueVariables
        curRight = VARS.curRight;
        curUp = VARS.curUp;
        #endregion

        #region Collision

        #region Ray
        leftTopPoint = catTransform.position - curRight * catBreadth / 4 + curUp * (catBreadth / 2 - 0.02f);
        leftBottomPoint = catTransform.position - curRight * catBreadth / 4 - curUp * (catBreadth / 2 - 0.02f);
        rightTopPoint = catTransform.position + curRight * catBreadth / 4 + curUp * (catBreadth / 2 - 0.02f);
        rightBottomPoint = catTransform.position + curRight * catBreadth / 4 - curUp * (catBreadth / 2 - 0.02f);
        topLeftPoint = catTransform.position + curUp * catBreadth / 4 - curRight * (catBreadth / 2 - 0.02f);
        topRightPoint = catTransform.position + curUp * catBreadth / 4 + curRight * (catBreadth / 2 - 0.02f);
        bottomLeftPoint = catTransform.position - curUp * catBreadth / 4 - curRight * (catBreadth / 2 - 0.02f);
        bottomRightPoint = catTransform.position - curUp * catBreadth / 4 + curRight * (catBreadth / 2 - 0.02f);
        //liquidCenterPoint = catTransform.position + curUp * catBreadth / 40;
        //gasCenterPoint = catTransform.position - curUp * catBreadth / 40;
        topPoint = catTransform.position + curUp * (catBreadth / 2 - 0.02f);
        bottomPoint = catTransform.position - curUp * (catBreadth / 2 - 0.02f);
        leftPoint = catTransform.position - curRight * (catBreadth / 2 - 0.02f);
        rightPoint = catTransform.position + curRight * (catBreadth / 2 - 0.02f);

        leftRay1 = new Ray(leftTopPoint, -curRight);
        leftRay2 = new Ray(leftBottomPoint, -curRight);
        rightRay1 = new Ray(rightTopPoint, curRight);
        rightRay2 = new Ray(rightBottomPoint, curRight);
        upRay1 = new Ray(topLeftPoint, curUp);
        upRay2 = new Ray(topRightPoint, curUp);
        downRay1 = new Ray(bottomLeftPoint, -curUp);
        downRay2 = new Ray(bottomRightPoint, -curUp);
        //liquidCenterRay = new Ray(liquidCenterPoint, -curUp);
        //gasCenterRay = new Ray(gasCenterPoint, curUp);
        liquidDownRay = new Ray(topPoint, -curUp);
        liquidUpRay = new Ray(bottomPoint, curUp);
        gasUpRay = new Ray(bottomPoint, curUp);
        gasDownRay = new Ray(topPoint, -curUp);
        mistUpRay = new Ray(bottomPoint, curUp);
        mistDownRay = new Ray(topPoint, -curUp);
        mistLeftRay = new Ray(rightPoint, -curRight);
        mistRightRay = new Ray(leftPoint, curRight);
        #endregion        

        if (!VARS.IsRotating &&
            !VARS.IsTwisting)
        {
            #region OnGroundDetect
            if (Physics.Raycast(downRay1, out downHit1, rayDistance - VARS.verCurSpeed / 1000) ||
                Physics.Raycast(downRay2, out downHit2, rayDistance - VARS.verCurSpeed / 1000))
            {
                if (Physics.Raycast(downRay1, out downHit1, rayDistance - VARS.verCurSpeed / 1000))
                {
                    Detect(2, 1);
                }
                if (Physics.Raycast(downRay2, out downHit2, rayDistance - VARS.verCurSpeed / 1000))
                {
                    Detect(2, 2);
                }
            }

            if (VARS.IsGroundDetected)
            {
                VARS.IsGroundDetected = false;
                VARS.IsOnGround = true;
            }
            else
            {
                VARS.IsOnGround = false;
            }
            #endregion

            #region ToCeilingDetect
            if (Physics.Raycast(upRay1, out upHit1, rayDistance + VARS.verCurSpeed / 1000) ||
                Physics.Raycast(upRay2, out upHit2, rayDistance + VARS.verCurSpeed / 1000))
            {
                if (Physics.Raycast(upRay1, out upHit1, rayDistance + VARS.verCurSpeed / 1000))
                {
                    Detect(1, 1);
                }
                if (Physics.Raycast(upRay2, out upHit2, rayDistance + VARS.verCurSpeed / 1000))
                {
                    Detect(1, 2);
                }
            }

            if (VARS.IsCeilingDetected)
            {
                VARS.IsCeilingDetected = false;
                VARS.IsToCeiling = true;
            }
            else
            {
                VARS.IsToCeiling = false;
            }
            #endregion

            #region BlockDetect
            if (Physics.Raycast(leftRay1, out leftHit1, rayDistance - VARS.horCurSpeed / 1000) ||
                    Physics.Raycast(leftRay2, out leftHit2, rayDistance - VARS.horCurSpeed / 1000))
            {
                if (Physics.Raycast(leftRay1, out leftHit1, rayDistance - VARS.horCurSpeed / 1000))
                {
                    Detect(3, 1);
                }
                if (Physics.Raycast(leftRay2, out leftHit2, rayDistance - VARS.horCurSpeed / 1000))
                {
                    Detect(3, 2);
                }
            }

            if (VARS.IsLeftBlockDetected)
            {
                VARS.IsLeftBlockDetected = false;
                VARS.IsLeftBlocked = true;
            }
            else
            {
                VARS.IsLeftBlocked = false;
            }

            if (Physics.Raycast(rightRay1, out rightHit1, rayDistance + VARS.horCurSpeed / 1000) ||
                Physics.Raycast(rightRay2, out rightHit2, rayDistance + VARS.horCurSpeed / 1000))
            {
                if (Physics.Raycast(rightRay1, out rightHit1, rayDistance + VARS.horCurSpeed / 1000))
                {
                    Detect(4, 1);
                }
                if (Physics.Raycast(rightRay2, out rightHit2, rayDistance + VARS.horCurSpeed / 1000))
                {
                    Detect(4, 2);
                }
            }

            if (VARS.IsRightBlockDetected)
            {
                VARS.IsRightBlockDetected = false;
                VARS.IsRightBlocked = true;
            }
            else
            {
                VARS.IsRightBlocked = false;
            }
            #endregion

            #region LiquidDetect
            if (Physics.Raycast(liquidDownRay, out liquidDownHit, longRayDistance) ||
                    Physics.Raycast(liquidUpRay, out liquidUpHit, longLongLongRayDistance))
            {
                if (Physics.Raycast(liquidDownRay, out liquidDownHit, longRayDistance))
                {
                    Detect(5, 1);
                }
                if (Physics.Raycast(liquidUpRay, out liquidUpHit, longLongLongRayDistance))
                {
                    Detect(5, 2);
                }
            }

            if (VARS.IsLiquidDetected)
            {
                //intoLiquidSlowDown
                if (!VARS.IsInLiquid)
                {
                    if (Mathf.Abs(VARS.verCurSpeed) > verMaxSpeed / 2)
                    {
                        if (curTileData != null)
                            //verCurSpeed = verCurSpeed / curTileData.fluidDrag;
                            UFL.SetVerCurSpeed(VARS.verCurSpeed / curTileData.fluidDrag);
                    }
                }

                VARS.IsLiquidDetected = false;
                VARS.IsInLiquid = true;
            }
            else
            {
                //outLiquidSlowDown
                if (VARS.IsInLiquid)
                {
                    if (Mathf.Abs(VARS.verCurSpeed) > verMaxSpeed / 2)
                    {
                        //verCurSpeed = verCurSpeed / 2;
                        UFL.SetVerCurSpeed(VARS.verCurSpeed / 2);
                    }
                }

                VARS.IsInLiquid = false;
            }
            #endregion

            #region GasDetect
            if (Physics.Raycast(gasUpRay, out gasUpHit, longRayDistance) ||
                    Physics.Raycast(gasDownRay, out gasDownHit, longLongLongRayDistance))
            {
                if (Physics.Raycast(gasUpRay, out gasUpHit, longRayDistance))
                {
                    Detect(6, 1);
                }
                if (Physics.Raycast(gasDownRay, out gasDownHit, longLongLongRayDistance))
                {
                    Detect(6, 2);
                }
            }

            if (VARS.IsGasDetected)
            {
                VARS.IsGasDetected = false;
                VARS.IsInGas = true;
            }
            else
            {
                VARS.IsInGas = false;
            }

            //mightAdoptedTigherVersion
            //VARS.IsInGas = VARS.IsGasDetected;
            //VARS.IsGasDetected = false;
            #endregion

            #region MistDetect
            if (Physics.Raycast(mistUpRay, out mistUpHit, longLongRayDistance) ||
                Physics.Raycast(mistDownRay,out mistDownHit,longLongRayDistance) ||
                Physics.Raycast(mistLeftRay, out mistLeftHit, longLongRayDistance) ||
                Physics.Raycast(mistRightRay, out mistRightHit, longLongRayDistance))
            {
                if (Physics.Raycast(mistUpRay, out mistUpHit, longLongRayDistance))
                {
                    Detect(7, 1);
                }
                if (Physics.Raycast(mistDownRay, out mistDownHit, longLongRayDistance))
                {
                    Detect(7, 2);
                }
                if (Physics.Raycast(mistLeftRay, out mistLeftHit, longLongRayDistance))
                {
                    Detect(7, 3);
                }
                if (Physics.Raycast(mistRightRay, out mistRightHit, longLongRayDistance))
                {
                    Detect(7, 4);
                }
            }

            if (VARS.IsMistDetected)
            {
                VARS.IsMistDetected = false;
                VARS.IsInMist = true;
            }
            else
            {
                VARS.IsInMist = false;
            }
            #endregion
        }
        #endregion

        #region OnGroundOrInLiquidReset
        if (!VARS.IsRotating &&
            !VARS.IsTwisting)
        {
            if (VARS.IsOnGround ||
                VARS.IsInLiquid)
            {
                VARS.IsAttachWall = false;
            }
        }
        #endregion
    }

    //dir(~~?): 1-up, 2-down, 3-left, 4-right, 5-liquid, 6-Gas, 7-Mist
    //ray: 1-1, 2-2 or 1-liquidDown, 2-liquidUp or 1-gasUp, 2-gasDown or 1-mistUp, 2-mistDown, 3-mistLeft, 4-mistRight
    void Detect(int dirIndex, int rayIndex)
    {
        //hitDetermine(~~?)
        if (dirIndex < 7)
        {
            //up
            if (dirIndex == 1)
            {
                if (rayIndex == 1)
                    tempHit = upHit1;
                else if (rayIndex == 2)
                    tempHit = upHit2;

                tempVector = curUp;
                tempInt = -1;
                tempFloat = 1;
            }
            //down
            else if (dirIndex == 2)
            {
                if (rayIndex == 1)
                    tempHit = downHit1;
                else if (rayIndex == 2)
                    tempHit = downHit2;

                tempVector = curUp;
                tempInt = 1;
                tempFloat = 1;
            }
            //left
            else if (dirIndex == 3)
            {
                if (rayIndex == 1)
                    tempHit = leftHit1;
                else if (rayIndex == 2)
                    tempHit = leftHit2;

                tempVector = curRight;
                tempInt = 1;
                tempFloat = 0;
            }
            //right
            else if (dirIndex == 4)
            {
                if (rayIndex == 1)
                    tempHit = rightHit1;
                else if (rayIndex == 2)
                    tempHit = rightHit2;

                tempVector = curRight;
                tempInt = -1;
                tempFloat = 0;
            }
            //liquid
            else if (dirIndex == 5)
            {
                if (rayIndex == 1)
                    tempHit = liquidDownHit;
                else if (rayIndex == 2)
                    tempHit = liquidUpHit;
            }
            //gas
            else if (dirIndex == 6)
            {
                if (rayIndex == 1)
                    tempHit = gasUpHit;
                else if (rayIndex == 2)
                    tempHit = gasDownHit;
            }
        }
        else if (dirIndex == 7)
        {
            if (rayIndex == 1)
                tempHit = mistUpHit;
            else if (rayIndex == 2)
                tempHit = mistDownHit;
            else if (rayIndex == 3)
                tempHit = mistLeftHit;
            else if (rayIndex == 4)
                tempHit = mistRightHit;
        }

            curTile = tempHit.transform.gameObject;
        curTileData = curTile.GetComponent<TileData>();

        if (curTileData != null)
        {
            //notTrigger
            if (curTileData.triggerTypeIndex == 0)
            {
                //switch (dirIndex)
                //{
                //    case 1:
                //        curUpTileData = curTileData;
                //        break;
                //    case 2:
                //        curDownTileData = curTileData;
                //        break;
                //    case 3:
                //        curLeftTileData = curTileData;
                //        break;
                //    case 4:
                //        curRightTileData = curTileData;
                //        break;
                //}

                //solid
                if (curTileData.stateOfMatterIndex == 0 &&
                    dirIndex <= 4)
                {
                    //void
                    if (curTileData.blockTypeIndex == 2010)
                    {
                        VARS.IsToDie = true;
                    }

                    switch (dirIndex)
                    {
                        case 1:
                            VARS.curUpTileData = curTileData;
                            if (!curTileData.isPlatform)
                                VARS.IsCeilingDetected = true;
                            break;
                        case 2:
                            VARS.curDownTileData = curTileData;
                            VARS.IsGroundDetected = true; ;
                            break;
                        case 3:
                            VARS.curLeftTileData = curTileData;
                            if (!curTileData.isPlatform)
                                VARS.IsLeftBlockDetected = true;
                            break;
                        case 4:
                            VARS.curRightTileData = curTileData;
                            if (!curTileData.isPlatform)
                                VARS.IsRightBlockDetected = true; ;
                            break;
                    }

                    if (dirIndex == 2 ||
                        !curTileData.isPlatform)
                    {
                        VARS.curTilePosition = curTile.transform.position;

                        if (dirIndex == 1)
                        {
                            positionFixOffset += 0.001f * tempFloat;
                        }

                        catTransform.position = new Vector3
                            (catTransform.position.x * Mathf.Abs(Mathf.Abs(tempVector.x) - 1) + VARS.curTilePosition.x * Mathf.Abs(tempVector.x) + tempInt * (positionFixOffset - 0.001f * tempFloat) * tempVector.x,
                            catTransform.position.y * Mathf.Abs(Mathf.Abs(tempVector.y) - 1) + VARS.curTilePosition.y * Mathf.Abs(tempVector.y) + tempInt * (positionFixOffset - 0.001f * tempFloat) * tempVector.y,
                            catTransform.position.z * Mathf.Abs(Mathf.Abs(tempVector.z) - 1) + VARS.curTilePosition.z * Mathf.Abs(tempVector.z) + tempInt * (positionFixOffset - 0.001f * tempFloat) * tempVector.z);

                        if (dirIndex == 1)
                        {
                            positionFixOffset -= 0.001f * tempFloat;
                        }

                        ////notSharp
                        //if (curTileData.sharpDirVector == Vector3.zero)
                        //{
                        //}
                        ////sharp
                        //else
                        //{
                        //    if (Vector3.Dot(tempVector * tempInt, curTileData.sharpDirVector) > 0)
                        //    {
                        //        //Die();

                        //        VARS.IsToDie = true;
                        //    }
                        //}

                        //toughness
                        if (curTileData.toughness != 999)
                        {
                            if (dirIndex == 2)
                            {
                                breakingPower = Mathf.Max(-VARS.verCurSpeed, breakingPower);

                                if (breakingPower > curTileData.toughness)
                                {
                                    curTile.SetActive(false);
                                }
                            }
                        }
                        else
                        {
                            breakingPower = 0;
                        }

                        //elasticity
                        if (dirIndex == 1 ||
                            dirIndex == 2)
                        {
                            //verCurSpeed = -verCurSpeed * curTileData.elasticity;
                            UFL.SetVerCurSpeed(-VARS.verCurSpeed * curTileData.elasticity);

                            if (dirIndex == 2)
                            {
                                //curEnergy += elasticEnergyRestoreAmount;

                                UFL.AddCurTargetEnergy(elasticEnergyRestoreAmount);
                            }
                        }
                        else if (dirIndex == 3 ||
                            dirIndex == 4)
                        {
                            //horCurSpeed = -horCurSpeed * curTileData.elasticity;

                            UFL.SetHorCurSpeed(-VARS.horCurSpeed * curTileData.elasticity);
                        }
                    }
                    else
                    {
                        //Debug.Log("enter");
                    }
                }
                //liquid
                else if (curTileData.stateOfMatterIndex == 1
                    && dirIndex == 5)
                {
                    VARS.curLiquidTileData = curTileData;

                    if (rayIndex == 1)
                    {
                        VARS.buoyancyDistanceFixFloat = tempHit.distance;
                    }
                    else if (rayIndex == 2)
                    {
                        VARS.buoyancyDistanceFixFloat = 0;
                    }

                    VARS.IsLiquidDetected = true;

                    breakingPower = 0;
                }
                //gas
                else if (curTileData.stateOfMatterIndex == 2
                    && dirIndex == 6)
                {
                    VARS.curGasTileData = curTileData;

                    VARS.IsGasDetected = true;

                    breakingPower = 0;
                }
                else if (curTileData.stateOfMatterIndex == 3
                    && dirIndex == 7)
                {
                    VARS.curMistTileData = curTileData;

                    VARS.IsMistDetected = true;

                    breakingPower = 0;
                }
            }
            //trigger
            else
            {
                VARS.curTriggerTile = curTile;
                VARS.curTriggerTileData = curTileData;

                //strawberry(get)
                if (curTileData.triggerTypeIndex == 1)
                {
                    //isCarryingStrawberries = true;

                    //carriedStrawberries.Add(curTile);
                    //carriedStrawberriesIniPositions.Add(curTile.transform.position);

                    VARS.IsGettingAStrawberry = true;
                }

                //energyCrystal(get)
                else if (curTileData.triggerTypeIndex == 2)
                {
                    if (curTile.transform.localScale != Vector3.one * 0.2f)
                    {
                        VARS.IsGettingAnEnergyCrystal = true;
                    }
                }

                //gate
                else if (curTileData.triggerTypeIndex == 3)
                {

                }

                //edgeGate(enter)
                else if (curTileData.triggerTypeIndex == 4)
                {
                    VARS.IsEnteringAnEdgeGate = true;
                }

                //edgeGateTrigger(triggerEdgeGate)
                else if (curTileData.triggerTypeIndex == 5)
                {
                    VARS.IsEdgeGateTriggered = true;
                }

                //activateSavePoint(notActiavted)
                else if (curTileData.triggerTypeIndex == 6)
                {
                    VARS.IsToActivateASavePoint = true;
                }

                //activatedSavePoint(~~?)
                else if (curTileData.triggerTypeIndex == 7)
                {

                }

                //center(in)
                else if (curTileData.triggerTypeIndex == 8)
                {
                    VARS.IsInCenter = true;
                }
			}

			//center(out)
			if (curTileData.triggerTypeIndex != 8)
			{
				VARS.IsInCenter = false;
			}
		}
    }

}
