namespace TheTecniQ.Core.Domain.Logging
{
    /// <summary>
    /// Represents a action
    /// </summary>
    public enum EnumLogAction
    {
        /*
         - Need to add enum option in alphabetical order, so that its value we can use directly in sorting queries
         - We will use some gap in two option e.g 10 gap, so that at any stage based on alphabetical order we can add new option
         - If option text need to change e.g from Edit to Update, than we need to update its value and text both and for old order need 
           to update in DB too with migreation, Enum has very less chance of change so should be ok.
         */

        /// <summary>
        /// Add
        /// </summary>
        Add = 10,

        /// <summary>
        /// Delete
        /// </summary>
        Delete = 20,

        /// <summary>
        /// Edit
        /// </summary>
        Edit = 30,
                
        /// <summary>
        /// Read
        /// </summary>
        Read = 40,

        /// <summary>
        /// Read
        /// </summary>
        Restore = 50,

        /// <summary>
        /// Read
        /// </summary>
        Login = 60,
    }
}
