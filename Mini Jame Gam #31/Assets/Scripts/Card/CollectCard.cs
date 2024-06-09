using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectCard : MonoBehaviour
{
    [SerializeField] private AudioClip levelCompleteChime;
    [SerializeField] private float chimeVol;
    private LevelManager levelScript;

    private void Awake() {
        levelScript = transform.root.GetComponent<LevelManager>();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag == "Magician" || other.gameObject.tag == "Rabbit") {
            CollectedCard();
        }
    }

    private void CollectedCard() {
        // play victory sfx
        SoundManager.current.PlaySFX(levelCompleteChime, chimeVol);

        // go to next level
        levelScript.ChangeLevel(levelScript.CurLevel + 1);

        // deactivate card
        gameObject.SetActive(false);
    }

}
