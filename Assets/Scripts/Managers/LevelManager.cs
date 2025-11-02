using Data;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] LevelConfig m_LevelConfig;

    public LevelData GetLevelData(int level)
    {
        return LevelDataConvertor.LoadLevel(m_LevelConfig.GetLevel(level));
    }

    public void NextLevel()
    {
        int level = GlobalConfig.Instance.UserData.level;
        level++;

        if (level >= m_LevelConfig.GetTotalLevel())
        {
            level = m_LevelConfig.GetTotalLevel();
        }

        GlobalConfig.Instance.UserData.level = level;
    }
}
