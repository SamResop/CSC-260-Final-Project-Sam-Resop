using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthState : MonoBehaviour
{

    public static float Player1Health = 1.0f;
    public static float Player2Health = 1.0f;
    public static float Player1Timer = 2.0f;
    public static float Player2Timer = 2.0f;
    public static bool Player1Mode = true;
    public static string P1Select;
    public static string P2Select;
    public static GameObject Player1Load;
    public static GameObject Player2Load;

    // Start is called before the first frame update
    void Start()
    {
        Player1Health = 1.0f;
        Player2Health = 1.0f;
        //P1Select = "MrPinkP1";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
