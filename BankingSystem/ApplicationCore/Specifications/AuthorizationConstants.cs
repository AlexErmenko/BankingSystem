using System;

namespace ApplicationCore.Specifications
{
  public static class AuthorizationConstants
  {
    public const string DEFAULT_PASSWORD = "Pass@word1";

    public static class Roles
    {
      public const string ADMINISTRATORS = "Administrators";
      public const string MANAGER = "Manager";
      public const string CLIENT = "Client";
    }
  }
}
