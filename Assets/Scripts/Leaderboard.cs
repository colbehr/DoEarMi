using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Leaderboard : MonoBehaviour
{

    public GameObject leaderboardPositions;
    public GameObject userPosPrefab;
    // string path = "./Assets/Users/";   // Moved loading user logic to DoEarMiMeta.cs, path not necessary here
    private Vector3 offset;
    private Quaternion rotation;
    List<User> users = new List<User>();
    // Start is called before the first frame update
    void Start()
    {
        offset = Vector3.zero;
        rotation = Quaternion.Euler(0, 0, 0);
        
        users = DoEarMiMeta.Instance().load_all_users();

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
