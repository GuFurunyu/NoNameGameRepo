using System.Collections;
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
    //keyCodes
    public List<KeyCode> keyCodes = new List<KeyCode>();

    //keySprites
    public List<Sprite> keySprites = new List<Sprite>();
    public List<Sprite> keyChosenSprites = new List<Sprite>();

    public GameObject keysGuideEmpty;
    //public List<GameObject> keysGuideTexts = new List<GameObject>();
    public GameObject intoMinimapGuideText;
    public GameObject climbGuideText;
    public GameObject twistGuideText;
    public GameObject backCenterGuideText;
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
        keyCodes = CONS.keyCodes;
        keySprites = CONS.keySprites;
        keyChosenSprites = CONS.keyChosenSprites;
        keysGuideEmpty = CONS.keysGuideEmpty;
        intoMinimapGuideText = CONS.intoMinimapGuideText;
        climbGuideText = CONS.climbGuideText;
        twistGuideText = CONS.twistGuideText;
        backCenterGuideText = CONS.backCenterGuideText;
        #endregion

        #region ImportReferenceVariable
        #endregion
    }

    void Update()
    {
        #region ImportValueVariables
        #endregion

        #region IntoGuide
        if (!VARS.HasFinishedKeysGuide &&
            !VARS.IsInKeysGuide)
        {
            keysGuideEmpty.SetActive(true);

            VARS.IsInKeysGuide = true;
        }

        //intoMinimap
        if (!VARS.HasBeenIntoMinimap &&
            !VARS.IsInIntoMinimapGuide &&
            VARS.curRoomIndex != 2)
        {
            VARS.IsMinimapActivated = true;

            for (int i = 0; i < keyCodes.Count; i++)
            {
                if (keyCodes[i] == VARS.confirmKeyCode)
                {
                    intoMinimapGuideText.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = keyChosenSprites[i];
                    break;
                }
            }

            intoMinimapGuideText.SetActive(true);

            VARS.IsInIntoMinimapGuide = true;
        }
        //climb
        else if (!VARS.HasClimbed &&
            !VARS.IsInClimbGuide &&
            VARS.IsRightBlocked &&
            !VARS.IsOnGround &&
            VARS.curRoomIndex > 2)
        {

            for (int i = 0; i < keyCodes.Count; i++)
            {
                if (keyCodes[i] == VARS.rightKeyCode)
                {
                    climbGuideText.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = keyChosenSprites[i];
                }
                else if (keyCodes[i] == VARS.grabKeyCode)
                {
                    climbGuideText.transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = keyChosenSprites[i];
                }
                else if (keyCodes[i] == VARS.upKeyCode)
                {
                    climbGuideText.transform.GetChild(2).GetComponent<SpriteRenderer>().sprite = keyChosenSprites[i];
                }
            }

            climbGuideText.SetActive(true);

            VARS.IsInClimbGuide = true;
        }
        //twist
        else if (!VARS.HasTwisted &&
            !VARS.IsInTwistGuide &&
            VARS.IsInCenter &&
            Mathf.Abs(VARS.verCurSpeed) < 0.1f &&
            Mathf.Abs(VARS.horCurSpeed) < 1)
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
        else if (!VARS.HasBackCentered &&
            !VARS.IsInBackCenterGuide &&
            VARS.HasTwisted && 
            !VARS.IsInCenter &&
            Vector3.Magnitude(VARS.curLatestCenterSavePointPosition) > 1)
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
        #endregion

        #region InGuide
        //keysGuide
        if (VARS.IsInKeysGuide)
        {
            //up
            if (VARS.curKeysGuideIndex == 0)
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
            else if (VARS.curKeysGuideIndex == 1)
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
            //left
            else if (VARS.curKeysGuideIndex == 2)
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
            else if (VARS.curKeysGuideIndex == 3)
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
            //grab
            else if (VARS.curKeysGuideIndex == 6)
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
            //confirm
            else if (VARS.curKeysGuideIndex == 7)
            {
                if (VARS.IsConfirmKeyDown)
                {
                    Debug.Log("keysGuideOver");

                    keysGuideEmpty.transform.GetChild(1).GetChild(VARS.curKeysGuideIndex).GetChild(0).gameObject.SetActive(true);
                    keysGuideEmpty.transform.GetChild(1).GetChild(VARS.curKeysGuideIndex).GetChild(1).gameObject.SetActive(false);
                    keysGuideEmpty.transform.GetChild(1).GetChild(VARS.curKeysGuideIndex).GetChild(2).gameObject.SetActive(false);

                    VARS.curKeysGuideIndex = 0;

                    keysGuideEmpty.SetActive(false);

                    VARS.HasFinishedKeysGuide = true;

                    VARS.IsToWriteProgressData = true;

                    VARS.IsInKeysGuide = false;
                }
            }
        }
        //intoMinimap
        if (VARS.IsInIntoMinimapGuide &&
            VARS.IsConfirmKeyDown)
        {
            Debug.Log("intoMinimapGuideOver");

            intoMinimapGuideText.SetActive(false);

            VARS.HasBeenIntoMinimap = true;

            VARS.IsToWriteProgressData = true;

            VARS.IsInIntoMinimapGuide = false;
        }
        //climb
        else if (VARS.IsInClimbGuide &&
            VARS.IsInputtingRightKey &&
            VARS.IsInputtingGrabKey &&
            VARS.IsInputtingUpKey)
        {
            Debug.Log("climbGuideOver");

            climbGuideText.SetActive(false);

            VARS.HasClimbed = true;

            VARS.IsToWriteProgressData = true;

            VARS.IsInClimbGuide = false;
        }

        //twist
        else if (VARS.IsInTwistGuide &&
            VARS.IsInputtingDownKey &&
            (VARS.IsInputtingLeftKey || VARS.IsInputtingRightKey))
        {
            Debug.Log("twistGuideOver");

            twistGuideText.SetActive(false);

            VARS.HasTwisted = true;

            VARS.IsToWriteProgressData= true;

            VARS.IsInTwistGuide = false;
        }
        //backCenter
        else if (VARS.IsInBackCenterGuide &&
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
