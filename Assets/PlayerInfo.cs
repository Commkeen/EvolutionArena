using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
    public string HorizontalAxisName = "Horizontal";
    public string JumpButtonName = "Jump";
    public string Action1ButtonName = "Action1";
    public string Action2ButtonName = "Action2";

    public string Name = "Mouse";
    public int PlayerIndex = 0;
    public int Kills = 0;

    // Start is called before the first frame update
    void Start()
    {
        Name = GameSettings.PlayerNames[PlayerIndex];
        if (!GameSettings.PlayerIsActive[PlayerIndex])
            gameObject.SetActive(false);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LogKill()
    {
        Kills++;
        if (Kills >= RoundManager.Instance.killsToWin)
        {
            RoundManager.Instance.EndRound(Name);
        }
    }
}
