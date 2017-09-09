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

    public float speed = 0.4f;
    public Vector2 destination = Vector2.zero;
    private new Rigidbody2D rigidbody;
    private Animator animator;

    private void Awake () {
        _instance = this;
    }

    void Start () {
        rigidbody = GetComponent<Rigidbody2D> ();
        animator = GetComponent<Animator> ();
        destination = transform.position;
    }

    private void FixedUpdate () {
        Vector2 move = Vector2.MoveTowards (transform.position, destination, speed);
        rigidbody.MovePosition (move);

        print (Vector2.Distance (destination, transform.position));

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
        animator.SetFloat ("Vertical",   direction.y);
    }

    private bool ValidDirection(Vector2 direction) {
        Vector2 position = transform.position;
        RaycastHit2D hit = Physics2D.Linecast (position + direction, position);

        //We just hit ourself
        if (hit.collider == GetComponent<Collider2D> ()) {
            print ("Valid");
            return true;
        }
        //We hit something else
        else {
            print ("Invalid");
            return false;
        }
    }
}
