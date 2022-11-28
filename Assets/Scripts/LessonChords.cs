using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class LessonChords : MonoBehaviour
{
    private static int page_count = 5;

    // UI Components
    // Unmoving
    public Slider progressBar;
    public Button nextButton;
    public Button prevButton;
    // Pages
    public GameObject page1;
    public GameObject page2;
    public GameObject page3;
    public GameObject page4;
    public GameObject page5;
    public GameObject page6;


    private float progress;
    private float fillSpeed = 0.15f;
    private int curr_page;
    
    // Start is called before the first frame update
    void OnEnable()
    {
        this.progress = 0.0f;
        this.curr_page = 0;

        nextButton.onClick.AddListener(() => next_page());

        prevButton.onClick.AddListener(() => prev_page());
        prevButton.gameObject.SetActive(false);

        // Make only first page visible TODO: Add animation support making this obsolete
        this.page1.SetActive(true);
        this.page2.SetActive(false);
        this.page3.SetActive(false);
        this.page4.SetActive(false);
        this.page5.SetActive(false);
        this.page6.SetActive(false);
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
