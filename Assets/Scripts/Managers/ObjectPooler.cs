using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class ObjectToPoolItem{

	public int amountToPool;

	public GameObject objectToPool;

	public bool shouldExpand;
}


public class ObjectPooler : MonoBehaviour {

	public static ObjectPooler sharedInstance;

	public List<ObjectToPoolItem> objectToPoolItemList = new List<ObjectToPoolItem> ();	

	List<GameObject> pooledObjectsList = new List<GameObject> ();

	List<GameObject> pooledObjectsListByTag = new List<GameObject> ();

	void Awake() {
		sharedInstance = this;
	}

	// Use this for initialization
	void Start () {
		foreach(ObjectToPoolItem objToPoolItem in objectToPoolItemList){

			for(int i = 0;i < objToPoolItem.amountToPool;i++){
				GameObject pooledObject = GameObject.Instantiate (objToPoolItem.objectToPool);
				pooledObject.SetActive (false);
				pooledObjectsList.Add(pooledObject);
			}
		}
	}

	//从对象池中获取未激活的对象
	public GameObject GetPooledObjectByTag(string tag){

		foreach(GameObject pooledObject in pooledObjectsList){
			if (tag.Equals(pooledObject.tag) && !pooledObject.activeInHierarchy) {
				return pooledObject;
			}
		}
			
		foreach(ObjectToPoolItem objToPoolItem in objectToPoolItemList){

			if(tag.Equals (objToPoolItem.objectToPool.tag) && objToPoolItem.shouldExpand){

				GameObject pooledObject = GameObject.Instantiate (objToPoolItem.objectToPool);
				pooledObject.SetActive (false);
				pooledObjectsList.Add(pooledObject);

				objToPoolItem.amountToPool++;

				return pooledObject;
			}

		}

		return null;
	}

	public List<GameObject> GetPooledObjectsList()
	{
		return pooledObjectsList;
	}

	public List<GameObject> GetPooledObjectsListByTag(string tagName)
	{
		if (tagName == null || "".Equals (tagName)) {
			return GetPooledObjectsList ();
		}

		pooledObjectsListByTag.RemoveRange (0,pooledObjectsListByTag.Count);

		foreach(GameObject pooledObject in pooledObjectsList){
			if (tagName.Contains(pooledObject.tag)){
				pooledObjectsListByTag.Add(pooledObject);
			}
		}

		return pooledObjectsListByTag;
	}
}
