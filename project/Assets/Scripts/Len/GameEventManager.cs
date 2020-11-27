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

        // This command is added when final order is submitted. Customer will give overall
        // feedback for entire order.
        // DEATH BY PLAYER INPUT
        POST_ORDER_EVAL,

        // This triggers end of day report.
        // DEATH BY PLAYER INPUT
        END_OF_DAY_EVAL,

        // This is to push the game over screen
        GAME_OVER,

        // This is to push the game win screen
        GAME_WON
    }

    public enum TeaStage
    {
        FILL_KETTLE = 1,
        FILL_TEAPOT = 2,
        ADD_TEA = 3,
        FILL_CUP = 4,
        ADD_CONDIMENT = 5,
        ADD_MILK = 6,
        SERVE = 7
    }

    public System.Random randomGenerator = new System.Random();

    public Queue<GameEvent> eventQueue = new Queue<GameEvent>();
    public Stack<TeaStage> makingProgress = new Stack<TeaStage>();

    public bool showTips;

    public int currentDay;

    public Customer openCustomer;
    public Timer missionTimer;
    public TimeDisplayUI timeDisplay;

    public Order openOrder;

    public int customersEachDay;
    public int completedCustomers;

    public float totalDayAccuracy;
    public float totalDaySpeed;
    public int currentStoreReputation;

    public CustomerViewer customerViewer;
    public Animation cameraAnimator;

    public GameObject pauseScreen;
    public GameObject gameOverScreen;
    public GameObject gameWinScreen;

    public GameEvent currentEvent = GameEvent.BEGIN_DAY;
    public bool eventFired;
    public bool eventComplete;
    public bool gamePaused;
    public bool rememberDocketUI;
    public bool rememberTeapotUI;
    public bool rememberCupUI;
    public bool rememberKettleUI;
    public bool rememberTimeUI;
    public bool rememberIngredientUI;
    public bool rememberTipUI;

    #region UI Elements
    public BeginDayUI beginDayUI;
    public CustomerUI customerUI;
    public BeginOrderUI beginOrderUI;
    public DocketUI docketUI;
    public PostOrderEvaluationUI postOrderEvaluationUI;
    public EndDayEvaluationUI endDayEvaluationUI;
    public KettleUI kettleUI;
    public TeapotUI teapotUI;
    public CupUI cupUI;
    public KettleMenuController kettleTemperatureUI;
    public IngredientUI ingredientUI;
    public TimeDisplayUI timeUI;
    public TipUI tipUI;
    #endregion

    #region Objects
    [Header("")]
    [Header("Game Objects")]
    public GameObject kettleObject;
    public GameObject teapotObject;
    public GameObject cupObject;
    #endregion

    #region Object Interfaces
    [Header("")]
    [Header("Object Interfaces")]
    public KettleInterface kettleInterface;
    public TeapotInterface teapotInterface;
    public CupInterface cupInterface;
    #endregion

    #endregion

    #region Properties

    public float MinTasteAdjusted { get { return Mathf.Min(Mathf.Max(openOrder.targetTaste - (1.0f / currentDay) * openCustomer.ToleranceTaste, -1.0f), 1.0f); } }
    public float MaxTasteAdjusted { get { return Mathf.Min(Mathf.Max(openOrder.targetTaste + (1.0f / currentDay) * openCustomer.ToleranceTaste, -1.0f), 1.0f); } }

    public float MinStrengthAdjusted { get { return Mathf.Min(Mathf.Max(openOrder.targetStrength - (1.0f / currentDay) * openCustomer.ToleranceStrength, -1.0f), 1.0f); } }
    public float MaxStrengthAdjusted { get { return Mathf.Min(Mathf.Max(openOrder.targetStrength + (1.0f / currentDay) * openCustomer.ToleranceStrength, -1.0f), 1.0f); } }

    public float MinTemperatureAdjusted { get { return Mathf.Min(Mathf.Max(openOrder.targetTemperature - (1.0f / currentDay) * openCustomer.ToleranceTemperature, -1.0f), 1.0f); } }
    public float MaxTemperatureAdjusted { get { return Mathf.Min(Mathf.Max(openOrder.targetTemperature + (1.0f / currentDay) * openCustomer.ToleranceTemperature, -1.0f), 1.0f); } }

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

                    openOrder = openCustomer.GenerateOrder();

                    customerUI.ShowGreeting();

                    break;
                }
            #endregion
            #region BEGIN_ORDER_SWIPE
            case GameEvent.BEGIN_ORDER_SWIPE:
                {
                    PushToQueue(GameEvent.MAKE_ORDER);

                    beginOrderUI.TriggerBeginOrderSwipe();

                    break;
                }
            #endregion
            #region MAKE_ORDER
            case GameEvent.MAKE_ORDER:
                {
                    PushToQueue(GameEvent.POST_ORDER_EVAL);

                    docketUI.ShowDocket();
                    kettleUI.gameObject.SetActive(true);
                    teapotUI.gameObject.SetActive(true);
                    cupUI.gameObject.SetActive(true);

                    if (currentDay == 1 && completedCustomers == 1)
                    {
                        PushTeaStage(TeaStage.FILL_KETTLE);
                        tipUI.gameObject.SetActive(true);
                    }
                    else
                    {
                        tipUI.gameObject.SetActive(false);
                    }

                    InputController.Instance.EnableInteraction();
                    missionTimer.ResumeTimer();

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

                    InputController.Instance.DisableInteraction();
                    postOrderEvaluationUI.ShowPostOrderEvaluation();
                    TriggerCameraZoomOut();
                    timeDisplay.gameObject.SetActive(false);
                    docketUI.gameObject.SetActive(false);
                    teapotUI.gameObject.SetActive(false);
                    kettleUI.gameObject.SetActive(false);
                    cupUI.gameObject.SetActive(false);
                    tipUI.gameObject.SetActive(false);
                    missionTimer.PauseTimer();
                    totalDaySpeed += missionTimer.ElapsedTime();
                    openCustomer = null;
                    openOrder = null;
                    ResetContainers();

                    break;
                }
            #endregion
            #region END_OF_DAY_EVAL
            case GameEvent.END_OF_DAY_EVAL:
                {
                    endDayEvaluationUI.ShowEndDayEvaluation();
                    customerViewer.SetCustomer(null);

                    break;
                }
            #endregion
            #region GAME_OVER
            case GameEvent.GAME_OVER:
                {
                    gameOverScreen.SetActive(true);

                    break;
                }
            #endregion
            #region GAME_WON
            case GameEvent.GAME_WON:
                {
                    gameWinScreen.SetActive(true);

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

    public void PushTeaStage(TeaStage stage)
    {
        if (makingProgress.Count == 0 || makingProgress.Peek() == stage - 1)
        {
            makingProgress.Push(stage);
            tipUI.UpdateTip();
        }
    }

    public void ResetContainers()
    {
        kettleObject.GetComponent<KettleInterface>().kettle.ResetKettle();
        teapotObject.GetComponent<TeapotInterface>().teapot.ResetTeapot();
        cupObject.GetComponent<CupInterface>().cup.ResetCup();
    }

    public void EvaluateOrder()
    {
        OrderEvaluation newEvaluation = new OrderEvaluation();

        if (!cupInterface.cup.IsFull)
        {
            newEvaluation.InsertEvaluation(Evaluation.Error.EMPTY_CUP);
        }
        else if (cupInterface.cup.IsFull && cupInterface.cup.additiveRepository.Count == 0)
        {
            newEvaluation.InsertEvaluation(Evaluation.Error.JUST_WATER);
        }
        else
        {
            float tasteMin = MinTasteAdjusted;
            float tasteMax = MaxTasteAdjusted;
            float strengthMin = MinStrengthAdjusted;
            float strengthMax = MaxStrengthAdjusted;
            float temperatureMin = MinTemperatureAdjusted;
            float temperatureMax = MaxTemperatureAdjusted;

            int maxScore = 3 + openOrder.additiveRepository.Count;

            if (cupInterface.cup.Taste < tasteMin)
            {
                newEvaluation.InsertEvaluation(Evaluation.Error.TOO_BITTER);
            }
            else if (cupInterface.cup.Taste > tasteMax)
            {
                newEvaluation.InsertEvaluation(Evaluation.Error.TOO_SWEET);
            }
            else
            {
                newEvaluation.scoreTaste = 1.0f;
            }

            if (cupInterface.cup.Strength < strengthMin)
            {
                newEvaluation.InsertEvaluation(Evaluation.Error.TOO_WEAK);
            }
            else if (cupInterface.cup.Strength > strengthMax)
            {
                newEvaluation.InsertEvaluation(Evaluation.Error.TOO_STRONG);
            }
            else
            {
                newEvaluation.scoreStrength = 1.0f;
            }

            if (cupInterface.cup.Temperature < temperatureMin)
            {
                newEvaluation.InsertEvaluation(Evaluation.Error.TOO_COLD);
            }
            else if (cupInterface.cup.Temperature > temperatureMax)
            {
                newEvaluation.InsertEvaluation(Evaluation.Error.TOO_HOT);
            }
            else
            {
                newEvaluation.scoreTemperature = 1.0f;
            }

            if (openOrder.additiveRepository.Count != 0)
            {
                foreach (Additive additive in openOrder.additiveRepository)
                {
                    if (cupInterface.cup.ContainsAdditive(additive))
                    {
                        newEvaluation.scoreAdditive = 1.0f / openOrder.additiveRepository.Count;
                    }
                }
            }
            else
            {
                newEvaluation.scoreAdditive = 3.0f;
            }
        }

        openOrder.evaluation = newEvaluation;

        SetEventToComplete();
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

        if (gamePaused)
        {
            InputController.Instance.DisableInteraction();
        }
        else
        {
            InputController.Instance.EnableInteraction();
        }

        ToggleUI();
    }

    public void ToggleUI()
    {
        if (gamePaused)
        {
            rememberDocketUI = docketUI.gameObject.activeSelf;
            rememberCupUI = cupUI.gameObject.activeSelf;
            rememberTeapotUI = teapotUI.gameObject.activeSelf;
            rememberKettleUI = kettleUI.gameObject.activeSelf;
            rememberTimeUI = timeUI.gameObject.activeSelf;
            rememberIngredientUI = ingredientUI.gameObject.activeSelf;
            rememberTipUI = tipUI.gameObject.activeSelf;

            docketUI.gameObject.SetActive(false);
            cupUI.gameObject.SetActive(false);
            teapotUI.gameObject.SetActive(false);
            kettleUI.gameObject.SetActive(false);
            timeUI.gameObject.SetActive(false);
            ingredientUI.gameObject.SetActive(false);
            tipUI.gameObject.SetActive(false);
        }
        else
        {
            docketUI.gameObject.SetActive(rememberDocketUI);
            cupUI.gameObject.SetActive(rememberCupUI);
            teapotUI.gameObject.SetActive(rememberTeapotUI);
            kettleUI.gameObject.SetActive(rememberKettleUI);
            timeUI.gameObject.SetActive(rememberTimeUI);
            ingredientUI.gameObject.SetActive(rememberIngredientUI);
            tipUI.gameObject.SetActive(rememberTipUI);
        }
    }

    #endregion
}