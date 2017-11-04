using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour {
	public float health = 150;
	public GameObject projectile;
	public float projectileSpeed = 5f;
	public float shotsPerSeconds = 0.5f;
	public int scoreValue = 150;
	public AudioClip fireSound;
	public AudioClip deathSound;

	private ScoreKeeper scoreKeeper;

	void Start() {
		scoreKeeper = (ScoreKeeper)FindObjectOfType (typeof(ScoreKeeper));
	}
		
	void Update(){
		float probablity = Time.deltaTime * shotsPerSeconds;
		if (Random.value < probablity) {
			FireBeam ();
		}
	}

	void OnTriggerEnter2D(Collider2D collider){
		Projectile missile = collider.gameObject.GetComponent<Projectile> ();
		if (missile) {
			health -= missile.GetDamage ();
			missile.Hit ();	
			if (health <= 0) {
				Destroy (gameObject);
				scoreKeeper.Score(scoreValue);
				AudioSource.PlayClipAtPoint (deathSound, transform.position);
			}				
		}
	}

	void FireBeam(){
		Vector3 startPosition = transform.position + new Vector3 (0, -0.5f, 0);
		GameObject beam = Instantiate (projectile, startPosition, Quaternion.identity) as GameObject;
		beam.GetComponent<Rigidbody2D> ().velocity = new Vector3 (0, -projectileSpeed, 0);
		AudioSource.PlayClipAtPoint (fireSound, transform.position);
	}
}
