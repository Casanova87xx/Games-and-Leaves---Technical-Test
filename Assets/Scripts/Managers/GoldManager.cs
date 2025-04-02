using System;
using UnityEngine;

public class GoldManager : MonoBehaviour
{
    public static Action<int> GoldChanged;

    void Awake()
    {
        GoldChanged += OnGoldChanged;
    }

    private void OnGoldChanged(int pAmount)
    {
        ResourcesManager.Instance.Data.Gold += pAmount;
        
        if (ResourcesManager.Instance.Data.Gold < 0)
        {
            ResourcesManager.Instance.Data.Gold = 0;
        } 
        else if (ResourcesManager.Instance.Data.Gold > ResourcesManager.Instance.Data.MaxGold)
        {
            ResourcesManager.Instance.Data.Gold = ResourcesManager.Instance.Data.MaxGold;
        }
        
        UIManager.Instance.ShowGold();
    }

    void OnDestroy()
    {
        GoldChanged -= OnGoldChanged;
    }
}