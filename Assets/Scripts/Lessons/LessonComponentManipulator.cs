using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class LessonComponentManipulator : MonoBehaviour
{
    private Color32 offColor = new Color32(33, 90, 124, 255);
    private Color32 playColor = new Color32(100, 200, 130, 255);

    public List<GameObject> pitches;

    // // Start is called before the first frame update
    // void Start()
    // {
        
    // }

    // // Update is called once per frame
    // void Update()
    // {
        
    // }

    public void play_color(int pitchIndex)
    {
        GameObject pitch = pitches.ElementAt(pitchIndex);

        pitch.GetComponent<Image>().color = playColor;
    }

}
