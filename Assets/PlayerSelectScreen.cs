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

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i< 4; i++)
        {
            GameSettings.PlayerIsActive[i] = false;
            playerNamesText[i].text = GameSettings.PlayerNames[i];
            playerImage[i].color = Color.grey;

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
                playerImage[i].color = Color.white;
            }

            if (Input.GetButtonDown("P" + controlnum + "Action1"))
            {
                GameSettings.PlayerIsActive[i] = false;
                playerStatusText[i].text = "Press A to Join";
                playerImage[i].color = Color.grey;
            }

        }

        if (Input.GetButtonDown("GameStart") && GameSettings.PlayerIsActive.Count<bool>(a => a == true) > 1)
            UnityEngine.SceneManagement.SceneManager.LoadScene("SampleScene");


    }
}
