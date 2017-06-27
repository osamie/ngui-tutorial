
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using Zynga.Casino.Unity;

/// <summary>
/// This script will play an animation with a random speed multiplier.  
/// </summary>
[RequireComponent(typeof(Animation))]
public class VariableSpeedAnimationComponent : Behavior
{
    [SerializeField]
    private bool shouldPlayAnimationOnEnable = true;
    [SerializeField]
    private Vector2 animationSpeedRandomRange = new Vector2(0f, 2f); 

    private float rate = 1f; 

    private Animation animationComponent;

    private void OnEnable()
    {
        if(this.shouldPlayAnimationOnEnable)
        {
            this.animationComponent = this.GetComponent<Animation>();
            if(this.animationComponent.clip == null)
            {
                Debug.LogWarning(this.name + " no animation clip found.  Disabling."); 
                this.enabled = false; 
            }
            StartCoroutine(this.StartAnimationLoop());
        }
    }

    public IEnumerator PlayAnimation() 
    {
        foreach(AnimationState animState in this.animationComponent)
        {
            animState.speed = rate;
        }
        this.animationComponent.Play();
        yield return new WaitForSeconds(this.animationComponent.clip.length * rate);

    }

    public IEnumerator StartAnimationLoop()
    {  
        while(this.gameObject.activeSelf)
        {
            rate = Random.Range(this.animationSpeedRandomRange.x, this.animationSpeedRandomRange.y);
            StartCoroutine(this.PlayAnimation());
            yield return new WaitForSeconds(this.animationComponent.clip.length * rate); 
        }
    }
}
