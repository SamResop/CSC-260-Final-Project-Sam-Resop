using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBars : MonoBehaviour
{
    public Image LeftYellow;
    public Image LeftRed;
    public Image RightYellow;
    public Image RightRed;
    public GameObject P1Victory;
    public GameObject P2Victory;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        LeftYellow.fillAmount = HealthState.Player1Health;
        RightYellow.fillAmount = HealthState.Player2Health;

        if(HealthState.Player2Timer > 0)
        {
            HealthState.Player2Timer -= 2.0f * Time.deltaTime;
        }

        if(HealthState.Player2Timer <= 0)
        {
            if(RightRed.fillAmount > HealthState.Player2Health)
            {
                RightRed.fillAmount -= 0.003f;
            }
        }
        if (HealthState.Player1Timer > 0)
        {
            HealthState.Player1Timer -= 2.0f * Time.deltaTime;
        }

        if (HealthState.Player1Timer <= 0)
        {
            if (LeftRed.fillAmount > HealthState.Player1Health)
            {
                LeftRed.fillAmount -= 0.003f;
            }
        }
        if (HealthState.Player1Health <= 0)
        {
            P2Victory.SetActive(true);
        }
        else if (HealthState.Player2Health <= 0)
        {
            P1Victory.SetActive(true);
        }
        else
        {
            P1Victory.SetActive(false);
            P2Victory.SetActive(false);
        }
    }
}
