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
    private Transform moduleCommandsContent;

    [SerializeField]
    private GameObject commandButtonPrefab;

    [SerializeField]
    private List<ModuleItem> moduleItems;

    public void ClearAll()
    {
        for (int i = 0; i < moduleCommandsContent.childCount; i++)
        {
            Destroy(moduleCommandsContent.GetChild(i).gameObject);
        }

        cameraRenderScreen.gameObject.SetActive(false);
    }

    public void SetModuleItemWithNumber(int i)
    {
        ClearAll();
        foreach (var item in moduleItems[i].commands)
        {
            GameObject commandButton = Instantiate(commandButtonPrefab, moduleCommandsContent);
            item.commandTextBox = commandButton.GetComponent<MessageItem>().messageTextBox;
            item.commandTextBox.text = item.commandTitleDefault;
            commandButton.GetComponent<Button>().onClick.AddListener(() => item.ExecuteCommand());
        }

        if (moduleItems[i].moduleCamera != null)
        {
            cameraRenderScreen.gameObject.SetActive(true);
            cameraRenderScreen.texture = moduleItems[i].moduleCamera.activeTexture;
        }
    }
}

[System.Serializable]
public class ModuleItem
{
    public Camera moduleCamera;
    public List<ModuleCommand> commands;
}

[System.Serializable]
public class ModuleCommand
{
    public bool useDefaultTitle;
    public string commandTitleDefault;
    public string commandTitleSecond;
    public UnityEvent executeAction;
    public Text commandTextBox;

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
