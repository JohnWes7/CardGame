using System.IO;
using System.Reflection;
using UnityEditor.U2D;
using UnityEditor;
using UnityEngine.U2D;
using UnityEngine;

public class SpriteAtlasExportTool : EditorWindow
{
    const string MenuItemPath_ExportSelectSpriteAtlas = "MyTools/Export Select SpriteAtlas";

    [MenuItem(MenuItemPath_ExportSelectSpriteAtlas, true)]

    private static bool MenuItemValidator_ExportSelectSpriteAtlas()
    {
        if (Application.isPlaying)
            return false;
        if (null == Selection.activeObject)
            return false;
        SpriteAtlas atlas = Selection.activeObject as SpriteAtlas;
        if (null == atlas)
            return false;
        return true;
    }

    [MenuItem(MenuItemPath_ExportSelectSpriteAtlas)]
    private static void MenuItem_ExportSelectSpriteAtlas()
    {
        if (null == Selection.activeObject)
            return;
        SpriteAtlas atlas = Selection.activeObject as SpriteAtlas;
        if (null == atlas)
            return;

        TextureImporterPlatformSettings texImpPlatSettings = SpriteAtlasExtensions.GetPlatformSettings(atlas, "DefaultTexturePlatform");
        TextureImporterPlatformSettings texImpPlatSettingsBak = new TextureImporterPlatformSettings();
        texImpPlatSettings.CopyTo(texImpPlatSettingsBak); //备份贴图平台设置, 用于后面恢复
        //texImpPlatSettings.overridden = true;
        texImpPlatSettings.format = TextureImporterFormat.RGBA32; //高清图
        SpriteAtlasExtensions.SetPlatformSettings(atlas, texImpPlatSettings);

        SpriteAtlasTextureSettings atlasTexSettings = SpriteAtlasExtensions.GetTextureSettings(atlas);
        atlasTexSettings.generateMipMaps = false; //关掉mipMaps

        SpriteAtlasUtility.PackAtlases(new SpriteAtlas[] { atlas }, EditorUserBuildSettings.activeBuildTarget); //导出前打一下图集
        MethodInfo getPreviewTextureMI = typeof(SpriteAtlasExtensions).GetMethod("GetPreviewTextures", BindingFlags.Static | BindingFlags.NonPublic);
        Texture2D[] atlasTexs = (Texture2D[])getPreviewTextureMI.Invoke(null, new object[] { atlas });
        if (null != atlasTexs && atlasTexs.Length > 0)
        {
            string outFolderName = "_AtlasTemp";
            if (!Directory.Exists(outFolderName))
                Directory.CreateDirectory(outFolderName);

            for (int i = 0; i < atlasTexs.Length; ++i)
            {
                var tex2D = atlasTexs[i];
                Debug.Log($"tex: name:{tex2D.name}, format:{tex2D.format}, path:{AssetDatabase.GetAssetPath(tex2D)}");

                var exportPngFilePath = "";
                if (1 == atlasTexs.Length)
                    exportPngFilePath = Path.Combine(outFolderName, $"{tex2D.name}.png");
                else
                    exportPngFilePath = Path.Combine(outFolderName, $"{tex2D.name}-Page{i}.png");

                var rawBytes = tex2D.GetRawTextureData();

                Texture2D tempTex = new Texture2D(tex2D.width, tex2D.height, tex2D.format, false, false);
                tempTex.LoadRawTextureData(rawBytes);
                tempTex.Apply();

                var pngBytes = tempTex.EncodeToPNG();
                Debug.Log($"Export {i}: {exportPngFilePath}");
                File.WriteAllBytes(exportPngFilePath, pngBytes);
            }
        }
        Debug.Log($"finish");

        SpriteAtlasExtensions.SetPlatformSettings(atlas, texImpPlatSettingsBak); //恢复设置
    }
}