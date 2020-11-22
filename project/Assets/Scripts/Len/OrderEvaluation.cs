using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class OrderEvaluation
{
    public float scoreTaste;
    public float scoreStrength;
    public float scoreTemperature;
    public float scoreAdditive;

    List<Evaluation> evaluations = new List<Evaluation>();

    public void InsertEvaluation(Evaluation.Error error, string additiveName = null)
    {
        Evaluation newEvaluation = new Evaluation(error, additiveName);
        evaluations.Add(newEvaluation);
    }

    public Evaluation GetRandomEvaluation()
    {
        if (evaluations.Count == 0)
        {
            return null;
        }

        Random randomGenerator = new Random();
        int randomNumber = randomGenerator.Next(0, evaluations.Count - 1);
        return evaluations[randomNumber];
    }
}
