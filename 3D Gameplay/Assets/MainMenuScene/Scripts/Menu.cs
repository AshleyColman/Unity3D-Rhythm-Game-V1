using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class Menu : MonoBehaviour {

    public List<GameObject> pages = new List<GameObject>();
    public int currentPageIndex = 0;
    public GameObject currentPage;
    public CanvasGroup cg;

    public bool fadeOut = false;
    public bool fadeIn = false;
    public float fadeRate = 8f;
    public float growRate = 0.5f;

    public TextMeshProUGUI modeDescriptionText;
    public Animator modeDescriptionTextAnimator;

    string playTextValue = "PLAY: Complete against others on community created Beatmaps";
    string createTextValue = "CREATE: Create your very own Beatmap for others to play";
    string optionTextValue = "OPTIONS: Coming soon";
    string discordTextValue = "DISCORD: Join the Discord community <3";
    string exitTextValue = "EXIT: Thanks for playing!";

    void Start()
    {

    }


    // Update is called once per frame
    void Update()
    {

        if (fadeOut)
        {
            cg.alpha -= fadeRate * Time.deltaTime;
        }
        if (fadeIn)
        {
            cg.alpha += fadeRate * Time.deltaTime;
            currentPage.transform.localScale += Vector3.one * growRate * Time.deltaTime;
        }

    }

    public void newPage(int newPage)
    {
        if (newPage != currentPageIndex)
        {
            StartCoroutine("ChangePage", newPage);
        }
    }

    public IEnumerator ChangePage(int newPage)
    {
        // Deactivate current page
        cg = currentPage.GetComponent<CanvasGroup>();
        cg.alpha = 1f;
        fadeIn = false;
        fadeOut = true;

        while (cg.alpha > 0)
        {
            yield return 0;
        }
        currentPage.SetActive(false);

        // Open new page
        fadeIn = true;
        fadeOut = false;
        currentPageIndex = newPage;
        currentPage = pages[currentPageIndex];
        currentPage.SetActive(true);

        cg = currentPage.GetComponent<CanvasGroup>();
        cg.alpha = 0f;
        currentPage.transform.localScale = Vector3.one * 0.95f;

        while (cg.alpha < 1f || currentPage.transform.localScale.x < 1f)
        {
            yield return 0;
        }
        cg.alpha = 1f;
        currentPage.transform.localScale = Vector3.one;
        fadeIn = false;
    }


    public void UpdatePage(string buttonSelectedPass)
    {
        if (buttonSelectedPass == "PLAY")
        {
            modeDescriptionText.text = playTextValue;
        }
        if (buttonSelectedPass == "DISCORD")
        {
            modeDescriptionText.text = discordTextValue;
        }
        if (buttonSelectedPass == "CREATE")
        {
            modeDescriptionText.text = createTextValue;

        }
        if (buttonSelectedPass == "OPTIONS")
        {
            modeDescriptionText.text = optionTextValue;
        }
        if (buttonSelectedPass == "EXIT")
        {
            modeDescriptionText.text = exitTextValue;
        }

        // Play text animation
        modeDescriptionTextAnimator.Play("ModeDescriptionTextAnimation");
    }

    // Close the game
    public void ExitGame()
    {
        Application.Quit();
    }

}
