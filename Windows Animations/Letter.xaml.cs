using Gma.System.MouseKeyHook;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using Point = System.Windows.Point;

namespace Windows_Animations
{
    /// <summary>
    /// Letter.xaml 的交互逻辑
    /// </summary>
    public partial class Letter : Window
    {
        private static IKeyboardMouseEvents m_GlobalHook;
        public Letter()
        {
            InitializeComponent();

            Sub();
        }
        public void Sub()
        {
            // 初始化全局鼠标和键盘事件钩子
            m_GlobalHook = Hook.GlobalEvents();

            m_GlobalHook.KeyDown += M_GlobalHook_KeyDown;
        }

        private void M_GlobalHook_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyCode.ToString().Length == 1)
            {

                Letters.Text = e.KeyCode.ToString();
                Visibility = Visibility.Visible;
                Point point = new Point();
                point.X = System.Windows.Forms.Control.MousePosition.X + 1;
                point.Y = System.Windows.Forms.Control.MousePosition.Y - Height - 1;
                Left = point.X;
                Top = point.Y;

                Dispatcher.Invoke(() =>
                {
                    Visibility = Visibility.Hidden;
                }, DispatcherPriority.Normal);
                Thread.Sleep(3000);
            }
            else
            {
                Visibility = Visibility.Hidden;
            }
            e.Handled = true;
        }

        private void Window_MouseEnter(object sender, MouseEventArgs e)
        {
            Point point = new Point();
            point.X = System.Windows.Forms.Control.MousePosition.X;
            point.Y = System.Windows.Forms.Control.MousePosition.Y - Height - 1;
            Left = point.X;
            Top = point.Y;
        }
    }
}
