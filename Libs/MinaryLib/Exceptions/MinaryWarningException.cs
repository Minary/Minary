using System;

namespace MinaryLib.Exceptions
{
  public class MinaryWarningException : Exception
  {
    public MinaryWarningException(string message)
      : base(message)
    {
    }
  }
}
