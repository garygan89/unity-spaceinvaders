using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    // graffiti
    public Text uiText;

    //states
    enum State { NotStarted, Playing, TransitionLevel, Restart, GameOver, WonGame }

    // current state
    State currState;

    // Enemy Manager
    EnemyManager enemyManager;

	// UI Manager
	UIManager uiManager;

	// Player Data
	// TODO: Use List for multiplayers
	PlayerData player1;

	// Player total lives
	// a live is deducted when an alien 'touched' the player or hit the ground
	public int totalPlayerLives = 3;

	// Starting level
	public LevelManager.LEVEL startingLevel = LevelManager.LEVEL.LEVEL_1;

	public LevelManager.LEVEL currentLevel;

	// whether to clear all the dropped aliens before loading the next level
	// probably best to set to true if the levels are having too much 'dead' aliens to prevent pilling up
	public bool removeAllEnemiesUponLevelTransition = false; 

	// Panels
	GameObject gameOverPanel;
	GameObject controlHintPanel;

	// Audio source
	// sound when there is enemy on screen that is moving
	AudioSource audioEnemyMovement;


	// Use this for initialization
	void Start () {
        // start as not playing
        currState = State.NotStarted;

        // refresh UI
        RefreshUI();

        // find the enemy manager
        enemyManager = GameObject.FindObjectOfType<EnemyManager>();

		// find the ui manager
		uiManager = GameObject.FindObjectOfType<UIManager>();

        // log error if it wasn't found
        if(enemyManager == null)
        {
            Debug.LogError("there needs to be an EnemyManager in the scene");
        }

		if(uiManager == null)
		{
			Debug.LogError("there needs to be an UIManager in the scene");
		}

		// data container that stores player progress
		player1 = new PlayerData(totalPlayerLives);

		// panel that shows when game over
		gameOverPanel = GameObject.Find("FPS_UI_Canvas").transform.Find("Panel_GameOver").gameObject;

		// panel that shows control
		controlHintPanel = GameObject.Find("FPS_UI_Canvas").transform.Find("Panel_ControlHint").gameObject;

		audioEnemyMovement = GetComponent<AudioSource> ();
    }

	void Update() {
		if (Input.GetKeyDown (KeyCode.Return))
			this.InitGame (this.startingLevel);
		else if (Input.GetKeyDown (KeyCode.F5))
			this.ResetGame ();
		else if (Input.GetKeyDown (KeyCode.F1))
			this.ShowPlayerControlPanel ();
	}

	void ShowPlayerControlPanel() {
		controlHintPanel.SetActive (!controlHintPanel.activeSelf); // toggle between active and inactive
	}

    void RefreshUI()
    {
        // act according to the state
        switch(currState)
        {
            case State.NotStarted:
                uiText.text = "Shoot here to begin";
                break;

		case State.Playing:
			uiText.text = "Enemies left: " + enemyManager.numEnemies;
			uiManager.updateEnemyRemains (enemyManager.numEnemies);
			uiManager.updateTotalScore (player1.TotalScore);
			uiManager.updateLivesRemaining (player1.CurrentLives);
            break;

		case State.GameOver:
			uiText.text = "Game Over! Shoot here";
			Debug.Log ("Game Over, You LOSE!");
			uiManager.updateEnemyRemains (enemyManager.numEnemies);
			uiManager.updateTotalScore (player1.TotalScore);
			uiManager.updateLivesRemaining (player1.CurrentLives);
			gameOverPanel.SetActive (true);
			gameOverPanel.GetComponentInChildren<Text>().text = "Game Over!\nYou LOSE!";
            break;

		case State.WonGame:
			Debug.Log ("Game Over, You WIN!");
            uiText.text = "YOU WON! Shoot here";
			gameOverPanel.SetActive (true);
			gameOverPanel.GetComponentInChildren<Text>().text = "Congratulations!\nYou WIN!";
            break;
        }

       
    }

	public void InitGame(LevelManager.LEVEL playingLevel)
    {
        //don't initiate the game if the game is already running!
        if (currState == State.Playing) return;

        // set the state
        currState = State.Playing;

		audioEnemyMovement.Play ();

		// get level info
		LevelManager.LevelInfo lvlInfo = LevelManager.GetLevelInfo(playingLevel);

		// set current level to starting level
		currentLevel = playingLevel;

        // create enemy wave
		enemyManager.CreateEnemyWave(lvlInfo.enemyInXPos, lvlInfo.enemyInYPos, lvlInfo.enemyInZPos, lvlInfo.enemyMovingSpeedFactor, lvlInfo.enemySeparationSpacing);

		// update player lives in ui
		uiManager.updateLivesRemaining(player1.StartingTotalLives);

		// update current level in ui
		uiManager.updateLevelNumber((int)playingLevel+1);

        // show text on the graffiti
        RefreshUI();
    }

    // game over
    public void GameOver()
    {
        // do nothing if we were already on game over
        if (currState == State.GameOver) return;

        // set the state to game over
        currState = State.GameOver;

		// stop playing enemy movement sound
		audioEnemyMovement.Stop();

        // show text on the graffiti
        RefreshUI();


        // remove all enemies
        enemyManager.KillAll();
    }

	// Deduct player lives and restart current Level (score reset to 0) when aliens hit either player or ground
	public void HandleEnemyCollision() {
		player1.CurrentLives--;

		if (player1.CurrentLives <= 0) {
			GameOver ();
		} else { // gives player chance to replay the level, however the total score will reset to 0
			RestartLevel(); 
		}

	}

	/**
	 * Restart a level with player score reset to 0 and removes all enemies
	 */
	public void RestartLevel() {
		player1.ResetScore (); // re-initialize score

		// remove all enemies
		enemyManager.KillAll();

		// set game state to restart
		currState = State.Restart;

		Debug.Log ("Restarting current level: " + currentLevel);
		this.InitGame (currentLevel);

		RefreshUI ();
	}
	
	// checks whether we've won, and if we did win, refresh UI
    public void HandleEnemyDead()
    {
        if (currState != State.Playing) return;

		// update player total score, accumulative/carry over across levels
		// TODO: scoring multiplier per level. E.g. Lv1, 1pt per enemy, Lv2, 2 pt per enemy, etc...
		this.player1.TotalScore++;

        RefreshUI();

        // have we won the game?
        if(enemyManager.numEnemies <= 0)
        {
			audioEnemyMovement.Stop (); // stop playing the enemy movement audio

			// get the next level
			currentLevel = LevelManager.GetNextLevel (currentLevel);

			// is this the last level?
			if (currentLevel == LevelManager.LEVEL.LEVEL_NULL) { 
				// this is the last level, display congratulations message and add to leaderboard
				Debug.Log ("YES YOU WIN!");

				// set the state of the game
				currState = State.WonGame;
			} else {
				currState = State.TransitionLevel;

				Debug.Log ("Loading next level: " + currentLevel);
				this.InitGame (currentLevel);

				// remove all enemies
				if (removeAllEnemiesUponLevelTransition)
					enemyManager.KillAll();
			}

            // show text on the graffiti
            RefreshUI();        
        }
    }

	/**
	 * Reset the whole game
	 */
	public void ResetGame() {
		Debug.Log ("Game reset!");
		currState = State.NotStarted;

		// reset player score and total lives remains
		player1.ResetData ();

		// clear all aliens on the field
		enemyManager.KillAll();

		// set starting level
		currentLevel = startingLevel;

		// hide any existing panel
		gameOverPanel.SetActive (false);

		// initialize game
		this.InitGame(currentLevel);
	}

}
