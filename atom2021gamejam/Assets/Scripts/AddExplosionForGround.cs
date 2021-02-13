using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(Explodable))]
public class AddExplosionForGround : MonoBehaviour
{
	private Explodable _explodable;

	private PolygonCollider2D polygonCollider2D;
	void Start()
	{
        try
        {
			polygonCollider2D = GetComponent<PolygonCollider2D>();
			polygonCollider2D.isTrigger = true;

			_explodable = GetComponent<Explodable>();
			_explodable.explode();

			ExplosionForce ef = GameObject.FindObjectOfType<ExplosionForce>();
			ef.doExplosion(new Vector2(transform.position.x, transform.position.y + 1500f));

		}
		catch
        {

        }
	}


	/*public float minforce = 150f;
	public float maxforce = 200f;
	public float radius = 10f;

	private void Start()
	{
		foreach (Transform t in transform)
		{
			var rb = t.GetComponent<Rigidbody2D>();
			if (rb != null)
			{
				//AddExplosionForce(rb, 150f, Vector2.down, 10f);
				//rb.AddExplosionForce(Random.Range(minforce, maxforce), transform.position, radius, 0, ForceMode.Acceleration);
				rb.AddForce(Vector2.down, ForceMode2D.Impulse);
				rb.AddForceAtPosition(Vector2.down, Vector2.down, ForceMode2D.Impulse);
			}
		}
		//Destroy(this.gameObject, 5f);
	}*/
	
	/*private void AddExplosionForce(this Rigidbody2D rb, float explosionForce, Vector2 explosionPosition, float explosionRadius, float upwardsModifier = 0.0F, ForceMode2D mode = ForceMode2D.Force)
	{
		var explosionDir = rb.position - explosionPosition;
		var explosionDistance = explosionDir.magnitude;

		// Normalize without computing magnitude again
		if (upwardsModifier == 0)
			explosionDir /= explosionDistance;
		else
		{
			// From Rigidbody.AddExplosionForce doc:
			// If you pass a non-zero value for the upwardsModifier parameter, the direction
			// will be modified by subtracting that value from the Y component of the centre point.
			explosionDir.y += upwardsModifier;
			explosionDir.Normalize();
		}

		rb.AddForce(Mathf.Lerp(0, explosionForce, (1 - explosionDistance)) * explosionDir, mode);
	}*/
}
