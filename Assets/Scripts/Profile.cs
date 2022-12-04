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
    public Image boostIcon;
    public Image freezeStreakIcon;
    public TMPro.TMP_Text noBoostsText;

    // Backend Fields
    private DoEarMiMeta meta;
    public User user;

    public void OnEnable()
    {
        this.meta = DoEarMiMeta.Instance();

        // TODO: testing only!! gets first user from all users list, need sign in implemented to get proper user
        this.user = meta.get_curr_user();

        // Set text fields unique to user
        // basic user info
        usernameText.GetComponent<TMPro.TMP_Text>().SetText(user.get_username().ToString());
        emailText.GetComponent<TMPro.TMP_Text>().SetText(user.get_email().ToString());
        
        // TODO: for testing, get profile icon name from meta... should be getting it from user.
        Sprite sprite = Resources.Load<Sprite>(meta.get_default_icon());
        icon.GetComponent<Image>().sprite = sprite;
        profileButtonIcon.GetComponent<Image>().sprite = sprite;

        // stats
        creditText.GetComponent<TMPro.TMP_Text>().SetText(user.get_credits().ToString());
        xpText.GetComponent<TMPro.TMP_Text>().SetText("XP " + user.get_xp().ToString());
        streakText.GetComponent<TMPro.TMP_Text>().SetText("STREAK " + user.get_streak().ToString());

        //boosts icons
        if (user.isBoosted()){
            boostIcon.transform.gameObject.SetActive(true);
        }
        else{
            boostIcon.transform.gameObject.SetActive(false);
        }

        if (user.isFrozen())
        {
            freezeStreakIcon.transform.gameObject.SetActive(true);
        }
        else
        {
            freezeStreakIcon.transform.gameObject.SetActive(false);
        }

        if (!user.isBoosted() && !user.isFrozen())
        {
            noBoostsText.transform.gameObject.SetActive(true);
        }
        else { 
            noBoostsText.transform.gameObject.SetActive(false);
        }
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
