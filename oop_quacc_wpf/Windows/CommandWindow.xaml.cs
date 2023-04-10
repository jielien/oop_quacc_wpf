using oop_quacc_wpf.CommandsSystem;
using oop_quacc_wpf.CommandsSystem.CommandsExecuters;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace oop_quacc_wpf
{
    /// <summary>
    /// Interaction logic for CommandWindow.xaml
    /// </summary>
    public partial class CommandWindow : Window
    {
        #region Hotkeys
        [DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);
        [DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        private const int HK_ID_SHOW_WINDOW = 9000;

        private const uint MOD_CTRL = 0x02;
        private const uint MOD_SHIFT = 0x04;

        private const uint VK_P = 0x50;

        IntPtr WindowHandle { get; set; }
        HwndSource Source { get; set; }
        #endregion

        /// <summary>
        /// QUACC app default executers.
        /// </summary>
        private CommandsExecuter[] DefaultExecuters =
        {
            new QUACCComandsExecuter(),
            new QUACCMathCommandsExecuter()
        };

        public CommandsSystemManager CommandsSystemManager { get; private set; }

        public CommandWindow()
        {
            // Init CSM with default executers
            CommandsSystemManager = new CommandsSystemManager(new List<CommandsExecuter>(DefaultExecuters));

            InitializeComponent();

            // add options to ExecuterComboBox
            foreach (var e in CommandsSystemManager.Executers)
                ExecuterComboBox.Items.Add(e.Name);
            ExecuterComboBox.SelectedIndex = 0;
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);

            InitializeHotkeys();
            CommandTextBox.Focus();
        }

        /// <summary>
        /// Create bindings to hotkeys and overall keyboard input
        /// </summary>
        private void InitializeHotkeys()
        {
            // Show command window hotkey
            WindowHandle = new WindowInteropHelper(this).Handle;
            Source = HwndSource.FromHwnd(WindowHandle);
            Source.AddHook(On_WindowShow);
            RegisterHotKey(WindowHandle, HK_ID_SHOW_WINDOW, MOD_CTRL | MOD_SHIFT, VK_P);

            // Hide command window hotkey
            RoutedCommand hideCmdWindow = new RoutedCommand();
            hideCmdWindow.InputGestures.Add(new KeyGesture(Key.Escape, ModifierKeys.None));
            CommandBindings.Add(new CommandBinding(hideCmdWindow, On_WindowHide));
        }

        /// <summary>
        /// Is triggered when KeyDown event fires on <see cref="CommandTextBox"/>.
        /// </summary>
        private void CommandTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            // on command accept (there is a command in TextBox and Enter was pressed)
            if (e.Key == Key.Enter && sender is TextBox textBox)
                if (textBox.Text != String.Empty)
                {
                    var respond = CommandsSystemManager.ExecuteCommand(textBox.Text.Trim(), ExecuterComboBox.SelectedItem as string);

                    if(respond == CommandExecutionRespond.Exit) Application.Current.Shutdown();
                    if (respond == CommandExecutionRespond.Executed)
                        textBox.Clear();
                    else
                        ShowErrorMessageBox(respond.ToString());
                }
        }

        /// <summary>
        /// Shows command window and sets focus to it.
        /// </summary>
        public void ShowWindow()
        {
            Visibility = Visibility.Visible;
            CommandTextBox.Focus();
        }

        /// <summary>
        /// Hides command window.
        /// </summary>
        public void HideWindow()
        {
            Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// Unregisters hotkeys and closes the app.
        /// </summary>
        protected override void OnClosed(EventArgs e)
        {
            Source.RemoveHook(On_WindowShow);
            UnregisterHotKey(WindowHandle, HK_ID_SHOW_WINDOW);
            base.OnClosed(e);
        }

        #region Utilities
        /// <summary>
        /// Shows a <see cref="MessageBox"/> with Error styling.
        /// </summary>
        private void ShowErrorMessageBox(string content)
        {
            MessageBox.Show(content, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        #endregion

        #region Events
        // Recieves event from win os
        private IntPtr On_WindowShow(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            const int WM_HOTKEY = 0x0312;
            switch (msg)
            {
                // Is hotkey
                case WM_HOTKEY:
                    switch (wParam.ToInt32())
                    {
                        case HK_ID_SHOW_WINDOW:
                            ShowWindow();
                            handled = true;
                            break;
                    }
                    break;
            }
            return IntPtr.Zero;
        }
        private void On_WindowHide(object sender, RoutedEventArgs e)
        {
            HideWindow();
        }
        #endregion
    }
}
