using System;
using System.Collections.Generic;
using System.Text;

namespace MpstatsParser.Exceptions
{
    public class InternalServerException : Exception
    {
        public InternalServerException() : base("Внутренняя ошибка сервера")
        {
           
        }
    }
}
