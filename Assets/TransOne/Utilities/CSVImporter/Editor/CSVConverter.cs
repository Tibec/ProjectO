using System;
using System.IO;
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Windows.Forms;


enum TrackingSystem
{
    Optitrack
}


public class CSVConverter : EditorWindow
{

    public string InverseSign(string s)
    {
        if (s.StartsWith("-"))
            return s.Remove(0, 1);
        else
        {
            return s.Insert(0, "-");
        }
        
    }



    public void Convert(string s)
    {

        DataLogger inputfile = new DataLogger(s, FileAccess.Read);
        DataLogger outputFile = new DataLogger(Path.ChangeExtension(s,"txt"),FileAccess.Write);
        
            
        const string c = " ";
        char[] d = new char[1] { ',' };




        bool isdata=false;
        bool ismarker = false;
        string marker="";
        string line;
        while ((line = inputfile.Read()) != null)
        {
           
            //Process row
            string[] fields = line.Split(d);
            if (fields.Length >2) {
                if (isdata)
                {
                    if (t == TrackingSystem.Optitrack)
                        outputFile.Write(objName + c + fields[1] + c + marker + c + InverseSign(fields[6]) + c + fields[7] + c + fields[8] + c + InverseSign(fields[2]) + c + fields[3] + c + fields[4] + c + InverseSign(fields[5]));
                }
                else
                    isdata = fields[0] == "Frame";

                if (ismarker)
                    marker = fields[2];
                ismarker = fields[2] == "Rigid Body";



            }
            


        }
        inputfile.CloseFile();
        outputFile.CloseFile();
        
    }

    
    string sourceFilePath = "";
    string objName = "gesture";
    TrackingSystem t = TrackingSystem.Optitrack;
    bool isOptitrack = false;

    // Add menu item named "My Window" to the Window menu
    [UnityEditor.MenuItem("TransOne/CSVConverter")]
    public static void ShowWindow()
    {
        //Show existing window instance. If one doesn't exist, make one.
        EditorWindow.GetWindow(typeof(CSVConverter));
        


    }






    void OnGUI()
    {

     

        GUILayout.Label("Import", EditorStyles.boldLabel);
       
        if (GUILayout.Button("Pick file"))
        {
            sourceFilePath = LoadFile();

        }
        sourceFilePath = EditorGUILayout.TextField("File", sourceFilePath);
        objName = EditorGUILayout.TextField("Name", objName);
        GUILayout.Label("Tracking System");

        isOptitrack = EditorGUILayout.Toggle("Optitrack", isOptitrack);



        if (isOptitrack)
            t = TrackingSystem.Optitrack;

       
        if (GUILayout.Button("Convert File"))
        {
            
            Convert(sourceFilePath);
            
        }


    }

    
    public string LoadFile()
    {

        OpenFileDialog openFileDialog1 = new OpenFileDialog();
        openFileDialog1.Filter = "CSV file|*.csv";
        openFileDialog1.Title = "Choose file";

        if (openFileDialog1.ShowDialog() != System.Windows.Forms.DialogResult.OK)
        {
            return "";
        }

        return openFileDialog1.FileName;

    }


}
