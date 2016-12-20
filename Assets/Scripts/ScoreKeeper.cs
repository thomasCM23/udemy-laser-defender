using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScoreKeeper : MonoBehaviour {

	public static int score = 0;
	Text myText;
	void Start()
	{
		myText = GetComponent<Text>();
	}
	
	public void Score (int points)
	{
		score += points;
		myText.text = "Score: "+ score;
	}
	public static void Reset()
	{
		score = 0;
		//myText.text = "Score: "+ score;
	}
	
}
