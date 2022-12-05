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
        foreach (GameObject button in answerButtons) 
        {
            button.GetComponent<Button>().enabled = false;
        }
        playButton.enabled = false;
        soundPlayer.PlayOneShot(instrument[currentQuestion.root], 1);
        yield return new WaitForSeconds(0.7F);
        switch (currentQuestion.interval) {
        case 1:
            soundPlayer.PlayOneShot(instrument[currentQuestion.root + 2], 1); // Major 2nd
            break; 
        case 2:
            soundPlayer.PlayOneShot(instrument[currentQuestion.root + 4], 1); // Major 3rd
            break; 
        case 3:
            soundPlayer.PlayOneShot(instrument[currentQuestion.root + 5], 1); // Perfect 4th
            break; 
        case 4:
            soundPlayer.PlayOneShot(instrument[currentQuestion.root + 7], 1); // Perfect 5th
            break; 
        case 5:
            soundPlayer.PlayOneShot(instrument[currentQuestion.root + 9], 1); // Major 6th
            break; 
        case 6:
            soundPlayer.PlayOneShot(instrument[currentQuestion.root + 11], 1); // Major 7th
            break; 
        case 7:
            soundPlayer.PlayOneShot(instrument[currentQuestion.root + 12], 1); // Octave
            break; 
        }
        playButton.enabled = true;
        foreach (GameObject button in answerButtons) 
        {
            button.GetComponent<Button>().enabled = true;
        }
        Debug.Log("Interval: " + currentQuestion.interval);
    }

    // Function linked to buttons onClick()
    public override void answer() 
    {
        GameObject button = EventSystem.current.currentSelectedGameObject;
        // Check if answer is correct
        if (answerSet[currentQuestion.interval].getName() == button.name)
        {
            base.correct(button, currentQuestion.interval);
        } 
        else 
        {
            Debug.Log("Incorrect");
            failed = true;
            button.GetComponent<Image>().color = new Color32(255, 80, 80, 255);
        }
    }

    public override string resultsToString() 
    {
        string results = "";

        foreach(KeyValuePair<int, Answer> entry in answerSet)
        {
            results += entry.Value.toString();
        }

        return results;
    }
}
