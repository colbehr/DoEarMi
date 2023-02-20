using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class User
{   
    // Meta
    DoEarMiMeta meta;
    PasswordManager pwm;

    // BASIC INFO
    [SerializeField]
    private string username, uID, password_hash, password_salt, email;
    [SerializeField]
    private DateTime last_active, last_boosted, last_frozen;

    // STATS
    [SerializeField]
    private int xp, streak, credits;
    [SerializeField]
    private bool streak_frozen, boosted;

    // COLLECTIONS
    // Instruments and icons stored as list of string instrument names and filenames respectively, 
    // load audio clips via LoadAudioAsInstrument with name
    [SerializeField]
    private List<string> instruments, icons;
    [SerializeField]
    private string active_icon, active_instrument; 

    public User(string username, string password, string email)
    {
        this.meta = DoEarMiMeta.Instance();
        this.pwm = PasswordManager.Instance();

        this.username = username;
        this.uID = new_uID();
        this.password_salt = pwm.create_salt();
        this.password_hash = pwm.generate_salt_hash(password, pwm.text_to_byte(password_salt));
        this.email = email;
        this.last_active = DateTime.Now;
        this.last_boosted = DateTime.Now;
        this.last_frozen = DateTime.Now;

        this.xp = 0;
        this.streak = 0;
        this.credits = 100;
        this.streak_frozen = false;
        this.boosted = false;

        // Add default icon to user collection
        this.icons = new List<string>();
        this.active_icon = meta.get_default_icon();
        this.icons.Add(active_icon);
        
        // Add default instrument to user collection
        this.instruments = new List<string>();

        string default_instrument = meta.get_default_instrument();
        this.instruments.Add(default_instrument);
        this.active_instrument = default_instrument;
        
        // this.boosts.Add(2xXP)                     // TODO: free boost to start, encourage initial engagement ?

        // Add user to app meta data, must occur at end of instantiation
        meta.add_user(this);
    }


    private string new_uID()
    {
        return Guid.NewGuid().ToString();
    }

    public string user_to_json()
    {
        // for prototype this is fine, but long-term could cause loading errors for old user types is new User class isn't created
        return JsonUtility.ToJson(this);
    }


    public string get_username()
    {
        return this.username;
    }

    public void update_username(string name)
    {
        // TODO: check name against dictionary of banned words
        this.username = name;
        update_json();
    }

    public string get_password_hash()
    {
        return this.password_hash;
    }

    public string get_password_salt()
    {
        return this.password_salt;
    }

    public void update_password(string password)
    {
        this.password_salt = pwm.create_salt();
        this.password_hash = pwm.generate_salt_hash(password, pwm.text_to_byte(password_salt));
        update_json();
    }


    public string get_email()
    {
        return this.email;
    }

    public void update_email(string email)
    {
        // TODO: check if valid email address
        this.email = email;
        update_json();
    }

    public void update_last_active()
    {
        this.last_active = DateTime.Now;
        update_json();
    }

   public string get_uID()
    {
        return this.uID;
    }

    public int get_streak()
    {
        return this.streak;
    }

    public void freeze_streak()
    {
        this.last_frozen = DateTime.Now;
        this.streak_frozen = true;
        update_json();
        // TODO: change streak color/add snowflake icon beside it ?
    }
    public void unfreeze_streak()
    {
        this.streak_frozen = false;
        update_json();
        // TODO: change streak color/add fire icon beside it ?
    }

    // checks against dateTime and updates accordingly
    public bool isFrozen()
    {
        if ((DateTime.Now.Date - last_frozen.Date).TotalDays > 1)
        {
            this.streak_frozen = false;
            update_json();
        }
        return this.streak_frozen;
    }

    public void activate_user_boost()
    {
        this.last_boosted = DateTime.Now;
        this.boosted = true;
        update_json();
    }
    public bool isBoosted()
    {
        if ((DateTime.Now.Date - last_boosted.Date).TotalHours > 1)
        {
            this.boosted = false;
            update_json();
        }
        return this.boosted;
    }

    public int get_xp()
    {
        return this.xp;
    }

    // xp increase for practice session
    public void update_xp(int xp_base_increase)
    {
        int boostMult = 1;
        if (boosted) { boostMult = 2; }
        this.xp += (10*this.streak + xp_base_increase)*boostMult;
        update_json();
    }

    // base xp increase for shop purchase
    public void update_xp_purchased(int xpPurchased)
    {
        this.xp += xpPurchased;
        update_json();
    }

    public int get_credits()
    {
        return this.credits;
    }

    public void update_credits(int credit_increase)
    {
        this.credits += credit_increase;
        update_json();
    }


    // called from shop on purchase, auto set instrument to active
    public void add_instrument(string instrument)
    {
        this.instruments.Add(instrument);
        update_json();
    }

    public void set_active_instrument(string instrument)
    {
        this.active_instrument = instrument;
        update_json();
    }

    public string get_active_instrument()
    {
        return this.active_instrument;
    }
    public List<string> get_instruments()
    {
        return this.instruments;
    }

    public void add_icon(string icon)
    {
        this.icons.Add(icon);
        for(int i=0; i<icons.Count; i++)
        {
            Debug.Log(icons[i]);
        }
        update_json();
    }

    public List<string> get_icons()
    {
        return this.icons;
    }

    public string get_active_icon()
    {
        return this.active_icon;
    }

    // called from all setters
    public void update_json()
    {
        this.meta = DoEarMiMeta.Instance();
        this.meta.save_user_data(this);
    }

}
