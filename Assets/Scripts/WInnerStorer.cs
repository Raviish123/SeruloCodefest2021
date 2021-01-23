using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WInnerStorer : MonoBehaviour
{

    public static string winnerName;

    public Text winnerText;

    // Start is called before the first frame update
    void Start()
    {
        winnerText.text = "Winner: " + winnerName;
    }

    public void Return()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Menu");
    }
}
