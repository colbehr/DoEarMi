using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class User
{
    // BASIC INFO 
    private string username;
    private string password;
    private string email;
    private DateTime last_active;
    // private string active_icon_filename; // TODO: not sure if this is how it should be stored? Sprite maybe?

    // STATS
    private int xp;
    private int streak;
    private int credits;
    private bool streak_frozen;

    // COLLECTIONS
    // private List<string> icon_filenames; // TODO: not sure if this is how it should be stored? Sprite[] maybe?
    // private List<Instrument> instruments;  
    // private List<Boost> boosts;

    // COMPLETIONS
    // private List<Lesson> lessons;


    public User(string username, string password, string email)
    {
        this.username = username;
        this.password = password;  // TODO: plaintext password oof
        this.email = email;
        this.last_active = DateTime.Now;

        this.xp = 0;
        this.streak = 0;
        this.credits = 100;        // TODO: free money to start, enough to maybe buy a simple icon ?
        this.streak_frozen = false;

        // this.icon_filenames.Add(default_icon.png) // TODO: not sure if this is how it should be stored? Sprite maybe?
        // this.instruments.Add(DefaultInstrument)
        // this.boosts.Add(2xXP)                     // TODO: free boost to start, encourage initial engagement ?
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


    // // called from shop on purchase
    // public void update_instruments(Instrument<T> instrument)
    // {
    //     this.instruments.Add(instrument);
    // }

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