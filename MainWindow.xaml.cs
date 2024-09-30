using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace StickyNoteApp
{
    public partial class MainWindow : Window
    {
        private string _lastClipboardText = ""; // 追踪上一次的剪贴板内容
        private bool _isFirstCheck = true; // 标志位，用于判断是否是程序启动后的第一次检查

        public MainWindow()
        {
            InitializeComponent();
            SetWindowPositionToTopRight(); // 启动时将窗口设置到右上角
            StartClipboardWatcher(); // 启动剪贴板监控
        }

        // 将窗口设置到屏幕的右上角
        private void SetWindowPositionToTopRight()
        {
            var screenWidth = SystemParameters.PrimaryScreenWidth; // 获取屏幕宽度
            var windowWidth = this.Width; // 获取窗口宽度

            this.Left = screenWidth - windowWidth - 100; // 设置窗口的左侧边缘为屏幕宽度减去窗口宽度
            this.Top = 100; // 设置窗口的顶部边缘为0，即顶部
        }

        // 启动剪贴板监控
        private void StartClipboardWatcher()
        {
            DispatcherTimer timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1) // 每秒检查一次剪贴板内容
            };
            timer.Tick += (sender, args) =>
            {
                if (Clipboard.ContainsText())
                {
                    string clipboardText = Clipboard.GetText();
                    
                    // 如果是第一次检查，则跳过不填充，直接设置 _lastClipboardText
                    if (_isFirstCheck)
                    {
                        _lastClipboardText = clipboardText; // 记录第一次检查时的剪贴板内容
                        _isFirstCheck = false; // 设置为 false，之后将填充文本框
                    }
                    else if (clipboardText != _lastClipboardText)
                    {
                        _lastClipboardText = clipboardText; // 更新上一次的剪贴板内容
                        NoteTextBox.Text = clipboardText;  // 填充文本框
                    }
                }
            };
            timer.Start();
        }

        // 捕捉鼠标点击顶部区域事件并拖动窗口
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }
    }
}
