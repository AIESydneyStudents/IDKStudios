using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OrderFeedbackUI : MonoBehaviour
{
    public Text feedbackText;

    public void ShowOrderFeedback()
    {
        // Tilt camera up.
        // Show UI with customer reaction.

        gameObject.SetActive(true);
        GameEventManager.Instance.timeDisplay.ShowPause(true);

        if (GameEventManager.Instance.lastToBeEvaluated == 1)
        {
            Evaluation evaluation = GameEventManager.Instance.order1.evaluation.GetRandomEvaluation();
            string customerReacton = GameEventManager.Instance.openCustomer.GetEvaluationResponse(evaluation);
            feedbackText.text = customerReacton;
        }
        else if (GameEventManager.Instance.lastToBeEvaluated == 2)
        {
            Evaluation evaluation = GameEventManager.Instance.order2.evaluation.GetRandomEvaluation();
            string customerReacton = GameEventManager.Instance.openCustomer.GetEvaluationResponse(evaluation);
            feedbackText.text = customerReacton;
        }
        else
        {
            feedbackText.text = "";
        }
    }
}
