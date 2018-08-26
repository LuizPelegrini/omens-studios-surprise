using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudController : MonoBehaviour {

	private ObjectPool _cloudPool;

	private float _nextSpawnTime;
	public float minSpawnTime, maxSpawnTime;
	private float _minPosY, _maxPosY;

	// Use this for initialization
	void Start () 
	{
		_cloudPool = GetComponent<ObjectPool>();
		_nextSpawnTime = Time.timeSinceLevelLoad + Random.Range(1f, 2f);
		
		_minPosY = 0f;
		_maxPosY = CameraUtil.halfScreenHeightInWorldUnits;
	}
	
	// Update is called once per frame
	void Update () 
	{
		// Spawn Control
		if(Time.timeSinceLevelLoad > _nextSpawnTime)
		{
			_nextSpawnTime = Time.timeSinceLevelLoad + Random.Range(minSpawnTime, maxSpawnTime);
			Spawn();
		}
	}

	void Spawn()
	{
		GameObject cloudToSpawn = _cloudPool.GetPooledObject();
		cloudToSpawn.transform.position = new Vector3(transform.position.x, Random.Range(_minPosY, _maxPosY));

		// To prevent clouds moving based on camera
		cloudToSpawn.transform.SetParent(null);		// I am batman haha...
		cloudToSpawn.transform.rotation = Quaternion.identity;

		cloudToSpawn.SetActive(true);
	}

}
