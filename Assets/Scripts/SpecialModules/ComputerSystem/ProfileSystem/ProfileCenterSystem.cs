using System.Collections.Generic;
using UnityEngine;

public class ProfileCenterSystem : MonoBehaviour
{
    [SerializeField] private GameObject ProfileMenu;
    [SerializeField] private Transform profileItemsContent;
    [SerializeField] private GameObject profileITemUIPrefab;


    private List<ProfileItem> profileItems = new List<ProfileItem>();

    private void Start()
    {
        ComputerModule[] computers = FindObjectsByType<ComputerModule>(FindObjectsInactive.Include, FindObjectsSortMode.None);

        foreach (ComputerModule computer in computers)
        {
            profileItems.Add(computer.profile);
        }
    }

    public void ShowProfileMenu()
    {
        ProfileMenu.SetActive(true);

        ClearProfileItemsContent();

        foreach (ProfileItem item in profileItems)
        {
            ProfileUIItem uiItem = Instantiate(profileITemUIPrefab, profileItemsContent).GetComponent<ProfileUIItem>();
            uiItem.Init(item);

        }
    }

    private void ClearProfileItemsContent()
    {
        for (int i = 0; i < profileItemsContent.childCount; i++)
        {
            Destroy(profileItemsContent.GetChild(i));
        }
    }
}
