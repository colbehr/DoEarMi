using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Leaderboard : MonoBehaviour
{

    public GameObject leaderboardPositions;
    public GameObject userPosPrefab;
    string path = "./Assets/Users/";
    private Vector3 offset;
    private Quaternion rotation;
    List<User> users = new List<User>();
    // Start is called before the first frame update
    void Start()
    {
        offset = Vector3.zero;
        rotation = Quaternion.Euler(0, 0, 0);
        foreach (string file in System.IO.Directory.GetFiles(path)) {
            if (!file.EndsWith(".meta")) {
                string s = System.IO.File.ReadAllText(file);
                User employeesInJson = JsonUtility.FromJson<User>(s);
                users.Add(employeesInJson);
                Debug.Log(employeesInJson.get_username() + " " + employeesInJson.get_xp());
                
            }
        }

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
