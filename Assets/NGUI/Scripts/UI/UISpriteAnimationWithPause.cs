
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// This is an Updated Version of  UISpriteAnimation, wich alows you to pause a loop animation for a set amount of time.
/// Very simple sprite animation. Attach to a sprite and specify a common prefix such as "idle" and it will cycle through them.
/// </summary>

[ExecuteInEditMode]
[RequireComponent(typeof(UISprite))]
[AddComponentMenu("NGUI/UI/Sprite Animation With Pause")]
public class UISpriteAnimationWithPause : MonoBehaviour
{
    [SerializeField]protected int FPS = 30;
    [SerializeField]protected string Prefix = "";
    [SerializeField]protected bool Loop = true;
    [SerializeField]protected bool Snap = true;
    [SerializeField]protected float PauseAnimationTime = 0.1f;
    [SerializeField]protected bool UseRandomAnimationTime = false;
    [SerializeField]protected Vector2 SetRandomPauseRange = new Vector2(1, 2);

    protected UISprite mSprite;
    protected float mDelta = 0f;
    protected int mIndex = 0;
    protected bool mActive = true;
    protected List<string> mSpriteNames = new List<string>();

    /// <summary>
    /// Returns is the animation is still playing or not
    /// </summary>

    public bool isPlaying { get { return mActive; } }

    /// <summary>
    /// Rebuild the sprite list first thing.
    /// </summary>

    protected virtual void Start()
    {
        RebuildSpriteList();
    }

    /// <summary>
    /// Advance the sprite animation process.
    /// </summary>

    protected virtual void Update()
    {
        if(mActive && mSpriteNames.Count > 1 && Application.isPlaying && FPS > 0)
        {
            mDelta += RealTime.deltaTime;
            float rate = 1f / FPS;

            if(rate < mDelta)
            {
                mDelta = (rate > 0f) ? mDelta - rate : 0f;

                if(++mIndex >= mSpriteNames.Count)
                {
                    if(PauseAnimationTime > 0)
                    {
                        if(UseRandomAnimationTime)
                        {
                            PauseAnimationTime = Random.Range(SetRandomPauseRange.x, SetRandomPauseRange.y);
                        }
                        StartCoroutine(PauseAnimation(PauseAnimationTime));
                    }
                    else
                    {
                        mIndex = 0;
                        mActive = Loop;
                    }
                }

                if(mActive)
                {
                    if(!mSprite.enabled)
                    {
                        mSprite.enabled = true;
                    }

                    mSprite.spriteName = mSpriteNames[mIndex];
                    if(Snap)
                    {
                        mSprite.MakePixelPerfect();
                    }
                }
            }
        }
    }

    /// <summary>
    /// Rebuild the sprite list after changing the sprite name.
    /// </summary>

    public void RebuildSpriteList()
    {
        if(mSprite == null)
        {
            mSprite = GetComponent<UISprite>();
        }
        mSpriteNames.Clear();

        if(mSprite != null && mSprite.atlas != null)
        {
            List<UISpriteData> sprites = mSprite.atlas.spriteList;

            for(int i = 0, imax = sprites.Count; i < imax; ++i)
            {
                UISpriteData sprite = sprites[i];

                if(string.IsNullOrEmpty(Prefix) || sprite.name.StartsWith(Prefix))
                {
                    mSpriteNames.Add(sprite.name);
                }
            }
            mSpriteNames.Sort();
        }
    }
    
    /// <summary>
    /// Reset the animation to the beginning.
    /// </summary>

    public void Play()
    {
        mActive = true;
    }

    /// <summary>
    /// Pause the animation.
    /// </summary>

    public void Pause()
    {
        mActive = false;
    }

    /// <summary>
    /// Reset the animation to frame 0 and activate it.
    /// </summary>

    public void ResetToBeginning()
    {
        mActive = true;
        mIndex = 0;

        if(mSprite != null && mSpriteNames.Count > 0)
        {
            mSprite.spriteName = mSpriteNames[mIndex];
            if(Snap)
            {
                mSprite.MakePixelPerfect();
            }
        }
    }

    /// <summary>
    /// Pause the animation, and disable the ngui UI sprite renderer, enableing after wait.
    /// </summary>

    private IEnumerator PauseAnimation(float howLongToPause)
    {   
        Pause();
        mSprite.enabled = false;
        yield return new WaitForSeconds(howLongToPause);
        mIndex = 0;
        mActive = Loop;
    }
}
