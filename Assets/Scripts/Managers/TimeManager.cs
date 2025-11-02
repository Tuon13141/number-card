using System;
using UnityEngine;
using UnityEngine.Events;
using Cysharp.Threading.Tasks;
using System.Threading;

[DefaultExecutionOrder(-200)]
public class TimeManager : Singleton<TimeManager>
{
    [Header("Time Settings")]
    [SerializeField] private bool m_AutoStart = false;
    [SerializeField, Tooltip("Thời gian đếm ban đầu (giây)")]
    private float m_DefaultTime = 10f;

    private float m_RemainingTime;
    private bool m_IsRunning;
    private CancellationTokenSource m_CTS;

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

        StopTimer(false); // Dừng trước đó nếu có

        m_RemainingTime = time;
        OnCount?.Invoke(m_RemainingTime);
        m_IsRunning = true;

        m_CTS = new CancellationTokenSource();

        OnBegin?.Invoke();
        OnBeginEvent?.Invoke();

        RunTimerAsync(m_CTS.Token).Forget();
    }

    public void StopTimer(bool trigger = true)
    {
        if (!m_IsRunning) return;

        m_IsRunning = false;

        // Hủy task đang chạy
        m_CTS?.Cancel();
        m_CTS?.Dispose();
        m_CTS = null;

        if (!trigger) return;

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

    private async UniTaskVoid RunTimerAsync(CancellationToken token)
    {
        try
        {
            while (m_IsRunning && m_RemainingTime > 0f)
            {
                await UniTask.Delay(1000, cancellationToken: token);
                if (!m_IsRunning) break;

                m_RemainingTime = Mathf.Max(0f, m_RemainingTime - 1f);

                OnCount?.Invoke(m_RemainingTime);
                OnCountEvent?.Invoke(m_RemainingTime);
            }
        }
        catch (OperationCanceledException)
        {
            // Bị hủy — không làm gì thêm
            return;
        }

        if (m_IsRunning)
        {
            m_IsRunning = false;
            OnEnd?.Invoke();
            OnEndEvent?.Invoke();
        }
    }
}
