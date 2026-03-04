using System.IO;
using Unity.VisualScripting;
using UnityEngine;


[DefaultExecutionOrder((int)ScriptsExecutionOrder.ExecutionOrder.dataManager)]
public class DataManager : MonoBehaviour
{
    Constants CONS;
    Variables VARS;
    UniversalFunctionsLibrary UFL;
    ScriptsExecutionController SEC;

    GameObject gameManager;

    public class KeyCodesData
    {
        public KeyCode upKeyCode;
        public KeyCode downKeyCode;
        public KeyCode leftKeyCode;
        public KeyCode rightKeyCode;
        public KeyCode jumpKeyCode;
        public KeyCode dashKeyCode;
    }

    KeyCodesData curKeyCodesData = new KeyCodesData();

    #region ConstantsUsed

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

        ReadKeyCodesData();
    }

    void Update()
    {
        if (VARS.isToWriteKeyCodesData)
        {
            WriteKeyCodesData();

            VARS.isToWriteKeyCodesData = false;
        }
    }

    void WriteKeyCodesData()
    {
        curKeyCodesData.upKeyCode = VARS.upKeyCode;
        curKeyCodesData.downKeyCode = VARS.downKeyCode;
        curKeyCodesData.leftKeyCode = VARS.leftKeyCode;
        curKeyCodesData.rightKeyCode = VARS.rightKeyCode;
        curKeyCodesData.jumpKeyCode = VARS.jumpKeyCode;
        curKeyCodesData.dashKeyCode = VARS.dashKeyCode;

        string jsonString = JsonUtility.ToJson(curKeyCodesData);

        string path = Path.Combine(Application.persistentDataPath, "Datas","KeyCodesData.txt");

        File.WriteAllText(path, jsonString);
    }

    void ReadKeyCodesData()
    {
        string path = Path.Combine(Application.persistentDataPath, "Datas","KeyCodesData.txt");

        if (File.Exists(path))
        {
            string jsonString = File.ReadAllText(path);
            curKeyCodesData = JsonUtility.FromJson<KeyCodesData>(jsonString);

            VARS.upKeyCode = curKeyCodesData.upKeyCode;
            VARS.downKeyCode = curKeyCodesData.downKeyCode;
            VARS.leftKeyCode = curKeyCodesData.leftKeyCode;
            VARS.rightKeyCode = curKeyCodesData.rightKeyCode;
            VARS.jumpKeyCode = curKeyCodesData.jumpKeyCode;
            VARS.dashKeyCode = curKeyCodesData.dashKeyCode;

            VARS.curKeyCodes.Clear();

            VARS.curKeyCodes.Add(VARS.upKeyCode);
            VARS.curKeyCodes.Add(VARS.downKeyCode);
            VARS.curKeyCodes.Add(VARS.leftKeyCode);
            VARS.curKeyCodes.Add(VARS.rightKeyCode);
            VARS.curKeyCodes.Add(VARS.jumpKeyCode);
            VARS.curKeyCodes.Add(VARS.dashKeyCode);
        }
        else
        {
            Debug.Log("没有找到存档文件");
        }
    }
}
