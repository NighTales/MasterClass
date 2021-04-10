using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    // Start is called before the first frame update
    void Start()
    {
        ClearPointer();
        ClearPassword();
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
}
