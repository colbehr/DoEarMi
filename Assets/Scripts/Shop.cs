using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Shop : MonoBehaviour
{
    // Constants
    private static string FileDirectory = "ProfileIcons/";
    private static readonly object transaction_padlock = new object();
    private static int[] icon_costs = {150, 400, 1200};
    private static int[] instrument_costs = {250, 400, 250};
    private static int iconsPerPage = 3;
    private Vector3 textShift = new Vector3(17.5f, 0, 0); // used to move cost text after purchase
    private Color32 ownedColor = new Color(189, 231, 230, 255);

    // purchase message, replace @ with object type and * with credits cost
    private string purchaseMessage = "Purchase this @ for * credits?";
    private string cantPurchaseMessage = "Sorry, you don't have enough credits for this.\n\nYou can earn more credits in practice sessions.";
    private string addedMessage = "Purchase successful!\n\nOpen your profile settings to set as your active @.";

    // UI
    // Icons
    public List<GameObject> iconPages;
    public List<GameObject> iconGOs;
    public List<Button> iconPageButtons;
    // Instruments
    public List<GameObject> instrumentPages;
    public List<GameObject> instrumentGOs; 
    // Boosts
    // Other
    public GameObject creditsText;
    public GameObject purchasePopUp;
    public GameObject okPopUp;
    public AudioSource soundPlayer;

    private DoEarMiMeta meta;
    private User user;
    private LoadAudioAsInstrument instrumentLoader;
    private List<Sprite> iconSprites;
    private List<AudioClip> instruments;
    private Dictionary<string, int> spriteNameToCost;
    private Dictionary<string, int> instrumentNameToCost;
    private Dictionary<string, int> instrumentNameToIndex;
    // shouldn't be defined in this class
    private static int[] major_chord = { 0, 4, 7 };
    private static int[] chord_II    = { 5, 9, 12 };
    private static int[] chord_III   = { 7, 11, 14 };
    private List<int[]> chordProgression = new List<int[]>
    {
        major_chord,
        chord_II,
        chord_III,
        major_chord
    };

    private int curr_icon_page;
    private string[] curr_selected_item; // triplet contains item object type and filepath
    private GameObject curr_selected_GO;

    void Start()
    {
        this.meta = DoEarMiMeta.Instance();
        this.instrumentLoader = LoadAudioAsInstrument.Instance();
        this.iconSprites = new List<Sprite>();
        this.spriteNameToCost = new Dictionary<string, int>();
        this.instrumentNameToCost = new Dictionary<string, int>();
        this.instrumentNameToIndex = new Dictionary<string, int>();
        this.curr_selected_item = new string[] {"none", "none", "0"};

        // Load all Sprites
        UnityEngine.Object[] iconObjects = Resources.LoadAll(FileDirectory, typeof(Sprite));

        for (int i=0; i<iconObjects.Length; i++)
        {   
            // don't include any default images
            if (! ((Sprite) iconObjects[i]).name.Contains("default"))
            {
                iconSprites.Add((Sprite) iconObjects[i]);
            }
        }

        icon_page_change(0);
        init_sprite_costs();
        init_instrument_costs();
    }

    void OnEnable()
    {
        this.meta = DoEarMiMeta.Instance();
        user = meta.get_curr_user();

        icon_page_change(0);
        StartCoroutine(UI_purchased_wrapper());
    }

    void Update()
    {
        creditsText.GetComponent<TMPro.TextMeshProUGUI>().text = user.get_credits().ToString();
    }

    // ensures cost text replaced with owned text on enable
    IEnumerator UI_purchased_wrapper()
    {
        yield return new WaitForSeconds(0.001f);

        set_UI_purchased();
    }

    public void icon_page_change(int pageNum)
    {
        curr_icon_page = pageNum;
        iconPageButtons[pageNum].interactable = false;
        iconPages[pageNum].SetActive(true);

        for (int i=0; i<iconPageButtons.Count; i++)
        {
            if (i != pageNum)
            {
                iconPageButtons[i].interactable = true;
                iconPages[i].SetActive(false);
            }
        }
    }

    private void init_sprite_costs()
    {
        int cost_ind = 0;
        for (int i=0; i<iconGOs.Count; i++)
        {
            if(i%iconsPerPage == 0 && i != 0)
            {
                cost_ind +=1;
            }

            iconGOs[i].transform.GetChild(0).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = icon_costs[cost_ind].ToString();
            spriteNameToCost.Add(iconGOs[i].GetComponent<Image>().sprite.name, icon_costs[cost_ind]);
        }
    }

    // Weirdly done for now bc running out of time, should probs use dictionary
    private void init_instrument_costs()
    {
        for (int i=0; i<instrumentGOs.Count; i++)
        {
            instrumentGOs[i].transform.GetChild(0).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = instrument_costs[i].ToString();
            instrumentNameToCost.Add(instrumentGOs[i].transform.GetChild(1).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text, instrument_costs[i]);
            instrumentNameToIndex.Add(instrumentGOs[i].transform.GetChild(1).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text, i);
        }
    }

    // Opens prompt popup to confirm purchase or ok popup if its too expensive
    public void purchase_icon(int iconNum)
    {
        string spriteName = iconGOs[iconNum].GetComponent<Image>().sprite.name;
        int cost = spriteNameToCost[spriteName];

        // reset curr item for purchase confirmation
        curr_selected_item[0] = "icon";
        curr_selected_item[1] = FileDirectory + spriteName;
        curr_selected_item[2] = cost.ToString();
        curr_selected_GO = iconGOs[iconNum];

        // if can purchase, open purchase confirm prompt
        if (can_purchase(cost))
        {
            can_purchase_popup(curr_selected_item[0], curr_selected_item[2]);
        }
        // if can't purchase, open too expensive prompt
        else
        {
            cant_purchase_popup();
        }
    }

    // Opens prompt popup to confirm purchase or ok popup if its too expensive
    public void purchase_instrument(string name)
    {
        // play audio sample chord progression
        List<AudioClip> instruClips = instrumentLoader.get_instrument(name);
        play_sample(instruClips);

        int cost = instrumentNameToCost[name];

        curr_selected_item[0] = "instrument";
        curr_selected_item[1] = name;
        curr_selected_item[2] = cost.ToString();
        curr_selected_GO = instrumentGOs[instrumentNameToIndex[name]];

        if (can_purchase(cost))
        {
            can_purchase_popup(curr_selected_item[0], curr_selected_item[2]);
        }
        // if can't purchase, open too expensive prompt
        else
        {
            cant_purchase_popup();
        }
    }

    // set popup window string to match current purchase name and cost
    private void can_purchase_popup(string objectName, string cost)
    {
        string messageCopy = purchaseMessage;
        messageCopy = messageCopy.Replace("@", objectName);
        messageCopy = messageCopy.Replace("*", cost);
        GameObject child = purchasePopUp.transform.GetChild(0).gameObject; 
        child.transform.Find("PromptText").transform.gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = messageCopy;

        purchasePopUp.SetActive(true);
    }

    private void cant_purchase_popup()
    {
        GameObject child = okPopUp.transform.GetChild(0).gameObject;
        child.transform.Find("PromptText").transform.gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = cantPurchaseMessage;

        okPopUp.SetActive(true);
    }

    // Adds to user collections and opens successful purchase popup
    public void confirm_purchase()
    {
        string itemName = curr_selected_item[1];
        string messageCopy = addedMessage;
        GameObject child;

        if (itemName.Contains("Icon"))
        {
            user.add_icon(itemName);
            Debug.Log("icon added to user collection");
        }
        else if (itemName.Contains("Boost"))
        {
            Debug.Log("boosts aren't implemented sorry");
        }
        else
        {
            user.add_instrument(itemName);
            Debug.Log("instrument added to user collection");
        }

        user.update_credits(-1 * Int32.Parse(curr_selected_item[2]));
        set_single_UI_purchased();

        messageCopy = messageCopy.Replace("@", curr_selected_item[0]);
        child = okPopUp.transform.GetChild(0).gameObject;
        child.transform.Find("PromptText").transform.gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = messageCopy;

        purchasePopUp.SetActive(false);
        okPopUp.SetActive(true);
    }

    public void close_popups()
    {
        okPopUp.SetActive(false);
        purchasePopUp.SetActive(false);
    }

    private bool can_purchase(int cost)
    {
        return (user.get_credits() >= cost);
    }

    private void set_UI_purchased()
    {
        List<string> user_icons = user.get_icons();
        List<string> user_instruments = user.get_instruments();
        GameObject iconChild;
        GameObject instruChild;

        // sets icons owned
        foreach(GameObject iconGO in iconGOs)
        {
            foreach(string iconName in user_icons)
            {
                if(FileDirectory + iconGO.GetComponent<Image>().sprite.name == iconName)
                {
                    iconChild = iconGO.transform.GetChild(0).gameObject;
                    iconChild.GetComponent<TMPro.TextMeshProUGUI>().text = "Owned";
                    iconChild.GetComponent<TMPro.TextMeshProUGUI>().color = ownedColor;
                    iconChild.transform.position -= textShift;
                    iconChild.transform.GetChild(0).gameObject.SetActive(false);
                    iconGO.GetComponent<Button>().interactable = false;
                }
            }
        }

        foreach(GameObject instrumentGO in instrumentGOs)
        {
            foreach(string instruName in user_instruments)
            {
                if(instrumentGO.transform.GetChild(1).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text == instruName)
                {
                    instruChild = instrumentGO.transform.GetChild(0).gameObject;
                    instruChild.GetComponent<TMPro.TextMeshProUGUI>().text = "Owned";
                    instruChild.GetComponent<TMPro.TextMeshProUGUI>().color = ownedColor;
                    instruChild.transform.position -= textShift;
                    instruChild.transform.GetChild(0).gameObject.SetActive(false);
                    instrumentGO.GetComponent<Button>().interactable = false;
                }
            }
        }
    }

    private void set_single_UI_purchased()
    {
        GameObject go = curr_selected_GO;
        GameObject goChild;
        goChild = go.transform.GetChild(0).gameObject;
        goChild.GetComponent<TMPro.TextMeshProUGUI>().text = "Owned";
        goChild.GetComponent<TMPro.TextMeshProUGUI>().color = ownedColor;
        goChild.transform.position -= textShift;
        goChild.transform.GetChild(0).gameObject.SetActive(false);
        go.GetComponent<Button>().interactable = false;
    }

    // hacky 
    IEnumerator play_chord(int[] chordType, float wait_time, List<AudioClip> instru)
    {
        yield return new WaitForSeconds(wait_time);

        soundPlayer.PlayOneShot(instru[12 + chordType[0]], 1);
        soundPlayer.PlayOneShot(instru[12 + chordType[1]], 1);
        soundPlayer.PlayOneShot(instru[12 + chordType[2]], 1);  
    }

    // hacky, plays chord progression for C major
    public void play_sample(List<AudioClip> instru)
    {
        for (int i=0; i<chordProgression.Count; i++)
        {
            StartCoroutine(play_chord(chordProgression[i], 0.51f*i, instru));
        } 
    }

}
