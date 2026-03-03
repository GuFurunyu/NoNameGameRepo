//ByCopilot
using UnityEngine;
using UnityEditor;

public class EDICustomTextureImporter : AssetPostprocessor
{
    void OnPreprocessTexture()
    {
        // 只针对特定文件夹
        if (assetPath.StartsWith("Assets/Resourses/Pictures/BlocksArrangementTextures"))
        {
            TextureImporter importer = (TextureImporter)assetImporter;
            importer.textureType = TextureImporterType.Default;
            importer.isReadable = true;
            importer.mipmapEnabled = false;
            importer.filterMode = FilterMode.Point;
            importer.textureCompression = TextureImporterCompression.Uncompressed;
            importer.npotScale = TextureImporterNPOTScale.None;
            importer.maxTextureSize = 4096; // 可根据需要调整
        }
    }
}
