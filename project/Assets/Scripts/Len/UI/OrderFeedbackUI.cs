using UnityEngine;
using UnityEngine.UI;

public class OrderFeedbackUI : MonoBehaviour
{
    public Text feedbackText;

    public void ShowOrderFeedback()
    {
        gameObject.SetActive(true);
        GameEventManager.Instance.timeDisplay.ShowPause(true);

        Evaluation evaluation = GameEventManager.Instance.openOrder.evaluation.GetRandomEvaluation();
        string customerReacton = GameEventManager.Instance.openCustomer.GetEvaluationResponse(evaluation);
        feedbackText.text = customerReacton;
    }
}
