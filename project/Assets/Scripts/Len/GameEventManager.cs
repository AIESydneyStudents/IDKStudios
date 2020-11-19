using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

public class GameEventManager : Singleton<GameEventManager>
{
    public int currentDay;

    public Customer openCustomer;
    private Timer openCustomerTimer = new Timer();
    public List<Order> openCustomerOrders = new List<Order>();
    public List<Order> closedCustomerOrders = new List<Order>();

    private bool proceedWithCustomerOrder;

    public float totalDayScore;
    public float totalDayTime;

    public int customersEachDay;
    public int completedCustomers;

    public CustomerLoader customerLoader;
    public CustomerSpeechUIController customerSpeechUI;

    public GameObject teapotObject1;
    public GameObject teapotObject2;
    public GameObject cupObject1;
    public GameObject cupObject2;
    public GameObject saucerObject1;
    public GameObject saucerObject2;
    public GameObject kettleObject;

    public SaucerMenuController saucerMenu1;
    public SaucerMenuController saucerMenu2;

    private void Start()
    {
        StartNewCustomer();
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

        
        openCustomer = Customer.GetRandomCustomer();
        customerLoader.SetCustomer(openCustomer);

        customerSpeechUI.gameObject.SetActive(true);

        // Push customer greeting
        //customerSpeechUI.Pus
    }

    public void ProceedWithOrder()
    {
        customerSpeechUI.gameObject.SetActive(false);
        
        System.Random randomGenerator = new System.Random();
        int orderCount = randomGenerator.Next(1, 100) % 2 + 1;

        for (int i = 0; i < orderCount; i++)
        {
            Order newOrder = openCustomer.GenerateOrder();
            openCustomerOrders.Add(newOrder);
        }

        saucerMenu1.SetOrder(openCustomerOrders[0]);
        saucerObject1.SetActive(true);
        cupObject1.SetActive(true);

        if (orderCount == 2)
        {
            saucerMenu2.SetOrder(openCustomerOrders[1]);
            saucerObject2.SetActive(true);
            cupObject2.SetActive(true);
        }

        // Show dockets somewhere.

        openCustomerTimer.StartTimer();

        // Show timer at top of screen
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

        // Show customer reaction

        if (openCustomerOrders.Count == 0)
        {
            completedCustomers++;
        }

        if (completedCustomers == customersEachDay)
        {
            EndDay();
        }
    }

    
}