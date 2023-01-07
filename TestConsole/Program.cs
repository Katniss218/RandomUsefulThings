using Geometry;
using System;

namespace TestConsole
{
    class Program
    {
        static void Main( string[] args )
        {
            Quaternion q = Quaternion.FromEulerAngles( -1.4f, 0.5f, 1.6f );
            Vector3 euler = q.ToEulerAngles();
        }
    }
}
