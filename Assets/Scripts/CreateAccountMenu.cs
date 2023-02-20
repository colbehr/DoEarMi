using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class CreateAccountMenu : MonoBehaviour
{
    // UI Components
    public GameObject create;
    public GameObject upperNav;
    public GameObject lowerNav;
    public GameObject practice;
    public GameObject uname;
    public GameObject password;
    public GameObject rePassword;
    public GameObject failText;
    public Image profIcon; // sets profile nav icon here on login
    public GameObject createButton;
    public GameObject showPassword;
    public GameObject hidePassword;

    // Backend
    private DoEarMiMeta meta;
    private string username;
    private string userPassword; // TODO: plaintext password oof
    private bool nameFilled;
    private bool passwordFilled;
    private bool rePasswordFilled;
    private bool passwordsMatch;

    // Start is called before the first frame update
    void Start()
    {
        practice.SetActive(false);
        failText.SetActive(false); 

        meta = DoEarMiMeta.Instance();

        createButton.GetComponent<Button>().interactable = false;
        this.nameFilled = false;
        this.passwordFilled = false;
        this.rePasswordFilled = false;
        this.passwordsMatch = false;

        showPassword.SetActive(false);
        hidePassword.SetActive(true);
    }

    public void set_user_name()
    {
        // Debug.Log("uname set");
        this.username = uname.GetComponent<TMP_InputField>().text;
        this.nameFilled = true;

        failText.SetActive(false); 
        attempt_enable_create_account();
    }
    public void set_user_password()
    {
        // Debug.Log("password set");
        this.userPassword = password.GetComponent<TMP_InputField>().text;
        this.passwordFilled = true;

        failText.SetActive(false); 
        attempt_enable_create_account();
    }
    public void reenter_user_password()
    {
        // Debug.Log("password set");
        this.userPassword = rePassword.GetComponent<TMP_InputField>().text;
        this.rePasswordFilled = true;

        failText.SetActive(false);
        check_password_match();
        attempt_enable_create_account();
    }


    private void check_password_match()
    {
        if (this.password == this.rePassword)
        {
            this.passwordsMatch = true;
        }
        else
        {
            this.passwordsMatch = false;
            failText.SetActive(true); 
        }
    }

    // Makes create account button clickable if input fields are populated properly
    public void attempt_enable_create_account()
    {
        if (this.nameFilled && this.passwordFilled && this.rePasswordFilled && this.passwordsMatch)
        {
            this.createButton.GetComponent<Button>().interactable = true;
        }
    }


    public void hide_password()
    {
        password.GetComponent<TMP_InputField>().contentType = TMP_InputField.ContentType.Password;
        showPassword.SetActive(false);
        hidePassword.SetActive(true);
        password.GetComponent<TMP_InputField>().ForceLabelUpdate();
    }

    public void show_password()
    {
        password.GetComponent<TMP_InputField>().contentType = TMP_InputField.ContentType.Standard;
        showPassword.SetActive(true);
        hidePassword.SetActive(false);
        password.GetComponent<TMP_InputField>().ForceLabelUpdate();
    }


    public void create_account()
    {
        User user = new User(this.username, this.userPassword, "");
        meta.add_user(user);

        create.SetActive(false);
        practice.SetActive(true);
        upperNav.SetActive(true);
        lowerNav.SetActive(true);

        string icon = user.get_active_icon();
        profIcon.GetComponent<Image>().sprite = Resources.Load<Sprite>(icon);
    }

}
