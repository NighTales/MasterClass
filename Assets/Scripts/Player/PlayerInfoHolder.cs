using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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
