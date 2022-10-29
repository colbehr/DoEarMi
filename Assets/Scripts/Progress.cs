using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Progress 
{
    private int page_count;
    private int curr_page;
    

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
        return curr_page/page_count;
    }
    
}
