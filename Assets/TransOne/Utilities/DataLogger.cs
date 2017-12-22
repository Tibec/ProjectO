using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataLogger {

    public string nameFile;
    private FileStream fsData;
    private StreamWriter stwData;
    private StreamReader strData;


    public DataLogger(string nameFile,FileAccess access)
    {
        this.nameFile = nameFile;
        
        if ((access & FileAccess.Write)!=0)
        {
            fsData = new FileStream(nameFile, FileMode.Append, access, FileShare.Read);
            stwData = new StreamWriter(fsData);
            stwData.AutoFlush = true;
        }
        if((access & FileAccess.Read) != 0)
        {
            if(fsData == null)
                fsData = new FileStream(nameFile, FileMode.Open, access, FileShare.Read);
            strData = new StreamReader(fsData);
        }
       
        
    }

    public void Write(string s)
    {
        stwData.WriteLine(s);
    }

    public string Read()
    {
        return strData.ReadLine();
    }


    public void CloseFile()
    {
        
        if (fsData.CanWrite)
        {
            stwData.Dispose();
            stwData.Close();
        }
        if (fsData.CanRead)
        {
            strData.Dispose();
            strData.Close();
        }
       
    }

}
