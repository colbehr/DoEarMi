using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LoadAudioAsInstrument
{   
    // Singleton
    private static LoadAudioAsInstrument instance = null;
    // Thread-safety locks for singleton and files
    private static readonly object instance_padlock = new object();
    
    // Directory in Resources folder containing all audio clip subfolders
    private static string FileDirectory = "AudioClips/";

    // Instance is null initializer
    private LoadAudioAsInstrument() {}

    // Entry point for DoEarMiMeta singleton
    public static LoadAudioAsInstrument Instance()
    {
        // double locking
        if (instance == null)
        {
            lock (instance_padlock)
            {
                if (instance == null)
                {
                    instance = new LoadAudioAsInstrument();
                }
            }
        }

        return instance;
    } 

    // All Instruments must be declared as elements of All_Instruments dictionary
    // key (string): instrument name
    // value (string): intermediate path to audioclips without specifying each note (must be .wav files)
    private Dictionary<string, string> All_Instruments = new Dictionary<string, string>
    {
        { "EPianoI", "EPianoI/" },
        { "EPianoII", "EPianoII/" },
        { "BassI", "BassI/" },
        { "GuitarI", "GuitarI/" }
    };


    // parameter string name must be a valid name from All_Instruments
    public List<AudioClip> get_instrument(string name)
    {
        List<AudioClip> instrument = new List<AudioClip>();

        // Ensure instrument is valid and listed
        if(!All_Instruments.TryGetValue(name, out string path))
        {
            Debug.Log("Trying to get an instrument not specified in Instruments.cs All_Instruments dictionary");
        }
        else
        {

            // Load all files from specified path into objects list 
            Object[] clips = Resources.LoadAll(FileDirectory + path, typeof(AudioClip));

            // Convert each object to AudioClip and add to instrument array
            for (int i = 0; i < clips.Length; i++)
            {
                instrument.Add((AudioClip) clips[i]);
            }
            // Sort clips by name
            instrument.Sort((x, y) => x.name.CompareTo(y.name));
        }

        return instrument;
    }


    // public InstrumentPathWrapper get_instrument_paths(string name)
    // {
    //     InstrumentPathWrapper paths = new InstrumentPathWrapper();
    //     paths.instrument_paths = new List<string>();

    //     // Ensure instrument is valid and listed
    //     if(!All_Instruments.TryGetValue(name, out string path))
    //     {
    //         Debug.Log("Trying to get an instrument not specified in Instruments.cs All_Instruments dictionary");
    //     }
    //     else
    //     {
    //         var directory = new DirectoryInfo(FileDirectory + path);
    //         paths.instrument_paths = directory.GetFiles();

    //         // // Load all files from specified path into objects list 
    //         // Object[] clips = Resources.LoadAll(FileDirectory + path, typeof(AudioClip));

    //         // // Convert each object to AudioClip and add to instrument array
    //         // for (int i = 0; i < clips.Length; i++)
    //         // {
    //         //     instrument.Add((AudioClip) clips[i]);
    //         // }
    //     }

    //     return paths;
    // } 
    

    // // start method used for testing, add MonoBehaviour to script to use
    // public void Start()
    // {
    //     source = gameObject.GetComponent<AudioSource>();

    //     if (source == null)
    //     {
    //         source = gameObject.AddComponent<AudioSource>();
    //     }

    //     List<AudioClip> instrument = get_instrument("EPianoI");
    // // only plays once, need to add wait before playing next            
    //     for(int i = 0; i < instrument.Count; i++)
    //     {
    //         source.clip = instrument[i];
    //         source.Play();
    //     }

    // }
    
}
