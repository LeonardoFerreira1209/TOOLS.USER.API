namespace APPLICATION.DOMAIN.DTOS.REQUEST.USER
{
    /// <summary>
    /// Classe de envio das claims
    /// </summary>
    public class ClaimRequest
    {
        public ClaimRequest(string type, string value)
        {
            Type = type;

            Value = value;
        }

        /// <summary>
        /// Nome da claim.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Valor da claim.
        /// </summary>
        public string Value { get; set; }
    }
}
