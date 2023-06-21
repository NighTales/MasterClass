using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageSystem : MonoBehaviour
{
    [SerializeField]
    private ComputerModule computerModule;
    [SerializeField]
    private List<MessageDatabase> messageSessions;
    [SerializeField]
    private GameObject messageButtonPrefab;
    [SerializeField]
    private GameObject myMessagePrefab;
    [SerializeField] 
    private GameObject friendMessageItem;
    [SerializeField]
    private Transform messagesConten;
    [SerializeField] 
    private Transform messagesScrollViewConten;

    private const int heightForOneString = 29;
    private const int widthForOneCharacter = 4;

    public void DrawMessagesFromBaseWithIndex(int index)
    {
        for (int i = 0; i < messagesScrollViewConten.childCount; i++)
        {
            Destroy(messagesScrollViewConten.GetChild(i).gameObject);
        }

        RectTransform messageItemRectTransform;

        foreach (var item in messageSessions[index].messages)
        {
            if (item.groupMessageFromOne || !item.sender.Equals(computerModule.profile.login))
            {
                messageItemRectTransform = Instantiate(friendMessageItem, messagesScrollViewConten).GetComponent<RectTransform>();
            }
            else
            {
                messageItemRectTransform = Instantiate(myMessagePrefab, messagesScrollViewConten).GetComponent<RectTransform>();
            }

            MessageItem messageItem = messageItemRectTransform.GetComponent<MessageItem>();
            messageItem.messageTextBlock.text = item.message;

            int partsCount = item.message.Split('\n').Length;

            //float parts = 0;
            //if (partsCount > 0) 
            //{
            //   parts = (partsCount) * heightForOneString;
            //}

            //Canvas.ForceUpdateCanvases();
            //float lines = messageItem.messageTextBlock.cachedTextGenerator.lineCount;
            //if (lines > 1)
            //{
            //    lines++;
            //}
           float height = 
                messageItem.messageTextBlock.cachedTextGenerator.GetPreferredHeight(item.message, messageItem.messageTextBlock.GetGenerationSettings(new Vector2(500, 500)));
            if (height > heightForOneString*2)
            {
                height += height/heightForOneString * 12;
            }
            messageItemRectTransform.SetHeight(height + 10);
        }
    }

    public void PrepareAllCommandModules()
    {
        for (int i = 0; i < messageSessions.Count; i++)
        {
            MessageDatabase item = messageSessions[i];
            GameObject commandButton = Instantiate(messageButtonPrefab, messagesConten);

            commandButton.GetComponent<MessageItem>().messageTextBlock.text = item.title;
            int bufer = i;
            commandButton.GetComponent<Button>().onClick.AddListener(() => DrawMessagesFromBaseWithIndex(bufer));
        }
    }
}
