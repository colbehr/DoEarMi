using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class LessonChords : MonoBehaviour
{
    private static int page_count = 6;

    // UI Components
    // Unmoving
    public Slider progressBar;
    public Button nextButton;
    public Button prevButton;
    // Pages
    public GameObject pages;
    public GameObject page1;
    public GameObject page2;
    public GameObject page3;
    public GameObject page4;
    public GameObject page5;
    public GameObject page6;
    //Animations
    public Animation nextPage;
    public Transform transform;


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
        nextPage = pages.GetComponent<Animation>();
        transform = pages.GetComponent<Transform>();

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
                break;
            case 5:
                Debug.Log(curr_page);
                this.page5.SetActive(false);
                this.page6.SetActive(true);
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
                this.page5.SetActive(true);
                this.page6.SetActive(false);
                break;
            case 5:
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
