using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour {

	public GameObject EnemyPrefab;

	public float width = 10f;
	public float height = 5f;
	public float spawnDelay = .5f;
	public float moveSpeed; 
	float xmin = -5f;
	float xmax = 5f;
	public float padding = 1f;
	bool isMin = false;
	// Use this for initialization
	void Start ()
	{
		float distance = transform.position.z - Camera.main.transform.position.z;
		Vector3 leftmost = Camera.main.ViewportToWorldPoint(new Vector3(0f,0f,distance));
		Vector3 rightmost = Camera.main.ViewportToWorldPoint(new Vector3(1f,.5f,distance));
		xmin = leftmost.x + padding;
		xmax = rightmost.x - padding;
		
		SpawnUntilFull();
	
	}
	void Spawn()
	{
		foreach (Transform child in transform) {
			GameObject enemy = (GameObject)Instantiate (EnemyPrefab, child.position, Quaternion.identity);
			enemy.transform.parent = child;
		}
	}
	void SpawnUntilFull ()
	{
		Transform freePostion = NextFreePosition ();
		if (freePostion != null) {
			GameObject enemy = (GameObject)Instantiate (EnemyPrefab, freePostion.position, Quaternion.identity);
			enemy.transform.parent = freePostion;
		}
		if (NextFreePosition ()) {
			Invoke ("SpawnUntilFull", spawnDelay);
		}
	}

	void OnDrawGizmos()
	{
		Gizmos.DrawWireCube(transform.position, new Vector3(width, height, 1f));
	}
	
	// Update is called once per frame
	void Update ()
	{
		MoveEnemy ();
		if (AllMemberDead ()) {
			SpawnUntilFull();
		}

	
	}
	Transform NextFreePosition()
	{
		foreach (Transform child in transform) {
			if (child.transform.childCount == 0) {
				return child;
			}
		}
		return null;

	}

	    
	bool AllMemberDead ()
	{
		foreach (Transform child in transform) {
			if (child.transform.childCount > 0) {
				return false;
			}
		}
		return true;
	}
	void MoveEnemy ()
	{
		if (isMin) {
			transform.position += Vector3.right * moveSpeed * Time.deltaTime;
		} else {
			transform.position += Vector3.left * moveSpeed * Time.deltaTime;
		}

		transform.position = new Vector3 (Mathf.Clamp (transform.position.x, xmin, xmax), transform.position.y, transform.position.z);

		if (transform.position.x <= xmin) {
			isMin = true;
		} else if (transform.position.x >= xmax) {
			isMin = false;
		}
	}	
}
