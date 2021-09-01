using System;
using System.Collections.Generic;
using System.Text;

namespace MpstatsParser.Exceptions
{
    public class APIUnauthorizedException : Exception
    {
        public APIUnauthorizedException() : base("Неверный API ключ")
        {
           
        }
    }
}
