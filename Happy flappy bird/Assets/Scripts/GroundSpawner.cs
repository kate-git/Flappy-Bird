using UnityEngine;

public class GroundSpawner : MonoBehaviour 
{
public GameObject groundTile;
Vector3 nextSpawnPoint;

void SpawnTile () 
{
GameObject temp = Instantiate(groundTile, nextSpawnPoint, Quaternion.identity);
nextSpawnPoint = temp.transform.GetChild(4).transform.position;
}

private void Start () {
SpawnTile();
}
    
}
