using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class LessonChords : MonoBehaviour
{
    private static int page_count = 2;

    // UI Components
    public Slider progressBar;
    public Button nextButton;
    public Button prevButton;

    private Progress progress;
    private int curr_page;
    
    // Start is called before the first frame update
    void Start()
    {
        this.progress = new Progress(page_count);
        this.curr_page = 0;

        nextButton.onClick.AddListener(() => next_page());

        prevButton.onClick.AddListener(() => prev_page());
        // prevButton.gameObject.SetActive(false);
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
                break;
        }
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


        switch(curr_page)
        {
            case 0:
                Debug.Log(curr_page);
                break;
            case 1:
                Debug.Log(curr_page);
                break;
        }
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
