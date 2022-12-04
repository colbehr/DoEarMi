using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class LessonMeloDict : MonoBehaviour
{
    private static int page_count = 5;
    private static float wait_time = 1.0f; // wait time before playing next audioclip

    // UI Components
    // Unmoving
    public Slider progressBar;
    public Button nextButton;
    public Button prevButton;
    public LessonComponentManipulator uiManipulator;
    public LessonPlayer audioPlayer;
    // Pages
    public GameObject page1;
    public GameObject page2;
    public GameObject page3;
    public GameObject page4;
    public GameObject page5;

    private static readonly object reset_padlock = new object();
    private float progress;
    private float fillSpeed = 0.3f;
    private int curr_page;
    
    // Start is called before the first frame update
    void OnEnable()
    {
        uiManipulator = GetComponent<LessonComponentManipulator>();
        audioPlayer = GetComponent<LessonPlayer>();
        reset();
    }

    void OnDisable()
    { 
        reset();
    }

    void Update()
    {
        if (progressBar.value < progress) 
        {
            progressBar.value += fillSpeed * Time.deltaTime; 
        }  
        else if (progressBar.value > progress)
        {
            progressBar.value -= fillSpeed * Time.deltaTime; 
        }       
    }

    public void reset()
    {
        lock (reset_padlock)
        {
            this.progress = 0.0f;
            this.curr_page = 0;

            
            nextButton.onClick.RemoveAllListeners();
            prevButton.onClick.RemoveAllListeners();

            nextButton.onClick.AddListener(() => next_page());

            prevButton.onClick.AddListener(() => prev_page());
            prevButton.gameObject.SetActive(false);

            progressBar.value = 0;
            update_progress_text();

            // Make only first page visible TODO: Add animation support making this obsolete
            this.page1.SetActive(true);
            this.page2.SetActive(false);
            this.page3.SetActive(false);
            this.page4.SetActive(false);
            this.page5.SetActive(false);

            nextButton.gameObject.SetActive(true);
            prevButton.gameObject.SetActive(false);
            uiManipulator.activate_pitches(true);
        }
    }


    public void play_scale(Button btn)
    {
        int pitches = uiManipulator.pitches.Count;
        // disable button until scale finishes playing (avoids bugs & weird behavior)
        btn.interactable = false;
        StartCoroutine(enable_btn(pitches, btn, wait_time*pitches + (wait_time/3)));

        for (int i=0; i<pitches; i++)
        {
            StartCoroutine(wait_play(i, wait_time*i));
            audioPlayer.play_major_scale("C", 1, wait_time);
            StartCoroutine(wait_off(i, wait_time*i + (wait_time)));
        }
    }

    public void play_simple_melody(Button btn)
    {
        string[] simple_melody = new string[]{"C", "G", "D", "E"};
        play_melody(btn, simple_melody);
    }

    public void play_complex_melody(Button btn)
    {
        string[] complex_melody = new string[]{"C", "A", "G", "B", "E", "F", "D", "C"};
        play_melody(btn, complex_melody);
    }

    public void play_octave_melody(Button btn)
    {
        string[] octave_melody = new string[]{"A", "C", "E"};
        play_melody(btn, octave_melody);
        StartCoroutine(audioPlayer.play_note_by_name("C", 2, wait_time*(octave_melody.Length)));
        StartCoroutine(wait_play(7, wait_time*(octave_melody.Length)));
        StartCoroutine(wait_off(7, wait_time*(octave_melody.Length) + wait_time));
    }

    public void play_melody(Button btn, string[] melody)
    {
        List<int> melodyIndices = audioPlayer.get_pitch_indices(melody);

        for (int i=0; i<melody.Length; i++)
        {
            StartCoroutine(wait_play(melodyIndices[i], wait_time*i));
            StartCoroutine(audioPlayer.play_note_by_name(melody[i], 1, wait_time*i));
            // audioPlayer.play_melody("C", 1, wait_time);
            StartCoroutine(wait_off(melodyIndices[i], wait_time*i + (wait_time)));
        }
    }

    IEnumerator enable_btn(int pitches, Button btn, float time)
    {
        yield return new WaitForSeconds(time);
        btn.interactable = true;
    }


    IEnumerator wait_play(int pitchIndex, float time)
    {
        yield return new WaitForSeconds(time);
        uiManipulator.play_color(pitchIndex);
    }

    IEnumerator wait_off(int pitchIndex, float time)
    {
        yield return new WaitForSeconds(time);
        uiManipulator.off_color(pitchIndex);
    }


    private void update_progress_text()
    {
        progressBar.transform.Find("PercentCompleteText").GetComponent<TMPro.TMP_Text>().SetText((int)(progress*100) + "% Complete");  
    }

    public void next_page()
    {
        increase_curr_page();

        prevButton.gameObject.SetActive(true);
        // If on last page, disable next page button
        if (curr_page + 1 == page_count)
        {
            nextButton.gameObject.SetActive(false);
        }

        switch(curr_page)
        {
            case 0:
                Debug.Log(curr_page);
                break;
            case 1:
                Debug.Log(curr_page);
                this.page1.SetActive(false);
                this.page2.SetActive(true);
                break;
            case 2:
                Debug.Log(curr_page);
                this.page2.SetActive(false);
                this.page3.SetActive(true);
                break;
            case 3:
                Debug.Log(curr_page);
                this.page3.SetActive(false);
                this.page4.SetActive(true);
                break;
            case 4:
                Debug.Log(curr_page);
                this.page4.SetActive(false);
                this.page5.SetActive(true);
                uiManipulator.activate_pitches(false);
                break;
        }

        progress += (float) 1/(page_count-1);
        update_progress_text();  
    }


    public void prev_page()
    {
        decrease_curr_page();
        nextButton.gameObject.SetActive(true);

        // If on first page, disable previous page button
        if (curr_page == 0)
        {
            prevButton.gameObject.SetActive(false);
        }

        uiManipulator.activate_pitches(true);

        switch(curr_page)
        {
            case 0:
                Debug.Log(curr_page);
                this.page1.SetActive(true);
                this.page2.SetActive(false);
                break;
            case 1:
                Debug.Log(curr_page);
                this.page2.SetActive(true);
                this.page3.SetActive(false);
                break;
            case 2:
                Debug.Log(curr_page);
                this.page3.SetActive(true);
                this.page4.SetActive(false);
                break;
            case 3:
                Debug.Log(curr_page);
                this.page4.SetActive(true);
                this.page5.SetActive(false);
                break;
            case 4:
                Debug.Log(curr_page);
                break;
        }

        progress -= (float) 1/(page_count-1);
        update_progress_text();
    }


    private void increase_curr_page()
    {
        if (curr_page+1 < page_count)
        {
            curr_page += 1;
        }
    }

    private void decrease_curr_page()
    {
        if (curr_page > 0)
        {
            curr_page -= 1;
        }
    }

}
