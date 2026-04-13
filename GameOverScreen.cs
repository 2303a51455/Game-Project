using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverScreen : MonoBehaviour
{
    public Text score;
    public void Setup(int time)
    {
        gameObject.SetActive(true);
        score.text = time.ToString() + " SECONDS";
    }
}
