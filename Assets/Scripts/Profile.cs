using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Profile : MonoBehaviour
{
    // UI Components
    public TMPro.TMP_Text creditText;
    public TMPro.TMP_Text usernameText;
    public TMPro.TMP_Text xpText;
    public TMPro.TMP_Text streakText;
    public TMPro.TMP_Text emailText;
    public Image icon;
    public Image profileButtonIcon;

    // Backend Fields
    private DoEarMiMeta meta;
    public User user;

    void Start()
    {
        this.meta = DoEarMiMeta.Instance();

        // TODO: testing only!! gets first user from all users list, need sign in implemented to get proper user
        // this.user = meta.load_all_users().ElementAt(0);

        // Set text fields unique to user
        // basic user info
        // usernameText.GetComponent<TMPro.TMP_Text>().SetText(user.get_username().ToString());
        // emailText.GetComponent<TMPro.TMP_Text>().SetText(user.get_email().ToString());
        
        // TODO: for testing, get profile icon name from meta... should be getting it from user.
        Sprite sprite = Resources.Load<Sprite>(meta.get_default_icon());
        icon.GetComponent<Image>().sprite = sprite;
        profileButtonIcon.GetComponent<Image>().sprite = sprite;

        // stats
        // creditText.GetComponent<TMPro.TMP_Text>().SetText(user.get_credits().ToString());
        // xpText.GetComponent<TMPro.TMP_Text>().SetText("XP " + user.get_xp().ToString());
        // streakText.GetComponent<TMPro.TMP_Text>().SetText("STREAK " + user.get_streak().ToString());

    }

    // void Update()
    // {
    //     // testing purposes... TODO: get proper user
    //     this.user = this.meta.get_user();

    //     //username.text = this.user.get_username();
        
    //     // TODO: set rest of text fields
    //     creditText.GetComponent<TMPro.TMP_Text>().SetText(user.get_credits().ToString());
    // }


    // TODO: implement on click functions to call user.update_active_instruments and such
}
