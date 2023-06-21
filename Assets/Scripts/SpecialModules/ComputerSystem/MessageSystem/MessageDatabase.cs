using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DefaultMessageDatabase", menuName = "IgoGoTools/MessageDatabase")]
public class MessageDatabase : ScriptableObject
{
    public string title;
    public List<MessageInfo> messages;
}
