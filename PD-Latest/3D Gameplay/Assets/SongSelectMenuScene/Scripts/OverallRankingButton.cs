using UnityEngine;
using TMPro;

public class OverallRankingButton : MonoBehaviour {

    public TextMeshProUGUI songRankingText, overallRankingText;

    // Show the song ranking text
    public void ShowSongRankingText()
    {
        songRankingText.gameObject.SetActive(true);
        overallRankingText.gameObject.SetActive(false);
    }

    // Show the overall ranking text
    public void ShowOverallRankingText()
    {
        songRankingText.gameObject.SetActive(false);
        overallRankingText.gameObject.SetActive(true);
    }
}
