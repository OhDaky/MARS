using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class MovingAveragePosition
{
    public DVector3 CalculateAveragePosition()
    {
        return m_Position;
    }

    public double aMin = 2.0;
    public double aMax = 10.0;
    public double cutoff = 0.01;
    public double alpha = 0.25;

    private DVector3 m_Position;
    private bool first = true;

    public double EWMA_Weight(double a, double aMin, double aMax, double cutoff = 0.01)
    {
        if (a <= aMin)
        {
            return 1.0;
        }

        if (a >= aMax)
        {
            return 0.0;
        }

        var lambda = System.Math.Log(1 / cutoff) / (aMax - aMin);

        return System.Math.Exp(-lambda * (a - aMin));
    }

    public void AddEntry(DVector3 position, double accuracy)
    {
        if (first)
        {
            m_Position = position;
            first = false;
        }
        else
        {
            var b = EWMA_Weight(accuracy, aMin, aMax, cutoff);
            var a = alpha * b;

            m_Position = a * position + (1 - a) * m_Position;
        }
    }

    public void Rest()
    {
        first = true;
        m_Position = new DVector3();
    }
}
