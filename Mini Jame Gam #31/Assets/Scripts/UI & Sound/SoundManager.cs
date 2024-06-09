using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [Header ("Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;
    private float curMusicVol;
    private float lowerMusicCountdown;


    //global reference to this script
    public static SoundManager current;
    private void Awake() {
        current = this; 
    }

    /** Stops old music and plays new music into music audio source **/
    public void PlayMusic(AudioClip musicClip, float volumePercent) {

        //don't restart music if already playing
        if(musicClip == musicSource.clip)
            return;
    
        // clear old music and load new music
        musicSource.Stop();
        musicSource.clip = musicClip;

        //volume
        curMusicVol = volumePercent;
        musicSource.volume = curMusicVol;
        
        // play music
        musicSource.Play();
    }

    /** Plays sfx clip in the sfx audio source **/
    public void PlaySFX(AudioClip sfxClip, float volumePercent) {
        sfxSource.PlayOneShot(sfxClip, volumePercent);
    }

    /** Lowers music volume, for transitioning between music **/
    public IEnumerator LowerMusicVolumeCoroutine(float lowerMusicTime) {

        // start a little less incase trying to lower music while playing new music clip
        lowerMusicCountdown = lowerMusicTime -= 0.1f;

        // while music is not silent
        while(lowerMusicCountdown > 0) {

            lowerMusicCountdown -= Time.deltaTime;

            if(lowerMusicCountdown <= 0) {
                musicSource.volume = 0f;
                yield break;
            }
            else {
                musicSource.volume = (lowerMusicCountdown/lowerMusicTime) * curMusicVol;
                yield return null;
            }
        }
    }

}
