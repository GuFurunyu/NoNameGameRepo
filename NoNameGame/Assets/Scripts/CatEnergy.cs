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
    #endregion

    #region VariablesUsed
    bool isOnGround;
    bool isInLiquid;

    //float curEnergy;
    #endregion

    void Start()
    {
        gameManager = GameObject.Find("GameManager");

        CONS = gameManager.GetComponent<Constants>();
        VARS = gameManager.GetComponent<Variables>();
        UFL = gameManager.GetComponent<UniversalFunctionsLibrary>();
        SEC = gameManager.GetComponent<ScriptsExecutionController>();

        maxEnergy = CONS.maxEnergy;
        onGroundEnergyRestoreSpeed = CONS.onGroundEnergyRestoreSpeed;
    }

    void Update()
    {
        isOnGround=VARS.IsOnGround;
        isInLiquid = VARS.IsInLiquid;
        //curEnergy = VARS.curEnergy;

        #region OnGroundOrInLiquidReset
        if (!VARS.IsRotating && 
            !VARS.IsTwisting)
        {
            if (isOnGround ||
                isInLiquid)
            {
                //energyRestore
                if (VARS.curEnergy < maxEnergy)
                {
                    //curEnergy += onGroundEnergyRestoreSpeed * Time.deltaTime;
                    UFL.AddCurEnergy(onGroundEnergyRestoreSpeed * Time.deltaTime);
                }
                if (VARS.curEnergy > maxEnergy)
                {
                    //curEnergy = maxEnergy;
                    UFL.SetCurEnergy(maxEnergy);
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
        if (VARS.curEnergy <= 0)
        {
            VARS.IsToDie = true;
        }

        //VARS.curEnergy = curEnergy;
    }
}
