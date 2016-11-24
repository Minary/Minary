using System;
using System.IO;

namespace TestsHttpReverseProxy.Lib
{
  public class FileAction
  {

    /// <summary>
    /// 
    /// </summary>
    /// <param name="directory"></param>
    /// <returns></returns>
    public static bool CreateDirectoryIfNotExists(string directory)
    {
      if (string.IsNullOrEmpty(directory))
      {
        return false;
      }

      if (string.IsNullOrEmpty(directory))
      {
        return false;
      }

      if (Directory.Exists(directory))
      {
        return true;
      }

      try
      {
        Directory.CreateDirectory(directory);
        return true;
      }
      catch (Exception ex)
      {
        Console.WriteLine("CreateDirectoryIfNotExists(EXCEPTION): {0}", ex.Message);
        return false;
      }
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="directory"></param>
    /// <returns></returns>
    public static bool RemoveDirectoryIfExists(string directory)
    {
      if (string.IsNullOrEmpty(directory))
      {
        return false;
      }

      if (string.IsNullOrEmpty(directory))
      {
        return false;
      }

      if (!Directory.Exists(directory))
      {
        return true;
      }

      try
      {
        Directory.Delete(directory, true);
        return true;
      }
      catch (Exception ex)
      {
        Console.WriteLine("RemoveDirectoryIfExists(EXCEPTION): {0}", ex.Message);
        return false;
      }
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="source"></param>
    /// <param name="destination"></param>
    /// <returns></returns>
    public static bool CopyFileIfNotExists(string source, string destination)
    {
      if (string.IsNullOrEmpty(source) || string.IsNullOrEmpty(destination))
      {
        return false;
      }

      try
      {
        File.Copy(source, destination);
        return true;
      }
      catch (Exception ex)
      {
        Console.WriteLine("CopyFileIfNotExists(EXCEPTION): {0}", ex.Message);
        return false;
      }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="filePath"></param>
    /// <returns></returns>
    public static bool RemoveFileIfExists(string filePath)
    {
      if (string.IsNullOrEmpty(filePath))
      {
        return false;
      }

      try
      {
        File.Delete(filePath);
        Console.WriteLine("RemoveFileIfExists(): DONE");
        return true;
      }
      catch (Exception ex)
      {
        Console.WriteLine("RemoveFileIfExists(EXCEPTION): {0}", ex.Message);
        return false;
      }
    }


    public static bool WriteToFile(string filePath, string content)
    {
      if (string.IsNullOrEmpty(filePath))
      {
        return false;
      }

      Console.WriteLine("WriteToFile(): Remove:{0}", filePath);
      RemoveFileIfExists(filePath);

      try
      {
        if (content == null)
        {
          content = string.Empty;
        }

        Console.WriteLine("WriteToFile(): WriteTo:{0}, Content:{1}", filePath, content);
        File.AppendAllText(filePath, content);
        if (File.Exists(filePath))
          Console.WriteLine("WriteToFile(): Done successfully");
        else
          Console.WriteLine("WriteToFile(): Not one");
        return true;
      }
      catch (Exception ex)
      {
        Console.WriteLine("WriteToFile(EXCEPTION): {0}", ex.Message);
        return false;
      }
    }
  }
}
