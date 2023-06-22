using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ControlSystemCore : MonoBehaviour
{
    [SerializeField]
    private RawImage cameraRenderScreen;

    [SerializeField]
    private Transform commandModuleContent;
    [SerializeField]
    private Transform commandsContent;

    [SerializeField]
    private GameObject commandButtonPrefab;

    [SerializeField]
    private List<ModuleItem> moduleItems;

    public void ClearAllCommands()
    {
        for (int i = 0; i < commandsContent.childCount; i++)
        {
            Destroy(commandsContent.GetChild(i).gameObject);
        }

        cameraRenderScreen.gameObject.SetActive(false);
    }

    public void ClearAllCommandModules()
    {
        for (int i = 0; i < commandModuleContent.childCount; i++)
        {
            Destroy(commandModuleContent.GetChild(i).gameObject);
        }

        cameraRenderScreen.gameObject.SetActive(false);
    }

    public void SetModuleItemWithNumber(int i)
    {
        ClearAllCommands();
        foreach (var item in moduleItems[i].commands)
        {
            GameObject commandButton = Instantiate(commandButtonPrefab, commandsContent);
            item.commandTextBox = commandButton.GetComponent<MessageItem>().messageTextBlock;
            item.commandTextBox.text = item.commandTitleDefault;
            commandButton.GetComponent<Button>().onClick.AddListener(() => item.ExecuteCommand());
        }

        if (moduleItems[i].moduleCamera != null)
        {
            cameraRenderScreen.gameObject.SetActive(true);
            cameraRenderScreen.texture = moduleItems[i].moduleCamera.activeTexture;
        }
    }

    public void PrepareAllCommandModules()
    {
        ClearAllCommandModules();
        for (int i = 0; i < moduleItems.Count; i++)
        {
            ModuleItem item = moduleItems[i];
            GameObject commandButton = Instantiate(commandButtonPrefab, commandModuleContent);
            commandButton.GetComponent<MessageItem>().messageTextBlock.text = item.name;
            int bufer = i;
            commandButton.GetComponent<Button>().onClick.AddListener(() => SetModuleItemWithNumber(bufer));
        }
    }
}

[System.Serializable]
public class ModuleItem
{
    public string name;
    public Camera moduleCamera;
    public List<ModuleCommand> commands;
}

[System.Serializable]
public class ModuleCommand
{
    public bool useDefaultTitle = true;
    public string commandTitleDefault;
    public string commandTitleSecond;
    public UnityEvent executeAction;
    [HideInInspector] public Text commandTextBox;

    public void ChangeCommandTitle()
    {
        useDefaultTitle = !useDefaultTitle;
        commandTextBox.text = useDefaultTitle ? commandTitleDefault : commandTitleSecond;
    }

    public void ExecuteCommand()
    {
        executeAction.Invoke();
        ChangeCommandTitle();
    }
}
