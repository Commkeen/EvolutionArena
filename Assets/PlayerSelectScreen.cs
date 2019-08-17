using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class PlayerSelectScreen : MonoBehaviour
{
    public Text[] playerStatusText = null;
    public Text[] playerNamesText = null;
    public Image[] playerImage = null;
    public Material grayscaleShader = null;

    private readonly Color disabledColor = new Color(0.5f, 0.5f, 0.5f, 0.25f);

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i< 4; i++)
        {
            GameSettings.PlayerIsActive[i] = false;
            playerNamesText[i].text = GameSettings.PlayerNames[i];
            // playerImage[i].color = disabledColor;
            playerImage[i].material = grayscaleShader;

        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < 4; i++)
        {
            int controlnum = i + 1;
            if (Input.GetButtonDown("P" + controlnum + "Jump"))
            {
                GameSettings.PlayerIsActive[i] = true;
                playerStatusText[i].text = "Ready";
                playerImage[i].material = null;
            }

            if (Input.GetButtonDown("P" + controlnum + "Action1"))
            {
                GameSettings.PlayerIsActive[i] = false;
                playerStatusText[i].text = "Press A to Join";
                playerImage[i].material = grayscaleShader;
            }

        }

        if (Input.GetButtonDown("GameStart") && GameSettings.PlayerIsActive.Count<bool>(a => a == true) > 1)
            UnityEngine.SceneManagement.SceneManager.LoadScene("SampleScene");


    }
}
