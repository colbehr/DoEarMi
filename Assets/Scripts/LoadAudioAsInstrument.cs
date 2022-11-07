using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LoadAudioAsInstrument
{
    // All Instruments must be declared as elements of All_Instruments dictionary
    // key (string): instrument name
    // value (string): intermediate path to audioclips without specifying each note (must be .wav files)
    private Dictionary<string, string> All_Instruments = new Dictionary<string, string>
    {
        { "EPianoI", "EPianoI/" },
        { "BassI", "BassI/" },
        { "SynthI", "SynthI/" }
    };


    // parameter string name must be a valid name from All_Instruments
    public List<AudioClip> get_instrument(string name)
    {
        List<AudioClip> instrument = new List<AudioClip>();

        if(!All_Instruments.TryGetValue(name, out string path))
        {
            Debug.Log("Trying to get an instrument not specified in Instruments.cs All_Instruments dictionary");
        }
        else
        {

            // Load all files from specified path into objects list 
            Object[] clips = Resources.LoadAll("AudioClips/" + path, typeof(AudioClip));

            // Convert each object to AudioClip and add to instrument array
            for (int i = 0; i < clips.Length; i++)
            {
                instrument.Add((AudioClip) clips[i]);
            }
        }

        return instrument;
    }
    

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
