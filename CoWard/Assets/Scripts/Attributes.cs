using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class Attributes : MonoBehaviour
{
	[SerializeField] GameObject scoreObject, ratingObject;
	//[SerializeField] GameObject commonkitObject,deluxekitObject,expensivekitObject, commonkitObject2,deluxekitObject2,expensivekitObject2;
	Text scoreComponent, ratingComponent;//,deluxeComponent,expensiveComponent,commonComponent,deluxeComponent2,expensiveComponent2,commonComponent2;
	[SerializeField] GameObject discharge;
	Text dischargeText;
	public static int score = 50000;
	int num_discharged;
	public static float rating = 0f;

	[HideInInspector] public bool inventoryOn = false;

	public int GetScore(){
	    return score;
	}
	public float GetRating(){
	    return rating;
	}
	public IEnumerator UpdateScore(int delta_score, int disc){
        scoreComponent.text = (int.Parse(scoreComponent.text) + delta_score).ToString();
		if(disc!=2)
		{
	    score += delta_score;
		}
		if(disc!=0){
		dischargeText.text = delta_score.ToString();
		yield return new WaitForSeconds(0.5f);
		}
		dischargeText.text = "";
		
	}
	public void UpdateRating(float delta_rating){
		num_discharged+=1;
		rating=(rating*(num_discharged-1)+delta_rating)/(num_discharged);
		ratingComponent.text = (rating).ToString("0.00");
		
	}
	void Start(){
		rating = 0f;
		num_discharged= 0;
		scoreComponent = scoreObject.GetComponent<Text>();
		scoreComponent.text = score.ToString();
		dischargeText = discharge.GetComponent<Text>();
		ratingComponent = ratingObject.GetComponent<Text>();
		ratingComponent.text = rating.ToString();

	}
	void Update()
    {
    	
    }

}