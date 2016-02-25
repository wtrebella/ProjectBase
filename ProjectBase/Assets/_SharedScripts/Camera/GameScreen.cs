﻿using UnityEngine;
using System.Collections;

public class GameScreen : MonoBehaviour {
	public static GameScreen instance;

	public Camera cam {get; private set;}

	[SerializeField] private tk2dCameraAnchor lowerLeftAnchor;
	[SerializeField] private tk2dCameraAnchor upperRightAnchor;
	[SerializeField] private float onscreenMargin = 10;
	
	private void Awake() {
		instance = this;
		cam = GetComponent<Camera>();
	}

	public Vector2 center {
		get {
			Vector2 centerPoint = Vector2.zero;
			centerPoint.x = origin.x + width / 2f;
			centerPoint.y = origin.y + height / 2f;
			return centerPoint;
		}
	}

	public Rect worldRect {
		get {return new Rect(lowerLeft, size);}
	}

	public Vector2 GetRandomScreenPoint() {
		float x = Random.Range(0, Screen.width);
		float y = Random.Range(0, Screen.height);
		return new Vector2(x, y);
	}

	public Vector3 GetRandomWorldPoint(float z) {
		Vector2 screenPoint = GetRandomScreenPoint();
		Vector3 worldPoint = cam.ScreenToWorldPoint(new Vector3(screenPoint.x, screenPoint.y, z));
		return worldPoint;
	}

	public Vector3 WorldPointToScreenPoint(Vector3 worldPoint) {
		return cam.WorldToScreenPoint(worldPoint);
	}

	public Vector3 ScreenPointToWorldPoint(Vector2 screenPoint, float z) {
		return cam.ScreenToWorldPoint(new Vector3(screenPoint.x, screenPoint.y, z));
	}

	public Vector2 origin {
		get {return lowerLeftAnchor.transform.position;}
	}

	public float width {
		get {return upperRightAnchor.transform.position.x - lowerLeftAnchor.transform.position.x;}
	}

	public float height {
		get {return upperRightAnchor.transform.position.y - lowerLeftAnchor.transform.position.y;}
	}

	public Vector2 size {
		get {return new Vector2(width, height);}
	}

	public float minX {
		get {return origin.x;}
	}

	public float maxX {
		get {return lowerRight.x;}
	}

	public float minY {
		get {return lowerLeft.y;}
	}

	public float maxY {
		get {return upperLeft.y;}
	}

	public float minMarginX {
		get {return lowerLeftWithMargin.x;}
	}
	
	public float maxMarginX {
		get {return lowerRightWithMargin.x;}
	}
	
	public float minMarginY {
		get {return lowerLeftWithMargin.y;}
	}
	
	public float maxMarginY {
		get {return upperLeftWithMargin.y;}
	}

	public Vector2 lowerLeft {
		get {return origin;}
	}

	public Vector2 lowerRight {
		get {return new Vector2(upperRightAnchor.transform.position.x, origin.y);}
	}

	public Vector2 upperLeft {
		get {return new Vector2(origin.x, upperRightAnchor.transform.position.y);}
	}

	public Vector2 upperRight {
		get {return upperRightAnchor.transform.position;}
	}

	public Vector2 lowerLeftWithMargin {
		get {return new Vector2(origin.x - onscreenMargin, origin.y - onscreenMargin);}
	}
	
	public Vector2 lowerRightWithMargin {
		get {return new Vector2(upperRightAnchor.transform.position.x + onscreenMargin, origin.y - onscreenMargin);}
	}
	
	public Vector2 upperLeftWithMargin {
		get {return new Vector2(origin.x - onscreenMargin, upperRightAnchor.transform.position.y + onscreenMargin);}
	}
	
	public Vector2 upperRightWithMargin {
		get {return new Vector2(upperRightAnchor.transform.position.x + onscreenMargin, upperRightAnchor.transform.position.y + onscreenMargin);}
	}

	public bool IsOnscreenHorizontally(float x) {
		return x >= lowerLeft.x && x <= lowerRight.x;
	}
	
	public bool IsOnscreenVertically(float y) {
		return y >= lowerLeft.y && y <= upperLeft.y;
	}
	
	public bool IsOnscreen(Vector2 point) {
		return IsOnscreenHorizontally(point.x) && IsOnscreenVertically(point.y);
	}

	public bool IsOnscreenHorizontallyWithMargin(float x) {
		return x >= lowerLeftWithMargin.x && x <= lowerRightWithMargin.x;
	}

	public bool IsOnscreenVerticallyWithMargin(float y) {
		return y >= lowerLeftWithMargin.y && y <= upperLeftWithMargin.y;
	}

	public bool IsOnscreenWithMargin(Vector2 point) {
		return IsOnscreenHorizontallyWithMargin(point.x) && IsOnscreenVerticallyWithMargin(point.y);
	}

	public bool IsOffLeftOfScreenWithMargin(float x) {
		return x < lowerLeftWithMargin.x;
	}
	
	public bool IsOffBottomOfScreenWithMargin(float y) {
		return y < lowerLeftWithMargin.y;
	}

	public bool IsOffRightOfScreenWithMargin(float x) {
		return x > lowerRightWithMargin.x;
	}
	
	public bool IsOffscreenTopOfScreenWithMargin(float y) {
		return y > upperLeftWithMargin.y;
	}
}
