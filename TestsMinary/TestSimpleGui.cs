using Microsoft.VisualStudio.TestTools.UnitTesting;
using Minary.DataTypes.ArpScan;
using System;
using TestsMinary.DataTypes;


namespace TestsMinary
{
  [TestClass]
  public class TestSimpleGui
  {

    [TestMethod]
    public void AddNewRecord_Success()
    {
      var simpleGui = this.GenerateSimpleGui();
      var newRecord = new SystemFound("00-11-22-33-44-55", "192.168.1.2");
      simpleGui.UpdateNewRecord(newRecord);
      simpleGui.RemoveOutdatedRecords();

      Assert.IsTrue(simpleGui.TargetStringList.Count == 1);
    }


    [TestMethod]
    public void RemoveOutdatedRecord1_Success()
    {
      var simpleGui = this.GenerateSimpleGui();
      var newRecord = new SystemFound("00-11-22-33-44-55", "192.168.1.2");
      
      simpleGui.UpdateNewRecord(newRecord);
      Assert.IsTrue(simpleGui.TargetStringList.Count == 1);
      simpleGui.TargetStringList[0].LastSeen = DateTime.Now.AddSeconds(-182);
      simpleGui.RemoveOutdatedRecords();
      Assert.IsTrue(simpleGui.TargetStringList.Count == 0);
    }


    [TestMethod]
    public void RemoveOutdatedRecord2_Success()
    {
      var simpleGui = this.GenerateSimpleGui();
      var newRecord = new SystemFound("00-11-22-33-44-55", "192.168.1.2");

      simpleGui.UpdateNewRecord(newRecord);
      Assert.IsTrue(simpleGui.TargetStringList.Count == 1);
      simpleGui.TargetStringList[0].LastSeen = DateTime.Now.AddSeconds(-178);
      simpleGui.RemoveOutdatedRecords();
      Assert.IsTrue(simpleGui.TargetStringList.Count == 1);
    }


    #region PRIVATE

    private MockSimpleGui GenerateSimpleGui()
    {
      string[] args = new string[0];
      MockMinaryMain minaryMain = new MockMinaryMain(args);
      MockSimpleGui simpleGui = new MockSimpleGui(minaryMain);

      return simpleGui;
    }

    #endregion

  }
}
