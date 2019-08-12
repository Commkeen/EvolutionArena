using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupSpawnController : MonoBehaviour
{
    public static PowerupSpawnController Instance {get; private set;}

    public Powerup powerupPrefab;

    public int cooldownQueue = 2;
    public int powerupsOnBoard = 2;
    private List<PowerupSpawnLocation> _spawnLocations = new List<PowerupSpawnLocation>();

    private Queue<PowerupSpawnLocation> _cooldownQueue = new Queue<PowerupSpawnLocation>();


    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        _spawnLocations.AddRange(GameObject.FindObjectsOfType<PowerupSpawnLocation>());
        if (_spawnLocations.Count < cooldownQueue) {cooldownQueue = _spawnLocations.Count - 1;}
        ReplenishPowerups();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnPowerup()
    {
        var location = GetNextSpawnLocation();
        GameObject.Instantiate(powerupPrefab, location, Quaternion.identity);
    }

    public Vector2 GetPlayerSpawnLocation()
    {
        return GetNextSpawnLocation();
    }

    public void ClearPowerups()
    {
        var activePowerups = GameObject.FindObjectsOfType<Powerup>();
        foreach (var powerup in activePowerups)
        {
            GameObject.Destroy(powerup.gameObject);
        }
    }

    public void ReplenishPowerups()
    {
        for (var i = 0; i < powerupsOnBoard; i++)
        {
            SpawnPowerup();
        }
    }

    private Vector2 GetNextSpawnLocation()
    {
        var index = 0;
        PowerupSpawnLocation targetLocation = null;
        var freeLocation = false;
        var attempts = 0;
        while (!freeLocation && attempts < 50)
        {
            index = Random.Range(0,_spawnLocations.Count);
            targetLocation = _spawnLocations[index];
            freeLocation = IsSpawnLocationFree(targetLocation.transform.position);
            attempts++;
        }
        if (!freeLocation)
        {
            Debug.LogError("Failed to find a good spawn location!");
        }
        
        _spawnLocations.RemoveAt(index);
        _cooldownQueue.Enqueue(targetLocation);
        if (_cooldownQueue.Count > cooldownQueue)
        {
            var readyLocation = _cooldownQueue.Dequeue();
            _spawnLocations.Add(readyLocation);
        }
        return targetLocation.transform.position;
    }

    private bool IsSpawnLocationFree(Vector2 location)
    {
        var layerMask = LayerMask.GetMask("Players", "Default");
        var overlap = Physics2D.OverlapBox(location, new Vector2(1,1), 0, layerMask);
        if (overlap != null)
        {
            Debug.Log("Tried to spawn but found " + overlap.name + " at " + location);
        }
        return overlap == null;
    }
}
