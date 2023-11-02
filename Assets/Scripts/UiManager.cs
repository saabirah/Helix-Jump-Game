using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
  
   [SerializeField] private TextMeshProUGUI textBest;
   [SerializeField] private TextMeshProUGUI textScore;


    // Update is called once per frame
    void Update()
    {
        textBest.text = "Best: " + GameManager.singleton.best;
        textScore.text = "Score: " + GameManager.singleton.score;
        
    }
}
