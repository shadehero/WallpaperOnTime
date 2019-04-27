using System;
using System.Threading;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Drawing;
using System.Diagnostics;
using System.Threading;

namespace WallpaperOnTime
{
	class MainClass
	{
		[DllImport("user32.dll", SetLastError = true)]
			static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

		[DllImport("user32.dll")]
			static extern IntPtr GetShellWindow();

		[DllImport("user32.dll")]
			static extern IntPtr GetDesktopWindow();

		[DllImport("kernel32.dll")]
		static extern IntPtr GetConsoleWindow();

		[DllImport("user32.dll")]
		static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

		const int SW_HIDE = 0;
		const int SW_SHOW = 5;

		static NotifyIcon notifyIcon;
		static IntPtr processHandle;
		static IntPtr WinShell;
		static IntPtr WinDesktop;
		static MenuItem HideMenu;
		static MenuItem RestoreMenu;

		public static void Main(string[] args)
		{
			Thread _thread = new Thread(HandleThreadStart);
			_thread.IsBackground = true;
			_thread.Start();

			var handle = GetConsoleWindow();
			ShowWindow(handle, SW_HIDE);

			notifyIcon = new NotifyIcon();
			notifyIcon.Icon = new Icon("icon.ico");
			notifyIcon.Text = "Monitor";
			notifyIcon.Visible = true;

			ContextMenu menu = new ContextMenu();
			HideMenu = new MenuItem("Hide", new EventHandler(Minimize_Click));
			RestoreMenu = new MenuItem("Show", new EventHandler(Maximize_Click));

			menu.MenuItems.Add(RestoreMenu);
			menu.MenuItems.Add(HideMenu);
			menu.MenuItems.Add(new MenuItem("Exit", new EventHandler(CleanExit)));

			notifyIcon.ContextMenu = menu;

			processHandle = Process.GetCurrentProcess().MainWindowHandle;
			WinShell = GetShellWindow();
			WinDesktop = GetDesktopWindow();

			ResizeWindow(false);
			Application.Run();

			Console.CursorVisible = false;
			Console.Title = "Wallpaper on Time by Doctor Bogdan";


		}

		static void HandleThreadStart()
		{
				while (true)
			{
				//int h = DateTime.Now.Hour;
				int h = 6;
				Console.Clear();

				// Set Wallpaper
				if (h >= 0 && h < 3) { Console.WriteLine("Time 0-3 : Late Night"); Wallpaper.Set("Wallpapers/Late Night.png", Wallpaper.Style.Tiled); }
				if (h >= 3 && h < 6) { Console.WriteLine("Time 3-6 : Morning"); Wallpaper.Set("Wallpapers/Morning.png", Wallpaper.Style.Tiled); }
				if (h >= 6 && h < 9) { Console.WriteLine("Time 6-9 : Late Morning"); Wallpaper.Set("Wallpapers/Late Morning.png", Wallpaper.Style.Tiled); }
				if (h >= 9 && h < 12) { Console.WriteLine("Time 9-12 : Afternoon"); Wallpaper.Set("Wallpapers/Afternoon.png", Wallpaper.Style.Tiled); }
				if (h >= 12 && h < 15) { Console.WriteLine("Time 12-15 : Afternoont"); Wallpaper.Set("Wallpapers/Afternoon.png", Wallpaper.Style.Tiled); }
				if (h >= 15 && h < 19) { Console.WriteLine("Time 15-19 : Late Afternoon"); Wallpaper.Set("Wallpapers/Late Afternoon.png", Wallpaper.Style.Tiled); }
				if (h >= 19 && h < 22) { Console.WriteLine("Time 19-22 : Evening"); Wallpaper.Set("Wallpapers/Evening.png", Wallpaper.Style.Tiled); }
				if (h >= 22 && h < 24) { Console.WriteLine("Time 22-24 : Night"); Wallpaper.Set("Wallpapers/Night.png", Wallpaper.Style.Tiled); }

				Thread.Sleep(1000);
			}
		}

		static void Run()
		{
			Console.WriteLine("Listening to messages");

			while (true)
			{
				System.Threading.Thread.Sleep(1000);
			}
		}

		private static void CleanExit(object sender, EventArgs e)
		{
			notifyIcon.Visible = false;
			Application.Exit();
			Environment.Exit(1);
		}


		static void Minimize_Click(object sender, EventArgs e)
		{
			var handle = GetConsoleWindow();
			ShowWindow(handle, SW_HIDE);
			ResizeWindow(false);
		}


		static void Maximize_Click(object sender, EventArgs e)
		{
			var handle = GetConsoleWindow();
			ShowWindow(handle, SW_SHOW);
			ResizeWindow();
		}

		static void ResizeWindow(bool Restore = true)
		{
			if (Restore)
			{
				RestoreMenu.Enabled = false;
				HideMenu.Enabled = true;
				SetParent(processHandle, WinDesktop);
			}
			else
			{
				RestoreMenu.Enabled = true;
				HideMenu.Enabled = false;
				SetParent(processHandle, WinShell);
			}
		}
	}
}
