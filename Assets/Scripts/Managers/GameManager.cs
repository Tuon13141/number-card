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

    [SerializeField] private Transform m_GameHolder;

    [ReadOnly] public static int totalLayer;
    [ReadOnly] public static int totalCard;

    private void Start()
    {
        m_TimeManager.OnEnd += OnLose;
    }

    public void StartLevel()
    {
        int level = GlobalConfig.Instance.UserData.level;



        LevelData = m_LevelManager.GetLevelData(level);

        m_TimeManager.SetUp(LevelData.playTime);


        totalLayer = LevelData.layerDatas.Count;

        int maxWidth = 3;

        for (int i = 0; i < totalLayer; i++)
        {
            LayerData layerData = LevelData.layerDatas[i];

            LayerElement layerElement = PoolingManager.Instance.Get<LayerElement>();
            layerElement.SetUp(layerData, i);

            layerElement.transform.parent = m_GameHolder;

            _layerElements.Add(layerElement);

            if(layerData.width > maxWidth) maxWidth = layerData.width;
        }

        for (int i = 0; i < _layerElements.Count - 1; i++)
        {
            _layerElements[i].SetUpperCard(_layerElements[i + 1]);
        }


        GameUI.Instance.Get<UIInGame>().Show();
        GameUI.Instance.Get<UIInGame>().SetLevelText(level + 1);

        Camera.main.orthographicSize = 2 * maxWidth + 1;
    }

    public void OnReset()
    {
        foreach (LayerElement layerElement in _layerElements)
        {
            layerElement.OnReset();
        }

        m_TimeManager.StopTimer(false);

        LevelData = null;
        totalLayer = 0;
        totalCard = 0;
    }

    #region Conditions
    public void OnWin()
    {
        GameUI.Instance.Get<UIInGame>().Hide();
        GameUI.Instance.Get<UIWin>().Show();

        m_LevelManager.NextLevel();
    }

    public void OnLose()
    {
        GameUI.Instance.Get<UIInGame>().Hide();
        GameUI.Instance.Get<UIRetry>().Show();
    }

    public void CheckWin()
    {
        if(totalCard == 0)
        {
            OnWin();
            m_TimeManager.StopTimer(false);
        }
    }
    #endregion

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            Time.timeScale = 0;
        }
    }
}
