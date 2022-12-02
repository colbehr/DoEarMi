using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LoginMenu : MonoBehaviour
{
    // UI Components
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
        Debug.Log("email set");
        this.userEmail = email.GetComponent<TMP_InputField>().text;
        Debug.Log(userEmail);
    }
    public void set_user_password()
    {
        Debug.Log("password set");
        this.userPassword = password.GetComponent<TMP_InputField>().text;
    }

    // TODO: for now, can probably simulate log in by creating a new user... 
    // Where and how to store user base ? Get user by hashing username ?



}
