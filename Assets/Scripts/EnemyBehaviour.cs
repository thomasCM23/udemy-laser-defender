using UnityEngine;
using System.Collections;

public class EnemyBehaviour : MonoBehaviour {

	public float health;
	public float projectileSpeed;
	public float minRate;
	public float maxRate;
	public GameObject projectile;
	public int scoreVal = 150;
	private ScoreKeeper keeper;
	public AudioClip fireSound;
	public AudioClip DeathSound;
	float fireRate;

	void OnTriggerEnter2D (Collider2D other)
	{
		Projectile missle = other.GetComponent<Projectile> ();
		if (missle != null && !missle.isEnemyBeam) {
			health -= missle.GetDamage ();
			missle.Hit ();
			if (health <= 0) {
				GetComponent<AudioSource>().PlayOneShot(DeathSound);
				GetComponent<Renderer>().enabled = false;
				keeper.Score(scoreVal);
				Destroy(gameObject, DeathSound.length);

			}
		}
	}
	void Start()
	{
		fireRate = Random.Range(minRate, maxRate);
		keeper = GameObject.Find("Score").GetComponent<ScoreKeeper>();
	}
	void Update ()
	{
		Fire();
	}	
	void Fire ()
	{
		fireRate -= Time.deltaTime;
		if (fireRate <= 0) {
			GameObject beam = (GameObject) Instantiate(projectile, transform.position, Quaternion.identity);
			beam.GetComponent<Rigidbody2D>().velocity = Vector2.down * projectileSpeed;
			fireRate = Random.Range(minRate, maxRate);
			GetComponent<AudioSource>().PlayOneShot(fireSound);
		}
	}
}
