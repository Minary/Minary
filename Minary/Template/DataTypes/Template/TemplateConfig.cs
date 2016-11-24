namespace Minary.Template.DataTypes.Template
{
  using System;


  [Serializable]
  public class TemplateConfig
  {

    #region MEMBERS

    private string name;
    private string description;
    private string timestamp;
    private string version;
    private string author;
    private string reference;

    #endregion MEMBERS


    #region PROPERTIES

    public string Name { get { return this.name; } set { this.name = value; } }

    public string Description { get { return this.description; } set { this.description = value; } }

    public string Timestamp { get { return this.timestamp; } set { this.timestamp = value; } }

    public string Version { get { return this.version; } set { this.version = value; } }

    public string Author { get { return this.author; } set { this.author = value; } }

    public string Reference { get { return this.reference; } set { this.reference = value; } }

    #endregion PROPERTIES


    #region PUBLIC

    /// <summary>
    /// Initializes a new instance of the <see cref="TemplateConfig"/> class.
    ///
    /// </summary>
    public TemplateConfig()
    {
      this.name = string.Empty;
      this.description = string.Empty;
      this.author = string.Empty;
      this.reference = string.Empty;
      this.timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
      this.version = string.Empty;
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
      this.name = name;
      this.description = description;
      this.author = author;
      this.reference = reference;
      this.timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
      this.version = version;
    }

    #endregion PUBLIC

  }
}
