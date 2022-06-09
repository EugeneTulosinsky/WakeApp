namespace WakeApp.Type
{
    using System;

    public sealed class Model
    {
        public DateTime Arrival { get; set; }

        public int TravelTimeInMin { get; set; }

        public int PrepTimeInMin { get; set; }

        public int Delay { get; set; }

        public DateTime WakeTime { get; set; }

        public Model()
        {
        }

        public Model(DateTime arrival, int travelTime, int prepTime, DateTime wakeTime)
        {
            this.Arrival = arrival;
            this.TravelTimeInMin = travelTime;
            this.PrepTimeInMin = prepTime;
            this.WakeTime = wakeTime;
        }

        public Model(DateTime arrival, int travelTime, int prepTime, int delay, DateTime wakeTime)
        {
            this.Arrival = arrival;
            this.TravelTimeInMin = travelTime;
            this.PrepTimeInMin = prepTime;
            this.Delay = delay;
            this.WakeTime = wakeTime;
        }
    }
}
