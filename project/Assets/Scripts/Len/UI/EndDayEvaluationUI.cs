using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndDayEvaluationUI : MonoBehaviour
{
    public Image[] accuracyStars;
    public Image[] speedStars;
    public Image[] reputationStars;

    public Button continueButton;

    public Sprite emptyStar;
    public Sprite fullStar;

    int starsAccuracy;
    int starsSpeed;
    int oldStarsReputation;
    int newstarsReputation;
    
    public float counter;
    public int currentStar;
    public int currentRepStar;
    public int currentGroup;
    public bool moveToNextGroup;
    public bool currentGroupFinished;
    public bool complete;

    public float subStarDelay;
    public float supStarDelay;
    public int growthDirection;

    public bool alreadyFiredEnd;

    private void Update()
    {
        if (complete)
        {
            counter = 0;
            currentStar = 0;
            currentRepStar = 0;
            currentGroup = 0;
            moveToNextGroup = false;
            currentGroupFinished = false;

            if (!alreadyFiredEnd)
            {
                if (GameEventManager.Instance.currentStoreReputation == 0)
                {
                    GameEventManager.Instance.PushToQueue(GameEventManager.GameEvent.GAME_OVER);
                }
                else if (GameEventManager.Instance.currentStoreReputation == 7)
                {
                    GameEventManager.Instance.PushToQueue(GameEventManager.GameEvent.GAME_WON);
                }
                else
                {
                    GameEventManager.Instance.PushToQueue(GameEventManager.GameEvent.BEGIN_DAY);
                }

                alreadyFiredEnd = true;
            }
            

            return;
        }

        SetStars();
    }

    public void ShowEndDayEvaluation()
    {
        complete = false;
        continueButton.interactable = false;
        alreadyFiredEnd = false;

        foreach (Image star in accuracyStars)
        {
            star.sprite = emptyStar;
        }

        foreach (Image star in speedStars)
        {
            star.sprite = emptyStar;
        }

        float totalDayAccuracy = GameEventManager.Instance.totalDayAccuracy / 
                                 GameEventManager.Instance.customersEachDay;

        float timeTarget = GameEventManager.Instance.timeDisplay.happyTime +
                           GameEventManager.Instance.timeDisplay.normalTime;

        float totalDaySpeed = 1.0f - (GameEventManager.Instance.totalDaySpeed /
                                       GameEventManager.Instance.customersEachDay) / timeTarget;

        starsAccuracy = (int)Mathf.Min(Mathf.Max(3.0f * totalDayAccuracy, 1), 3);
        starsSpeed = (int)Mathf.Min(Mathf.Max(3.0f * totalDaySpeed, 1), 3);

        oldStarsReputation = GameEventManager.Instance.currentStoreReputation;
        GameEventManager.Instance.currentStoreReputation += starsAccuracy + starsSpeed - 4;
        GameEventManager.Instance.currentStoreReputation = Mathf.Clamp(GameEventManager.Instance.currentStoreReputation, 0, 7);
        newstarsReputation = GameEventManager.Instance.currentStoreReputation;

        growthDirection = (int)Mathf.Min(Mathf.Max(newstarsReputation - oldStarsReputation, -1.0f), 1.0f);
        currentRepStar = oldStarsReputation + growthDirection + (growthDirection < 0 ? 0 : -1);

        for (int i = 0; i < reputationStars.Length; i++)
        {
            if (i < oldStarsReputation)
            {
                reputationStars[i].sprite = fullStar;
            }
            else
            {
                reputationStars[i].sprite = emptyStar;
            }
        }

        gameObject.SetActive(true);
    }

    private void SetStarsAccuracy()
    {
        counter += Time.deltaTime;

        if (counter > subStarDelay)
        {
            accuracyStars[currentStar].sprite = fullStar;
            counter = 0.0f;

            if (++currentStar == starsAccuracy)
            {
                currentGroupFinished = true;
                currentStar = 0;
            }
        }
    }

    public void SetStarsSpeed()
    {
        counter += Time.deltaTime;

        if (counter > subStarDelay)
        {
            speedStars[currentStar].sprite = fullStar;
            counter = 0.0f;

            if (++currentStar == starsSpeed)
            {
                currentGroupFinished = true;
                currentStar = 0;
            }
        }
    }

    public void SetStarsReputation()
    {
        counter += Time.deltaTime;

        if (counter > subStarDelay)
        {
            reputationStars[currentRepStar].sprite = growthDirection > 0 ? fullStar : emptyStar;
            counter = 0.0f;

            currentRepStar += growthDirection;

            if (currentRepStar == newstarsReputation + (growthDirection < 0 ? -1 : 0))
            {
                currentGroupFinished = true;
            }
        }
    }

    public void SetStars()
    {
        if (currentGroupFinished)
        {
            counter += Time.deltaTime;

            if (counter > supStarDelay)
            {
                counter = 0.0f;
                currentGroup++;

                if (currentGroup > 2)
                {
                    continueButton.interactable = true;
                    complete = true;
                    return;
                }

                currentGroupFinished = false;
            }
        }
        else
        {
            switch (currentGroup)
            {
                case 0:
                    {
                        SetStarsAccuracy();
                        break;
                    }
                case 1:
                    {
                        SetStarsSpeed();
                        break;
                    }
                case 2:
                    {
                        if (growthDirection == 0)
                        {
                            continueButton.interactable = true;
                            complete = true;
                        }
                        else
                        {
                            SetStarsReputation();
                        }

                        break;
                    }
            }
        }
    }
}
