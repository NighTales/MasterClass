using UnityEngine;
using UnityEngine.UI;

public class MessageItem : MonoBehaviour
{
    public Text messageTextBlock;
}

[System.Serializable]
public class MessageInfo
{
    public bool groupMessageFromOne;
    public string sender;
    [TextArea]
    public string message;
}

public static class RectTransformExtensions
{
    public static Vector2 GetSize(this RectTransform source) => source.rect.size;
    public static float GetWidth(this RectTransform source) => source.rect.size.x;
    public static float GetHeight(this RectTransform source) => source.rect.size.y;

    /// <summary>
    /// Sets the sources RT size to the same as the toCopy's RT size.
    /// </summary>
    public static void SetSize(this RectTransform source, RectTransform toCopy)
    {
        source.SetSize(toCopy.GetSize());
    }

    /// <summary>
    /// Sets the sources RT size to the same as the newSize.
    /// </summary>
    public static void SetSize(this RectTransform source, Vector2 newSize)
    {
        source.SetSize(newSize.x, newSize.y);
    }

    /// <summary>
    /// Sets the sources RT size to the new width and height.
    /// </summary>
    public static void SetSize(this RectTransform source, float width, float height)
    {
        source.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
        source.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
    }

    public static void SetWidth(this RectTransform source, float width)
    {
        source.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
    }

    public static void SetHeight(this RectTransform source, float height)
    {
        source.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
    }
}