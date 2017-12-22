using System;
using UnityEngine;

/// <summary>
/// This class augments the original OptitrackRigidbody by integrating more features
/// and integrating it with TransOne
/// </summary>
public class TOOptitrackRigidBody : MonoBehaviour
{
	[Serializable]
	public struct Vec3Bool
	{
		public bool x;
		public bool y;
		public bool z;
	};

	public OptitrackStreamingClient StreamingClient;
    public Int32 RigidBodyId;
	public bool leftHanded = false;
    /// <summary>
    /// Tracking will change the local or global position/rotation of the object ?
    /// </summary>
	public bool isWorld = false;
	public Vec3Bool freezePosition; 
	public Vec3Bool freezeRotation;
	public Vector3 scale= Vector3.one;
	/// <summary>
	/// Precision of postition data
	/// </summary>
	public int precision_pos=5;
	/// <summary>
	/// Precision of rotation data
	/// </summary>
	public int precision_rot=2;

	private float tmp_precision_pos;
	private float tmp_precision_rot;
    private Vector3 scaleCave = Vector3.one;
	private int sign=1;
    void Start()
    {
        // If the user didn't explicitly associate a client, find a suitable default.
        if ( this.StreamingClient == null )
        {
            this.StreamingClient = OptitrackStreamingClient.FindDefaultClient();

            // If we still couldn't find one, disable this component.
            if ( this.StreamingClient == null )
            {
                Debug.LogError( GetType().FullName + ": Streaming client not set, and no " + typeof( OptitrackStreamingClient ).FullName + " components found in scene; disabling this component.", this );
                this.enabled = false;
                return;
            }
        }
		tmp_precision_pos = Mathf.Pow (10, precision_pos);
		tmp_precision_rot = Mathf.Pow (10, precision_rot);
		if (leftHanded)
			sign = -1;

		//Tmp because TOinput is not implemented for optitrack 
		if (TOController.GetInstance != null &&(TOParameters.isClient ||TOParameters.isServer)) {
			this.StreamingClient.LocalAddress = TOParameters.ip_client;
			this.StreamingClient.enabled = false;
			this.StreamingClient.enabled = true;

		}

		//Check if we use TO inputs
        if(TOInputController.GetInstance != null)
        {
			(TOInputController.GetInstance).positionTrackers.Add(transform);
            
        }

        if (TOCAVEController.GetInstance != null)
        {
            if(GetComponent<TOCAVEController>() == null)
            scaleCave = (TOCAVEController.GetInstance).scaleCave;

        }


    }


    void Update()
    {


        OptitrackRigidBodyState rbState = StreamingClient.GetLatestRigidBodyState( RigidBodyId );
        if ( rbState != null )
        {
			Vector3 tmp_pos = Vector3.zero;
			Vector3 tmp_rot = Vector3.zero;

			if (!freezePosition.x)
				tmp_pos += new Vector3 (sign * Mathf.Round(rbState.Pose.Position.x*tmp_precision_pos)/tmp_precision_pos, 0, 0);
			if (!freezePosition.y)
				tmp_pos += new Vector3 (0, Mathf.Round(rbState.Pose.Position.y*tmp_precision_pos)/tmp_precision_pos, 0);
			if (!freezePosition.z)
				tmp_pos += new Vector3 (0, 0,Mathf.Round(rbState.Pose.Position.z*tmp_precision_pos)/tmp_precision_pos);

			Vector3 euAngle = rbState.Pose.Orientation.eulerAngles;
			if (!freezeRotation.x)
				tmp_rot += new Vector3 (Mathf.Round(euAngle.x*tmp_precision_rot)/tmp_precision_rot, 0, 0);
			if (!freezeRotation.y)
				tmp_rot += new Vector3 (0, Mathf.Round(euAngle.y*tmp_precision_rot)/tmp_precision_rot, 0);
			if (!freezeRotation.z)
				tmp_rot += new Vector3 (0, 0,Mathf.Round(euAngle.z*tmp_precision_rot)/tmp_precision_rot);

            Vector3 tmp_scale = Vector3.Scale(scaleCave, scale);

            if (isWorld) {
				this.transform.position = Vector3.Scale(tmp_pos, tmp_scale);
				this.transform.eulerAngles = tmp_rot;

			} else {
				this.transform.localPosition = Vector3.Scale(tmp_pos, tmp_scale);
				this.transform.localEulerAngles = tmp_rot;
			}

        }
    }
}
