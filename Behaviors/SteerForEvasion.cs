using UnityEngine;
using UnitySteer;
using UnitySteer.Helpers;

/// <summary>
/// Steers a vehicle to avoid another object. The objects velocity is tracked by
/// a MovementTracker.
/// </summary>
[AddComponentMenu("UnitySteer/Steer/... for Evasion")]
public class SteerForEvasion : Steering
{
	#region Private fields
	MovementTracker _movementTracker;
	
	[SerializeField]
	Transform _menace;

	[SerializeField]
	float _predictionTime;
	
	[SerializeField]
	float _trackingInterval = 0.25f;
	#endregion
	
	#region Public properties
	/// <summary>
	/// How many seconds to look ahead for position prediction
	/// </summary>
	public float PredictionTime {
		get {
			return this._predictionTime;
		}
		set {
			_predictionTime = value;
		}
	}
	
	/// <summary>
	/// Vehicle menace
	/// </summary>
	public Transform Menace {
		get {
			return this._menace;
		}
		set {
			_menace = value;
			if (_movementTracker != null)
			{
				_movementTracker.target = _menace;
				_movementTracker.enabled = _menace != null;
			}
		}
	}
	#endregion
	
	protected override void Awake()
	{
		base.Awake();
		_movementTracker = new MovementTracker(Menace, _trackingInterval);
		_movementTracker.enabled = Menace != null;
	}
	
	protected override Vector3 CalculateForce()
	{
		if (_menace == null)
		{
			return Vector3.zero;
		}
		
		// offset from this to menace, that distance, unit vector toward menace
		Vector3 offset = _menace.position - Vehicle.Position;
		float distance = offset.magnitude;

		float roughTime = _movementTracker.isMoving ? distance / _movementTracker.velocity.magnitude : 0;
		float predictionTime = Mathf.Min(roughTime, _predictionTime);
		
		Vector3 target = Menace.position + _movementTracker.velocity * predictionTime;

		// This was the totality of SteerToFlee
		Vector3 desiredVelocity = Vehicle.Position - target;
		return desiredVelocity - Vehicle.Velocity;		
	}
	
}