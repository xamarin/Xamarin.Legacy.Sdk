#if ANDROID
using Android.App;
#elif IOS
using Foundation;
#endif

namespace Hello
{
    public class HelloService
    {
        public string Hello() =>
#if NET6_0
    #if ANDROID
            Application.Context.Resources.GetString (Resource.String.net6);
    #elif IOS
            NSBundle.MainBundle.GetLocalizedString (key: "net6", value: "Hello, .NET 6!");
    #endif
#else
    #if ANDROID
            Application.Context.Resources.GetString (Resource.String.legacy);
    #elif IOS
            NSBundle.MainBundle.GetLocalizedString (key: "legacy", value: "Hello, Xamarin.iOS!");
    #endif
#endif
    }
}