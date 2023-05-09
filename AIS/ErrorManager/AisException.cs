using System.Net;

namespace AIS.ErrorManager
{
    public class AisException: Exception
    {
        public HttpStatusCode StatusCode { get; set; }

        public AisException(string message, HttpStatusCode code) : base(message)
        {
            StatusCode = code;
        }
    }
}
