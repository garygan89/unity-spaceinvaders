using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour{

    // movement range
    public float rangeH = 5;
    public float rangeV = 1;

    // speed
    public float speed = 2;

    // material for dead enemies
    public Material deadMaterial;

    // direction
    int direction = 1;

    // accumulated movement
    float accMovement = 0;

	// movement delay
	float movementDelayMillis = 1000;

	private int frameMovementDelay;

    // available states
    enum State { MovingHorizontally, MovingVertically, Dead};

    // keep track of the current state
    State currState;

    // Game Manager
    GameManager gm;

    // Enemy Manager
    EnemyManager em;

	// Frame counter
	int frameCounter;

	// Use this for initialization
	void Start () {
        // initial state
        currState = State.MovingHorizontally;

        // game manager
        gm = GameObject.FindObjectOfType<GameManager>();

        // log error if it wasn't found
        if (gm == null)
        {
            Debug.LogError("there needs to be an GameManager in the scene");
        }

        // enemy manager
        em = GameObject.FindObjectOfType<EnemyManager>();

        // log error if it wasn't found
        if (em == null)
        {
            Debug.LogError("there needs to be an EnemyManager in the scene");
        }

		frameCounter = 0;
    }
	
	// Update is called once per frame
	void Update () {
        // nothing happens if the enemy is dead
        if (currState == State.Dead) return;

//		frameCounter++;
//		if (frameCounter % 60 != 0)
//			return;

        // calculate movement  v = d / t --> d = v * t
        float movement = speed * Time.deltaTime;

        // update accumulate movement
        accMovement += movement;

        // are we moving horizontally?
        if (currState == State.MovingHorizontally)
        {
            // if yes, then transition to moving vertically
            if(accMovement >= rangeH)
            {
                // transition to moving vertically
                currState = State.MovingVertically;

                // reverse direction (for horizontal movement)
                direction *= -1;

                // reset acc movement
                accMovement = 0;
            }
            // if not, move the invader horizontally
            else
            {
                transform.position += transform.forward * movement * direction;
            }
        }
        // this is, if we are moving vertically
        else
        {
            // if yes, then transition to moving horizontally
            if (accMovement >= rangeV)
            {
                // transition to moving horiz
                currState = State.MovingHorizontally;

                // reset acc movement
                accMovement = 0;
            }
            // if not, move the invader vertically
            else
            {
                transform.position += Vector3.down * movement;
            }
        }

		// play moving sound
		em.audioSource.PlayOneShot (em.soundMoving, 0.05f);
	}

    // is called when the enemy is shot
    public void KillEnemy()
    {
        // nothing will happen if already dead
        if (currState == State.Dead) return;

        // set the state to dead
        currState = State.Dead;

		// ------------------------------------ MODIFIED CODE STARTS ------------------------------------------------------------------
		/**
		 * Following code will:
		 * 1. Change 'hit' model to dark gray
		 * 2. Drop the object to the ground (might collide with other model if they are on its dropping path
		 */

		em.audioSource.PlayOneShot (em.soundKilled, 1.0f);

		// Change material color to dark gray
		Transform[] models = gameObject.GetComponentsInChildren<Transform> ();
		foreach (Transform t in models) {
			if (t.name.Contains ("Invader Model")) { // we are only interested to modify the material of "Invader Model"
				// has to reassign modified materials back to the game object since t.GetComponent<MeshRenderer> ().materials
				// only returns a copy of the material array, (not by ref)
				Material[] m = t.GetComponent<MeshRenderer> ().materials;
				m [0] = deadMaterial;
				t.GetComponent<MeshRenderer> ().materials = m;
			}
		}

		// Drop the object, will collide with other upon impact
		gameObject.GetComponent<Rigidbody> ().isKinematic = false;

		// ------------------------------------ MODIFIED CODE ENDS ------------------------------------------------------------------

        // decrease number of enemies
        em.numEnemies--;
        
        // check winning condition
        gm.HandleEnemyDead();
    }

    void OnTriggerEnter(Collider other)
    {
        // nothing will happen if already dead
        if (currState == State.Dead) return;

        //check if the enemy hit the player
        if (other.CompareTag("Player Body"))
        {
			Debug.Log ("Hit player body!");

            gm.HandleEnemyCollision();
        }

        //check if the enemy reached the floor
        else if (other.CompareTag("Ground"))
        {
			Debug.Log ("Hit Ground!");

            gm.HandleEnemyCollision();
        }
    }


}
