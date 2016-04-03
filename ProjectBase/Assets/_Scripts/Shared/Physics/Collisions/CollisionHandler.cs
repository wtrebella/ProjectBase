﻿using UnityEngine;
using System.Collections;
using UnityEngine.Serialization;

public class CollisionHandler : MonoBehaviour {
	[FormerlySerializedAs("layer")]	
	public LayerMask layerMask;

	protected virtual void BaseAwake() {

	}

	public bool ObjectIsInLayer(GameObject obj) {
		return WhitTools.IsInLayer(obj, layerMask);
	}

	public bool HasNoLayer() {
		return layerMask.value == 0;
	}

	private void Awake() {
		BaseAwake();
	}
	
	public virtual void HandleCollisionEnter(Rigidbody2D rigid, Collision2D collision) {

	}

	public virtual void HandleCollisionStay(Rigidbody2D rigid, Collision2D collision) {

	}

	public virtual void HandleCollisionExit(Rigidbody2D rigid, Collision2D collision) {

	}
		
	public virtual void HandleTriggerEnter(Rigidbody2D rigid, Collider2D collider) {

	}

	public virtual void HandleTriggerStay(Rigidbody2D rigid, Collider2D collider) {

	}

	public virtual void HandleTriggerExit(Rigidbody2D rigid, Collider2D collider) {

	}

	protected float GetCollisionAngle(Rigidbody2D rigid, Collision2D collision) {
		Vector3 normal = collision.contacts[0].normal;
		Vector3 vel = rigid.velocity;
		float angle = 90 - Vector3.Angle(vel, -normal);
		return angle;
	}

	protected float GetCollisionSpeed(Collision2D collision) {
		return collision.relativeVelocity.magnitude;
	}

	protected void ShakeScreen(Rigidbody2D rigid, Collision2D collision) {
		float speed = GetCollisionSpeed(collision);
		float angle = GetCollisionAngle(rigid, collision);
		ScreenShaker.instance.CollisionShake(speed, angle);
	}
}
