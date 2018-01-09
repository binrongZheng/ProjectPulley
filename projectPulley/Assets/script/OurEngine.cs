using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

		public Vector3 position;
		public Vector3 velocity = new Vector3();
        private Vector3 force = new Vector3();

        public Vector3 rightForce = new Vector3();
        public Vector3 leftForce = new Vector3();

        private float mass;

		public bool isFixed;
		                
        public ourParticle (Vector3 pos, float m, bool isF)
        {
            position = pos;
            mass = m;
            isFixed = isF;
        }
        public void Update(float delta)
        {
			if (!isFixed && position.y > 0) { //si no es l'agarre apliquem el solver de euler
                
                position += velocity*delta;
				velocity += delta* (force / mass);
                //calculem les forces de la corda                
                force = rightForce + leftForce;
                //apliquem la gravetat
				force += new Vector3(0, -9.81f * mass*delta, 0);                      
                                   
            }
            
        }

		public bool PulleyCollision(Vector3 pulleyPos, float radius, float delta)
        {
            //calculem quina seria la seva seguent posicio
            Vector3 posCreuada = position + delta * velocity; ;

			Vector3 distVector = posCreuada - pulleyPos;
			float dist = distVector.magnitude;
            


            if (dist < radius)
			{
				
				//UnityEngine.Debug.DrawLine(position, position + velocity * 5, UnityEngine.Color.blue);
                //trobem el punt d'interseccio
				Vector3 dir = velocity.normalized; //normalitzem velocitat per fer la recta que surt de PosActual i va en dir de la velocitat

				float distIntersec;
				distIntersec = -Vector3.Dot(dir, (position - pulleyPos)) - UnityEngine.Mathf.Sqrt((Vector3.Dot(dir, (position - pulleyPos))) * (Vector3.Dot(dir, (position - pulleyPos))) - ( (position - pulleyPos).magnitude * (position - pulleyPos).magnitude ) + (radius * radius));
				Vector3 intersectionPoint = position + dir * distIntersec;

                //comprovem si el punt d'interseccio que hem calculat es el que tenim mes a prop o el de l'altre costat
				if ( Mathf.Abs( (position - intersectionPoint).magnitude /*+ (intersectionPoint - posCreuada).magnitude - (position - posCreuada).magnitude*/ ) > radius ){
					distIntersec = -Vector3.Dot(dir, (position - pulleyPos)) + UnityEngine.Mathf.Sqrt((Vector3.Dot(dir, (position - pulleyPos))) * (Vector3.Dot(dir, (position - pulleyPos))) - ((position - pulleyPos).magnitude * (position - pulleyPos).magnitude) + (radius * radius));
					intersectionPoint = position + dir * distIntersec;
				}
                 //vector interseccio-centre sera la normal del pla
				Vector3 n = (intersectionPoint - pulleyPos).normalized;

                 //calcular d del pla i pos de rebot

                 float d = -1*Vector3.Dot(n, intersectionPoint);
                 position = posCreuada - 2 * (Vector3.Dot(n, posCreuada) + d) * n;
                
                //elasticitat
				velocity += -(1+0.1f) * (n * Vector3.Dot(n, velocity));

                //friccion
                Vector3 vN = Vector3.Dot(n, velocity) * n;
                velocity += -1 * (velocity - vN); //velocity = velocity -u*vT

                //UnityEngine.Debug.DrawLine(position, position + velocity * 5, UnityEngine.Color.green);
				//UnityEngine.Debug.DrawLine(pulleyPos, intersectionPoint, UnityEngine.Color.black);          

				return true;

            }
			return false;
        }       
        
    }
}
