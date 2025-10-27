using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelConfig", menuName = "Scriptable Objects/Level/LevelConfig")]
public class LevelConfig : ScriptableObject
{
    public List<TextAsset> levels = new List<TextAsset>();

    public TextAsset GetLevel(int level)
    {
        return levels[level];
    }
}
