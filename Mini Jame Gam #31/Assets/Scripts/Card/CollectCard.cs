using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectCard : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag == "Magician" || other.gameObject.tag == "Rabbit") {
            CollectedCard();
        }
    }

    private void CollectedCard() {
        // go to next level
        Destroy(gameObject);
    }

}
