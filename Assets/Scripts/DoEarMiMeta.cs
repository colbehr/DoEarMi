using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    private User current_user;

    private static string filepath = "./Assets/Users/";
    
    // Default instrument for all users
    private static string default_instrument = "EPianoI";
    // Default icon for all users
    private static string default_icon = "ProfileIcons/defaultIconTest";

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

    public string get_default_icon()
    {
        return default_icon;
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
    // if we call this from this class, we overflow because user also needs this class
    public void synthetic_user_data()
    {
        string[] randomUsername = new string[] { "ShrimpAce",  "HackSource", "ArmDomino", "PonyChain", "BrownHip", "FreakyUnity", "CottonTrash", "SoftPro", "BravoMouth", "LooHammer", "RedWalnut", "MewRapture", "AbyssBalloon", "MountCloud", "ToeBeetle", "TradeBling", "StrifeLab", "ReinNet", "DittoFate", "FlatRugby", "SublimeMisfit", "PuddingValley", "JungleRealm", "UnholyCastle", "HonMisfit", "HoodieSalter", "PastKitten", "SkunkFrenzy", "GabSucker", "StereoSoldier", "LedRude", "BrownBale", "ExpatCatcher", "RabbiBrigade", "ArtsyCaptain", "GladNurse", "RiseAgency", "LimeTango", "SoapCob", "CafeBond", "WoollySquad", "MissionTest", "RabbitBomb", "BranchNorm", "HideMagnum", "CandidSparrow", "WeepingWake", "HydroDelight", "GumCity", "OfficerSprite", "ElegantHill", "KiloJoke", "PawBloom", "BanterWonder", "BiggerBeam", "MacroPanda", "PsychPile", "PuckPeanut",  "GentleBull", "TruckFort", "RoteActive", "FizzyFable", "ByteDrink", "CheesyTaker", "SadRobe", "PonchoRand", "ChicLog", "MoodySlave", "CraftNone", "RiverRabbit", "TalonSwitch", "AbyssalChoice", "GentlePilgrim", "LoopHop", "HonestAge", "ShadyParson", "SealLawyer", "PlanPharaoh", "SlimRoyalty", "CorpusGrunt", "LoudDraw", "RagBeater", "WifeAlchemy", "TechieNexus", "MonsterMilk", "LiftedWaste", "DubiousCoffee", "AverageJanitor", "GossipGirl", "BisonArmour", "DynastyFame", "PugVenture", "ViciousWasabi", "DayBread", "BrazenSwift", "SketchyBeast", "FeelingWord", "PaperMachine", "BrassHorse", "DruidTycoon", "PlagueGarage", "QueerWorker", "BoneBurrito", "GloomyVet", "CreepEngine", "MonkeyTrooper", "CorruptJag", "OrganicEnigma", "BloggerDrop", "SaintDelight", "FactNose", "GamingRadar", "SackColon", "SharpCrew", "HarshBoxer", "EnigmaPerk", "FlowerJinx" };
        int rnd = Random.Range(0, randomUsername.Length);
        Debug.Log(randomUsername[rnd]);

        User user = new User((string)randomUsername[rnd], "password1", (string)randomUsername[rnd] + "@email.com");
        user.update_xp(Random.Range(0, 1200));
        save_user_data(user);
    }

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

    public List<User> load_all_users()
    {
        // clears list of users before re-loading them
        this.users = new List<User>();

        foreach (string file in System.IO.Directory.GetFiles(filepath)) 
        {
            if (!file.EndsWith(".meta"))
            {
                string s = System.IO.File.ReadAllText(file);
                User user = JsonUtility.FromJson<User>(s);
                this.users.Add(user);
                // this.user_files.Add(user.get_uID(), (string) file);
                // Debug.Log(user.get_username() + " " + user.get_xp());
            }
        }

        return this.users;
    }

    public List<User> get_users()
    {
        return this.users;
    }

    public User get_curr_user()
    {
        return this.current_user;
    }

    // Must only be called on successful sign in
    public void set_curr_user(User user)
    {
        this.current_user = user;
    }
}
