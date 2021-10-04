using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchOnP1 : MonoBehaviour
{
    public GameObject P1Character;

    // Start is called before the first frame update
    void Start()
    {
        HealthState.Player1Load = P1Character;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
