namespace Minary.Form
{
  using System;
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

    public DialogResult ShowError(string title, string message, Form parentForm, MessageBoxButtons button = MessageBoxButtons.OK)
    {
      return this.Show($"Error: {title}", message, button, MessageBoxIcon.Error, parentForm);
    }


    public DialogResult ShowWarning(string title, string message, Form parentForm, MessageBoxButtons button = MessageBoxButtons.OK)
    {
      return this.Show($"Warning: {title}", message, button, MessageBoxIcon.Warning, parentForm);
    }


    public DialogResult ShowInformation(string title, string message, Form parentForm, MessageBoxButtons button = MessageBoxButtons.OK)
    {
      return this.Show($"Info: {title}", message, button, MessageBoxIcon.Information, parentForm);
    }

    #endregion


    #region PRIVATE

    private delegate DialogResult ShowDelegate(string title, string message, MessageBoxButtons buttons, MessageBoxIcon icon, Form parentForm = null);
    private DialogResult Show(string title, string message, MessageBoxButtons buttons, MessageBoxIcon icon, Form parentForm = null)
    {
      if (parentForm != null)
      {
        if (parentForm.InvokeRequired)
        {
          return (DialogResult)parentForm.Invoke(new Func<DialogResult>(() => MessageBox.Show(parentForm, message, title, buttons, icon)));
        }
        else
        {
          return MessageBox.Show(parentForm, message, title, buttons, icon);
        }
      }
      else
      {
        return MessageBox.Show(message, title, buttons, icon);
      }
    }

    #endregion

  }
}
