using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FormationController : MonoBehaviour {
	public GameObject enemyPrefab;
	public float width = 11f;
	public float height = 9.5f;
	public float speed = 4f;
	public float spawnDelay = 0.7f;

	private bool movingRight = true;
	private float xmin, xmax;

	void Start () {
		float distanceToCamera = transform.position.z - Camera.main.transform.position.z;
		Vector3 leftBoundary = Camera.main.ViewportToWorldPoint (new Vector3 (0, 0, distanceToCamera));
		Vector3 rightBoundary = Camera.main.ViewportToWorldPoint (new Vector3 (1, 0, distanceToCamera));
		xmin = leftBoundary.x;
		xmax = rightBoundary.x;

		SpawnAllEnemies ();
	}

	void Update () {
		FormationMovement ();
		if(AllMembersDead()){
			SpawnUntilFull ();
		}
	}

	void FormationMovement() {
		if (movingRight) {
			transform.position += Vector3.right * speed * Time.deltaTime;
		} else {
			transform.position += Vector3.left * speed * Time.deltaTime;
		}

		//Formation playspace check
		float rightEdgeOfFormation = transform.position.x + (0.5f * width);
		float leftEdgeOfFormation = transform.position.x - (0.5f * width);
		if (leftEdgeOfFormation <= xmin) {
			movingRight = true;
		} else if (rightEdgeOfFormation >= xmax) {
			movingRight = false;
		}
	}

	void SpawnAllEnemies() {
		foreach (Transform child in transform) {
			GameObject enemy = Instantiate (enemyPrefab, child.transform.position, Quaternion.identity) as GameObject;
			enemy.transform.parent = child;
		}		
	}

	void SpawnUntilFull() {
		Transform freePosition = NextFreePosition ();
		if (freePosition) {
			GameObject enemy = Instantiate (enemyPrefab, freePosition.position, Quaternion.identity) as GameObject;
			enemy.transform.parent = freePosition;
		}
		if(NextFreePosition()) {
			Invoke ("SpawnUntilFull", spawnDelay);			
		}
	}

	bool AllMembersDead(){
		foreach (Transform childPositionGameObject in transform) {
			if (childPositionGameObject.childCount > 0)
				return false;
		}
		return true;
	}

	Transform NextFreePosition(){
		foreach (Transform childPositionGameObject in transform) {
			if (childPositionGameObject.childCount == 0)
				return childPositionGameObject;
		}
		return null;
	}

	void OnDrawGizmos() {
		Gizmos.DrawWireCube (transform.position, new Vector3(width,height));
	}
}
