namespace Minary.Form
{
  using System.Windows.Forms;


  public static class MessageDialog
  {

    #region PUBLIC

    public static void ShowError(string title, string message, IWin32Window owner = null)
    {
      Show(string.Format($"Error: {title}"), message, MessageBoxButtons.OK, MessageBoxIcon.Error, owner);
    }


    public static void ShowWarning(string title, string message, IWin32Window owner = null)
    {
      Show(string.Format($"Warning: {title}"), message, MessageBoxButtons.OK, MessageBoxIcon.Warning, owner);
    }


    public static void ShowInformation(string title, string message, IWin32Window owner = null)
    {
      Show(string.Format($"Info: {title}"), message, MessageBoxButtons.OK, MessageBoxIcon.Information, owner);
    }

    #endregion


    #region PRIVATE

    private static void Show(string title, string message, MessageBoxButtons buttons, MessageBoxIcon icon, IWin32Window owner = null)
    {
      if (owner != null)
      {
        MessageBox.Show(owner, message, title, buttons, icon);
      }
      else
      {
        MessageBox.Show(message, title, buttons, icon);
      }
    }

    #endregion

  }
}
