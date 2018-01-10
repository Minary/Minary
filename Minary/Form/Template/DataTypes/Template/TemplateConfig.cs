namespace Minary.Form.Template.DataTypes.Template
{
  using System;


  [Serializable]
  public class TemplateConfig
  {

    #region PROPERTIES

    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public string Timestamp { get; set; } = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

    public string Version { get; set; } = string.Empty;

    public string Author { get; set; } = string.Empty;

    public string Reference { get; set; } = string.Empty;

    #endregion PROPERTIES


    #region PUBLIC

    /// <summary>
    /// Initializes a new instance of the <see cref="TemplateConfig"/> class.
    ///
    /// </summary>
    public TemplateConfig()
    {
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="name"></param>
    /// <param name="description"></param>
    /// <param name="author"></param>
    /// <param name="reference"></param>
    /// <param name="version"></param>
    public TemplateConfig(string name, string description, string author, string reference, string version)
    {
      this.Name = name;
      this.Description = description;
      this.Author = author;
      this.Reference = reference;
      this.Timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
      this.Version = version;
    }

    #endregion PUBLIC

  }
}
