using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

/// <summary>
/// Управление интерфейсом
/// </summary>
[HelpURL("https://docs.google.com/document/d/1Gtygy76qzWktagW6KIqYZ_6ulFcFOMskSB0LZN84AL0/edit?usp=sharing")]
public class PlayerUI : MonoBehaviour
{
    public Animator damagePanel;
    public Animator shieldPanel;
    [Tooltip("Слайдер уровня заряда скафандра")] public Slider healthSlider;
    [Tooltip("Слайдер уровня заряда скафандра")] public Slider energySlider;
    public Text healItemsCount;
    [SerializeField, Tooltip("Текст - подсказка при наведении на интерактивный объект")] private Text text;
    [SerializeField, Tooltip("Текст - иконка прицела")] private Image pointerImage;
    [SerializeField, Tooltip("Стандартная иконка прицела")] private Sprite defaultPointerSprite;
    [SerializeField, Tooltip("Иконка эффекта от опасных зон")] private Image effectImage;
    [SerializeField, Tooltip("Панель, которая показывается при смерти - телепорт")] private Image deathPanel;
    [SerializeField, Tooltip("Панель меню")] private GameObject mainMenu;
    [SerializeField, Tooltip("Панель с задачей")] private GameObject taskPanel;
    [SerializeField, Tooltip("Текст задачи")] private Text taskText;
    [SerializeField, Tooltip("Финальная панель")] private GameObject finalPanel;
    [SerializeField, Tooltip("Иконка финальной панели")] private Image finalIcon;
    [SerializeField, Tooltip("Текст финальной панели")] private Text finalText;
    [SerializeField] private SettingUIPack settingsPack;

    public event Action foolAlphaDeathPanelEvent;
    public event Action noAlphaDeathPanelEvent;

    private bool endGame = false;
    private PlayerLook playerLook;
    private AsyncOperation sceneLoading;

    [HideInInspector]
    public UnityEvent<bool> pauseStateChanged = new UnityEvent<bool>();

    void Start()
    {
        ClearPointer();
        deathPanel.color = new Color(deathPanel.color.r, deathPanel.color.g, deathPanel.color.b, deathPanel.color.a);
        DeathPanelToZeroAlpha();
        mainMenu.SetActive(!mainMenu.activeSelf);
        Time.timeScale = mainMenu.activeSelf ? 0 : 1;
        playerLook = FindObjectOfType<PlayerLook>();
        StartCoroutine(PrepareSceneCoroutine());
        taskPanel.SetActive(false);
        finalPanel.SetActive(false);
        settingsPack.Init();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) && !endGame)
        {
            mainMenu.SetActive(!mainMenu.activeSelf);
            pauseStateChanged?.Invoke(mainMenu.activeSelf);
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

    public void SetEffect(Sprite sprite)
    {
        effectImage.enabled = true;
        effectImage.sprite = sprite;
    }
    public void ReturnEffectToDefault()
    {
        effectImage.enabled = false; ;
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

        noAlphaDeathPanelEvent?.Invoke();
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

[System.Serializable]
public class SettingUIPack
{
    public Slider sensivitySlider;
    public Slider musicSlider;
    public Slider voiceSlider;
    public Slider soundsSlider;
    public Toggle useSusbsToggle;

    public void Init()
    {
        sensivitySlider.value = SettingsHolder.Sensivity;
        sensivitySlider.onValueChanged.AddListener(InvokeSensivityChanged);
        musicSlider.value = SettingsHolder.Music;
        musicSlider.onValueChanged.AddListener(InvokeMusicVolumeChanged);
        voiceSlider.value = SettingsHolder.Voice;
        voiceSlider.onValueChanged.AddListener(InvokeVoiceVolumeChanged);
        soundsSlider.value = SettingsHolder.Effects;
        soundsSlider.onValueChanged.AddListener(InvokeSoundVolumeChanged);
        useSusbsToggle.isOn = SettingsHolder.UseSubs;
        useSusbsToggle.onValueChanged.AddListener(InvokeUseSubsChanged);
    }

    public void InvokeMusicVolumeChanged(float value)
    {
        SettingsHolder.Music = value;
    }
    public void InvokeVoiceVolumeChanged(float value)
    {
        SettingsHolder.Voice = value;
    }
    public void InvokeSoundVolumeChanged(float value)
    {
        SettingsHolder.Effects = value;
    }
    public void InvokeSensivityChanged(float value)
    {
        SettingsHolder.Sensivity = value;
    }
    public void InvokeUseSubsChanged(bool value)
    {
        SettingsHolder.UseSubs = value;
    }
}

public static class SettingsHolder
{
    public static float Sensivity
    {
        get 
        { 
            return _sensivity;
        }
        set 
        {
            _sensivity = value;
            SensivityChanged?.Invoke(value);
        }
    }
    private static float _sensivity = 0.5f;

    public static float Music
    {
        get
        {
            return _music;
        }
        set
        {
            _music = value;
            MusicVolumeChanged?.Invoke(value);
        }
    }
    private static float _music = 1;

    public static float Effects
    {
        get
        {
            return _sounds;
        }
        set
        {
            _sounds = value;
            SoundsVolumeChanged?.Invoke(value);
        }
    }
    private static float _sounds = 1;

    public static float Voice
    {
        get
        {
            return _voice;
        }
        set
        {
            _voice = value;
            VoiceVolumeChanged?.Invoke(value);
        }
    }
    private static float _voice = 1;

    public static bool UseSubs
    {
        get
        {
            return _useSubs;
        }
        set
        {
            _useSubs = value;
            UseSubsChanged?.Invoke(value);
        }
    }
    private static bool _useSubs = true;

    public static UnityEvent<float> VoiceVolumeChanged = new UnityEvent<float>();
    public static UnityEvent<float> MusicVolumeChanged = new UnityEvent<float>();
    public static UnityEvent<float> SoundsVolumeChanged = new UnityEvent<float>();
    public static UnityEvent<float> SensivityChanged = new UnityEvent<float>();
    public static UnityEvent<bool> UseSubsChanged = new UnityEvent<bool>();
}
