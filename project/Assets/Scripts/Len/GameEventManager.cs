using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

public class GameEventManager : Singleton<GameEventManager>
{
    public enum GameEvent
    {
        // This will show current day on screen. This then triggers first customer arrival.
        // DEATH BY TIMER
        BEGIN_DAY,

        // Customer display sets model, sends DISPLAY_GREETING command.
        // DEATH BY TIMER
        CUSTOMER_ARRIVE,

        // Shows dialogue panel, displays greeting, and shows "Take order" button.
        // Take order pushes "BEGIN_ORDER SWIPE"
        // Orders are generated and displayed in UI.
        // DEATH BY PLAYER INPUT
        DISPLAY_GREETING,

        // Fires a begin making order graphic when "Take Order"
        // DEATH BY TIMER        
        BEGIN_ORDER_SWIPE,

        // This is the gameplay state. Dies when tea is submitted.
        // DEATH BY PLAYER INPUT
        MAKE_ORDER,

        // This command is added that will either allow the session to continue, or will
        // trigger post order evaluation. This is when customer displays feedback for 
        // submitted tea.
        // DEATH BY PLAYER INPUT
        CUSTOMER_FEEDBACK,

        // This command is added when final order is submitted. Customer will give overall
        // feedback for entire order.
        // DEATH BY PLAYER INPUT
        POST_ORDER_EVAL,

        // This triggers end of day report.
        // DEATH BY PLAYER INPUT
        END_OF_DAY_EVAL
    }

    public Queue<GameEvent> eventQueue = new Queue<GameEvent>();

    public int currentDay;

    public Customer openCustomer;
    public Timer missionTimer;

    public int orderCount;
    public Order order1;
    public Order order2;

    public float totalDayScore;
    public float totalDayTime;

    public int customersEachDay;
    public int completedCustomers;

    public CustomerViewer customerViewer;

    #region UI Elements
    public BeginDayUI beginDayUI;
    public CustomerUI customerUI;
    public BeginOrderUI beginOrderUI;
    public DocketUI docketUI;
    public OrderFeedbackUI orderFeedbackUI;
    public PostOrderEvaluationUI postOrderEvaluationUI;
    public EndDayEvaluationUI endDayEvaluationUI;
    #endregion

    #region Objects
    [Header("")]
    [Header("Game Objects")]
    public GameObject kettleObject;
    public GameObject teapotObject1;
    public GameObject teapotObject2;
    public GameObject cupObject1;
    public GameObject cupObject2;
    public GameObject saucerObject1;
    public GameObject saucerObject2;
    #endregion

    #region Object Interfaces
    [Header("")]
    [Header("Object Interfaces")]
    public KettleInterface kettleInterface;
    public TeapotInterface teapotInterface1;
    public TeapotInterface teapotInterface2;
    public CupInterface cupInterface1;
    public CupInterface cupInterface2;
    #endregion    

    public GameEvent currentEvent = GameEvent.BEGIN_DAY;
    public bool eventFired;
    public bool eventComplete;

    #region Properties

    public float MinTasteAdjusted1 { get { return order1.targetTaste - (1.0f / currentDay) * openCustomer.ToleranceTaste; } }
    public float MinTasteAdjusted2 { get { return order2.targetTaste - (1.0f / currentDay) * openCustomer.ToleranceTaste; } }
    public float MaxTasteAdjusted1 { get { return order1.targetTaste + (1.0f / currentDay) * openCustomer.ToleranceTaste; } }
    public float MaxTasteAdjusted2 { get { return order2.targetTaste + (1.0f / currentDay) * openCustomer.ToleranceTaste; } }
    
    public float MinStrengthAdjusted1 { get { return order1.targetStrength - (1.0f / currentDay) * openCustomer.ToleranceStrength; } }
    public float MinStrengthAdjusted2 { get { return order2.targetStrength - (1.0f / currentDay) * openCustomer.ToleranceStrength; } }
    public float MaxStrengthAdjusted1 { get { return order1.targetStrength + (1.0f / currentDay) * openCustomer.ToleranceStrength; } }
    public float MaxStrengthAdjusted2 { get { return order2.targetStrength + (1.0f / currentDay) * openCustomer.ToleranceStrength; } }

    public float MinTemperatureAdjusted1 { get { return order1.targetTemperature - (1.0f / currentDay) * openCustomer.ToleranceTemperature; } }
    public float MinTemperatureAdjusted2 { get { return order2.targetTemperature - (1.0f / currentDay) * openCustomer.ToleranceTemperature; } }
    public float MaxTemperatureAdjusted1 { get { return order1.targetTemperature + (1.0f / currentDay) * openCustomer.ToleranceTemperature; } }
    public float MaxTemperatureAdjusted2 { get { return order2.targetTemperature + (1.0f / currentDay) * openCustomer.ToleranceTemperature; } }

    #endregion

    private void Update()
    {
        if (eventComplete)
        {
            PullFromQueue();
            eventComplete = false;
        }

        if (eventFired)
        {
            return;
        }

        switch (currentEvent)
        {
            #region BEGIN_DAY
            case GameEvent.BEGIN_DAY:
                {
                    PushToQueue(GameEvent.CUSTOMER_ARRIVE);

                    currentDay++;

                    //Run day begin graphic
                    beginDayUI.TriggerBeginDaySplash(currentDay);

                    break;
                }
            #endregion
            #region CUSTOMER_ARRIVE
            case GameEvent.CUSTOMER_ARRIVE:
                {
                    PushToQueue(GameEvent.DISPLAY_GREETING);

                    completedCustomers++;
                    openCustomer = Customer.GetRandomCustomer();
                    customerViewer.SetCustomer(openCustomer);

                    break;
                }
            #endregion
            #region DISPLAY_GREETING
            case GameEvent.DISPLAY_GREETING:
                {
                    PushToQueue(GameEvent.BEGIN_ORDER_SWIPE);

                    // Generate order/s
                    System.Random randomGenerator = new System.Random();
                    orderCount = randomGenerator.Next(1, 9) % 2 + 1;

                    order1 = openCustomer.GenerateOrder();
                    order2 = orderCount == 2 ? openCustomer.GenerateOrder() : null;

                    customerUI.ShowGreeting();

                    break;
                }
            #endregion
            #region BEGIN_ORDER_SWIPE
            case GameEvent.BEGIN_ORDER_SWIPE:
                {
                    PushToQueue(GameEvent.MAKE_ORDER);

                    // Activate screen swipe begin order effect
                    beginOrderUI.TriggerBeginOrderSwipe();

                    break;
                }
            #endregion
            #region MAKE_ORDER
            case GameEvent.MAKE_ORDER:
                {
                    PushToQueue(GameEvent.CUSTOMER_FEEDBACK);

                    missionTimer.StartTimer();

                    break;
                }
            #endregion
            #region CONTINUE
            case GameEvent.CUSTOMER_FEEDBACK:
                {
                    if (orderCount == 2 && !order2.IsEvaluated)
                    {
                        PushToQueue(GameEvent.MAKE_ORDER);
                    }
                    else
                    {
                        PushToQueue(GameEvent.POST_ORDER_EVAL);
                    }

                    // Evaluate the current order.
                    orderFeedbackUI.ShowOrderFeedback();

                    break;
                }
            #endregion
            #region POST_ORDER_EVAL
            case GameEvent.POST_ORDER_EVAL:
                {
                    if (completedCustomers == customersEachDay)
                    {
                        PushToQueue(GameEvent.END_OF_DAY_EVAL);
                    }
                    else
                    {
                        PushToQueue(GameEvent.CUSTOMER_ARRIVE);
                    }

                    // Show post order evaluation.
                    postOrderEvaluationUI.ShowPostOrderEvaluation();

                    // Clean up parameters
                    openCustomer = null;
                    order1 = null;
                    order2 = null;
                    orderCount = 0;

                    break;
                }
            #endregion
            #region END_OF_DAY_EVAL
            case GameEvent.END_OF_DAY_EVAL:
                {
                    PushToQueue(GameEvent.BEGIN_DAY);

                    // Show end of day evaluation.
                    endDayEvaluationUI.ShowEndDayEvaluation();

                    break;
                }
            #endregion
            #region DEFAULT
            default:
                {
                    break;
                }
            #endregion
        }

        eventFired = true;
    }

    public void PushToQueue(GameEvent gameEvent)
    {
        eventQueue.Enqueue(gameEvent);
    }

    public void PullFromQueue()
    {
        currentEvent = eventQueue.Dequeue();
    }

    public void SetEventToComplete()
    {
        eventComplete = true;
        eventFired = false;
    }

    public void EvaluateOrder1()
    {
        EvaluateOrder(order1, cupInterface1.cup);
        docketUI.docket1.SetActive(false);
        SetEventToComplete();
    }

    public void EvaluateOrder2()
    {
        EvaluateOrder(order2, cupInterface2.cup);
        docketUI.docket2.SetActive(false);
        SetEventToComplete();
    }

    public void ResetContainers()
    {
        kettleObject.GetComponent<KettleInterface>().kettle.ResetKettle();
        teapotObject1.GetComponent<TeapotInterface>().teapot.ResetTeapot();
        teapotObject2.GetComponent<TeapotInterface>().teapot.ResetTeapot();
        cupObject1.GetComponent<CupInterface>().cup.ResetCup();
        cupObject2.GetComponent<CupInterface>().cup.ResetCup();
    }

    public void EvaluateOrder(Order order, Cup cup)
    {
        OrderEvaluation newEvaluation = new OrderEvaluation();

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
    }
}