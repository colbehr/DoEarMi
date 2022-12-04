using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LessonPlayer : MonoBehaviour
{
    private static int octave_index = 12;
    private static float interval_time = 0.75f;

    public AudioSource soundPlayer;

    private DoEarMiMeta meta;
    private LoadAudioAsInstrument instrumentLoader;
    private List<AudioClip> instrument;

    private Dictionary<string, int> noteToIndex = new Dictionary<string, int>
    {
        { "C" , 0 },
        { "C#", 1 },
        { "D" , 2 },
        { "D#", 3 },
        { "E" , 4 },
        { "F" , 5 },
        { "F#", 6 },
        { "G" , 7 },
        { "G#", 8 },
        { "A" , 9 },
        { "A#", 10 },
        { "B" , 11 }
    };

    private int[] major_scale = { 0, 2, 4, 5, 7, 9, 11, 12 };   // steps as indices from scale root
    private int[] major_chord = { 0, 4, 7 };                    // steps as indices from chord root
    private int[] minor_chord = { 0, 3, 7 };                    // steps as indices from chord root
    private int[] aug_chord   = { 0, 4, 8 };                    // steps as indices from chord root
    private int[] dim_chord   = { 0, 3, 6 };                    // steps as indices from chord root

    void Start()
    {
        // get default from meta for scalability, avoid breaking if default changes
        meta = DoEarMiMeta.Instance();
        string instrumentName = meta.get_default_instrument();

        instrumentLoader = LoadAudioAsInstrument.Instance();
        instrument = instrumentLoader.get_instrument(instrumentName);
        Debug.Log(instrument.Count.ToString());
    }

    
    public List<int> get_note_indices(string[] notes)
    {
        List<int> indices = new List<int>();
        
        for(int i=0; i<notes.Length; i++)
        {
            indices.Add(noteToIndex[notes[i]]);
        }

        return indices;
    }

    public List<int> get_pitch_indices(string[] notes)
    {
        List<int> indices = new List<int>();
        
        for(int i=0; i<notes.Length; i++)
        {
            indices.Add(System.Array.IndexOf(major_scale, noteToIndex[notes[i]]));
        }

        return indices;
    }


    // BACKEND AUDIO PLAYERS //
    public IEnumerator play_note_by_name(string note, int octave, float wait_time)
    {
        yield return new WaitForSeconds(wait_time);
        soundPlayer.PlayOneShot(instrument[noteToIndex[note] + (octave*octave_index)], 1);
    }

    IEnumerator play_note_by_index(int index, int octave, float wait_time)
    {
        yield return new WaitForSeconds(wait_time);
        soundPlayer.PlayOneShot(instrument[index + (octave*octave_index)], 1);
    }

    public void play_major_scale(string rootNote, int octave, float wait_time)
    {
        int rootIndex = noteToIndex[rootNote];
        for (int i=0; i < major_scale.Length; i++)
        {
            StartCoroutine(play_note_by_index(rootIndex + major_scale[i], octave, wait_time*i));
        }
    }

    // END BACKEND AUDIO PLAYERS //


    // FRONT END AUDIO PLAYERS

    public void play_note_from_btn(string note)
    {
        StartCoroutine(play_note_by_name(note, 1, 0.0f));
    }

    // CHORDS //
    // for this, chords all have root C
    public void play_chord(int chordType)
    {
        int[] chord = major_chord; // defaults to major if chordType not foudn

        // get note indices by chord type
        switch(chordType)
        {
            case 1:
                chord = major_chord;
                break;
            case 2:
                chord = minor_chord;
                break;
            case 3:
                chord = aug_chord;
                break;
            case 4:
                chord = dim_chord;
                break;
        }

        soundPlayer.PlayOneShot(instrument[12 + chord[0]], 1);
        soundPlayer.PlayOneShot(instrument[12 + chord[1]], 1);
        soundPlayer.PlayOneShot(instrument[12 + chord[2]], 1); 
    }
    // END CHORDS //


    // HARMONIC INTERVALS //
    // these interval methods are for the lessons page, and as such always begin with root note C
    public void play_interval(int interIndex)
    {
        StartCoroutine(play_note_by_index(0, 1, 0.0f));
        StartCoroutine(play_note_by_index(interIndex, 1, interval_time));
    }
    public void play_interval_octave()
    {
        StartCoroutine(play_note_by_index(0, 1, 0.0f));
        StartCoroutine(play_note_by_index(0, 2, interval_time));
    }

    public void play_harmonic_interval(int interIndex)
    {
        soundPlayer.PlayOneShot(instrument[12], 1);
        soundPlayer.PlayOneShot(instrument[12+interIndex], 1);
    }
    // END HARMONIC INTERVALS //

    // END FRONT END AUDIO PLAYERS //

}
