using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Data;
public class GlobalConfig : SingletonDontDestroyOnLoad<GlobalConfig>
{
    public UserData UserData
    {
        get; private set;
    }

    protected override void Awake()
    {
        base.Awake();
        Game.Launch();
        UserData = Game.Data.Load<UserData>();
        UserData.Init();
        Application.targetFrameRate = 60;
        QualitySettings.vSyncCount = 0;
    }

    private void Start()
    {
        GameUI.Instance.Get<UIHome>().Show();
    }
}