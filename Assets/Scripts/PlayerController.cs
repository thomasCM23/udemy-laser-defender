using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour 
{

	public float moveSpeed;
	public GameObject projectile;
	public float projectileSpeed;
	public float fireRate;
	public float health;

	float xmin = -5f;
	float xmax = 5f;
	float ymin = -1f;
	float ymax = -4f;
	public float padding = 1f;

	public AudioClip fireSound;

	// Use this for initialization
	void Start () 
	{
		float distance = transform.position.z - Camera.main.transform.position.z;
		Vector3 leftmost = Camera.main.ViewportToWorldPoint(new Vector3(0f,0f,distance));
		Vector3 rightmost = Camera.main.ViewportToWorldPoint(new Vector3(1f,.5f,distance));
		xmin = leftmost.x + padding;
		xmax = rightmost.x - padding;
		ymin = leftmost.y + padding;
		ymax = rightmost.y - padding;
	}

	// Update is called once per frame
	void Update ()
	{
		ArrowControl ();

		if (Input.GetKeyDown (KeyCode.Space)) {
			InvokeRepeating ("Shoot", 0.000001f, fireRate);
		}
		if (Input.GetKeyUp (KeyCode.Space)) {
			CancelInvoke("Shoot");
		}
	
	}
	void ArrowControl()
	{
		float h = Input.GetAxis("Horizontal");
		float v = Input.GetAxis("Vertical");
		Vector3 speed = new Vector3(h, v, 0.0f)* moveSpeed * Time.deltaTime;
		transform.position += speed;

		//restrict play to game space
		Vector3 clamped = new Vector3(Mathf.Clamp(transform.position.x, xmin, xmax), Mathf.Clamp(transform.position.y, ymin, ymax), 0.0f);

		transform.position = clamped;	
	}
	void Shoot ()
	{
		GameObject beam = (GameObject) Instantiate(projectile, transform.position, Quaternion.identity);
		beam.GetComponent<Rigidbody2D>().velocity = Vector2.up * projectileSpeed;
		GetComponent<AudioSource>().PlayOneShot(fireSound);
		
	}
	void OnTriggerEnter2D (Collider2D other)
	{
		Projectile missle = other.GetComponent<Projectile> ();
		if (missle != null && missle.isEnemyBeam) {
			health -= missle.GetDamage ();
			missle.Hit ();
			if (health <= 0) {
				GameObject.FindObjectOfType<LevelManager>().LoadLevel("Lose");
				Destroy(gameObject);
			}
		}
	}
}
