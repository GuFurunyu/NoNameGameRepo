using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder((int)ScriptsExecutionOrder.ExecutionOrder.cheatManager)]
public class CheatManager : MonoBehaviour
{
    Constants CONS;
    Variables VARS;
    UniversalFunctionsLibrary UFL;
    ScriptsExecutionController SEC;

    GameObject gameManager;

    #region ConstantsUsed
    float immortalEnergyBonus;

    string immortalCheatActivatingString;
    string immortalCheatDeactivatingString;
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
        immortalEnergyBonus = CONS.immortalEnergyBonus;
        immortalCheatActivatingString = CONS.immortalCheatActivatingString;
        immortalCheatDeactivatingString = CONS.immortalCheatDeactivatingString;
        #endregion

        #region ImportReferenceVariable
        #endregion
    }

    void Update()
    {
        #region ImportValueVariables
        #endregion

        if (Input.anyKeyDown)
        {
            //immortalActivating
            if (Input.inputString.ToLower() == immortalCheatActivatingString[VARS.immortalCheatCurActivatingCharIndex].ToString().ToLower())
            {
                Debug.Log("immortalCheatActivating " + VARS.immortalCheatCurActivatingCharIndex);

                VARS.immortalCheatCurActivatingCharIndex++;

                if (VARS.immortalCheatCurActivatingCharIndex == immortalCheatActivatingString.Length)
                {
                    Debug.Log("immortalCheatActivated");

                    VARS.maxEnergyBonus += immortalEnergyBonus;

                    VARS.immortalCheatCurActivatingCharIndex = 0;
                }
            }
            else
            {
                VARS.immortalCheatCurActivatingCharIndex = 0;
            }

            //immortalDeactivating
            if (Input.inputString.ToLower() == immortalCheatDeactivatingString[VARS.immortalCheatCurDeactivatingCharIndex].ToString().ToLower())
            {
                Debug.Log("immortalCheatDeactivating " + VARS.immortalCheatCurDeactivatingCharIndex);

                VARS.immortalCheatCurDeactivatingCharIndex++;

                if (VARS.immortalCheatCurDeactivatingCharIndex == immortalCheatDeactivatingString.Length)
                {
                    Debug.Log("immortalCheatDeactivated");

                    while (VARS.maxEnergyBonus > immortalEnergyBonus)
                    {
                        VARS.maxEnergyBonus -= immortalEnergyBonus;
                    }
                    VARS.maxEnergyBonus -= immortalEnergyBonus;

                    VARS.immortalCheatCurDeactivatingCharIndex = 0;
                }
            }
            else
            {
                VARS.immortalCheatCurDeactivatingCharIndex = 0;
            }
        }
    }
}
