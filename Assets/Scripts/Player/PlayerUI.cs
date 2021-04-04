using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private Text text;
    [SerializeField] private Image pointerImage;
    [SerializeField] private Sprite defaultSprite;
    [SerializeField] private GameObject passwordPanel;
    [SerializeField] private Text passwordText;

    // Start is called before the first frame update
    void Start()
    {
        ClearPointer();
        ClearPassword();
    }

    public void ClearPointer()
    {
        text.text = string.Empty;
        pointerImage.sprite = defaultSprite;
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
}
