using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using Gma.System.MouseKeyHook;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Forms;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace Windows_Animations
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private static IKeyboardMouseEvents m_GlobalHook;
        [System.Runtime.InteropServices.DllImportAttribute("user32.dll", EntryPoint = "MoveWindow")]
        public static extern bool MoveWindow(System.IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);
        [DllImport("User32.dll", EntryPoint = "FindWindow")]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool GetWindowRect(IntPtr hWnd, ref RECT lpRect);

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left;                             //最左坐标
            public int Top;                             //最上坐标
            public int Right;                           //最右坐标
            public int Bottom;                        //最下坐标
        }
        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern IntPtr GetForegroundWindow();

        public MainWindow()
        {
            InitializeComponent();

            Sub();
        }
        List<string> list = new List<string>();
        public void Sub()
        {
            // 初始化全局鼠标和键盘事件钩子
            m_GlobalHook = Hook.GlobalEvents();

            // 订阅鼠标事件
            m_GlobalHook.MouseDownExt += M_GlobalHook_MouseDownExt;
            m_GlobalHook.MouseUpExt += M_GlobalHook_MouseUpExt;

            m_GlobalHook.KeyDown += M_GlobalHook_KeyDown;
        }

        private void M_GlobalHook_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if(e.KeyCode == Keys.S && e.Control && e.Alt)
            {
                Process.GetCurrentProcess().Kill();
            }
        }

        private bool IsDraging = false;

        private void M_GlobalHook_MouseUpExt(object sender, MouseEventExtArgs e)
        {
            IsDraging = false;
            
            
        }

        private void M_GlobalHook_MouseDownExt(object sender, MouseEventExtArgs e)
        {
            IsDraging = true;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Thread thread = new Thread(() =>
            {
                bool Isright = true;
                double speed = 0.15;
                while (true)
                {
                    IntPtr hWnd = GetForegroundWindow();
                    RECT rc = new RECT();
                    GetWindowRect(hWnd, ref rc);
                    int Y = rc.Top; //Y
                    int width = rc.Right - rc.Left;                        //宽
                    int height = rc.Bottom - rc.Top;                   //高
                    int X = rc.Left; //X
                    var w = SystemParameters.WorkArea.Height - height; //工作区域宽
                    bool IsFullScreenApp = false;
                    bool IsTaskbar = false;
                    if (width == SystemParameters.PrimaryScreenWidth && height == SystemParameters.PrimaryScreenHeight)
                        IsFullScreenApp = true;
                    if (rc.Bottom == SystemParameters.PrimaryScreenHeight && rc.Left == 0 && rc.Right == SystemParameters.PrimaryScreenWidth)
                        IsTaskbar = true;
                    if (!IsFullScreenApp && !IsTaskbar)
                    {
                        if (Y < w && !IsDraging)
                        {
                            for (double i = Y; i < w; i += speed)
                            {
                                if (speed < 50)
                                    speed += new Random().NextDouble() * 2;
                                if (Isright && rc.Right < SystemParameters.WorkArea.Width)
                                    X += new Random().Next(0, 15);
                                else if (X > 0)
                                    X -= new Random().Next(0, 15);
                                MoveWindow(hWnd, X, (int)i + 2, width, height, false);
                                Thread.Sleep(2);
                            }
                        }
                        else if (Y > w && !IsDraging)
                        {
                            MoveWindow(hWnd, X, (int)w, width, height, true);
                        }
                        else
                        {
                            Isright = new Random().Next(0, 2) == 1;
                            speed = 0.15;
                        }
                    }
                    else
                    {
                        Isright = new Random().Next(0, 2) == 1;
                        speed = 0.15;
                    }
                    Thread.Sleep(10);
                }
            });
            thread.Start();
        }
        
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Process.GetCurrentProcess().Kill();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private void Bar_Click(object sender, RoutedEventArgs e)
        {
            new Letter().Show();
        }
    }
}
