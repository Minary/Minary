namespace Minary.Common.Associations
{
  using Microsoft.Win32;
  using System;
  using System.Collections.Generic;
  using System.Security.AccessControl;

  /// <summary>
  /// AF_Lib's class for associating files with programs and icons.
  /// </summary>
  public class AF_FileAssociator
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="AF_FileAssociator"/> class.
    /// Initializes a new AF_FileAssociator class object for the specified file extension.
    /// </summary>
    /// <param name="extension">the file extension to control (such as .txt).</param>
    public AF_FileAssociator(string extension)
    {
      Extension = extension;
    }

    /// <summary>
    /// Gets the extension set for this file associator to control when you initialized it.
    /// </summary>
    public readonly string Extension;

    public string GetProgID
    {
      get
      {
        string toReturn = string.Empty;

        if (Registry.ClassesRoot.OpenSubKey(Extension,
            RegistryKeyPermissionCheck.ReadWriteSubTree,
            RegistryRights.FullControl) != null)
        {
          if (Registry.ClassesRoot.OpenSubKey(Extension,
              RegistryKeyPermissionCheck.ReadWriteSubTree,
              RegistryRights.FullControl).GetValue(string.Empty) != null)
          {
            toReturn = Registry.ClassesRoot.OpenSubKey(Extension,
                RegistryKeyPermissionCheck.ReadWriteSubTree,
                RegistryRights.FullControl).GetValue(string.Empty).ToString();
          }
        }

        return toReturn;
      }
    }

    /// <summary>
    /// Gets a value indicating whether the association keys exist. If the extension tmpKeyKey doesn't, the program cannot get the name of the program associatitmpKeytmpKey making it appear to not exist.
    /// </summary>
    public bool Exists
    {
      get
      {
        bool extKeyExists = false;
        bool progIDkeyExists = false;

        if (Registry.ClassesRoot.OpenSubKey(Extension) != null)
        {
          extKeyExists = true;

          if (GetProgID != null)
          {
            if (Registry.ClassesRoot.OpenSubKey(GetProgID) != null)
            {
              progIDkeyExists = true;
            }
          }
        }

        if (extKeyExists && progIDkeyExists)
        {
          return true;
        }
        else
        {
          return false;
        }
      }
    }

    /// <summary>
    /// Create or overwrite a current file association for this FileAssociator's set extension.
    /// </summary>
    /// <param name="progID">The basic application name that uses this file extension.</param>
    /// <param name="description">The desription of this file extension and/or program that uses it.</param>
    /// <param name="defaultIcon">The icon to show on the program and it's files.</param>
    /// <param name="execApp">The application that will be run when the file extension is clicked.</param>
    /// <param name="openWith">The programs that appear in the OpenWith list.</param>
    /// <exception cref="Exception">Thrown when an error occurs that will prevent it from working correctly.</exception>
    public void Create(string progID, string description, ProgramIcon defaultIcon, ExecApplication execApp, OpenWithList openWith)
    {
      if (progID == null)
      {
        throw new Exception("The program ID you entered is null...");
      }

      if (!defaultIcon.IsValid || !execApp.IsValid)
      {
        throw new Exception("Either the icon or executable application object is invalid...");
      }

      Registry.ClassesRoot.CreateSubKey(this.Extension).SetValue(string.Empty, progID);
      RegistryKey key = Registry.ClassesRoot.CreateSubKey(progID, RegistryKeyPermissionCheck.ReadWriteSubTree);

      if (description != null)
      {
        key.SetValue(string.Empty, description, RegistryValueKind.String);
      }

      if (defaultIcon != null && defaultIcon.IsValid)
      {
        key.CreateSubKey("DefaultIcon").SetValue(string.Empty, defaultIcon.IconPath, RegistryValueKind.String);
      }
      else
      {
        throw new Exception("The default icon you entered is either null or doesn't exist...");
      }

      if (execApp != null && execApp.IsValid)
      {
        key.CreateSubKey(@"Shell\Open\Command").SetValue(string.Empty, execApp.Path + " %1", RegistryValueKind.String);
      }
      else
      {
        throw new Exception("The executable application you entered is either null or not an .exe format...");
      }

      if (openWith != null)
      {
        key = key.CreateSubKey("OpenWithList", RegistryKeyPermissionCheck.ReadWriteSubTree);
        foreach (string file in openWith.List)
        {
          key.CreateSubKey(file);
        }
      }

      key.Flush();
      key.Close();
    }

    /// <summary>
    /// Gets or sets the program ID for this extension.
    /// </summary>
    public string ID
    {
      get
      {
        string toReturn = string.Empty;

        if (this.Exists)
        {
          if (Registry.ClassesRoot.OpenSubKey(
                                              this.Extension,
                                              RegistryKeyPermissionCheck.ReadWriteSubTree,
                                              RegistryRights.FullControl) != null)
          {
            toReturn = this.GetProgID;
          }
          else
          {
            throw new Exception($"The extension's association tmpKeyKey ({this.GetProgID}) doesn't exist, please use the Create() function to setup everything...");
          }
        }
        else
        {
          throw new Exception("One of your association keys don't exist, use the create method to get started...");
        }

        return toReturn;
      }

      set
      {
        if (this.Exists)
        {
          if (Registry.ClassesRoot.OpenSubKey(Extension,
              RegistryKeyPermissionCheck.ReadWriteSubTree,
              RegistryRights.FullControl) != null)
          {
            string beforeID = GetProgID;
            RegistryUtilities reg = new RegistryUtilities();
            reg.RenameSubKey(Registry.ClassesRoot, beforeID, value);

            Registry.ClassesRoot.OpenSubKey(Extension, RegistryKeyPermissionCheck.ReadWriteSubTree, RegistryRights.FullControl).SetValue(string.Empty, value, RegistryValueKind.String);
          }
          else
          {
            throw new Exception("The extension's association tmpKeyKey (" + GetProgID + ") doesn't exist, please use the Create() function to setup everything...");
          }
        }
        else
        {
          throw new Exception("One of your association keys don't exist, use the create method to get started...");
        }
      }
    }

    /// <summary>
    /// Gets or sets the description for this file extension and/or it's program association.
    /// </summary>
    public string Description
    {
      get
      {
        string toReturn = string.Empty;

        if (this.Exists)
        {
          if (Registry.ClassesRoot.OpenSubKey(Extension,
              RegistryKeyPermissionCheck.ReadWriteSubTree,
              RegistryRights.FullControl) != null)
          {
            if (Registry.ClassesRoot.OpenSubKey(GetProgID,
                RegistryKeyPermissionCheck.ReadWriteSubTree,
                RegistryRights.FullControl) != null)
            {
              if (Registry.ClassesRoot.OpenSubKey(GetProgID,
                  RegistryKeyPermissionCheck.ReadWriteSubTree,
                  RegistryRights.FullControl).GetValue(string.Empty) != null)
              {
                toReturn = Registry.ClassesRoot.OpenSubKey(GetProgID,
                    RegistryKeyPermissionCheck.ReadWriteSubTree,
                    RegistryRights.FullControl).GetValue(string.Empty).ToString();
              }
            }
            else
            {
              throw new Exception("The extension's progam association tmpKeyKey (" + GetProgID + ") doesn't exist, please use the Create() function to setup everything...");
            }
          }
          else
          {
            throw new Exception("The extension association tmpKeyKey doesn't exist, please use the Create() function to setup everything...");
          }
        }
        else
        {
          throw new Exception("One of your association keys don't exist, use the create method to get started...");
        }

        return toReturn;
      }

      set
      {
        if (this.Exists)
        {
          if (Registry.ClassesRoot.OpenSubKey(Extension,
              RegistryKeyPermissionCheck.ReadWriteSubTree,
              RegistryRights.FullControl) != null)
          {
            if (Registry.ClassesRoot.OpenSubKey(GetProgID,
                RegistryKeyPermissionCheck.ReadWriteSubTree,
                RegistryRights.FullControl) != null)
            {
              Registry.ClassesRoot.OpenSubKey(GetProgID,
                  RegistryKeyPermissionCheck.ReadWriteSubTree,
                  RegistryRights.FullControl).SetValue(string.Empty, value, RegistryValueKind.String);
            }
            else
            {
              throw new Exception("The extension's progam association tmpKeyKey (" + GetProgID + ") doesn't exist, please use the Create() function to setup everything...");
            }
          }
          else
          {
            throw new Exception("The extension association tmpKeyKey doesn't exist, please use the Create() function to setup everything...");
          }
        }
        else
        {
          throw new Exception("One of your association keys don't exist, use the create method to get started...");
        }
      }
    }

    /// <summary>
    /// Gets or sets the icon shown on this file extension and/or it's program association.
    /// </summary>
    public ProgramIcon DefaultIcon
    {
      get
      {
        ProgramIcon toReturn = null;

        if (this.Exists)
        {
          if (Registry.ClassesRoot.OpenSubKey(Extension,
              RegistryKeyPermissionCheck.ReadWriteSubTree,
              RegistryRights.FullControl) != null)
          {
            if (Registry.ClassesRoot.OpenSubKey(GetProgID,
                RegistryKeyPermissionCheck.ReadWriteSubTree,
                RegistryRights.FullControl) != null)
            {
              if (Registry.ClassesRoot.OpenSubKey(GetProgID + @"\DefaultIcon",
                  RegistryKeyPermissionCheck.ReadWriteSubTree,
                  RegistryRights.FullControl) != null)
              {
                if (Registry.ClassesRoot.OpenSubKey(GetProgID + @"\DefaultIcon",
                    RegistryKeyPermissionCheck.ReadWriteSubTree,
                    RegistryRights.FullControl).GetValue(string.Empty) != null)
                {
                  toReturn = new ProgramIcon(Registry.ClassesRoot.OpenSubKey(GetProgID + @"\DefaultIcon",
                      RegistryKeyPermissionCheck.ReadWriteSubTree,
                      RegistryRights.FullControl).GetValue(string.Empty).ToString());
                }
              }
            }
            else
            {
              throw new Exception("The extension's progam default icon association tmpKeyKey doesn't exist, please use the Create() function to setup everything...");
            }
          }
          else
          {
            throw new Exception("The extension association tmpKeyKey doesn't exist, please use the Create() function to setup everything...");
          }
        }
        else
        {
          throw new Exception("One of your association keys don't exist, use the create method to get started...");
        }

        return toReturn;
      }

      set
      {
        if (this.Exists)
        {
          if (value.IsValid)
          {
            if (Registry.ClassesRoot.OpenSubKey(Extension,
                RegistryKeyPermissionCheck.ReadWriteSubTree,
                RegistryRights.FullControl) != null)
            {
              if (Registry.ClassesRoot.OpenSubKey(GetProgID + @"\DefaultIcon",
                  RegistryKeyPermissionCheck.ReadWriteSubTree,
                  RegistryRights.FullControl) != null)
              {
                Registry.ClassesRoot.OpenSubKey(GetProgID + @"\DefaultIcon",
                    RegistryKeyPermissionCheck.ReadWriteSubTree,
                    RegistryRights.FullControl).SetValue(string.Empty, value.IconPath, RegistryValueKind.String);
              }
              else
              {
                throw new Exception("The extension's progam default icon association tmpKeyKey doesn't exist, please use the Create() function to setup everything...");
              }
            }
            else
            {
              throw new Exception("The extension association tmpKeyKey doesn't exist, please use the Create() function to setup everything...");
            }
          }
          else
          {
            throw new Exception("The value your trying to set to this DefaultIcon variable is not valid... the icon doesn't exist or it's not an .ico file.");
          }
        }
        else
        {
          throw new Exception("One of your association keys don't exist, use the create method to get started...");
        }
      }
    }

    /// <summary>
    /// Gets or sets the executable ran when this file extension is opened.
    /// </summary>
    public ExecApplication Executable
    {
      get
      {
        ExecApplication execApp = null;

        if (this.Exists)
        {
          if (Registry.ClassesRoot.OpenSubKey(Extension,
              RegistryKeyPermissionCheck.ReadWriteSubTree,
              RegistryRights.FullControl) != null)
          {
            if (Registry.ClassesRoot.OpenSubKey(GetProgID + @"\Shell\Open\Command",
                RegistryKeyPermissionCheck.ReadWriteSubTree,
                RegistryRights.FullControl) != null)
            {
              if (Registry.ClassesRoot.OpenSubKey(GetProgID + @"\Shell\Open\Command",
                  RegistryKeyPermissionCheck.ReadWriteSubTree,
                  RegistryRights.FullControl).GetValue(string.Empty) != null)
              {
                string path = Registry.ClassesRoot.OpenSubKey(GetProgID + @"\Shell\Open\Command",
                    RegistryKeyPermissionCheck.ReadWriteSubTree,
                    RegistryRights.FullControl).GetValue(string.Empty).ToString();

                execApp = new ExecApplication(path.Substring(0, path.LastIndexOf('%') - 1));
              }
            }
            else
            {
              throw new Exception("The extension's progam executable association tmpKeyKey doesn't exist, please use the Create() function to setup everything...");
            }
          }
          else
          {
            throw new Exception("The extension association tmpKeyKey doesn't exist, please use the Create() function to setup everything...");
          }
        }
        else
        {
          throw new Exception("One of your association keys don't exist, use the create method to get started...");
        }

        return execApp;
      }

      set
      {
        if (this.Exists)
        {
          if (value.IsValid)
          {
            if (Registry.ClassesRoot.OpenSubKey(Extension,
                RegistryKeyPermissionCheck.ReadWriteSubTree,
                RegistryRights.FullControl) != null)
            {
              if (Registry.ClassesRoot.OpenSubKey(GetProgID + @"\Shell\Open\Command",
                  RegistryKeyPermissionCheck.ReadWriteSubTree,
                  RegistryRights.FullControl) != null)
              {
                Registry.ClassesRoot.OpenSubKey(GetProgID + @"\Shell\Open\Command",
                    RegistryKeyPermissionCheck.ReadWriteSubTree,
                    RegistryRights.FullControl).SetValue(string.Empty, value.Path + " %1", RegistryValueKind.String);
              }
              else
              {
                throw new Exception("The extension's progam executable association tmpKeyKey doesn't exist, please use the Create() function to setup everything...");
              }
            }
            else
            {
              throw new Exception("The extension association tmpKeyKey doesn't exist, please use the Create() function to setup everything...");
            }
          }
          else
          {
            throw new Exception("The value uses to set this variable isn't valid... the file doesn't exist or it's not an .exe file.");
          }
        }
        else
        {
          throw new Exception("One of your association keys don't exist, use the create method to get started...");
        }
      }
    }

    /// <summary>
    /// Gets or sets the list of programs shown in the OpenWith list.
    /// </summary>
    public OpenWithList OpenWith
    {
      get
      {
        OpenWithList toReturn = null;

        if (this.Exists)
        {
          if (Registry.ClassesRoot.OpenSubKey(Extension,
              RegistryKeyPermissionCheck.ReadWriteSubTree,
              RegistryRights.FullControl) != null)
          {
            if (Registry.ClassesRoot.OpenSubKey(GetProgID + @"\OpenWithList",
                RegistryKeyPermissionCheck.ReadWriteSubTree,
                RegistryRights.FullControl) != null)
            {
              List<string> list = new List<string>();
              foreach (string file in Registry.ClassesRoot.OpenSubKey(GetProgID + @"\OpenWithList",
                   RegistryKeyPermissionCheck.ReadWriteSubTree,
                   RegistryRights.FullControl).GetSubKeyNames())
              {
                list.Add(file);
              }

              toReturn = new OpenWithList(list.ToArray());
              list.Clear();
            }
            else
            {
              throw new Exception("The extension's progam open with executable association tmpKeyKey doesn't exist, please use the Create() function to setup everything...");
            }
          }
          else
          {
            throw new Exception("The extension association tmpKeyKey doesn't exist, please use the Create() function to setup everything...");
          }
        }
        else
        {
          throw new Exception("One of your association keys don't exist, use the create method to get started...");
        }

        return toReturn;
      }

      set
      {
        if (this.Exists)
        {
          if (Registry.ClassesRoot.OpenSubKey(Extension,
              RegistryKeyPermissionCheck.ReadWriteSubTree,
              RegistryRights.FullControl) != null)
          {
            if (Registry.ClassesRoot.OpenSubKey(GetProgID + @"\OpenWithList",
                RegistryKeyPermissionCheck.ReadWriteSubTree,
                RegistryRights.FullControl) != null)
            {
              Registry.ClassesRoot.DeleteSubKeyTree(GetProgID + @"\OpenWithList");
              RegistryKey key = Registry.ClassesRoot.CreateSubKey(GetProgID + @"\OpenWithList",
                  RegistryKeyPermissionCheck.ReadWriteSubTree);

              foreach (string file in value.List)
              {
                key.CreateSubKey(file);
              }

              key.Close();
            }
            else
            {
              throw new Exception("The extension's progam open with executable association tmpKeyKey doesn't exist, please use the Create() function to setup everything...");
            }
          }
          else
          {
            throw new Exception("The extension association tmpKeyKey doesn't exist, please use the Create() function to setup everything...");
          }
        }
        else
        {
          throw new Exception("One of your association keys don't exist, use the create method to get started...");
        }
      }
    }

    /// <summary>
    /// Deletes all registry resources used for this file associations.
    /// </summary>
    public void Delete()
    {
      if (this.Exists)
      {
        if (Registry.ClassesRoot.OpenSubKey(Extension,
            RegistryKeyPermissionCheck.ReadWriteSubTree,
            RegistryRights.FullControl) != null)
        {
          try
          {
            Registry.ClassesRoot.DeleteSubKeyTree(GetProgID);
            Registry.ClassesRoot.DeleteSubKeyTree(Extension);
          }
          catch (Exception ex)
          {
            throw new Exception("Failed to delete all keys used in the '" + Extension + "' file association, error: " + ex.Message);
          }
        }
      }
      else
      {
        throw new Exception("One of your association keys don't exist, use the create method to get started...");
      }
    }
  }

}
