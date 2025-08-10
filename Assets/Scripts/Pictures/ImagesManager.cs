using System.Collections;
using UnityEngine;

public class ImagesManager : MonoBehaviour
{
    public GameObject deathScreenUI;
    public GameObject infoScreenUI;
    public GameObject introImage;
    public GameObject gjImage;
    public GameObject timer;

    //cutscene delovi
    public IntroCutscene cutscene;
    public bool skipCutscene = false; //cisto togl za kad testiram

    //end scene delovi
    public GameObject blackSc;
    public GameObject bubble1;
    public GameObject bubble2;
    public GameObject bubble3;
    public GameObject end_question;

    //menu delovi
    public GameObject menuScreenUI;
    private bool isMenuActive = false;

    private void Awake()
    {
        if(deathScreenUI != null)
            deathScreenUI.SetActive(false);
        if(infoScreenUI != null)
            infoScreenUI.SetActive(false);
        if(gjImage != null)
            gjImage.SetActive(false);
        if(introImage != null)
            introImage.SetActive(false);
        if(timer != null)
            timer.SetActive(false);
        //if(blackSc != null)
        //    blackSc.SetActive(false);
        if(bubble1 != null)
            bubble1.SetActive(false);
        if(bubble2 != null) 
            bubble2.SetActive(false);
        if(bubble3 != null) 
            bubble3.SetActive(false);
        if(end_question != null)
            end_question.SetActive(false);
        if(menuScreenUI != null)
            menuScreenUI.SetActive(false);


        if (cutscene != null && !skipCutscene)
        {
            cutscene.OnCutsceneEnd += HandleCutsceneEnd;
            cutscene.BeginCutscene();
        }
        else
        {
            StartCoroutine(ShowIntroImage());
        }
    }

    private void HandleCutsceneEnd()
    {
        StartCoroutine(ShowIntroImage());
    }



    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I) && infoScreenUI != null)
        {
            infoScreenUI.SetActive(!infoScreenUI.activeSelf);

        }
        //za menu
        if (Input.GetKeyDown(KeyCode.M))
        {
            if (!isMenuActive)
            {
                OpenMenu();
            }
            else 
            {
                CloseMenu();
            }
        }
        if (isMenuActive) 
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                RestartGame();
            }
            else if (Input.GetKeyDown(KeyCode.X)) 
            {
                ExitGame();
            }
        }
    }

    private IEnumerator ShowIntroImage()
    {
        if (introImage != null)
        {
            Time.timeScale = 0f;
            introImage.SetActive(true);
            yield return new WaitForSecondsRealtime(3f);

            introImage.SetActive(false);
            Time.timeScale = 1f;
        }
    }

    //za menu delovi
    private void OpenMenu()
    {
        if (menuScreenUI == null) return;

        menuScreenUI.SetActive(true);
        Time.timeScale = 0f;
        isMenuActive = true;

        // da zatvori i info ako nije vec
        if (infoScreenUI != null && infoScreenUI.activeSelf)
            infoScreenUI.SetActive(false);
    }

    private void CloseMenu()
    {
        if (menuScreenUI == null) return;

        menuScreenUI.SetActive(false);
        Time.timeScale = 1f;
        isMenuActive = false;
    }

    private void RestartGame()
    {
        Time.timeScale = 1f;
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }

    private void ExitGame()
    {
        Time.timeScale = 1f;
        Application.Quit();
    }

}
