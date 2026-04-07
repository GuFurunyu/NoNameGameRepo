using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder((int)ScriptsExecutionOrder.ExecutionOrder.optionsManager)]
public class OptionsManager : MonoBehaviour
{
    Constants CONS;
    Variables VARS;
    UniversalFunctionsLibrary UFL;
    ScriptsExecutionController SEC;

    GameObject gameManager;

    //bool isOptionPanelActivated;
    int curOptionIndex;

    bool isInKeySetSub;
    int curKeySetSubIndex;
    bool isSettingAKey;
    int curSetKeyIndex;

    bool isFromOptionsToKeySetSub;
    bool isFromKeySetSubToOptions;

    Transform tempTransform;
    KeyCode tempKeyCode;

    #region ConstantsUsed
    GameObject optionsPanel;

    GameObject optionsEmpty;
    List<GameObject> optionEmpties = new List<GameObject>();

    GameObject keySetSubEmpty;
    List<GameObject> keySetSubEmpties = new List<GameObject>();

    List<KeyCode> keyCodes = new List<KeyCode>();

    List<Sprite> keySprites = new List<Sprite>();
    List<Sprite> keyChosenSprites = new List<Sprite>();
    #endregion

    #region VariablesUsed
    List<KeyCode> curKeyCodes = new List<KeyCode>();
    #endregion


    void Start()
    {
        gameManager = GameObject.Find("GameManager");

        CONS = gameManager.GetComponent<Constants>();
        VARS = gameManager.GetComponent<Variables>();
        UFL = gameManager.GetComponent<UniversalFunctionsLibrary>();
        SEC = gameManager.GetComponent<ScriptsExecutionController>();

        #region ImportConstants
        optionsPanel = CONS.optionsPanel;
        optionsEmpty = CONS.optionsEmpty;
        optionEmpties = CONS.optionEmpties;
        keySetSubEmpty = CONS.keySetSubEmpty;
        keySetSubEmpties = CONS.keySetSubEmpties;
        keyCodes = CONS.keyCodes;
        keySprites = CONS.keySprites;
        keyChosenSprites = CONS.keyChosenSprites;
        #endregion

        #region ImportReferenceVariables
        curKeyCodes = VARS.curKeyCodes;
        #endregion

        //setKeySetSubKeysAppearance
        for (int i = 0; i < curKeyCodes.Count; i++)
        {
            for (int j = 0; j < keyCodes.Count; j++)
            {
                if (curKeyCodes[i] == keyCodes[j])
                {
                    keySetSubEmpties[i].transform.GetChild(2).GetComponent<SpriteRenderer>().sprite = keySprites[j];
                    keySetSubEmpties[i].transform.GetChild(3).GetComponent<SpriteRenderer>().sprite = keyChosenSprites[j];

                    break;
                }
            }
        }
    }

    void Update()
    {
        #region ImportValueVariables
        #endregion

        //activateOptionPanel
        if (VARS.IsOptionsManagerActivationExecutable)
        {
            if (VARS.IsBackKeyDown)
            {
                if (!isInKeySetSub)
                {
                    optionsPanel.SetActive(!optionsPanel.activeSelf);

                    VARS.IsOptionPanelActivated = optionsPanel.activeSelf;

                    curOptionIndex = 0;
                }
            }
        }

        if (VARS.IsOptionPanelActivated)
        {
            #region Options
            if (!isInKeySetSub)
            {
                //chooseOptions
                if (VARS.IsDownKeyDown)
                {
                    curOptionIndex++;

                    if (curOptionIndex > optionEmpties.Count - 1)
                    {
                        curOptionIndex = optionEmpties.Count - 1;
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

                //highLightTheChosenOption
                for (int i = 0; i < optionEmpties.Count; i++)
                {
                    tempTransform = optionEmpties[i].transform;

                    tempTransform.GetChild(0).gameObject.SetActive(i != curOptionIndex);
                    tempTransform.GetChild(1).gameObject.SetActive(i == curOptionIndex);
                }
            }
            #endregion

            #region KeySetSub
            if (curOptionIndex == 0)
            {
                //intoKeySetSub
                if (!isInKeySetSub &&
                    VARS.IsConfirmKeyDown)
                {
                    isFromOptionsToKeySetSub = true;
                }
                if (isFromOptionsToKeySetSub)
                {
                    isFromOptionsToKeySetSub = false;

                    isInKeySetSub = true;

                    optionsEmpty.SetActive(false);
                    keySetSubEmpty.SetActive(true);

                    //Input.ResetInputAxes();

                    VARS.IsConfirmKeyDown = false;
                }

                if (isInKeySetSub)
                {
                    //chooseKeys
                    if (!isSettingAKey)
                    {
                        if (VARS.IsDownKeyDown)
                        {
                            curKeySetSubIndex++;

                            if (curKeySetSubIndex > keySetSubEmpties.Count - 1)
                            {
                                curKeySetSubIndex = keySetSubEmpties.Count - 1;
                            }
                        }
                        else if (VARS.IsUpKeyDown)
                        {
                            curKeySetSubIndex--;

                            if (curKeySetSubIndex < 0)
                            {
                                curKeySetSubIndex = 0;
                            }
                        }
                    }

                    //highLightTheChosenKey
                    for (int i = 0; i < keySetSubEmpties.Count; i++)
                    {
                        tempTransform = keySetSubEmpties[i].transform;

                        tempTransform.GetChild(0).gameObject.SetActive(i != curKeySetSubIndex || isSettingAKey);
                        tempTransform.GetChild(1).gameObject.SetActive(i == curKeySetSubIndex && !isSettingAKey);
                        if (tempTransform.childCount > 2)
                        {
                            tempTransform.GetChild(2).gameObject.SetActive(i != curKeySetSubIndex || !isSettingAKey);
                            tempTransform.GetChild(3).gameObject.SetActive(i == curKeySetSubIndex && isSettingAKey);
                        }
                    }

                    //toSetAKey
                    if (!isSettingAKey &&
                        curKeySetSubIndex < keySetSubEmpties.Count - 1 &&
                        VARS.IsConfirmKeyDown)
                    {
                        isSettingAKey = true;

                        Input.ResetInputAxes();
                    }

                    //setAKey
                    if (isSettingAKey)
                    {
                        if (Input.anyKeyDown)
                        {
                            tempKeyCode = GetTheInputedKey();

                            if ((tempKeyCode != KeyCode.None &&
                                !curKeyCodes.Contains(tempKeyCode)) ||
                                curKeyCodes[curKeySetSubIndex] == tempKeyCode)
                            {
                                //logicChange
                                switch (curKeySetSubIndex)
                                {
                                    case 0:
                                        VARS.upKeyCode = tempKeyCode;
                                        break;
                                    case 1:
                                        VARS.downKeyCode = tempKeyCode;
                                        break;
                                    case 2:
                                        VARS.leftKeyCode = tempKeyCode;
                                        break;
                                    case 3:
                                        VARS.rightKeyCode = tempKeyCode;
                                        break;
                                    case 4:
                                        VARS.jumpKeyCode = tempKeyCode;
                                        break;
                                    case 5:
                                        VARS.acceKeyCode = tempKeyCode;
                                        break;
                                    case 6:
                                        VARS.grabKeyCode = tempKeyCode;
                                        break;
                                }
                                curKeyCodes[curKeySetSubIndex] = tempKeyCode;

                                //appearanceChange
                                keySetSubEmpties[curKeySetSubIndex].transform.GetChild(2).GetComponent<SpriteRenderer>().sprite = keySprites[curSetKeyIndex];
                                keySetSubEmpties[curKeySetSubIndex].transform.GetChild(3).GetComponent<SpriteRenderer>().sprite = keyChosenSprites[curSetKeyIndex];

                                VARS.IsToWriteKeyCodesData = true;

                                VARS.IsKeyCodeChanged = true;

                                isSettingAKey = false;
                            }
                        }
                    }

                    //ok
                    if ((curKeySetSubIndex == keySetSubEmpties.Count - 1 &&
                        VARS.IsConfirmKeyDown) ||
                        VARS.IsBackKeyDown)
                    {
                        isFromKeySetSubToOptions = true;
                    }
                    if (isFromKeySetSubToOptions)
                    {
                        isFromKeySetSubToOptions = false;

                        isInKeySetSub = false;

                        keySetSubEmpty.SetActive(false);
                        optionsEmpty.SetActive(true);

                        curKeySetSubIndex = 0;

                        isSettingAKey = false;
                    }
                }
            }
            #endregion

            #region Exit
            if (curOptionIndex == 1)
            {
                if (VARS.IsConfirmKeyDown)
                {
                    VARS.IsWritingAllData = true;
                    VARS.IsExiting = true;
                }
                if (VARS.IsExiting)
                {
                    if (!VARS.IsWritingAllData)
                    {
                        #if UNITY_EDITOR
                        UnityEditor.EditorApplication.isPlaying = false;
                        #endif

                        Application.Quit();
                    }
                }
            }
            #endregion
        }
    }

    //setKeyIndex:
    //Space-0,
    //0-1, 1-2, 2-3, 3-4, 4-5, 5-6, 6-7, 7-8, 8-9, 9-10,
    //A-11, B-12, C-13, D-14, E-15, F-16, G-17, H-18, I-19, J-20, K-21, L-22, M- 23, N-24,
    //O-25, P-26, Q-27, R-28, S-29, T-30, U-31, V-32, W-33, X-34, Y-35, Z-36,
    //UpArrow-37, DownArrow-38, LeftArrow-39, RightArrow-40
    KeyCode GetTheInputedKey()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            curSetKeyIndex = 0;
            return KeyCode.Space;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            curSetKeyIndex = 1;
            return KeyCode.Alpha0;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            curSetKeyIndex = 2;
            return KeyCode.Alpha1;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            curSetKeyIndex = 3;
            return KeyCode.Alpha2;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            curSetKeyIndex = 4;
            return KeyCode.Alpha3;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            curSetKeyIndex = 5;
            return KeyCode.Alpha4;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            curSetKeyIndex = 6;
            return KeyCode.Alpha5;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            curSetKeyIndex = 7;
            return KeyCode.Alpha6;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            curSetKeyIndex = 8;
            return KeyCode.Alpha7;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            curSetKeyIndex = 9;
            return KeyCode.Alpha8;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            curSetKeyIndex = 10;
            return KeyCode.Alpha9;
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            curSetKeyIndex = 11;
            return KeyCode.A;
        }
        else if (Input.GetKeyDown(KeyCode.B))
        {
            curSetKeyIndex = 12;
            return KeyCode.B;
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            curSetKeyIndex = 13;
            return KeyCode.C;
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            curSetKeyIndex = 14;
            return KeyCode.D;
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            curSetKeyIndex = 15;
            return KeyCode.E;
        }
        else if (Input.GetKeyDown(KeyCode.F))
        {
            curSetKeyIndex = 16;
            return KeyCode.F;
        }
        else if (Input.GetKeyDown(KeyCode.G))
        {
            curSetKeyIndex = 17;
            return KeyCode.G;
        }
        else if (Input.GetKeyDown(KeyCode.H))
        {
            curSetKeyIndex = 18;
            return KeyCode.H;
        }
        else if (Input.GetKeyDown(KeyCode.I))
        {
            curSetKeyIndex = 19;
            return KeyCode.I;
        }
        else if (Input.GetKeyDown(KeyCode.J))
        {
            curSetKeyIndex = 20;
            return KeyCode.J;
        }
        else if (Input.GetKeyDown(KeyCode.K))
        {
            curSetKeyIndex = 21;
            return KeyCode.K;
        }
        else if (Input.GetKeyDown(KeyCode.L))
        {
            curSetKeyIndex = 22;
            return KeyCode.L;
        }
        else if (Input.GetKeyDown(KeyCode.M))
        {
            curSetKeyIndex = 23;
            return KeyCode.M;
        }
        else if (Input.GetKeyDown(KeyCode.N))
        {
            curSetKeyIndex = 24;
            return KeyCode.N;
        }
        else if (Input.GetKeyDown(KeyCode.O))
        {
            curSetKeyIndex = 25;
            return KeyCode.O;
        }
        else if (Input.GetKeyDown(KeyCode.P))
        {
            curSetKeyIndex = 26;
            return KeyCode.P;
        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            curSetKeyIndex = 27;
            return KeyCode.Q;
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            curSetKeyIndex = 28;
            return KeyCode.R;
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            curSetKeyIndex = 29;
            return KeyCode.S;
        }
        else if (Input.GetKeyDown(KeyCode.T))
        {
            curSetKeyIndex = 30;
            return KeyCode.T;
        }
        else if (Input.GetKeyDown(KeyCode.U))
        {
            curSetKeyIndex = 31;
            return KeyCode.U;
        }
        else if (Input.GetKeyDown(KeyCode.V))
        {
            curSetKeyIndex = 32;
            return KeyCode.V;
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            curSetKeyIndex = 33;
            return KeyCode.W;
        }
        else if (Input.GetKeyDown(KeyCode.X))
        {
            curSetKeyIndex = 34;
            return KeyCode.X;
        }
        else if (Input.GetKeyDown(KeyCode.Y))
        {
            curSetKeyIndex = 35;
            return KeyCode.Y;
        }
        else if (Input.GetKeyDown(KeyCode.Z))
        {
            curSetKeyIndex = 36;
            return KeyCode.Z;
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            curSetKeyIndex = 37;
            return KeyCode.UpArrow;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            curSetKeyIndex = 38;
            return KeyCode.DownArrow;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            curSetKeyIndex = 39;
            return KeyCode.LeftArrow;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            curSetKeyIndex = 40;
            return KeyCode.RightArrow;
        }
        else
            return KeyCode.None;
    }
}
