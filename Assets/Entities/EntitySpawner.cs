using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntitySpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject entity;
    private List<GameObject> entities = new List<GameObject>();

    [SerializeField]
    public GameObject player;
    private Vector3 playerPos;

    [SerializeField]
    private float spawnRadiusInner;
    [SerializeField]
    private float spawnRadiusOuter;
    [SerializeField]
    private float spawnProbability;

    [SerializeField]
    private int maxEntitiesAllowed;

    private float timeElapsed;
    // Start is called before the first frame update
    void Start()
    {
        if (spawnRadiusInner > spawnRadiusOuter) // Makes sure Inner Radius is less than Outer Radius
        {
            float ri = spawnRadiusInner;
            float ro = spawnRadiusOuter;
            spawnRadiusInner = ro;
            spawnRadiusOuter = ri;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null)
        {
            Debug.Log("No player object attached");
            return;
        }
        playerPos = player.transform.position;
        timeElapsed += Time.deltaTime;
        RemoveNulls(entities);
        SpawnEntityNearPlayer(spawnRadiusInner, spawnRadiusOuter, spawnProbability);
        DespawnEntity();
    }

    private void RemoveNulls(List<GameObject> entities)
    {
        entities.RemoveAll(item => item == null);
    }

    // todo: implement dont spawn on top of another entity.
    private void SpawnEntityNearPlayer(float ri, float ro, float p)
    {
        int radiusDirection;
        if (entities.Count < maxEntitiesAllowed && timeElapsed > 1)
        {
            float radius = UnityEngine.Random.Range(ri, ro);
            var xz = GetXZFromRadius(radius);
            radiusDirection = UnityEngine.Random.Range(0, 2) * 2 - 1; // Returns -1 or 1
            float x = playerPos.x + xz.x * radiusDirection;
            radiusDirection = UnityEngine.Random.Range(0, 2) * 2 - 1; // Returns -1 or 1
            float z = playerPos.z + xz.z * radiusDirection;
            float y;
            try
            {
                y = GetGroundHeight(x, z);
            }
            catch (InvalidOperationException)
            {
                return;
            }

            float px = UnityEngine.Random.Range(0f, 1f);
            if (px <= p) // if the random number is less than the probability, spawn the entity. This will spawn an entity with probability p.
            {
                GameObject spawnedEntity = Instantiate(entity, new Vector3(x, y, z), Quaternion.identity);
                entities.Add(spawnedEntity);
            }

            timeElapsed = 0;
        }
        else
        {
            return;
        }
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

    private (float x, float z) GetXZFromRadius(float r)
    {
        float x, z;
        x = UnityEngine.Random.Range(0, r);
        z = Mathf.Sqrt(Mathf.Pow(r, 2) - Mathf.Pow(x, 2));
        return (x, z);
    }

    private void DespawnEntity()
    {
        foreach (GameObject entity in entities)
        {
            if (Vector3.Distance(entity.transform.position, playerPos) > 100f)
            {
                Destroy(entity);
            }
        }
    }
}
