using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Accuracy
{
    private int correct;
    private int incorrect;

    // initialize correct and incorrect question counts to 0
    public Accuracy()
    {
        this.correct = 0;
        this.incorrect = 0;
    }

    // add 1 to correct question count upon correct answer
    public void add_correct()
    {
        this.correct += 1;
    }

    // add 1 to incorrect question count upon incorrect answer
    public void add_incorrect()
    {
        this.incorrect += 1;
    }

    // returns accuracy as percentage of correct over incorrect
    public float get_accuracy()
    {
        if (incorrect == 0) 
        {
            return correct;
        } 
        else 
        {
            return correct/incorrect;
        }
    }

}
