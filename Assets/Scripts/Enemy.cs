using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour 
{
	public float vel;

	private Transform myTransform;
	private Rigidbody myRigidbody;
	private Vector3 startPosition;

	void Start()
	{
		myTransform = transform;
		myRigidbody = rigidbody;

		startPosition = myTransform.position;

		StartCoroutine (DestroyMe (8f));
	}

	// Update is called once per frame
	void Update () 
	{
		myRigidbody.velocity = myTransform.forward * vel;
	}

	void OnCollisionEnter(Collision col)
	{
		if(col.gameObject.tag == "Player")
		{
			collider.enabled = false;
			GameController.chickensHit++;
			HUDController.Instance.RefreshPoints();
		}
	}

	IEnumerator DestroyMe(float waitTime)
	{
		yield return new WaitForSeconds(waitTime);

		if(collider.enabled)
		{
			GameController.chickensDodged++;
			HUDController.Instance.RefreshPoints();
		}

		Destroy (gameObject);
	}

	public void Flee()
	{
		myTransform.LookAt (startPosition);

		collider.enabled = false;
	}
}
