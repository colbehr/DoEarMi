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
    public GameObject profileScreen;
    public GameObject chordsLessonScreen;
    public GameObject storeScreen;
    public GameObject mainScreens;
    public GameObject pageTitleText;
    public GameObject practiceManager;
    private Animation buttonHighlightAnim;
    private Animation practiceScreenAnim;
    private Animation chordsLessonScreenAnim;
    private Animation mainScreensAnim;
    private Animation storeScreenAnim;
    private Animation profileScreenAnim;
    private string currentLocation;

    // Start is called before the first frame update
    void Start()
    {
        buttonHighlightAnim = buttonHighlight.GetComponent<Animation>();
        practiceScreenAnim = practiceScreen.GetComponent<Animation>();
        chordsLessonScreenAnim = chordsLessonScreen.GetComponent<Animation>();
        profileScreenAnim = profileScreen.GetComponent<Animation>();
        storeScreenAnim = storeScreen.GetComponent<Animation>();
        mainScreensAnim = mainScreens.GetComponent<Animation>();
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
            mainScreensAnim.Play("screenLessonsToPractice");
        }
        else if (currentLocation == "leaderboard")
        {
            buttonHighlightAnim.Play("leaderboardToPractice");
            mainScreensAnim.Play("screenLeaderboardToPractice");
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
            mainScreensAnim.Play("screenPracticeToLeaderboard");
        }
        else if (currentLocation == "lessons")
        { 
            buttonHighlightAnim.Play("lessonsToLeaderboard");
            mainScreensAnim.Play("screenLessonsToLeaderboard");
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
            mainScreensAnim.Play("screenPracticeToLessons");
        }
        else if (currentLocation == "leaderboard")
        {
            buttonHighlightAnim.Play("leaderboardToLessons");
            mainScreensAnim.Play("screenLeaderboardToLessons");
        }
        currentLocation = "lessons";

        pageTitleText.GetComponent<TMPro.TMP_Text>().SetText(currentLocation.ToUpper());

        print("toLessons");
    }


    public void openStore()
    {
        storeScreenAnim.Play("OverlayShow");
        print("openStore");
    }

    public void closeStore()
    {
        storeScreenAnim.Play("OverlayHide");
        print("closeStore");
    }
    public void openProfile()
    {
        profileScreenAnim.Play("OverlayShow");
        print("openProfile");
    }

    public void closeProfile()
    {
        profileScreenAnim.Play("OverlayHide");
        print("closeProfile");
    }

    public void openPractice()
    {
        practiceManager.SetActive(true);
        practiceScreenAnim.Play("OverlayShow");
        print("practice button hit");

    }
    public void closePractice()
    {
        practiceManager.SetActive(false);
        practiceScreenAnim.Play("OverlayHide");
        print("close button hit");
    }

    public void openLesson()
    {
        chordsLessonScreenAnim.Play("OverlayShow");
        print("chords button hit");
    }
    public void closeLesson()
    {
        chordsLessonScreenAnim.Play("OverlayHide");
        print("chords button hit");
    }
}