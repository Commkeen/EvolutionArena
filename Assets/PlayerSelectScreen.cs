using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class PlayerSelectScreen : MonoBehaviour
{
    public Text[] playerStatusText = null;
    public Text[] playerNamesText = null;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i< 4; i++)
        {
            GameSettings.PlayerIsActive[i] = false;
            playerNamesText[i].text = GameSettings.PlayerNames[i];

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
            }

            if (Input.GetButtonDown("P" + controlnum + "Action1"))
            {
                GameSettings.PlayerIsActive[i] = false;
                playerStatusText[i].text = "Press A to Join";
            }

        }

        if (Input.GetButtonDown("GameStart") && GameSettings.PlayerIsActive.Count<bool>(a => a == true) > 0)
            UnityEngine.SceneManagement.SceneManager.LoadScene("SampleScene");


    }
}
