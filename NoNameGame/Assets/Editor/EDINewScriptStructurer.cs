//ByCopilot
//NotFullyUnderstood
using UnityEditor;
using System.IO;
using System.Text.RegularExpressions;

public class EDINewScriptStructurer : AssetPostprocessor
{
    // 匹配Unity默认新建脚本的Start和Update方法
    private static readonly Regex UnityDefaultScriptRegex = new Regex(
        @"// Start is called before the first frame update\s*void Start\s*\(\)\s*\{\s*\}\s*// Update is called once per frame\s*void Update\s*\(\)\s*\{\s*\}",
        RegexOptions.Compiled | RegexOptions.Singleline);

    static void OnPostprocessAllAssets(
        string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
    {
        foreach (var assetPath in importedAssets)
        {
            if (!assetPath.StartsWith("Assets/Scripts/") || !assetPath.EndsWith(".cs"))
                continue;

            string fullPath = Path.Combine(Directory.GetCurrentDirectory(), assetPath);
            if (!File.Exists(fullPath))
                continue;

            string content = File.ReadAllText(fullPath);

            // 只处理Unity默认新建脚本
            if (!UnityDefaultScriptRegex.IsMatch(content))
                continue;

            string fileName = Path.GetFileNameWithoutExtension(assetPath);
            if (string.IsNullOrEmpty(fileName))
                continue;

            string executionOrderName = char.ToLowerInvariant(fileName[0]) + fileName.Substring(1);

            string template = $@"using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder((int)ScriptsExecutionOrder.ExecutionOrder.{executionOrderName})]
public class {fileName} : MonoBehaviour
{{
    Constants CONS;
    Variables VARS;
    UniversalFunctionsLibrary UFL;
    ScriptsExecutionController SEC;

    GameObject gameManager;

    #region ConstantsUsed

    #endregion

    #region VariablesUsed

    #endregion

    void Start()
    {{
        gameManager = GameObject.Find(""GameManager"");

        CONS = gameManager.GetComponent<Constants>();
        VARS = gameManager.GetComponent<Variables>();
        UFL = gameManager.GetComponent<UniversalFunctionsLibrary>();
        SEC = gameManager.GetComponent<ScriptsExecutionController>();

        #region ImportConstants
        #endregion

        #region ImportReferenceVariable
        #endregion
    }}

    void Update()
    {{
        #region ImportValueVariables
        #endregion
    }}
}}
";
            File.WriteAllText(fullPath, template, System.Text.Encoding.UTF8);
            AssetDatabase.ImportAsset(assetPath);
        }
    }
}
