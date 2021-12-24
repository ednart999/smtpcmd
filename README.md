# smtpcmd
Small SMTP client program using C# 

smtpcmd parameters:

  -h, --Host         Required. SMTP Host

  -p, --Port         Required. (Default: 25) SMTP Port

  -l, --SSL          (Default: true) SSL Enabled

  -u, --User         User to log into SMTP server.  If omitted,
                     DefaultCredential will be set to true.

  -a, --Password     Password to log into SMTP server.  Password must be
                     specified if User is specified.

  -t, --To           Required. Email address to send mail To

  -f, --From         Required. Email address the email originated from

  -s, --Subject      (Default: Message from smtpcmd tool) Subject of the Email

  -b, --Body         (Default: Message from smtpcmd tool has been delivered
                     successfully.) Body of the Email

  -e, --Encoding     (Default: ASCII) Encoding: UTF8, UFT7, Unicode, ASCII, etc.

  -o, --Timeout      (Default: 60000) Timeout in miliseconds.  Default to 1
                     minute.

  -x, --Proxy        Address of Proxy server

  -y, --ProxyPort    Port of Proxy server

  --help             Display this help screen.

  --version          Display version information.

Examples:
1) SMTP server hosted by Mochahost
smtpcmd.exe --Host mi3-wts6.my-hosting-panel.com --Port 587 --SSL true --User [Login-user] --Password [Login-password] --To [To-email] --From [From-email] --Subject "Test" --Body "Test" --Encoding ASCII

2) SMTP server hosted by Microsoft
smtpcmd.exe --Host smtp-mail.outlook.com --Port 587 --SSL true --User [Login-user] --Password [Login-password] --To [To-email] --From [From-email] --Subject "Test from Microsoft SMTP" --Body "Test" --Encoding ascii

4) SMTP server hosted by Google
To try out Google SMTP, you need to do a few steps below since Google doesn't trust apps that it doesn't know about:
  a) Turn off Multi-Factor authentication first (https://myaccount.google.com/security)
  b) Turn on Less Secure Apps option (https://myaccount.google.com/lesssecureapps)
smtpcmd.exe --Host smtp.gmail.com --Port 587 --SSL true --User [Login-user] --Password [Login-password] --To [To-email] --From [From-email] --Subject "Test from Google SMTP" --Body "Test" --Encoding ascii


Build solution:
  The solution is a C# Visual Studio 2022 Community solution with .NET Framework 4.8.
  After having Visual Studio 2022 installed, Open smtpcmd.sln with VS2022. It should be building without errors.
  
