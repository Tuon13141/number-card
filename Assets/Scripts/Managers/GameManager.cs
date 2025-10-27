using UnityEngine;
using NaughtyAttributes;
using System.Collections.Generic;


public class GameManager : Singleton<GameManager>
{
    [Header("Reference")]
    [SerializeField] LevelManager m_LevelManager;
    [SerializeField] TimeManager m_TimeManager;
    private List<LayerElement> _layerElements = new List<LayerElement> ();

    [Header("Data")]
    [ReadOnly] public LevelData LevelData;

    private void Start()
    {
        StartLevel();
    }

    public void StartLevel()
    {
        LevelData = m_LevelManager.GetLevelData(0);

        for (int i = 0; i < LevelData.layerDatas.Count; i++)
        {
            LayerData layerData = LevelData.layerDatas[i];

            LayerElement layerElement = PoolingManager.Instance.Get<LayerElement>();
            layerElement.SetUp(layerData, i);

            _layerElements.Add(layerElement);
        }

    }
}
