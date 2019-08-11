using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupSpawnController : MonoBehaviour
{
    public static PowerupSpawnController Instance {get; private set;}

    public Powerup powerupPrefab;

    public int cooldownQueue = 2;

    private List<PowerupSpawnLocation> _spawnLocations = new List<PowerupSpawnLocation>();

    private Queue<PowerupSpawnLocation> _cooldownQueue = new Queue<PowerupSpawnLocation>();


    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        _spawnLocations.AddRange(GameObject.FindObjectsOfType<PowerupSpawnLocation>());
        if (_spawnLocations.Count < cooldownQueue) {cooldownQueue = _spawnLocations.Count - 1;}
        SpawnPowerup();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnPowerup()
    {
        var index = Random.Range(0,_spawnLocations.Count);
        var targetLocation = _spawnLocations[index];
        GameObject.Instantiate(powerupPrefab, targetLocation.transform.position, Quaternion.identity);

        // Put the location where we did the spawn in the cooldown queue
        _spawnLocations.RemoveAt(index);
        _cooldownQueue.Enqueue(targetLocation);

        // Dequeue the next location that's ready to be put back into randomization
        if (_cooldownQueue.Count > cooldownQueue)
        {
            var readyLocation = _cooldownQueue.Dequeue();
            _spawnLocations.Add(readyLocation);
        }
    }
}
