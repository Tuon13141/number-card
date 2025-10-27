using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] LevelConfig m_LevelConfig;

    public LevelData GetLevelData(int level)
    {
        return LevelDataConvertor.LoadLevel(m_LevelConfig.GetLevel(level));
    }
}
