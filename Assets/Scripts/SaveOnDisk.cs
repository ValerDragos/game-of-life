using UnityEngine;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Collections.Generic;

public static class SaveOnDisk {

	public static readonly PersistentDataFile<SavedRules> savedRules = PersistentDataFile<SavedRules>.Create<SavedRules>("SavedRules.dat");

}

public class PersistentDataFile<T> where T : PersistentData, new()
{
	private static readonly BinaryFormatter _bf = new BinaryFormatter();

	private T _data;

	public T data {get{return _data;}}

	private string path;

	private PersistentDataFile (string path)
	{
		this.path = path;
		Load ();
		if (_data == null) _data = new T();
	}

	public static PersistentDataFile<T> Create<T> (string path) where T : PersistentData, new()
	{
		if (string.IsNullOrEmpty(path)) return null;
		return new PersistentDataFile<T>(Application.persistentDataPath+"/"+path);
	}

	public void Save ()
	{
		FileStream fs = File.Open(path,FileMode.Open);
		_bf.Serialize(fs,data);
		fs.Close();
	}

	public void Load ()
	{
		if (File.Exists(path))
		{
			FileStream fs = File.Open(path,FileMode.Open);
			_data = (T) _bf.Deserialize(fs);
			fs.Close();
		}
	}
}
[System.Serializable]
public abstract class PersistentData { public PersistentData() {}}

public class SavedRules : PersistentData
{
	public List<Rule> rules;

	public class Rule
	{
		public string name;
		public int ruleForSurvival;
		public int ruleForReviving;
	}
}
