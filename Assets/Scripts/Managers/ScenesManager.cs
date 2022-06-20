using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScenesManager : MonoBehaviour
{
    [SerializeField]
    private GameEventEmpty allowPlayerInput;
    [SerializeField]
    private GameEventEmpty disAllowPlayerInput;
    [SerializeField]
    private Image fadePanel;
    [SerializeField]
    private float fadeSpeed;
    private float fadeAmount;
    private bool fadeToBlack;
    private Color currentFadeColor;

    private void OnEnable()
    {
        fadeToBlack = true;
        DontDestroyOnLoad(this);
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
            LoadMiniGameScene();
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
            allowPlayerInput.Raise();
        }
    }
}
