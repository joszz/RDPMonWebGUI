using System.Collections.Generic;

namespace RDPMonWebGUI.Models
{
    public class ErrorViewModel
    {
        private Dictionary<int, string> StatusCodeMessages { get; set; } = new Dictionary<int, string>
        {
            { 404, "Page not found!"}
        };

        public int StatusCode { get; set; }

        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

        public string ErrorMessage
        {
            get
            {
                return StatusCodeMessages.ContainsKey(StatusCode) ? StatusCodeMessages[StatusCode] : string.Empty;
            }
        }
    }
}
