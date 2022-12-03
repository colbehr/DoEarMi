using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LoginMenu : MonoBehaviour
{
    // UI Components
    public GameObject login;
    public GameObject practice;
    public GameObject upperNav;
    public GameObject lowerNav;
    public GameObject email;
    public GameObject password;

    // Backend
    private string userEmail;
    private string userPassword; // TODO: plaintext password oof
    private DoEarMiMeta meta;

    // Start is called before the first frame update
    void Start()
    {
        practice.SetActive(false);
        upperNav.SetActive(false);
        lowerNav.SetActive(false);

        meta = DoEarMiMeta.Instance();
    }

    public void set_user_email()
    {
        // Debug.Log("email set");
        this.userEmail = email.GetComponent<TMP_InputField>().text;
        // Debug.Log(userEmail);
    }
    public void set_user_password()
    {
        // Debug.Log("password set");
        this.userPassword = password.GetComponent<TMP_InputField>().text;
    }

    public void sign_in()
    {
        User user = meta.find_user(userEmail);

        if (user != null && user.get_password() == userPassword)
        {
            Debug.Log("found user for sign in");

            meta.set_curr_user(user);
            user.update_last_active();

            // TODO: welcome back <user> ! popup

            login.SetActive(false);
            practice.SetActive(true);
            upperNav.SetActive(true);
            lowerNav.SetActive(true);
        }
        else
        {
            Debug.Log("no user matching name");

            // TODO: bad sign in, reprompt
        }
    }


    public void dev_skip()
    {
        User user = meta.find_user("ShrimpAce");
        meta.set_curr_user(user);

        Debug.Log(user.get_uID());

        login.SetActive(false);
        practice.SetActive(true);
        upperNav.SetActive(true);
        lowerNav.SetActive(true);
    }

    // TODO: for now, can probably simulate log in by creating a new user... 
    // Where and how to store user base ? Get user by hashing username ?



}
