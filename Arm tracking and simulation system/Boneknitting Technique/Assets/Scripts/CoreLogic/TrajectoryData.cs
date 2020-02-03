using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace TrajectoryData
{
    using Point = Vec3;
    public struct Vec3
    {
        public float x;
        public float y;
        public float z;
        public Vec3(Vec3 p) { x = p.x; y = p.y; z = p.z; }
        public Vec3(Vector3 p) { x = p.x; y = p.y; z = p.z; }
        public Vec3(float a, float b, float c) { x = a; y = b; z = c; }

        public static Vec3 operator +(Vec3 a, Vec3 b)
        {
            Vec3 c;
            c.x = a.x + b.x;
            c.y = a.y + b.y;
            c.z = a.z + b.z;

            return c;
        }

        public static Vec3 operator -(Vec3 a, Vec3 b)
        {
            Vec3 c;
            c.x = a.x - b.x;
            c.y = a.y - b.y;
            c.z = a.z - b.z;

            return c;
        }
        public static Vec3 operator *(Vec3 a, Vec3 b)
        {
            Vec3 c;
            c.x = a.x * b.x;
            c.y = a.y * b.y;
            c.z = a.z * b.z;

            return c;
        }
        public static Vec3 operator *(Vec3 a, float b)
        {
            Vec3 c;
            c.x = a.x * b;
            c.y = a.y * b;
            c.z = a.z * b;

            return c;
        }
        public static Vec3 operator /(Vec3 a, float b)
        {

            if (b <= 0.000001 && b >= -0.000001)
            {
                return a;
            }

            Vec3 c;
            c.x = a.x / b;
            c.y = a.y / b;
            c.z = a.z / b;

            return c;
        }
        public static Vec3 operator -(Vec3 a)
        {
            return new Vec3(-a.x, -a.y, -a.z);
        }

        public double dot(Vec3 b)
        {
            return x * b.x + y * b.y + z * b.z;
        }

        public Vec3 cross(Vec3 b)
        {
            Vec3 c;

            c.x = y * b.z - z * b.y;
            c.y = z * b.x - x * b.z;
            c.z = x * b.y - y * b.x;

            return c;
        }
        public double norm()
        {
            return Math.Sqrt(x * x + y * y + z * z);
        }
        public Vec3 normalize()
        {
            float n = (float)norm();
            return new Vec3(x / n, y / n, z / n);
        }
        double distanceTo(Vec3 b)
        {
            double d = Math.Sqrt((x - b.x) * (x - b.x)
            + (y - b.y) * (y - b.y) + (z - b.z) * (z - b.z));

            return d;
        }

    };

    public struct TPose
    {
        public Point position;
        public float azimuth;
        public float elevation;
        public float roll;
        public string time;

        public TPose(Point p)
        {
            position = p;
            azimuth = 0;
            elevation = 0;
            roll = 0;
            time = "";
        }

        public TPose(TPose p)
        {
            position = p.position;
            azimuth = p.azimuth;
            elevation = p.elevation;
            roll = p.roll;
            time = p.time;
        }

        public static TPose operator +(TPose a, Vec3 b)
        {

            TPose p = new TPose(a);
            p.position = a.position + b;
            return p;
        }
        public static TPose operator -(TPose a, Vec3 b)
        {
            TPose p = new TPose(a);
            p.position = a.position - b;
            return p;
        }
        public static TPose operator -(TPose a, TPose b)
        {
            TPose p = new TPose(a);
            p.position = a.position - b.position;
            return p;
        }
        public static TPose operator -(TPose a)
        {
            TPose p = new TPose(a);
            p.position = -a.position;
            return p;
        }
        public static TPose operator *(TPose a, Vec3 b)
        {
            TPose p = new TPose(a);
            p.position = a.position * b;
            return p;
        }
        public static TPose operator *(TPose a, float b)
        {
            TPose p = new TPose(a);
            p.position = a.position * b;
            return p;
        }
        public static TPose operator /(TPose a, float b)
        {
            TPose p = new TPose(a);
            p.position = a.position / b;
            return p;
        }
        public double norm()
        {
            return position.norm();
        }
    };

    public class Trajectory
    {
        public Trajectory()
        {
            //vec = new ArrayList();
        }
        public Trajectory(Trajectory traj)
        {
            vec = traj.vec;
        }

        public int size()
        {
            return vec.Count;
        }

        public Trajectory append(Trajectory traj)
        {
            foreach (TPose p in traj.vec)
            {
                vec.Add(p);
            }

            return this;
        }

        public bool getActive()
        {
            return IsActive;
        }
        public void setActive(bool IsOn)
        {
            IsActive = IsOn;
        }

        public void add(TPose p)
        {
            vec.Add(p);
        }
        public void push_back(TPose p)
        {
            vec.Add(p);
        }
        public void remove(TPose p)
        {
            vec.Remove(p);
        }
        public void removeAt(int i)
        {
            vec.RemoveAt(i);
        }
        public bool insert(int i, TPose p)
        {
            if (i > vec.Count || i < 0)
            {
                return false;
            }

            vec.Insert(i, p);
            return true;
        }

        public void clear()
        {
            vec.Clear();
        }

        public static Trajectory operator +(Trajectory t, Vec3 a)
        {
            Trajectory traj = new Trajectory();
            foreach (TPose p in t.vec)
            {
                traj.push_back(p + a);
            }

            return traj;
        }
        public static Trajectory operator -(Trajectory t, Vec3 a)
        {
            Trajectory traj = new Trajectory();
            foreach (TPose p in t.vec)
            {
                traj.push_back(p - a);
            }

            return traj;
        }
        public static Trajectory operator *(Trajectory t, Vec3 a)
        {
            Trajectory traj = new Trajectory();
            foreach (TPose p in t.vec)
            {
                traj.push_back(p * a);
            }

            return traj;
        }
        public List<float> speed()
        {
            List<float> v = new List<float>();
            if (vec.Count < 2)
            {
                return v;
            }
            else
            {
                int len = vec.Count;
                v.Add((float)(vec[1].position - vec[0].position).norm());
                for (int i = 1; i < len - 1; ++i)
                {
                    v.Add((float)((vec[i + 1].position - vec[i - 1].position).norm() / 2.0));
                }
                v.Add((float)(vec[len - 1].position - vec[len - 2].position).norm());

                return v;
            }
        }
        public float lastSpeed(int deta = 0)
        {
            float v = 0.0f;
            if (vec.Count < 2)
            {
                return v;
            }
            else
            {
                int len = vec.Count - deta;
                if (len - 2 < 0)
                    return v;
                v = (float)(vec[len - 1].position - vec[len - 2].position).norm();

                return v;
            }
        }
        public List<float> acceleration()
        {
            List<float> s = speed();
            List<float> v = new List<float>();

            if (s.Count < 2)
            {
                return v;
            }
            else
            {
                int len = s.Count;
                v.Add((s[1] - s[0]));
                for (int i = 1; i < len - 1; ++i)
                {
                    v.Add((float)((s[i + 1] - s[i - 1]) / 2.0));
                }
                v.Add((s[len - 1] - s[len - 2]));

                return v;
            }
        }
        public float lastAcceleration()
        {
            float v = 0.0f;

            if (vec.Count < 2)
            {
                return v;
            }
            else
            {
                v = (lastSpeed() - lastSpeed(1));

                return v;
            }
        }
        public List<Point> diff(List<TPose> vec)
        {
            List<Point> v = new List<Point>();
            if (vec.Count < 2)
            {
                return v;
            }
            else
            {
                int len = vec.Count;
                v.Add(vec[1].position - vec[0].position);
                for (int i = 1; i < len - 1; ++i)
                {
                    v.Add((vec[i + 1].position - vec[i - 1].position) / (float)2);
                }
                v.Add(vec[len - 1].position - vec[len - 2].position);

                return v;
            }
        }
        public List<Point> diff(List<Point> vec)
        {
            List<Point> v = new List<Point>();
            if (vec.Count < 2)
            {
                return v;
            }
            else
            {
                int len = vec.Count;
                v.Add(vec[1] - vec[0]);
                for (int i = 1; i < len - 1; ++i)
                {
                    v.Add((vec[i + 1] - vec[i - 1]) / (float)2);
                }
                v.Add(vec[len - 1] - vec[len - 2]);

                return v;
            }
        }
        public List<float> curvature()
        {
            List<float> v = new List<float>();
            int len = vec.Count;
            List<Point> d1 = diff(vec);
            List<Point> d2 = diff(d1);

            for (int i = 0; i < len; ++i)
            {
                Point c = d1[i].cross(d2[i]);
                double n1 = c.norm();
                double n2 = Math.Pow(d1[i].norm(), 3);
                v.Add((float)(n1 / n2));
            }

            return v;
        }
        public float lastCurvature()
        {
            float v = 0.0f;
            int len = vec.Count;
            if (len < 2)
                return v;

            List<TPose> nv = new List<TPose>();
            nv.Add(vec[len - 1]);
            nv.Add(vec[len - 2]);

            List<Point> d1 = diff(nv);
            List<Point> d2 = diff(d1);
            Point c = d1[1].cross(d2[1]);
            double n1 = c.norm();
            double n2 = Math.Pow(d1[1].norm(), 3);
            v = (float)(n1 / n2);

            return v;
        }
        public List<float> torsion()
        {
            List<float> v = new List<float>();
            int len = vec.Count;
            List<Point> d1 = diff(vec);
            List<Point> d2 = diff(d1);
            List<Point> d3 = diff(d2);

            for (int i = 0; i < len; ++i)
            {
                Vec3 c = d1[i].cross(d2[i]);
                double n1 = c.dot(d3[i]);
                double n2 = Math.Pow(c.norm(), 2);
                v.Add((float)(n1 / n2));
            }

            return v;
        }
        public float lastTorsion()
        {
            float v = 0.0f;
            int len = vec.Count;
            if (len < 2)
                return v;

            List<TPose> nv = new List<TPose>();
            nv.Add(vec[len - 1]);
            nv.Add(vec[len - 2]);

            List<Point> d1 = diff(nv);
            List<Point> d2 = diff(d1);
            List<Point> d3 = diff(d2);

            Vec3 c = d1[1].cross(d2[1]);
            double n1 = c.dot(d3[1]);
            double n2 = Math.Pow(c.norm(), 2);

            v = (float)(n1 / n2);

            return v;
        }
        public void clip(int start, int end)
        {
            if (vec.Count <= 0 || end <= start) return;
            if (start <= 0) start = 0;
            if (end <= 0) return;
            if (end >= vec.Count) end = vec.Count - 1;

            List<TPose> temp = new List<TPose>();

            for (int i = start; i < end + 1; ++i)
            {
                temp.Add(vec[i]);
            }

            vec = temp;
        }
        public void clip(float start, float end)
        {
            if (vec.Count <= 0 || end <= start) return;
            if (start <= 0.0F) start = 0.0F;
            if (end <= 0.0F) return;
            if (end >= 1.0F) end = 1.0F;

            List<TPose> temp = new List<TPose>();

            int start_index = (int)(start * vec.Count);
            int end_index = (int)(end * vec.Count);

            for (int i = start_index; i < end_index + 1; ++i)
            {
                temp.Add(vec[i]);
            }

            vec = temp;
        }


        public List<TPose> vec = new List<TPose>();

        private bool IsActive = true;
    };

    public class HandMotion
    {
        public HandMotion()
        {

        }

        public HandMotion(HandMotion motion)
        {
            if (motion == this)
            {
                return;
            }

            this.m_vecTrajs.Clear();
            for (int i = 0; i < motion.m_vecTrajs.Count; ++i)
            {
                this.m_vecTrajs.Add(motion.m_vecTrajs[i]);
            }
        }
        public Trajectory getTraj(int i)
        {
            if (i >= m_vecTrajs.Count) i = m_vecTrajs.Count - 1;
            if (i < 0) i = 0;

            return m_vecTrajs[i];
        }
        public int size()
        {
            return m_vecTrajs.Count;
        }
        public void push_back(Trajectory traj)
        {
            this.m_vecTrajs.Add(traj);
        }
        public void Add(Trajectory traj)
        {
            this.m_vecTrajs.Add(traj);
        }
        public bool remove(int i)
        {
            if (i >= m_vecTrajs.Count || i < 0)
                return false;

            m_vecTrajs.RemoveAt(i);
            return true;
        }
        public bool insert(int i, Trajectory traj)
        {
            if (i >= m_vecTrajs.Count || i < 0)
                return false;

            m_vecTrajs.Insert(i, traj);
            return true;
        }

        public void clear()
        {
            m_vecTrajs.Clear();
        }
        public void clearTrailData()
        {
            for (int i = 0; i < m_vecTrajs.Count; ++i)
            {
                m_vecTrajs[i].clear();
            }
        }
        public int maxLength()
        {
            int len = 0;
            for (int i = 0; i < m_vecTrajs.Count; ++i)
            {
                int size = m_vecTrajs.Count;
                if (len < size)
                    len = size;
            }

            return len;
        }
        public static Dictionary<float, float> matching8Max(Trajectory traj1, Trajectory traj2)
        {
            Dictionary<float, float> temp_result = new Dictionary<float, float>();
            TPose temp = new TPose(traj1.vec[0] - traj2.vec[0]);
            float delta = (float)temp.norm();

            float a = 0.0F, b = 0.0F;
            int upORdown = 0;

            int match_length = (int)(traj1.size() > traj2.size() ? traj1.size() : traj2.size());

            for (int i = 1; i < match_length - 1; ++i)
            {
                temp = new TPose(traj1.vec[i] - traj2.vec[i]);
                b = (float)temp.norm() - delta;
                if (b > a && upORdown >= 0)
                {
                    a = b;
                    upORdown = 1;
                }
                else if (b < a && upORdown <= 0)
                {
                    a = b;
                    upORdown = -1;
                }
                else if (b > a && upORdown < 0)
                {
                    temp_result.Add(((float)(i - 1) / match_length), Math.Abs(a));
                    a = b;
                    upORdown = 1;
                }
                else if (b < a && upORdown > 0)
                {
                    temp_result.Add(((float)(i - 1) / match_length), Math.Abs(a));
                    a = b;
                    upORdown = -1;
                }
            }
            temp_result = (from entry in temp_result
                           orderby entry.Value descending
                           select entry).ToDictionary(pair => pair.Key, pair => pair.Value);

            Dictionary<float, float> result = new Dictionary<float, float>();

            int num = 0;
            foreach (KeyValuePair<float, float> dic in temp_result)
            {
                result.Add(dic.Key, dic.Value);
                num++;
                if (num >= 6)
                    break;
            }

            result.Add(0.0F, 0.0F);
            temp = new TPose(traj1.vec[match_length - 1] - traj2.vec[match_length - 1]);
            result.Add(1.0F, Math.Abs((float)(temp.norm() - delta)));

            return result;
        }

        public static List<float> matchingAll(Trajectory traj1, Trajectory traj2)
        {
            List<float> result = new List<float>();
            result.Add(0.0F);
            TPose temp = new TPose(traj1.vec[0] - traj2.vec[0]);
            float delta = (float)temp.norm();

            float a = 0.0F, b = 0.0F;

            int match_length = (int)(traj1.size() > traj2.size() ? traj1.size() : traj2.size());

            for (int i = 1; i < match_length; ++i)
            {
                temp = new TPose(traj1.vec[i] - traj2.vec[i]);
                b = (float)temp.norm() - delta;
                result.Add(Math.Abs(a));
                a = b;
            }

            return result;
        }

        private List<Trajectory> m_vecTrajs = new List<Trajectory>();
        private string m_path;
    }

}
