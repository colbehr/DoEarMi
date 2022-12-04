using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{

    public TMPro.TMP_InputField usernameText;
    public TMPro.TMP_InputField emailText;
    public TMPro.TMP_Dropdown instrumentsDropdown;
    public GameObject profileManager;
    public Image icon;

    // Backend Fields
    private DoEarMiMeta meta;
    public User user;


    private void Start()
    {

        gameObject.active = false;

    }

    private void OnEnable()
    {
        this.meta = DoEarMiMeta.Instance();

        // TODO: testing only!! gets first user from all users list, need sign in implemented to get proper user
        this.user = meta.get_curr_user();

        //get username email instruments and icon 
        usernameText.GetComponent<TMPro.TMP_InputField>().text = user.get_username().ToString();
        emailText.GetComponent<TMPro.TMP_InputField>().text = user.get_email().ToString();

        // TODO: for testing, get profile icon name from meta... should be getting it from user.
        Sprite sprite = Resources.Load<Sprite>(meta.get_default_icon());
        icon.GetComponent<Image>().sprite = sprite;
        instrumentsDropdown.ClearOptions();
        instrumentsDropdown.AddOptions(user.get_instruments());
        //add code to pick option 
    }

    public void saveSettings()
    {
        user.update_email(emailText.GetComponent<TMPro.TMP_InputField>().text);
        user.update_username(usernameText.GetComponent<TMPro.TMP_InputField>().text);
        //user.set_active_instrument(instrumentsDropdown.GetComponent<TMPro.TMP_Dropdown>().options[instrumentsDropdown.GetComponent<TMPro.TMP_Dropdown>().value].text);
        user.update_json();
        //update profile screen
        profileManager.GetComponent<Profile>().OnEnable();
        print("Settings Saved");

    }
}