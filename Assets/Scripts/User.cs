using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class User
{   
    // BASIC INFO
    [SerializeField]
    private string username, uID, password, email;
    [SerializeField]
    private DateTime last_active;
    // private string active_icon_filename; // TODO: not sure if this is how it should be stored? Sprite maybe?

    // STATS
    [SerializeField]
    private int xp, streak, credits;
    [SerializeField]
    private bool streak_frozen;

    // COLLECTIONS
    // private List<string> icon_filenames; // TODO: not sure if this is how it should be stored? Sprite[] maybe?
    // Instruments stored as list of string instrument names, load audio clips via LoadAudioAsInstrument with name
    [SerializeField]
    private List<string> instruments, active_instruments;
    // private List<InstrumentPathWrapper> instruments, active_instruments; 

    // private List<Boost> boosts;

    // COMPLETIONS
    // private List<Lesson> lessons;


    public User(string username, string password, string email)
    {
        DoEarMiMeta meta = DoEarMiMeta.Instance();

        this.username = username;
        this.uID = new_uID();
        this.password = password;  // TODO: plaintext password oof
        this.email = email;
        this.last_active = DateTime.Now;

        this.xp = 0;
        this.streak = 0;
        this.credits = 100;        // TODO: free money to start, enough to maybe buy a simple icon ?
        this.streak_frozen = false;

        // this.icon_filenames.Add(default_icon.png) // TODO: not sure if this is how it should be stored? Sprite maybe?
        
        // Add default instrument to user collection
        this.instruments = new List<string>();
        this.active_instruments = new List<string>();

        string default_instrument = meta.get_default_instrument();
        this.instruments.Add(default_instrument);
        this.active_instruments.Add(default_instrument);
        
        // this.boosts.Add(2xXP)                     // TODO: free boost to start, encourage initial engagement ?

        // Add user to app meta data, must occur at end of instantiation
        meta.add_user(this);
    }


    private string new_uID()
    {
        return Guid.NewGuid().ToString();
    }

    public string get_uID()
    {
        return this.uID;
    }

    public int get_xp()
    {
        return this.xp;
    }

    public string user_to_json()
    {
        // for prototype this is fine, but long-term could cause loading errors for old user types is new User class isn't created
        return JsonUtility.ToJson(this);
    }

    public void update_username(string name)
    {
        // TODO: check name against dictionary of banned words
        this.username = name;
    }
    public string get_username()
    {
        return this.username;
    }


    public void update_password(string password)
    {
        this.password = password;  // TODO: plaintext password oof
    }

    public void update_email(string email)
    {
        // TODO: check if valid email address
        this.email = email;
    }


    // TODO: must only be called after checking streak status ?
    private void update_active()
    {
        this.last_active = DateTime.Now;
    }


    // TODO: how often should this be called? where should it be called from ?
    public void update_streak()
    {
        // TODO: 
        // check if streak is frozen
        //      check last_active against DateTime.Now
        //      update streak if necessary
        //      this.update_active() ?
    }


    // TODO: how and when to check time limit before unfreeze streak ?
    public void freeze_streak()
    {
        this.streak_frozen = true;
        // TODO: change streak color/add snowflake icon beside it ?
    }
    public void unfreeze_streak()
    {
        this.streak_frozen = false;
        // TODO: change streak color/add fire icon beside it ?
    }


    public void update_xp(int xp_base_increase)
    {
        // TODO: include boosts in formula
        this.xp += (10*this.streak + xp_base_increase);
    }


    public void update_credits(int credit_increase)
    {
        this.credits += credit_increase;
    }


    // called from shop on purchase, auto set instrument to active
    public void add_instrument(string instrument)
    {
        this.instruments.Add(instrument);
        this.active_instruments.Add(instrument);
    }

    public void remove_active(string instrument)
    {
        this.active_instruments.Remove(instrument);
    }

    // // called from shop on purchase
    // public void update_icons(string icon_file)
    // {
    //     this.icon_filenames.Add(icon_file);
    // }

    // // called from shop on purchase
    // public void update_boosts(Boost<T> boost)
    // {
    //     this.boosts.Add(boost);
    // }


}
