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

            return;
        }

        SetStars();
    }

    public void ShowEndDayEvaluation()
    {
        complete = false;
        continueButton.interactable = false;

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

        float timeTarget = GameEventManager.Instance.timeDisplay.greenTime +
                           GameEventManager.Instance.timeDisplay.yellowTime;

        float totalDaySpeed = 1.0f - (GameEventManager.Instance.totalDaySpeed /
                                       GameEventManager.Instance.customersEachDay) / timeTarget;

        starsAccuracy = (int)Mathf.Max(3.0f * totalDayAccuracy, 1.0f);
        starsSpeed = (int)Mathf.Max(3.0f * totalDaySpeed, 1.0f);

        oldStarsReputation = GameEventManager.Instance.currentStoreReputation;
        GameEventManager.Instance.currentStoreReputation += starsAccuracy + starsSpeed - 4;
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
