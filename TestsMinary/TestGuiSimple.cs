using Microsoft.VisualStudio.TestTools.UnitTesting;
using Minary.DataTypes.ArpScan;
using System;
using TestsMinary.DataTypes;


namespace TestsMinary
{
  [TestClass]
  public class TestGuiSimple
  {

    [TestMethod]
    public void AddNewRecord_Success()
    {
      var guiSimple = this.GenerateGuiSimple();
      var newRecord = new SystemFound("00-11-22-33-44-55", "192.168.1.2");
      guiSimple.UpdateNewRecord(newRecord);
      guiSimple.RemoveOutdatedRecords();

      Assert.IsTrue(guiSimple.TargetStringList.Count == 1);
    }


    [TestMethod]
    public void RemoveOutdatedRecord1_Success()
    {
      var guiSimple = this.GenerateGuiSimple();
      var newRecord = new SystemFound("00-11-22-33-44-55", "192.168.1.2");
      
      guiSimple.UpdateNewRecord(newRecord);
      Assert.IsTrue(guiSimple.TargetStringList.Count == 1);
      guiSimple.TargetStringList[0].LastSeen = DateTime.Now.AddSeconds(-182);
      guiSimple.RemoveOutdatedRecords();
      Assert.IsTrue(guiSimple.TargetStringList.Count == 0);
    }


    [TestMethod]
    public void RemoveOutdatedRecord2_Success()
    {
      var guiSimple = this.GenerateGuiSimple();
      var newRecord = new SystemFound("00-11-22-33-44-55", "192.168.1.2");

      guiSimple.UpdateNewRecord(newRecord);
      Assert.IsTrue(guiSimple.TargetStringList.Count == 1);
      guiSimple.TargetStringList[0].LastSeen = DateTime.Now.AddSeconds(-178);
      guiSimple.RemoveOutdatedRecords();
      Assert.IsTrue(guiSimple.TargetStringList.Count == 1);
    }


    #region PRIVATE

    private MockGuiSimple GenerateGuiSimple()
    {
      string[] args = new string[0];
      MockMinaryMain minaryMain = new MockMinaryMain(args);
      MockGuiSimple guiSimple = new MockGuiSimple(minaryMain);

      return guiSimple;
    }

    #endregion

  }
}
