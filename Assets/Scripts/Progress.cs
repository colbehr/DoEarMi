using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Progress : MonoBehaviour
{
    private int page_count;
    private int curr_page;
    public int progress;

    //progress gameobject and text
    private GameObject progressRect;
    private int progressMinWidth;
    private int progressMaxWidth;
    private GameObject progressText;

    private void Start()
    {
        //set progress to 0%
        progressRect =  this.gameObject.transform.GetChild(0).gameObject;
        progressText =  this.gameObject.transform.GetChild(1).gameObject;

        progressMinWidth = 100;
        progressMaxWidth = 1050;
        progressRect.GetComponent<RectTransform>().sizeDelta = new Vector2(progressMinWidth, progressRect.GetComponent<RectTransform>().sizeDelta.y);
        progressText.GetComponent<TMPro.TMP_Text>().SetText("0% COMPLETE");

    }
    public Progress(int pages)
    {
        if (pages <= 0)
        {
            throw new ArgumentException("Number of pages for a session must be positive, non-zero value.\n");
        }
        else
        {
            this.page_count = pages;
            this.curr_page = 0;
        }
    }

    public void update_pagenum()
    {
        this.curr_page += 1;
    }

    public float get_progress()
    {
        progress = curr_page / page_count;
        return curr_page / page_count;
    }


    public void update_progressbar()
    {
        if (progress != 0)
        {
            float progressWidth = ((progress) * (progressMaxWidth - progressMinWidth)/100) + 100;
            progressRect.GetComponent<RectTransform>().sizeDelta = new Vector2(progressWidth, progressRect.GetComponent<RectTransform>().sizeDelta.y);
            progressText.GetComponent<TMPro.TMP_Text>().SetText((int)progress + "% COMPLETE");

        }
    }
}
