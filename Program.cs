using CommandLine;
using System;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace smtpcmd
{
    /// <summary>
    /// Developed by De Tran.
    /// </summary>
    internal class Program
    {
        public class Options
        {
            [Option('h', "Host", Required = true, HelpText = "SMTP Host")]
            public string Host { get; set; }
            [Option('p', "Port", Required = true, Default = 25, HelpText = "SMTP Port")]
            public int Port { get; set; }
            [Option('l', "SSL", Required = false, Default = true, HelpText = "SSL Enabled")]
            public bool SSLEnabled { get; set; }
            [Option('u', "User", Required = false, HelpText = "User to log into SMTP server.  If omitted, DefaultCredential will be set to true.")]
            public string User { get; set; }
            [Option('a', "Password", Required = false, HelpText = "Password to log into SMTP server.  Password must be specified if User is specified.")]
            public string Password { get; set; }
            [Option('t', "To", Required = true, HelpText = "Email address to send mail To")]
            public string ToRecipient { get; set; }
            [Option('f', "From", Required = true, HelpText = "Email address the email originated from")]
            public string From { get; set; }
            [Option('s', "Subject", Required = false, Default = "Message from smtpcmd tool", HelpText = "Subject of the Email")]
            public string Subject { get; set; }
            [Option('b', "Body", Required = false, Default = "Message from smtpcmd tool has been delivered successfully.", HelpText = "Body of the Email")]
            public string Body { get; set; }

            [Option('e', "Encoding", Required = false, Default = "ASCII", HelpText = "Encoding: UTF8, UFT7, Unicode, ASCII, etc.")]
            public string Encoding { get; set; }
            [Option('o', "Timeout", Required = false, Default = 60000, HelpText = "Timeout in miliseconds.  Default to 1 minute.")]
            public int TimeoutInMiliseconds { get; set; }
            [Option('x', "Proxy", Required = false, HelpText = "Address of Proxy server")]
            public string ProxyAddress { get; set; }
            [Option('y', "ProxyPort", Required = false, HelpText = "Port of Proxy server")]
            public int ProxyPort { get; set; }
        }

        static void Main(string[] args)
        {
            Parser
               .Default
               .ParseArguments<Options>(args)
               .WithParsed<Options>(o =>
                {
                    using(var mail = new MailMessage(o.From, o.ToRecipient))
                    {
                        mail.Subject = o.Subject;
                        mail.Body = o.Body;

                        Encoding encoding = Encoding.ASCII;
                        switch(o.Encoding.ToLower())
                        {
                            case "ascii": encoding = Encoding.ASCII; break;
                            case "unicode": encoding = Encoding.Unicode; break;
                            case "utf7": encoding = Encoding.UTF7; break;
                            case "utf8": encoding = Encoding.UTF8; break;
                            case "utf32": encoding = Encoding.UTF32; break;
                        }
                        
                        mail.BodyEncoding = encoding;
                        mail.SubjectEncoding = encoding;
                        mail.HeadersEncoding = encoding;

                        // Send email out
                        try
                        {
                            // Set proxy server if configured
                            if (!string.IsNullOrEmpty(o.ProxyAddress))
                            {
                                WebRequest.DefaultWebProxy = new WebProxy(o.ProxyAddress, o.ProxyPort);
                            }

                            using (var smtp = new SmtpClient())
                            {
                                smtp.Host = o.Host;
                                smtp.Port = o.Port;
                                var defaultCredential = string.IsNullOrEmpty(o.User);
                                smtp.UseDefaultCredentials = defaultCredential;
                                if(!defaultCredential)
                                    smtp.Credentials = new NetworkCredential(o.User, o.Password);
                                smtp.EnableSsl = o.SSLEnabled;
                                smtp.Timeout = o.TimeoutInMiliseconds;

                                Log("Sending SMTP message...");
                                smtp.SendCompleted += (object sender, System.ComponentModel.AsyncCompletedEventArgs e) =>
                                {
                                    Log("SMTP Message has been sent successfully.");
                                };
                                smtp.Send(mail);
                                Log("SMTP Message has been sent successfully.");
                            }
                        }
                        catch (Exception ex)
                        {
                            Log(string.Format("SMTP Message failed to send with Error: {0}.\n{1}", ex.Message, ex.InnerException != null ? ex.InnerException.ToString() : string.Empty));
                        }
                    }
                });
        }

        private static void Log(string msg)
        {
            Console.WriteLine($"{DateTime.Now.ToLongTimeString()}: {msg}");
        }
    }
}
