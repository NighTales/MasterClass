using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Хранилище паролей
/// </summary>
[HelpURL("https://docs.google.com/document/d/127uNKdKt6TkNNAkLwextH6860VjVnsCwIjknC6yWlf0/edit?usp=sharing")]
public class PlayerInfoHolder : MonoBehaviour
{
    private List<ComputerModule> computers;

    private PlayerUI playerUI;

    private void Start()
    {
        computers = new List<ComputerModule>();
        playerUI = FindObjectOfType<PlayerUI>();
    }

    public void AddPasswordItem(ComputerModule computer)
    {
        if (computers == null)
            computers = new List<ComputerModule>();

        computers.Add(computer);
    }

    public void FindPassword(ComputerModule computer)
    {
        foreach (var item in computers)
        {
            if(item == computer)
            {
                playerUI.SetPassword(item.password);
            }
        }
    }
}
