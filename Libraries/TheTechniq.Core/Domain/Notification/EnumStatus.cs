namespace TheTecniQ.Core.Domain.Notification
{
    /// <summary>
    /// Represents a mail queue status
    /// </summary>
    public enum EnumStatus
    {
        /*
         - Need to add enum option in alphabetical order, so that its value we can use directly in sorting queries
         - We will use some gap in two option e.g 10 gap, so that at any stage based on alphabetical order we can add new option
         - If option text need to change e.g from Edit to Update, than we need to update its value and text both and for old order need 
           to update in DB too with migreation, Enum has very less chance of change so should be ok.
         */

        /// <summary>
        /// Fail
        /// </summary>
        Fail = 10,

        /// <summary>
        /// In-Progress
        /// </summary>
        InProgress = 20,

        /// <summary>
        /// Pending
        /// </summary>
        Pending = 30,

        /// <summary>
        /// Sent
        /// </summary>
        Sent = 40,
    }
}
