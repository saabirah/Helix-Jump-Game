using System.Collections;
using System.Collections.Generic;
using UnityEditor.Advertisements;
using UnityEngine;
using UnityEngine.Advertisements;

public class GameManager : MonoBehaviour
{
    public int best;
    public int currentStage = 0;
    public int score;

    public static GameManager singleton;


    // Start is called before the first frame update
    void Awake()
    {
       // Advertisement.Initialize("id");
        if(singleton == null)
        {
            singleton = this;
        }else if (singleton != this) {
            Destroy(gameObject);
        }

        //  Load the saved highscore
        best = PlayerPrefs.GetInt("Hightscore");
    }


    public void NextLevel()
    {
        currentStage++;
        FindObjectOfType<BallController>().ResetBall();
        FindObjectOfType<HelixController>().LoadStage(currentStage);
        Debug.Log("Next level called");
    }


    public void RestartLevel()
    {
        Debug.Log("Game Over");

        //show Ads
        // copier l'Id
       // Advertisement.Show();


        singleton.score = 0;

        FindObjectOfType<BallController>().ResetBall();

        // Reload the stages
        FindObjectOfType<HelixController>().LoadStage(currentStage);

        
    }


    public void AddScore(int scoreToAdd)
    {
        score += scoreToAdd;

        if(score > best) 
        { 
            best = score;

            //stocker le score le plus élevé et le meilleur score dans les préférences du joueurs
            PlayerPrefs.SetInt("Hightscore", score);
        }
    }
    
    // date is called once per frame
    void Update()
    {
        
    }
}
