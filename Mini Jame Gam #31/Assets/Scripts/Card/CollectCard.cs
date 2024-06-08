using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectCard : MonoBehaviour
{
    [Header ("References")]
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
        // play victory effects

        // go to next level
        levelScript.ChangeLevel(levelScript.CurLevel + 1);

        // deactivate card
        gameObject.SetActive(false);
    }

}
