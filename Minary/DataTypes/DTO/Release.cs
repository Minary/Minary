namespace Minary.DataTypes.DTO
{
  using RestSharp.Deserializers;
  using System.Collections.Generic;


  public class Release
  {
    public class Asset
    {
      [DeserializeAs(Name = "state")]
      public string State { get; set; }

      [DeserializeAs(Name = "browser_download_url")]
      public string BrowserDownloadUrl { get; set; }
    }


    [DeserializeAs(Name = "name")]
    public string Name { get; set; }

    [DeserializeAs(Name = "tag_name")]
    public string TagName { get; set; }

    [DeserializeAs(Name = "published_at")]
    public string PublishedAt { get; set; }

    [DeserializeAs(Name = "tarball_url")]
    public string SourceZipBallUrl { get; set; }

    [DeserializeAs(Name = "body")]
    public string Body { get; set; }

    [DeserializeAs(Name = "assets")]
    public List<Asset> Assets { get; set; }
  }
}
