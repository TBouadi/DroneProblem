namespace DroneProblem.DataClasses
{
    #region Using

    using System;

    #endregion

    public class TrafficReport
    {
        #region Properties

        public string DroneId { get; set; }

        public double Speed { get; set; }

        public DateTime Time { get; set; }

        public TrafficConditionEnum TrafficCondition => (TrafficConditionEnum) Enum
            .GetValues(typeof(TrafficConditionEnum))
            .GetValue(new Random().Next(0, 2));

        #endregion
    }
}