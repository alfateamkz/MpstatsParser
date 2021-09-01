using System;
using System.Collections.Generic;
using System.Text;
using MpstatsParser.Exceptions;

namespace MpstatsParser.Services
{
    public static class StatusCodeChecker
    {      
        public static void CheckStatusCode(int code)
        {
            if (code == 401)
            {
                throw new APIUnauthorizedException();
            }
            if (code == 429)
            {
                throw new APIRequestLimitException();
            }
            if (code == 500)
            {
                throw new InternalServerException();
            }
        }
    }
}
