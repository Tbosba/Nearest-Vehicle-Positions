using System;

namespace Nearest_Vehicle_Positions
{
    internal class Position
    {
        public string Name { get; set; }
        public float Latitude { get; set; }
        public float Longitude { get; set; }

        public Position() { }

        public Position(float mLatitude, float mLongitude,string mName)
        {
            this.Latitude = mLatitude;
            this.Longitude = mLongitude;
            this.Name = mName;
        }
    }
}