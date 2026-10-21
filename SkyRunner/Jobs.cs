using System;

namespace SkyRunner
{
    public static class JobStatus
    {
        public const string Available = "Available";
        public const string Current = "Current";
        public const string Done = "Done";
        public const string Lost = "Lost";
    }

    public static class JobTaskType
    {
        public const string Passengers = "Passengers";
        public const string Cargo = "Cargo";
    }

    public class Job
    {
        public string OriginIcao { get; set; }
        public string DestinationIcao { get; set; }
        public string DestinationName { get; set; }
        public double DistanceNm { get; set; }
        public string TaskType { get; set; }
        public int PassengerCount { get; set; }
        public double CargoWeightKg { get; set; }
        public string Status { get; set; }
        public DateTime CreatedUtc { get; set; }
        public DateTime? CompletedUtc { get; set; }

        public string ToDisplayString() {
            string payload = TaskType == JobTaskType.Passengers
                ? $"{PassengerCount} pax"
                : $"{CargoWeightKg:F0} kg";

            return $"{OriginIcao} → {DestinationIcao} | {DistanceNm:F0} nm | {TaskType} ({payload})";
        }


    }
}
