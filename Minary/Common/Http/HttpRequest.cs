namespace Minary.Common.Http
{
  using System;
  using System.IO;
  using System.Net;
  using System.Text;

  public static class HttpRequest
  {

    /// <summary>
    ///
    /// </summary>
    /// <param name="url"></param>
    /// <returns></returns>
    public static bool HTTPPing(string url)
    {
      bool retVal = false;
      HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
      request.Timeout = 4000;
      request.Method = "HEAD";

      try
      {
        using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
        {
          if (response.StatusCode == HttpStatusCode.OK)
          {
            retVal = true;
          }
        }
      }
      catch (WebException)
      {
      }

      return retVal;
    }



    /// <summary>
    ///
    /// </summary>
    /// <param name="url"></param>
    /// <param name="postData"></param>
    /// <param name="userAgent"></param>
    /// <returns></returns>
    public static string SendPostRequest(string url, string postData, string userAgent)
    {
      string retVal = string.Empty;
      HttpWebRequest request;
      byte[] byteArray;
      Stream dataStream = null;
      HttpWebResponse webResponse = null;
      StreamReader reader = null;

      request = (HttpWebRequest)WebRequest.Create(url);
      request.UserAgent = userAgent;
      request.KeepAlive = false;
      request.Method = "POST";
      request.ServicePoint.Expect100Continue = false;
      request.Timeout = 10000;

      // Build request
      byteArray = Encoding.UTF8.GetBytes(postData);
      request.ContentType = "application/x-www-form-urlencoded";
      request.ContentLength = byteArray.Length;

      try
      {
        dataStream = request.GetRequestStream();
        dataStream.Write(byteArray, 0, byteArray.Length);

        webResponse = (HttpWebResponse)request.GetResponse();
        dataStream = webResponse.GetResponseStream();
        reader = new StreamReader(dataStream);
        retVal = reader.ReadToEnd();
      }
      catch (WebException)
      {
      }
      finally
      {
        if (reader != null)
        {
          reader.Close();
        }

        if (dataStream != null)
        {
          dataStream.Close();
        }

        if (webResponse != null)
        {
          webResponse.Close();
        }
      }

      return retVal;
    }




    /// <summary>
    ///
    /// </summary>
    /// <param name="url"></param>
    /// <param name="userAgent"></param>
    /// <param name="cookies"></param>
    /// <returns></returns>
    public static string SendGetRequest(string url, string userAgent, string cookies)
    {
      string retVal = string.Empty;
      HttpWebRequest request = null;
      HttpWebResponse webResponse = null;
      StreamReader reader = null;

      try
      {
        request = (HttpWebRequest)WebRequest.Create(url);
        if (userAgent.Length > 0)
        {
          request.UserAgent = userAgent;
        }

        request.Method = "GET";
        request.KeepAlive = false;
        request.Timeout = 10000;
        if (cookies.Length > 0)
        {
          request.Headers.Add("Cookie", cookies);
        }
      }
      catch (Exception)
      {
      }

      try
      {
        webResponse = (HttpWebResponse)request.GetResponse();
        reader = new StreamReader(webResponse.GetResponseStream());
        retVal = reader.ReadToEnd();
      }
      catch (WebException)
      {
      }
      finally
      {
        if (reader != null)
        {
          reader.Close();
        }

        if (webResponse != null)
        {
          webResponse.Close();
        }
      }

      return retVal;
    }



    /// <summary>
    ///
    /// </summary>
    /// <param name="url"></param>
    /// <param name="localFilePath"></param>
    public static void DownloadFile(string url, string localFilePath)
    {
      HttpWebRequest webRequest;
      HttpWebResponse webResponse = null;
      Stream downloadStream = null;
      FileStream fileStreamOutput = null;
      byte[] readBuf;
      int byteCount;

      try
      {
        webRequest = (HttpWebRequest)WebRequest.Create(url);
        webRequest.Timeout = 5000;
        webRequest.AllowWriteStreamBuffering = false;
        webResponse = (HttpWebResponse)webRequest.GetResponse();
        downloadStream = webResponse.GetResponseStream();

        // Write downloaded data to disk
        fileStreamOutput = new FileStream(localFilePath, FileMode.Create);
        readBuf = new byte[256];

        while ((byteCount = downloadStream.Read(readBuf, 0, readBuf.Length)) > 0)
        {
          fileStreamOutput.Write(readBuf, 0, byteCount);
        }
      }
      catch (Exception)
      {
        throw;
      }
      finally
      {
        if (fileStreamOutput != null)
        {
          fileStreamOutput.Close();
        }

        if (downloadStream != null)
        {
          downloadStream.Close();
        }

        if (webResponse != null)
        {
          webResponse.Close();
        }
      }
    }
  }
}
