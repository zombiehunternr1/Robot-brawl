using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScenesManager : MonoBehaviour
{
    public static ScenesManager instance;
    public bool allowRestart { get; set; }

    [SerializeField]
    private GameEventEmpty allowPlayerInput;
    [SerializeField]
    private GameEventEmpty disAllowPlayerInput;
    [SerializeField]
    private Image fadePanel;
    [SerializeField]
    private float waitTillSwitchScenes;
    [SerializeField]
    private float fadeSpeed;
    private float fadeAmount;
    private bool fadeToBlack;
    private bool gameFinished;
    private Color currentFadeColor;

    private void OnEnable()
    {
        gameFinished = false;
        allowRestart = false;
        fadeToBlack = true;
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
        }
        else
        {
            Destroy(this);
        }
    }
    
    public void GameOver()
    {
        gameFinished = true;
        StartCoroutine(fadeEffect());
    }

    public void RestartGame()
    {
        if (allowRestart)
        {
            allowRestart = false;
            StartCoroutine(fadeEffect());
        }
    }

    public void SwitchScene()
    {
        disAllowPlayerInput.Raise();
        StartCoroutine(fadeEffect());
    }

    private void LoadMiniGameScene()
    {
        SceneManager.LoadScene("Game");
        StartCoroutine(fadeEffect());
    }

    private void LoadRankScene()
    {
        SceneManager.LoadScene("Rank");
        StartCoroutine(fadeEffect());
    }

    private IEnumerator fadeEffect()
    {
        if (fadeToBlack)
        {
            while(fadePanel.color.a < 1)
            {
                fadeAmount = currentFadeColor.a + (fadeSpeed * Time.deltaTime);
                currentFadeColor = new Color(currentFadeColor.r, currentFadeColor.g, currentFadeColor.b, fadeAmount);
                fadePanel.color = currentFadeColor;
                yield return null;
            }
            fadeToBlack = false;
            yield return new WaitForSeconds(waitTillSwitchScenes);
            if (gameFinished)
            {
                LoadRankScene();
            }
            else
            {
                LoadMiniGameScene();
            }
        }
        else
        {
            while (fadePanel.color.a > 0)
            {
                fadeAmount = currentFadeColor.a - (fadeSpeed * Time.deltaTime);
                currentFadeColor = new Color(currentFadeColor.r, currentFadeColor.g, currentFadeColor.b, fadeAmount);
                fadePanel.color = currentFadeColor;
                yield return null;
            }
            fadeToBlack = true;
            if(!gameFinished)
            {
                allowPlayerInput.Raise();
            }
        }
    }
}
