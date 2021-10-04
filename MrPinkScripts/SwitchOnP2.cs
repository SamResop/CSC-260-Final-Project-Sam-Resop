using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchOnP2 : MonoBehaviour
{
    public GameObject P2Character;

    // Start is called before the first frame update
    void Start()
    {
        HealthState.Player2Load = P2Character;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
