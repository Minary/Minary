namespace Minary.Form.SimpleGUI
{
  using System;
  using System.Collections.Generic;
  using System.ComponentModel;
  using System.Drawing;
  using System.Data;
  using System.Linq;
  using System.Text;
  using System.Threading.Tasks;
  using System.Windows.Forms;


  public partial class SimpleGuiUserControl : UserControl
  {
    public SimpleGuiUserControl()
    {
      InitializeComponent();


      DataGridViewTextBoxColumn columnType = new DataGridViewTextBoxColumn();
      columnType.DataPropertyName = "PluginType";
      columnType.Name = "PluginType";
      columnType.HeaderText = "Type";
      columnType.Visible = true;
      this.dgv_SimpleGui.Columns.Add(columnType);
    }
  }
}
