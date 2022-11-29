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
    public GameObject practiceManager_Chords;
    public GameObject practiceManager_ScaleDegrees;
    public GameObject practiceManager_Intervals;
    public GameObject practiceManager_MelodicDictation;
    public GameObject lessonsManager;

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

    public void openLesson()
    {
        lessonsManager.SetActive(true);
        chordsLessonScreenAnim.Play("OverlayShow");
        print("chords button hit");
    }
    public void closeLesson()
    {
        lessonsManager.SetActive(false);
        chordsLessonScreenAnim.Play("OverlayHide");
        print("chords button hit");
    }

    // Practice Navigation
    public void openPractice_Chords()
    {
        practiceManager_Chords.SetActive(true);
        practiceScreenAnim.Play("OverlayShow");
        print("chord practice button hit");

    }
    public void openPractice_Intervals()
    {
        practiceManager_Intervals.SetActive(true);
        practiceScreenAnim.Play("OverlayShow");
        print("Intervals practice button hit");
    }
    public void openPractice_ScaleDegrees()
    {
        practiceManager_ScaleDegrees.SetActive(true);
        practiceScreenAnim.Play("OverlayShow");
        print("ScaleDegrees practice button hit");

    }
    public void openPractice_MelodicDictation()
    {
        practiceManager_MelodicDictation.SetActive(true);
        practiceScreenAnim.Play("OverlayShow");
        print("MelodicDictation practice button hit");
    }
    public void closePractice()
    {
        if (practiceManager_Chords.activeSelf)
            practiceManager_Chords.SetActive(false);
        else if (practiceManager_ScaleDegrees.activeSelf)
            practiceManager_ScaleDegrees.SetActive(false);
        else if (practiceManager_Intervals.activeSelf)
            practiceManager_Intervals.SetActive(false);
        else if (practiceManager_MelodicDictation.activeSelf)
            practiceManager_MelodicDictation.SetActive(false);

        practiceScreenAnim.Play("OverlayHide");
        print("close button hit");
    }
}