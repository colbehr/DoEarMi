using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Linq;

public class PracticeChord : Practice
{
    private Question currentQuestion;

    private struct Question {
        public int root;
        // 1 = Major, 2 = Minor, 3 = Augmented, 4 = Diminished
        public int chordType;
    }

    // Randomly generate and setup a question after short delay
    public override void generateQuestion(float delayTime)
    {
        StartCoroutine(DelayNextQuestion(delayTime));
    }

    IEnumerator DelayNextQuestion(float delayTime)
    {
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
        currentQuestion.root = (int) Random.Range(10, instrument.Count - 10);
        currentQuestion.chordType = (int) Random.Range(1, 5);
        playQuestion();
    }

    // Play chord based on current Question
    public override void playQuestion() {
        switch (currentQuestion.chordType) {
        case 1:
            soundPlayer.PlayOneShot(instrument[currentQuestion.root], 1);
            soundPlayer.PlayOneShot(instrument[currentQuestion.root + 4], 1);
            soundPlayer.PlayOneShot(instrument[currentQuestion.root + 7], 1); 
            break; 
        case 2:
            soundPlayer.PlayOneShot(instrument[currentQuestion.root], 1);
            soundPlayer.PlayOneShot(instrument[currentQuestion.root + 3], 1);
            soundPlayer.PlayOneShot(instrument[currentQuestion.root + 7], 1); 
            break;
        case 3:
            soundPlayer.PlayOneShot(instrument[currentQuestion.root], 1);
            soundPlayer.PlayOneShot(instrument[currentQuestion.root + 4], 1);
            soundPlayer.PlayOneShot(instrument[currentQuestion.root + 8], 1); 
            break; 
        case 4:
            soundPlayer.PlayOneShot(instrument[currentQuestion.root], 1);
            soundPlayer.PlayOneShot(instrument[currentQuestion.root + 3], 1);
            soundPlayer.PlayOneShot(instrument[currentQuestion.root + 6], 1); 
            break;
        }
        Debug.Log("Root: " + currentQuestion.root + " ChordType: " + currentQuestion.chordType);
    }

    // Function linked to buttons onClick()
    public override void answer() 
    {
        GameObject button = EventSystem.current.currentSelectedGameObject;
        bool correct = false;

        // Check if answer is correct (should probably find a nicer way to do this)
        if ((currentQuestion.chordType == 1 && button.name == "Major") 
            || (currentQuestion.chordType == 2 && button.name == "Minor") 
            || (currentQuestion.chordType == 3 && button.name == "Augmented") 
            || (currentQuestion.chordType == 4 && button.name == "Diminished")) 
        {
            correct = true;
        } 

        if (correct) 
        {
            playQuestion();
            base.correct(button);
        } 
        else 
        {
            Debug.Log("Incorrect");
            failed = true;
            button.GetComponent<Image>().color = new Color32(255, 80, 80, 255);
        }
    }
}
