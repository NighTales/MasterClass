using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

/// <summary>
/// Управление интерфейсом
/// </summary>
public class PlayerUI : MonoBehaviour
{
    public Slider energySlider;
    [SerializeField] private Text text;
    [SerializeField] private Image pointerImage;
    [SerializeField] private Sprite defaultPointerSprite;
    [SerializeField] private GameObject passwordPanel;
    [SerializeField] private Text passwordText;
    [SerializeField] private Image effectImage;
    [SerializeField] private Sprite defaultEffectSprite;
    [SerializeField] private Image deathPanel;
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject taskPanel;
    [SerializeField] private Text taskText;
    [SerializeField] private GameObject finalPanel;
    [SerializeField] private Image finalIcon;
    [SerializeField] private Text finalText;

    public event Action foolAlphaDeathPanelEvent;
    public event Action noAlphaDeathPanelEvent;

    private bool endGame = false;
    private PlayerLook playerLook;
    private AsyncOperation sceneLoading;

    void Start()
    {
        ClearPointer();
        ClearPassword();
        deathPanel.color = new Color(deathPanel.color.r, deathPanel.color.g, deathPanel.color.b, deathPanel.color.a);
        DeathPanelToZeroAlpha();
        mainMenu.SetActive(!mainMenu.activeSelf);
        Time.timeScale = mainMenu.activeSelf ? 0 : 1;
        playerLook = FindObjectOfType<PlayerLook>();
        StartCoroutine(PrepareSceneCoroutine());
        taskPanel.SetActive(false);
        finalPanel.SetActive(false);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) && !endGame)
        {
            mainMenu.SetActive(!mainMenu.activeSelf);
            Time.timeScale = mainMenu.activeSelf ? 0 : 1;
            playerLook.SetCursorVisible(mainMenu.activeSelf);
            if (mainMenu.activeSelf)
            {
                SetTask(taskText.text);
            }
        }
    }

    public void ClearPointer()
    {
        text.text = string.Empty;
        pointerImage.sprite = defaultPointerSprite;
    }

    public void SetPointerVisible(bool value)
    {
        pointerImage.enabled = value;
    }

    public void SetMessage(string message, Sprite icon)
    {
        text.text = message;
        pointerImage.sprite = icon;
    }

    public void SetPassword(string password)
    {
        passwordPanel.SetActive(true);
        passwordText.text = password;
    }
    public void ClearPassword()
    {
        passwordPanel.SetActive(false);
    }

    public void SetEffect(Sprite sprite)
    {
        effectImage.sprite = sprite;
    }
    public void ReturnEffectToDefault()
    {
        effectImage.sprite = defaultEffectSprite;
    }

    public void DeathPanelToFoolAlpha()
    {
        StartCoroutine(DeathPanelToFoolAlphaCoroutine());
    }
    public void DeathPanelToZeroAlpha()
    {
        StartCoroutine(DeathPanelToZeroCoroutine());
    }

    public void OnExitClick()
    {
        #if UNITY_EDITOR
        Debug.Log("Выход из игры");
        #endif
        Application.Quit();
    }
    public void OnRestartClick()
    {
        sceneLoading.allowSceneActivation = true;
    }

    public void SetTask(string taskString)
    {
        taskPanel.SetActive(true);
        taskText.text = taskString;
        StartCoroutine(ViewTaskCoroutine());
    }

    public void SetFinal(Sprite icon, string text)
    {
        StartCoroutine(FinalCoroutine(icon, text));
    }

    private IEnumerator FinalCoroutine(Sprite finalSprite, string text)
    {
        Color noAlphaColor = deathPanel.color;
        Color foolAlphaColor = deathPanel.color;
        noAlphaColor.a = 0;
        foolAlphaColor.a = 1;
        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime;
            deathPanel.color = Color.Lerp(noAlphaColor, foolAlphaColor, t);
            yield return null;
        }

        deathPanel.color = foolAlphaColor;

        mainMenu.SetActive(true);
        Time.timeScale = 0;
        playerLook.SetCursorVisible(true);

        finalPanel.SetActive(true);
        finalIcon.sprite = finalSprite;
        finalText.text = text;
    }
    private IEnumerator ViewTaskCoroutine()
    {
        yield return new WaitForSeconds(5);
        taskPanel.SetActive(false);
    }
    private IEnumerator DeathPanelToFoolAlphaCoroutine()
    {
        Color noAlphaColor = deathPanel.color;
        Color foolAlphaColor = deathPanel.color;
        noAlphaColor.a = 0;
        foolAlphaColor.a = 1;
        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime;
            deathPanel.color = Color.Lerp(noAlphaColor, foolAlphaColor, t);
            yield return null;
        }

        deathPanel.color = foolAlphaColor;

        foolAlphaDeathPanelEvent?.Invoke();

        while (t > 0)
        {
            t -= Time.deltaTime;
            deathPanel.color = Color.Lerp(noAlphaColor, foolAlphaColor, t);
            yield return null;
        }
    }
    private IEnumerator DeathPanelToZeroCoroutine()
    {
        Color noAlphaColor = deathPanel.color;
        Color foolAlphaColor = deathPanel.color;
        noAlphaColor.a = 0;
        foolAlphaColor.a = 1;
        float t = 1;
        deathPanel.color = foolAlphaColor;

        while (t > 0)
        {
            t -= Time.deltaTime;
            deathPanel.color = Color.Lerp(noAlphaColor, foolAlphaColor, t);
            yield return null;
        }

        deathPanel.color = noAlphaColor;
        ReturnEffectToDefault();
        noAlphaDeathPanelEvent?.Invoke();
    }
    private IEnumerator PrepareSceneCoroutine()
    {
        sceneLoading = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
        sceneLoading.allowSceneActivation = false;
        while (!sceneLoading.isDone)
        {
            yield return null;
        }
    }
}
