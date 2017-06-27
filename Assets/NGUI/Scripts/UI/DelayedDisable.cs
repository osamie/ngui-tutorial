using UnityEngine;
using System.Collections;

public class DelayedDisable : MonoBehaviour {


    public float waitTimeBeforeDisabling = 1f; 

    private float timeRemaining; 


	void Start () 
    {
	    timeRemaining = waitTimeBeforeDisabling; 
	}
	
    void OnEnable()
    {
        timeRemaining = waitTimeBeforeDisabling; 
    }

	void Update () 
    {
	    
        timeRemaining -= Time.deltaTime; 

        if(timeRemaining <= 0)
        {
            this.gameObject.SetActive(false); 
        }
        

	}
}
