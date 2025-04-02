using UnityEngine;
using TMPro;

public class ShowCurrentTime : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI currentTimeLabel;

    // Update is called once per frame
    void Update()
    {
        currentTimeLabel.text = TimerUtility.CurrentTime.ToString("F");
    }
}
