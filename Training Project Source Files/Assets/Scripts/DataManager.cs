using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class DataManager
{
    public static void SaveBrain(NetData brain)
    {
        int ind = PlayerPrefs.GetInt("SaveIndex", 0);
        BinaryFormatter formatter = new BinaryFormatter();
        string path = "D:/anim/project/project with actual deadlines/grp 4 project/BrainSave/brain_" + ind.ToString() + ".neural";
        FileStream stream = new FileStream(path, FileMode.CreateNew);
        PlayerPrefs.SetInt("SaveIndex", ind + 1);

        formatter.Serialize(stream, brain);
        stream.Close();
    }

    public static NetData LoadBrain(int index)
    {
        string path = "D:/anim/project/project with actual deadlines/grp 4 project/BrainSave/brain_" + index.ToString() + ".neural";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            NetData brain_output = formatter.Deserialize(stream) as NetData;
            stream.Close();

            return brain_output;
        }
        else
        {
            Debug.LogError("no existing index file");
            return null;
        }
    }
}
