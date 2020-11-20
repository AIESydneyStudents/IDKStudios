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
        CONTINUE,

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
    public Timer openCustomerTimer;
    public List<Order> openCustomerOrders = new List<Order>();
    public List<Order> closedCustomerOrders = new List<Order>();

    public float totalDayScore;
    public float totalDayTime;

    public int customersEachDay;
    public int completedCustomers;

    public CustomerLoader customerLoader;
    public CustomerSpeechUIController customerSpeechUI;
    public DocketUIController docket1;
    public DocketUIController docket2;

    public GameObject teapotObject1;
    public GameObject teapotObject2;
    public GameObject cupObject1;
    public GameObject cupObject2;
    public GameObject saucerObject1;
    public GameObject saucerObject2;
    public GameObject kettleObject;

    public SaucerMenuController saucerMenu1;
    public SaucerMenuController saucerMenu2;

    public GameEvent currentEvent = GameEvent.BEGIN_DAY;
    public bool eventFired;
    public bool eventComplete;
    public bool eventTimer;

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

                    break;
                }
            #endregion
            #region CUSTOMER_ARRIVE
            case GameEvent.CUSTOMER_ARRIVE:
                {
                    PushToQueue(GameEvent.DISPLAY_GREETING);

                    openCustomer = Customer.GetRandomCustomer();
                    customerLoader.SetCustomer(openCustomer);

                    break;
                }
            #endregion
            #region DISPLAY_GREETING
            case GameEvent.DISPLAY_GREETING:
                {
                    PushToQueue(GameEvent.BEGIN_ORDER_SWIPE);

                    // Generate order/s
                    System.Random randomGenerator = new System.Random();
                    int orderCount = randomGenerator.Next(1, 9) % 2 + 1;

                    for (int i = 0; i < orderCount; i++)
                    {
                        Order newOrder = openCustomer.GenerateOrder();
                        openCustomerOrders.Add(newOrder);
                    }

                    // Show dockets.
                    saucerMenu1.SetOrder(openCustomerOrders[0]);
                    saucerObject1.SetActive(true);
                    cupObject1.SetActive(true);
                    docket1.SetDocket(openCustomerOrders[0]);

                    if (orderCount == 2)
                    {
                        saucerMenu2.SetOrder(openCustomerOrders[1]);
                        saucerObject2.SetActive(true);
                        cupObject2.SetActive(true);
                        docket2.SetDocket(openCustomerOrders[1]);
                    }

                    customerSpeechUI.PushGreeting(openCustomer);

                    break;
                }
            #endregion
            #region BEGIN_ORDER_SWIPE
            case GameEvent.BEGIN_ORDER_SWIPE:
                {
                    PushToQueue(GameEvent.MAKE_ORDER);

                    // Activate screen swipe begin order effect

                    break;
                }
            #endregion
            #region MAKE_ORDER
            case GameEvent.MAKE_ORDER:
                {
                    PushToQueue(GameEvent.CONTINUE);

                    openCustomerTimer.StartTimer();

                    break;
                }
            #endregion
            #region CONTINUE
            case GameEvent.CONTINUE:
                {
                    if (openCustomerOrders.Count == 2)
                    {
                        PushToQueue(GameEvent.MAKE_ORDER);
                    }
                    else
                    {
                        PushToQueue(GameEvent.POST_ORDER_EVAL);
                    }

                    // Evaluate the current order.

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

                    break;
                }
            #endregion
            #region END_OF_DAY_EVAL
            case GameEvent.END_OF_DAY_EVAL:
                {
                    PushToQueue(GameEvent.BEGIN_DAY);

                    // Show end of day evaluation.

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

























    public void BeginNewDay()
    {
        //InputController.Instance.DisableInteraction();
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
        customerSpeechUI.PushGreeting(openCustomer);

        openCustomerOrders.Clear();
        closedCustomerOrders.Clear();
    }

    public void ProceedWithOrder()
    {
        customerSpeechUI.gameObject.SetActive(false);
        
        System.Random randomGenerator = new System.Random();
        int orderCount = 2;// randomGenerator.Next(1, 100) % 2 + 1;

        for (int i = 0; i < orderCount; i++)
        {
            Order newOrder = openCustomer.GenerateOrder();
            openCustomerOrders.Add(newOrder);
        }

        // Show dockets somewhere.
        saucerMenu1.SetOrder(openCustomerOrders[0]);
        saucerObject1.SetActive(true);
        cupObject1.SetActive(true);
        docket1.SetDocket(openCustomerOrders[0]);

        if (orderCount == 2)
        {
            saucerMenu2.SetOrder(openCustomerOrders[1]);
            saucerObject2.SetActive(true);
            cupObject2.SetActive(true);
            docket2.SetDocket(openCustomerOrders[1]);
        }

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
        customerSpeechUI.PushReactionDialogue(openCustomer, newEvaluation.GetRandomEvaluation());

        if (openCustomerOrders.Count == 0)
        {
            completedCustomers++;

            // Show final reaction
            customerSpeechUI.PushFinalComment();

            if (completedCustomers == customersEachDay)
            {
                EndDay();
            }
            else
            {
                StartNewCustomer();
            }
        }
    }
}