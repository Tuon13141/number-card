using System.IO;
using UnityEngine;
using Newtonsoft.Json;

public static class LevelDataConvertor
{
    public static LevelData LoadLevel(TextAsset jsonFile)
    {
        if (jsonFile == null)
        {
            Debug.LogError("[LevelDataIO] TextAsset null!");
            return null;
        }

        try
        {
            var data = JsonConvert.DeserializeObject<LevelData>(jsonFile.text);
            return data;
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"[LevelDataIO] Lỗi khi parse JSON từ TextAsset: {ex.Message}");
            return null;
        }
    }
}
