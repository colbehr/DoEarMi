using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Linq;

public abstract class Practice : MonoBehaviour
{
    public AudioSource soundPlayer;
    public LoadAudioAsInstrument instrumentLoader;
    public List<AudioClip> instrument;

    public int numberOfQuestions = 5;
    public int questionNumber;
    public int score;
    // True if user failed current question
    public bool failed;
    public List<GameObject> answerButtons = new List<GameObject>();
    public GameObject practiceScreen;
    public Animation practiceScreenAnim;
    
    public Button playButton;
    public Slider progressBar;
    public float progress;
    public float fillSpeed = 0.15f;

    // Backend Fields
    public DoEarMiMeta meta;
    public User user;

    // Start is called the first time GameObejct is enabled
    void Start()
    {
        practiceScreenAnim = practiceScreen.GetComponent<Animation>();
        soundPlayer = gameObject.GetComponent<AudioSource>();
        if (soundPlayer == null)
        {
            soundPlayer = gameObject.AddComponent<AudioSource>();
        }
    }

    // OnEnable is called every time GameObejct is enabled
    void OnEnable() 
    {
        meta = DoEarMiMeta.Instance();
        user = meta.load_all_users().ElementAt(0); // TODO: this is test user until login is implemented 
        playButton.onClick.AddListener(playQuestion);

        instrumentLoader = LoadAudioAsInstrument.Instance();
        instrument = instrumentLoader.get_instrument("EPianoI"); // TODO: get instrument from user
        foreach (GameObject button in answerButtons) 
        {
            button.SetActive(true);
            button.GetComponent<Image>().color = new Color32(98, 182, 203, 255);
            button.GetComponent<Button>().enabled = true;
        }

        score = 0;
        failed = false;
        progress = 0;
        progressBar.value = 0;
        questionNumber = 1;
        progressBar.transform.Find("PercentCompleteText").GetComponent<TMPro.TMP_Text>().SetText(0 + "% Complete");
        generateQuestion(0.5F);
    }

    // OnDisable is called when Object is disabled
    void OnDisable()
    {
        foreach (GameObject button in answerButtons) 
        {
            button.SetActive(false);
        }
        playButton.onClick.RemoveListener(playQuestion);
    }

    // Update is called once per frame
    void Update()
    {
        if (progressBar.value < progress) 
        {
            progressBar.value += fillSpeed * Time.deltaTime; 
        }         
    }

    // Randomly generate and setup a question after short delay
    public abstract void generateQuestion(float delay);

    // Play chord based on current Question
    public abstract void playQuestion();

    // Function linked to buttons onClick()
    public abstract void answer();

    // Called on correct answer
    public void correct(GameObject button) {
        Debug.Log("Correct");
        button.GetComponent<Image>().color = new Color32(100, 200, 130, 255);
        if (!failed)
            score++;
        progress = progressBar.value + (float)1/numberOfQuestions;
        progressBar.transform.Find("PercentCompleteText").GetComponent<TMPro.TMP_Text>().SetText((int)(progress*100) + "% Complete");
        
        if (questionNumber == numberOfQuestions) 
        {
            completePractice();
        } 
        else 
        {
            questionNumber++;
            generateQuestion(2.5F);
        }
    }

    // Called when practice is finished
    public void completePractice() 
    {
        disable(4);
        int xpGain = score * 10;
        int creditGain = score * 100;
        user.update_xp(xpGain);
        user.update_credits(creditGain);
        Debug.Log("Score:" + score + "/" + numberOfQuestions);
        Debug.Log("Exp increase: " + xpGain + " Currency increase: " + creditGain);
        Debug.Log("User now has " + user.get_xp() + " XP and " + user.get_credits() + " credits");
        //TODO: user feedback on credits/exp
    } 

    // Disable GameObject after short delay
    private void disable(float delayTime)
    {
        //TODO: Update user based on results
        StartCoroutine(DelayDisable(delayTime));
    }
    
    IEnumerator DelayDisable(float delayTime)
    {
        foreach (GameObject button in answerButtons) 
        {
            button.GetComponent<Button>().enabled = false;
        }
        yield return new WaitForSeconds(delayTime);
        gameObject.SetActive(false);
        practiceScreenAnim.Play("OverlayHide");
    }
}
