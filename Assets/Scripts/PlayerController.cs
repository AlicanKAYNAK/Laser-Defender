using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
	public float health = 250;
	public float speed = 15.0f;
	public float padding = 1.0f;
	public float projectileSpeed = 5f;
	public float firingRate = 0.2f;
	public GameObject projectile;
	public AudioClip fireSound;
	public AudioClip damageSound;

	private float xmin, xmax;

	void Start () {
		float distanceToCamera = transform.position.z - Camera.main.transform.position.z;
		Vector3 leftBoundary = Camera.main.ViewportToWorldPoint (new Vector3 (0, 0, distanceToCamera));
		Vector3 rightBoundary = Camera.main.ViewportToWorldPoint (new Vector3 (1, 0, distanceToCamera));
		xmin = leftBoundary.x + padding;
		xmax = rightBoundary.x - padding;
	}

	void Update () {
		if (Input.GetKeyDown(KeyCode.Space)) {
			InvokeRepeating ("FireBeam", 0.000001f, firingRate);
		}
		if (Input.GetKeyUp (KeyCode.Space)) {
			CancelInvoke ("FireBeam");
		}
		if (Input.GetKey (KeyCode.LeftArrow)) {
			//transform.position += new Vector3 (-speed * Time.deltaTime, 0, 0);
			transform.position += Vector3.left * speed * Time.deltaTime;
		}else if (Input.GetKey (KeyCode.RightArrow)) {
			//transform.position += new Vector3(speed * Time.deltaTime, 0, 0);
			transform.position += Vector3.right * speed * Time.deltaTime;
		}

		//Resctriction of Gamespoce
		float newX = Mathf.Clamp (transform.position.x, xmin, xmax);
		transform.position = new Vector3 (newX, transform.position.y, transform.position.z);
	}

	void FireBeam(){
		Vector3 startPosition = transform.position + new Vector3 (0, 0.5f, 0);
		GameObject beam = Instantiate (projectile, startPosition, Quaternion.identity) as GameObject;
		beam.GetComponent<Rigidbody2D> ().velocity = new Vector3 (0, projectileSpeed, 0);
		AudioSource.PlayClipAtPoint (fireSound, transform.position);
	}

	void OnTriggerEnter2D(Collider2D collider){
		Projectile missile = collider.gameObject.GetComponent<Projectile> ();
		if (missile) {
			health -= missile.GetDamage ();
			missile.Hit ();	
			AudioSource.PlayClipAtPoint (damageSound, transform.position);
			if (health <= 0) {
				Die ();
			}				
		}
	}

	void Die(){
		LevelManager manager = GameObject.Find ("LevelManager").GetComponent<LevelManager> ();
		manager.LoadLevel ("Win Screen");
		Destroy (gameObject);
	}
}
