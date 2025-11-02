using System.Collections;
using UnityEngine;
using Data;
using UnityEngine.AI;

public class UIManager : SingletonDontDestroyOnLoad<UIManager>
{  
    [SerializeField] private UIStates _state = UIStates.None;
    
    GameUI gameUI = null;
    private void Start()
    {
        gameUI = GameUI.Instance;
        ChangeState(UIStates.Home); 
    }

    public void ChangeState(UIStates newState)
    {
        if (newState == _state) return;
        
        ExitCurrentState();
        _state = newState;
        EnterNewState();
    }

    private void EnterNewState()
    {
        switch (_state)
        {
            case UIStates.Home:

                break;
            case UIStates.Play:

                break;
            case UIStates.Setting:

                break;
            default:
                break;
        }
    }

    public void ExitCurrentState()
    {
        switch (_state)
        {
            case UIStates.Home:
               
                break;
            case UIStates.Play:

                break;
            case UIStates.Setting:
   
                break;
            default:
                break;
        }
    }

    public void EnterGame()
    {
        ChangeState(UIStates.Play);
    }
}

public enum UIStates
{
    Play, Home, Setting, None
}
