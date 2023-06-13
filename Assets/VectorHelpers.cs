using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets
{
    public static class VectorHelpers
    {
        public static Vector2 RotateLeft(this Vector2 vector)
        {
            return new Vector2(vector.y * -1, vector.x);
        }

        public static Vector2 RotateRight(this Vector2 vector)
        {
            return new Vector2(vector.y, vector.x * -1);
        }

        public static Vector3Int ToVector3Int(this Vector2Int vector)
        {
            return new Vector3Int(vector.x, vector.y, 0);
        }

        public static float ToRotation(this Vector2 vector)
        {
            return Mathf.Atan2(vector.y, vector.x) * Mathf.Rad2Deg;
            throw new NotImplementedException("Unsupported vector for rotation");
        }

        public static Vector2 RotateBy(this Vector2 v, float degrees)
        {
            float radians = degrees * Mathf.Deg2Rad;
            float sin = Mathf.Sin(radians);
            float cos = Mathf.Cos(radians);

            float tx = v.x;
            float ty = v.y;

            return new Vector2(cos * tx - sin * ty, sin * tx + cos * ty);
        }

        public static Vector2 FromRotation(float rotation)
        {
            return new Vector2(Mathf.Sin(rotation * Mathf.Deg2Rad), Mathf.Cos(rotation * Mathf.Deg2Rad));
        }

        public static Vector2Int AsIntVector(this Vector2 vector)
        {
            return new Vector2Int((int)vector.x, (int)vector.y);
        }
    }
}
