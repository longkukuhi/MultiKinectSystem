using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChartController : MonoBehaviour
{
    public LineChart Speed;
    public LineChart SpeedRef;

    public LineChart Accelerate;
    public LineChart AccelerateRef;

    public LineChart Curvature;
    public LineChart CurvatureRef;

    public LineChart Torsion;
    public LineChart TorsionRef;

    public void InitChart()
    {
        Speed.InitLineChart();
        Accelerate.InitLineChart();
        Curvature.InitLineChart();
        Torsion.InitLineChart();
    }

    public void InitRefChart()
    {
        SpeedRef.InitLineChart();
        AccelerateRef.InitLineChart();
        CurvatureRef.InitLineChart();
        TorsionRef.InitLineChart();
    }

    // Update is called once per frame
    public void UpdateLineChart(ChartType chartType, float x, float y, float z = 0f)
    {
        switch (chartType)
        {
            case ChartType.CHART_SPEED:
                {
                    Speed.UpdateCurveData(x, y, z);
                }
                break;
            case ChartType.CHART_ACCELERATE:
                {
                    Accelerate.UpdateCurveData(x, y, z);
                }
                break;
            case ChartType.CHART_CURVATURE:
                {
                    Curvature.UpdateCurveData(x, y, z);
                }
                break;
            case ChartType.CHART_TORSION:
                {
                    Torsion.UpdateCurveData(x, y, z);
                }
                break;
            default:
                break;
        }
    }

    public void UpdateRefLineChart(ChartType chartType, float x, float y, float z=0f)
    {
        switch (chartType)
        {
            case ChartType.CHART_SPEED:
                {
                    SpeedRef.UpdateCurveData(x, y, z);
                }
                break;
            case ChartType.CHART_ACCELERATE:
                {
                    AccelerateRef.UpdateCurveData(x, y, z);
                }
                break;
            case ChartType.CHART_CURVATURE:
                {
                    CurvatureRef.UpdateCurveData(x, y, z);
                }
                break;
            case ChartType.CHART_TORSION:
                {
                    TorsionRef.UpdateCurveData(x, y, z);
                }
                break;
            default:
                break;
        }
    }

    public void HideLineChart(ChartType chartType)
    {
        switch (chartType)
        {
            case ChartType.CHART_SPEED:
                {
                    Speed.HideChart();
                }
                break;
            case ChartType.CHART_ACCELERATE:
                {
                    Accelerate.HideChart();
                }
                break;
            case ChartType.CHART_CURVATURE:
                {
                    Curvature.HideChart();
                }
                break;
            case ChartType.CHART_TORSION:
                {
                    Torsion.HideChart();
                }
                break;
            default:
                break;
        }
    }

    public void HideRefLineChart(ChartType chartType)
    {
        switch (chartType)
        {
            case ChartType.CHART_SPEED:
                {
                    SpeedRef.HideChart();
                }
                break;
            case ChartType.CHART_ACCELERATE:
                {
                    AccelerateRef.HideChart();
                }
                break;
            case ChartType.CHART_CURVATURE:
                {
                    CurvatureRef.HideChart();
                }
                break;
            case ChartType.CHART_TORSION:
                {
                    TorsionRef.HideChart();
                }
                break;
            default:
                break;
        }
    }

    public void ReShowLineChart(ChartType chartType)
    {
        switch (chartType)
        {
            case ChartType.CHART_SPEED:
                {
                    Speed.ReShowChart();
                }
                break;
            case ChartType.CHART_ACCELERATE:
                {
                    Accelerate.ReShowChart();
                }
                break;
            case ChartType.CHART_CURVATURE:
                {
                    Curvature.ReShowChart();
                }
                break;
            case ChartType.CHART_TORSION:
                {
                    Torsion.ReShowChart();
                }
                break;
            default:
                break;
        }
    }

    public void ReShowRefLineChart(ChartType chartType)
    {
        switch (chartType)
        {
            case ChartType.CHART_SPEED:
                {
                    SpeedRef.ReShowChart();
                }
                break;
            case ChartType.CHART_ACCELERATE:
                {
                    AccelerateRef.ReShowChart();
                }
                break;
            case ChartType.CHART_CURVATURE:
                {
                    CurvatureRef.ReShowChart();
                }
                break;
            case ChartType.CHART_TORSION:
                {
                    TorsionRef.ReShowChart();
                }
                break;
            default:
                break;
        }
    }
}
