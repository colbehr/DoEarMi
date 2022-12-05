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

        this.user = meta.get_curr_user();

        //get username email instruments and icon 
        usernameText.GetComponent<TMPro.TMP_InputField>().text = user.get_username().ToString();
        emailText.GetComponent<TMPro.TMP_InputField>().text = user.get_email().ToString();

        Sprite sprite = Resources.Load<Sprite>(meta.get_default_icon());
        icon.GetComponent<Image>().sprite = sprite;
        instrumentsDropdown.ClearOptions();
        // TODO: current instrument is not top instrument in list
        instrumentsDropdown.AddOptions(user.get_instruments());
        //add code to pick option 
    }

    public void saveSettings()
    {
        user.update_email(emailText.GetComponent<TMPro.TMP_InputField>().text);
        user.update_username(usernameText.GetComponent<TMPro.TMP_InputField>().text);
        user.remove_all_active();
        user.set_active(instrumentsDropdown.GetComponent<TMPro.TMP_Dropdown>().options[instrumentsDropdown.GetComponent<TMPro.TMP_Dropdown>().value].text);
        user.update_json();
        //update profile screen
        profileManager.GetComponent<Profile>().OnEnable();
        print("Settings Saved");

    }

    public void giveInst() {
        user.add_instrument("BassI");
        user.add_instrument("EPianoII");
    }
}