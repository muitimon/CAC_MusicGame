using UnityEngine;
using System.Collections;

public class BallTrail : MonoBehaviour {

	public float trailEnableSpeed = 6f;

	private TrailRenderer trailRenderer;

	private Rigidbody _rigidbody;

	void Start () {
		_rigidbody = GetComponent<Rigidbody> ();
        trailRenderer = GetComponent<TrailRenderer> ();
	}

	void Update () {
		//trailRenderer.enabled = (_rigidbody.velocity.magnitude > trailEnableSpeed);
		trailRenderer.enabled = true;
	}
}
