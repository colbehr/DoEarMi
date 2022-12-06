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

    public Dictionary<int, Answer> answerSet = new Dictionary<int, Answer> ();
    public int numberOfQuestions = 5;
    public int questionNumber;
    public int score;
    // True if user failed current question
    public bool failed;
    public List<GameObject> answerButtons = new List<GameObject>();
    public GameObject practiceScreen;
    public GameObject accuracyText;
    public GameObject expCreditText;
    public GameObject pulse;
    public Animation practiceScreenAnim;
    
    public Button playButton;
    public Button retryButton;
    public Slider progressBar;
    public float progress;
    public float fillSpeed = 0.15f;

    public GameObject resultPanel;

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
        loadAnswers();
    }

    // OnEnable is called every time GameObejct is enabled
    void OnEnable() 
    {
        meta = DoEarMiMeta.Instance();
        user = meta.get_curr_user();
        playButton.onClick.AddListener(playQuestion);
        retryButton.onClick.AddListener(initializePractice);

        instrumentLoader = LoadAudioAsInstrument.Instance();
        instrument = instrumentLoader.get_instrument(user.get_active_instrument()); // TODO: get instrument from user
        initializePractice();
    }

    // OnDisable is called when Object is disabled
    void OnDisable()
    {
        foreach (GameObject button in answerButtons) 
        {
            button.SetActive(false);
        }
        playButton.onClick.RemoveListener(playQuestion);
        retryButton.onClick.RemoveListener(initializePractice);
        practiceScreenAnim.Play("OverlayHide");
    }

    // Update is called once per frame
    void Update()
    {
        if (progressBar.value < progress) 
        {
            progressBar.value += fillSpeed * Time.deltaTime; 
        }         
    }

    void initializePractice()
    {
        resultPanel.SetActive(false);
        foreach (GameObject button in answerButtons) 
        {
            button.SetActive(true);
            button.GetComponent<Image>().color = new Color32(98, 182, 203, 255);
            button.GetComponent<Button>().enabled = true;
        }
        foreach(KeyValuePair<int, Answer> entry in answerSet)
        {
            entry.Value.reset();
        }
        score = 0;
        failed = false;
        progress = 0;
        progressBar.value = 0;
        questionNumber = 1;
        progressBar.transform.Find("PercentCompleteText").GetComponent<TMPro.TMP_Text>().SetText(0 + "% Complete");
        generateQuestion(0.5F);
    }

    // Randomly generate and setup a question after short delay
    public abstract void generateQuestion(float delay);

    // Play audio based on current Question
    public abstract void playQuestion();

    // Function linked to buttons onClick()
    public abstract void answer();

    // Initialize answer dictionary
    public abstract void loadAnswers();

    // Create results string based on practice mode
    public abstract string resultsToString();

    // Called on correct answer
    public void correct(GameObject button, int answerKey) {
        Debug.Log("Correct");
        button.GetComponent<Image>().color = new Color32(100, 200, 130, 255);
        if (!failed) 
        {
            answerSet[answerKey].correct();
            score++;
        }
        else
        {
            answerSet[answerKey].incorrect();
        }
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
        int xpGain = score * 100;
        int creditGain = score * 2;
        user.update_xp(xpGain);
        user.update_credits(creditGain);
        Debug.Log("Score:" + score + "/" + numberOfQuestions);
        Debug.Log("Exp increase: " + xpGain + " Currency increase: " + creditGain);
        Debug.Log("User now has " + user.get_xp() + " XP and " + user.get_credits() + " credits");
        accuracyText.GetComponent<TMPro.TMP_Text>().SetText(resultsToString());
        expCreditText.GetComponent<TMPro.TMP_Text>().SetText("XP gained: " + xpGain 
        + " XP\nCurrency earned: " + creditGain
        + " <sprite=0 color=#E6B436>");
        StartCoroutine(displayResults(2));
    } 

    IEnumerator displayResults(float delayTime)
    {
        foreach (GameObject button in answerButtons) 
        {
            button.GetComponent<Button>().enabled = false;
        }
        yield return new WaitForSeconds(delayTime);
        resultPanel.SetActive(true);
    }

    public void activatePulse() 
    {
        if (pulse.activeSelf)
            pulse.SetActive(false);
        pulse.SetActive(true);
    }
}
