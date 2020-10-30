namespace DroneProblem.DataClasses
{
    /// <summary>
    /// All the implementation is on BaseDrone on purpose
    /// </summary>
    public class Drone : BaseDrone
    {
        #region

        /// <inheritdoc />
        public Drone(string inId) : base(inId) { }

        #endregion
    }
}