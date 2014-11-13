using UnityEngine;
using System.Collections;

public class Cachorro : Pet 
{
	public float cooldown = 5f;
	private float cooldownCounter;
	private bool canAttack = true;
	private TextMesh textMesh;

	protected override void Start ()
	{
		base.Start ();

		textMesh = transform.FindChild ("cooldown").GetComponent<TextMesh> ();
		textMesh.text = "";

		textMesh.transform.LookAt (Camera.main.transform);
	}

	void OnTriggerEnter(Collider col)
	{
		if(LayerMask.LayerToName(col.gameObject.layer) == "Enemies" && canAttack)
		{
			canAttack = false;

			col.gameObject.GetComponent<Enemy>().Flee();

			cooldownCounter = cooldown;

			StartCoroutine(RunCooldown(cooldown));
		}
	}

	override protected void Update()
	{
		base.Update ();

		this.textMesh.transform.forward = Camera.mainCamera.transform.forward;

		if(!canAttack)
		{
			cooldownCounter -= Time.deltaTime;

			int seconds = (int)cooldownCounter;
			int deciSeconds = (int)((cooldownCounter - Mathf.Floor(cooldownCounter)) * 10f);

			textMesh.text = string.Format("{0:0}.{1:0}", seconds, deciSeconds);
		}


	}

	IEnumerator RunCooldown(float waitTime)
	{
		yield return new WaitForSeconds(waitTime);

		canAttack = true;

		textMesh.text = "";
	}
}
