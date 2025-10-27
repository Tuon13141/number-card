using TMPro;
using UnityEngine;

public class StackCounter : MonoBehaviour
{
    [SerializeField] private TextMeshPro m_CountText;
    [SerializeField] private GameObject m_TextHolder;

    public void SetText(int count)
    {
        if(count <= 1)
        {
            m_TextHolder.SetActive(false);
            return;
        }
        else
        {
            m_TextHolder.SetActive(true);
            m_CountText.text = count.ToString();
        }
    }
}
