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
    // END CHORDS//


    // NOTES //   # don't judge, I know.
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
    public void hc_note()
    {
        soundPlayer.PlayOneShot(instrument[12], 1);
    }
    // END NOTES //


    // HARMONIC INTERVALS //
    public void maj_sec()
    {
        soundPlayer.PlayOneShot(instrument[0], 1);
        soundPlayer.PlayOneShot(instrument[2], 1);
    }
    public void maj_thr()
    {
        soundPlayer.PlayOneShot(instrument[0], 1);
        soundPlayer.PlayOneShot(instrument[4], 1);
    }
    public void maj_six()
    {
        soundPlayer.PlayOneShot(instrument[0], 1);
        soundPlayer.PlayOneShot(instrument[9], 1);
    }
    public void maj_sev()
    {
        soundPlayer.PlayOneShot(instrument[0], 1);
        soundPlayer.PlayOneShot(instrument[11], 1);
    }
    public void min_sec()
    {
        soundPlayer.PlayOneShot(instrument[0], 1);
        soundPlayer.PlayOneShot(instrument[1], 1);
    }
    public void min_thr()
    {
        soundPlayer.PlayOneShot(instrument[0], 1);
        soundPlayer.PlayOneShot(instrument[3], 1);
    }
    public void min_six()
    {
        soundPlayer.PlayOneShot(instrument[0], 1);
        soundPlayer.PlayOneShot(instrument[8], 1);
    }
    public void min_sev()
    {
        soundPlayer.PlayOneShot(instrument[0], 1);
        soundPlayer.PlayOneShot(instrument[10], 1);
    }
    public void perf_uni()
    {
        soundPlayer.PlayOneShot(instrument[0], 1);
        soundPlayer.PlayOneShot(instrument[0], 1);
    }
    public void perf_four()
    {
        soundPlayer.PlayOneShot(instrument[0], 1);
        soundPlayer.PlayOneShot(instrument[5], 1);
    }
    public void perf_fift()
    {
        soundPlayer.PlayOneShot(instrument[0], 1);
        soundPlayer.PlayOneShot(instrument[7], 1);
    }
    public void perf_oct()
    {
        soundPlayer.PlayOneShot(instrument[0], 1);
        soundPlayer.PlayOneShot(instrument[12], 1);
    }
    // END HARMONIC INTERVALS //


}
