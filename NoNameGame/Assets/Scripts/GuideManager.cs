using System.Collections.Generic;
using Steamworks;
using UnityEngine;
using UnityEngine.Video;

[DefaultExecutionOrder((int)ScriptsExecutionOrder.ExecutionOrder.guideManager)]
public class GuideManager : MonoBehaviour
{
    Constants CONS;
    Variables VARS;
    UniversalFunctionsLibrary UFL;
    ScriptsExecutionController SEC;

    GameObject gameManager;

    #region ConstantsUsed
    Transform catTransform;

    GameObject catIniPositionPoint;

    //keyCodes
    List<KeyCode> keyCodes = new List<KeyCode>();

    //keySprites
    List<Sprite> keySprites = new List<Sprite>();
    List<Sprite> keyChosenSprites = new List<Sprite>();

    //GameObject keysGuideTextEmpty;
    ////public List<GameObject> keysGuideTexts = new List<GameObject>();
    //GameObject jumpGuideText;
    //GameObject dashGuideText;
    //GameObject intoMinimapGuideText;
    ////GameObject climbGuideText;
    //GameObject twistGuideText;
    //GameObject rotateGuideText;
    //GameObject backCenterGuideText;
    //GameObject outOfCenterTwistGuideText;
    //GameObject betweenCentersTransportGuideText;

    GameObject keysGuideMask;

    GameObject[] guideOverTextEmpties = new GameObject[2];
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
        catTransform = CONS.catTransform;
        catIniPositionPoint = CONS.catIniPositionPoint;
        keyCodes = CONS.keyCodes;
        keySprites = CONS.keySprites;
        keyChosenSprites = CONS.keyChosenSprites;
        keysGuideMask = CONS.keysGuideMask;
        guideOverTextEmpties = CONS.guideOverTextEmpties;
        #endregion

        #region ImportReferenceVariable
        #endregion
    }

    void Update()
    {
        #region ImportValueVariables
        #endregion

        //language
        for (int i = 0; i < guideOverTextEmpties.Length; i++)
        {
            guideOverTextEmpties[i].SetActive(i == VARS.CurLanguageIndex);
        }

        //keysGuideMask
        if (VARS.HasFinishedKeysGuide)
        {
            keysGuideMask.SetActive(false);
        }

        if (VARS.IsGuideManagerMainPartExecutable)
        {
            #region IntoGuide
            //rotateGuideTemporarilyOut
            if (!VARS.HasRotated &&
                !VARS.IsRotateEnabled)
            {
                //rotateGuideText.SetActive(false);
                guideOverTextEmpties[VARS.CurLanguageIndex].transform.GetChild(4).gameObject.SetActive(false);

                VARS.IsInRotateGuide = false;
            }
            //twistGuideTemporarilyOut
            if (!VARS.HasTwisted &&
                !VARS.IsInCenter)
            {
                //twistGuideText.SetActive(false);
                guideOverTextEmpties[VARS.CurLanguageIndex].transform.GetChild(5).gameObject.SetActive(false);

                VARS.IsInTwistGuide = false;
            }
            //outOfCenterTwistGuideTemporarilyOut
            if (!VARS.HasOutOfCenterTwisted &&
                (!VARS.isCenterFulfilled[VARS.curRoomIndex / 9] ||
                VARS.IsInCenter))
            {
                //outOfCenterTwistGuideText.SetActive(false);
                guideOverTextEmpties[VARS.CurLanguageIndex].transform.GetChild(7).gameObject.SetActive(false);

                VARS.IsInOutOfCenterTwistGuide = false;
            }
            //betweenCentersTransportGuideTemporarilyOut
            if (!VARS.HasBetweenCentersTransported &&
                !VARS.IsInCenter)
            {
                //betweenCentersTransportGuideText.SetActive(false);
                guideOverTextEmpties[VARS.CurLanguageIndex].transform.GetChild(8).gameObject.SetActive(false);

                VARS.IsInBetweenCentersTransportGuide = false;
            }

            if (!VARS.IsInGuide &&
                !VARS.IsInMinimap)
            {
                //keys
                if (!VARS.HasFinishedKeysGuide &&
                    !VARS.IsInKeysGuide)
                {
                    //keysGuideTextEmpty.SetActive(true);
                    guideOverTextEmpties[VARS.CurLanguageIndex].transform.GetChild(0).gameObject.SetActive(true);

                    VARS.IsInKeysGuide = true;
                }
                //jump
                if (!VARS.HasJumped &&
                    !VARS.IsInJumpGuide &&
                    VARS.HasFinishedKeysGuide)
                {
                    for (int i = 0; i < keyCodes.Count; i++)
                    {
                        if (keyCodes[i] == VARS.jumpKeyCode)
                        {
                            //jumpGuideText.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = keyChosenSprites[i];
                            guideOverTextEmpties[VARS.CurLanguageIndex].transform.GetChild(1).GetChild(0).GetComponent<SpriteRenderer>().sprite = keyChosenSprites[i];
                            break;
                        }
                    }

                    //jumpGuideText.SetActive(true);
                    guideOverTextEmpties[VARS.CurLanguageIndex].transform.GetChild(1).gameObject.SetActive(true);

                    VARS.curTargetEnergy = 0.1f;
                    VARS.curEnergy = 0.1f;

                    VARS.IsInJumpGuide = true;
                }
                //dash
                if (!VARS.HasDashed &&
                    !VARS.IsInDashGuide &&
                     VARS.HasJumped &&
                     Vector3.Distance(catTransform.position, catIniPositionPoint.transform.position) > 6)
                {
                    for (int i = 0; i < keyCodes.Count; i++)
                    {
                        if (keyCodes[i] == VARS.dashKeyCode)
                        {
                            //dashGuideText.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = keyChosenSprites[i];
                            guideOverTextEmpties[VARS.CurLanguageIndex].transform.GetChild(2).GetChild(0).GetComponent<SpriteRenderer>().sprite = keyChosenSprites[i];
                            break;
                        }
                    }

                    //dashGuideText.SetActive(true);
                    guideOverTextEmpties[VARS.CurLanguageIndex].transform.GetChild(2).gameObject.SetActive(true);

                    VARS.IsInDashGuide = true;
                }
                //intoMinimap
                if (!VARS.HasBeenIntoMinimap &&
                    !VARS.IsInIntoMinimapGuide &&
                    VARS.curRoomIndex != 2)
                {
                    VARS.IsMinimapActivated = true;

                    for (int i = 0; i < keyCodes.Count; i++)
                    {
                        if (keyCodes[i] == VARS.minimapKeyCode)
                        {
                            //intoMinimapGuideText.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = keyChosenSprites[i];
                            guideOverTextEmpties[VARS.CurLanguageIndex].transform.GetChild(3).GetChild(0).GetComponent<SpriteRenderer>().sprite = keyChosenSprites[i];
                            break;
                        }
                    }

                    //intoMinimapGuideText.SetActive(true);
                    guideOverTextEmpties[VARS.CurLanguageIndex].transform.GetChild(3).gameObject.SetActive(true);

                    VARS.IsInIntoMinimapGuide = true;
                }
                //rotate
                if (!VARS.HasRotated &&
                    !VARS.IsInRotateGuide &&
                    VARS.HasCollectedFragment &&
                    VARS.IsRotateEnabled)
                {
                    for (int i = 0; i < keyCodes.Count; i++)
                    {
                        if (keyCodes[i] == VARS.downKeyCode)
                        {
                            //rotateGuideText.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = keyChosenSprites[i];
                            guideOverTextEmpties[VARS.CurLanguageIndex].transform.GetChild(4).GetChild(1).GetComponent<SpriteRenderer>().sprite = keyChosenSprites[i];
                        }
                        else if (keyCodes[i] == VARS.rightKeyCode)
                        {
                            //rotateGuideText.transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = keyChosenSprites[i];
                            guideOverTextEmpties[VARS.CurLanguageIndex].transform.GetChild(4).GetChild(4).GetComponent<SpriteRenderer>().sprite = keyChosenSprites[i];
                        }
                    }

                    //rotateGuideText.SetActive(true);
                    guideOverTextEmpties[VARS.CurLanguageIndex].transform.GetChild(4).gameObject.SetActive(true);

                    VARS.IsInRotateGuide = true;
                }
                ////climb
                //twist
                if (!VARS.HasTwisted &&
                    !VARS.IsInTwistGuide &&
                    VARS.IsInCenter &&
                    UFL.IsInRoom(VARS.curRoomIndex, VARS.curLatestCenterSavePointPosition)
                    //Mathf.Abs(VARS.verCurSpeed) < 0.1f &&
                    //Mathf.Abs(VARS.horCurSpeed) < 1 &&
                    //Vector3.Magnitude(VARS.curLatestCenterSavePointPosition) > 1
                    )
                {
                    for (int i = 0; i < keyCodes.Count; i++)
                    {
                        if (keyCodes[i] == VARS.downKeyCode)
                        {
                            //twistGuideText.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = keyChosenSprites[i];
                            guideOverTextEmpties[VARS.CurLanguageIndex].transform.GetChild(5).GetChild(1).GetComponent<SpriteRenderer>().sprite = keyChosenSprites[i];
                        }
                        else if (keyCodes[i] == VARS.rightKeyCode)
                        {
                            //twistGuideText.transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = keyChosenSprites[i];
                            guideOverTextEmpties[VARS.CurLanguageIndex].transform.GetChild(5).GetChild(4).GetComponent<SpriteRenderer>().sprite = keyChosenSprites[i];
                        }
                    }

                    //twistGuideText.SetActive(true);
                    guideOverTextEmpties[VARS.CurLanguageIndex].transform.GetChild(5).gameObject.SetActive(true);

                    VARS.IsInTwistGuide = true;
                }
                //backCenter
                if (!VARS.HasBackCentered &&
                    !VARS.IsInBackCenterGuide &&
                    VARS.HasTwisted &&
                    !VARS.IsInCenter &&
                    Vector3.Magnitude(VARS.curLatestCenterSavePointPosition) > 1 &&
                    Vector3.Distance(catTransform.position, VARS.curLatestCenterSavePointPosition) > 6)
                {
                    for (int i = 0; i < keyCodes.Count; i++)
                    {
                        if (keyCodes[i] == VARS.upKeyCode)
                        {
                            //backCenterGuideText.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = keyChosenSprites[i];
                            //backCenterGuideText.transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = keyChosenSprites[i];
                            guideOverTextEmpties[VARS.CurLanguageIndex].transform.GetChild(6).GetChild(1).GetComponent<SpriteRenderer>().sprite = keyChosenSprites[i];
                            guideOverTextEmpties[VARS.CurLanguageIndex].transform.GetChild(6).GetChild(4).GetComponent<SpriteRenderer>().sprite = keyChosenSprites[i];
                            break;
                        }
                    }

                    //backCenterGuideText.SetActive(true);
                    guideOverTextEmpties[VARS.CurLanguageIndex].transform.GetChild(6).gameObject.SetActive(true);

                    VARS.IsInBackCenterGuide = true;
                }
                //outOfCenterTwist
                if (!VARS.HasOutOfCenterTwisted &&
                    !VARS.IsInOutOfCenterTwistGuide &&
                    VARS.isCenterFulfilled[VARS.curRoomIndex / 9] &&
                    !VARS.IsInCenter)
                {
                    for (int i = 0; i < keyCodes.Count; i++)
                    {
                        if (keyCodes[i] == VARS.downKeyCode)
                        {
                            //outOfCenterTwistGuideText.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = keyChosenSprites[i];
                            guideOverTextEmpties[VARS.CurLanguageIndex].transform.GetChild(7).GetChild(1).GetComponent<SpriteRenderer>().sprite = keyChosenSprites[i];
                        }
                        else if (keyCodes[i] == VARS.rightKeyCode)
                        {
                            //outOfCenterTwistGuideText.transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = keyChosenSprites[i];
                            guideOverTextEmpties[VARS.CurLanguageIndex].transform.GetChild(7).GetChild(4).GetComponent<SpriteRenderer>().sprite = keyChosenSprites[i];
                        }
                    }

                    //outOfCenterTwistGuideText.SetActive(true);
                    guideOverTextEmpties[VARS.CurLanguageIndex].transform.GetChild(7).gameObject.SetActive(true);

                    VARS.IsInOutOfCenterTwistGuide = true;
                }
                //betweenCentersTransport
                if (!VARS.HasBetweenCentersTransported &&
                    !VARS.IsInBetweenCentersTransportGuide &&
                    VARS.curAccessedCenterSavePointPositions.Count > 1 &&
                    VARS.IsInCenter)
                {
                    for (int i = 0; i < keyCodes.Count; i++)
                    {
                        if (keyCodes[i] == VARS.upKeyCode)
                        {
                            //betweenCentersTransportGuideText.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = keyChosenSprites[i];
                            //betweenCentersTransportGuideText.transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = keyChosenSprites[i];
                            guideOverTextEmpties[VARS.CurLanguageIndex].transform.GetChild(8).GetChild(1).GetComponent<SpriteRenderer>().sprite = keyChosenSprites[i];
                            guideOverTextEmpties[VARS.CurLanguageIndex].transform.GetChild(8).GetChild(4).GetComponent<SpriteRenderer>().sprite = keyChosenSprites[i];
                            break;
                        }
                    }

                    //betweenCentersTransportGuideText.SetActive(true);
                    guideOverTextEmpties[VARS.CurLanguageIndex].transform.GetChild(8).gameObject.SetActive(true);

                    VARS.IsInBetweenCentersTransportGuide = true;
                }
            }
            #endregion

            #region InGuide
            //keysGuide
            if (VARS.IsInKeysGuide)
            {
                //left
                if (VARS.curKeysGuideIndex == 0)
                {
                    if (VARS.IsLeftKeyDown)
                    {
                        //keysGuideTextEmpty.transform.GetChild(1).GetChild(VARS.curKeysGuideIndex).GetChild(0).gameObject.SetActive(true);
                        //keysGuideTextEmpty.transform.GetChild(1).GetChild(VARS.curKeysGuideIndex).GetChild(1).gameObject.SetActive(false);
                        //keysGuideTextEmpty.transform.GetChild(1).GetChild(VARS.curKeysGuideIndex).GetChild(2).gameObject.SetActive(false);
                        guideOverTextEmpties[VARS.CurLanguageIndex].transform.GetChild(0).GetChild(1).GetChild(VARS.curKeysGuideIndex).GetChild(0).gameObject.SetActive(true);
                        guideOverTextEmpties[VARS.CurLanguageIndex].transform.GetChild(0).GetChild(1).GetChild(VARS.curKeysGuideIndex).GetChild(1).gameObject.SetActive(false);
                        guideOverTextEmpties[VARS.CurLanguageIndex].transform.GetChild(0).GetChild(1).GetChild(VARS.curKeysGuideIndex).GetChild(2).gameObject.SetActive(false);

                        VARS.curKeysGuideIndex++;

                        //keysGuideTextEmpty.transform.GetChild(1).GetChild(VARS.curKeysGuideIndex).GetChild(0).gameObject.SetActive(false);
                        //keysGuideTextEmpty.transform.GetChild(1).GetChild(VARS.curKeysGuideIndex).GetChild(1).gameObject.SetActive(true);
                        //keysGuideTextEmpty.transform.GetChild(1).GetChild(VARS.curKeysGuideIndex).GetChild(2).gameObject.SetActive(true);
                        guideOverTextEmpties[VARS.CurLanguageIndex].transform.GetChild(0).GetChild(1).GetChild(VARS.curKeysGuideIndex).GetChild(0).gameObject.SetActive(false);
                        guideOverTextEmpties[VARS.CurLanguageIndex].transform.GetChild(0).GetChild(1).GetChild(VARS.curKeysGuideIndex).GetChild(1).gameObject.SetActive(true);
                        guideOverTextEmpties[VARS.CurLanguageIndex].transform.GetChild(0).GetChild(1).GetChild(VARS.curKeysGuideIndex).GetChild(2).gameObject.SetActive(true);
                    }
                }
                //right
                else if (VARS.curKeysGuideIndex == 1)
                {
                    if (VARS.IsRightKeyDown)
                    {
                        //keysGuideTextEmpty.transform.GetChild(1).GetChild(VARS.curKeysGuideIndex).GetChild(0).gameObject.SetActive(true);
                        //keysGuideTextEmpty.transform.GetChild(1).GetChild(VARS.curKeysGuideIndex).GetChild(1).gameObject.SetActive(false);
                        //keysGuideTextEmpty.transform.GetChild(1).GetChild(VARS.curKeysGuideIndex).GetChild(2).gameObject.SetActive(false);
                        guideOverTextEmpties[VARS.CurLanguageIndex].transform.GetChild(0).GetChild(1).GetChild(VARS.curKeysGuideIndex).GetChild(0).gameObject.SetActive(true);
                        guideOverTextEmpties[VARS.CurLanguageIndex].transform.GetChild(0).GetChild(1).GetChild(VARS.curKeysGuideIndex).GetChild(1).gameObject.SetActive(false);
                        guideOverTextEmpties[VARS.CurLanguageIndex].transform.GetChild(0).GetChild(1).GetChild(VARS.curKeysGuideIndex).GetChild(2).gameObject.SetActive(false);

                        VARS.curKeysGuideIndex++;

                        //keysGuideTextEmpty.transform.GetChild(1).GetChild(VARS.curKeysGuideIndex).GetChild(0).gameObject.SetActive(false);
                        //keysGuideTextEmpty.transform.GetChild(1).GetChild(VARS.curKeysGuideIndex).GetChild(1).gameObject.SetActive(true);
                        //keysGuideTextEmpty.transform.GetChild(1).GetChild(VARS.curKeysGuideIndex).GetChild(2).gameObject.SetActive(true);
                        guideOverTextEmpties[VARS.CurLanguageIndex].transform.GetChild(0).GetChild(1).GetChild(VARS.curKeysGuideIndex).GetChild(0).gameObject.SetActive(false);
                        guideOverTextEmpties[VARS.CurLanguageIndex].transform.GetChild(0).GetChild(1).GetChild(VARS.curKeysGuideIndex).GetChild(1).gameObject.SetActive(true);
                        guideOverTextEmpties[VARS.CurLanguageIndex].transform.GetChild(0).GetChild(1).GetChild(VARS.curKeysGuideIndex).GetChild(2).gameObject.SetActive(true);
                    }
                }
                //up
                else if (VARS.curKeysGuideIndex == 2)
                {
                    //keysGuideTextEmpty.transform.GetChild(1).GetChild(VARS.curKeysGuideIndex).GetChild(0).gameObject.SetActive(false);
                    //keysGuideTextEmpty.transform.GetChild(1).GetChild(VARS.curKeysGuideIndex).GetChild(1).gameObject.SetActive(true);
                    //keysGuideTextEmpty.transform.GetChild(1).GetChild(VARS.curKeysGuideIndex).GetChild(2).gameObject.SetActive(true);

                    if (VARS.IsUpKeyDown)
                    {
                        //keysGuideTextEmpty.transform.GetChild(1).GetChild(VARS.curKeysGuideIndex).GetChild(0).gameObject.SetActive(true);
                        //keysGuideTextEmpty.transform.GetChild(1).GetChild(VARS.curKeysGuideIndex).GetChild(1).gameObject.SetActive(false);
                        //keysGuideTextEmpty.transform.GetChild(1).GetChild(VARS.curKeysGuideIndex).GetChild(2).gameObject.SetActive(false);
                        guideOverTextEmpties[VARS.CurLanguageIndex].transform.GetChild(0).GetChild(1).GetChild(VARS.curKeysGuideIndex).GetChild(0).gameObject.SetActive(true);
                        guideOverTextEmpties[VARS.CurLanguageIndex].transform.GetChild(0).GetChild(1).GetChild(VARS.curKeysGuideIndex).GetChild(1).gameObject.SetActive(false);
                        guideOverTextEmpties[VARS.CurLanguageIndex].transform.GetChild(0).GetChild(1).GetChild(VARS.curKeysGuideIndex).GetChild(2).gameObject.SetActive(false);

                        VARS.curKeysGuideIndex++;

                        //keysGuideTextEmpty.transform.GetChild(1).GetChild(VARS.curKeysGuideIndex).GetChild(0).gameObject.SetActive(false);
                        //keysGuideTextEmpty.transform.GetChild(1).GetChild(VARS.curKeysGuideIndex).GetChild(1).gameObject.SetActive(true);
                        //keysGuideTextEmpty.transform.GetChild(1).GetChild(VARS.curKeysGuideIndex).GetChild(2).gameObject.SetActive(true);
                        guideOverTextEmpties[VARS.CurLanguageIndex].transform.GetChild(0).GetChild(1).GetChild(VARS.curKeysGuideIndex).GetChild(0).gameObject.SetActive(false);
                        guideOverTextEmpties[VARS.CurLanguageIndex].transform.GetChild(0).GetChild(1).GetChild(VARS.curKeysGuideIndex).GetChild(1).gameObject.SetActive(true);
                        guideOverTextEmpties[VARS.CurLanguageIndex].transform.GetChild(0).GetChild(1).GetChild(VARS.curKeysGuideIndex).GetChild(2).gameObject.SetActive(true);
                    }
                }
                //down
                else if (VARS.curKeysGuideIndex == 3)
                {
                    if (VARS.IsDownKeyDown)
                    {
                        //keysGuideTextEmpty.transform.GetChild(1).GetChild(VARS.curKeysGuideIndex).GetChild(0).gameObject.SetActive(true);
                        //keysGuideTextEmpty.transform.GetChild(1).GetChild(VARS.curKeysGuideIndex).GetChild(1).gameObject.SetActive(false);
                        //keysGuideTextEmpty.transform.GetChild(1).GetChild(VARS.curKeysGuideIndex).GetChild(2).gameObject.SetActive(false);
                        guideOverTextEmpties[VARS.CurLanguageIndex].transform.GetChild(0).GetChild(1).GetChild(VARS.curKeysGuideIndex).GetChild(0).gameObject.SetActive(true);
                        guideOverTextEmpties[VARS.CurLanguageIndex].transform.GetChild(0).GetChild(1).GetChild(VARS.curKeysGuideIndex).GetChild(1).gameObject.SetActive(false);
                        guideOverTextEmpties[VARS.CurLanguageIndex].transform.GetChild(0).GetChild(1).GetChild(VARS.curKeysGuideIndex).GetChild(2).gameObject.SetActive(false);

                        VARS.curKeysGuideIndex++;

                        //keysGuideTextEmpty.transform.GetChild(1).GetChild(VARS.curKeysGuideIndex).GetChild(0).gameObject.SetActive(false);
                        //keysGuideTextEmpty.transform.GetChild(1).GetChild(VARS.curKeysGuideIndex).GetChild(1).gameObject.SetActive(true);
                        //keysGuideTextEmpty.transform.GetChild(1).GetChild(VARS.curKeysGuideIndex).GetChild(2).gameObject.SetActive(true);
                        guideOverTextEmpties[VARS.CurLanguageIndex].transform.GetChild(0).GetChild(1).GetChild(VARS.curKeysGuideIndex).GetChild(0).gameObject.SetActive(false);
                        guideOverTextEmpties[VARS.CurLanguageIndex].transform.GetChild(0).GetChild(1).GetChild(VARS.curKeysGuideIndex).GetChild(1).gameObject.SetActive(true);
                        guideOverTextEmpties[VARS.CurLanguageIndex].transform.GetChild(0).GetChild(1).GetChild(VARS.curKeysGuideIndex).GetChild(2).gameObject.SetActive(true);
                    }
                }
                //jump
                else if (VARS.curKeysGuideIndex == 4)
                {
                    if (VARS.IsJumpKeyDown)
                    {
                        //keysGuideTextEmpty.transform.GetChild(1).GetChild(VARS.curKeysGuideIndex).GetChild(0).gameObject.SetActive(true);
                        //keysGuideTextEmpty.transform.GetChild(1).GetChild(VARS.curKeysGuideIndex).GetChild(1).gameObject.SetActive(false);
                        //keysGuideTextEmpty.transform.GetChild(1).GetChild(VARS.curKeysGuideIndex).GetChild(2).gameObject.SetActive(false);
                        guideOverTextEmpties[VARS.CurLanguageIndex].transform.GetChild(0).GetChild(1).GetChild(VARS.curKeysGuideIndex).GetChild(0).gameObject.SetActive(true);
                        guideOverTextEmpties[VARS.CurLanguageIndex].transform.GetChild(0).GetChild(1).GetChild(VARS.curKeysGuideIndex).GetChild(1).gameObject.SetActive(false);
                        guideOverTextEmpties[VARS.CurLanguageIndex].transform.GetChild(0).GetChild(1).GetChild(VARS.curKeysGuideIndex).GetChild(2).gameObject.SetActive(false);

                        VARS.curKeysGuideIndex++;

                        //keysGuideTextEmpty.transform.GetChild(1).GetChild(VARS.curKeysGuideIndex).GetChild(0).gameObject.SetActive(false);
                        //keysGuideTextEmpty.transform.GetChild(1).GetChild(VARS.curKeysGuideIndex).GetChild(1).gameObject.SetActive(true);
                        //keysGuideTextEmpty.transform.GetChild(1).GetChild(VARS.curKeysGuideIndex).GetChild(2).gameObject.SetActive(true);
                        guideOverTextEmpties[VARS.CurLanguageIndex].transform.GetChild(0).GetChild(1).GetChild(VARS.curKeysGuideIndex).GetChild(0).gameObject.SetActive(false);
                        guideOverTextEmpties[VARS.CurLanguageIndex].transform.GetChild(0).GetChild(1).GetChild(VARS.curKeysGuideIndex).GetChild(1).gameObject.SetActive(true);
                        guideOverTextEmpties[VARS.CurLanguageIndex].transform.GetChild(0).GetChild(1).GetChild(VARS.curKeysGuideIndex).GetChild(2).gameObject.SetActive(true);
                    }
                }
                //dash
                else if (VARS.curKeysGuideIndex == 5)
                {
                    if (VARS.IsDashKeyDown)
                    {
                        //keysGuideTextEmpty.transform.GetChild(1).GetChild(VARS.curKeysGuideIndex).GetChild(0).gameObject.SetActive(true);
                        //keysGuideTextEmpty.transform.GetChild(1).GetChild(VARS.curKeysGuideIndex).GetChild(1).gameObject.SetActive(false);
                        //keysGuideTextEmpty.transform.GetChild(1).GetChild(VARS.curKeysGuideIndex).GetChild(2).gameObject.SetActive(false);
                        guideOverTextEmpties[VARS.CurLanguageIndex].transform.GetChild(0).GetChild(1).GetChild(VARS.curKeysGuideIndex).GetChild(0).gameObject.SetActive(true);
                        guideOverTextEmpties[VARS.CurLanguageIndex].transform.GetChild(0).GetChild(1).GetChild(VARS.curKeysGuideIndex).GetChild(1).gameObject.SetActive(false);
                        guideOverTextEmpties[VARS.CurLanguageIndex].transform.GetChild(0).GetChild(1).GetChild(VARS.curKeysGuideIndex).GetChild(2).gameObject.SetActive(false);

                        VARS.curKeysGuideIndex++;

                        //keysGuideTextEmpty.transform.GetChild(1).GetChild(VARS.curKeysGuideIndex).GetChild(0).gameObject.SetActive(false);
                        //keysGuideTextEmpty.transform.GetChild(1).GetChild(VARS.curKeysGuideIndex).GetChild(1).gameObject.SetActive(true);
                        //keysGuideTextEmpty.transform.GetChild(1).GetChild(VARS.curKeysGuideIndex).GetChild(2).gameObject.SetActive(true);
                        guideOverTextEmpties[VARS.CurLanguageIndex].transform.GetChild(0).GetChild(1).GetChild(VARS.curKeysGuideIndex).GetChild(0).gameObject.SetActive(false);
                        guideOverTextEmpties[VARS.CurLanguageIndex].transform.GetChild(0).GetChild(1).GetChild(VARS.curKeysGuideIndex).GetChild(1).gameObject.SetActive(true);
                        guideOverTextEmpties[VARS.CurLanguageIndex].transform.GetChild(0).GetChild(1).GetChild(VARS.curKeysGuideIndex).GetChild(2).gameObject.SetActive(true);
                    }
                }
                //Minimap
                else if (VARS.curKeysGuideIndex == 6)
                {
                    if (VARS.IsMinimapKeyDown)
                    {
                        Debug.Log("keysGuideOver");

                        //keysGuideTextEmpty.transform.GetChild(1).GetChild(VARS.curKeysGuideIndex).GetChild(0).gameObject.SetActive(true);
                        //keysGuideTextEmpty.transform.GetChild(1).GetChild(VARS.curKeysGuideIndex).GetChild(1).gameObject.SetActive(false);
                        //keysGuideTextEmpty.transform.GetChild(1).GetChild(VARS.curKeysGuideIndex).GetChild(2).gameObject.SetActive(false);
                        guideOverTextEmpties[VARS.CurLanguageIndex].transform.GetChild(0).GetChild(1).GetChild(VARS.curKeysGuideIndex).GetChild(0).gameObject.SetActive(true);
                        guideOverTextEmpties[VARS.CurLanguageIndex].transform.GetChild(0).GetChild(1).GetChild(VARS.curKeysGuideIndex).GetChild(1).gameObject.SetActive(false);
                        guideOverTextEmpties[VARS.CurLanguageIndex].transform.GetChild(0).GetChild(1).GetChild(VARS.curKeysGuideIndex).GetChild(2).gameObject.SetActive(false);

                        VARS.curKeysGuideIndex = 0;

                        //keysGuideTextEmpty.SetActive(false);
                        guideOverTextEmpties[VARS.CurLanguageIndex].transform.GetChild(0).gameObject.SetActive(false);

                        VARS.curTargetEnergy = 0.1f;
                        VARS.curEnergy = 0.1f;

                        VARS.HasFinishedKeysGuide = true;

                        //VARS.IsToWriteProgressData = true;
                        VARS.IsToWriteGuideData = true;

                        VARS.IsInKeysGuide = false;
                    }
                }
                ////grab
                //else if (VARS.curKeysGuideIndex == 7)
                //{
                //    if (VARS.IsGrabKeyDown)
                //    {
                //        keysGuideTextEmpty.transform.GetChild(1).GetChild(VARS.curKeysGuideIndex).GetChild(0).gameObject.SetActive(true);
                //        keysGuideTextEmpty.transform.GetChild(1).GetChild(VARS.curKeysGuideIndex).GetChild(1).gameObject.SetActive(false);
                //        keysGuideTextEmpty.transform.GetChild(1).GetChild(VARS.curKeysGuideIndex).GetChild(2).gameObject.SetActive(false);

                //        VARS.curKeysGuideIndex++;

                //        keysGuideTextEmpty.transform.GetChild(1).GetChild(VARS.curKeysGuideIndex).GetChild(0).gameObject.SetActive(false);
                //        keysGuideTextEmpty.transform.GetChild(1).GetChild(VARS.curKeysGuideIndex).GetChild(1).gameObject.SetActive(true);
                //        keysGuideTextEmpty.transform.GetChild(1).GetChild(VARS.curKeysGuideIndex).GetChild(2).gameObject.SetActive(true);
                //    }
                //}
            }
            //jump
            if (VARS.IsInJumpGuide &&
                VARS.IsInputtingJumpKey)
            {
                Debug.Log("jumpGuideOver");

                //jumpGuideText.SetActive(false);
                guideOverTextEmpties[VARS.CurLanguageIndex].transform.GetChild(1).gameObject.SetActive(false);

                VARS.HasJumped = true;

                //VARS.IsToWriteProgressData = true;
                VARS.IsToWriteGuideData = true;

                VARS.IsInJumpGuide = false;
            }
            //dash
            if (VARS.IsInDashGuide &&
                VARS.IsDashing)
            {
                Debug.Log("dashGuideOver");

                //dashGuideText.SetActive(false);
                guideOverTextEmpties[VARS.CurLanguageIndex].transform.GetChild(2).gameObject.SetActive(false);

                VARS.HasDashed = true;

                //VARS.IsToWriteProgressData = true;
                VARS.IsToWriteGuideData = true;

                VARS.IsInDashGuide = false;
            }
            //intoMinimap
            if (VARS.IsInIntoMinimapGuide &&
                VARS.IsMinimapKeyDown)
            {
                Debug.Log("intoMinimapGuideOver");

                //intoMinimapGuideText.SetActive(false);
                guideOverTextEmpties[VARS.CurLanguageIndex].transform.GetChild(3).gameObject.SetActive(false);

                VARS.HasBeenIntoMinimap = true;

                //VARS.IsToWriteProgressData = true;
                VARS.IsToWriteGuideData = true;

                VARS.IsInIntoMinimapGuide = false;
            }
            //rotate
            if (VARS.IsInRotateGuide &&
                VARS.IsInputtingDownKey &&
                (VARS.IsInputtingLeftKey || VARS.IsInputtingRightKey))
            {
                Debug.Log("rotateGuideOver");

                //rotateGuideText.SetActive(false);
                guideOverTextEmpties[VARS.CurLanguageIndex].transform.GetChild(4).gameObject.SetActive(false);

                VARS.HasRotated = true;

                //VARS.IsToWriteProgressData= true;
                VARS.IsToWriteGuideData = true;

                VARS.IsInRotateGuide = false;
            }
            ////climb
            //twist
            if (VARS.IsInTwistGuide &&
                VARS.IsInputtingDownKey &&
                (VARS.IsInputtingLeftKey || VARS.IsInputtingRightKey))
            {
                Debug.Log("twistGuideOver");

                //twistGuideText.SetActive(false);
                guideOverTextEmpties[VARS.CurLanguageIndex].transform.GetChild(5).gameObject.SetActive(false);

                VARS.HasTwisted = true;

                //VARS.IsToWriteProgressData = true;
                VARS.IsToWriteGuideData = true;

                VARS.IsInTwistGuide = false;
            }
            //backCenter
            if (VARS.IsInBackCenterGuide &&
                VARS.IsBackCenterTriggered)
            {
                Debug.Log("backCenterGuideOver");

                //backCenterGuideText.SetActive(false);
                guideOverTextEmpties[VARS.CurLanguageIndex].transform.GetChild(6).gameObject.SetActive(false);

                VARS.HasBackCentered = true;

                //VARS.IsToWriteProgressData = true;
                VARS.IsToWriteGuideData = true;

                VARS.IsInBackCenterGuide = false;
            }
            //outOfCenterTwist
            if (VARS.IsInOutOfCenterTwistGuide &&
                VARS.IsInputtingDownKey &&
                (VARS.IsInputtingLeftKey || VARS.IsInputtingRightKey))
            {
                Debug.Log("outOfCenterTwistGuideOver");

                //outOfCenterTwistGuideText.SetActive(false);
                guideOverTextEmpties[VARS.CurLanguageIndex].transform.GetChild(7).gameObject.SetActive(false);

                VARS.HasOutOfCenterTwisted = true;

                //VARS.IsToWriteProgressData = true;
                VARS.IsToWriteGuideData = true;

                VARS.IsInOutOfCenterTwistGuide = false;
            }
            //betweenCentersTransport
            if (VARS.IsInBetweenCentersTransportGuide &&
                VARS.IsBackCenterTriggered)
            {
                Debug.Log("betweenCentersTransportGuideOver");

                //betweenCentersTransportGuideText.SetActive(false);
                guideOverTextEmpties[VARS.CurLanguageIndex].transform.GetChild(8).gameObject.SetActive(false);

                VARS.HasBetweenCentersTransported = true;

                //VARS.IsToWriteProgressData = true;
                VARS.IsToWriteGuideData = true;

                VARS.IsInBetweenCentersTransportGuide = false;
            }
            #endregion
        }
    }
}
