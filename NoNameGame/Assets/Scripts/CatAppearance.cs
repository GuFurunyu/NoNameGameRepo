using UnityEngine;

[DefaultExecutionOrder((int)ScriptsExecutionOrder.ExecutionOrder.catAppearance)]
public class CatAppearance : MonoBehaviour
{
    Constants CONS;
    Variables VARS;
    ScriptsExecutionController SEC;

    GameObject gameManager;

    //contraction
    public float contractionSpeed;
    public float contractionMin;

    Vector3 tempVector;

    #region ConstantsUsed
    GameObject cat;
    Transform catTransform;

    float maxEnergy;

    Material normalColor;
    Material fadedColor;

    GameObject catEnergyBar;
    GameObject catEnergyBarMask;

    GameObject catEnergyMask;

    GameObject outerOutlinesEmpty;
    GameObject outerGrayOutlinesEmpty;
    #endregion

    #region VariablesUsed
    //Vector3 planeForward;
    Vector3 curRoomStableForward;
    Vector3 curUp;
    Vector3 curRight;

    float curEnergy;
    #endregion

    void Start()
    {
        gameManager = GameObject.Find("GameManager");

        CONS = gameManager.GetComponent<Constants>();
        VARS = gameManager.GetComponent<Variables>();
        SEC = gameManager.GetComponent<ScriptsExecutionController>();

        cat = CONS.cat;
        catTransform = CONS.catTransform;
        maxEnergy = CONS.maxEnergy;
        normalColor = CONS.normalColor;
        fadedColor = CONS.fadedColor;
        catEnergyBar = CONS.catEnergyBar;
        catEnergyBarMask = CONS.catEnergyBarMask;
        catEnergyMask = CONS.catEnergyMask;
        outerOutlinesEmpty = CONS.outerOutlinesEmpty;
        outerGrayOutlinesEmpty = CONS.outerGrayOutlinesEmpty;
    }

    void Update()
    {
        //planeForward = VARS.planeForward;
        curRoomStableForward = VARS.curRoomStableForward;
        curUp = VARS.curUp;
        curRight = VARS.curRight;
        curEnergy = VARS.curEnergy;

        #region contraction
        if (VARS.isContracting)
        {
            catTransform.localScale -= new Vector3
                (Mathf.Abs(curRight.x), Mathf.Abs(curRight.y), Mathf.Abs(curRight.z))
                * contractionSpeed * Time.deltaTime;

            if (catTransform.localScale.magnitude < contractionMin)
            {
                VARS.isContracting = false;
            }
        }
        else
        {
            if (catTransform.localScale.magnitude < 1.73f)
            {
                if (!VARS.isHighJumping)
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
        if (VARS.isInFadedColor)
        {
            cat.GetComponent<MeshRenderer>().material = fadedColor;
        }
        else
        {
            cat.GetComponent<MeshRenderer>().material = normalColor;
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
        if (curEnergy < 0)
        {
            curEnergy = 0;
        }

        if (curEnergy >= maxEnergy)
        {
            catEnergyMask.SetActive(false);
        }
        else
        {
            //localScale
            //catEnergyMask.transform.localScale = Vector3.one + curUp * ((maxEnergy - curEnergy) / maxEnergy - 1);
            catEnergyMask.transform.localScale = new Vector3
                (1+Mathf.Abs(curUp.x)*((maxEnergy-curEnergy)/maxEnergy-1),
                1 + Mathf.Abs(curUp.y) * ((maxEnergy - curEnergy) / maxEnergy - 1),
                1 + Mathf.Abs(curUp.z) * ((maxEnergy - curEnergy) / maxEnergy - 1));

            //position
            catEnergyMask.transform.localPosition = -curRoomStableForward * 0.01f + curUp * (1 - (maxEnergy - curEnergy) / maxEnergy) / 2;

            catEnergyMask.SetActive(true);
        }
        #endregion

        #region Outlines
        //outerOutlinesEmpty.SetActive(!VARS.isInLiquid && !VARS.isInGas);
        //outerGrayOutlinesEmpty.SetActive(VARS.isInLiquid || VARS.isInGas);

        //if (VARS.isInLiquid || VARS.isInGas)
        //{
        //    outerOutlinesEmpty.SetActive(false);
        //}
        //else if (VARS.isOnGround)
        //{
        //    outerOutlinesEmpty.SetActive(true);
        //}

        //outerOutlinesEmpty.SetActive(VARS.isOnGround && !VARS.isInLiquid && !VARS.isInGas);
        #endregion
    }
}
