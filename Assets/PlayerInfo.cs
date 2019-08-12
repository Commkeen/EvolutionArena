using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
    public string HorizontalAxisName = "Horizontal";
    public string JumpButtonName = "Jump";
    public string Action1ButtonName = "Action1";
    public string Action2ButtonName = "Action2";

    public int PlayerIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        if (!GameSettings.PlayerIsActive[PlayerIndex])
            gameObject.SetActive(false);

        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
