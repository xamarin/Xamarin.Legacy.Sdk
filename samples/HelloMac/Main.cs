using AppKit;

namespace HelloMac {
	static class Application {
		static void Main (string [] args)
		{
			NSApplication.Init ();
			NSApplication.Main (args);
		}
	}
}
