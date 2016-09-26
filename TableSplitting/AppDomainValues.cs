namespace TableSplitting
{
    using System;
    using System.IO;

    /// <summary>
    /// Wrapper for values in AppDomain
    /// </summary>
    public static class AppDomainValues
    {
        /// <summary>
        /// Sets the data directory, typically used in connectionstrings: AttachDbFileName=|DataDirectory|\database.mdf
        /// </summary>
        public static void SetDataDirectory()
        {
            var dataDirectory = Path.GetFullPath($@"{AppDomain.CurrentDomain.BaseDirectory}\..\..\App_Data");

            AppDomain.CurrentDomain.SetData("DataDirectory", dataDirectory);
        }
    }
}
