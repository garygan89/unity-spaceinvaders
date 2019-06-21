using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    // Bullet velocity
    public float bulletSpeed = 10;

    // Gun
    public GameObject gun;

    // bullet prefab
    public GameObject bulletPrefab;

    // Game Manager
    GameManager gm;

	// Audio Source (from gun game object)
	AudioSource audioSource;

	// Bullet fire particle
	GameObject bulletFireParticle;

	// Per gun initial bullet spawn pos offset
	public float GUN_FWD_X_OFFSET = 0f;
	public float GUN_FWD_Y_OFFSET = 0f;
	public float GUN_FWD_Z_OFFSET = 0f;


    void Awake()
    {
        gm = GameObject.FindObjectOfType<GameManager>();

		audioSource = gun.GetComponent<AudioSource> ();

		bulletFireParticle = GameObject.Find ("WFX_MF FPS RIFLE1");
    }

    // Update is called once per frame
    void Update () {
        // Get user input
        HandleInput();
	}

    void HandleInput()
    {
        if(Input.GetButtonDown("Fire1"))
        {
            // spawn a new bullet
            GameObject newBullet = Instantiate(bulletPrefab);

            // pass the game manager
            newBullet.GetComponent<BulletController>().gm = gm;

			// position will be that of the gun
			 newBullet.transform.position = gun.transform.position;

            // get rigid body
            Rigidbody bulletRb = newBullet.GetComponent<Rigidbody>();

            // give the bullet velocity
            bulletRb.velocity = gun.transform.forward * bulletSpeed;

			// spawm fire particle
			//bulletFireParticle.SetActive(true);

			audioSource.Play ();
        }
    }
}
