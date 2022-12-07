using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class LoginMenu : MonoBehaviour
{
    // UI Components
    public GameObject login;
    public GameObject practice;
    public GameObject upperNav;
    public GameObject lowerNav;
    public GameObject uname;
    public GameObject password;
    public GameObject failText;
    public Image profIcon; // sets profile nav icon here on login
    // Buttons
    public GameObject signInButton;
    public GameObject showPassword;
    public GameObject hidePassword;

    // Backend
    private DoEarMiMeta meta;
    private string username;
    private string userPassword; // TODO: plaintext password oof
    private bool nameFilled;
    private bool passwordFilled;
    
    // Start is called before the first frame update
    void Start()
    {
        practice.SetActive(false);
        upperNav.SetActive(false);
        lowerNav.SetActive(false); 
        failText.SetActive(false); 

        meta = DoEarMiMeta.Instance();

        signInButton.GetComponent<Button>().interactable = false;
        this.nameFilled = false;
        this.passwordFilled = false;

        showPassword.SetActive(false);
        hidePassword.SetActive(true);
    }

    public void set_user_name()
    {
        // Debug.Log("uname set");
        this.username = uname.GetComponent<TMP_InputField>().text;
        this.nameFilled = true;

        failText.SetActive(false); 
        attempt_enable_signin();
    }
    public void set_user_password()
    {
        // Debug.Log("password set");
        this.userPassword = password.GetComponent<TMP_InputField>().text;
        this.passwordFilled = true;

        failText.SetActive(false); 
        attempt_enable_signin();
    }

    public void sign_in()
    {
        User user = meta.find_user(username);

        if (user != null && user.get_password() == userPassword)
        {
            Debug.Log("found user for sign in");

            meta.set_curr_user(user);
            user.update_last_active();
            string icon = user.get_active_icon();

            // TODO: welcome back <user> ! popup

            login.SetActive(false);
            practice.SetActive(true);
            upperNav.SetActive(true);
            lowerNav.SetActive(true);
            profIcon.GetComponent<Image>().sprite = Resources.Load<Sprite>(icon);
        }
        else
        {
            failText.SetActive(true); 
        }
    }

    public void sign_out()
    {
        meta.set_curr_user(null);
        //send user to home screen
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void dev_skip()
    {
        User user = meta.find_user("ShrimpAce");

        if (user == null) { 
            user = JsonUtility.FromJson<User>("{\"username\":\"ShrimpAce\",\"uID\":\"d224fb65-be55-4be3-a8b1-605b00179cb2\",\"password\":\"password1\",\"email\":\"ShrimpAce@email.com\",\"xp\":10213,\"streak\":0,\"credits\":1100,\"streak_frozen\":false,\"boosted\":false,\"instruments\":[\"GuitarI\",\"BassI\",\"EPianoII\"],\"active_instrument\":[\"PianoI\"],\"icons\":[\"ProfileIcons/defaultIcon\"],\"active_icon\":\"ProfileIcons/defaultIcon\"}");
            meta.add_user(user);
        }
        meta.set_curr_user(user);

        // Debug.Log(user.get_uID());

        login.SetActive(false);
        practice.SetActive(true);
        upperNav.SetActive(true);
        lowerNav.SetActive(true);
    }


    // Makes sign in button clickable if input fields are populated
    public void attempt_enable_signin()
    {
        if (this.nameFilled && this.passwordFilled)
        {
            this.signInButton.GetComponent<Button>().interactable = true;
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

    // TODO: for now, can probably simulate log in by creating a new user... 
    // Where and how to store user base ? Get user by hashing username ?



}
