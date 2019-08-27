using imbSCI.Core.config;
using imbSCI.Core.reporting.render;

using System;
using System.Xml.Serialization;
//using WordPressRestApiStandard;

namespace imbSCI.Reporting.wordpress
{
    public class wordpressConnection
    {

        public string BaseUrl { get; set; } = "";
        public string username { get; set; } = "";

        public string password { get; set; } = "";

        
        public void Report(ITextRender output)
        {
            output.AppendLine(BaseUrl);
            if (username != "")
            {
                output.AppendLine(username);
                output.AppendLine(password);
            }
        }

        public AuthenticationTokens tokens() {

            var _tokens = new AuthenticationTokens()
            {
                ApplicationPassword = password, //"AxlI q7CQ 8JtR Us6I LHcK 6I0Y",
                UserName = username

            };

            return _tokens;
        }
       

        public Int32 blogid { get; set; } = 1;

        public wordpressConnection()
        {

        }
    }
}