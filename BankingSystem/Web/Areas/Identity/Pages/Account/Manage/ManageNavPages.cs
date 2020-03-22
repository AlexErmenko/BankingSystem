using System;
using System.IO;

using Microsoft.AspNetCore.Mvc.Rendering;

namespace Web.Areas.Identity.Pages.Account.Manage
{
  public static class ManageNavPages
  {
    public static string Index => "Index";

    public static string Email => "Email";

    public static string ChangePassword => "ChangePassword";

    public static string ExternalLogins => "ExternalLogins";

    public static string PersonalData => "PersonalData";

    public static string TwoFactorAuthentication => "TwoFactorAuthentication";

    public static string IndexNavClass(ViewContext viewContext) => PageNavClass(viewContext: viewContext, page: Index);

    public static string EmailNavClass(ViewContext viewContext) => PageNavClass(viewContext: viewContext, page: Email);

    public static string ChangePasswordNavClass(ViewContext viewContext) => PageNavClass(viewContext: viewContext, page: ChangePassword);

    public static string ExternalLoginsNavClass(ViewContext viewContext) => PageNavClass(viewContext: viewContext, page: ExternalLogins);

    public static string PersonalDataNavClass(ViewContext viewContext) => PageNavClass(viewContext: viewContext, page: PersonalData);

    public static string TwoFactorAuthenticationNavClass(ViewContext viewContext) => PageNavClass(viewContext: viewContext, page: TwoFactorAuthentication);

    private static string PageNavClass(ViewContext viewContext, string page)
    {
      string activePage = viewContext.ViewData[index: "ActivePage"] as string ?? Path.GetFileNameWithoutExtension(path: viewContext.ActionDescriptor.DisplayName);
      return string.Equals(a: activePage, b: page, comparisonType: StringComparison.OrdinalIgnoreCase) ? "active" : null;
    }
  }
}
