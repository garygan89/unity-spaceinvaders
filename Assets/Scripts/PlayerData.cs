using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData {

	// Constructor
	public PlayerData (int TotalPlayerLives) {
		this.StartingTotalLives = TotalPlayerLives;
		this.CurrentLives = StartingTotalLives;
	}

	// Total # of lives that the player have when starting a game
	public int StartingTotalLives { get; set; }

	public int CurrentLives { get; set; }

	public int TotalScore { get; set; }

	public void ResetScore () {
		this.TotalScore = 0;
	}
		
	public void ResetLives() {
		this.CurrentLives = StartingTotalLives;
	}

	/**
	 * Initialize total score and player lives. Called when a game is newly started or restarted.
	 */
	public void ResetData() {
		ResetScore ();
		ResetLives ();
	}
}
