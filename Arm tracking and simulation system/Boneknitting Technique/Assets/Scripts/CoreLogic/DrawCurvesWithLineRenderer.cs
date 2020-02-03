using System.Collections.Generic;
using TrajectoryData;
using UnityEngine;

public class DrawCurvesWithLineRenderer : MonoBehaviour
{

    private int start_index = 0;
    private int end_index = 0;
    private int cur_traj = 0;
    private HandMotion motion = new HandMotion();
    private List<LineRenderer> lineRenderer = new List<LineRenderer>();
    private int length = 0;

    private bool button = false;


    // Use this for initialization
    void OnEnable()
    {
        motion = TrailCurveDrawCtrl.Instance().curMotion;
        //isPlay = TrailCurveDrawCtrl.Instance().getIsPlay();
        length = TrailCurveDrawCtrl.Instance().curve_length;

        start_index = 0;
        end_index = (int)motion.getTraj(cur_traj).size();
        for (int i = 0; i < motion.size(); ++i)
        {
            initLineRenderer(i, Color.red, Color.red, 0.04f, 0.04f);
        }
        /*残影部分gameobject初始化
        root1 = GameObject.Find("GameObject1");
        root2 = GameObject.Find("GameObject2");
        hand1 = root1.transform.Find("lefthand").gameObject;
         **/
    }
    void Start()
    {

    }

    void initLineRenderer(int index, Color sc, Color ec, float sw = 0.5F, float ew = 0.5F)
    {
        string name = string.Format("LineRenderer{0}", index+1);
        LineRenderer temp_lr = GameObject.FindGameObjectWithTag(name).GetComponent<LineRenderer>();
        temp_lr.material = new Material(Shader.Find("Particles/Additive"));
        temp_lr.startColor = sc;
        temp_lr.endColor = ec;
        temp_lr.positionCount = (int)motion.getTraj(cur_traj).size();
        temp_lr.startWidth = sw;
        temp_lr.endWidth = ew;

        lineRenderer.Add(temp_lr);
    }
    // Update is called once per frame
    void Update()
    {
        length = TrailCurveDrawCtrl.Instance().curve_length;
        end_index = (int)motion.getTraj(cur_traj).size();
        if (end_index - start_index >= length)
        {
            start_index = end_index - length;
        }
        drawCurve();
    }

    void drawCurve()
    {
        for (int traj = 0; traj < motion.size(); ++traj)
        {
            if (motion.getTraj(traj).getActive() == true)
            {
                lineRenderer[traj].positionCount = end_index - start_index;
                Vec3 temp = new Vec3();
                for (int pos = start_index; pos < end_index; ++pos)
                {
                    temp = motion.getTraj(traj).vec[pos].position;
                    lineRenderer[traj].SetPosition(pos - start_index, new Vector3(temp.x, temp.y, temp.z));
                }
            }
            else
            {
                lineRenderer[traj].positionCount = 0;
            }
        }

        //drawGhost();
    }

    void OnDisable()
    {
        for(int i = 0; i< 2; ++i)
        {
            lineRenderer[i].positionCount = 0;
        }

     }
    /*
    void drawGhost()
    {
        if (button)
        {
            Vector3 rotation = transform.localEulerAngles;
            hand1.transform.position = new Vector3(motion.getTraj(0).vec[end_index - 1].position.x, motion.getTraj(0).vec[end_index - 1].position.y, motion.getTraj(0).vec[end_index - 1].position.z);
            rotation = new Vector3(motion.getTraj(0).vec[end_index - 1].azimuth, motion.getTraj(0).vec[end_index - 1].elevation, motion.getTraj(0).vec[end_index - 1].roll);
            hand1.transform.localEulerAngles = rotation;

            var canying = GameObject.Instantiate(hand1, root1.transform);
        }
    }
    */
}
