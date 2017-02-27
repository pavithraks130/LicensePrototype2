namespace CalLicense.Core.Model
{
    /// <summary>
    /// This class holds credit card details 
    /// </summary>
    public class CardDetails
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public CardDetails()
        {

        }
        /// <summary>
        /// Credit card holder name 
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Credit card holder Number
        /// </summary>
        public string Number { get; set; }
        /// <summary>
        /// Credit card holder Expiry month
        /// </summary>
        public string Month { get; set; }
        /// <summary>
        /// Credit card expiry Year
        /// </summary>
        public short Years { get; set; }
        /// <summary>
        /// Credit card CVV number
        /// </summary>
        public short CVVNum { get; set; }
    }
}
