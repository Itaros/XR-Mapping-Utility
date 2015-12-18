using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Itaros.Math
{
    public struct Vector3
    {

        public Vector3(decimal x, decimal y, decimal z){
            this.X=x;
            this.Y=y;
            this.Z=z;
        }

        public decimal X, Y, Z;

        public static Vector3 operator - (Vector3 a, Vector3 b){
            return new Vector3(a.X-b.X, a.Y-b.Y, a.Z-b.Z);
        }
        public static Vector3 operator +(Vector3 a, Vector3 b)
        {
            return new Vector3(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        }
        public static Vector3 operator /(Vector3 a, decimal b)
        {
            return new Vector3(a.X / b, a.Y / b, a.Z / b);
        }
        public static Vector3 operator *(Vector3 a, decimal b)
        {
            return new Vector3(a.X * b, a.Y * b, a.Z * b);
        }

        public string GetXRPositionXML()
        {
            return "<position x=\"" + X + "\" y=\"" + Y + "\" z=\"" + Z + "\" />";
        }

        public string GetXRArguments()
        {
            return "x=\"" + X + "\" y=\"" + Y + "\" z=\"" + Z + "\"";
        }

    }
}
