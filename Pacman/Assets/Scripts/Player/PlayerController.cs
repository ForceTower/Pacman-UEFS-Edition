using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    private static PlayerController _instance;
    public static PlayerController Instance {
        get {
            return _instance;
        }
    }

    public static int killingSpree = 0;

    public float speed = 0.4f;
    public Vector2 destination = Vector2.zero;
    private new Rigidbody2D rigidbody;
    private Animator animator;

    private Vector2 startLocation;
    private bool playingDead = false;
    private bool alreadyDead = false;

    private void Awake () {
        _instance = this;
    }

    void Start () {
        rigidbody = GetComponent<Rigidbody2D> ();
        animator = GetComponent<Animator> ();
        destination = transform.position;
        startLocation = transform.position;
    }

    private void FixedUpdate () {
        switch(GameManager.Instance.gameState) {
            case GameManager.GameState.Game:
                InputAndMove ();
                break;
            case GameManager.GameState.Dead:
                DeadAnimation ();
                break;
        }
    }

    private void InputAndMove () {
        Vector2 move = Vector2.MoveTowards (transform.position, destination, speed);
        rigidbody.MovePosition (move);

        if (Vector2.Distance (destination, transform.position) < 0.00001f) {
            if (Input.GetKey (KeyCode.UpArrow) && ValidDirection (Vector3.up))
                destination = (Vector2)transform.position + Vector2.up;
            if (Input.GetKey (KeyCode.DownArrow) && ValidDirection (Vector3.down))
                destination = (Vector2)transform.position + Vector2.down;
            if (Input.GetKey (KeyCode.LeftArrow) && ValidDirection (Vector3.left))
                destination = (Vector2)transform.position + Vector2.left;
            if (Input.GetKey (KeyCode.RightArrow) && ValidDirection (Vector3.right))
                destination = (Vector2)transform.position + Vector2.right;
        }

        Vector2 direction = destination - (Vector2)transform.position;
        animator.SetFloat ("Horizontal", direction.x);
        animator.SetFloat ("Vertical", direction.y);
    }

    private bool ValidDirection(Vector2 direction) {
        Vector2 position = transform.position;
        RaycastHit2D hit = Physics2D.Linecast (position + direction, position);

        //We just hit ourself
        if (hit.collider == GetComponent<Collider2D> ()) {
            return true;
        }
        //We hit something else
        else {
            return false;
        }
    }

    private void DeadAnimation() {
        if (!playingDead && !alreadyDead)
            StartCoroutine (DieAndRestartScene ());
    }

    private IEnumerator DieAndRestartScene () {
        playingDead = true;
        alreadyDead = true;
        animator.SetTrigger ("Dead");
        yield return new WaitForSeconds (1);
        playingDead = false;

        if (GameManager.Lives <= 0) {
            print ("Show some screen! Game is Over");
        } else {
            GameManager.Instance.RestartLevel ();
        }
    }

    public void Reset () {
        transform.position = startLocation;
        destination = transform.position;
        playingDead = false;
        alreadyDead = false;
        animator.SetFloat ("Horizontal", 1);
        animator.SetFloat ("Vertical",   0);
    }

    public void UpdateScore () {
        killingSpree++;

        if (killingSpree > 4)
            killingSpree = 4;

        Debug.Log ("Player just killed a ghost and received: " + ((killingSpree - 1) * 200) + " points");
    }
}
