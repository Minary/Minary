namespace Minary.Form
{
  using System.Windows.Forms;


  public class MessageDialog
  {

    #region MEMBERS

    private static MessageDialog instance;

    #endregion


    #region PROPERTIES

    public static MessageDialog Inst { get { return instance ?? (instance = new MessageDialog());  } set { } }

    #endregion


    #region PUBLIC
    
    private MessageDialog()
    {
    }

    public void ShowError(string title, string message, Form parentForm)
    {
      this.Show(string.Format($"Error: {title}"), message, MessageBoxButtons.OK, MessageBoxIcon.Error, parentForm);
    }


    public void ShowWarning(string title, string message, Form parentForm)
    {
      this.Show(string.Format($"Warning: {title}"), message, MessageBoxButtons.OK, MessageBoxIcon.Warning, parentForm);
    }


    public void ShowInformation(string title, string message, Form parentForm)
    {
      this.Show(string.Format($"Info: {title}"), message, MessageBoxButtons.OK, MessageBoxIcon.Information, parentForm);
    }

    #endregion


    #region PRIVATE

    private delegate void ShowDelegate(string title, string message, MessageBoxButtons buttons, MessageBoxIcon icon, Form parentForm = null);
    private void Show(string title, string message, MessageBoxButtons buttons, MessageBoxIcon icon, Form parentForm = null)
    {
      if (parentForm != null)
      {
        if (parentForm.InvokeRequired)
        {
          parentForm.BeginInvoke(new ShowDelegate(this.Show), new object[] { title, message, buttons, icon, parentForm });
          return;
        }

        MessageBox.Show(parentForm, message, title, buttons, icon);
      }
      else
      {
        MessageBox.Show(message, title, buttons, icon);
      }
    }

    #endregion

  }
}
