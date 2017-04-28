using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyStatus : Status{
	
	public Slider healthBar;
	
    public override void onStart() {
		health = maxHealth;
		healthBar.value = health / maxHealth;
    }
	
	 public override float getHealth() {
        return health;
    }

    public override void hurt(float damage, int type) {
        health = Mathf.Max(0f, health - damage);
		if(health == 0)
			dead();
		healthBar.value = health / maxHealth;
    }
	
	private void dead() {
		/* Add points to player... */
	}
	
	
	
	
}
