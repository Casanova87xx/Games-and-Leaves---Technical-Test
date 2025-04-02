using UnityEngine;

public enum RoundResult
{
    Win,
    Lose,
    Percentage
}

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Data")]
public class SO_Data : ScriptableObject
{
    [Header("Dynamic Data")]
    public int Gold;
    public string NextClaimDate;

    [Header("GAME SETTINGS")]
    public int MaxGold;
    public int GoldEarnedInRoulette;
    [Space(10)]
    [Tooltip("Set the time for the next claim")]
    public int DailyHourForClaim;
    
    [Header("WIN / LOSE messages")]
    public string WinMessage;
    public string LoseMessage;
    
    [Header("Percentage win for roulette")]
    public int PercentageWin;
    
    [Header("Round Results")]
    public RoundResult[] RoundResults;
}
