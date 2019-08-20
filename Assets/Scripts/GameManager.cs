using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Text scoreText = null;
    private int score;

    // Start is called before the first frame update
    void Start()
    {
        score = 0;
    }

    private void updateScoreText()
    {
        scoreText.text = "Score: " + score;
    }

    // Update is called once per frame
    void Update()
    {
        score++;
        updateScoreText();
    }
}
