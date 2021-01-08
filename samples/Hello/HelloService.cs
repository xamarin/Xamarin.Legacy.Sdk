using Android.App;

namespace Hello
{
    public class HelloService
    {
        public string Hello() =>
#if NET6_0
            Application.Context.Resources.GetString (Resource.String.net6);
#else
            Application.Context.Resources.GetString (Resource.String.legacy);
#endif
    }
}