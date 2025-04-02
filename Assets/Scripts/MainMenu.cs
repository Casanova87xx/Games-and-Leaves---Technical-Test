using System;
using System.Globalization;
using UnityEngine;
using TMPro;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private BlockingWait blockingWaitPopup;
    [SerializeField] private TMP_Text claimCooldownText;
    
    public static Action<bool> OnCheckClaim;

    private int claimHour;
    private bool _firstCheck;
    private bool _isWaitingForClaim;

    private void Start()
    {
        claimHour = ResourcesManager.Instance.Data.DailyHourForClaim;
        OnCheckClaim?.Invoke(CheckClaim());
    }

    private void Update()
    {
        if (_isWaitingForClaim)
        {
            UpdateCooldownText();
        }
    }

    public void Action_SpendOneCoin()
    {
        Debug.Log("MainMenu:Action_SpendOneCoin");
        GoldManager.GoldChanged?.Invoke(-1);
    }

    public void Action_GetExtraCoin()
    {
        Debug.Log("MainMenu:Action_GetExtraCoin");
        GoldManager.GoldChanged?.Invoke(1);
        blockingWaitPopup.gameObject.SetActive(true);
    }

    public void Action_ClaimFreeCoin()
    {
        Debug.Log("MainMenu:Action_ClaimFreeCoin");

        if (!CheckClaim()) return;

        ClaimGold();
        OnCheckClaim?.Invoke(false);
    }

    private bool CheckClaim()
    {
        DateTime now = TimerUtility.CurrentTime;

        if (string.IsNullOrEmpty(ResourcesManager.Instance.Data.NextClaimDate))
        {
            _isWaitingForClaim = true;
            return true;
        }

        DateTime nextClaimDate = DateTime.ParseExact(ResourcesManager.Instance.Data.NextClaimDate, "MM/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture);

        if (now >= nextClaimDate)
        {
            _isWaitingForClaim = false;
            return true;
        }

        _isWaitingForClaim = true;
        EnableClaimDelay(nextClaimDate - now);
        return false;
    }

    private void SetNextClaimDate(DateTime now)
    {
        DateTime nextClaimTime = now.Hour < claimHour ? 
            new DateTime(now.Year, now.Month, now.Day, claimHour, 0, 0) : 
            new DateTime(now.Year, now.Month, now.Day, claimHour, 0, 0).AddDays(1);

        ResourcesManager.Instance.Data.NextClaimDate = nextClaimTime.ToString("MM/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
    }

    private void EnableClaimDelay(TimeSpan delay)
    {
        CancelInvoke(nameof(ForceCheckClaim));
        if (delay.TotalSeconds > 0)
        {
            Invoke(nameof(ForceCheckClaim), (float)delay.TotalSeconds);
        }
    }

    private void ForceCheckClaim()
    {
        OnCheckClaim?.Invoke(CheckClaim());
    }

    private void UpdateCooldownText()
    {
        DateTime now = TimerUtility.CurrentTime;

        if (DateTime.TryParseExact(ResourcesManager.Instance.Data.NextClaimDate, "MM/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime nextClaimDate))
        {
            TimeSpan remaining = nextClaimDate - now;

            if (remaining.TotalSeconds <= 0)
            {
                claimCooldownText.text = "Reclaim enabled!";
                _isWaitingForClaim = false;
                OnCheckClaim?.Invoke(true);
            }
            else
            {
                claimCooldownText.text = $"COOLDOWN: {remaining.Hours:D2}:{remaining.Minutes:D2}:{remaining.Seconds:D2}";
            }
        }
    }

    private void ClaimGold()
    {
        GoldManager.GoldChanged?.Invoke(1);
        SetNextClaimDate(TimerUtility.CurrentTime);
    }
}