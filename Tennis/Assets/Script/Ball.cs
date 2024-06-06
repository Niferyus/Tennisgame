using JetBrains.Rider.Unity.Editor;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Ball : MonoBehaviour
{
    Vector3 initialPos;
    public string hitter;

    int playerScore;
    int botScore;

    public Text playerScoretext;
    public Text botScoretext;

    public bool playing = true;
    // Start is called before the first frame update
    void Start()
    {
        initialPos = transform.position;
        playerScore = 0;
        botScore = 0;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.CompareTag("Wall"))
        {
            
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            //transform.position = initialPos;

            GameObject.Find("player").GetComponent<Player>().Reset();

            if (playing)
            {
                if (hitter == "player")
                {
                    playerScore++;
                }
                else if (hitter == "bot")
                {
                    botScore++;
                }
                playing = false;
                updateScores();
            }
           

        }

        else if (collision.transform.CompareTag("Out"))
        {

            GetComponent<Rigidbody>().velocity = Vector3.zero;
            GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
          

            GameObject.Find("player").GetComponent<Player>().Reset();

            if (playing)
            {
                if (hitter == "bot")
                {
                    playerScore++;
                }
                else if (hitter == "player")
                {
                    botScore++;
                }
                playing = false;
                updateScores();
            }


        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Out") && playing)
        {
            if(hitter == "player")
            {
                botScore++;
            }
            else if(hitter == "bot")
            {
                playerScore++;
            }
            playing=false;
            updateScores();
        }
    }

    void updateScores()
    {
        playerScoretext.text = "Player: " + playerScore;
        botScoretext.text = "Bot: " + botScore;
    }
}
