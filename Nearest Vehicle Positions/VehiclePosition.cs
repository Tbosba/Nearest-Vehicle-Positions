using System;

namespace Nearest_Vehicle_Positions
{
    internal class VehiclePosition
    {
        public int PositionId { get; set; }
        public string? VehicleRegistraton { get; set; }
        public Position? Position {get;set;} 
        public ulong RecordedTimeUTC { get; set; }
    }
}