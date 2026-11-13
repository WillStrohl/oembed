using WillStrohl.API.oEmbed;
using System; using System.Text.RegularExpressions;

namespace WillStrohl.API.oEmbed.Providers
{
    public abstract class ProviderBase { private string p_Proxy=""; private int p_Port=-1; protected string Proxy { get { return p_Proxy; } } protected int Port { get { return p_Port; } } protected bool HasProxy { get { return !string.IsNullOrEmpty(Proxy) && Port>-1; } } public ProviderBase(){} public ProviderBase(string a,int p){if(p>-1)p_Port=p;if(!string.IsNullOrEmpty(a))p_Proxy=a;} protected bool UseSSL(string URL){return Regex.IsMatch(URL,Constants.SSL_PATTERN,RegexOptions.IgnoreCase);} }}
