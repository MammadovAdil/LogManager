using System;

namespace Ma.LogManager
{
    public class ExceptionDetails
    {
        public string SpecialID { get; set; }
        public string MethodName { get; set; }
        public string ErrorMessage { get; set; }
        public string Note { get; set; }
        public DateTime ErrorDate { get; set; }
        public string StackTrace { get; set; }
    }
}
