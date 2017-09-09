using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostController : MonoBehaviour {
    public enum State {
        Walking, Following, Running, Waiting
    }

    public float speed;

    public float distanceToFollow;
    public State currentState;

    public Vector2 direction;
    public Vector2 destination;

    private new Rigidbody2D rigidbody;
    private Animator animator;

    public LayerMask mazeLayer;

    public Vector2 diff;
    private Vector2 startLocation;

    private List<Vector2> ways;

    // Use this for initialization
    void Start () {
        destination = transform.position;
        rigidbody = GetComponent<Rigidbody2D> ();
        animator = GetComponent<Animator> ();
        startLocation = transform.position;
        SelectNextDirection ();
    }

    private void FixedUpdate () {
        switch(GameManager.Instance.gameState) {
            case GameManager.GameState.Game:
                MoveGhost ();
                break;
        }
    }

    private void MoveGhost() {
        Vector2 move = Vector2.MoveTowards (transform.position, destination, speed);
        rigidbody.MovePosition (move);

        if (Vector2.Distance (transform.position, destination) == 0.0f) {
            FollowUpdate ();

            Vector2 pDirection = direction;

            if (SelectNextDirection()) {
                destination = (Vector2)transform.position + direction;
                return;
            } else {
                direction = pDirection;
            }

            if (ValidDirection (direction)) {
                destination = (Vector2)transform.position + direction;
            }
            else {
                SelectNextDirection ();
                destination = (Vector2)transform.position + direction;
            }
        }

        Vector2 dir = destination - (Vector2)transform.position;
        animator.SetFloat ("Horizontal", dir.x);
        animator.SetFloat ("Vertical", dir.y);
    }

    private void FollowUpdate () {
        if (currentState != State.Running) {
            Vector2 playerPosition = PlayerController.Instance.transform.position;
            Vector2 myPosition = transform.position;

            if (Vector2.Distance (playerPosition, myPosition) <= distanceToFollow) {
                currentState = State.Following;
            }
            else {
                currentState = State.Walking;
            }
        }
    }

    private bool SelectNextDirection() {
        List<Vector2> avaliableDirections = CheckDirections ();

        if (currentState == State.Walking) {
            int selected = UnityEngine.Random.Range (0, avaliableDirections.Count);
            direction = avaliableDirections[selected];
        } else if (currentState == State.Following) {
            FollowLogic (avaliableDirections);
        } else if (currentState == State.Running) {
            FollowLogic (avaliableDirections);
            if (avaliableDirections.Contains (direction * -1))
                direction = direction * -1;
            else if (avaliableDirections.Count != 1) {
                avaliableDirections.Remove (direction);
                direction = avaliableDirections[0];
            }

        }

        if (ways != null) {
            foreach (Vector2 v2 in avaliableDirections) {
                if (!ways.Contains (v2)) {
                    ways = avaliableDirections;
                    return true;
                }
            }
        }
        ways = avaliableDirections;

        return false;
    }

    private void FollowLogic (List<Vector2> avaliableDirections) {
        Vector2 playerPosition = PlayerController.Instance.transform.position;
        Vector2 myPosition = transform.position;

        Vector2 difference = playerPosition - myPosition;
        diff = difference;

        if (Math.Abs(difference.x) > Math.Abs(difference.y)) {
            if (!TryX (difference, avaliableDirections))
                TryY (difference, avaliableDirections);
        } else {
            if (!TryY (difference, avaliableDirections))
                TryX (difference, avaliableDirections);
        }

        
    }

    private bool TryX (Vector2 difference, List<Vector2> directions) {
        //Needs to go right!
        if (difference.x > 0) {
            if (directions.Contains (Vector2.right)) {
                direction = Vector2.right;
                return true;
            } else {
                direction = directions[0];
                return false;
            }
        }
        //Needs to go left
        else {
            if (directions.Contains(Vector2.left)) {
                direction = Vector2.left;
                return true;
            }
            direction = directions[0];
            return false;
        }
    }

    private bool TryY (Vector2 difference, List<Vector2> directions) {
        //needs to go up
        if (difference.y > 0) {
            if (directions.Contains(Vector2.up)) {
                direction = Vector2.up;
                return true;
            } else {
                direction = directions[0];
                return false;
            }
        }
        //Needs to go down
        else {
            if (directions.Contains(Vector2.down)) {
                direction = Vector2.down;
                return true;
            }
            direction = directions[0];
            return false;
        }
    }

    private List<Vector2> CheckDirections () {
        List<Vector2> avaliableDirections = new List<Vector2> ();

        if (ValidDirection (Vector3.up))
            avaliableDirections.Add(Vector2.up);
        if (ValidDirection (Vector3.down))
            avaliableDirections.Add (Vector2.down);
        if (ValidDirection (Vector3.left))
            avaliableDirections.Add (Vector2.left);
        if (ValidDirection (Vector3.right))
            avaliableDirections.Add (Vector2.right);

        return avaliableDirections;
    }

    private bool ValidDirection (Vector2 direction) {
        Vector2 position = transform.position;
        RaycastHit2D hit = Physics2D.Linecast (position + direction, position, mazeLayer);

        if (hit/*.collider == GetComponent<Collider2D> ()*/) {
            return false;
        }

        else {
            return true;
        }
    }

    private void OnTriggerEnter2D (Collider2D collision) {
        if (collision.tag == "Player" && GameManager.Instance.gameState == GameManager.GameState.Game) {
            if (currentState == State.Running) {
                PlayerController.Instance.UpdateScore ();
                GhostKilled ();
            }
            else {
                GameManager.Instance.KillPlayer ();
            }
        }
    }

    private void GhostKilled () {
        print ("A ghost was killed, now it should chill");
        Calm ();
        Reset ();
    }

    public void Scare () {
        print ("Ghost is scared!");
    }

    public void Calm () {
        print ("Calm down ghost!");
    }

    public void Reset () {
        currentState = State.Waiting;

        transform.position = startLocation;
        destination = transform.position;
        animator.SetFloat ("Horizontal", 1);
        animator.SetFloat ("Vertical", 0);
    }
}