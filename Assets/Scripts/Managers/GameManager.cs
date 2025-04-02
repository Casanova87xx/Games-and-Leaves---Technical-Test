using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static Action OnGoGame;
    public static Action OnGoMenu;

    void Start()
    {
        GoMenu();
    }

    public void GoGame()
    {
        OnGoGame?.Invoke();
    }
    
    public void GoMenu()
    {
        OnGoMenu?.Invoke();
    }
}
