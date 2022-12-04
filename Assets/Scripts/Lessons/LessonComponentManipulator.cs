using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class LessonComponentManipulator : MonoBehaviour
{
    // colors for highlighting pitch being played, change here if aesthetic changes (future scablability should have a color file)
    private Color32 offColor = new Color32(33, 90, 124, 255);
    private Color32 playColor = new Color32(100, 200, 130, 255);

    public List<GameObject> pitches;

    public void activate_pitches(bool status)
    {
        for(int i=0; i<pitches.Count; i++)
        {
            pitches[i].SetActive(status);
        }
    }

    public void play_color(int pitchIndex)
    {
        GameObject pitch = pitches.ElementAt(pitchIndex);

        pitch.GetComponent<Image>().color = playColor;
    }

    public void off_color(int pitchIndex)
    {
        GameObject pitch = pitches.ElementAt(pitchIndex);

        pitch.GetComponent<Image>().color = offColor;
    }

}
