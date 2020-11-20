using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomerSpeechUIController : MonoBehaviour
{
    public Text customerSpeechField;
    public float timer;
    public bool coolDown;

    private void Update()
    {
        if (coolDown)
        {
            timer += Time.deltaTime;

            if (timer > 2.0f)
            {
                timer = 0.0f;
                gameObject.SetActive(false);
            }
        }
    }

    public void PushGreeting(Customer customer)
    {
        customerSpeechField.text = "Hi!";

        // NEED WORK
    }

    public void PushReactionDialogue(Customer customer, Evaluation evaluation)
    {
        gameObject.SetActive(true);
        customerSpeechField.text = evaluation.error.ToString();
        coolDown = true;

        //switch (evaluation.error)
        //{
        //    case Evaluation.Error.TOO_BITTER:
        //        {
        //            customerSpeechField.text = evaluation.error.ToString();
        //            break;
        //        }
        //    case Evaluation.Error.TOO_SWEET:
        //        {
        //            customerSpeechField.text = "";
        //            break;
        //        }
        //    case Evaluation.Error.TOO_WEAK:
        //        {
        //            customerSpeechField.text = "";
        //            break;
        //        }
        //    case Evaluation.Error.TOO_STRONG:
        //        {
        //            customerSpeechField.text = "";
        //            break;
        //        }
        //    case Evaluation.Error.TOO_COLD:
        //        {
        //            customerSpeechField.text = "";
        //            break;
        //        }
        //    case Evaluation.Error.TOO_HOT:
        //        {
        //            customerSpeechField.text = "";
        //            break;
        //        }
        //    case Evaluation.Error.EMPTY_CUP:
        //        {
        //            customerSpeechField.text = "";
        //            break;
        //        }
        //    case Evaluation.Error.JUST_WATER:
        //        {
        //            customerSpeechField.text = "";
        //            break;
        //        }
        //    case Evaluation.Error.NO_ADDITIVE:
        //        {
        //            customerSpeechField.text = "";
        //            break;
        //        }
        //    default:
        //        {
        //            customerSpeechField.text = "";
        //            break;
        //        }
        //}
    }
    
    public void PushFinalComment()
    {
        customerSpeechField.text = "Thanks!";
        coolDown = true;
    }
}
