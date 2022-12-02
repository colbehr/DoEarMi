using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LessonPlayer : MonoBehaviour
{
    public AudioSource soundPlayer;

    private DoEarMiMeta meta;
    private LoadAudioAsInstrument instrumentLoader;
    private List<AudioClip> instrument;

    void Start()
    {
        // get default from meta for scalability, avoid breaking if default changes
        meta = DoEarMiMeta.Instance();
        string instrumentName = meta.get_default_instrument();

        instrumentLoader = LoadAudioAsInstrument.Instance();
        instrument = instrumentLoader.get_instrument(instrumentName);
    }

    // CHORDS //
    public void c_major()
    {
        soundPlayer.PlayOneShot(instrument[0], 1);
        soundPlayer.PlayOneShot(instrument[4], 1);
        soundPlayer.PlayOneShot(instrument[7], 1); 
    }

    public void c_minor()
    {
        soundPlayer.PlayOneShot(instrument[0], 1);
        soundPlayer.PlayOneShot(instrument[3], 1);
        soundPlayer.PlayOneShot(instrument[7], 1); 
    }

    public void c_aug()
    {
        soundPlayer.PlayOneShot(instrument[0], 1);
        soundPlayer.PlayOneShot(instrument[4], 1);
        soundPlayer.PlayOneShot(instrument[8], 1); 
    }

    public void c_dim()
    {
        soundPlayer.PlayOneShot(instrument[0], 1);
        soundPlayer.PlayOneShot(instrument[3], 1);
        soundPlayer.PlayOneShot(instrument[6], 1); 
    }

    public void c_note()
    {
        soundPlayer.PlayOneShot(instrument[0], 1);
    }
    public void eb_note()
    {
        soundPlayer.PlayOneShot(instrument[3], 1);
    }
    public void e_note()
    {
        soundPlayer.PlayOneShot(instrument[4], 1);
    }
    public void gb_note()
    {
        soundPlayer.PlayOneShot(instrument[6], 1);
    }
    public void g_note()
    {
        soundPlayer.PlayOneShot(instrument[7], 1);
    }
    public void gs_note()
    {
        soundPlayer.PlayOneShot(instrument[8], 1);
    }
    // END CHORDS//


}
