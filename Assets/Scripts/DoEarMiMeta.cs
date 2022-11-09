using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


// Class to contain info about all users, generate/load user save states
public sealed class DoEarMiMeta
{
    // Singleton
    private static DoEarMiMeta instance = null;

    // Thread-safety locks for singleton and files
    private static readonly object instance_padlock = new object();
    private static readonly object file_padlock = new object();

    private List<User> users;
    private Hashtable user_files;

    private static string filepath = "./Assets/Users/";
    
    // Default instrument for all users
    private static string default_instrument = "EPianoI";

    // Instance is null initializer
    private DoEarMiMeta()
    {
        users = new List<User>();
        user_files = new Hashtable();
    }

    // Entry point for DoEarMiMeta singleton
    public static DoEarMiMeta Instance()
    {
        // double locking
        if (instance == null)
        {
            lock (instance_padlock)
            {
                if (instance == null)
                {
                    instance = new DoEarMiMeta();
                }
            }
        }

        return instance;
    }


    public string get_default_instrument()
    {
        return default_instrument;
    }


    // User added when new account is created
    public void add_user(User user)
    {
        // add user to user list
        users.Add(user);

        // add user to user_files hashtable, where uID is the key and filepath is the value
        string uID = user.get_uID();
        string user_file = "user_" + uID + ".json";
        user_files.Add(uID, user_file);

        // save new user data
        save_user_data(user);
    }


    // TODO: Add function to generate synthetic user data for prototype
    // public void synthetic_user_data() {}


    // TODO: how often/where should this be called? Do we need an observer class?
    public void save_user_data(User user)
    {
        string user_json = user.user_to_json();
        string uID = user.get_uID();
        string path = filepath + user_files[uID].ToString();

        lock (file_padlock)
        {
            // write user json to file, overwrites any existing content
            File.WriteAllText(path, user_json);
        }
    }
}
