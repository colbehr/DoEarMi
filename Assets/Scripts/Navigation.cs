using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Navigation : MonoBehaviour
{
    public GameObject buttonHighlight;
    private Animation anim;
    private string currentLocation;
    // Start is called before the first frame update
    void Start()
    {
        anim = buttonHighlight.GetComponent<Animation>();
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
            anim.Play("lessonsToPractice");
        }
        else if (currentLocation == "leaderboard")
        {
            anim.Play("leaderboardToPractice");
        }
        currentLocation = "practice";
        print("toPractice");
    }

    public void toLeaderboard()
    {
        // transition to screen
        if (currentLocation == "practice")
        {
            anim.Play("practiceToLeaderboard");
        }
        else if (currentLocation == "lessons")
        { 
            anim.Play("lessonsToLeaderboard");
        }

        currentLocation = "leaderboard";


        print("toLeaderboard");
    }

    public void toLessons()
    {
        // transition to screen
        if (currentLocation == "practice")
        {
            anim.Play("practiceToLessons");
        }
        else if (currentLocation == "leaderboard")
        {
            anim.Play("leaderboardToLessons");
        }
        currentLocation = "lessons";

        print("toLessons");
    }    
}