using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PostOrderEvaluationUI : MonoBehaviour
{
    public Sprite emptyStar;
    public Sprite fullStar;

    public Text customerResponseText;
    public Button continueButton;

    public Image[] tasteStars;
    public Image[] strengthStars;
    public Image[] temperatureStars;
    public Image[] ingredientsStars;


    public float averageScoreTaste;
    public float averageScoreStrength;
    public float averageScoreTemperature;
    public float averageScoreIngredients;

    int starsTaste;
    int starsStrength;
    int starsTemperature;
    int starsIngredients;

    public float counter;
    public int currentStar;
    public int currentGroup;
    public bool moveToNextGroup;
    public bool currentGroupFinished;
    public bool complete;

    public float subStarDelay;
    public float supStarDelay;

    private void Update()
    {
        if (complete)
        {
            starsTaste = 0;
            starsStrength = 0;
            starsTemperature = 0;
            starsIngredients = 0;
            counter = 0;
            currentStar = 0;
            currentGroup = 0;
            moveToNextGroup = false;
            currentGroupFinished = false;

            return;
        }

        SetStars();
    }

    public void ShowPostOrderEvaluation()
    {
        complete = false;
        continueButton.interactable = false;

        foreach (Image star in tasteStars)
        {
            star.sprite = emptyStar;
        }

        foreach (Image star in strengthStars)
        {
            star.sprite = emptyStar;
        }

        foreach (Image star in temperatureStars)
        {
            star.sprite = emptyStar;
        }

        foreach (Image star in ingredientsStars)
        {
            star.sprite = emptyStar;
        }

        Order order1 = GameEventManager.Instance.order1;
        Order order2 = GameEventManager.Instance.order2;

        averageScoreTaste = order1.evaluation.scoreTaste + (order2 != null ? order2.evaluation.scoreTaste : 0);
        averageScoreStrength = order1.evaluation.scoreStrength + (order2 != null ? order2.evaluation.scoreStrength : 0);
        averageScoreTemperature = order1.evaluation.scoreTemperature + (order2 != null ? order2.evaluation.scoreTemperature : 0);
        averageScoreIngredients = order1.evaluation.scoreAdditive + (order2 != null ? order2.evaluation.scoreAdditive : 0);
        
        if (order2 != null)
        {
            averageScoreTaste *= 0.5f;
            averageScoreStrength *= 0.5f;
            averageScoreTemperature *= 0.5f;
            averageScoreIngredients *= 0.5f;
        }

        starsTaste = Mathf.CeilToInt(3.0f * averageScoreTaste);
        starsStrength = Mathf.CeilToInt(3.0f * averageScoreStrength);
        starsTemperature = Mathf.CeilToInt(3.0f * averageScoreTemperature);
        starsIngredients = Mathf.CeilToInt(3.0f * averageScoreIngredients);

        starsTaste = Mathf.Max(starsTaste, 1);
        starsStrength = Mathf.Max(starsStrength, 1);
        starsTemperature = Mathf.Max(starsTemperature, 1);
        starsIngredients = Mathf.Max(starsIngredients, 1);

        float averageScore = 0.25f * (averageScoreTaste + averageScoreStrength + averageScoreTemperature + averageScoreIngredients);
        GameEventManager.Instance.totalDayAccuracy += averageScore;

        if (averageScore < 0.33f)
        {
            customerResponseText.text = GameEventManager.Instance.openCustomer.GetPoorResponse();
        }
        else if (averageScore < 0.67f)
        {
            customerResponseText.text = GameEventManager.Instance.openCustomer.GetMediocreResponse();
        }
        else
        {
            customerResponseText.text = GameEventManager.Instance.openCustomer.GetGreatResponse();
        }

        gameObject.SetActive(true);
    }

    private void SetStarsTaste()
    {
        counter += Time.deltaTime;

        if (counter > subStarDelay)
        {
            tasteStars[currentStar].sprite = fullStar;
            counter = 0.0f;

            if (++currentStar == starsTaste)
            {
                currentGroupFinished = true;
                currentStar = 0;
            }
        }
    }

    public void SetStarsStrength()
    {
        counter += Time.deltaTime;

        if (counter > subStarDelay)
        {
            strengthStars[currentStar].sprite = fullStar;
            counter = 0.0f;

            if (++currentStar == starsStrength)
            {
                currentGroupFinished = true;
                currentStar = 0;
            }
        }
    }

    public void SetStarsTemperature()
    {
        counter += Time.deltaTime;

        if (counter > subStarDelay)
        {
            temperatureStars[currentStar].sprite = fullStar;
            counter = 0.0f;

            if (++currentStar == starsTemperature)
            {
                currentGroupFinished = true;
                currentStar = 0;
            }
        }
    }

    public void SetStarsIngredients()
    {
        counter += Time.deltaTime;

        if (counter > subStarDelay)
        {
            ingredientsStars[currentStar].sprite = fullStar;
            counter = 0.0f;

            if (++currentStar == starsIngredients)
            {
                currentGroupFinished = true;
                currentStar = 0;
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

                if (currentGroup > 3)
                {
                    continueButton.interactable = true;
                    complete = true;
                    return;
                }
                else
                {
                    currentGroupFinished = false;
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
                        SetStarsTaste();
                        break;
                    }
                case 1:
                    {
                        SetStarsStrength();
                        break;
                    }
                case 2:
                    {
                        SetStarsTemperature();
                        break;
                    }
                case 3:
                    {
                        SetStarsIngredients();
                        break;
                    }
            }
        }
    }
}