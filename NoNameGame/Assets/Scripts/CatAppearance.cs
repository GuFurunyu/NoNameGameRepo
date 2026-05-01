using UnityEngine;

[DefaultExecutionOrder((int)ScriptsExecutionOrder.ExecutionOrder.catAppearance)]
public class CatAppearance : MonoBehaviour
{
    Constants CONS;
    Variables VARS;
    UniversalFunctionsLibrary UFL;
    ScriptsExecutionController SEC;

    GameObject gameManager;

    //contraction
    public float contractionSpeed;
    public float contractionMin;

    Transform catLeftEyeTransform;
    Transform catRightEyeTransform;

    Vector3 tempVector;

    #region ConstantsUsed
    GameObject cat;
    Transform catTransform;
    MeshRenderer catMeshRenderer;

    float maxEnergy;

    Material normalColor;
    Material energyMaskNormalColor;
    Material fadedColor;
    Material hotColor;
    Material coldColor;
    Material electricColor;
    Material toxicColor;

    //GameObject catEnergyBar;
    //GameObject catEnergyBarMask;

    GameObject catEnergyMask;
    MeshRenderer catEnergyMaskMeshRenderer;

    //GameObject outerOutlinesEmpty;
    //GameObject outerGrayOutlinesEmpty;

    GameObject catLeftEye;
    GameObject catRightEye;
    #endregion

    #region VariablesUsed
    //Vector3 planeForward;
    Vector3 curRoomStableForward;
    Vector3 curUp;
    Vector3 curRight;

    //float curEnergy;
    #endregion

    void Start()
    {
        gameManager = GameObject.Find("GameManager");

        CONS = gameManager.GetComponent<Constants>();
        VARS = gameManager.GetComponent<Variables>();
        UFL = gameManager.GetComponent<UniversalFunctionsLibrary>();
        SEC = gameManager.GetComponent<ScriptsExecutionController>();

        #region ImportConstants
        cat = CONS.cat;
        catTransform = CONS.catTransform;
        catMeshRenderer = CONS.catMeshRenderer;
        maxEnergy = CONS.maxEnergy;
        normalColor = CONS.normalColor;
        energyMaskNormalColor = CONS.energyMaskNormalColor;
        fadedColor = CONS.fadedColor;
        hotColor = CONS.hotColor;
        coldColor = CONS.coldColor;
        electricColor = CONS.electricColor;
        toxicColor = CONS.toxicColor;
        catEnergyMask = CONS.catEnergyMask;
        catEnergyMaskMeshRenderer = CONS.catEnergyMaskMeshRenderer;
        catLeftEye = CONS.catLeftEye;
        catRightEye = CONS.catRightEye;
        #endregion

        #region ImportReferenceVariables
        #endregion

        catLeftEyeTransform = catLeftEye.transform;
        catRightEyeTransform = catRightEye.transform;
    }

    void Update()
    {
        #region ImportValueVariables
        curRoomStableForward = VARS.curRoomStableForward;
        curUp = VARS.curUp;
        curRight = VARS.curRight;
        #endregion

        #region Contraction
        if (VARS.IsContracting)
        {
            catTransform.localScale -= new Vector3
                (Mathf.Abs(curRight.x), Mathf.Abs(curRight.y), Mathf.Abs(curRight.z))
                * contractionSpeed * Time.deltaTime;

            if (catTransform.localScale.magnitude < contractionMin)
            {
                VARS.IsContracting = false;
            }
        }
        else
        {
            if (catTransform.localScale.magnitude < 1.73f)
            {
                if (!VARS.IsHighJumping)
                {
                    catTransform.localScale += new Vector3
                        (Mathf.Abs(curRight.x), Mathf.Abs(curRight.y), Mathf.Abs(curRight.z))
                        * contractionSpeed * Time.deltaTime;
                }
            }
            else
            {
                catTransform.localScale = Vector3.one;
            }
        }
        #endregion

        #region Color
        if (VARS.IsInNormalColor)
        {
            catEnergyMaskMeshRenderer.material = energyMaskNormalColor;
        }
        else
        {
            //faded(rotationRestNumZero)
            if (VARS.IsInFadedColor)
            {
                catEnergyMaskMeshRenderer.material = fadedColor;
            }
            //afflicted
            if (VARS.IsAfflicted)
            {
                //temperature
                if (VARS.IsInHotState)
                {
                    catEnergyMaskMeshRenderer.material = hotColor;
                }
                else if (VARS.IsInColdState)
                {
                    catEnergyMaskMeshRenderer.material = coldColor;
                }

                //electricity
                else if (VARS.IsInElectricState)
                {
                    catEnergyMaskMeshRenderer.material = electricColor;
                }

                //toxicity
                else if (VARS.IsInToxicState)
                {
                    catEnergyMaskMeshRenderer.material = toxicColor;
                }
            }
        }

        #endregion

        #region EnergyBar(OutVersion)
        //if (curEnergy < 0)
        //{
        //    curEnergy = 0;
        //}

        //if (curEnergy >= maxEnergy)
        //{
        //    catEnergyBar.SetActive(false);
        //    catEnergyBarMask.SetActive(false);
        //}
        //else
        //{
        //    //if (curUp == iniUp)
        //    //{
        //    //    catEnergyBar.transform.eulerAngles = Vector3.zero;
        //    //    catEnergyBarMask.transform.eulerAngles = Vector3.zero;
        //    //}
        //    //else if (curUp == -iniRight)
        //    //{
        //    //    catEnergyBar.transform.eulerAngles = rightRotationVector;
        //    //    catEnergyBarMask.transform.eulerAngles = rightRotationVector;
        //    //}
        //    //else if (curUp == iniRight)
        //    //{
        //    //    catEnergyBar.transform.eulerAngles = leftRotationVector;
        //    //    catEnergyBarMask.transform.eulerAngles = leftRotationVector;
        //    //}
        //    //else if (curUp == -iniUp)
        //    //{
        //    //    catEnergyBar.transform.eulerAngles = new Vector3(0, 0, -180);
        //    //    catEnergyBarMask.transform.eulerAngles = new Vector3(0, 0, -180);
        //    //}

        //    //catEnergyBar.transform.eulerAngles = planeForward * Vector3.SignedAngle(planeRight, curRight, planeForward);
        //    //catEnergyBarMask.transform.eulerAngles = planeForward * Vector3.SignedAngle(planeRight, curRight, planeForward);

        //    //catEnergyBar.transform.localPosition = curUp * 0.75f + new Vector3(0, 0, -0.1f);
        //    catEnergyBar.transform.localPosition = curUp * 0.75f + curRoomStableForward * -1f;
        //    tempVector = curRight + curUp * 0.1f + curRoomStableForward;
        //    catEnergyBar.transform.localScale = new Vector3(Mathf.Abs(tempVector.x), Mathf.Abs(tempVector.y), Mathf.Abs(tempVector.z));

        //    if (curEnergy > 0)
        //    {
        //        //catEnergyBarMask.transform.localPosition = curUp * 0.75f + curRight * (curEnergy / maxEnergy + ((maxEnergy - curEnergy) / 2) / maxEnergy - 0.5f) + new Vector3(0, 0, -0.2f);
        //        catEnergyBarMask.transform.localPosition = curUp * 0.75f + curRight * (curEnergy / maxEnergy + ((maxEnergy - curEnergy) / 2) / maxEnergy - 0.5f) + curRoomStableForward * -2f;
        //        //catEnergyBarMask.transform.localScale = new Vector3((maxEnergy - curEnergy) / maxEnergy, 0.1f, 1);
        //        //catEnergyBarMask.transform.localScale = new Vector3((maxEnergy - curEnergy) / maxEnergy, 0.1f, (maxEnergy - curEnergy) / maxEnergy);
        //        tempVector = curRight * (maxEnergy - curEnergy) / maxEnergy + curUp * 0.1f + curRoomStableForward;
        //        catEnergyBarMask.transform.localScale = new Vector3(Mathf.Abs(tempVector.x), Mathf.Abs(tempVector.y), Mathf.Abs(tempVector.z));
        //    }
        //    else
        //    {
        //        //catEnergyBarMask.transform.localPosition = curUp * 0.75f + new Vector3(0, 0, -0.2f);
        //        catEnergyBarMask.transform.localPosition = curUp * 0.75f + curRoomStableForward * -2f;
        //        //catEnergyBarMask.transform.localScale = new Vector3(1, 0.1f, 1);
        //        tempVector = curRight + curUp * 0.1f + curRoomStableForward;
        //        catEnergyBarMask.transform.localScale = new Vector3(Mathf.Abs(tempVector.x), Mathf.Abs(tempVector.y), Mathf.Abs(tempVector.z));
        //    }

        //    catEnergyBar.SetActive(true);
        //    catEnergyBarMask.SetActive(true);
        //}
        #endregion

        #region EnergyMask
        if (VARS.curEnergy < 0)
        {
            //curEnergy = 0;
            //UFL.SetCurEnergy(0);
            VARS.curEnergy = 0;
        }

        if (VARS.curEnergy >= maxEnergy)
        {
            catEnergyMask.SetActive(false);
        }
        else
        {
            //position
            catEnergyMask.transform.localPosition = -curRoomStableForward * 0.01f + curUp * (1 - (maxEnergy - VARS.curEnergy) / maxEnergy) / 2;

            //localScale
            //catEnergyMask.transform.localScale = Vector3.one + curUp * ((maxEnergy - curEnergy) / maxEnergy - 1);
            catEnergyMask.transform.localScale = new Vector3
                (1 + Mathf.Abs(curUp.x) * ((maxEnergy - VARS.curEnergy) / maxEnergy - 1),
                1 + Mathf.Abs(curUp.y) * ((maxEnergy - VARS.curEnergy) / maxEnergy - 1),
                1 + Mathf.Abs(curUp.z) * ((maxEnergy - VARS.curEnergy) / maxEnergy - 1));

            catEnergyMask.SetActive(true);
        }
        #endregion

        #region Outlines(NotApplied)
        //outerOutlinesEmpty.SetActive(!VARS.IsInLiquid && !VARS.IsInGas);
        //outerGrayOutlinesEmpty.SetActive(VARS.IsInLiquid || VARS.IsInGas);

        //if (VARS.IsInLiquid || VARS.IsInGas)
        //{
        //    outerOutlinesEmpty.SetActive(false);
        //}
        //else if (VARS.IsOnGround)
        //{
        //    outerOutlinesEmpty.SetActive(true);
        //}

        //outerOutlinesEmpty.SetActive(VARS.IsOnGround && !VARS.IsInLiquid && !VARS.IsInGas);
        #endregion

        #region Eyes
        if (VARS.curFacingDirectionIndex == 0)
        {
            //still
            if (!VARS.IsInputtingUpKey &&
                !VARS.IsInputtingDownKey)
            {
                //position
                catLeftEyeTransform.localPosition = -curRoomStableForward * 0.1f - curUp * 0.25f - curRight * 0.175f;
                catRightEyeTransform.localPosition = -curRoomStableForward * 0.1f - curUp * 0.25f + curRight * 0.175f;

                //localScale
                catLeftEyeTransform.localScale = UFL.Vector3Abs(curRoomStableForward) + UFL.Vector3Abs(curUp) * 0.2f + UFL.Vector3Abs(curRight) * 0.15f;
                catRightEyeTransform.localScale = UFL.Vector3Abs(curRoomStableForward) + UFL.Vector3Abs(curUp) * 0.2f + UFL.Vector3Abs(curRight) * 0.15f;
            }
            //up
            else if (VARS.IsInputtingUpKey)
            {
                catLeftEyeTransform.localPosition = -curRoomStableForward * 0.1f - curUp * 0.2f - curRight * 0.175f;
                catRightEyeTransform.localPosition = -curRoomStableForward * 0.1f - curUp * 0.2f + curRight * 0.175f;

                catLeftEyeTransform.localScale = UFL.Vector3Abs(curRoomStableForward) + UFL.Vector3Abs(curUp) * 0.2f + UFL.Vector3Abs(curRight) * 0.15f;
                catRightEyeTransform.localScale = UFL.Vector3Abs(curRoomStableForward) + UFL.Vector3Abs(curUp) * 0.2f + UFL.Vector3Abs(curRight) * 0.15f;
            }
            //down
            else if (VARS.IsInputtingDownKey)
            {
                catLeftEyeTransform.localPosition = -curRoomStableForward * 0.1f - curUp * 0.3f - curRight * 0.175f;
                catRightEyeTransform.localPosition = -curRoomStableForward * 0.1f - curUp * 0.3f + curRight * 0.175f;

                catLeftEyeTransform.localScale = UFL.Vector3Abs(curRoomStableForward) + UFL.Vector3Abs(curUp) * 0.2f + UFL.Vector3Abs(curRight) * 0.15f;
                catRightEyeTransform.localScale = UFL.Vector3Abs(curRoomStableForward) + UFL.Vector3Abs(curUp) * 0.2f + UFL.Vector3Abs(curRight) * 0.15f;
            }
        }
        //left
        else if (VARS.curFacingDirectionIndex == 1)
        {
            catLeftEyeTransform.localPosition = -curRoomStableForward * 0.1f - curUp * 0.25f - curRight * 0.25f;
            catRightEyeTransform.localPosition = -curRoomStableForward * 0.1f - curUp * 0.25f + curRight * 0.025f;

            catLeftEyeTransform.localScale = UFL.Vector3Abs(curRoomStableForward) + UFL.Vector3Abs(curUp) * 0.2f + UFL.Vector3Abs(curRight) * 0.1f;
            catRightEyeTransform.localScale = UFL.Vector3Abs(curRoomStableForward) + UFL.Vector3Abs(curUp) * 0.2f + UFL.Vector3Abs(curRight) * 0.15f;
        }
        //right
        else if (VARS.curFacingDirectionIndex == 2)
        {
            catLeftEyeTransform.localPosition = -curRoomStableForward * 0.1f - curUp * 0.25f - curRight * 0.025f;
            catRightEyeTransform.localPosition = -curRoomStableForward * 0.1f - curUp * 0.25f + curRight * 0.25f;

            catLeftEyeTransform.localScale = UFL.Vector3Abs(curRoomStableForward) + UFL.Vector3Abs(curUp) * 0.2f + UFL.Vector3Abs(curRight) * 0.15f;
            catRightEyeTransform.localScale = UFL.Vector3Abs(curRoomStableForward) + UFL.Vector3Abs(curUp) * 0.2f + UFL.Vector3Abs(curRight) * 0.1f;
        }
        #endregion
    }
}
