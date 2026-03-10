using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder((int)ScriptsExecutionOrder.ExecutionOrder.screenPostProcessor)]
public class ScreenPostProcessor : MonoBehaviour
{
    Constants CONS;
    Variables VARS;
    UniversalFunctionsLibrary UFL;
    ScriptsExecutionController SEC;

    GameObject gameManager;

    public Shader postProcessShader;
    private Material postProcessMaterial;

    public List<Color> sourceColors = new List<Color>();
    public List<Color> targetColors = new List<Color>();
    public bool enablePostProcess = true;

    void Start()
    {
        gameManager = GameObject.Find("GameManager");

        CONS = gameManager.GetComponent<Constants>();
        VARS = gameManager.GetComponent<Variables>();
        UFL = gameManager.GetComponent<UniversalFunctionsLibrary>();
        SEC = gameManager.GetComponent<ScriptsExecutionController>();

        if (postProcessShader == null)
        {
            postProcessShader = Shader.Find("Custom/ScreenColorReplace");
        }
        if (postProcessShader != null)
        {
            postProcessMaterial = new Material(postProcessShader);
        }
    }

    void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        if (!enablePostProcess || postProcessMaterial == null)
        {
            Graphics.Blit(src, dest);
            return;
        }

        if (sourceColors.Count != targetColors.Count)
        {
            Debug.LogError("ScreenPostProcessor: sourceColors和targetColors数量不一致！");
            Graphics.Blit(src, dest);
            return;
        }

        // 限制最大支持数量（与shader一致，建议32以内）
        int maxCount = 32;
        int colorCount = Mathf.Min(sourceColors.Count, maxCount);

        // 传递颜色数组和数量
        postProcessMaterial.SetInt("_ColorCount", colorCount);
        postProcessMaterial.SetColorArray("_SourceColors", sourceColors.GetRange(0, colorCount));
        postProcessMaterial.SetColorArray("_TargetColors", targetColors.GetRange(0, colorCount));

        Graphics.Blit(src, dest, postProcessMaterial);
    }
}
