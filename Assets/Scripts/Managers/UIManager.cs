using System;
using TMPro;
using UnityEngine;
using Button = UnityEngine.UI.Button;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    
    [SerializeField] private TextMeshProUGUI goldText;
    [SerializeField] private Button claimButton;
    [SerializeField] private Canvas mainMenuCanvas;
    [SerializeField] private Canvas gameMenuCanvas;
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        
        MainMenu.OnCheckClaim += UpdateClaimButton;
        GameManager.OnGoGame += GoGame;
        GameManager.OnGoMenu += GoMenu;
    }

    void Start()
    {
        ShowGold();
    }

    private void GoGame()
    {
        mainMenuCanvas.gameObject.SetActive(false);
        gameMenuCanvas.gameObject.SetActive(true);
    }

    private void GoMenu()
    {
        gameMenuCanvas.gameObject.SetActive(false);
        mainMenuCanvas.gameObject.SetActive(true);
    }

    private void UpdateClaimButton(bool pEnabled)
    {
        claimButton.interactable = pEnabled;
    }

    private void UpdateGoldText(int pAmount)
    {
        ShowGold();
    }
    
    public void ShowGold()
    {
        goldText.text = ResourcesManager.Instance.Data.Gold.ToString() + " / " + ResourcesManager.Instance.Data.MaxGold;
    }

    void OnDestroy()
    {
        GoldManager.GoldChanged -= UpdateGoldText;
        MainMenu.OnCheckClaim -= UpdateClaimButton;
        GameManager.OnGoGame -= GoGame;
        GameManager.OnGoMenu -= GoMenu;
    }
}
