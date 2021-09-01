using System;
using System.Collections.Generic;
using System.Text;

namespace MpstatsParser.Exceptions
{
    public class APIRequestLimitException : Exception
    {
        public APIRequestLimitException() : base("Исчерпан лимит запросов по текущему тарифному плану")
        {
           
        }
    }
}
