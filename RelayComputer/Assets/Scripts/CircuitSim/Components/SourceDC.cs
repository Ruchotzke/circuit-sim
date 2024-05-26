namespace CircuitSim.Components
{
    /// <summary>
    /// A model of a DC supply.
    /// </summary>
    public class SourceDC : Component
    {
        /// <summary>
        /// Generate a new resistor.
        /// </summary>
        /// <param name="voltage"></param>
        public SourceDC(float voltage)
        {
            IsVoltageSource = true;
            _sourceVoltage = voltage;
        }
    }
}