using UnityEngine;
using System.Collections;
using UnityEngine.VR;

/// <summary>
/// Calculate the projection for each eye
/// </summary>
public class StereoObliqueMatrix : MonoBehaviour {


	public Transform projectionScreen;
	public bool estimateViewFrustum = true;
	private TOCAVEController controller;
	private Camera cam;

	Vector3 pa,pb,pc,pe,va,vb,vc,vn,vr,vu,pos;
	float n,f;
	Matrix4x4 p,rm,tm;

	void Start(){
		controller = TOCAVEController.GetInstance;
		if (TOParameters.displayParameters.stereoInvert && !controller.advancedOptions.dualCameras)
			transform.localPosition = -transform.localPosition;
	}

	//We need to wait for every calculations to be done (ideally we should be doing this in OnPreRender)
	void LateUpdate()
	{

		if (null != projectionScreen)
		{
			
			cam = GetComponent< Camera > ();
			// lower left corner in world coordinates
			pa = projectionScreen.TransformPoint(new Vector3(-0.5f, -0.5f, 0.0f));
			// lower right corner
			pb=
				projectionScreen.TransformPoint(
					new Vector3(0.5f, -0.5f, 0.0f));
			// upper left corner
			pc =
				projectionScreen.TransformPoint(
					new Vector3(-0.5f, 0.5f, 0.0f));
			
			// distance of near clipping plane
			n = GetComponent< Camera > ().nearClipPlane;
			// distance of far clipping plane
			f = GetComponent< Camera > ().farClipPlane;


			///if we use one camera for two eyes then we need to calculate the matrices for each eye 
			if(controller.advancedOptions.dualCameras)
			{
				int invert;
				if (TOParameters.displayParameters.stereoInvert)
					invert = -1;
				else
					invert = 1;
				CalculateMatrices (transform.TransformPoint (transform.localPosition + new Vector3 (-invert*cam.stereoSeparation, 0f)),StereoTargetEyeMask.Right);
				CalculateMatrices (transform.TransformPoint (transform.localPosition + new Vector3 (invert*cam.stereoSeparation, 0f)),StereoTargetEyeMask.Left);

			}
			else
			{
				CalculateMatrices (transform.position,cam.stereoTargetEye);
			}

			// The original paper puts everything into the projection 
			// matrix (i.e. sets it to p * rm * tm and the other 
			// matrix to the identity), but this doesn't appear to 
			// work with Unity's shadow maps.
			transform.parent.localRotation = controller.transform.localRotation;
			if (estimateViewFrustum)
			{
				// rotate camera to screen for culling to work
				Quaternion q = new Quaternion();
				q.SetLookRotation((0.5f * (pb + pc) - transform.position), vu);
				// look at center of screen
				GetComponent< Camera > ().transform.rotation = q;

				// set fieldOfView to a conservative estimate 
				// to make frustum tall enough
				if (GetComponent< Camera > ().aspect >= 1.0)
				{
					GetComponent< Camera > ().fieldOfView = Mathf.Rad2Deg *
						Mathf.Atan(((pb - pa).magnitude + (pc - pa).magnitude)
							/ (pa - transform.position).magnitude);
				}
				else
				{
					// take the camera aspect into account to 
					// make the frustum wide enough 
					GetComponent< Camera > ().fieldOfView =
						Mathf.Rad2Deg / GetComponent< Camera > ().aspect *
						Mathf.Atan(((pb - pa).magnitude + (pc - pa).magnitude)
							/ (pa - transform.position).magnitude);
				}
			}
		}
	}

	/// <summary>
	/// Calculates the matrices
	/// </summary>
	/// <param name="pe">Position of the eye</param>
	/// <param name="eye">Type of eye</param>
	void CalculateMatrices(Vector3 pe,StereoTargetEyeMask eye){

		float l; // distance to left screen edge
		float r; // distance to right screen edge
		float b; // distance to bottom screen edge
		float t; // distance to top screen edge
		float d; // distance from eye to screen 

		// we need the minus sign because Unity 
		// uses a left-handed coordinate system
		vr = projectionScreen.right ;
		vu = projectionScreen.up;
		vn = -projectionScreen.forward;

		va = pa - pe;
		vb = pb - pe;
		vc = pc - pe;

		d = -Vector3.Dot(va, vn);
		l = Vector3.Dot(vr, va) * n / d;
		r = Vector3.Dot(vr, vb) * n / d;
		b = Vector3.Dot(vu, va) * n / d;
		t = Vector3.Dot(vu, vc) * n / d;

		// projection matrix
		p = new Matrix4x4();
		p[0, 0] = 2.0f * n / (r - l);
		p[0, 1] = 0.0f;
		p[0, 2] = (r + l) / (r - l);
		p[0, 3] = 0.0f;

		p[1, 0] = 0.0f;
		p[1, 1] = 2.0f * n / (t - b);
		p[1, 2] = (t + b) / (t - b);
		p[1, 3] = 0.0f;

		p[2, 0] = 0.0f;
		p[2, 1] = 0.0f;
		p[2, 2] = (f + n) / (n - f);
		p[2, 3] = 2.0f * f * n / (n - f);

		p[3, 0] = 0.0f;
		p[3, 1] = 0.0f;
		p[3, 2] = -1.0f;
		p[3, 3] = 0.0f;

		// rotation matrix
		rm = new Matrix4x4(); 
		rm[0, 0] = vr.x;
		rm[0, 1] = vr.y;
		rm[0, 2] = vr.z;
		rm[0, 3] = 0.0f;

		rm[1, 0] = vu.x;
		rm[1, 1] = vu.y;
		rm[1, 2] = vu.z;
		rm[1, 3] = 0.0f;

		rm[2, 0] = vn.x;
		rm[2, 1] = vn.y;
		rm[2, 2] = vn.z;
		rm[2, 3] = 0.0f;

		rm[3, 0] = 0.0f;
		rm[3, 1] = 0.0f;
		rm[3, 2] = 0.0f;
		rm[3, 3] = 1.0f;

		// translation matrix
		tm = new Matrix4x4();
		tm[0, 0] = 1.0f;
		tm[0, 1] = 0.0f;
		tm[0, 2] = 0.0f;
		tm[0, 3] = -pe.x;

		tm[1, 0] = 0.0f;
		tm[1, 1] = 1.0f;
		tm[1, 2] = 0.0f;
		tm[1, 3] = -pe.y;

		tm[2, 0] = 0.0f;
		tm[2, 1] = 0.0f;
		tm[2, 2] = 1.0f;
		tm[2, 3] = -pe.z;

		tm[3, 0] = 0.0f;
		tm[3, 1] = 0.0f;
		tm[3, 2] = 0.0f;
		tm[3, 3] = 1.0f;

		//Stereo doesn't work in Unity Editor
		#if UNITY_EDITOR
		cam.projectionMatrix = p;
		cam.worldToCameraMatrix = rm * tm;
		#else
		if (eye == StereoTargetEyeMask.Left) {
			cam.SetStereoProjectionMatrix (Camera.StereoscopicEye.Left, p);
			cam.SetStereoViewMatrix (Camera.StereoscopicEye.Left, rm * tm);

		} else if (eye == StereoTargetEyeMask.Right) {
			cam.SetStereoProjectionMatrix (Camera.StereoscopicEye.Right, p);
			cam.SetStereoViewMatrix (Camera.StereoscopicEye.Right, rm * tm);

		} else {
			cam.projectionMatrix = p;
			cam.worldToCameraMatrix = rm * tm;
		}
		#endif
	}
}
