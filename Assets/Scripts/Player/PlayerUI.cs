using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private Text text;
    [SerializeField] private Image pointerImage;
    [SerializeField] private Sprite defaultSprite;

    // Start is called before the first frame update
    void Start()
    {
        Clear();
    }

    public void Clear()
    {
        text.text = string.Empty;
        pointerImage.sprite = defaultSprite;
    }

    public void SetMessage(string message, Sprite icon)
    {
        text.text = message;
        pointerImage.sprite = icon;
    }
}
