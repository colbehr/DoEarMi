using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Session interface to be used in PracticeSession and LessonSession 
interface Session
{
    // Calls Session.update_progress(), load next page (scene?)
    public void next_page();

    // For practice sessions calls User.update_xp, User.update_credits
    // For lesson sessions, add checkmark next to lesson on main lesson page (User.update_xp?)
    public void end_session();
}
