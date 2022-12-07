using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Leaderboard : MonoBehaviour
{

    public GameObject leaderboardPositions;
    public GameObject top5Text;
    public GameObject userPosPrefab;
    // string path = "./Assets/Users/";   // Moved loading user logic to DoEarMiMeta.cs, path not necessary here
    private Vector3 offset;
    private Quaternion rotation;
    List<User> users = new List<User>();
    User currUser;
    // Start is called before the first frame update
    void OnEnable()
    {
        offset = Vector3.zero;
        rotation = Quaternion.Euler(0, 0, 0);
        DoEarMiMeta meta = DoEarMiMeta.Instance();
        users = meta.get_users();
        currUser = meta.get_curr_user();
        users.Sort((p1, p2) => p2.get_xp().CompareTo(p1.get_xp()));
        int rank = 1;
        foreach (User user in users){
            offset += new Vector3(0, -55, 0);
            // create prefab
            GameObject i = Instantiate(userPosPrefab, leaderboardPositions.transform.position + offset, rotation, leaderboardPositions.transform);
            /*
             * First child is icon
             * second is name
             * third is rank
             * fourth is xp
             */

            i.transform.GetChild(1).GetComponent<TMPro.TMP_Text>().SetText(user.get_username().ToUpper());
            i.transform.GetChild(3).GetComponent<TMPro.TMP_Text>().SetText(""+user.get_xp());
            i.transform.GetChild(2).GetComponent<TMPro.TMP_Text>().SetText(""+rank);
            rank++;

            //check if current user and change their color
            if (user.get_uID() == currUser.get_uID())
            {
                i.transform.GetChild(1).GetComponent<TMPro.TMP_Text>().color = new Color(103f / 255f, 170f / 255f, 110f / 255f);
                if (rank > 5) {
                    top5Text.GetComponent<TMPro.TMP_Text>().SetText("You are not in the Top 5");
                }
            }
            if (rank > 20) {
                break;
            }

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
