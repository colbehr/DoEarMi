using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Profile : MonoBehaviour
{
    public User user;

    [SerializeField]
    //public Text username;
    // public Text xp;
    // public Text streak;
    // public Text credits;

    // TODO: icons, instruments, email, etc.

    void Start()
    {
        // testing purposes... TODO: get proper user
        this.user = new User("Rollo", "y'all_should_play_hollow_knight", "rollo@gmail.com");

        //username.text = this.user.get_username();
        
        // TODO: set rest of text fields
    }


    // TODO: implement on click functions to call user.update_active_instruments and such
}
