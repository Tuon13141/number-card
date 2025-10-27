using System;
using UnityEngine;
using UnityEngine.Events;
using Cysharp.Threading.Tasks;

[DefaultExecutionOrder(-200)]
public class TimeManager : Singleton<TimeManager>
{
    [Header("Time Settings")]
    [SerializeField] private bool m_AutoStart = false;
    [SerializeField, Tooltip("Thời gian đếm ban đầu (giây)")]
    private float m_DefaultTime = 10f;

    private float m_RemainingTime;
    private bool m_IsRunning;

    public event Action OnBegin;
    public event Action<float> OnCount;
    public event Action OnEnd;

    [Header("Unity Events (optional)")]
    public UnityEvent OnBeginEvent;
    public UnityEvent<float> OnCountEvent;
    public UnityEvent OnEndEvent;

    private void Start()
    {
        if (m_AutoStart)
            SetUp(m_DefaultTime);
    }

    public void SetUp(float time)
    {
        if (time <= 0f)
        {
            Debug.LogWarning("[TimeManager] Thời gian khởi tạo <= 0!");
            return;
        }

        StopTimer(); 

        m_RemainingTime = time;
        m_IsRunning = true;

        OnBegin?.Invoke();
        OnBeginEvent?.Invoke();

        RunTimerAsync().Forget(); 
    }

    public void StopTimer()
    {
        if (!m_IsRunning) return;

        m_IsRunning = false;

        OnEnd?.Invoke();
        OnEndEvent?.Invoke();
    }

    public void AddTime(float extra)
    {
        if (!m_IsRunning) return;
        m_RemainingTime += extra;
    }

    public void ReduceTime(float amount)
    {
        if (!m_IsRunning) return;
        m_RemainingTime = Mathf.Max(0f, m_RemainingTime - amount);
    }

    public float GetRemainingTime() => m_RemainingTime;
    public bool IsRunning => m_IsRunning;

    private async UniTaskVoid RunTimerAsync()
    {
        while (m_IsRunning && m_RemainingTime > 0f)
        {
            await UniTask.Delay(1000); 
            if (!m_IsRunning) break;

            m_RemainingTime = Mathf.Max(0f, m_RemainingTime - 1f);

            OnCount?.Invoke(m_RemainingTime);
            OnCountEvent?.Invoke(m_RemainingTime);
        }

        if (m_IsRunning) 
        {
            m_IsRunning = false;
            OnEnd?.Invoke();
            OnEndEvent?.Invoke();
        }
    }
}
