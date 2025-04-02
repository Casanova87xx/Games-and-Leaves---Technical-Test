using System.Collections;
using TMPro;
using UnityEngine;
using Button = UnityEngine.UI.Button;

public class RouletteController : MonoBehaviour
{
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private Button _playButton;
    [SerializeField] private Button _backButton;
    [SerializeField] private RectTransform _rouletteRT;
    [SerializeField] private TextMeshProUGUI _messageText;
    [SerializeField] private ButtonOption[] _optionButtons;
    private int _currentOption;

    private int _spinNumber;

    private bool _playing;

    private int _degreesPerNumber;
    private float initialRouletteRotation;

    void Awake()
    {
        _playButton.interactable = false;
        _degreesPerNumber = 360 / _optionButtons.Length;
        initialRouletteRotation = _rouletteRT.eulerAngles.z;
    }

    public void SelectOption(ButtonOption pOption)
    {
        ResetButtons();
        ResetRoulette();
        pOption.Select();
        
        _currentOption = pOption.Option;
        _playButton.interactable = true;
    }

    private void ResetRoulette()
    {
        _rouletteRT.eulerAngles = new Vector3(0, 0, initialRouletteRotation);
    }

    public void Play()
    {
        if (_playing)
        {
            return;
        }

        _messageText.text = string.Empty;
        _playButton.interactable = false;
        _playing = true;
        
        foreach (ButtonOption _optionButtons in _optionButtons)
        {
            _optionButtons.Disable();
        }
        
        GoldManager.GoldChanged?.Invoke(-1);
        Invoke(nameof(Spin), 1.0f);
    }

    private void Spin()
    {
        _backButton.interactable = false;
        RoundResult result = ResourcesManager.Instance.Data.RoundResults[_spinNumber];
        _spinNumber++;
        if (_spinNumber >= ResourcesManager.Instance.Data.RoundResults.Length)
        {
            _spinNumber = ResourcesManager.Instance.Data.RoundResults.Length - 1;
        }

        switch (result)
        {
            case RoundResult.Win:
                Win();
                break;
            case RoundResult.Lose:
                Lose();
                break;
            case RoundResult.Percentage:
                int random = Random.Range(0, 100);
                if (random < ResourcesManager.Instance.Data.PercentageWin)
                {
                    Win();
                }
                else
                {
                    Lose();
                }
                break;
        }
    }

    private void Win()
    {
        StartCoroutine(SpinRoulette(true));
    }

    private void Lose()
    {
        StartCoroutine(SpinRoulette(false));
    }

    private IEnumerator SpinRoulette(bool pIsWin)
    {
        float finalRotation = 0f;
        float totalRotation = 0f;
    
        if (pIsWin)
        {
            finalRotation = 2160 + Random.Range(_degreesPerNumber * (_currentOption - 1), _degreesPerNumber * _currentOption);    
        }
        else
        {
            finalRotation = 2160 + Random.Range(_degreesPerNumber * (_currentOption + Random.Range(0, 3)), 
                _degreesPerNumber * (_currentOption + Random.Range(1, 3)));    
        }

        while (totalRotation < finalRotation)
        {
            float rotationStep = _rotationSpeed * Time.deltaTime;
            _rouletteRT.Rotate(Vector3.forward * rotationStep);
            totalRotation += rotationStep;
            yield return null;
        }

        _rouletteRT.eulerAngles = new Vector3(0, 0, finalRotation % 360);
        

        if (pIsWin)
        {
            GoldManager.GoldChanged?.Invoke(ResourcesManager.Instance.Data.GoldEarnedInRoulette);
            _messageText.text = "<color=yellow>"+ResourcesManager.Instance.Data.WinMessage+"</color>";
        }
        else
        {
            _messageText.text = "<color=red>"+ResourcesManager.Instance.Data.LoseMessage+"</color>";
        }
        
        Reset();
    }
    
    private void Reset()
    {
        ResetButtons();
        
        _backButton.interactable = true;
        _playButton.interactable = false;
        _playing = false;
    }

    private void ResetButtons()
    {
        foreach (ButtonOption buttonOption in _optionButtons)
        {
            buttonOption.Reset();
        }
    }
}