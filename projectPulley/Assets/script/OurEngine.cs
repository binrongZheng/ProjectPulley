using System.Collections;
using System.Collections.Generic;
//using UnityEngine;

namespace ourEngine {

    public class ourVector3 {
        public float x, y, z;

        public ourVector3() {
            x = y = z = 0;
        }
        public ourVector3(float _x, float _y, float _z) {
            x = _x;
            y = _y;
            z = _z;
        }

        public float GetMagnitude() {
            return UnityEngine.Mathf.Sqrt(x * x + y * y + z * z);
        }
        public static ourVector3 operator *(ourVector3 v1, float scalar) {
            ourVector3 res = new ourVector3();
            res.x = v1.x * scalar;
            res.y = v1.y * scalar;
            res.z = v1.z * scalar;
            return res;
        }
        public static ourVector3 operator *(float scalar, ourVector3 v1) {
            ourVector3 res = new ourVector3();
            res.x = v1.x * scalar;
            res.y = v1.y * scalar;
            res.z = v1.z * scalar;
            return res;
        }
        public static ourVector3 operator +(ourVector3 v1, ourVector3 v2) {
            ourVector3 res = new ourVector3();
            res.x = v1.x + v2.x;
            res.y = v1.y + v2.y;
            res.z = v1.z + v2.z;
            return res;
        }
        public static ourVector3 operator -(ourVector3 v1, ourVector3 v2) {
            ourVector3 res = new ourVector3();
            res.x = v1.x - v2.x;
            res.y = v1.y - v2.y;
            res.z = v1.z - v2.z;
            return res;
        }
        public static ourVector3 operator / (ourVector3 v1, float s)
        {
            ourVector3 res = new ourVector3();
            res.x = v1.x / s;
            res.y = v1.y / s;
            res.z = v1.z / s;
            return res;
        }

        public static float Dot(ourVector3 v1, ourVector3 v2) {
            float res = v1.x * v2.x + v1.y * v2.y + v1.z * v2.z;
            return res;
        }
        public static ourVector3 Cross(ourVector3 v1, ourVector3 v2) {
            ourVector3 res = new ourVector3();
            res.x = v1.y * v2.z - v1.z * v2.y;
            res.y = v1.z * v2.x - v1.x * v2.z;
            res.z = v1.x * v2.y - v1.y * v2.x;
            return res;
        }
        public static float Distance(ourVector3 v1, ourVector3 v2) {
            ourVector3 res = v1 - v2;
            return res.GetMagnitude();
        }

        public ourVector3 GetNormalized() {
            float magnitude = GetMagnitude();
            ourVector3 res = new ourVector3();

            if (magnitude < 0.01) //si es magnitud molt petita posar arrodonim a 0, ho fan aixi en el Vector3
                res.x = res.y = res.z = 0;
            else {
                res.x = x / magnitude;
                res.y = y / magnitude;
                res.z = z / magnitude;
            }
            return res;
        }
        public static implicit operator ourVector3(UnityEngine.Vector3 v)
        {
            ourVector3 temp = new ourVector3(v.x, v.y, v.z);
            return temp;
        }
        public static implicit operator UnityEngine.Vector3(ourVector3 v)
        {
            UnityEngine.Vector3 temp = new UnityEngine.Vector3(v.x, v.y, v.z);
            return temp;
        }

    }

    public class ourQuaternion {
        //variables
        public float x;
        public float y;
        public float z;
        public float w;

        public ourQuaternion() {
            x = 0;
            y = 0;
            z = 0;
            w = 1;
        }
        public ourQuaternion(float _x, float _y, float _z, float _w) {
            x = _x;
            y = _y;
            z = _z;
            w = _w;
        }
        public ourQuaternion(float angle, ourVector3 axis) {
            x = axis.x * UnityEngine.Mathf.Sin(angle / 2);
            y = axis.y * UnityEngine.Mathf.Sin(angle / 2);
            z = axis.z * UnityEngine.Mathf.Sin(angle / 2);
            w = UnityEngine.Mathf.Cos(angle / 2);
        }
        public static ourQuaternion operator *(ourQuaternion q1, ourQuaternion q2) {
            q1.Normalize();
            q2.Normalize();

            ourQuaternion newQ = new ourQuaternion();
            newQ.w = (q2.w * q1.w - q2.x * q1.x - q2.y * q1.y - q2.z * q1.z);
            newQ.x = (q2.w * q1.x + q2.x * q1.w - q2.y * q1.z + q2.z * q1.y);
            newQ.y = (q2.w * q1.y + q2.x * q1.z + q2.y * q1.w - q2.z * q1.x);
            newQ.z = (q2.w * q1.z - q2.x * q1.y + q2.y * q1.x + q2.z * q1.w);
            return newQ;
        }
        public bool isUnitary() {
            if (UnityEngine.Mathf.Sqrt(UnityEngine.Mathf.Pow(w, 2) + UnityEngine.Mathf.Pow(x, 2) + UnityEngine.Mathf.Pow(y, 2) + UnityEngine.Mathf.Pow(z, 2)) == 1) {
                return true;
            }
            return false;
        }

        public void invertQuaternion() {
            Normalize();
            x *= -1;
            y *= -1;
            z *= -1;
        }

        public void Normalize() {
            if (!isUnitary()) {
                float normal = UnityEngine.Mathf.Sqrt(UnityEngine.Mathf.Pow(w, 2) + UnityEngine.Mathf.Pow(x, 2) + UnityEngine.Mathf.Pow(y, 2) + UnityEngine.Mathf.Pow(z, 2));
                w /= normal;
                x /= normal;
                y /= normal;
                z /= normal;
            }
            //UnityEngine.Assertions.Assert.IsTrue (isUnitary(), "Quaterion is not unitary");
        }

        public static implicit operator ourQuaternion(UnityEngine.Quaternion q)
        {
            ourQuaternion temp = new ourQuaternion(q.x, q.y, q.z, q.w);
            return temp;
        }
        public static implicit operator UnityEngine.Quaternion(ourQuaternion q)
        {
            UnityEngine.Quaternion temp = new UnityEngine.Quaternion(q.x, q.y, q.z, q.w);
            return temp;
        }
    }

    public class ourParticle {

        private ourVector3 position;
        private ourVector3 velocity = new ourVector3();
        private ourVector3 force = new ourVector3();

        private ourVector3 rightForce;
        private ourVector3 leftForce;

        private float mass;

        public ourParticle (ourVector3 pos, float m)
        {
            position = pos;
            mass = m;
        }
        public void Update(float delta)
        {
            //Apliquem el solver de euler
            position += velocity * delta;
            velocity += (force / mass) * delta;
            //calculem les forces de la corda
            force = rightForce + leftForce;
            //apliquem la gravetat
            force += new ourVector3(0, -9.81f * mass * delta, 0);
            
        }

        public void CalculateStringForces(ourParticle leftP, ourParticle rightP, float ke, float kd, float longitude) {
            if (rightP != null)
            {
                ourVector3 AB = position - rightP.GetPos();
                rightForce = -1 * (ke * (AB.GetMagnitude() - longitude) + kd * ourVector3.Dot((velocity - rightP.velocity), (AB / AB.GetMagnitude()))) * AB / AB.GetMagnitude();
            }
            else rightForce = new ourVector3();
            if (leftP != null)
            {
                ourVector3 AB = position - leftP.GetPos();
                leftForce = -1 * (ke * (AB.GetMagnitude() - longitude) + kd * ourVector3.Dot((velocity - leftP.velocity), (AB / AB.GetMagnitude()))) * AB / AB.GetMagnitude();
            }
            else leftForce = new ourVector3();

        }

        public ourVector3 GetPos() {
            return position;
        }
    }
}
