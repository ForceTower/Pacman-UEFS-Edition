using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Energizer : MonoBehaviour {
    private void OnTriggerEnter2D (Collider2D collision) {
        if (collision.tag == "Player") {
            GameManager.Instance.ScareGhosts ();
            Destroy (gameObject);
        }
    }
}
