using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder((int)ScriptsExecutionOrder.ExecutionOrder.mainBoardManager)]
public class MainBoardManager : MonoBehaviour
{
    Constants CONS;
    Variables VARS;
    UniversalFunctionsLibrary UFL;
    ScriptsExecutionController SEC;

    GameObject gameManager;

    int curOptionIndex;

    #region ConstantsUsed
    GameObject mainBoard;
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
        mainBoard = CONS.mainBoard;
        #endregion

        #region ImportReferenceVariable
        #endregion
    }

    void Update()
    {
        #region ImportValueVariables
        #endregion

        if (VARS.IsInMainBoard)
        {
            if (VARS.IsDownKeyDown)
            {
                curOptionIndex++;

                if (curOptionIndex > 1)
                {
                    curOptionIndex = 1;
                }
            }
            else if (VARS.IsUpKeyDown)
            {
                curOptionIndex--;

                if (curOptionIndex < 0)
                {
                    curOptionIndex = 0;
                }
            }
        }

        if (curOptionIndex == 0)
        {
            mainBoard.transform.GetChild(1).GetChild(0).gameObject.SetActive(false);
            mainBoard.transform.GetChild(1).GetChild(1).gameObject.SetActive(true);
            mainBoard.transform.GetChild(2).GetChild(0).gameObject.SetActive(true);
            mainBoard.transform.GetChild(2).GetChild(1).gameObject.SetActive(false);

            if (VARS.IsSpaceDown || VARS.IsReturnDown)
            {
                mainBoard.SetActive(false);

                VARS.IsInMainBoard = false;
            }
        }
        else if (curOptionIndex == 1)
        {
            mainBoard.transform.GetChild(1).GetChild(0).gameObject.SetActive(true);
            mainBoard.transform.GetChild(1).GetChild(1).gameObject.SetActive(false);
            mainBoard.transform.GetChild(2).GetChild(0).gameObject.SetActive(false);
            mainBoard.transform.GetChild(2).GetChild(1).gameObject.SetActive(true);

            if (VARS.IsSpaceDown || VARS.IsReturnDown)
            {
                #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
                #endif

                Application.Quit();
            }
        }
    }
}