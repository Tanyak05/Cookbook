using System;
using Cookbook2;
using Xamarin.Forms;
using Cookbook2;
using Cookbook;

[assembly: Dependency(typeof(BaseUrl_Android))]
namespace Cookbook
{
    public class BaseUrl_Android : IBaseUrl
    {
        public string Get()
        {
            return "file:///android_asset/local.html";
        }
    }
}