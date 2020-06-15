using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    [SerializeField]
    Terrain terrain;
    [SerializeField]
    private GameObject player;
    GameObject instantiatedPlayer; // mayber a list for multiplayer?

    [SerializeField]
    List<EntitySpawner> spawners = new List<EntitySpawner>();

    List<EntitySpawner> instantiatedSpawners = new List<EntitySpawner>();

    CameraScript camScript;

    // Start is called before the first frame update
    void Start()
    {
        foreach (EntitySpawner spawner in spawners)
        {
            InstantiateSpawner(spawner);
        }
        camScript = Camera.main.GetComponent<CameraScript>();
    }

    private void InstantiateSpawner(EntitySpawner spawner)
    {
        instantiatedSpawners.Add(Instantiate(spawner));
    }

    private void InstantiatePlayer()
    {
        Vector3 pos = GetRandomPosInTerrain(terrain);
        instantiatedPlayer = Instantiate(player, pos, Quaternion.identity);
        camScript.target = instantiatedPlayer.transform.GetChild(0);
        camScript.Start();

        foreach(EntitySpawner spawner in instantiatedSpawners)
        {
            spawner.player = instantiatedPlayer;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (instantiatedPlayer == null)
        {
            instantiatedPlayer = new GameObject();
            Invoke("InstantiatePlayer", 2);
        }
    }

    private Vector3 GetRandomPosInTerrain(Terrain terrain)
    {
        Vector3 pos;
        Vector3 minBounds = terrain.terrainData.bounds.min;
        Vector3 maxBounds = terrain.terrainData.bounds.max;
        float x = UnityEngine.Random.Range(minBounds.x + 200, maxBounds.x - 200);
        float z = UnityEngine.Random.Range(minBounds.z + 200, maxBounds.z - 200);
        float y;
        try
        {
            y = GetGroundHeight(x, z);
            pos = new Vector3(x, y + 2f, z);
        }
        catch (InvalidOperationException)
        {
            pos = GetRandomPosInTerrain(terrain);
        }
        return pos;
    }

    private float GetGroundHeight(float x, float z) //todo: put this in a utilities script
    {
        RaycastHit hit;
        Vector3 position = new Vector3(x, 0, z);
        if (Physics.Raycast(position + new Vector3(0, 100.0f, 0), Vector3.down, out hit, 200.0f))
        {
            return hit.point.y;
        }
        else
        {
            Debug.Log("there seems to be no ground at this position");
            throw new InvalidOperationException();
        }
    }
}
