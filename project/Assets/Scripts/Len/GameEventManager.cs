using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

public class GameEventManager : Singleton<GameEventManager>
{
    public int currentDay;

    public Customer openCustomer;
    public Timer openCustomerTimer;
    public List<Order> openCustomerOrders;
    public List<Order> closedCustomerOrders;

    private bool proceedWithCustomerOrder;

    public float totalDayScore;
    public float totalDayTime;

    public int customersEachDay;
    public int completedCustomers;

    public CustomerLoader customerLoader;
    public GameObject customerSpeechUI;

    public GameObject teapotObject1;
    public GameObject teapotObject2;
    public GameObject cupObject1;
    public GameObject cupObject2;
    public GameObject saucerObject1;
    public GameObject saucerObject2;
    public GameObject kettleObject;

    private void Start()
    {
        //StartNewCustomer();
    }

    public void BeginNewDay()
    {
        currentDay++;
        openCustomer = null;
        openCustomerOrders.Clear();
        closedCustomerOrders.Clear();
        totalDayScore = 0.0f;
        totalDayTime = 0.0f;

        //Show day # fade in/out title
        StartNewCustomer();
    }

    public void EndDay()
    {
        //NEEDS CODE
    }

    public void StartNewCustomer()
    {
        // Reset containers.
        kettleObject.GetComponent<KettleInterface>().kettle.ResetKettle();
        teapotObject1.GetComponent<TeapotInterface>().teapot.ResetTeapot();
        teapotObject2.GetComponent<TeapotInterface>().teapot.ResetTeapot();
        cupObject1.GetComponent<CupInterface>().cup.ResetCup();
        cupObject2.GetComponent<CupInterface>().cup.ResetCup();

        openCustomerTimer.StartTimer();
        openCustomer = Customer.GetRandomCustomer();
        customerLoader.SetCustomer(openCustomer);

        // Show greeting dialogue.
        customerSpeechUI.SetActive(true);

        // Wait on button press to recieve order.
        while (!proceedWithCustomerOrder);
        proceedWithCustomerOrder = false;

        customerSpeechUI.SetActive(false);

        System.Random randomGenerator = new System.Random();
        int orderCount = randomGenerator.Next(1, 2);

        for (int i = 0; i < orderCount; i++)
        {
            Order newOrder = openCustomer.GenerateOrder();
            openCustomerOrders.Add(newOrder);
        }
    }

    public void EndCustomer()
    {
        openCustomerTimer.PauseTimer();
        totalDayTime += openCustomerTimer.ElapsedTime();

        foreach (Order order in closedCustomerOrders)
        {
            totalDayScore += order.evaluation.score;
        }
    }

    public void EvaluateOrder(Order order, Cup cup)
    {
        if (!openCustomerOrders.Contains(order))
        {
            return;
        }

        OrderEvaluation newEvaluation = new OrderEvaluation();

        if (openCustomer == null)
        {
            return;
        }

        float accumulatedScore = 0.0f;

        float tasteMin = order.targetTaste - order.toleranceTaste;
        float tasteMax = order.targetTaste + order.toleranceTaste;
        float strengthMin = order.targetStrength - order.toleranceStrength;
        float strengthMax = order.targetStrength + order.toleranceStrength;
        float temperatureMin = order.targetTemperature - order.toleranceTemperature;
        float temperatureMax = order.targetTemperature + order.toleranceTemperature;

        if (cup.Taste < tasteMin)
        {
            newEvaluation.InsertEvaluation(Evaluation.Error.TOO_BITTER);
        }
        else if (cup.Taste > tasteMax)
        {
            newEvaluation.InsertEvaluation(Evaluation.Error.TOO_SWEET);
        }
        else
        {
            accumulatedScore += Mathf.Abs(cup.Taste - order.targetTaste) / order.toleranceTaste;
        }

        if (cup.Strength < strengthMin)
        {
            newEvaluation.InsertEvaluation(Evaluation.Error.TOO_WEAK);
        }
        else if (cup.Strength > strengthMax)
        {
            newEvaluation.InsertEvaluation(Evaluation.Error.TOO_STRONG);
        }
        else
        {
            accumulatedScore += Mathf.Abs(cup.Strength - order.targetStrength) / order.toleranceStrength;
        }

        if (cup.Temperature < temperatureMin)
        {
            newEvaluation.InsertEvaluation(Evaluation.Error.TOO_COLD);
        }
        else if (cup.Temperature > temperatureMax)
        {
            newEvaluation.InsertEvaluation(Evaluation.Error.TOO_HOT);
        }
        else
        {
            accumulatedScore += Mathf.Abs(cup.Temperature - order.targetTemperature) / order.toleranceTemperature;
        }

        foreach (Additive additive in order.additiveRepository)
        {
            if (!cup.ContainsAdditive(additive))
            {
                newEvaluation.InsertEvaluation(Evaluation.Error.NO_ADDITIVE, additive.Name);
            }
            else
            {
                accumulatedScore += 1.0f;
            }
        }

        order.evaluation = newEvaluation;
        openCustomerOrders.Remove(order);
        closedCustomerOrders.Add(order);

        if (openCustomerOrders.Count == 0)
        {
            completedCustomers++;
        }

        if (completedCustomers == customersEachDay)
        {
            EndDay();
        }
    }

    public void ProceedWithOrder()
    {
        proceedWithCustomerOrder = true;
    }
}