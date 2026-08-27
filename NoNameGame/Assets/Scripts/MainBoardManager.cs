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

    GameObject[] mainBoardOverTextEmpties = new GameObject[2];
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
        mainBoardOverTextEmpties = CONS.mainBoardOverTextEmpties;
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
            //language
            for (int i = 0; i < mainBoardOverTextEmpties.Length; i++)
            {
                mainBoardOverTextEmpties[i].SetActive(i == VARS.CurLanguageIndex);
            }

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
            mainBoardOverTextEmpties[VARS.CurLanguageIndex].transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
            mainBoardOverTextEmpties[VARS.CurLanguageIndex].transform.GetChild(0).GetChild(1).gameObject.SetActive(true);
            mainBoardOverTextEmpties[VARS.CurLanguageIndex].transform.GetChild(1).GetChild(0).gameObject.SetActive(true);
            mainBoardOverTextEmpties[VARS.CurLanguageIndex].transform.GetChild(1).GetChild(1).gameObject.SetActive(false);

            if (VARS.IsSpaceDown || VARS.IsReturnDown)
            {
                mainBoard.SetActive(false);

                VARS.IsInMainBoard = false;
            }
        }
        else if (curOptionIndex == 1)
        {
            mainBoardOverTextEmpties[VARS.CurLanguageIndex].transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
            mainBoardOverTextEmpties[VARS.CurLanguageIndex].transform.GetChild(0).GetChild(1).gameObject.SetActive(false);
            mainBoardOverTextEmpties[VARS.CurLanguageIndex].transform.GetChild(1).GetChild(0).gameObject.SetActive(false);
            mainBoardOverTextEmpties[VARS.CurLanguageIndex].transform.GetChild(1).GetChild(1).gameObject.SetActive(true);

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