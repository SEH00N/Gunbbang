using UnityEngine;

namespace GB.Extensions
{
    public static class VectorExtensions
    {
        public static Vector3 GetRandom(this Vector3 left, Vector3 right)
        {
            float posX = Random.Range(left.x, right.x);
            float posY = Random.Range(left.y, right.y);
            float posZ = Random.Range(left.z, right.z);

            return new Vector3(posX, posY, posZ);
        }

        public static Vector3 GetMultipleEach(this Vector3 left, Vector3 right)
        {
            Vector3 value = Vector3.zero;
            value.x = left.x * right.x;
            value.y = left.x * right.y;
            value.z = left.z * right.z;

            return value;
        }

        public static Vector3 PlaneVector(this Vector3 left)
        {
            left.y = 0;
            return left;
        }

        public static Vector3 Sign(this Vector3 left, bool includeZero = false)
        {
            Vector3 value = Vector3.zero;
            value.x = Mathf.Sign(left.x);
            value.y = Mathf.Sign(left.y);
            value.z = Mathf.Sign(left.z);        

            if(includeZero)
            {
                if (left.x == 0)
                    value.x = 0;
                if (left.y == 0)
                    value.y = 0;
                if (left.z == 0)
                    value.z = 0;
            }

            return value;
        }

        public static Vector3Int GetRoundEach(this Vector3 left)
        {
            Vector3Int value = Vector3Int.zero;
            value.x = Mathf.RoundToInt(left.x);
            value.y = Mathf.RoundToInt(left.y);
            value.z = Mathf.RoundToInt(left.z);
            return value;
        }

        #region Division
        public static Vector3 GetDivisionEach(this Vector3 left, Vector3 right)
        {
            Vector3 value = Vector3.zero;
            value.x = left.x / right.x;
            value.y = left.y / right.y;
            value.z = left.z / right.z;

            return value;
        }

        public static Vector3Int GetDivisionEach(this Vector3Int left, Vector3Int right)
        {
            Vector3Int value = Vector3Int.zero;
            value.x = left.x / right.x;
            value.y = left.y / right.y;
            value.z = left.z / right.z;

            return value;
        }
        #endregion

        #region Mod
        public static Vector3 GetModEach(this Vector3 left, Vector3 right)
        {
            Vector3 value = Vector3.zero;
            value.x = left.x % right.x;
            value.y = left.y % right.y;
            value.z = left.z % right.z;

            return value;
        }

        public static Vector3Int GetModEach(this Vector3Int left, Vector3Int right)
        {
            Vector3Int value = Vector3Int.zero;
            value.x = left.x % right.x;
            value.y = left.y % right.y;
            value.z = left.z % right.z;

            return value;
        }
        #endregion

        #region Clamp
        public static Vector3 GetClampEach(this Vector3 left, Vector3 min, Vector3 max)
        {
            if (min.x > max.x)
                (min.x, max.x) = (max.x, min.x);
            if (min.y > max.y)
                (min.y, max.y) = (max.y, min.y);
            if (min.z > max.z)
                (min.z, max.z) = (max.z, min.z);

            Vector3 value = Vector3.zero;
            value.x = Mathf.Clamp(left.x, min.x, max.x);
            value.y = Mathf.Clamp(left.y, min.y, max.y);
            value.z = Mathf.Clamp(left.z, min.z, max.z);
            return value;
        }

        public static Vector3Int GetClampEach(this Vector3Int left, Vector3Int min, Vector3Int max)
        {
            if(min.x > max.x)
                (min.x, max.x) = (max.x, min.x);
            if(min.y > max.y)
                (min.y, max.y) = (max.y, min.y);
            if(min.z > max.z)
                (min.z, max.z) = (max.z, min.z);

            Vector3Int value = Vector3Int.zero;
            value.x = Mathf.Clamp(left.x, min.x, max.x);
            value.y = Mathf.Clamp(left.y, min.y, max.y);
            value.z = Mathf.Clamp(left.z, min.z, max.z);
            return value;
        }
        #endregion

        #region Abs
        public static void Abs(this Vector3 left)
        {
            left.x = Mathf.Abs(left.x);
            left.y = Mathf.Abs(left.y);
            left.z = Mathf.Abs(left.z);
        }

        public static Vector3 GetAbs(this Vector3 left)
        {
            Vector3 value = Vector3.zero;
            value.x = Mathf.Abs(left.x);
            value.y = Mathf.Abs(left.y);
            value.z = Mathf.Abs(left.z);
            
            return value;
        }

        public static void Abs(this Vector3Int left)
        {
            left.x = Mathf.Abs(left.x);
            left.y = Mathf.Abs(left.y);
            left.z = Mathf.Abs(left.z);
        }

        public static Vector3Int GetAbs(this Vector3Int left)
        {
            Vector3Int value = Vector3Int.zero;
            value.x = Mathf.Abs(left.x);
            value.y = Mathf.Abs(left.y);
            value.z = Mathf.Abs(left.z);
            
            return value;
        }
        #endregion
    }
}
