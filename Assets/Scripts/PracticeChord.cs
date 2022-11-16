using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PracticeChord : MonoBehaviour
{
    public AudioSource soundPlayer;
    public LoadAudioAsInstrument instrumentLoader;
    private List<AudioClip> instrument;

    private User user; // TODO: get user
    private int numberOfQuestions;
    private int questionNumber;
    private int score;
    // True if user failed current question
    private bool failed;
    private Question currentQuestion;
    public List<GameObject> answerButtons = new List<GameObject>();
    public GameObject practiceScreen;
    private Animation practiceScreenAnim;
    
    public Slider progressBar;
    private float progress;
    private float fillSpeed = 0.15f;

    struct Question {
        public int root;
        // 1 = Major, 2 = Minor, 3 = Augmented, 4 = Diminished
        public int chordType;
    }

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
        instrumentLoader = LoadAudioAsInstrument.Instance();
        instrument = instrumentLoader.get_instrument("EPianoI"); // later get instrument from user
        foreach (GameObject button in answerButtons) 
        {
            button.GetComponent<Image>().color = new Color32(98, 182, 203, 255);
            button.GetComponent<Button>().enabled = true;
        }

        score = 0;
        failed = false;
        progress = 0;
        progressBar.value = 0;
        questionNumber = 1;
        numberOfQuestions = 5;
        progressBar.transform.Find("PercentCompleteText").GetComponent<TMPro.TMP_Text>().SetText(0 + "% Complete");
        generateQuestion(0.5F);
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
    private void generateQuestion(float delayTime)
    {
        StartCoroutine(DelayNextQuestion(delayTime));
    }

    IEnumerator DelayNextQuestion(float delayTime)
    {
        failed = false;
        foreach (GameObject button in answerButtons) 
        {
            button.GetComponent<Button>().enabled = false;
        }
        yield return new WaitForSeconds(delayTime);

        foreach (GameObject button in answerButtons) 
        {
            button.GetComponent<Image>().color = new Color32(98, 182, 203, 255);
            button.GetComponent<Button>().enabled = true;
        }

        currentQuestion = new Question();
        currentQuestion.root = (int) Random.Range(0, 0);       // TODO: Change bounds based on instrument range
        currentQuestion.chordType = (int) Random.Range(1, 5);
        playChord();
    }

    // Play chord based on current Question
    public void playChord() {
        switch(currentQuestion.chordType) {
        case 1:
            soundPlayer.PlayOneShot(instrument[currentQuestion.root], 1);
            soundPlayer.PlayOneShot(instrument[currentQuestion.root + 4], 1);
            soundPlayer.PlayOneShot(instrument[currentQuestion.root + 7], 1); 
            break; 
        case 2:
            soundPlayer.PlayOneShot(instrument[currentQuestion.root], 1);
            soundPlayer.PlayOneShot(instrument[currentQuestion.root + 3], 1);
            soundPlayer.PlayOneShot(instrument[currentQuestion.root + 7], 1); 
            break;
        case 3:
            soundPlayer.PlayOneShot(instrument[currentQuestion.root], 1);
            soundPlayer.PlayOneShot(instrument[currentQuestion.root + 4], 1);
            soundPlayer.PlayOneShot(instrument[currentQuestion.root + 8], 1); 
            break; 
        case 4:
            soundPlayer.PlayOneShot(instrument[currentQuestion.root], 1);
            soundPlayer.PlayOneShot(instrument[currentQuestion.root + 3], 1);
            soundPlayer.PlayOneShot(instrument[currentQuestion.root + 6], 1); 
            break;
        }
        Debug.Log("Root: " + currentQuestion.root + " ChordType: " + currentQuestion.chordType);
    }

    // Function linked to buttons onClick()
    public void answer() 
    {
        GameObject button = EventSystem.current.currentSelectedGameObject;
        bool correct = false;

        // Check if answer is correct (should probably find a nicer way to do this)
        if ((currentQuestion.chordType == 1 && button.name == "Major") 
            || (currentQuestion.chordType == 2 && button.name == "Minor") 
            || (currentQuestion.chordType == 3 && button.name == "Augmented") 
            || (currentQuestion.chordType == 4 && button.name == "Diminished")) 
        {
            correct = true;
        } 

        if (correct) 
        {
            Debug.Log("Correct");
            playChord();
            button.GetComponent<Image>().color = new Color32(100, 200, 130, 255);
            if (!failed)
                score++;
            progress = progressBar.value + (float)1/numberOfQuestions;
            progressBar.transform.Find("PercentCompleteText").GetComponent<TMPro.TMP_Text>().SetText((int)(progress*100) + "% Complete");
            if (questionNumber == numberOfQuestions) 
            {
                disable(4);
                Debug.Log("Score:" + score + "/" + numberOfQuestions);
            } 
            else 
            {
                questionNumber++;
                generateQuestion(2.5F);
            }
        } 
        else 
        {
            Debug.Log("Incorrect");
            failed = true;
            button.GetComponent<Image>().color = new Color32(255, 80, 80, 255);
        }
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
