using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class ScoreManagerScript : MonoBehaviour
{
    public static ScoreManagerScript instance;
    [SerializeField] TextMeshProUGUI scoreDisplay;
    [SerializeField] int score = 0;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        displayScore(this.score);
    }

    private void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void displayScore(int score)
    {
        this.score += score;
        scoreDisplay.text = $"Score : {this.score}";
    }
}
