using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/**
 * Update the HUD info
 */
public class UIManager : MonoBehaviour {

	Text txtLevel;
	Text txtEnemyLeft;
	Text txtScore;
	Text txtlivesRemains;

	// Use this for initialization
	void Awake () {
		txtLevel = GameObject.Find ("txt_level").GetComponent<Text> ();
		txtEnemyLeft = GameObject.Find ("txt_enemyleft").GetComponent<Text> ();
		txtScore = GameObject.Find ("txt_score").GetComponent<Text> ();
		txtlivesRemains = GameObject.Find ("txt_livesremaining").GetComponent<Text> ();

	}
	
	public void updateTotalScore(int score) {
		this.txtScore.text = score.ToString ();
	}

	public void updateLevelNumber(int levelNum) {
		this.txtLevel.text = levelNum.ToString ();
	}

	public void updateLivesRemaining(int livesRemaining) {
		this.txtlivesRemains.text = livesRemaining.ToString ();
	}

	public void updateEnemyRemains(int enemyRemains) {
		this.txtEnemyLeft.text = enemyRemains.ToString ();
	}
}
