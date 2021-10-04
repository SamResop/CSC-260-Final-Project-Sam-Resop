using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class P2Select : MonoBehaviour
{
    public int MaxIcons= 2;
    public int IconsPerRow = 2;
    public int MaxRows = 1;
    public int IconNumber = 1;
    private int RowNumber = 1;
    private float PauseTime = 1.0f;
    private bool TimeCountDown = false;
    private bool ChangeCharacter = false;

    public GameObject MrRedP2;
    public GameObject MrPinkP2;

    public GameObject MrRedP2Text;
    public GameObject MrPinkP2Text;

    public TextMeshProUGUI Player2Name;

    private string CharacterSelectionP2;
    public static bool SelectedP2 = false;

    // Start is called before the first frame update
    void Start()
    {
        ChangeCharacter = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!SelectedP2)
        {
            if (ChangeCharacter)
            {
                switch (IconNumber)
                {
                    case 1:
                        SwitchOff();
                        MrRedP2.gameObject.SetActive(true);
                        MrRedP2Text.gameObject.SetActive(true);
                        Player2Name.text = "Mr. Red";
                        CharacterSelectionP2 = "MrRedP2";
                        ChangeCharacter = false;
                        break;
                    case 2:
                        SwitchOff();
                        MrPinkP2.gameObject.SetActive(true);
                        MrPinkP2Text.gameObject.SetActive(true);
                        Player2Name.text = "Mr. Pink";
                        CharacterSelectionP2 = "MrPinkP2";
                        ChangeCharacter = false;
                        break;
                }
            }

            //Actually select the character
            if (Input.GetButtonDown("LightAtkP2"))
            {
                HealthState.P2Select = CharacterSelectionP2;
                SelectedP2 = true;
                if (P1Select.SelectedP1)
                {
                    SceneManager.LoadScene(2);
                    SelectedP2 = false;
                    P1Select.SelectedP1 = false;
                }
            }

            //Slow down selection speed
            if (TimeCountDown)
            {
                if (PauseTime > 0.1f)
                {
                    PauseTime -= 1.0f * Time.deltaTime;
                }
                if (PauseTime <= 0.1f)
                {
                    PauseTime = 1.0f;
                    TimeCountDown = false;
                }
            }
            //Moving left and right on character select
            if (Input.GetAxis("HorizontalP2") > 0)
            {
                if (PauseTime == 1.0f)
                {
                    if (IconNumber < IconsPerRow * RowNumber)
                    {
                        IconNumber++;
                        TimeCountDown = true;
                        ChangeCharacter = true;
                    }
                }
            }
            if (Input.GetAxis("HorizontalP2") < 0)
            {
                if (PauseTime == 1.0f)
                {
                    if (IconNumber > (IconsPerRow * (RowNumber - 1)) + 1)
                    {
                        IconNumber--;
                        TimeCountDown = true;
                        ChangeCharacter = true;
                    }
                }
            }
            //Moving up and down on character select - NOT IN USE YET
            if (Input.GetAxis("VerticalP2") < 0)
            {
                if (PauseTime == 1.0f)
                {
                    if (RowNumber < MaxRows)
                    {
                        IconNumber += IconsPerRow;
                        RowNumber++;
                        TimeCountDown = true;
                        ChangeCharacter = true;
                    }
                }
            }
            if (Input.GetAxis("VerticalP2") > 0)
            {
                if (PauseTime == 1.0f)
                {
                    if (RowNumber > 1)
                    {
                        IconNumber -= IconsPerRow;
                        RowNumber--;
                        TimeCountDown = true;
                        ChangeCharacter = true;
                    }
                }
            }
        }
        if (Input.GetButton("HeavyAtkP2"))
        {
            SelectedP2 = false;
        }
    }

    void SwitchOff()
    {
        MrRedP2.gameObject.SetActive(false);
        MrPinkP2.gameObject.SetActive(false);
        MrRedP2Text.gameObject.SetActive(false);
        MrPinkP2Text.gameObject.SetActive(false);
    }
}
