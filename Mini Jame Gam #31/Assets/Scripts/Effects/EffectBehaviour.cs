using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectBehaviour : MonoBehaviour
{
    [SerializeField] private float despawnTime;
    [SerializeField] private AudioClip effectSpawnSound;
    [SerializeField] private float effectVol;

    private void Start() {
        SoundManager.current.PlaySFX(effectSpawnSound, effectVol);
    }
    
    private void Update() {
        despawnTime -= Time.deltaTime;

        if(despawnTime < 0f)
            Destroy(gameObject);
    }
}
