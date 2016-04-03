using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class StateMachine : MonoBehaviour {
	[HideInInspector] public Enum lastState;

    public class State {
        public Action DoUpdateState = DoNothing;
		public Action DoFixedUpdateState = DoNothing;
        public Action EnterState = DoNothing;
        public Action ExitState = DoNothing;
		public Action LeftSwipe = DoNothing;
		public Action RightSwipe = DoNothing;
		public Action UpSwipe = DoNothing;
		public Action DownSwipe = DoNothing;
		public Action Tap = DoNothing;
		public Action TouchUp = DoNothing;
		public Action TouchDown = DoNothing;
		public Action LeftTouchDown = DoNothing;
		public Action LeftTouchUp = DoNothing;
		public Action RightTouchDown = DoNothing;
		public Action RightTouchUp = DoNothing;

        public Enum currentState;
    }

    public Enum currentState {
        get {return state.currentState;}
        set {
            if (state.currentState == value) return;

            ChangingState();
            state.currentState = value;
            ConfigureCurrentState();
        }
    }

	protected float timeEnteredState;

	private State state = new State();
	private Dictionary<Enum, Dictionary<string, Delegate>> allStateDelegates = new Dictionary<Enum, Dictionary<string, Delegate>>();
	private bool swipeDetectionInitiated = false;

	private void HandleLeftSwipe() {
		state.LeftSwipe();
	}
	
	private void HandleRightSwipe() {
		state.RightSwipe();
	}
	
	private void HandleUpSwipe() {
		state.UpSwipe();
	}
	
	private void HandleDownSwipe() {
		state.DownSwipe();
	}

	private void HandleLeftTouchDown() {
		state.LeftTouchDown();
	}

	private void HandleLeftTouchUp() {
		state.LeftTouchUp();
	}

	private void HandleRightTouchDown() {
		state.RightTouchDown();
	}

	private void HandleRightTouchUp() {
		state.RightTouchUp();
	}
	
	private void HandleTap() {
		state.Tap();
	}

	private void HandleTouchUp() {
		state.TouchUp();
	}
	
	private void HandleTouchDown() {
		state.TouchDown();
	}

	private void Update() {
		PreUpdateState();
		state.DoUpdateState();
		PostUpdateState();
	}

	private void FixedUpdate() {
		PreFixedUpdateState();
		state.DoFixedUpdateState();
		PostFixedUpdateState();
	}

	private void ChangingState() {
        lastState = state.currentState;
        timeEnteredState = Time.time;
    }

	private void ConfigureCurrentState() {
		if (!swipeDetectionInitiated) InitiateSwipeDetection();

        if (state.ExitState != null) state.ExitState();

		state.DoUpdateState = ConfigureDelegate<Action>("UpdateState", DoNothing);
		state.DoFixedUpdateState = ConfigureDelegate<Action>("FixedUpdateState", DoNothing);
        state.EnterState = ConfigureDelegate<Action>("EnterState", DoNothing);
        state.ExitState = ConfigureDelegate<Action>("ExitState", DoNothing);
		state.LeftSwipe = ConfigureDelegate<Action>("LeftSwipe", DoNothing);
		state.RightSwipe = ConfigureDelegate<Action>("RightSwipe", DoNothing);
		state.UpSwipe = ConfigureDelegate<Action>("UpSwipe", DoNothing);
		state.DownSwipe = ConfigureDelegate<Action>("DownSwipe", DoNothing);
		state.Tap = ConfigureDelegate<Action>("Tap", DoNothing);
		state.TouchUp = ConfigureDelegate<Action>("TouchUp", DoNothing);
		state.TouchDown = ConfigureDelegate<Action>("TouchDown", DoNothing);
		state.LeftTouchDown = ConfigureDelegate<Action>("LeftTouchDown", DoNothing);
		state.LeftTouchUp = ConfigureDelegate<Action>("LeftTouchUp", DoNothing);
		state.RightTouchDown = ConfigureDelegate<Action>("RightTouchDown", DoNothing);
		state.RightTouchUp = ConfigureDelegate<Action>("RightTouchUp", DoNothing);

        if (state.EnterState != null) state.EnterState();
    }

	private T ConfigureDelegate<T>(string methodRoot, T Default) where T : class {
		Dictionary<string, Delegate> currentStateDelegates;
		Delegate currentStateDelegate;

		// Check if there's already a dictionary of state methods/delegates for the current state.
		// If there isn't one yet, create one.
        if (!allStateDelegates.TryGetValue(state.currentState, out currentStateDelegates)) {
            allStateDelegates[state.currentState] = currentStateDelegates = new Dictionary<string, Delegate>();
        }

		// If the current state already has a delegate defined for this method root, just return it.
		// Otherwise, create a connection to the appropriate method and return it.
        if (!currentStateDelegates.TryGetValue(methodRoot, out currentStateDelegate)) {
			string fullMethodName = state.currentState.ToString() + "_" + methodRoot;
            var method = GetType().GetMethod(fullMethodName,
			                                 System.Reflection.BindingFlags.Instance |
			                                 System.Reflection.BindingFlags.Public |
			                                 System.Reflection.BindingFlags.NonPublic |
			                                 System.Reflection.BindingFlags.InvokeMethod);

            if (method != null) currentStateDelegate = Delegate.CreateDelegate(typeof(T), this, method);
            else currentStateDelegate = Default as Delegate;

            currentStateDelegates[methodRoot] = currentStateDelegate;
        }
        return currentStateDelegate as T;
    }

    protected virtual void PreUpdateState() {}
    protected virtual void PostUpdateState() {}

	protected virtual void PreFixedUpdateState() {}
	protected virtual void PostFixedUpdateState() {}

	private void InitiateSwipeDetection() {
		if (InputManager.instance == null) return;

		swipeDetectionInitiated = true;

		InputManager.instance.SignalTap += HandleTap;
		InputManager.instance.SignalLeftSwipe += HandleLeftSwipe;
		InputManager.instance.SignalRightSwipe += HandleRightSwipe;
		InputManager.instance.SignalUpSwipe += HandleUpSwipe;
		InputManager.instance.SignalDownSwipe += HandleDownSwipe;
		InputManager.instance.SignalTouchDown += HandleTouchDown;
		InputManager.instance.SignalTouchUp += HandleTouchUp;
		InputManager.instance.SignalLeftTouchDown += HandleLeftTouchDown;
		InputManager.instance.SignalLeftTouchUp += HandleLeftTouchUp;
		InputManager.instance.SignalRightTouchDown += HandleRightTouchDown;
		InputManager.instance.SignalRightTouchUp += HandleRightTouchUp;
	}

	private void RemoveSwipeDetection() {
		if (InputManager.instance == null) return;

		InputManager.instance.SignalTap -= HandleTap;
		InputManager.instance.SignalLeftSwipe -= HandleLeftSwipe;
		InputManager.instance.SignalRightSwipe -= HandleRightSwipe;
		InputManager.instance.SignalUpSwipe -= HandleUpSwipe;
		InputManager.instance.SignalDownSwipe -= HandleDownSwipe;
		InputManager.instance.SignalTouchDown -= HandleTouchDown;
		InputManager.instance.SignalTouchUp -= HandleTouchUp;
		InputManager.instance.SignalLeftTouchDown -= HandleLeftTouchDown;
		InputManager.instance.SignalLeftTouchUp -= HandleLeftTouchUp;
		InputManager.instance.SignalRightTouchDown -= HandleRightTouchDown;
		InputManager.instance.SignalRightTouchUp -= HandleRightTouchUp;
	}

    static void DoNothing() {}

	private void OnDestroy() {
		RemoveSwipeDetection();
	}
}
