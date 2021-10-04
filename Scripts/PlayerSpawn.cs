using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawn : MonoBehaviour
{
    private GameObject Player1;
    private GameObject Player2;
    public GameObject Player1Character;
    public GameObject Player2Character;
    public Transform Player1Spawn;
    public Transform Player2Spawn;

    // Start is called before the first frame update
    void Start()
    { 
        Player1 = GameObject.Find(HealthState.P1Select);
        Player1.gameObject.GetComponent<SwitchOnP1>().enabled = true;
        Player2 = GameObject.Find(HealthState.P2Select);
        Player2.gameObject.GetComponent<SwitchOnP2>().enabled = true;
        StartCoroutine(SpawnPlayers());
    }

    IEnumerator SpawnPlayers()
    {
        yield return new WaitForSeconds(0.1f);
        Player1Character = HealthState.Player1Load;
        Player2Character = HealthState.Player2Load;
        Instantiate(Player1Character, Player1Spawn.position, Player1Spawn.rotation);
        Instantiate(Player2Character, Player2Spawn.position, Player2Spawn.rotation);
    }
}
