using UnityEngine.UI;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    //Instance sebagai global access
    public static ScoreManager instance;
    int playerScore;
    public Text scoreText;

    // singleton
    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != null)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    //Update score dan ui
    public void GetScore(int point)
    {
        playerScore += point * ++streak;
        scoreText.text = playerScore.ToString();
    }

    public void ResetStreak()
    {
        streak = 1;
    }

    private int streak;
}