using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

//saves every class as binary
public static class SaveSystem<T>
{

    public static void SavePlayer(T Data, string fileName)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/" + fileName + ".save";
        FileStream stream = new FileStream(path, FileMode.Create);

        formatter.Serialize(stream, Data);
        stream.Close();
    }

    public static T LoadPlayer(string fileName)
    {
        string path = Application.persistentDataPath + "/" + fileName + ".save";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            T data = (T)formatter.Deserialize(stream);
            stream.Close();
            return data;
        }
        else { return default; }

    }

}
