using System;

public class Answer 
{
    private string name;
    private int total = 0;
    private int numCorrect = 0;

    public Answer(string name) 
    {
        this.name = name;
    }

    public void correct() 
    {
        numCorrect++;
        total++;
    }

    public void incorrect() 
    {
        total++;
    }

    public void reset() 
    {
        total = 0;
        numCorrect = 0;
    }

    public string getName()
    {
        return name;
    }

    public string accuracyToString() {
        string accuracyString = numCorrect + "/" + total;
        if (total != 0) 
            accuracyString += " (" + Math.Round((float)numCorrect/total*100) + "%)\n";
        else 
            accuracyString += " (NA)\n";

        return accuracyString;
    }
}