using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Video;

[DefaultExecutionOrder((int)ScriptsExecutionOrder.ExecutionOrder.optionsManager)]
public class OptionsManager : MonoBehaviour
{
    Constants CONS;
    Variables VARS;
    UniversalFunctionsLibrary UFL;
    ScriptsExecutionController SEC;

    GameObject gameManager;

    //bool isOptionPanelActivated;
    bool isInOptions;
    int curOptionIndex;

    bool isInSettingsSub;
    int curSettingsSubIndex;

    bool isInKeySetSubSub;
    int curKeySetSubSubIndex;
    bool isSettingAKey;
    int curSetKeyIndex;

    bool isInSoundSubSub;
    int curSoundSubSubIndex;

    bool isInLanguageSubSub;
    int curLanguageSubSubIndex;

    bool isInNewGameSub;
    int curNewGameSubIndex;

    bool isInFragmentsSub;

    bool isFromOptionsToKeySetSubSub;
    bool isFromKeySetSubSubToOptions;

    bool isFromOptionsToNewGameSub;
    bool isFromNewGameSubToOptions;

    bool isFromOptionsToFragmentsSub;
    bool isFromFragmentsSubToOptions;

    Transform tempTransform;
    KeyCode tempKeyCode;

    #region ConstantsUsed
    GameObject optionsPanel;

    //GameObject optionsEmpty;
    //List<GameObject> optionEmpties = new List<GameObject>();

    //GameObject keySetSubEmpty;
    //List<GameObject> keySetSubEmpties = new List<GameObject>();

    List<KeyCode> keyCodes = new List<KeyCode>();

    List<Sprite> keySprites = new List<Sprite>();
    List<Sprite> keyChosenSprites = new List<Sprite>();

    //GameObject newGameSubEmpty;
    //List<GameObject> newGameSubEmpties = new List<GameObject>();

    //GameObject fragmentsSubEmpty;
    //GameObject redFragmentSubEmpty;
    //GameObject yellowFragmentSubEmpty;
    //GameObject blueFragmentSubEmpty;
    //GameObject orangeFragmentSubEmpty;
    //GameObject greenFragmentSubEmpty;
    //GameObject purpleFragmentSubEmpty;

    Material optionsFragmentNotEmbeddedColor;
    Material optionsRedFragmentColor;
    Material optionsYellowFragmentColor;
    Material optionsBlueFragmentColor;
    Material optionsOrangeFragmentColor;
    Material optionsGreenFragmentColor;
    Material optionsPurpleFragmentColor;

    GameObject[] optionsPanelOverTextEmpties = new GameObject[2];

    float setMusicVolumeStep;
    float maxSetMusicVolume;
    float minSetMusicVolume;
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
        keyCodes = CONS.keyCodes;
        keySprites = CONS.keySprites;
        keyChosenSprites = CONS.keyChosenSprites;
        optionsFragmentNotEmbeddedColor = CONS.optionsFragmentNotEmbeddedColor;
        optionsRedFragmentColor = CONS.optionsRedFragmentColor;
        optionsYellowFragmentColor = CONS.optionsYellowFragmentColor;
        optionsBlueFragmentColor = CONS.optionsBlueFragmentColor;
        optionsOrangeFragmentColor = CONS.optionsOrangeFragmentColor;
        optionsGreenFragmentColor = CONS.optionsGreenFragmentColor;
        optionsPurpleFragmentColor = CONS.optionsPurpleFragmentColor;
        optionsPanelOverTextEmpties = CONS.optionsPanelOverTextEmpties;
        setMusicVolumeStep = CONS.setMusicVolumeStep;
        maxSetMusicVolume = CONS.maxSetMusicVolume;
        minSetMusicVolume = CONS.minSetMusicVolume;
        #endregion

        #region ImportReferenceVariables
        curKeyCodes = VARS.curKeyCodes;
        #endregion

        //setKeySetSubSubKeysAppearance
        for (int i = 0; i < curKeyCodes.Count; i++)
        {
            for (int j = 0; j < keyCodes.Count; j++)
            {
                for (int k = 0; k < optionsPanelOverTextEmpties.Length; k++)
                {
                    if (curKeyCodes[i] == keyCodes[j])
                    {
                        //keySetSubEmpties[i].transform.GetChild(2).GetComponent<SpriteRenderer>().sprite = keySprites[j];
                        //keySetSubEmpties[i].transform.GetChild(3).GetComponent<SpriteRenderer>().sprite = keyChosenSprites[j];
                        optionsPanelOverTextEmpties[k].transform.GetChild(1).GetChild(i).GetChild(2).GetComponent<SpriteRenderer>().sprite = keySprites[j];
                        optionsPanelOverTextEmpties[k].transform.GetChild(1).GetChild(i).GetChild(3).GetComponent<SpriteRenderer>().sprite = keyChosenSprites[j];

                        break;
                    }
                }
            }
        }
    }

    void Update()
    {
        #region ImportValueVariables
        #endregion

        //language
        for (int i = 0; i < optionsPanelOverTextEmpties.Length; i++)
        {
            optionsPanelOverTextEmpties[i].SetActive(i == VARS.CurLanguageIndex);
        }

        //activateOptionPanel
        if (VARS.IsOptionsManagerActivationExecutable)
        {
            if (!VARS.IsOptionPanelActivated &&
                VARS.IsBackKeyDown)
            {
                VARS.IsOptionPanelActivated = true;

                optionsPanel.SetActive(true);

                isInOptions = true;
                curOptionIndex = 0;
                isInSettingsSub = false;
                isInKeySetSubSub = false;
                isInSoundSubSub = false;
                isInLanguageSubSub = false;
                isInFragmentsSub = false;
                isInNewGameSub = false;

                VARS.IsBackKeyDown = false;
            }
        }

        //inOptionPanel
        if (VARS.IsOptionPanelActivated)
        {
            //setActive
            //options
            optionsPanelOverTextEmpties[VARS.CurLanguageIndex].transform.GetChild(0).gameObject.SetActive(isInOptions);
            //settingsSub
            optionsPanelOverTextEmpties[VARS.CurLanguageIndex].transform.GetChild(1).gameObject.SetActive(isInSettingsSub);
            //keySetSubSub
            optionsPanelOverTextEmpties[VARS.CurLanguageIndex].transform.GetChild(2).gameObject.SetActive(isInKeySetSubSub);
            //soundSubSub
            optionsPanelOverTextEmpties[VARS.CurLanguageIndex].transform.GetChild(3).gameObject.SetActive(isInSoundSubSub);
            //languageSubSub
            optionsPanelOverTextEmpties[VARS.CurLanguageIndex].transform.GetChild(4).gameObject.SetActive(isInLanguageSubSub);
            //fragmentsSub
            optionsPanelOverTextEmpties[VARS.CurLanguageIndex].transform.GetChild(5).gameObject.SetActive(isInFragmentsSub);
            //newGameSub
            optionsPanelOverTextEmpties[VARS.CurLanguageIndex].transform.GetChild(6).gameObject.SetActive(isInNewGameSub);

            #region Options
            if (/*!isInKeySetSubSub &&
                !isInFragmentsSub &&
                !isInNewGameSub*/
                isInOptions)
            {
                //chooseOptions
                if (VARS.IsDownKeyDown)
                {
                    curOptionIndex++;

                    if (curOptionIndex > /*optionEmpties.Count - 1*/ optionsPanelOverTextEmpties[VARS.CurLanguageIndex].transform.GetChild(0).childCount - 1)
                    {
                        curOptionIndex = /*optionEmpties.Count - 1*/ optionsPanelOverTextEmpties[VARS.CurLanguageIndex].transform.GetChild(0).childCount - 1;
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
                for (int i = 0; i < /*optionEmpties.Count*/ optionsPanelOverTextEmpties[VARS.CurLanguageIndex].transform.GetChild(0).childCount; i++)
                {
                    tempTransform = /*optionEmpties[i].transform*/ optionsPanelOverTextEmpties[VARS.CurLanguageIndex].transform.GetChild(0).GetChild(i);

                    tempTransform.GetChild(0).gameObject.SetActive(i != curOptionIndex);
                    tempTransform.GetChild(1).gameObject.SetActive(i == curOptionIndex);
                }

                //back
                if (VARS.IsBackKeyDown)
                {
                    VARS.IsOptionPanelActivated = false;

                    optionsPanel.SetActive(false);

                    isInOptions = false;

                    VARS.IsBackKeyDown = false;
                }

                //transfer
                if (VARS.IsSpaceDown || VARS.IsReturnDown)
                {
                    //outOfOption
                    isInOptions = false;
                    //curOptionIndex = 0;

                    //toSettingsSub
                    if (curOptionIndex == 0)
                    {
                        isInSettingsSub = true;
                        curSettingsSubIndex = 0;
                    }
                    //toFragmentsSub
                    else if (curOptionIndex == 1)
                    {
                        isInFragmentsSub = true;

                        DetermineFragmentsSubFragmentStates();
                    }
                    //toNewGamesSub
                    else if (curOptionIndex == 2)
                    {
                        isInNewGameSub = true;
                        curNewGameSubIndex = 0;
                    }
                    //exit
                    else if (curOptionIndex == 3)
                    {
                        VARS.IsWritingAllData = true;
                        VARS.IsExiting = true;
                    }

                    VARS.IsSpaceDown = false;
                    VARS.IsReturnDown = false;
                }
            }
            #endregion

            #region SettingsSub
            if (isInSettingsSub)
            {
                //chooseSettings
                if (VARS.IsDownKeyDown)
                {
                    curSettingsSubIndex++;

                    if (curSettingsSubIndex > optionsPanelOverTextEmpties[VARS.CurLanguageIndex].transform.GetChild(1).childCount - 1)
                    {
                        curSettingsSubIndex = optionsPanelOverTextEmpties[VARS.CurLanguageIndex].transform.GetChild(1).childCount - 1;
                    }
                }
                else if (VARS.IsUpKeyDown)
                {
                    curSettingsSubIndex--;

                    if (curSettingsSubIndex < 0)
                    {
                        curSettingsSubIndex = 0;
                    }
                }

                //highLightTheChosenSetting
                for (int i = 0; i < optionsPanelOverTextEmpties[VARS.CurLanguageIndex].transform.GetChild(1).childCount; i++)
                {
                    tempTransform = optionsPanelOverTextEmpties[VARS.CurLanguageIndex].transform.GetChild(1).GetChild(i);

                    tempTransform.GetChild(0).gameObject.SetActive(i != curSettingsSubIndex);
                    tempTransform.GetChild(1).gameObject.SetActive(i == curSettingsSubIndex);
                }

                //back
                if (VARS.IsBackKeyDown)
                {
                    //fromSettingsSubToOptions
                    isInSettingsSub = false;
                    isInOptions = true;

                    VARS.IsBackKeyDown = false;
                }

                //transfer
                if (VARS.IsSpaceDown || VARS.IsReturnDown)
                {
                    //outOfSettings
                    isInSettingsSub = false;

                    //toKeySet
                    if (curSettingsSubIndex == 0)
                    {
                        isInKeySetSubSub = true;
                        curKeySetSubSubIndex = 0;
                        isSettingAKey = false;
                        //curSetKeyIndex = 0;
                    }
                    //toSound
                    else if (curSettingsSubIndex == 1)
                    {
                        isInSoundSubSub = true;
                        curSoundSubSubIndex = 0;
                    }
                    //toLanguage
                    else if (curSettingsSubIndex == 2)
                    {
                        isInLanguageSubSub = true;
                        curLanguageSubSubIndex = 0;
                    }

                    VARS.IsSpaceDown = false;
                    VARS.IsReturnDown = false;
                }
            }
            #endregion

            #region KeySetSubSub
            //if (curOptionIndex == 0)
            //{
            //    //intoKeySetSubSub
            //    if (!isInKeySetSubSub &&
            //        /*(VARS.IsConfirmKeyDown ||
            //        VARS.IsSpaceDown)*/
            //        (VARS.IsSpaceDown || VARS.IsReturnDown))
            //    {
            //        isFromOptionsToKeySetSubSub = true;
            //    }
            //    if (isFromOptionsToKeySetSubSub)
            //    {
            //        isFromOptionsToKeySetSubSub = false;

            //        isInKeySetSubSub = true;

            //        //optionsEmpty.SetActive(false);
            //        //keySetSubEmpty.SetActive(true);
            //        optionsPanelOverTextEmpties[VARS.CurLanguageIndex].transform.GetChild(0).gameObject.SetActive(false);
            //        optionsPanelOverTextEmpties[VARS.CurLanguageIndex].transform.GetChild(1).gameObject.SetActive(true);

            //        //Input.ResetInputAxes();

            //        //VARS.IsConfirmKeyDown = false;
            //        VARS.IsSpaceDown = false;
            //        VARS.IsReturnDown = false;
            //    }

            if (isInKeySetSubSub)
            {
                //chooseKeys
                if (!isSettingAKey)
                {
                    if (VARS.IsDownKeyDown)
                    {
                        curKeySetSubSubIndex++;

                        if (curKeySetSubSubIndex > /*keySetSubEmpties.Count - 1*/ optionsPanelOverTextEmpties[VARS.CurLanguageIndex].transform.GetChild(2).childCount - 1)
                        {
                            curKeySetSubSubIndex = /*keySetSubEmpties.Count - 1*/ optionsPanelOverTextEmpties[VARS.CurLanguageIndex].transform.GetChild(2).childCount - 1;
                        }
                    }
                    else if (VARS.IsUpKeyDown)
                    {
                        curKeySetSubSubIndex--;

                        if (curKeySetSubSubIndex < 0)
                        {
                            curKeySetSubSubIndex = 0;
                        }
                    }
                }

                //highLightTheChosenKey
                for (int i = 0; i < /*keySetSubEmpties.Count*/ optionsPanelOverTextEmpties[VARS.CurLanguageIndex].transform.GetChild(2).childCount; i++)
                {
                    //tempTransform = keySetSubEmpties[i].transform;
                    tempTransform = optionsPanelOverTextEmpties[VARS.CurLanguageIndex].transform.GetChild(2).GetChild(i);

                    tempTransform.GetChild(0).gameObject.SetActive(i != curKeySetSubSubIndex || isSettingAKey);
                    tempTransform.GetChild(1).gameObject.SetActive(i == curKeySetSubSubIndex && !isSettingAKey);
                    if (tempTransform.childCount > 2)
                    {
                        tempTransform.GetChild(2).gameObject.SetActive(i != curKeySetSubSubIndex || !isSettingAKey);
                        tempTransform.GetChild(3).gameObject.SetActive(i == curKeySetSubSubIndex && isSettingAKey);
                    }
                }

                //toSetAKey
                if (!isSettingAKey &&
                    curKeySetSubSubIndex < /*keySetSubEmpties.Count - 1*/ optionsPanelOverTextEmpties[VARS.CurLanguageIndex].transform.GetChild(2).childCount - 1 &&
                    /*(VARS.IsConfirmKeyDown ||
                VARS.IsSpaceDown)*/
                    (VARS.IsSpaceDown || VARS.IsReturnDown))
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
                            curKeyCodes[curKeySetSubSubIndex] == tempKeyCode)
                        {
                            //logicChange
                            switch (curKeySetSubSubIndex)
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
                                    VARS.dashKeyCode = tempKeyCode;
                                    break;
                                case 6:
                                    VARS.minimapKeyCode = tempKeyCode;
                                    break;
                            }
                            curKeyCodes[curKeySetSubSubIndex] = tempKeyCode;

                            //appearanceChange
                            //keySetSubEmpties[curKeySetSubSubIndex].transform.GetChild(2).GetComponent<SpriteRenderer>().sprite = keySprites[curSetKeyIndex];
                            //keySetSubEmpties[curKeySetSubSubIndex].transform.GetChild(3).GetComponent<SpriteRenderer>().sprite = keyChosenSprites[curSetKeyIndex];
                            optionsPanelOverTextEmpties[VARS.CurLanguageIndex].transform.GetChild(2).GetChild(curKeySetSubSubIndex).GetChild(2).GetComponent<SpriteRenderer>().sprite = keySprites[curSetKeyIndex];
                            optionsPanelOverTextEmpties[VARS.CurLanguageIndex].transform.GetChild(2).GetChild(curKeySetSubSubIndex).GetChild(3).GetComponent<SpriteRenderer>().sprite = keyChosenSprites[curSetKeyIndex];

                            VARS.IsToWriteKeyCodesData = true;

                            VARS.IsKeyCodeChanged = true;

                            isSettingAKey = false;
                        }
                    }
                }

                //back
                if (VARS.IsBackKeyDown)
                {
                    //fromKeySetSubSubToSettings
                    isInKeySetSubSub = false;
                    isInSettingsSub = true;

                    VARS.IsBackKeyDown = false;
                }

                //ok
                if (curKeySetSubSubIndex == /*keySetSubEmpties.Count - 1*/ optionsPanelOverTextEmpties[VARS.CurLanguageIndex].transform.GetChild(2).childCount - 1 &&
                    /*(VARS.IsConfirmKeyDown || VARS.IsSpaceDown)*/(VARS.IsSpaceDown || VARS.IsReturnDown))
                {
                    //    isFromKeySetSubSubToOptions = true;
                    //}
                    //if (isFromKeySetSubSubToOptions)
                    //{
                    //    isFromKeySetSubSubToOptions = false;

                    //isInKeySetSubSub = false;

                    //keySetSubEmpty.SetActive(false);
                    //optionsEmpty.SetActive(true);
                    //optionsPanelOverTextEmpties[VARS.CurLanguageIndex].transform.GetChild(1).gameObject.SetActive(false);
                    //optionsPanelOverTextEmpties[VARS.CurLanguageIndex].transform.GetChild(0).gameObject.SetActive(true);

                    //curKeySetSubSubIndex = 0;

                    //isSettingAKey = false;

                    //fromKeySetSubSubToSettings
                    isInKeySetSubSub = false;
                    isInSettingsSub = true;

                    VARS.IsSpaceDown = false;
                    VARS.IsReturnDown = false;
                }
            }
            //}
            #endregion

            #region SoundSubSub
            if (isInSoundSubSub)
            {
                //adjustVolume
                if (VARS.IsUpKeyDown)
                {
                    VARS.curSetMusicVolumeFixFloat = Mathf.Min(maxSetMusicVolume, VARS.curSetMusicVolumeFixFloat + setMusicVolumeStep);

                    optionsPanelOverTextEmpties[VARS.CurLanguageIndex].transform.GetChild(3).GetChild(0).GetChild(1).GetComponent<SpriteRenderer>().color =
                        new Color(1, 1, 1, 0.2f);

                    VARS.IsToWriteSoundData = true;
                }
                else if (VARS.IsDownKeyDown)
                {
                    VARS.curSetMusicVolumeFixFloat = Mathf.Max(minSetMusicVolume, VARS.curSetMusicVolumeFixFloat - setMusicVolumeStep);

                    optionsPanelOverTextEmpties[VARS.CurLanguageIndex].transform.GetChild(3).GetChild(0).GetChild(2).GetComponent<SpriteRenderer>().color =
                        new Color(1, 1, 1, 0.2f);

                    VARS.IsToWriteSoundData = true;
                }

                //changeColor
                optionsPanelOverTextEmpties[VARS.CurLanguageIndex].transform.GetChild(3).GetChild(0).GetChild(0).GetComponent<SpriteRenderer>().color = 
                    new Color(1, 1, 1, 0.01f + (VARS.curSetMusicVolumeFixFloat - minSetMusicVolume) / (maxSetMusicVolume - minSetMusicVolume));
                optionsPanelOverTextEmpties[VARS.CurLanguageIndex].transform.GetChild(3).GetChild(0).GetChild(1).GetComponent<SpriteRenderer>().color =
                    new Color(1, 1, 1, Mathf.Min
                    (1, optionsPanelOverTextEmpties[VARS.CurLanguageIndex].transform.GetChild(3).GetChild(0).GetChild(1).GetComponent<SpriteRenderer>().color.a + 5 * Time.deltaTime));
                optionsPanelOverTextEmpties[VARS.CurLanguageIndex].transform.GetChild(3).GetChild(0).GetChild(2).GetComponent<SpriteRenderer>().color =
                    new Color(1, 1, 1, Mathf.Min
                    (1, optionsPanelOverTextEmpties[VARS.CurLanguageIndex].transform.GetChild(3).GetChild(0).GetChild(2).GetComponent<SpriteRenderer>().color.a + 5 * Time.deltaTime));

                //back
                if (VARS.IsBackKeyDown)
                {
                    //fromSoundSubSubToSettingsSub
                    isInSoundSubSub = false;
                    isInSettingsSub = true;

                    VARS.IsBackKeyDown = false;
                }
            }
            #endregion

            #region LanguageSubSub
            if (isInLanguageSubSub)
            {
                //switchLanguage
                if (VARS.IsSpaceDown || VARS.IsReturnDown)
                {
                    VARS.CurLanguageIndex++;

                    VARS.IsToWriteLanguageData = true;

                    VARS.IsSpaceDown = false;
                    VARS.IsReturnDown = false;
                }

                //back
                if (VARS.IsBackKeyDown)
                {
                    //fromLanguageSubSubToSettingsSub
                    isInLanguageSubSub = false;
                    isInSettingsSub = true;

                    VARS.IsBackKeyDown = false;
                }
            }
            #endregion

            #region FragmentsSub
            //if (curOptionIndex == 1)
            //{
            //    //intoFragmentSub
            //    if (!isInFragmentsSub &&
            //        (VARS.IsSpaceDown || VARS.IsReturnDown))
            //    {
            //        isFromOptionsToFragmentsSub = true;
            //    }
            //    if (isFromOptionsToFragmentsSub)
            //    {
            //        for (int i = 0; i < /*redFragmentSubEmpty.transform.childCount*/ optionsPanelOverTextEmpties[VARS.CurLanguageIndex].transform.GetChild(5).GetChild(0).childCount; i++)
            //        {
            //            //tempTransform = redFragmentSubEmpty.transform.GetChild(i);
            //            tempTransform = optionsPanelOverTextEmpties[VARS.CurLanguageIndex].transform.GetChild(5).GetChild(0).GetChild(i);
            //            if (VARS.isRedFragmentsEmbeded[i])
            //                tempTransform.GetComponent<MeshRenderer>().material = optionsRedFragmentColor;
            //            else
            //                tempTransform.GetComponent<MeshRenderer>().material = optionsFragmentNotEmbeddedColor;
            //        }
            //        for (int i = 0; i < /*yellowFragmentSubEmpty.transform.childCount*/ optionsPanelOverTextEmpties[VARS.CurLanguageIndex].transform.GetChild(5).GetChild(1).childCount; i++)
            //        {
            //            //tempTransform = yellowFragmentSubEmpty.transform.GetChild(i);
            //            tempTransform = optionsPanelOverTextEmpties[VARS.CurLanguageIndex].transform.GetChild(5).GetChild(1).GetChild(i);
            //            if (VARS.isYellowFragmentsEmbeded[i])
            //                tempTransform.GetComponent<MeshRenderer>().material = optionsYellowFragmentColor;
            //            else
            //                tempTransform.GetComponent<MeshRenderer>().material = optionsFragmentNotEmbeddedColor;
            //        }
            //        for (int i = 0; i < /*blueFragmentSubEmpty.transform.childCount*/ optionsPanelOverTextEmpties[VARS.CurLanguageIndex].transform.GetChild(5).GetChild(2).childCount; i++)
            //        {
            //            //tempTransform = blueFragmentSubEmpty.transform.GetChild(i);
            //            tempTransform = optionsPanelOverTextEmpties[VARS.CurLanguageIndex].transform.GetChild(5).GetChild(2).GetChild(i);
            //            if (VARS.isBlueFragmentsEmbeded[i])
            //                tempTransform.GetComponent<MeshRenderer>().material = optionsBlueFragmentColor;
            //            else
            //                tempTransform.GetComponent<MeshRenderer>().material = optionsFragmentNotEmbeddedColor;
            //        }
            //        for (int i = 0; i < /*orangeFragmentSubEmpty.transform.childCount*/ optionsPanelOverTextEmpties[VARS.CurLanguageIndex].transform.GetChild(5).GetChild(3).childCount; i++)
            //        {
            //            //tempTransform = orangeFragmentSubEmpty.transform.GetChild(i);
            //            tempTransform = optionsPanelOverTextEmpties[VARS.CurLanguageIndex].transform.GetChild(5).GetChild(3).GetChild(i);
            //            if (VARS.isOrangeFragmentsEmbeded[i])
            //                tempTransform.GetComponent<MeshRenderer>().material = optionsOrangeFragmentColor;
            //            else
            //                tempTransform.GetComponent<MeshRenderer>().material = optionsFragmentNotEmbeddedColor;
            //        }
            //        for (int i = 0; i < /*greenFragmentSubEmpty.transform.childCount*/ optionsPanelOverTextEmpties[VARS.CurLanguageIndex].transform.GetChild(5).GetChild(4).childCount; i++)
            //        {
            //            //tempTransform = greenFragmentSubEmpty.transform.GetChild(i);
            //            tempTransform = optionsPanelOverTextEmpties[VARS.CurLanguageIndex].transform.GetChild(5).GetChild(4).GetChild(i);
            //            if (VARS.isGreenFragmentsEmbeded[i])
            //                tempTransform.GetComponent<MeshRenderer>().material = optionsGreenFragmentColor;
            //            else
            //                tempTransform.GetComponent<MeshRenderer>().material = optionsFragmentNotEmbeddedColor;
            //        }
            //        for (int i = 0; i < /*purpleFragmentSubEmpty.transform.childCount*/ optionsPanelOverTextEmpties[VARS.CurLanguageIndex].transform.GetChild(5).GetChild(5).childCount; i++)
            //        {
            //            //tempTransform = purpleFragmentSubEmpty.transform.GetChild(i);
            //            tempTransform = optionsPanelOverTextEmpties[VARS.CurLanguageIndex].transform.GetChild(5).GetChild(5).GetChild(i);
            //            if (VARS.isPurpleFragmentsEmbeded[i])
            //                tempTransform.GetComponent<MeshRenderer>().material = optionsPurpleFragmentColor;
            //            else
            //                tempTransform.GetComponent<MeshRenderer>().material = optionsFragmentNotEmbeddedColor;
            //        }

            //        isFromOptionsToFragmentsSub = false;

            //        isInFragmentsSub = true;

            //        //optionsEmpty.SetActive(false);
            //        //fragmentsSubEmpty.SetActive(true);
            //        optionsPanelOverTextEmpties[VARS.CurLanguageIndex].transform.GetChild(0).gameObject.SetActive(false);
            //        optionsPanelOverTextEmpties[VARS.CurLanguageIndex].transform.GetChild(2).gameObject.SetActive(true);

            //        //Input.ResetInputAxes();

            //        //VARS.IsConfirmKeyDown = false;
            //        VARS.IsSpaceDown = false;
            //        VARS.IsReturnDown = false;
            //    }
            //}

            if (isInFragmentsSub)
            {
                //back
                if (VARS.IsBackKeyDown)
                {
                    //fromFragmentsSubToOptions
                    isInFragmentsSub = false;
                    isInOptions = true;

                    VARS.IsBackKeyDown = false;
                }

                //ok
                if (VARS.IsSpaceDown || VARS.IsReturnDown)
                {
                    //fromFragmentsSubToOptions
                    isInFragmentsSub = false;
                    isInOptions = true;

                    VARS.IsSpaceDown = false;
                    VARS.IsReturnDown= false;
                }

                //if ((VARS.IsSpaceDown || VARS.IsReturnDown) ||
                //    VARS.IsBackKeyDown)
                //{
                //    isFromFragmentsSubToOptions = true;
                //}
                //if (isFromFragmentsSubToOptions)
                //{
                //    isFromFragmentsSubToOptions = false;

                //    isInFragmentsSub = false;

                //    //fragmentsSubEmpty.SetActive(false);
                //    //optionsEmpty.SetActive(true);
                //    optionsPanelOverTextEmpties[VARS.CurLanguageIndex].transform.GetChild(2).gameObject.SetActive(false);
                //    optionsPanelOverTextEmpties[VARS.CurLanguageIndex].transform.GetChild(0).gameObject.SetActive(true);
                //}
            }
            #endregion

            #region NewGameSub
            //if (curOptionIndex == 2)
            //{
            //    //if (VARS.IsConfirmKeyDown ||
            //    //    VARS.IsSpaceDown)
            //    //if(VARS.IsSpaceDown || VARS.IsReturnDown)
            //    //{
            //    //    optionsPanel.SetActive(false);
            //    //    VARS.IsOptionPanelActivated = false;

            //    //    VARS.IsToStartNewGame = true;
            //    //    VARS.IsToDie = true;
            //    //}
            //    if (!isInNewGameSub &&
            //        (VARS.IsSpaceDown || VARS.IsReturnDown))
            //    {
            //        isFromOptionsToNewGameSub = true;
            //    }
            //    if (isFromOptionsToNewGameSub)
            //    {
            //        isFromOptionsToNewGameSub = false;

            //        isInNewGameSub = true;

            //        //optionsEmpty.SetActive(false);
            //        //newGameSubEmpty.SetActive(true);
            //        optionsPanelOverTextEmpties[VARS.CurLanguageIndex].transform.GetChild(0).gameObject.SetActive(false);
            //        optionsPanelOverTextEmpties[VARS.CurLanguageIndex].transform.GetChild(3).gameObject.SetActive(true);

            //        //Input.ResetInputAxes();

            //        //VARS.IsConfirmKeyDown = false;
            //        VARS.IsSpaceDown = false;
            //        VARS.IsReturnDown = false;
            //    }

            if (isInNewGameSub)
            {
                //yesOrNo
                if (VARS.IsDownKeyDown)
                {
                    curNewGameSubIndex++;

                    if (curNewGameSubIndex > 1)
                    {
                        curNewGameSubIndex = 1;
                    }
                }
                else if (VARS.IsUpKeyDown)
                {
                    curNewGameSubIndex--;

                    if (curNewGameSubIndex < 0)
                    {
                        curNewGameSubIndex = 0;
                    }
                }

                //highLightTheChosenOne
                for (int i = 0; i < /*newGameSubEmpties.Count*/ optionsPanelOverTextEmpties[VARS.CurLanguageIndex].transform.GetChild(6).childCount - 1; i++)
                {
                    //tempTransform = newGameSubEmpties[i].transform;
                    tempTransform = optionsPanelOverTextEmpties[VARS.CurLanguageIndex].transform.GetChild(6).GetChild(i + 1);

                    tempTransform.GetChild(0).gameObject.SetActive(i != curNewGameSubIndex);
                    tempTransform.GetChild(1).gameObject.SetActive(i == curNewGameSubIndex);
                }

                //back
                if (VARS.IsBackKeyDown)
                {
                    //fromNewGameToOptions
                    isInNewGameSub = false;
                    isInOptions = true;

                    VARS.IsBackKeyDown = false;
                }

                //newGameOrTransfer
                if (VARS.IsSpaceDown || VARS.IsReturnDown)
                {
                    //HTR
                    //yes
                    if (curNewGameSubIndex == 0)
                    {
                        //newGameSubEmpty.SetActive(false);
                        //optionsEmpty.SetActive(true);
                        optionsPanelOverTextEmpties[VARS.CurLanguageIndex].transform.GetChild(6).gameObject.SetActive(false);
                        optionsPanelOverTextEmpties[VARS.CurLanguageIndex].transform.GetChild(0).gameObject.SetActive(true);
                        isInNewGameSub = false;
                        optionsPanel.SetActive(false);
                        VARS.IsOptionPanelActivated = false;

                        VARS.IsToStartNewGame = true;
                        //VARS.IsToDie = true;
                    }

                    //no
                    if (curNewGameSubIndex == 1)
                    {
                        //    isFromNewGameSubToOptions = true;
                        //}
                        //if (isFromNewGameSubToOptions)
                        //{
                        //    isFromNewGameSubToOptions = false;

                        //isInNewGameSub = false;

                        ////newGameSubEmpty.SetActive(false);
                        ////optionsEmpty.SetActive(true);
                        //optionsPanelOverTextEmpties[VARS.CurLanguageIndex].transform.GetChild(3).gameObject.SetActive(false);
                        //optionsPanelOverTextEmpties[VARS.CurLanguageIndex].transform.GetChild(0).gameObject.SetActive(true);

                        //curNewGameSubIndex = 0;

                        isInNewGameSub = false;
                        isInOptions = true;

                        VARS.IsSpaceDown = false;
                        VARS.IsReturnDown = false;
                    }
                }
            }
            //}
            #endregion

            #region Exit
            //exit
            if (VARS.IsExiting &&
                !VARS.IsWritingAllData)
            {
                #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
                #endif

                Application.Quit();
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

    void DetermineFragmentsSubFragmentStates()
    {
        for (int i = 0; i < /*redFragmentSubEmpty.transform.childCount*/ optionsPanelOverTextEmpties[VARS.CurLanguageIndex].transform.GetChild(5).GetChild(0).childCount; i++)
        {
            //tempTransform = redFragmentSubEmpty.transform.GetChild(i);
            tempTransform = optionsPanelOverTextEmpties[VARS.CurLanguageIndex].transform.GetChild(5).GetChild(0).GetChild(i);
            if (VARS.isRedFragmentsEmbeded[i])
                tempTransform.GetComponent<MeshRenderer>().material = optionsRedFragmentColor;
            else
                tempTransform.GetComponent<MeshRenderer>().material = optionsFragmentNotEmbeddedColor;
        }
        for (int i = 0; i < /*yellowFragmentSubEmpty.transform.childCount*/ optionsPanelOverTextEmpties[VARS.CurLanguageIndex].transform.GetChild(5).GetChild(1).childCount; i++)
        {
            //tempTransform = yellowFragmentSubEmpty.transform.GetChild(i);
            tempTransform = optionsPanelOverTextEmpties[VARS.CurLanguageIndex].transform.GetChild(5).GetChild(1).GetChild(i);
            if (VARS.isYellowFragmentsEmbeded[i])
                tempTransform.GetComponent<MeshRenderer>().material = optionsYellowFragmentColor;
            else
                tempTransform.GetComponent<MeshRenderer>().material = optionsFragmentNotEmbeddedColor;
        }
        for (int i = 0; i < /*blueFragmentSubEmpty.transform.childCount*/ optionsPanelOverTextEmpties[VARS.CurLanguageIndex].transform.GetChild(5).GetChild(2).childCount; i++)
        {
            //tempTransform = blueFragmentSubEmpty.transform.GetChild(i);
            tempTransform = optionsPanelOverTextEmpties[VARS.CurLanguageIndex].transform.GetChild(5).GetChild(2).GetChild(i);
            if (VARS.isBlueFragmentsEmbeded[i])
                tempTransform.GetComponent<MeshRenderer>().material = optionsBlueFragmentColor;
            else
                tempTransform.GetComponent<MeshRenderer>().material = optionsFragmentNotEmbeddedColor;
        }
        for (int i = 0; i < /*orangeFragmentSubEmpty.transform.childCount*/ optionsPanelOverTextEmpties[VARS.CurLanguageIndex].transform.GetChild(5).GetChild(3).childCount; i++)
        {
            //tempTransform = orangeFragmentSubEmpty.transform.GetChild(i);
            tempTransform = optionsPanelOverTextEmpties[VARS.CurLanguageIndex].transform.GetChild(5).GetChild(3).GetChild(i);
            if (VARS.isOrangeFragmentsEmbeded[i])
                tempTransform.GetComponent<MeshRenderer>().material = optionsOrangeFragmentColor;
            else
                tempTransform.GetComponent<MeshRenderer>().material = optionsFragmentNotEmbeddedColor;
        }
        for (int i = 0; i < /*greenFragmentSubEmpty.transform.childCount*/ optionsPanelOverTextEmpties[VARS.CurLanguageIndex].transform.GetChild(5).GetChild(4).childCount; i++)
        {
            //tempTransform = greenFragmentSubEmpty.transform.GetChild(i);
            tempTransform = optionsPanelOverTextEmpties[VARS.CurLanguageIndex].transform.GetChild(5).GetChild(4).GetChild(i);
            if (VARS.isGreenFragmentsEmbeded[i])
                tempTransform.GetComponent<MeshRenderer>().material = optionsGreenFragmentColor;
            else
                tempTransform.GetComponent<MeshRenderer>().material = optionsFragmentNotEmbeddedColor;
        }
        for (int i = 0; i < /*purpleFragmentSubEmpty.transform.childCount*/ optionsPanelOverTextEmpties[VARS.CurLanguageIndex].transform.GetChild(5).GetChild(5).childCount; i++)
        {
            //tempTransform = purpleFragmentSubEmpty.transform.GetChild(i);
            tempTransform = optionsPanelOverTextEmpties[VARS.CurLanguageIndex].transform.GetChild(5).GetChild(5).GetChild(i);
            if (VARS.isPurpleFragmentsEmbeded[i])
                tempTransform.GetComponent<MeshRenderer>().material = optionsPurpleFragmentColor;
            else
                tempTransform.GetComponent<MeshRenderer>().material = optionsFragmentNotEmbeddedColor;
        }
    }
}
