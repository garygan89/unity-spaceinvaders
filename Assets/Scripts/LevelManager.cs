using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager {

	/**
	 * Set your custom level details here.
	 * E.g. 
	 * enemies in x,y,z pos
	 * enemies moving speed
	 * enemies separation
	 * randomize movement
	 */
	public static LevelInfo GetLevelInfo(LEVEL level) {
		LevelInfo lvlInfo = new LevelInfo ();
		switch (level) {
		case LEVEL.LEVEL_1:
			lvlInfo.enemyInXPos = 2;
			lvlInfo.enemyInYPos = 2;
			lvlInfo.enemyInZPos = 2;
			lvlInfo.enemyMovingSpeedFactor = 1;
			lvlInfo.enemySeparationSpacing = 3f;
			break;

		case LEVEL.LEVEL_2:
			lvlInfo.enemyInXPos = 3;
			lvlInfo.enemyInYPos = 2;
			lvlInfo.enemyInZPos = 2;			
			lvlInfo.enemyMovingSpeedFactor = 1.5f; // 1.5x the speed
			lvlInfo.enemySeparationSpacing = 3.5f;
			break;

		case LEVEL.LEVEL_3:
			lvlInfo.enemyInXPos = 4;
			lvlInfo.enemyInYPos = 2;
			lvlInfo.enemyInZPos = 2;		
			lvlInfo.enemyMovingSpeedFactor = 2; // 2x the moving speed
			lvlInfo.enemySeparationSpacing = 4f;
			break;

		default:
			lvlInfo.enemyInXPos = 5;
			lvlInfo.enemyInYPos = 5;
			lvlInfo.enemyInZPos = 5;
			lvlInfo.enemyMovingSpeedFactor = 1;
			lvlInfo.enemySeparationSpacing = 3f;
			break;
		}

		return lvlInfo;
	}

	public class LevelInfo {
		public int enemyInXPos { get; set; }
		public int enemyInYPos { get; set; }
		public int enemyInZPos { get; set; }
		public float enemyMovingSpeedFactor { get; set;}
		public float enemySeparationSpacing {get;set;} 
	}

	public enum LEVEL {
		LEVEL_1,
		LEVEL_2,
		LEVEL_3,
		LEVEL_NULL
	}

	// Simplest way to get the next level
	public static LEVEL GetNextLevel(LEVEL level) {
		switch (level) {
		case LEVEL.LEVEL_1:
			return LEVEL.LEVEL_2;

		case LEVEL.LEVEL_2:
			return LEVEL.LEVEL_3;

		case LEVEL.LEVEL_3:
			return LEVEL.LEVEL_NULL;

		 default:
			return LEVEL.LEVEL_NULL;

		}
	}



}
