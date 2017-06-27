
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using Zynga.Casino.Unity;

/// <summary>
/// This script will loop an animation between a random pause range
/// </summary>
[RequireComponent(typeof(Animation))]
public class LoopAnimationWithPause : Behavior
{
    [SerializeField]
    private Vector2 pauseAnimationTimeRandomRange = new Vector2(1, 2);
    [SerializeField]
    private bool shouldPlayAnimationOnEnable = true;
    private Animation animationComponent;

    private void OnEnable()
    {
        if(this.shouldPlayAnimationOnEnable)
        {
            this.animationComponent = this.GetComponent<Animation>();
            StartCoroutine(this.StartAnimationLoop());
        }
    }

    public IEnumerator PlayAnimation() 
    {
        this.animationComponent.Play();
        yield return new WaitForSeconds(this.animationComponent.clip.length);
        this.animationComponent.Rewind();
        this.animationComponent.Stop();
    }

    public IEnumerator StartAnimationLoop()
    {  
        while(this.gameObject.activeSelf)
        {
            StartCoroutine(this.PlayAnimation());
            yield return new WaitForSeconds(Random.Range(this.pauseAnimationTimeRandomRange.x, this.pauseAnimationTimeRandomRange.y));
        }
    }
}
