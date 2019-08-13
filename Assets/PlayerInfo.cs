using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfo : MonoBehaviour
{
    public string HorizontalAxisName = "Horizontal";
    public string JumpButtonName = "Jump";
    public string Action1ButtonName = "Action1";
    public string Action2ButtonName = "Action2";

    public string Name = "Mouse";
    public int PlayerIndex = 0;
    public int Kills = 0;

    public GameObject ScorePanel = null;
    public Text textScore = null;

    // Start is called before the first frame update
    void Start()
    {
        Name = GameSettings.PlayerNames[PlayerIndex];

        if (Time.realtimeSinceStartup > 3.0f)
        {
            if (!GameSettings.PlayerIsActive[PlayerIndex])
            {
                gameObject.SetActive(false);
                ScorePanel.SetActive(false);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LogKill()
    {
        Kills++;
        textScore.text = Kills.ToString();
        if (Kills >= RoundManager.Instance.killsToWin)
        {
            RoundManager.Instance.EndRound(Name);
        }
    }
}
