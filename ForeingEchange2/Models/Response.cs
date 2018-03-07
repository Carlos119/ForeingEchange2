namespace ForeingEchange2.Models
{
    using System;

    public class Response
    {
        public bool IsSuccess
        {
            get;
            set;
        }

        public String Message
        {
            get;
            set;
        }

        public object Result
        {
            get;
            set;
        }
    }
}
