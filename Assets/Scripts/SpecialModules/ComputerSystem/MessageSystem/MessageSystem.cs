using System.Collections.Generic;
using UnityEngine;

public class MessageSystem : MonoBehaviour
{
    [SerializeField]
    private ComputerModule computerModule;
    [SerializeField]
    private List<MessageDatabase> messageSessions;
    [SerializeField]
    private GameObject myMessageItem;
    [SerializeField] 
    private GameObject friendMessageItem;
    [SerializeField] 
    private Transform messagesScrollViewConten;

    public void DrawMessagesFromBaseWithIndex(int index)
    {
        for (int i = 0; i < messagesScrollViewConten.childCount; i++)
        {
            Destroy(messagesScrollViewConten.GetChild(i).gameObject);
        }

        RectTransform messageItemRectTransform;

        foreach (var item in messageSessions[index].messages)
        {
            if (item.groupMessageFromOne || !item.sender.Equals(computerModule.login))
            {
                messageItemRectTransform = Instantiate(friendMessageItem, messagesScrollViewConten).GetComponent<RectTransform>();
            }
            else
            {
                messageItemRectTransform = Instantiate(myMessageItem, messagesScrollViewConten).GetComponent<RectTransform>();
            }

            MessageItem messageItem = messageItemRectTransform.GetComponent<MessageItem>();
            messageItem.messageTextBox.text = item.message;

            float height = 100;
            height += +(item.message.Length / 35)*40;
            char[] separator = { '\n' };
            height += item.message.Split(separator).Length-1*40;
            messageItemRectTransform.SetHeight(height);
        }
    }
}
