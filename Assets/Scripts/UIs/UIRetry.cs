using UnityEngine;
using UnityEngine.UI;

public class UIRetry : UIElement
{
    public override bool ManualHide => true;

    public override bool DestroyOnHide => false;

    public override bool UseBehindPanel => false;

    [SerializeField] Button m_RetryButton;

    void Start()
    {
        m_RetryButton.onClick.AddListener(RetryButton);
    }

    public void RetryButton()
    {
        GameManager.Instance.OnReset();
        GameManager.Instance.StartLevel();
        Hide();
    }
}
