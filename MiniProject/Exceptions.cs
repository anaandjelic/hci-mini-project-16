using System;
using System.Collections.Generic;
using System.Text;

namespace MiniProject
{
    public class NoValuesFoundException : Exception
    {
        public NoValuesFoundException()
        {
        }
    }

    public class APITimedOutException : Exception
    {
        public APITimedOutException()
        {
        }
    }
}
