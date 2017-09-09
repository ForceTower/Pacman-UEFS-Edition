using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pacdot : MonoBehaviour {

    private void OnTriggerEnter2D (Collider2D collision) {
        if (collision.tag == "Player") {
            GameManager.Score += 10;
            GameObject[] pacdots = GameObject.FindGameObjectsWithTag ("Pacdot");

            Destroy (gameObject);

            if (pacdots.Length == 1) {
                GameManager.Instance.Win ();
            }
        }
    }
}
