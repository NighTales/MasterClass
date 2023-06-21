using UnityEngine;
using UnityEngine.UI;

public class ProfileUIItem : MonoBehaviour
{
    private ProfileItem profileItem;

    [SerializeField] private InputField loginIF;
    [SerializeField] private InputField passwordIF;

    public void Init(ProfileItem profile)
    {
        profileItem = profile;
        loginIF.text = profile.login;
        passwordIF.text = profile.password;
    }


    public void ChangeData()
    {
        profileItem.InvokeChangeData(loginIF.text, passwordIF.text);
    }
}


