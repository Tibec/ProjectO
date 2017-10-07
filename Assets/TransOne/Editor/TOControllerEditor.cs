using UnityEngine;
using UnityEngine.Rendering;
using UnityEditor;
using System.Collections;
using System.IO;
using System.Windows.Forms;
using UnityEditor.Callbacks;

[CustomEditor(typeof(TOController))]
public class TOControllerEditor : Editor
{

	private TOController controller;


	void Awake()
	{
		controller = FindObjectOfType<TOController>();
	}

	public void ApplyVRSettings()
	{
		PlayerSettings.defaultIsFullScreen = false;
		PlayerSettings.displayResolutionDialog = ResolutionDialogSetting.HiddenByDefault;
		PlayerSettings.allowFullscreenSwitch = true;
		PlayerSettings.runInBackground = true;
		PlayerSettings.captureSingleScreen = false;
		PlayerSettings.MTRendering = true;
		PlayerSettings.SetGraphicsAPIs(BuildTarget.StandaloneWindows64,new GraphicsDeviceType[]{GraphicsDeviceType.OpenGLCore});


		Debug.Log("VR Player settings changed:");

		string[] names = QualitySettings.names;
		int qualityLevel = QualitySettings.GetQualityLevel();

		// Enable VSync on all quality levels
		for( int i=0 ; i<names.Length ; ++i )
		{
			QualitySettings.SetQualityLevel( i );
			QualitySettings.vSyncCount = 1;
		}

		QualitySettings.SetQualityLevel( qualityLevel );
	}

	public override void OnInspectorGUI()
	{

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Re-apply VR player settings"))
		{
			ApplyVRSettings();
		}

        
        if (GUILayout.Button("Pick Display configuration file"))
		{
			string path = LoadFile("Display");
            if (path!="")
            {
                controller.fileDisplay = path;
                EditorUtility.SetDirty(controller);
            }
            
		}
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();

        if (GUILayout.Button("Pick Node configuration file"))
        {

			string path = LoadFile("Nodes");
            if (path != "")
            {
                controller.fileNodes = path;
                EditorUtility.SetDirty(controller);
            }
        }
        if (GUILayout.Button("Pick Tracker configuration file"))
        {
			string path = LoadFile("Tracker");
            if (path != "")
            {
                controller.fileTracker = path;
                EditorUtility.SetDirty(controller);
            }
        }

        

        GUILayout.EndHorizontal();
        DrawDefaultInspector();
    }


	public string LoadFile(string s) {
		
			OpenFileDialog openFileDialog1 = new OpenFileDialog();

			openFileDialog1.Filter = string.Format("{0} configuration file|*.{0}.json", s);
			openFileDialog1.Title = string.Format("Choose {0} configuration file",s);

			if (openFileDialog1.ShowDialog() != System.Windows.Forms.DialogResult.OK)
			{
				return "";
			}

		return openFileDialog1.SafeFileName;

	}


}



public class FileSystemTools
{
	public static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs, bool overwrite)
	{
		// Get the subdirectories for the specified directory
		DirectoryInfo dir = new DirectoryInfo(sourceDirName);
		DirectoryInfo[] dirs = dir.GetDirectories();

		if (!dir.Exists)
		{
			throw new DirectoryNotFoundException(
				"Source directory does not exist or could not be found: '"
				+ sourceDirName + "'.");
		}

		// If the destination directory doesn't exist, create it
		if (!Directory.Exists(destDirName))
		{
			Directory.CreateDirectory(destDirName);
		}

		// Get the files in the directory and copy them to the new location
		FileInfo[] files = dir.GetFiles();
		foreach (FileInfo file in files)
		{
			if (!file.Name.ToLower().EndsWith(".meta"))
			{
				string temppath = Path.Combine(destDirName, file.Name);
				file.CopyTo(temppath, overwrite);
			}
		}

		// If copying subdirectories, copy them and their contents to new location
		if (copySubDirs)
		{
			foreach (DirectoryInfo subdir in dirs)
			{
				string temppath = Path.Combine(destDirName, subdir.Name);
				DirectoryCopy(subdir.FullName, temppath, copySubDirs, overwrite);
			}
		}
	}
}
