using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class Menu : MonoBehaviour {

    public List<GameObject> pages = new List<GameObject>();
    public int currentPageIndex = 0;
    public GameObject currentPage;
    public CanvasGroup cg;

    public bool fadeOut = false;
    public bool fadeIn = false;
    public float fadeRate = 8f;
    public float growRate = 0.5f;


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
            newPage(0);
        }
        if (buttonSelectedPass == "MULTIPLAYER")
        {
            newPage(1);
        }
        if (buttonSelectedPass == "EDITOR")
        {
            newPage(2);
        }
        if (buttonSelectedPass == "SETTINGS")
        {
            newPage(3);
        }
        if (buttonSelectedPass == "EXIT")
        {
            newPage(4);
        }
    }

    // Close the game
    public void ExitGame()
    {
        Application.Quit();
    }
}
