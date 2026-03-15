using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder((int)ScriptsExecutionOrder.ExecutionOrder.catState)]
public class CatState : MonoBehaviour
{
    Constants CONS;
    Variables VARS;
    UniversalFunctionsLibrary UFL;
    ScriptsExecutionController SEC;

    GameObject gameManager;

    #region ConstantsUsed
    float intoHotStateTemperature;
    float intoColdStateTemperature;
    float temperatureSetToZeroThres;

    float intoElectricStateElectricity;
    float electricitySetToZeroThres;

    float intoToxicStateToxicity;
    float toxicitySetToZeroThres;

    float inHotStateEnergyDecreaseSpeed;
    float inColdStateEnergyDecreaseSpeed;
    float inElectricStateEnergyDecreaseSpeed;
    float inToxicStateEnergyDecreaseSpeed;
    #endregion

    #region VariablesUsed

    #endregion

    void Start()
    {
        gameManager = GameObject.Find("GameManager");

        CONS = gameManager.GetComponent<Constants>();
        VARS = gameManager.GetComponent<Variables>();
        UFL = gameManager.GetComponent<UniversalFunctionsLibrary>();
        SEC = gameManager.GetComponent<ScriptsExecutionController>();

        #region ImportConstants
        intoHotStateTemperature = CONS.intoHotStateTemperature;
        intoColdStateTemperature = CONS.intoColdStateTemperature;
        temperatureSetToZeroThres = CONS.temperatureSetToZeroThres;
        intoElectricStateElectricity = CONS.intoElectricStateElectricity;
        electricitySetToZeroThres = CONS.electricitySetToZeroThres;
        intoToxicStateToxicity = CONS.intoToxicStateToxicity;
        toxicitySetToZeroThres = CONS.toxicitySetToZeroThres;
        inHotStateEnergyDecreaseSpeed = CONS.inHotStateEnergyDecreaseSpeed;
        inColdStateEnergyDecreaseSpeed = CONS.inColdStateEnergyDecreaseSpeed;
        inElectricStateEnergyDecreaseSpeed = CONS.inElectricStateEnergyDecreaseSpeed;
        inToxicStateEnergyDecreaseSpeed = CONS.inToxicStateEnergyDecreaseSpeed;
        #endregion

        #region ImportReferenceVariable
        #endregion
    }

    void Update()
    {
        #region ImportValueVariables
        #endregion

        #region Temperature
        //hot
        if (VARS.catCurTemperature > intoHotStateTemperature)
        {
            VARS.IsInHotState = true;

            UFL.AddCurTargetEnergy(-inHotStateEnergyDecreaseSpeed * Time.deltaTime);
        }
        else
        {
            VARS.IsInHotState = false;
        }
        //cold
        if (VARS.catCurTemperature < intoColdStateTemperature)
        {
            VARS.IsInColdState = true;

            UFL.AddCurTargetEnergy(-inColdStateEnergyDecreaseSpeed * Time.deltaTime);
        }
        else
        {
            VARS.IsInColdState = false;
        }

        //setToZero
        if (Mathf.Abs(VARS.catCurTemperature) < temperatureSetToZeroThres)
        {
            UFL.SetCatCurTemperature(0);
        }
        #endregion

        #region electricity
        if (VARS.catCurElectricity > intoElectricStateElectricity)
        {
            VARS.IsInElectricState = true;

            UFL.AddCurTargetEnergy(-inElectricStateEnergyDecreaseSpeed * Time.deltaTime);
        }
        else
        {
            VARS.IsInElectricState = false;
        }

        //setToZero
        if(Mathf.Abs(VARS.catCurElectricity) < electricitySetToZeroThres)
        {
            UFL.SetCatCurElectricity(0);
        }
        #endregion

        #region toxicity
        if (VARS.catCurToxicity > intoToxicStateToxicity)
        {
            VARS.IsInToxicState = true;

            UFL.AddCurTargetEnergy(-inToxicStateEnergyDecreaseSpeed * Time.deltaTime);
        }
        else
        {
            VARS.IsInToxicState = false;
        }

        //setToZero
        if(Mathf.Abs(VARS.catCurToxicity) < toxicitySetToZeroThres)
        {
            UFL.SetCatCurToxicity(0);
        }
        #endregion
    }
}
