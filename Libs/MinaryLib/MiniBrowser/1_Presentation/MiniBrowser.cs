using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Windows.Forms;



namespace Minary.MiniBrowser
{

  public partial class Browser : Form
  {

    #region IMPORTS

    [DllImport("wininet.dll", CharSet = CharSet.Auto, SetLastError = true)]
    public static extern bool InternetSetCookie(string lpszUrlName, string lbszCookieName, string lpszCookieData);

    [DllImport("wininet.dll", SetLastError = true, CharSet = CharSet.Auto, EntryPoint = "DeleteUrlCacheEntryA", CallingConvention = CallingConvention.StdCall)]
    public static extern bool DeleteUrlCacheEntry(string lpszUrlName);

    [DllImport("urlmon.dll", CharSet = CharSet.Ansi)]
    private static extern int UrlMkSetSessionOption(int dwOption, string pBuffer, int dwBufferLength, int dwReserved); const int URLMON_OPTION_USERAGENT = 0x10000001;

    #endregion


    #region DATATYPES

    enum UserAgentType
    {
      Custom = 0,
      InternetExplorer = 1
    }

    #endregion


    #region MEMBERS

    private readonly string userAgentIE = "Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.1; WOW64; Trident/6.0)";
    private readonly string[] userAgentSettings = new string[] { "Custom", "IE" };
    private static string headerData;
    private string userAgentCustom;
    private string cookies;
    private TaskFacade taskLayer;

    #endregion


    #region PUBLIC

    /// <summary>
    /// Initializes a new instance of the <see cref="Browser"/> class.
    ///
    /// </summary>
    /// <param firewallRuleName="url"></param>
    /// <param firewallRuleName="cookie"></param>
    /// <param firewallRuleName="srcIp"></param>
    /// <param firewallRuleName="userAgent"></param>
    public Browser(string url, string cookie, string srcIp, string userAgent)
    {
      this.InitializeComponent();

      this.taskLayer = TaskFacade.GetInstance();
      this.cmb_UserAgent.DataSource = this.userAgentSettings;
      this.cmb_UserAgent.SelectedIndex = 1;

      this.tb_URL.Text = url;
      this.tb_Cookies.Text = cookie;
      this.cookies = cookie;

      this.userAgentCustom = userAgent;
      this.cmb_UserAgent.SelectedIndex = 0;
      this.Text = "MiniBrowser 0.3";


      if (!string.IsNullOrEmpty(url))
      {
        if (!url.ToLower().StartsWith("http"))
        {
          url = string.Format("http://{0}", url);
          this.tb_URL.Text = url;
        }
      }


      string requestedUrl = this.tb_URL.Text;

      headerData = string.Empty;

      this.taskLayer.ClearIECache();
      this.taskLayer.ClearCookies();

      if (this.cb_Cookies.Checked && this.tb_Cookies.Text.Length > 0)
      {
        try
        {
          foreach (string tmpCookie in this.tb_Cookies.Text.ToString().Split(';'))
          {
            if (tmpCookie.Length > 0 && tmpCookie.Contains("="))
            {
              Regex regex = new Regex("=");
              string[] substrings = regex.Split(tmpCookie, 2);

              InternetSetCookie(requestedUrl, substrings[0], substrings[1]);
            }
          }
        }
        catch (Exception)
        {
        }
      }

      if (this.cb_UserAgent.Checked && this.tb_UserAgent.Text.Length > 0)
      {
        headerData = "User-Agent: " + this.tb_UserAgent.Text + "\r\n";
      }
      else
      {
        headerData = string.Format("User-Agent: {0}\r\n", this.tb_UserAgent.Text);
      }

      if (this.cb_Cookies.Checked && this.tb_Cookies.Text.Length > 0)
      {
        headerData += "Cookie: " + this.tb_Cookies.Text + "\r\n";
      }

      DeleteUrlCacheEntry(requestedUrl);
      this.taskLayer.ClearIECache();
      this.taskLayer.ClearCookies();

      UrlMkSetSessionOption(URLMON_OPTION_USERAGENT, this.tb_UserAgent.Text, this.tb_UserAgent.Text.Length, 0);
    }

    #endregion


    #region EVENTS

    /// <summary>
    ///
    /// </summary>
    /// <param firewallRuleName="sender"></param>
    /// <param firewallRuleName="e"></param>
    private void BT_Open_Click(object sender, EventArgs e)
    {
      string url = this.tb_URL.Text;
      string host = string.Empty;
      string tmpHost = string.Empty;

      if (!string.IsNullOrEmpty(url))
      {
        if (!url.Contains(Uri.SchemeDelimiter))
        {
          url = string.Concat(Uri.UriSchemeHttp, Uri.SchemeDelimiter, url);
          this.tb_URL.Text = url;
        }
      }

      try
      {
        Uri uri = new Uri(url);
        host = uri.Host;
      }
      catch (Exception)
      {
      }

      headerData = string.Empty;

      this.taskLayer.ClearIECache();
      this.taskLayer.ClearCookies();


      if (this.cb_Cookies.Checked && this.tb_Cookies.Text.Length > 0)
      {
        try
        {
          foreach (string tmpCookie in this.tb_Cookies.Text.ToString().Split(';'))
          {
            if (tmpCookie.Length > 0 && tmpCookie.Contains("="))
            {
              Regex regex = new Regex("=");
              string[] substrings = regex.Split(tmpCookie, 2);

              InternetSetCookie(url, substrings[0], substrings[1]);
            }
          }
        }
        catch (Exception)
        {
        }
      }

      headerData = "User-Agent: " + this.tb_UserAgent.Text + "\r\n";
      headerData = "Host: " + host + "\r\n";

      if (this.cb_Cookies.Checked && this.tb_Cookies.Text.Length > 0)
      {
        headerData += "Cookie: " + this.tb_Cookies.Text + "\r\n";
      }

      DeleteUrlCacheEntry(url);
      this.taskLayer.ClearIECache();
      this.taskLayer.ClearCookies();

      UrlMkSetSessionOption(URLMON_OPTION_USERAGENT, this.tb_UserAgent.Text, this.tb_UserAgent.Text.Length, 0);
      this.wb_MiniBrowser.ScriptErrorsSuppressed = true;
      this.wb_MiniBrowser.Navigate(url, string.Empty, null, headerData);
    }



    /// <summary>
    ///
    /// </summary>
    /// <param firewallRuleName="isEnabled"></param>
    public delegate void ActivateGBDetailsDelegate(bool pEnabled);
    public void ActivateGBDetails(bool isEnabled)
    {
      if (this.InvokeRequired)
      {
        this.BeginInvoke(new ActivateGBDetailsDelegate(this.ActivateGBDetails), new object[] { isEnabled });
        return;
      }

      if (isEnabled == false)
      {
        this.gb_Details.Enabled = false;
        //// gb_WebPage.Enabled = false;
        //// Cursor = Cursors.WaitCursor;
      }
      else
      {
        this.gb_Details.Enabled = true;
        //// gb_WebPage.Enabled = true;
        //// Cursor = Cursors.Default;
      }
    }




    /// <summary>
    ///  HTTP request Access token.
    ///  This is the tricky part! If somebody knows an easier way to get an AccessToken
    ///  -> let me know.
    /// </summary>
    /// <param firewallRuleName="sender"></param>
    /// <param firewallRuleName="e"></param>
    private void BGW_GetAccessToken_DoWork(object sender, DoWorkEventArgs e)
    {
    }


    /// <summary>
    ///
    /// </summary>
    /// <param firewallRuleName="sender"></param>
    /// <param firewallRuleName="e"></param>
    private void TB_URL_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode == Keys.Enter)
      {
        e.SuppressKeyPress = true;
        this.BT_Open_Click(null, null);
      }
    }


    /// <summary>
    ///
    /// </summary>
    /// <param firewallRuleName="sender"></param>
    /// <param firewallRuleName="e"></param>
    private void CMB_UserAgent_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.cmb_UserAgent.SelectedIndex != (int)UserAgentType.Custom)
      {
        this.userAgentCustom = this.tb_UserAgent.Text;
      }

      switch (this.cmb_UserAgent.SelectedIndex)
      {
        case (int)UserAgentType.Custom:
          this.tb_UserAgent.Text = this.userAgentCustom;
          this.tb_UserAgent.Enabled = true;
          break;
        default:
          this.tb_UserAgent.Text = this.userAgentIE;
          this.tb_UserAgent.Enabled = false;
          break;
      }
    }

    #endregion

  }
}
