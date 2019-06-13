namespace Minary.Form.Template.Infrastructure
{
  using Minary.Form.Template.DataTypes.Template;
  using System.IO;
  using System.Runtime.Serialization.Formatters.Binary;


  public class TemplateHandler
  {

    #region MEMBERS


    #endregion


    #region PUBLIC

    /// <summary>
    /// 
    /// </summary>
    public TemplateHandler()
    {
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="serializableObject"></param>
    /// <param name="outputFile"></param>
    public void SaveTemplate(MinaryTemplateData serializableObject, string outputFile)
    {
      BinaryFormatter myBinaryFormat = new BinaryFormatter();
      FileStream outputFileStream = new FileStream(outputFile, FileMode.Create);
      myBinaryFormat.Serialize(outputFileStream, serializableObject);
      outputFileStream.Close();
    }


    public bool IsFileATemplate(string filePath)
    {
      bool retVal = false;

      try
      {
        MinaryTemplateData deserializedObjectData = this.LoadAttackTemplate(filePath);
        if (deserializedObjectData != null)
        {
          retVal = true;
        }
      }
      catch
      {
      }

      return retVal;
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="templateFile"></param>
    /// <returns></returns>
    public MinaryTemplateData LoadAttackTemplate(string templateFile)
    {
      MinaryTemplateData deserializedObject;
      BinaryFormatter myBinaryFormat = new BinaryFormatter();

      using (Stream myStream = File.OpenRead(templateFile))
      {
        deserializedObject = (MinaryTemplateData)myBinaryFormat.Deserialize(myStream);
      }

      return deserializedObject;
    }

    #endregion
 
  }
}
