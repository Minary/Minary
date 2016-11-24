namespace Minary.Template.Infrastructure
{
  using Minary.Template.DataTypes.Template;
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
    public void SaveTemplate(RecordMinaryTemplate serializableObject, string outputFile)
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
        RecordMinaryTemplate deserializedObjectData = this.LoadAttackTemplate(filePath);
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
    public RecordMinaryTemplate LoadAttackTemplate(string templateFile)
    {
      RecordMinaryTemplate deserializedObject;
      BinaryFormatter myBinaryFormat = new BinaryFormatter();

      Stream myStream = File.OpenRead(templateFile);
      deserializedObject = (RecordMinaryTemplate)myBinaryFormat.Deserialize(myStream);
      myStream.Close();

      return deserializedObject;
    }

    #endregion
 
  }
}
