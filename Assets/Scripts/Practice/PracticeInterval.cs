using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Linq;

public class PracticeInterval : Practice
{
    private Question currentQuestion;

    private struct Question {
        public int root;
        public int interval;
    }

    // Initialize answer dictionary
    public override void loadAnswers()
    {
        answerSet.Add(1, new Answer("Major 2nd"));
        answerSet.Add(2, new Answer("Major 3rd"));
        answerSet.Add(3, new Answer("Perfect 4th"));
        answerSet.Add(4, new Answer("Perfect 5th"));
        answerSet.Add(5, new Answer("Major 6th"));
        answerSet.Add(6, new Answer("Major 7th"));
        answerSet.Add(7, new Answer("Octave"));
    }

    // Randomly generate and setup a question after short delay
    public override void generateQuestion(float delayTime)
    {
        StartCoroutine(DelayNextQuestion(delayTime));
    }

    IEnumerator DelayNextQuestion(float delayTime)
    {
        playButton.enabled = false;
        failed = false;
        foreach (GameObject button in answerButtons) 
        {
            button.GetComponent<Button>().enabled = false;
        }
        yield return new WaitForSeconds(delayTime);

        foreach (GameObject button in answerButtons) 
        {
            button.GetComponent<Image>().color = new Color32(98, 182, 203, 255);
            button.GetComponent<Button>().enabled = true;
        }

        currentQuestion = new Question();
        currentQuestion.root = (int) Random.Range(0, instrument.Count - 12);
        currentQuestion.interval = (int) Random.Range(1,8);
        playButton.enabled = true;
        playQuestion();
    }

    // Play chord based on current Question
    public override void playQuestion() {
        StartCoroutine(playQuestionCoroutine());
    }

    IEnumerator playQuestionCoroutine()
    {
        base.activatePulse();
        foreach (GameObject button in answerButtons) 
        {
            button.GetComponent<Button>().enabled = false;
        }
        playButton.enabled = false;
        soundPlayer.PlayOneShot(instrument[currentQuestion.root], 1);
        yield return new WaitForSeconds(0.7F);
        soundPlayer.PlayOneShot(instrument[currentQuestion.root + getIntervalOffset()], 1);
        playButton.enabled = true;
        foreach (GameObject button in answerButtons) 
        {
            button.GetComponent<Button>().enabled = true;
        }
        Debug.Log("Interval: " + currentQuestion.interval);
    }

    private int getIntervalOffset()
    {
        switch (currentQuestion.interval) {
        case 1:
            return 2; // Major 2nd
        case 2:
            return 4; // Major 3rd
        case 3:
            return 5; // Perfect 4th
        case 4:
            return 7; // Perfect 5th 
        case 5:
            return 9; // Major 6th
        case 6:
            return 11; // Major 7th
        case 7:
            return 12; // Octave
        default:
            return 0;
        }
    }

    // Function linked to buttons onClick()
    public override void answer() 
    {
        GameObject button = EventSystem.current.currentSelectedGameObject;
        // Check if answer is correct
        if (answerSet[currentQuestion.interval].getName() == button.name)
        {
            base.activatePulse();
            soundPlayer.PlayOneShot(instrument[currentQuestion.root], 1);
            soundPlayer.PlayOneShot(instrument[currentQuestion.root + getIntervalOffset()], 1);
            base.correct(button, currentQuestion.interval);
        } 
        else 
        {
            Debug.Log("Incorrect");
            failed = true;
            button.GetComponent<Image>().color = new Color32(255, 80, 80, 255);
        }
    }
}
