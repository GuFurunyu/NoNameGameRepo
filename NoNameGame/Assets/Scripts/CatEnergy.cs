using UnityEngine;


[DefaultExecutionOrder((int)ScriptsExecutionOrder.ExecutionOrder.catEnergy)]
public class CatEnergy : MonoBehaviour
{
    Constants CONS;
    Variables VARS;
    UniversalFunctionsLibrary UFL;
    ScriptsExecutionController SEC;

    GameObject gameManager;

    #region ConstantsUsed
    float maxEnergy;

    float onGroundEnergyRestoreSpeed;

    float curEnergyChangeToTargetEnergySpeed;
    #endregion

    #region VariablesUsed
    //float curEnergy;
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
        maxEnergy = CONS.maxEnergy;
        onGroundEnergyRestoreSpeed = CONS.onGroundEnergyRestoreSpeed;
        curEnergyChangeToTargetEnergySpeed = CONS.curEnergyChangeToTargetEnergySpeed;
        #endregion

        #region ImportReferenceVariables
        #endregion
    }

    void Update()
    {
        #region ImportValueVariables
        #endregion

        #region ImportBoolVariables
        isOnGround = VARS.IsOnGround;
        isInLiquid = VARS.IsInLiquid;
        #endregion

        #region curEnergyChangeToTargetEnergy
        //increase
        if (VARS.curEnergy < VARS.curTargetEnergy)
        {
            UFL.AddCurEnergy(curEnergyChangeToTargetEnergySpeed * Time.deltaTime);

            if (VARS.curEnergy > VARS.curTargetEnergy)
            {
                UFL.SetCurEnergy(VARS.curTargetEnergy);
            }
        }
        //decrease
        else if (VARS.curEnergy > VARS.curTargetEnergy)
        {
            UFL.AddCurEnergy(-curEnergyChangeToTargetEnergySpeed * Time.deltaTime);

            if (VARS.curEnergy < VARS.curTargetEnergy)
            {
                UFL.SetCurEnergy(VARS.curTargetEnergy);
            }
        }
        #endregion

        #region OutOfBoundReset     
        if (VARS.curEnergy > CONS.maxEnergy)
        {
            UFL.SetCurEnergy(CONS.maxEnergy);
        }

        if (VARS.curTargetEnergy > maxEnergy)
        {
            UFL.SetCurTargetEnergy(maxEnergy);
        }
        else if (VARS.curTargetEnergy < 0)
        {
            UFL.SetCurTargetEnergy(0);
        }
        #endregion

        #region OnGroundOrInLiquidReset
        if (!VARS.IsRotating &&
            !VARS.IsTwisting)
        {
            if (isOnGround ||
                isInLiquid)
            {
                //energyRestore
                if (VARS.curTargetEnergy < maxEnergy)
                {
                    //curEnergy += onGroundEnergyRestoreSpeed * Time.deltaTime;
                    UFL.AddCurTargetEnergy(onGroundEnergyRestoreSpeed * Time.deltaTime);
                }
            }
            else
            {
                //if (isIniRotation)
                //{
                //    if (rotationNumRestoreStartTime != 0)
                //    {
                //        rotationNumRestoreStartTime = 0;
                //    }
                //}
            }

            //if (isInLiquid)
            //{
            //    if (curEnergy < maxEnergy)
            //    {
            //        curEnergy += inLiquidEnergyRestoreSpeed * Time.deltaTime;
            //    }
            //    if (curEnergy > maxEnergy)
            //    {
            //        curEnergy = maxEnergy;
            //    }
            //}
        }
        #endregion

        //ifOutOfEnergyDie
        if (VARS.curEnergy <= 0)
        {
            VARS.IsToDie = true;
        }

        //VARS.curEnergy = curEnergy;
    }
}
