using UnityEngine;
using TMPro;

public class OverallRankingButton : MonoBehaviour {

    public TextMeshProUGUI songRankingText, overallRankingText;
    public Canvas overallRankingCanvas;

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

    // Activate the overall ranking leaderboard
    public void ShowOverallRankingCanvas()
    {
        if (overallRankingCanvas.gameObject.activeSelf == false)
        {
            overallRankingCanvas.gameObject.SetActive(true);
        }
    }

    // Hide the overall ranking canvas
    public void HideOverallRankingCanvas()
    {
        if (overallRankingCanvas.gameObject.activeSelf == true)
        {
            overallRankingCanvas.gameObject.SetActive(false);
        }
    }
}
