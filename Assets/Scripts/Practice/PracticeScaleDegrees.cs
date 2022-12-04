using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Linq;

public class PracticeScaleDegrees : Practice
{
    private Question currentQuestion;

    private struct Question {
        public int key;
        // Major Scale Degrees:
        // 0 = do, 2 = re, 4 = mi, 5 = fa, 7 = sol, 9 = la, 11 = ti
        public int degree;
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
        currentQuestion.key = (int) Random.Range(5, instrument.Count - 14);       
        // Set random Major scale degree
        switch (Random.Range(1, 8)) {
        case 1:
            currentQuestion.degree = 0; // Do
            break; 
        case 2:
            currentQuestion.degree = 2; // Re
            break; 
        case 3:
            currentQuestion.degree = 4; // Mi
            break; 
        case 4:
            currentQuestion.degree = 5; // Fa
            break; 
        case 5:
            currentQuestion.degree = 7; // Sol
            break; 
        case 6:
            currentQuestion.degree = 9; // La
            break; 
        case 7:
            currentQuestion.degree = 11; // Ti
            break; 
        }
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
        soundPlayer.PlayOneShot(instrument[currentQuestion.key], 1);
        soundPlayer.PlayOneShot(instrument[currentQuestion.key + 4], 1);
        soundPlayer.PlayOneShot(instrument[currentQuestion.key + 7], 1);
        yield return new WaitForSeconds(0.5F);
        soundPlayer.PlayOneShot(instrument[currentQuestion.key + 5], 1);
        soundPlayer.PlayOneShot(instrument[currentQuestion.key + 9], 1);
        soundPlayer.PlayOneShot(instrument[currentQuestion.key + 12], 1);
        yield return new WaitForSeconds(0.5F);
        soundPlayer.PlayOneShot(instrument[currentQuestion.key + 7], 1);
        soundPlayer.PlayOneShot(instrument[currentQuestion.key + 11], 1);
        soundPlayer.PlayOneShot(instrument[currentQuestion.key + 14], 1);
        yield return new WaitForSeconds(0.5F);
        soundPlayer.PlayOneShot(instrument[currentQuestion.key], 1);
        soundPlayer.PlayOneShot(instrument[currentQuestion.key + 4], 1);
        soundPlayer.PlayOneShot(instrument[currentQuestion.key + 7], 1);
        yield return new WaitForSeconds(1);
        soundPlayer.PlayOneShot(instrument[currentQuestion.key + currentQuestion.degree], 1);
        playButton.enabled = true;
        foreach (GameObject button in answerButtons) 
        {
            button.GetComponent<Button>().enabled = true;
        }
        Debug.Log("Degree: " + currentQuestion.degree);
    }

    // Function linked to buttons onClick()
    public override void answer() 
    {
        GameObject button = EventSystem.current.currentSelectedGameObject;
        bool correct = false;

        // Check if answer is correct (should probably find a nicer way to do this)
        if ((currentQuestion.degree == 0 && button.name == "Do") 
            || (currentQuestion.degree == 2 && button.name == "Re") 
            || (currentQuestion.degree == 4 && button.name == "Mi") 
            || (currentQuestion.degree == 5 && button.name == "Fa")
            || (currentQuestion.degree == 7 && button.name == "Sol") 
            || (currentQuestion.degree == 9 && button.name == "La") 
            || (currentQuestion.degree == 11 && button.name == "Ti")) 
        {
            correct = true;
        } 

        if (correct) 
        {
            soundPlayer.PlayOneShot(instrument[currentQuestion.key + currentQuestion.degree], 1);
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
