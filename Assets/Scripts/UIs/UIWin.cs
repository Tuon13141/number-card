using UnityEngine;
using UnityEngine.UI;

public class UIWin : UIElement
{
    public override bool ManualHide => true;

    public override bool DestroyOnHide => false;

    public override bool UseBehindPanel => false;

    [SerializeField] Button m_PlayButton;

    void Start()
    {
        m_PlayButton.onClick.AddListener(PlayButton);
    }

    public void PlayButton()
    {
        GameManager.Instance.StartLevel();
        Hide();
    }
}