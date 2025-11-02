using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIInGame : UIElement
{
    public override bool ManualHide => true;

    public override bool DestroyOnHide => false;

    public override bool UseBehindPanel => false;

    [SerializeField] Button m_RetryButton;
    [SerializeField] TextMeshProUGUI m_TimeText;
    [SerializeField] TextMeshProUGUI m_LevelText;


    void Start()
    {
        m_RetryButton.onClick.AddListener(RetryButton);
    }

    public void RetryButton()
    {
        GameManager.Instance.OnReset();
        GameManager.Instance.StartLevel();
    }

    public void SetTimeText(float time)
    {
        // Convert seconds to minutes and seconds
        int minutes = (int)(time / 60);
        int seconds = (int)(time % 60);

        // Format as mm:ss (with leading zeros)
        m_TimeText.text = $"{minutes:D2}:{seconds:D2}";
    }

    public void SetLevelText(int level)
    {
        m_LevelText.text = "Level " + level.ToString();
    }
    public override void Show()
    {
        base.Show();

        TimeManager.Instance.OnCount += SetTimeText;
    }

    public override void Hide()
    {
        base.Hide();

        TimeManager.Instance.OnCount -= SetTimeText;
    }
}