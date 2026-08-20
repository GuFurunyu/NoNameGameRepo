using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder((int)ScriptsExecutionOrder.ExecutionOrder.textManager)]
public class TextManager : MonoBehaviour
{
    Constants CONS;
    Variables VARS;
    UniversalFunctionsLibrary UFL;
    ScriptsExecutionController SEC;

    GameObject gameManager;

    #region ConstantsUsed
    GameObject oneColorFragmentCollectingTextEmpty;
    GameObject allColorsFragmentCollectingTextEmpty;
    GameObject keysAndLocksCollectingTextEmpty;
    GameObject oneColorFragmentCollectingTextLeft;
    GameObject allColorsFragmentCollectingTextLeft1;
    GameObject allColorsFragmentCollectingTextLeft2;
    GameObject keysAndLocksCollectingTextLeft1;
    GameObject keysAndLocksCollectingTextLeft2;
    GameObject oneColorFragmentCollectingTextRight;
    GameObject allColorsFragmentCollectingTextRight;
    GameObject keysAndLocksCollectingTextRight;

    float oneColorFragmentCollectingTextActivatedTime;
    float allColorsFragmentCollectingTextActivatedTime;
    float keysAndLocksCollectingTextActivatedTime;
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
        oneColorFragmentCollectingTextEmpty = CONS.oneColorFragmentCollectingTextEmpty;
        allColorsFragmentCollectingTextEmpty = CONS.allColorsFragmentCollectingTextEmpty;
        keysAndLocksCollectingTextEmpty = CONS.keysAndLocksCollectingTextEmpty;
        oneColorFragmentCollectingTextLeft = CONS.oneColorFragmentCollectingTextLeft;
        allColorsFragmentCollectingTextLeft1 = CONS.allColorsFragmentCollectingTextLeft1;
        allColorsFragmentCollectingTextLeft2 = CONS.allColorsFragmentCollectingTextLeft2;
        keysAndLocksCollectingTextLeft1 = CONS.keysAndLocksCollectingTextLeft1;
        keysAndLocksCollectingTextLeft2 = CONS.keysAndLocksCollectingTextLeft2;
        oneColorFragmentCollectingTextRight = CONS.oneColorFragmentCollectingTextRight;
        allColorsFragmentCollectingTextRight = CONS.allColorsFragmentCollectingTextRight;
        keysAndLocksCollectingTextRight = CONS.keysAndLocksCollectingTextRight;
        oneColorFragmentCollectingTextActivatedTime = CONS.oneColorFragmentCollectingTextActivatedTime;
        allColorsFragmentCollectingTextActivatedTime = CONS.allColorsFragmentCollectingTextActivatedTime;
        keysAndLocksCollectingTextActivatedTime = CONS.keysAndLocksCollectingTextActivatedTime;
        #endregion

        #region ImportReferenceVariable
        #endregion
    }

    void Update()
    {
        #region ImportValueVariables
        #endregion

        #region CollectingText
        //oneColorFragment
        if (oneColorFragmentCollectingTextEmpty.activeSelf)
        {
            //floatingIn
            if(Time.time-VARS.oneColorFragmentCollectingTextActivatedStartTime <= 0.15f)
            {
                if (oneColorFragmentCollectingTextLeft.GetComponent<SpriteRenderer>().color.a < 1)
                {
                    oneColorFragmentCollectingTextLeft.GetComponent<SpriteRenderer>().color += new Color(0, 0, 0, 10 * Time.deltaTime);
                    oneColorFragmentCollectingTextRight.GetComponent<SpriteRenderer>().color += new Color(0, 0, 0, 10 * Time.deltaTime);
                }
            }
            //floatingOut
            if (Time.time - VARS.oneColorFragmentCollectingTextActivatedStartTime >= oneColorFragmentCollectingTextActivatedTime - 0.15f)
            {
                if (oneColorFragmentCollectingTextLeft.GetComponent<SpriteRenderer>().color.a > 0)
                {
                    oneColorFragmentCollectingTextLeft.GetComponent<SpriteRenderer>().color -= new Color(0, 0, 0, 10 * Time.deltaTime);
                    oneColorFragmentCollectingTextRight.GetComponent<SpriteRenderer>().color -= new Color(0, 0, 0, 10 * Time.deltaTime);
                }
            }
            //disappear
            if (Time.time-VARS.oneColorFragmentCollectingTextActivatedStartTime> oneColorFragmentCollectingTextActivatedTime ||
                VARS.IsRotating ||
                VARS.IsInNewRoom ||
                VARS.IsOptionPanelActivated ||
                VARS.IsInMinimap)
            {
                oneColorFragmentCollectingTextEmpty.SetActive(false);
            }
        }

        //allColorsFragment
        if (allColorsFragmentCollectingTextEmpty.activeSelf)
        {
            //floatingIn
            if (Time.time - VARS.allColorsFragmentCollectingTextActivatedStartTime >= oneColorFragmentCollectingTextActivatedTime - 0.1f &&
                Time.time-VARS.allColorsFragmentCollectingTextActivatedStartTime <= oneColorFragmentCollectingTextActivatedTime - 0.1f + 0.15f)
            {
                if (allColorsFragmentCollectingTextLeft1.GetComponent<SpriteRenderer>().color.a < 1)
                {
                    allColorsFragmentCollectingTextLeft1.GetComponent<SpriteRenderer>().color += new Color(0, 0, 0, 10 * Time.deltaTime);
                    allColorsFragmentCollectingTextLeft2.GetComponent<SpriteRenderer>().color += new Color(0, 0, 0, 10 * Time.deltaTime);
                    allColorsFragmentCollectingTextRight.GetComponent<SpriteRenderer>().color += new Color(0, 0, 0, 10 * Time.deltaTime);
                }
            }
            //floatingOut
            if (Time.time - VARS.allColorsFragmentCollectingTextActivatedStartTime >= oneColorFragmentCollectingTextActivatedTime + allColorsFragmentCollectingTextActivatedTime - 0.1f - 0.15f)
            {
                if (allColorsFragmentCollectingTextLeft1.GetComponent<SpriteRenderer>().color.a > 0)
                {
                    allColorsFragmentCollectingTextLeft1.GetComponent<SpriteRenderer>().color -= new Color(0, 0, 0, 10 * Time.deltaTime);
                    allColorsFragmentCollectingTextLeft2.GetComponent<SpriteRenderer>().color -= new Color(0, 0, 0, 10 * Time.deltaTime);
                    allColorsFragmentCollectingTextRight.GetComponent<SpriteRenderer>().color -= new Color(0, 0, 0, 10 * Time.deltaTime);
                }
            }
            //disappear
            if (Time.time - VARS.allColorsFragmentCollectingTextActivatedStartTime > oneColorFragmentCollectingTextActivatedTime - 0.1f + allColorsFragmentCollectingTextActivatedTime ||
                VARS.IsRotating ||
                VARS.IsInNewRoom ||
                VARS.IsOptionPanelActivated ||
                VARS.IsInMinimap)
            {
                allColorsFragmentCollectingTextEmpty.SetActive(false);
            }
        }

        //keysAndLocks
        if (keysAndLocksCollectingTextEmpty.activeSelf)
        {
            //floatingIn
            if (Time.time - VARS.keysAndLocksCollectingTextActivatedStartTime <= keysAndLocksCollectingTextActivatedTime / 3)
            {
                if (keysAndLocksCollectingTextLeft1.GetComponent<SpriteRenderer>().color.a < 1)
                {
                    keysAndLocksCollectingTextLeft1.GetComponent<SpriteRenderer>().color += new Color(0, 0, 0, 10 * Time.deltaTime);
                    keysAndLocksCollectingTextLeft2.GetComponent<SpriteRenderer>().color += new Color(0, 0, 0, 10 * Time.deltaTime);
                    keysAndLocksCollectingTextRight.GetComponent<SpriteRenderer>().color += new Color(0, 0, 0, 10 * Time.deltaTime);
                }
            }
            //floatingOut
            if (Time.time - VARS.keysAndLocksCollectingTextActivatedStartTime >= keysAndLocksCollectingTextActivatedTime * 2 / 3)
            {
                if (keysAndLocksCollectingTextLeft1.GetComponent<SpriteRenderer>().color.a > 0)
                {
                    keysAndLocksCollectingTextLeft1.GetComponent<SpriteRenderer>().color -= new Color(0, 0, 0, 10 * Time.deltaTime);
                    keysAndLocksCollectingTextLeft2.GetComponent<SpriteRenderer>().color -= new Color(0, 0, 0, 10 * Time.deltaTime);
                    keysAndLocksCollectingTextRight.GetComponent<SpriteRenderer>().color -= new Color(0, 0, 0, 10 * Time.deltaTime);
                }
            }
            //disappear
            if (Time.time - VARS.keysAndLocksCollectingTextActivatedStartTime > keysAndLocksCollectingTextActivatedTime ||
                VARS.IsRotating ||
                VARS.IsInNewRoom ||
                VARS.IsOptionPanelActivated ||
                VARS.IsInMinimap)
            {
                keysAndLocksCollectingTextEmpty.SetActive(false);
            }
        }
        #endregion
    }
}
