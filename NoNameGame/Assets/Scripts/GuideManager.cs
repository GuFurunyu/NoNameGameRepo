using System.Collections.Generic;
using UnityEngine;

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

    //keyCodes
    List<KeyCode> keyCodes = new List<KeyCode>();

    //keySprites
    List<Sprite> keySprites = new List<Sprite>();
    List<Sprite> keyChosenSprites = new List<Sprite>();

    GameObject keysGuideEmpty;
    //public List<GameObject> keysGuideTexts = new List<GameObject>();
    GameObject jumpGuideText;
    GameObject intoMinimapGuideText;
    GameObject climbGuideText;
    GameObject twistGuideText;
    GameObject rotateGuideText;
    GameObject backCenterGuideText;

    GameObject keysGuideMask;
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
        keyCodes = CONS.keyCodes;
        keySprites = CONS.keySprites;
        keyChosenSprites = CONS.keyChosenSprites;
        keysGuideEmpty = CONS.keysGuideEmpty;
        jumpGuideText = CONS.jumpGuideText;
        intoMinimapGuideText = CONS.intoMinimapGuideText;
        climbGuideText = CONS.climbGuideText;
        twistGuideText = CONS.twistGuideText;
        rotateGuideText = CONS.rotateGuideText;
        backCenterGuideText = CONS.backCenterGuideText;
        keysGuideMask = CONS.keysGuideMask;
        #endregion

        #region ImportReferenceVariable
        #endregion
    }

    void Update()
    {
        #region ImportValueVariables
        #endregion

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
                rotateGuideText.SetActive(false);

                VARS.IsInRotateGuide = false;
            }
            //twistGuideTemporarilyOut
            if (!VARS.HasTwisted &&
                !VARS.IsInCenter)
            {
                twistGuideText.SetActive(false);

                VARS.IsInTwistGuide = false;
            }

            if (!VARS.IsInGuide &&
                !VARS.IsInMinimap)
            {
                //keys
                if (!VARS.HasFinishedKeysGuide &&
                    !VARS.IsInKeysGuide)
                {
                    keysGuideEmpty.SetActive(true);

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
                            jumpGuideText.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = keyChosenSprites[i];
                            break;
                        }
                    }

                    jumpGuideText.SetActive(true);

                    VARS.curTargetEnergy = 0.1f;
                    VARS.curEnergy = 0.1f;

                    VARS.IsInJumpGuide = true;
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
                            intoMinimapGuideText.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = keyChosenSprites[i];
                            break;
                        }
                    }

                    intoMinimapGuideText.SetActive(true);

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
                            rotateGuideText.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = keyChosenSprites[i];
                        }
                        else if (keyCodes[i] == VARS.rightKeyCode)
                        {
                            rotateGuideText.transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = keyChosenSprites[i];
                        }
                    }

                    rotateGuideText.SetActive(true);

                    VARS.IsInRotateGuide = true;
                }
                ////climb
                //if (!VARS.HasClimbed &&
                //    !VARS.IsInClimbGuide &&
                //    VARS.IsRightBlocked &&
                //    !VARS.IsOnGround &&
                //    VARS.curRoomIndex > 2)
                //{

                //    for (int i = 0; i < keyCodes.Count; i++)
                //    {
                //        if (keyCodes[i] == VARS.rightKeyCode)
                //        {
                //            climbGuideText.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = keyChosenSprites[i];
                //        }
                //        else if (keyCodes[i] == VARS.grabKeyCode)
                //        {
                //            climbGuideText.transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = keyChosenSprites[i];
                //        }
                //        else if (keyCodes[i] == VARS.upKeyCode)
                //        {
                //            climbGuideText.transform.GetChild(2).GetComponent<SpriteRenderer>().sprite = keyChosenSprites[i];
                //        }
                //    }

                //    climbGuideText.SetActive(true);

                //    VARS.IsInClimbGuide = true;
                //}
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
                            twistGuideText.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = keyChosenSprites[i];
                        }
                        else if (keyCodes[i] == VARS.rightKeyCode)
                        {
                            twistGuideText.transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = keyChosenSprites[i];
                        }
                    }

                    twistGuideText.SetActive(true);

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
                            backCenterGuideText.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = keyChosenSprites[i];
                            backCenterGuideText.transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = keyChosenSprites[i];
                            break;
                        }
                    }

                    backCenterGuideText.SetActive(true);

                    VARS.IsInBackCenterGuide = true;
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
                        keysGuideEmpty.transform.GetChild(1).GetChild(VARS.curKeysGuideIndex).GetChild(0).gameObject.SetActive(true);
                        keysGuideEmpty.transform.GetChild(1).GetChild(VARS.curKeysGuideIndex).GetChild(1).gameObject.SetActive(false);
                        keysGuideEmpty.transform.GetChild(1).GetChild(VARS.curKeysGuideIndex).GetChild(2).gameObject.SetActive(false);

                        VARS.curKeysGuideIndex++;

                        keysGuideEmpty.transform.GetChild(1).GetChild(VARS.curKeysGuideIndex).GetChild(0).gameObject.SetActive(false);
                        keysGuideEmpty.transform.GetChild(1).GetChild(VARS.curKeysGuideIndex).GetChild(1).gameObject.SetActive(true);
                        keysGuideEmpty.transform.GetChild(1).GetChild(VARS.curKeysGuideIndex).GetChild(2).gameObject.SetActive(true);
                    }
                }
                //right
                else if (VARS.curKeysGuideIndex == 1)
                {
                    if (VARS.IsRightKeyDown)
                    {
                        keysGuideEmpty.transform.GetChild(1).GetChild(VARS.curKeysGuideIndex).GetChild(0).gameObject.SetActive(true);
                        keysGuideEmpty.transform.GetChild(1).GetChild(VARS.curKeysGuideIndex).GetChild(1).gameObject.SetActive(false);
                        keysGuideEmpty.transform.GetChild(1).GetChild(VARS.curKeysGuideIndex).GetChild(2).gameObject.SetActive(false);

                        VARS.curKeysGuideIndex++;

                        keysGuideEmpty.transform.GetChild(1).GetChild(VARS.curKeysGuideIndex).GetChild(0).gameObject.SetActive(false);
                        keysGuideEmpty.transform.GetChild(1).GetChild(VARS.curKeysGuideIndex).GetChild(1).gameObject.SetActive(true);
                        keysGuideEmpty.transform.GetChild(1).GetChild(VARS.curKeysGuideIndex).GetChild(2).gameObject.SetActive(true);
                    }
                }
                //up
                else if (VARS.curKeysGuideIndex == 2)
                {
                    keysGuideEmpty.transform.GetChild(1).GetChild(VARS.curKeysGuideIndex).GetChild(0).gameObject.SetActive(false);
                    keysGuideEmpty.transform.GetChild(1).GetChild(VARS.curKeysGuideIndex).GetChild(1).gameObject.SetActive(true);
                    keysGuideEmpty.transform.GetChild(1).GetChild(VARS.curKeysGuideIndex).GetChild(2).gameObject.SetActive(true);

                    if (VARS.IsUpKeyDown)
                    {
                        keysGuideEmpty.transform.GetChild(1).GetChild(VARS.curKeysGuideIndex).GetChild(0).gameObject.SetActive(true);
                        keysGuideEmpty.transform.GetChild(1).GetChild(VARS.curKeysGuideIndex).GetChild(1).gameObject.SetActive(false);
                        keysGuideEmpty.transform.GetChild(1).GetChild(VARS.curKeysGuideIndex).GetChild(2).gameObject.SetActive(false);

                        VARS.curKeysGuideIndex++;

                        keysGuideEmpty.transform.GetChild(1).GetChild(VARS.curKeysGuideIndex).GetChild(0).gameObject.SetActive(false);
                        keysGuideEmpty.transform.GetChild(1).GetChild(VARS.curKeysGuideIndex).GetChild(1).gameObject.SetActive(true);
                        keysGuideEmpty.transform.GetChild(1).GetChild(VARS.curKeysGuideIndex).GetChild(2).gameObject.SetActive(true);
                    }
                }
                //down
                else if (VARS.curKeysGuideIndex == 3)
                {
                    if (VARS.IsDownKeyDown)
                    {
                        keysGuideEmpty.transform.GetChild(1).GetChild(VARS.curKeysGuideIndex).GetChild(0).gameObject.SetActive(true);
                        keysGuideEmpty.transform.GetChild(1).GetChild(VARS.curKeysGuideIndex).GetChild(1).gameObject.SetActive(false);
                        keysGuideEmpty.transform.GetChild(1).GetChild(VARS.curKeysGuideIndex).GetChild(2).gameObject.SetActive(false);

                        VARS.curKeysGuideIndex++;

                        keysGuideEmpty.transform.GetChild(1).GetChild(VARS.curKeysGuideIndex).GetChild(0).gameObject.SetActive(false);
                        keysGuideEmpty.transform.GetChild(1).GetChild(VARS.curKeysGuideIndex).GetChild(1).gameObject.SetActive(true);
                        keysGuideEmpty.transform.GetChild(1).GetChild(VARS.curKeysGuideIndex).GetChild(2).gameObject.SetActive(true);
                    }
                }
                //jump
                else if (VARS.curKeysGuideIndex == 4)
                {
                    if (VARS.IsJumpKeyDown)
                    {
                        keysGuideEmpty.transform.GetChild(1).GetChild(VARS.curKeysGuideIndex).GetChild(0).gameObject.SetActive(true);
                        keysGuideEmpty.transform.GetChild(1).GetChild(VARS.curKeysGuideIndex).GetChild(1).gameObject.SetActive(false);
                        keysGuideEmpty.transform.GetChild(1).GetChild(VARS.curKeysGuideIndex).GetChild(2).gameObject.SetActive(false);

                        VARS.curKeysGuideIndex++;

                        keysGuideEmpty.transform.GetChild(1).GetChild(VARS.curKeysGuideIndex).GetChild(0).gameObject.SetActive(false);
                        keysGuideEmpty.transform.GetChild(1).GetChild(VARS.curKeysGuideIndex).GetChild(1).gameObject.SetActive(true);
                        keysGuideEmpty.transform.GetChild(1).GetChild(VARS.curKeysGuideIndex).GetChild(2).gameObject.SetActive(true);
                    }
                }
                //acce
                else if (VARS.curKeysGuideIndex == 5)
                {
                    if (VARS.IsAcceKeyDown)
                    {
                        keysGuideEmpty.transform.GetChild(1).GetChild(VARS.curKeysGuideIndex).GetChild(0).gameObject.SetActive(true);
                        keysGuideEmpty.transform.GetChild(1).GetChild(VARS.curKeysGuideIndex).GetChild(1).gameObject.SetActive(false);
                        keysGuideEmpty.transform.GetChild(1).GetChild(VARS.curKeysGuideIndex).GetChild(2).gameObject.SetActive(false);

                        VARS.curKeysGuideIndex++;

                        keysGuideEmpty.transform.GetChild(1).GetChild(VARS.curKeysGuideIndex).GetChild(0).gameObject.SetActive(false);
                        keysGuideEmpty.transform.GetChild(1).GetChild(VARS.curKeysGuideIndex).GetChild(1).gameObject.SetActive(true);
                        keysGuideEmpty.transform.GetChild(1).GetChild(VARS.curKeysGuideIndex).GetChild(2).gameObject.SetActive(true);
                    }
                }
                //Minimap
                else if (VARS.curKeysGuideIndex == 6)
                {
                    if (VARS.IsMinimapKeyDown)
                    {
                        Debug.Log("keysGuideOver");

                        keysGuideEmpty.transform.GetChild(1).GetChild(VARS.curKeysGuideIndex).GetChild(0).gameObject.SetActive(true);
                        keysGuideEmpty.transform.GetChild(1).GetChild(VARS.curKeysGuideIndex).GetChild(1).gameObject.SetActive(false);
                        keysGuideEmpty.transform.GetChild(1).GetChild(VARS.curKeysGuideIndex).GetChild(2).gameObject.SetActive(false);

                        VARS.curKeysGuideIndex = 0;

                        keysGuideEmpty.SetActive(false);

                        VARS.curTargetEnergy = 0.1f;
                        VARS.curEnergy = 0.1f;

                        VARS.HasFinishedKeysGuide = true;

                        VARS.IsToWriteProgressData = true;

                        VARS.IsInKeysGuide = false;
                    }
                }
                //grab
                else if (VARS.curKeysGuideIndex == 7)
                {
                    if (VARS.IsGrabKeyDown)
                    {
                        keysGuideEmpty.transform.GetChild(1).GetChild(VARS.curKeysGuideIndex).GetChild(0).gameObject.SetActive(true);
                        keysGuideEmpty.transform.GetChild(1).GetChild(VARS.curKeysGuideIndex).GetChild(1).gameObject.SetActive(false);
                        keysGuideEmpty.transform.GetChild(1).GetChild(VARS.curKeysGuideIndex).GetChild(2).gameObject.SetActive(false);

                        VARS.curKeysGuideIndex++;

                        keysGuideEmpty.transform.GetChild(1).GetChild(VARS.curKeysGuideIndex).GetChild(0).gameObject.SetActive(false);
                        keysGuideEmpty.transform.GetChild(1).GetChild(VARS.curKeysGuideIndex).GetChild(1).gameObject.SetActive(true);
                        keysGuideEmpty.transform.GetChild(1).GetChild(VARS.curKeysGuideIndex).GetChild(2).gameObject.SetActive(true);
                    }
                }
            }
            //jump
            if (VARS.IsInJumpGuide &&
                VARS.IsInputtingJumpKey)
            {
                Debug.Log("jumpGuideOver");

                jumpGuideText.SetActive(false);

                VARS.HasJumped = true;

                VARS.IsToWriteProgressData = true;

                VARS.IsInJumpGuide = false;
            }
            //intoMinimap
            if (VARS.IsInIntoMinimapGuide &&
                VARS.IsMinimapKeyDown)
            {
                Debug.Log("intoMinimapGuideOver");

                intoMinimapGuideText.SetActive(false);

                VARS.HasBeenIntoMinimap = true;

                VARS.IsToWriteProgressData = true;

                VARS.IsInIntoMinimapGuide = false;
            }
            //rotate
            if (VARS.IsInRotateGuide &&
                VARS.IsInputtingDownKey &&
                (VARS.IsInputtingLeftKey || VARS.IsInputtingRightKey))
            {
                Debug.Log("rotateGuideOver");

                rotateGuideText.SetActive(false);

                VARS.HasRotated = true;

                VARS.IsToWriteProgressData= true;

                VARS.IsInRotateGuide = false;
            }
            ////climb
            //if (VARS.IsInClimbGuide &&
            //    VARS.IsInputtingRightKey &&
            //    VARS.IsInputtingGrabKey &&
            //    VARS.IsInputtingUpKey)
            //{
            //    Debug.Log("climbGuideOver");

            //    climbGuideText.SetActive(false);

            //    VARS.HasClimbed = true;

            //    VARS.IsToWriteProgressData = true;

            //    VARS.IsInClimbGuide = false;
            //}
            //twist
            if (VARS.IsInTwistGuide &&
                VARS.IsInputtingDownKey &&
                (VARS.IsInputtingLeftKey || VARS.IsInputtingRightKey))
            {
                Debug.Log("twistGuideOver");

                twistGuideText.SetActive(false);

                VARS.HasTwisted = true;

                VARS.IsToWriteProgressData = true;

                VARS.IsInTwistGuide = false;
            }
            //backCenter
            if (VARS.IsInBackCenterGuide &&
                VARS.IsBackCenterTriggered)
            {
                Debug.Log("backCenterGuideOver");

                backCenterGuideText.SetActive(false);

                VARS.HasBackCentered = true;

                VARS.IsToWriteProgressData = true;

                VARS.IsInBackCenterGuide = false;
            }
            #endregion
        }
    }
}
