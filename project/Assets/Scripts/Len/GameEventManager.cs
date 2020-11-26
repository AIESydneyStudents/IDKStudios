using System.Collections.Generic;
using UnityEngine;

public class GameEventManager : Singleton<GameEventManager>
{
    #region Fields

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

    public System.Random randomGenerator = new System.Random();

    public Queue<GameEvent> eventQueue = new Queue<GameEvent>();

    public int currentDay;

    public Customer openCustomer;
    public Timer missionTimer;
    public TimeDisplayUI timeDisplay;

    public Order order1;

    public int customersEachDay;
    public int completedCustomers;

    public float totalDayAccuracy;
    public float totalDaySpeed;
    public int currentStoreReputation; // between 1 and 7.

    public CustomerViewer customerViewer;
    public Animation cameraAnimator;

    public GameObject pauseScreen;

    public GameEvent currentEvent = GameEvent.BEGIN_DAY;
    public bool eventFired;
    public bool eventComplete;
    public bool gamePaused;

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
    public GameObject kettleBaseObject;
    public GameObject teapotObject1;
    public GameObject cupObject1;
    #endregion

    #region Object Interfaces
    [Header("")]
    [Header("Object Interfaces")]
    public KettleInterface kettleInterface;
    public TeapotInterface teapotInterface1;
    public CupInterface cupInterface1;
    #endregion

    #endregion

    #region Properties

    public float MinTasteAdjusted1 { get { return Mathf.Min(Mathf.Max(order1.targetTaste - (1.0f / currentDay) * openCustomer.ToleranceTaste, -1.0f), 1.0f); } }
    public float MaxTasteAdjusted1 { get { return Mathf.Min(Mathf.Max(order1.targetTaste + (1.0f / currentDay) * openCustomer.ToleranceTaste, -1.0f), 1.0f); } }
    
    public float MinStrengthAdjusted1 { get { return Mathf.Min(Mathf.Max(order1.targetStrength - (1.0f / currentDay) * openCustomer.ToleranceStrength, -1.0f), 1.0f); } }
    public float MaxStrengthAdjusted1 { get { return Mathf.Min(Mathf.Max(order1.targetStrength + (1.0f / currentDay) * openCustomer.ToleranceStrength, -1.0f), 1.0f); } }
    
    public float MinTemperatureAdjusted1 { get { return Mathf.Min(Mathf.Max(order1.targetTemperature - (1.0f / currentDay) * openCustomer.ToleranceTemperature, -1.0f), 1.0f); } }
    public float MaxTemperatureAdjusted1 { get { return Mathf.Min(Mathf.Max(order1.targetTemperature + (1.0f / currentDay) * openCustomer.ToleranceTemperature, -1.0f), 1.0f); } }
    
    #endregion

    #region Functions

    private void Update()
    {
        if (gamePaused)
        {
            return;
        }

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
                    completedCustomers = 0;
                    Customer.SetAllCustomersToUnvisited();

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

                    order1 = openCustomer.GenerateOrder();

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

                    InputController.Instance.EnableInteraction();
                    timeDisplay.ShowPause(false);
                    missionTimer.ResumeTimer();
                    docketUI.ShowDocketSubmit();

                    break;
                }
            #endregion
            #region CONTINUE
            case GameEvent.CUSTOMER_FEEDBACK:
                {
                        PushToQueue(GameEvent.POST_ORDER_EVAL);

                    InputController.Instance.DisableInteraction();
                    timeDisplay.ShowPause(true);
                    missionTimer.PauseTimer();
                    docketUI.HideDocketSubmit();

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
                    TriggerCameraZoomOut();
                    timeDisplay.gameObject.SetActive(false);
                    missionTimer.PauseTimer();

                    totalDaySpeed += missionTimer.ElapsedTime();

                    cupObject1.SetActive(false);
                    // Clean up parameters
                    openCustomer = null;
                    order1 = null;
                    ResetContainers();

                    break;
                }
            #endregion
            #region END_OF_DAY_EVAL
            case GameEvent.END_OF_DAY_EVAL:
                {
                    PushToQueue(GameEvent.BEGIN_DAY);

                    // Show end of day evaluation.
                    endDayEvaluationUI.ShowEndDayEvaluation();
                    customerViewer.SetCustomer(null);

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
        EvaluateOrder();
        docketUI.docket1.SetActive(false);
        SetEventToComplete();
    }

    public void ResetContainers()
    {
        kettleObject.GetComponent<KettleInterface>().kettle.ResetKettle();
        teapotObject1.GetComponent<TeapotInterface>().teapot.ResetTeapot();
        cupObject1.GetComponent<CupInterface>().cup.ResetCup();
    }

    public void EvaluateOrder()
    {
        OrderEvaluation newEvaluation = new OrderEvaluation();

        if (!cupInterface1.cup.IsFull)
        {
            newEvaluation.InsertEvaluation(Evaluation.Error.EMPTY_CUP);
        }
        else if (cupInterface1.cup.IsFull && cupInterface1.cup.additiveRepository.Count == 0)
        {
            newEvaluation.InsertEvaluation(Evaluation.Error.JUST_WATER);
        }
        else
        {
            float tasteMin;
            float tasteMax;
            float tasteRange;
            float strengthMin;
            float strengthMax;
            float strengthRange;
            float temperatureMin;
            float temperatureMax;
            float temperatureRange;

                tasteMin = MinTasteAdjusted1;
                tasteMax = MaxTasteAdjusted1;
                strengthMin = MinStrengthAdjusted1;
                strengthMax = MaxStrengthAdjusted1;
                temperatureMin = MinTemperatureAdjusted1;
                temperatureMax = MaxTemperatureAdjusted1;
           

            tasteRange = (tasteMax - tasteMin) * 0.5f;
            strengthRange = (strengthMax - strengthMin) * 0.5f;
            temperatureRange = (temperatureMax - temperatureMin) * 0.5f;

            if (cupInterface1.cup.Taste < tasteMin)
            {
                newEvaluation.InsertEvaluation(Evaluation.Error.TOO_BITTER);
            }
            else if (cupInterface1.cup.Taste > tasteMax)
            {
                newEvaluation.InsertEvaluation(Evaluation.Error.TOO_SWEET);
            }
            else
            {
                newEvaluation.scoreTaste = Mathf.Abs(cupInterface1.cup.Taste - order1.targetTaste) / tasteRange;
            }

            if (cupInterface1.cup.Strength < strengthMin)
            {
                newEvaluation.InsertEvaluation(Evaluation.Error.TOO_WEAK);
            }
            else if (cupInterface1.cup.Strength > strengthMax)
            {
                newEvaluation.InsertEvaluation(Evaluation.Error.TOO_STRONG);
            }
            else
            {
                newEvaluation.scoreStrength = Mathf.Abs(cupInterface1.cup.Strength - order1.targetStrength) / strengthRange;
            }

            if (cupInterface1.cup.Temperature < temperatureMin)
            {
                newEvaluation.InsertEvaluation(Evaluation.Error.TOO_COLD);
            }
            else if (cupInterface1.cup.Temperature > temperatureMax)
            {
                newEvaluation.InsertEvaluation(Evaluation.Error.TOO_HOT);
            }
            else
            {
                newEvaluation.scoreTemperature = Mathf.Abs(cupInterface1.cup.Temperature - order1.targetTemperature) / temperatureRange;
            }

            if (order1.additiveRepository.Count != 0)
            {
                foreach (Additive additive in order1.additiveRepository)
                {
                    if (!cupInterface1.cup.ContainsAdditive(additive))
                    {
                        newEvaluation.InsertEvaluation(Evaluation.Error.NO_ADDITIVE, additive.Name);
                    }
                    else
                    {
                        newEvaluation.scoreAdditive += 1.0f / order1.additiveRepository.Count;
                    }
                }
            }
        }

        order1.evaluation = newEvaluation;
    }

    public void TriggerCameraZoomIn()
    {
        cameraAnimator.Play("CameraZoomIn");
    }

    public void TriggerCameraZoomOut()
    {
        cameraAnimator.Play("CameraZoomOut");
    }

    public void TogglePauseGame()
    {
        gamePaused = !gamePaused;

        pauseScreen.SetActive(gamePaused);

        Time.timeScale = gamePaused ? 0.0f : 1.0f;
    }

    #endregion
}