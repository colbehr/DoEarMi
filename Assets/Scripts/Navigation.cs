using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/**
 * 
 * Uses EventSystem gameobject to trigger animations on button hightlight
 */
public class Navigation : MonoBehaviour
{
    public GameObject buttonHighlight;
    public GameObject practiceScreen;
    public GameObject pageTitleText;
    private Animation buttonHighlightAnim;
    private Animation practiceScreenAnim;
    private string currentLocation;
    // Start is called before the first frame update
    void Start()
    {
        buttonHighlightAnim = buttonHighlight.GetComponent<Animation>();
        practiceScreenAnim = practiceScreen.GetComponent<Animation>();
        currentLocation = "practice";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void toPractice() {
        // transition to screen
        if (currentLocation == "lessons")
        {
            buttonHighlightAnim.Play("lessonsToPractice");
        }
        else if (currentLocation == "leaderboard")
        {
            buttonHighlightAnim.Play("leaderboardToPractice");
        }
        currentLocation = "practice";
        pageTitleText.GetComponent<TMPro.TMP_Text>().SetText(currentLocation.ToUpper());
        print("toPractice");
    }

    public void toLeaderboard()
    {
        // transition to screen
        if (currentLocation == "practice")
        {
            buttonHighlightAnim.Play("practiceToLeaderboard");
        }
        else if (currentLocation == "lessons")
        { 
            buttonHighlightAnim.Play("lessonsToLeaderboard");
        }
        currentLocation = "leaderboard";

        pageTitleText.GetComponent<TMPro.TMP_Text>().SetText(currentLocation.ToUpper());


        print("toLeaderboard");
    }

    public void toLessons()
    {
        // transition to screen
        if (currentLocation == "practice")
        {
            buttonHighlightAnim.Play("practiceToLessons");
        }
        else if (currentLocation == "leaderboard")
        {
            buttonHighlightAnim.Play("leaderboardToLessons");
        }
        currentLocation = "lessons";

        pageTitleText.GetComponent<TMPro.TMP_Text>().SetText(currentLocation.ToUpper());

        print("toLessons");
    }

    public void toStore() { 
        print("toStore");
    }
    public void toProfile()
    {
        print("toProfile");
    }
    public void openPractice()
    {
        practiceScreenAnim.Play("OverlayShow");
        print("practice button hit");

    }
    public void closePractice()
    {
        practiceScreenAnim.Play("OverlayHide");
        print("close button hit");

    }
}