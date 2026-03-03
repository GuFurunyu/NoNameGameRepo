using UnityEngine;


[DefaultExecutionOrder((int)ScriptsExecutionOrder.ExecutionOrder.catEnergy)]
public class CatEnergy : MonoBehaviour
{
    Constants CONS;
    Variables VARS;
    ScriptsExecutionController SEC;

    GameObject gameManager;

    #region ConstantsUsed
    float maxEnergy;

    float onGroundEnergyRestoreSpeed;
    #endregion

    #region VariablesUsed
    bool isOnGround;
    bool isInLiquid;

    float curEnergy;
    #endregion

    void Start()
    {
        gameManager = GameObject.Find("GameManager");

        CONS = gameManager.GetComponent<Constants>();
        VARS = gameManager.GetComponent<Variables>();
        SEC = gameManager.GetComponent<ScriptsExecutionController>();

        maxEnergy = CONS.maxEnergy;
        onGroundEnergyRestoreSpeed = CONS.onGroundEnergyRestoreSpeed;
    }

    void Update()
    {
        isOnGround=VARS.isOnGround;
        isInLiquid = VARS.isInLiquid;
        curEnergy = VARS.curEnergy;

        #region OnGroundOrInLiquidReset
        if (!VARS.isRotating && 
            !VARS.isTwisting)
        {
            if (isOnGround ||
                isInLiquid)
            {
                //energyRestore
                if (curEnergy < maxEnergy)
                {
                    curEnergy += onGroundEnergyRestoreSpeed * Time.deltaTime;
                }
                if (curEnergy > maxEnergy)
                {
                    curEnergy = maxEnergy;
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

        //outOfEnergy
        if (curEnergy <= 0)
        {
            VARS.isToDie = true;
        }

        VARS.curEnergy = curEnergy;
    }
}
