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
    private Hashtable user_files; // key: user ID, value: json filepath
    private Hashtable user_names; // key: username, value: User object
    private User current_user;
    private string[] usersList; 
    private static string filepath = "./Assets/Users/";
    
    // Default instrument for all users
    private static string default_instrument = "PianoI";
    // Default icon for all users
    private static string default_icon = "ProfileIcons/defaultIcon";

    // Instance is null initializer
    private DoEarMiMeta()
    {
        users = new List<User>();       // kept for now, but use user_names hashtable instead
        user_files = new Hashtable();
        user_names = new Hashtable();
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
        user_names.Add(user.get_username(), user.get_password());


        // save new user data
        save_user_data(user);
    }

    // TODO: Add function to generate synthetic user data for prototype
    // if we call this from this class, we overflow because user also needs this class
    public void synthetic_user_data()
    {
        string[] randomUsername = new string[] { "HackSource", "ArmDomino", "PonyChain", "BrownHip", "FreakyUnity", "CottonTrash", "SoftPro", "BravoMouth", "LooHammer", "RedWalnut", "MewRapture", "AbyssBalloon", "MountCloud", "ToeBeetle", "TradeBling", "StrifeLab", "ReinNet", "DittoFate", "FlatRugby", "SublimeMisfit", "PuddingValley", "JungleRealm", "UnholyCastle", "HonMisfit", "HoodieSalter", "PastKitten", "SkunkFrenzy", "GabSucker", "StereoSoldier", "LedRude", "BrownBale", "ExpatCatcher", "RabbiBrigade", "ArtsyCaptain", "GladNurse", "RiseAgency", "LimeTango", "SoapCob", "CafeBond", "WoollySquad", "MissionTest", "RabbitBomb", "BranchNorm", "HideMagnum", "CandidSparrow", "WeepingWake", "HydroDelight", "GumCity", "OfficerSprite", "ElegantHill", "KiloJoke", "PawBloom", "BanterWonder", "BiggerBeam", "MacroPanda", "PsychPile", "PuckPeanut",  "GentleBull", "TruckFort", "RoteActive", "FizzyFable", "ByteDrink", "CheesyTaker", "SadRobe", "PonchoRand", "ChicLog", "MoodySlave", "CraftNone", "RiverRabbit", "TalonSwitch", "AbyssalChoice", "GentlePilgrim", "LoopHop", "HonestAge", "ShadyParson", "SealLawyer", "PlanPharaoh", "SlimRoyalty", "CorpusGrunt", "LoudDraw", "RagBeater", "WifeAlchemy", "TechieNexus", "MonsterMilk", "LiftedWaste", "DubiousCoffee", "AverageJanitor", "GossipGirl", "BisonArmour", "DynastyFame", "PugVenture", "ViciousWasabi", "DayBread", "BrazenSwift", "SketchyBeast", "FeelingWord", "PaperMachine", "BrassHorse", "DruidTycoon", "PlagueGarage", "QueerWorker", "BoneBurrito", "GloomyVet", "CreepEngine", "MonkeyTrooper", "CorruptJag", "OrganicEnigma", "BloggerDrop", "SaintDelight", "FactNose", "GamingRadar", "SackColon", "SharpCrew", "HarshBoxer", "EnigmaPerk", "FlowerJinx" };
        int rnd = Random.Range(0, randomUsername.Length);
        Debug.Log(randomUsername[rnd]);

        for(int i=0; i<randomUsername.Length; i++)
        {
            User user = new User((string)randomUsername[i], "password1", (string)randomUsername[i] + "@email.com");
            user.update_xp(Random.Range(0, 1200));
            save_user_data(user);
        }

    }

    // TODO: how often/where should this be called? Do we need an observer class?
    public void save_user_data(User user)
    {
        string user_json = user.user_to_json();
        string uID = user.get_uID();

        // check if first time saving and needs the full path
        string interpath = "";
        if (!user_files[uID].ToString().Contains(filepath))
        {
            interpath = filepath;
        }
        string path = interpath + user_files[uID].ToString();

        lock (file_padlock)
        {
            // write user json to file, overwrites any existing content
            File.WriteAllText(path, user_json);
        }
    }
/*
    public List<User> load_all_users()
    {
        lock (file_padlock)
        {
            // clears list of users before re-loading them
            this.users = new List<User>();
            this.user_files = new Hashtable();
            this.user_names = new Hashtable();

            foreach (string file in System.IO.Directory.GetFiles(filepath)) 
            {
                if (!file.EndsWith(".meta"))
                {
                    string s = System.IO.File.ReadAllText(file);
                    User user = JsonUtility.FromJson<User>(s);
                    this.users.Add(user);

                    this.user_files.Add(user.get_uID(), (string) file);
                    this.user_names.Add(user.get_username().ToLower(), user); // must store usernames as lowercase for login ignore case
                    // Debug.Log(user.get_username() + " " + user.get_xp());
                }
            }

        }

        return this.users;
    }
    */
    public List<User> load_all_users2()
    {
        lock (file_padlock)
        {
            this.usersList = new string[] {"{\"username\":\"ShrimpAce\",\"uID\":\"d224fb65-be55-4be3-a8b1-605b00179cb2\",\"password\":\"password1\",\"email\":\"ShrimpAce@email.com\",\"xp\":14513,\"streak\":0,\"credits\":1188,\"streak_frozen\":false,\"boosted\":false,\"instruments\":[\"PianoI\",\"BassI\"],\"icons\":[\"ProfileIcons/defaultIcon\",\"ProfileIcons/faceIcon2\"],\"active_icon\":\"ProfileIcons/defaultIcon\",\"active_instrument\":\"PianoI\"}",
"{\"username\":\"HarshBoxer\",\"uID\":\"5a24997c-4be7-407a-ab55-923ef0a81254\",\"password\":\"password1\",\"email\":\"HarshBoxer@email.com\",\"xp\":474,\"streak\":0,\"credits\":100,\"streak_frozen\":false,\"boosted\":false,\"instruments\":[\"PianoI\"],\"icons\":[\"ProfileIcons/defaultIcon\"],\"active_icon\":\"ProfileIcons/defaultIcon\",\"active_instrument\":\"PianoI\"}",
"{\"username\":\"BrownBale\",\"uID\":\"5a620185-c8b4-4b09-bdef-a4cf5c44fec4\",\"password\":\"password1\",\"email\":\"BrownBale@email.com\",\"xp\":986,\"streak\":0,\"credits\":100,\"streak_frozen\":false,\"boosted\":false,\"instruments\":[\"PianoI\"],\"icons\":[\"ProfileIcons/defaultIcon\"],\"active_icon\":\"ProfileIcons/defaultIcon\",\"active_instrument\":\"PianoI\"}",
"{\"username\":\"PaperMachine\",\"uID\":\"5aebfddd-75c3-42e1-966a-df0e2fbfdc16\",\"password\":\"password1\",\"email\":\"PaperMachine@email.com\",\"xp\":577,\"streak\":0,\"credits\":100,\"streak_frozen\":false,\"boosted\":false,\"instruments\":[\"PianoI\"],\"icons\":[\"ProfileIcons/defaultIcon\"],\"active_icon\":\"ProfileIcons/defaultIcon\",\"active_instrument\":\"PianoI\"}",
"{\"username\":\"FeelingWord\",\"uID\":\"5cfb17d9-e729-4ade-99ef-1b502ab6b25f\",\"password\":\"password1\",\"email\":\"FeelingWord@email.com\",\"xp\":784,\"streak\":0,\"credits\":100,\"streak_frozen\":false,\"boosted\":false,\"instruments\":[\"PianoI\"],\"icons\":[\"ProfileIcons/defaultIcon\"],\"active_icon\":\"ProfileIcons/defaultIcon\",\"active_instrument\":\"PianoI\"}",
"{\"username\":\"HideMagnum\",\"uID\":\"5d38ac27-267a-41ee-9dc3-81808c6f7878\",\"password\":\"password1\",\"email\":\"HideMagnum@email.com\",\"xp\":703,\"streak\":0,\"credits\":100,\"streak_frozen\":false,\"boosted\":false,\"instruments\":[\"PianoI\"],\"icons\":[\"ProfileIcons/defaultIcon\"],\"active_icon\":\"ProfileIcons/defaultIcon\",\"active_instrument\":\"PianoI\"}",
"{\"username\":\"PuckPeanut\",\"uID\":\"5e2160e3-8b09-4a89-8209-a91bbb01cbf0\",\"password\":\"password1\",\"email\":\"PuckPeanut@email.com\",\"xp\":17,\"streak\":0,\"credits\":100,\"streak_frozen\":false,\"boosted\":false,\"instruments\":[\"PianoI\"],\"icons\":[\"ProfileIcons/defaultIcon\"],\"active_icon\":\"ProfileIcons/defaultIcon\",\"active_instrument\":\"PianoI\"}",
"{\"username\":\"PlanPharaoh\",\"uID\":\"5f86c8a8-5587-4a47-85a2-14aecd0393b6\",\"password\":\"password1\",\"email\":\"PlanPharaoh@email.com\",\"xp\":913,\"streak\":0,\"credits\":100,\"streak_frozen\":false,\"boosted\":false,\"instruments\":[\"PianoI\"],\"icons\":[\"ProfileIcons/defaultIcon\"],\"active_icon\":\"ProfileIcons/defaultIcon\",\"active_instrument\":\"PianoI\"}",
"{\"username\":\"HydroDelight\",\"uID\":\"0d90673e-ce46-46f2-8236-5fb21995820e\",\"password\":\"password1\",\"email\":\"HydroDelight@email.com\",\"xp\":1067,\"streak\":0,\"credits\":100,\"streak_frozen\":false,\"boosted\":false,\"instruments\":[\"PianoI\"],\"icons\":[\"ProfileIcons/defaultIcon\"],\"active_icon\":\"ProfileIcons/defaultIcon\",\"active_instrument\":\"PianoI\"}",
"{\"username\":\"HonMisfit\",\"uID\":\"0ec8aa2c-7e69-4166-add5-fd59d0cbcdae\",\"password\":\"password1\",\"email\":\"HonMisfit@email.com\",\"xp\":708,\"streak\":0,\"credits\":100,\"streak_frozen\":false,\"boosted\":false,\"instruments\":[\"PianoI\"],\"icons\":[\"ProfileIcons/defaultIcon\"],\"active_icon\":\"ProfileIcons/defaultIcon\",\"active_instrument\":\"PianoI\"}",
"{\"username\":\"ExpatCatcher\",\"uID\":\"0ec030ce-39b8-449d-b215-3a8bf8fe8050\",\"password\":\"password1\",\"email\":\"ExpatCatcher@email.com\",\"xp\":219,\"streak\":0,\"credits\":100,\"streak_frozen\":false,\"boosted\":false,\"instruments\":[\"PianoI\"],\"icons\":[\"ProfileIcons/defaultIcon\"],\"active_icon\":\"ProfileIcons/defaultIcon\",\"active_instrument\":\"PianoI\"}",
"{\"username\":\"WifeAlchemy\",\"uID\":\"0f47acf8-a80e-4ed0-b27e-d4735da1a4da\",\"password\":\"password1\",\"email\":\"WifeAlchemy@email.com\",\"xp\":205,\"streak\":0,\"credits\":100,\"streak_frozen\":false,\"boosted\":false,\"instruments\":[\"PianoI\"],\"icons\":[\"ProfileIcons/defaultIcon\"],\"active_icon\":\"ProfileIcons/defaultIcon\",\"active_instrument\":\"PianoI\"}",
"{\"username\":\"RiseAgency\",\"uID\":\"1bacbf93-0058-4b7a-990d-113735a15717\",\"password\":\"password1\",\"email\":\"RiseAgency@email.com\",\"xp\":960,\"streak\":0,\"credits\":100,\"streak_frozen\":false,\"boosted\":false,\"instruments\":[\"PianoI\"],\"icons\":[\"ProfileIcons/defaultIcon\"],\"active_icon\":\"ProfileIcons/defaultIcon\",\"active_instrument\":\"PianoI\"}",
"{\"username\":\"LimeTango\",\"uID\":\"1ca6b7cc-49a4-4071-9273-313830c0f391\",\"password\":\"password1\",\"email\":\"LimeTango@email.com\",\"xp\":567,\"streak\":0,\"credits\":100,\"streak_frozen\":false,\"boosted\":false,\"instruments\":[\"PianoI\"],\"icons\":[\"ProfileIcons/defaultIcon\"],\"active_icon\":\"ProfileIcons/defaultIcon\",\"active_instrument\":\"PianoI\"}",
"{\"username\":\"FactNose\",\"uID\":\"1e57b27f-da61-4fe0-9701-1d5fb3823b95\",\"password\":\"password1\",\"email\":\"FactNose@email.com\",\"xp\":56,\"streak\":0,\"credits\":100,\"streak_frozen\":false,\"boosted\":false,\"instruments\":[\"PianoI\"],\"icons\":[\"ProfileIcons/defaultIcon\"],\"active_icon\":\"ProfileIcons/defaultIcon\",\"active_instrument\":\"PianoI\"}",
"{\"username\":\"PsychPile\",\"uID\":\"2de02f44-4a3b-44a1-a45c-e2dd11c32d89\",\"password\":\"password1\",\"email\":\"PsychPile@email.com\",\"xp\":625,\"streak\":0,\"credits\":100,\"streak_frozen\":false,\"boosted\":false,\"instruments\":[\"PianoI\"],\"icons\":[\"ProfileIcons/defaultIcon\"],\"active_icon\":\"ProfileIcons/defaultIcon\",\"active_instrument\":\"PianoI\"}",
"{\"username\":\"ToeBeetle\",\"uID\":\"3d6a7d16-1651-49f5-920a-e4468a081d4a\",\"password\":\"password1\",\"email\":\"ToeBeetle@email.com\",\"xp\":91,\"streak\":0,\"credits\":100,\"streak_frozen\":false,\"boosted\":false,\"instruments\":[\"PianoI\"],\"icons\":[\"ProfileIcons/defaultIcon\"],\"active_icon\":\"ProfileIcons/defaultIcon\",\"active_instrument\":\"PianoI\"}",
"{\"username\":\"PonyChain\",\"uID\":\"3e7d9bdf-5367-4a05-9c41-e3d1daa7cea3\",\"password\":\"password1\",\"email\":\"PonyChain@email.com\",\"xp\":851,\"streak\":0,\"credits\":100,\"streak_frozen\":false,\"boosted\":false,\"instruments\":[\"PianoI\"],\"icons\":[\"ProfileIcons/defaultIcon\"],\"active_icon\":\"ProfileIcons/defaultIcon\",\"active_instrument\":\"PianoI\"}",
"{\"username\":\"RedWalnut\",\"uID\":\"3eecd549-1b3a-477c-8919-e4fafffcbe97\",\"password\":\"password1\",\"email\":\"RedWalnut@email.com\",\"xp\":207,\"streak\":0,\"credits\":100,\"streak_frozen\":false,\"boosted\":false,\"instruments\":[\"PianoI\"],\"icons\":[\"ProfileIcons/defaultIcon\"],\"active_icon\":\"ProfileIcons/defaultIcon\",\"active_instrument\":\"PianoI\"}",
"{\"username\":\"ReinNet\",\"uID\":\"4c04e382-483d-4a42-9a3d-b52dacf3205a\",\"password\":\"password1\",\"email\":\"ReinNet@email.com\",\"xp\":848,\"streak\":0,\"credits\":100,\"streak_frozen\":false,\"boosted\":false,\"instruments\":[\"PianoI\"],\"icons\":[\"ProfileIcons/defaultIcon\"],\"active_icon\":\"ProfileIcons/defaultIcon\",\"active_instrument\":\"PianoI\"}",
"{\"username\":\"AbyssBalloon\",\"uID\":\"4dc165ec-1013-4e33-a006-d3d318182627\",\"password\":\"password1\",\"email\":\"AbyssBalloon@email.com\",\"xp\":880,\"streak\":0,\"credits\":100,\"streak_frozen\":false,\"boosted\":false,\"instruments\":[\"PianoI\"],\"icons\":[\"ProfileIcons/defaultIcon\"],\"active_icon\":\"ProfileIcons/defaultIcon\",\"active_instrument\":\"PianoI\"}",
"{\"username\":\"MountCloud\",\"uID\":\"4f93e5c0-7695-419c-aeda-f0e1d8d8a8d5\",\"password\":\"password1\",\"email\":\"MountCloud@email.com\",\"xp\":507,\"streak\":0,\"credits\":100,\"streak_frozen\":false,\"boosted\":false,\"instruments\":[\"PianoI\"],\"icons\":[\"ProfileIcons/defaultIcon\"],\"active_icon\":\"ProfileIcons/defaultIcon\",\"active_instrument\":\"PianoI\"}"};



            // clears list of users before re-loading them
            this.users = new List<User>();
            this.user_files = new Hashtable();
            this.user_names = new Hashtable();

            foreach (string file in this.usersList) 
            {
                User user = JsonUtility.FromJson<User>(file);
                this.users.Add(user);

                this.user_files.Add(user.get_uID(), (string) file);
                this.user_names.Add(user.get_username().ToLower(), user); // must store usernames as lowercase for login ignore case
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

    public User find_user(string name)
    {
        load_all_users2();

        name = name.ToLower(); // case insensitive username search

        if(user_names.ContainsKey(name))
        {
            return (User) user_names[name];
        }

        return null;
    }

}
