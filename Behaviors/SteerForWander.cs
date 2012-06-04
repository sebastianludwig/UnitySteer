#define ANNOTATE_WANDER
using UnityEngine;
using UnitySteer;

/// <summary>
/// Steers a vehicle to wander around
/// </summary>
[AddComponentMenu("UnitySteer/Steer/... for Wander")]
public class SteerForWander : Steering
{
    Vector3 _jitter;            // Last jitter angle vector
    float _angle;               // Current jitter angle off the velocity in degrees

    [SerializeField]
    float _radius = 0.4f;       // Jitter length 

    [SerializeField]
    float _angleMax = 5f;       // Maximum jitter angle change per tick in degrees.

    [SerializeField]
    float _distance = 0.6f;     // Offset distance of jitter

    #region Public properties

    /// <summary>
    /// Length of the jitter force.
    /// </summary>
    public float Radius
    {
        get
        {
            return _radius;
        }
        set
        {
            _radius = value;
        }
    }

    /// <summary>
    /// Maximum angle to adjust the jitter by in degrees.
    /// </summary>
    public float AngleMax
    {
        get
        {
            return _angleMax;
        }
        set
        {
            _angleMax = value;
        }
    }

    /// <summary>
    /// Offset of the jitter force along the vehicle's forward velocity.
    /// </summary>
    public float Distance
    {
        get
        {
            return _distance;
        }
        set
        {
            _distance = value;
        }
    }

    #endregion

    protected override Vector3 CalculateForce()
    {
        // Increment the angle
        _angle += _angleMax * Random.Range(-1f, 1f);

        // Place the base force out in front of the agent
        var baseForce = Vehicle.Speed == 0
            ? Vehicle.transform.forward.normalized  // Default to forward vector
            : Vehicle.NormalizedVelocity;           // Velocity
        
        // Rotate the jitter angle about the up vector.
        _jitter = Quaternion.AngleAxis(_angle, Vector3.up) * baseForce * _radius;

        return baseForce * _distance + _jitter;
    }

#if ANNOTATE_WANDER

    void OnDrawGizmos()
    {
        if (Vehicle != null)
        {
            var baseForce = Vehicle.Position + (Vehicle.NormalizedVelocity * _distance);

            Debug.DrawLine(Vehicle.Position, baseForce, Color.black);
            Debug.DrawLine(baseForce, baseForce + _jitter, Color.red);
        }
    }

#endif
}