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
            //UFL.AddCurEnergy(curEnergyChangeToTargetEnergySpeed * Time.deltaTime);
            VARS.curEnergy += curEnergyChangeToTargetEnergySpeed * Time.deltaTime;

            if (VARS.curEnergy > VARS.curTargetEnergy)
            {
                //UFL.SetCurEnergy(VARS.curTargetEnergy);
                VARS.curEnergy = VARS.curTargetEnergy;
            }
        }
        //decrease
        else if (VARS.curEnergy > VARS.curTargetEnergy)
        {
            //UFL.AddCurEnergy(-curEnergyChangeToTargetEnergySpeed * Time.deltaTime);
            VARS.curEnergy += -curEnergyChangeToTargetEnergySpeed * Time.deltaTime;

            if (VARS.curEnergy < VARS.curTargetEnergy)
            {
                //UFL.SetCurEnergy(VARS.curTargetEnergy);
                VARS.curEnergy = VARS.curTargetEnergy;
            }
        }
        #endregion

        #region OutOfBoundReset     
        if (VARS.curEnergy > maxEnergy + VARS.maxEnergyBonus)
        {
            //UFL.SetCurEnergy(CONS.maxEnergy + VARS.maxEnergyBonus);
            VARS.curEnergy = maxEnergy + VARS.maxEnergyBonus;
        }

        if (VARS.curTargetEnergy > maxEnergy + VARS.maxEnergyBonus)
        {
            //UFL.SetCurTargetEnergy(maxEnergy + VARS.maxEnergyBonus);
            VARS.curTargetEnergy = maxEnergy + VARS.maxEnergyBonus;
        }
        else if (VARS.curTargetEnergy < 0)
        {
            //UFL.SetCurTargetEnergy(0);
            VARS.curTargetEnergy = 0;
        }
        #endregion

        #region OnGroundOrInLiquidReset
        if (VARS.IsCatEnergyResetExecutable)
        {
            //energyRestore
            if (VARS.curTargetEnergy < maxEnergy + VARS.maxEnergyBonus)
            {
                //Debug.Log("energyRestore");

                //curEnergy += onGroundEnergyRestoreSpeed * Time.deltaTime;
                //UFL.AddCurTargetEnergy(onGroundEnergyRestoreSpeed * Time.deltaTime);
                VARS.curTargetEnergy += onGroundEnergyRestoreSpeed * Time.deltaTime;
            }
        }
        #endregion

        //ifOutOfEnergyDie
        if (VARS.curEnergy <= 0)
        {
            UFL.DebugLog("outOfEnergy");

            VARS.IsToDie = true;
        }

        //VARS.curEnergy = curEnergy;
    }
}
