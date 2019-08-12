using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RoundManager : MonoBehaviour
{
    public static RoundManager Instance {get; private set;}

    public float postgameTime = 10.0F;
    public int killsToWin = 5;

    public Text postgameTextField;

    private string _winnerName = "";
    private bool _inPostgame = false;
    private float _postgameTimer = 0.0F;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        postgameTextField.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePostgame();
    }

    public void EndRound(string winnerName)
    {
        _winnerName = winnerName;
        _inPostgame = true;
        postgameTextField.enabled = true;
        _postgameTimer = postgameTime;
        KillAllPlayers();
    }

    void UpdatePostgame()
    {
        if (!_inPostgame) {return;}
        _postgameTimer -= Time.deltaTime;
        if (_postgameTimer <= 0) {RestartGame();}
        int displayTime = Mathf.CeilToInt(_postgameTimer);
        postgameTextField.text = _winnerName + " wins!"
            + "\nNext round in " + displayTime + "...";
    }

    void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void KillAllPlayers()
    {
        var players = GameObject.FindObjectsOfType<PlayerController>();
        foreach (var player in players)
        {
            GameObject.Destroy(player.gameObject);
        }
    }
}
