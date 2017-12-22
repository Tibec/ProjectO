using UnityEngine;
using UnityEngine.VR;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Runtime.InteropServices;




[Serializable]
/// <summary>
/// Advanced options 
/// </summary>
public struct OptionsCAVE {
	/// <summary>
	/// Debug Mode : Debug mode shows 3d objects to help calibrate the CAVE
	/// </summary>
	public bool debugIsOn;
	[NonSerializedAttribute]
	public GameObject g_debug;
	/// <summary>
	/// The debug objects.
	/// </summary>
	public GameObject[] debugObjects;
	/// <summary>
	/// Show the cave limits
	/// </summary>
	public bool showLimits;
	[NonSerializedAttribute]
	public int index;
	/// <summary>
	/// Mode of rendrer for the cameras. 
	/// False : we use one camera for each eye 
	/// True  : we use a camera for both eyes (not working atm)
	/// </summary>
	[Tooltip("Mode of rendrer for the cameras. \n False : we use one camera for each eye \n True  : we use a camera for both eyes (not working atm)")]
	public bool dualCameras;
	/// <summary>
	/// update position  after all scripts instead of before (can interfere with StereoObliqueMatrix)
	/// </summary>
	public bool lateUpdate;
	/// <summary>
	/// Texture for the fake cave mode
	/// </summary>
	public Material textureCAVE;
    /// <summary>
	/// plane Prefab
	/// </summary>
    public GameObject plane;
}

/// <summary>
/// The controller class for TransOne CAVE
/// </summary>
public class TOCAVEController : MonoBehaviour
{
    public static TOCAVEController GetInstance
    {
        get
        {
            return Singleton<TOCAVEController>.instance;
        }

    }
    /// <summary>
    /// Cave Mode 
    /// False : Real Cave
    /// True : Fake CAVE
    /// </summary>
    [Tooltip("Cave Mode \nFalse : Real Cave\nTrue : Virtual CAVE")]
    public bool virtualCAVEMode;
	/// <summary>
	/// Distance minimal to a plane for a warning to appear (in meters)
	/// </summary>
	[Tooltip("Distance minimal to a plane for a warning to appear (in meters)")]
	public float boundsDetectionMin;
	/// <summary>
	/// Camera to target  (must have no rotation for now)
	/// </summary>
	[Tooltip("Camera to target  (must have no rotation for now)")]
	public GameObject mainCamera;
	/// <summary>
	/// The object where movement and rotation will occur
	/// </summary>
	[Tooltip("The object where movement and rotation will occur")]
	public GameObject target;
	/// <summary>
	/// Size of the CAVE in unity 
	/// </summary>
	[Tooltip("Size of the CAVE in unity ")]
	public Vector3 scaleCave;
	/// <summary>
	/// advanced options
	/// </summary>
	public OptionsCAVE advancedOptions;


	/// <summary>
	/// the CAVE gameobject
	/// </summary>
	private GameObject Cave;
	/// <summary>
	/// the fake cave gameobject
	/// </summary>
	private GameObject Cave_b;
	private Vector3 lastPositionController;

	private GameObject camerasCAVE; //Cameras gameobject
	/// <summary>
	/// Reference to all cameras
	/// </summary>
	private Camera[] cameras;
	/// <summary>
	/// The window position x
	/// </summary>
	private int xMin=0; 
	/// <summary>
	/// The window position y
	/// </summary>
	private int yMin=0;
	private int targetDisplay;//TODO only one instance per display for now




	[DllImport("user32.dll",EntryPoint="SetWindowPos")]
	private static extern bool SetWindowPos (IntPtr hwnd, int hWndInsertAfter, int x, int y, int cx, int cy, int wFlags);
	[DllImport("user32.dll")]
	public static extern IntPtr FindWindowEx(IntPtr parentWindow, IntPtr previousChildWindow, string windowClass, string windowTitle);
	[DllImport("user32.dll")]
	private static extern IntPtr GetWindowThreadProcessId(IntPtr window, out int process);

	/// <summary>
	///Get the pointers to the window for a processus
	/// </summary>
	/// <returns>All pointers to the window</returns>
	/// <param name="process">the processus we want the window</param>
	private static IntPtr[] GetProcessWindows(int process) 
	{
		IntPtr[] apRet = (new IntPtr[256]);
		int iCount = 0;
		IntPtr pLast = IntPtr.Zero;
		do {
			pLast = FindWindowEx(IntPtr.Zero, pLast, null, Application.productName);
			int iProcess_;
			GetWindowThreadProcessId(pLast, out iProcess_);
			if(iProcess_ == process) apRet[iCount++] = pLast;
		} while(pLast != IntPtr.Zero);
		System.Array.Resize(ref apRet, iCount);
		return apRet;
	}

	/// <summary>
	/// Sets the window position
	/// </summary>
	/// <param name="x">The x coordinate.</param>
	/// <param name="y">The y coordinate.</param>
	public static void SetPosition(int x,int y)
	{
		IntPtr[] tmp = GetProcessWindows (System.Diagnostics.Process.GetCurrentProcess ().Id);
		SetWindowPos(tmp[0],0,x,y,0,0,0);
	}
		
	/// <summary>
	/// Set the new window position
	/// </summary>
	/// <description> Note : for some reason, the unity method Display.main.SetParams dosent work </description>
	private IEnumerator SetPositionHack()
	{
		// Wait a frame.
		yield return null;
		SetPosition (xMin, yMin);
		//We need to reset the registry keys otherwise it will not behave like we want at the next start
		PlayerPrefs.DeleteKey("Screenmanager Is Fullscreen mode");
		PlayerPrefs.DeleteKey("Screenmanager Resolution Width");
		PlayerPrefs.DeleteKey("Screenmanager Resolution Height");
		PlayerPrefs.Save ();
	}


	void Awake()
	{

        if (Singleton<TOCAVEController>.CheckSingletonExists(this)) return;

        //Resolution here 
        if (TOParameters.displayParameters.displays!=null)
		{
			if (TOParameters.id_displays == null)
				targetDisplay = 0;
			else
				targetDisplay = TOParameters.id_displays [0];
			
			//check if the display is not for a mono camera then disable stereo
			for (int i = 0; i < TOParameters.displayParameters.displays [targetDisplay].screens.Count; i++) 
			{
				int id = TOParameters.displayParameters.displays [targetDisplay].screens [i].id_node;
				if(TOParameters.nodesParameters.planes.Exists(x=> (x.id == id) && (x.type == TONodeType.Camera)))
					VRSettings.enabled = false;
			}

			//Set Position and Resolution 
			if (!TOParameters.displayParameters.displays [targetDisplay].fullscreen) 
			{

				xMin = int.MaxValue;
				yMin = int.MaxValue;
				int xMax = int.MinValue, yMax = int.MinValue;

				for (int i = 0; i < TOParameters.displayParameters.displays [targetDisplay].screens.Count; i++) {

					if ((int)TOParameters.displayParameters.displays [targetDisplay].screens [i].r.x < xMin)
						xMin = (int)TOParameters.displayParameters.displays [targetDisplay].screens [i].r.x;

					if ((int)TOParameters.displayParameters.displays [targetDisplay].screens [i].r.y < yMin)
						yMin = (int)TOParameters.displayParameters.displays [targetDisplay].screens [i].r.y;

					if ((int)TOParameters.displayParameters.displays [targetDisplay].screens [i].r.xMax > xMax)
						xMax = (int)TOParameters.displayParameters.displays [targetDisplay].screens [i].r.xMax;

					if ((int)TOParameters.displayParameters.displays [targetDisplay].screens [i].r.yMax > yMax)
						yMax = (int)TOParameters.displayParameters.displays [targetDisplay].screens [i].r.yMax;

				}
				Screen.SetResolution(xMax-xMin, yMax-yMin,false);

#if !UNITY_EDITOR
                if(VRSettings.enabled)
				    StartCoroutine("SetPositionHack");
#endif

            }
            else 
			{
				Screen.SetResolution((int)TOParameters.displayParameters.displays[targetDisplay].resolution.x, 
					(int)TOParameters.displayParameters.displays[targetDisplay].resolution.y, 
					true);
			}

			Initialisation();
		}
		else
		{

			PlayerPrefs.DeleteKey("Screenmanager Is Fullscreen mode");
			PlayerPrefs.DeleteKey("Screenmanager Resolution Width");
			PlayerPrefs.DeleteKey("Screenmanager Resolution Height");
			PlayerPrefs.Save ();

			this.enabled = false;
		}


	}
		
	void Update()
	{
		if (!advancedOptions.lateUpdate )
			Appli();

	}
	void LateUpdate()
	{
		if (advancedOptions.lateUpdate)
			Appli();
	}

	void OnDestroy()
	{
		//PlayerPrefs.DeleteKey("UnitySelectMonitor");
		PlayerPrefs.DeleteKey("Screenmanager Is Fullscreen mode");
		PlayerPrefs.DeleteKey("Screenmanager Resolution Width");
		PlayerPrefs.DeleteKey("Screenmanager Resolution Height");
		PlayerPrefs.Save ();

	}





	void Initialisation()
	{
		if (mainCamera == null)
			mainCamera = Camera.main.transform.gameObject;

        //TODO : Add rotation support ?
        mainCamera.transform.rotation = mainCamera.transform.parent.rotation;


		camerasCAVE = new GameObject ("cameras_CAVE");
		camerasCAVE.transform.position = mainCamera.transform.position;
		camerasCAVE.transform.rotation = mainCamera.transform.rotation;
		camerasCAVE.transform.parent = mainCamera.transform;
		//initialRotation = camerasCAVE.transform.rotation;

		lastPositionController = transform.localPosition;

		Vector3 pos = mainCamera.transform.position + Quaternion.Euler(mainCamera.transform.eulerAngles)* Vector3.Scale(- transform.localPosition, scaleCave);


		Cave = new GameObject ("CAVE");
		Cave.transform.parent = mainCamera.transform;
		Cave.transform.position = pos;
		Cave.transform.rotation = mainCamera.transform.rotation;
		Cave.transform.localScale = Vector3.Scale(Cave.transform.localScale, scaleCave);


		if (advancedOptions.debugIsOn) 
		{
			advancedOptions.g_debug = new GameObject ("Debug");
			advancedOptions.g_debug.transform.localScale = Cave.transform.localScale;
			advancedOptions.g_debug.transform.parent = Cave.transform;
			advancedOptions.g_debug.transform.position = Cave.transform.position;
			advancedOptions.g_debug.transform.rotation = Cave.transform.rotation;
		}

		if (virtualCAVEMode) 
		{
			Cave_b = Instantiate (Cave, this.transform.parent.position,this.transform.parent.rotation,this.transform.parent);
			Cave_b.transform.localScale = Vector3.one;
		}
			

		//Initialize Stereo Cameras
		for (int i = 0; i < TOParameters.nodesParameters.planes.Count && (TOParameters.nodesParameters.planes[i].type == TONodeType.Plane); i++) 
		{
			initPlane (i);

		}

		cameras = camerasCAVE.GetComponentsInChildren<Camera>();

        if(virtualCAVEMode)
            SetCameraOptions(mainCamera.GetComponent<Camera>(), gameObject.AddComponent<Camera>(),true);

        for (int k = 0; k < TOParameters.nodesParameters.planes.Count && (TOParameters.nodesParameters.planes[k].type == TONodeType.Plane); k++) 
		{
			if (!virtualCAVEMode) 
			{
				List<TOScreen> s_tmp = TOParameters.displayParameters.displays [targetDisplay].screens.FindAll (x => x.id_node == k);

				for (int i = 0; (i < s_tmp.Count); i++) 
				{
					StartCoroutine(loadCameraStereo (k, s_tmp [i].r));

				}

				//For an eventutal multidisplay ?
				/*
				for (int h = 0; (h < TOParameters.displayParameters.displays.Count) && (h < Display.displays.Length); h++) {

					List<TOScreen> s_tmp = TOParameters.displayParameters.displays [h].screens.FindAll (x => x.id_node == k);

					for (int i = 0; (i < s_tmp.Count)&&display_found; i++) {
						Notworking in opengl
						if (Display.displays.Length > TOParameters.displayParameters.displays[h].screens[i].id_node-1 && SystemInfo.graphicsDeviceType != UnityEngine.Rendering.GraphicsDeviceType.OpenGLCore)
						Display.displays [TOParameters.displayParameters.displays[h].screens[i].id_node-1].Activate ();
						loadCameraStereo(k,s_tmp [i].r);
					}

				}*/

				

			} 
			else 
			{
				StartCoroutine(loadCameraStereo (k));
			}

		}


		//Initialize Mono Cameras
		TOMonoCamera[] m = FindObjectsOfType<TOMonoCamera> ();
		for (int i = 0; i < m.Length; i++) 
		{
			List<TOScreen> s_tmp = TOParameters.displayParameters.displays [targetDisplay].screens.FindAll (x => x.id_node == m [i].idNode);
			for (int k = 0; (k < s_tmp.Count); k++) 
			{
				GameObject tmp_cam = new GameObject ("Camera_" + TOParameters.displayParameters.displays [targetDisplay].screens [k] + m [i].idNode);
				tmp_cam.transform.parent = m [i].transform;
				tmp_cam.transform.localPosition = Vector3.zero;
				tmp_cam.transform.localRotation = Quaternion.identity;

				Camera c = tmp_cam.AddComponent<Camera> ();
				SetCameraOptions (m[i].GetComponent<Camera>(),c);
				StartCoroutine(loadCameraMono(c,s_tmp [k].r));
			}

		}
        
        mainCamera.GetComponent<Camera>().enabled = false;
	}



	IEnumerator loadCameraMono(Camera c, Rect r = default(Rect)){
		yield return null;
		c.pixelRect = new Rect (r.x - xMin, r.y - yMin, r.width, r.height);
	}


	IEnumerator loadCameraStereo(int id, Rect r = default(Rect)){
		//We need to wait one frame because we need to change display resolution first 
		yield return null;
		Rect r_tmp = new Rect (r.x-xMin, r.y-yMin, r.width, r.height);
		for (int j = 0; j < 2; j++) 
		{
			int tmp;
			if (advancedOptions.dualCameras || virtualCAVEMode)
				tmp = id;
			else
				tmp = id * 2 + j;

			if(!r.Equals(default(Rect)))
				cameras [tmp].pixelRect = r_tmp;

			cameras [tmp].enabled = true;
			if (advancedOptions.dualCameras || virtualCAVEMode)
				j = 2;

		}

	}


	void Appli()
	{

		if (advancedOptions.debugIsOn)
		{

			if (Input.GetKeyDown(KeyCode.Keypad6))
			{
				advancedOptions.showLimits = !advancedOptions.showLimits;

				if (advancedOptions.showLimits)
					boundsDetectionMin = 50f;
				else
					boundsDetectionMin = 0.8f;
			}
			if (Input.GetKeyDown(KeyCode.D))
			{
				advancedOptions.index = (advancedOptions.index + 1) % advancedOptions.debugObjects.Length;
				DestroyChildren (advancedOptions.g_debug);
				for (int i = Cave.transform.childCount-1; i >0 ; i--) 
				{
					InstantiateDebug (Cave.transform.GetChild(i),advancedOptions.index);
				}
			}
		}

	
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			Application.Quit();
		}

		Vector3 res = Vector3.Scale(transform.localPosition - lastPositionController, scaleCave);
        Cave.transform.localPosition -= res;
        if (target != null)
		{
            target.transform.localPosition += target.transform.localRotation*new Vector3(res.x, 0, res.z);
            mainCamera.transform.localPosition += mainCamera.transform.localRotation * new Vector3(0, res.y, 0);
        }
        
        
        lastPositionController = transform.localPosition;
	}

	/// <summary>
	/// Initialize a plane.
	/// </summary>
	/// <param name="i">The plane index</param>
	void initPlane(int i){
        
		//Plane gameobject initialiation
		int id = TOParameters.nodesParameters.planes [i].id;
		GameObject tmp_plane = Instantiate(advancedOptions.plane, Cave.transform);
		tmp_plane.transform.localPosition = TOParameters.nodesParameters.planes [id].pos + TOParameters.nodesParameters.root.pos;
		tmp_plane.transform.localRotation = advancedOptions.plane.transform.rotation *Quaternion.Euler (TOParameters.nodesParameters.root.rot) * Quaternion.Euler (TOParameters.nodesParameters.planes[id].rot);
		tmp_plane.name = TOParameters.nodesParameters.planes [id].name;
		tmp_plane.transform.localScale = new Vector3(TOParameters.nodesParameters.planes [id].size.x,TOParameters.nodesParameters.planes[id].size.y,0);

		if (advancedOptions.debugIsOn)
		{
			InstantiateDebug (tmp_plane.transform,0);
		}

		GameObject tmp_cam = new GameObject ("Camera_" + TOParameters.nodesParameters.planes[id].name);
		tmp_cam.transform.parent= camerasCAVE.transform;
		tmp_cam.transform.position = camerasCAVE.transform.position;
		tmp_cam.transform.rotation = camerasCAVE.transform.rotation;
        tmp_cam.transform.localScale = scaleCave;

		//Texture initialisation if we use a fake CAVE
		RenderTexture t = new RenderTexture(1,1,24);
		if (virtualCAVEMode) 
		{
			Rect r = new Rect();
			TOScreen s = new TOScreen();
           
            for (int j = 0; j < TOParameters.displayParameters.displays.Count && (s.id_node==-1); j++) {
				TOScreen tmp = TOParameters.displayParameters.displays [j].screens.Find (x => x.id_node == i);
				if (tmp != null)
                {
                    s = tmp;
                    r = s.r;
                }    
			}

			t= new RenderTexture(Convert.ToInt32(r.width),Convert.ToInt32(r.height),24);
			GameObject tmp_plane_b = Instantiate (tmp_plane, Cave_b.transform, false);
			tmp_plane_b.transform.Rotate (0, 180, 0, Space.Self);
			t.Create ();
			Material m = new Material (advancedOptions.textureCAVE);
			m.SetTexture ("_MainTex", t);
			tmp_plane_b.GetComponent<MeshRenderer> ().material = m;
		}
		else 
		{
			t.Release ();
			Destroy (t);
		} 

		if (boundsDetectionMin>0.0f) tmp_plane.AddComponent<DetectionBounds>();

		//cameras initialiation
		for (int j = 0; j < 2; j++) 
		{
			GameObject tmp_cam_2 = new GameObject ("Camera_" + TOParameters.nodesParameters.planes[id].name + j);
			tmp_cam_2.transform.parent= tmp_cam.transform;
			Camera c =tmp_cam_2.AddComponent<Camera> ();
			c.transform.position = camerasCAVE.transform.position;
			c.transform.rotation = camerasCAVE.transform.rotation;

			SetCameraOptions (mainCamera.GetComponent<Camera>(), c);

			if (advancedOptions.dualCameras || virtualCAVEMode) 
			{
				c.stereoTargetEye = StereoTargetEyeMask.Both;
				j = 2;
			} 
			else 
			{
				if (j == 0) 
				{
					c.stereoTargetEye = StereoTargetEyeMask.Left;
					c.transform.localPosition += new Vector3 (-c.stereoSeparation, 0, 0);
				} 
				else 
				{
					c.stereoTargetEye = StereoTargetEyeMask.Right;
					c.transform.localPosition += new Vector3 (c.stereoSeparation, 0, 0);
				}
			}

			StereoObliqueMatrix s = tmp_cam_2.AddComponent<StereoObliqueMatrix> ();
			s.projectionScreen = tmp_plane.transform;

			if (virtualCAVEMode) 
				c.targetTexture = t;

			c.enabled = false;
		}
	}

	/// <summary>
	/// Copy camera options into another camera
	/// </summary>
	/// <param name="in_camera">Camera we want to copy.</param>
	/// <param name="out_camera">Camera we want the data to cpoied to</param>
	void SetCameraOptions(Camera in_camera,Camera out_camera,bool setViewport = false )
	{
		out_camera.tag = "MainCamera";
		out_camera.renderingPath = in_camera.renderingPath;
		out_camera.farClipPlane = in_camera.farClipPlane;
		out_camera.nearClipPlane = in_camera.nearClipPlane;
		out_camera.depth = in_camera.depth;
		out_camera.fieldOfView = in_camera.fieldOfView;
		out_camera.cullingMask = in_camera.cullingMask;
		out_camera.clearFlags = in_camera.clearFlags;
		out_camera.backgroundColor = in_camera.backgroundColor;
		out_camera.stereoSeparation = in_camera.stereoSeparation;
		if (setViewport) {
			out_camera.pixelRect = in_camera.pixelRect;
		}

	}

	/// <summary>
	/// Inverse a Vector3
	/// </summary>
	/// <returns>The vector to inverse</returns>
	/// <param name="v">V.</param>
	public static Vector3 InverseVec3(Vector3 v)
	{
		if ((v.x != 0) && (v.y != 0) && (v.z != 0)) return (new Vector3(1 / v.x, 1 / v.y, 1 / v.z));
		return v;
	}

	/// <summary>
	/// Destroies a gameobject children.
	/// </summary>
	/// <param name="g">A gameobject</param>
	void DestroyChildren(GameObject g){

		for (int i = g.transform.childCount-1; i >=0 ; i--) 
		{
			Destroy (g.transform.GetChild(i).gameObject);
		}
	}

	/// <summary>
	/// Instantiates the debug objects fo a plane
	/// </summary>
	/// <param name="t">Trnsform of the plane</param>
	/// <param name="i">the type of ojecx</param>
	void InstantiateDebug(Transform t,int i)
	{

		Quaternion rot = t.rotation * advancedOptions.debugObjects[i].transform.rotation;

		float offsetX;
		float offsetY;
		if (i == 1) {
			offsetX = advancedOptions.debugObjects[i].transform.localScale.x / 2 / t.localScale.x;
			offsetY = advancedOptions.debugObjects[i].transform.localScale.y / 2 / t.localScale.y;
		} else {
			offsetX = 0.0f;
			offsetY = 0.0f;
		}

		Instantiate(advancedOptions.debugObjects[i], t.position,rot, advancedOptions.g_debug.transform.transform);
		Instantiate(advancedOptions.debugObjects[i], t.TransformPoint(new Vector3(0.0f,  0.5f-offsetY,0.0f)),rot , advancedOptions.g_debug.transform);
		Instantiate(advancedOptions.debugObjects[i], t.TransformPoint(new Vector3(0.0f, -0.5f+offsetY,0.0f)),rot, advancedOptions.g_debug.transform);
		Instantiate(advancedOptions.debugObjects[i], t.TransformPoint(new Vector3(0.5f-offsetX, 0.0f, 0.0f)), rot, advancedOptions.g_debug.transform);
		Instantiate(advancedOptions.debugObjects[i], t.TransformPoint(new Vector3(0.5f-offsetX, 0.5f-offsetY,0.0f)), rot, advancedOptions.g_debug.transform);
		Instantiate(advancedOptions.debugObjects[i], t.TransformPoint(new Vector3(0.5f-offsetX, -0.5f+offsetY,0.0f)), rot, advancedOptions.g_debug.transform);
		Instantiate(advancedOptions.debugObjects[i], t.TransformPoint(new Vector3(-0.5f+offsetX, 0.0f, 0.0f)), rot, advancedOptions.g_debug.transform);
		Instantiate(advancedOptions.debugObjects[i], t.TransformPoint(new Vector3(-0.5f+offsetX,  0.5f-offsetY,0.0f)), rot, advancedOptions.g_debug.transform);
		Instantiate(advancedOptions.debugObjects[i], t.TransformPoint(new Vector3(-0.5f+offsetX, -0.5f+offsetY,0.0f)), rot, advancedOptions.g_debug.transform);

	}




}