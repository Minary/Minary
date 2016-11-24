namespace Minary.Common.Associations
{
  using Microsoft.Win32;
  using System.Security.AccessControl;

  public class RegistryUtilities
  {

    /// <summary>
    ///
    /// </summary>
    /// <param name="parentKey"></param>
    /// <param name="subKeyName"></param>
    /// <param name="newSubKeyName"></param>
    /// <returns></returns>
    public bool RenameSubKey(RegistryKey parentKey, string subKeyName, string newSubKeyName)
    {
      CopyKey(parentKey, subKeyName, newSubKeyName);
      parentKey.DeleteSubKeyTree(subKeyName);
      return true;
    }


    /// <summary>
    ///
    /// </summary>
    /// <param name="parentKey"></param>
    /// <param name="keyNameToCopy"></param>
    /// <param name="newKeyName"></param>
    /// <returns></returns>
    public bool CopyKey(RegistryKey parentKey, string keyNameToCopy, string newKeyName)
    {
      // Create new tmpKeyKey
      RegistryKey destinationKey = parentKey.CreateSubKey(newKeyName, RegistryKeyPermissionCheck.ReadWriteSubTree);

      // Open the sourceKey we are copying from
      RegistryKey sourceKey = parentKey.OpenSubKey(keyNameToCopy, RegistryKeyPermissionCheck.ReadWriteSubTree, RegistryRights.FullControl);

      RecurseCopyKey(sourceKey, destinationKey);

      return true;
    }


    /// <summary>
    ///
    /// </summary>
    /// <param name="sourceKey"></param>
    /// <param name="destinationKey"></param>
    private void RecurseCopyKey(RegistryKey sourceKey, RegistryKey destinationKey)
    {
      // copy all the values
      foreach (string valueName in sourceKey.GetValueNames())
      {
        object objValue = sourceKey.GetValue(valueName);
        RegistryValueKind valKind = sourceKey.GetValueKind(valueName);
        destinationKey.SetValue(valueName, objValue, valKind);
      }

      // For Each subKey
      // Create a new subKey in destinationKey
      // Call myself
      foreach (string sourceSubKeyName in sourceKey.GetSubKeyNames())
      {
        RegistryKey sourceSubKey = sourceKey.OpenSubKey(sourceSubKeyName, RegistryKeyPermissionCheck.ReadWriteSubTree, RegistryRights.FullControl);
        RegistryKey destSubKey = destinationKey.CreateSubKey(sourceSubKeyName, RegistryKeyPermissionCheck.ReadWriteSubTree);
        RecurseCopyKey(sourceSubKey, destSubKey);
      }
    }
  }
}
