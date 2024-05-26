namespace CircuitSim.Components
{
    /// <summary>
    /// A model of a resistor.
    /// </summary>
    public class Resistor : Component
    {
        /// <summary>
        /// Generate a new resistor.
        /// </summary>
        /// <param name="resistance"></param>
        public Resistor(float resistance)
        {
            IsVoltageSource = false;
            _impedance = resistance;
        }
    }
}