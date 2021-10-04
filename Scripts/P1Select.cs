using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class P1Select : MonoBehaviour
{
    public int MaxIcons= 2;
    public int IconsPerRow = 2;
    public int MaxRows = 1;
    public int IconNumber = 1;
    private int RowNumber = 1;
    private float PauseTime = 1.0f;
    private bool TimeCountDown = false;
    private bool ChangeCharacter = false;

    public GameObject MrRedP1;
    public GameObject MrPinkP1;

    public GameObject MrRedP1Text;
    public GameObject MrPinkP1Text;

    public TextMeshProUGUI Player1Name;

    public string CharacterSelectionP1;
    public static bool SelectedP1 = false;

    // Start is called before the first frame update
    void Start()
    {
        ChangeCharacter = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!SelectedP1)
        {
            if (ChangeCharacter)
            {
                switch (IconNumber)
                {
                    case 1:
                        SwitchOff();
                        MrRedP1.gameObject.SetActive(true);
                        MrRedP1Text.gameObject.SetActive(true);
                        Player1Name.text = "Mr. Red";
                        CharacterSelectionP1 = "MrRedP1";
                        ChangeCharacter = false;
                        break;
                    case 2:
                        SwitchOff();
                        MrPinkP1.gameObject.SetActive(true);
                        MrPinkP1Text.gameObject.SetActive(true);
                        Player1Name.text = "Mr. Pink";
                        CharacterSelectionP1 = "MrPinkP1";
                        ChangeCharacter = false;
                        break;
                }
            }

            //Actually select the character
            if (Input.GetButtonDown("Fire1"))
            {
                HealthState.P1Select = CharacterSelectionP1;
                SelectedP1 = true;
                if (P2Select.SelectedP2)
                {
                    SceneManager.LoadScene(2);
                    SelectedP1 = false;
                    P2Select.SelectedP2 = false;
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
            if (Input.GetAxis("Horizontal") > 0)
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
            if (Input.GetAxis("Horizontal") < 0)
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
            if (Input.GetAxis("Vertical") < 0)
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
            if (Input.GetAxis("Vertical") > 0)
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
        if (Input.GetButton("Fire2"))
        {
            SelectedP1 = false;
        }
    }

    void SwitchOff()
    {
        MrRedP1.gameObject.SetActive(false);
        MrPinkP1.gameObject.SetActive(false);
        MrRedP1Text.gameObject.SetActive(false);
        MrPinkP1Text.gameObject.SetActive(false);
    }
}
