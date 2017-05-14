using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Status component for player character
/// </summary>
public class PlayerStatus : Status
{
	protected int score; 
	
    public override void onStart() {
		health = maxHealth;
		score = 0;
    }
	
	public int getScore() {
		return score;
	}
	
	/* ?? */
	public void addScore(int added){
		score += added;
	}

    public override float getHealth() {
        return health;
    }

	public float getMaxHealth() {
		return maxHealth;
	}
	
    public override void hurt(float damage, int type) {
        health = Mathf.Max(0f, health - damage);
    }
}
