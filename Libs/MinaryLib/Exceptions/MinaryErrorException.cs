namespace MinaryLib.Exceptions
{
  using System;


  public class MinaryErrorException : Exception
  {
    public MinaryErrorException(string message)
      : base(message)
    {
    }
  }
}
