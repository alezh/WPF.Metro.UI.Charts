using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using System.IO.Ports;
using System.Threading;
using System.Windows;

namespace PowerManage.SerialPort
{
    class InitSerialPort
    {

        public InitSerialPort()
        {
            serialPort = new System.IO.Ports.SerialPort();
            clockTimer = new DispatcherTimer();
            FindPorts();
            InitClockTimer();
            InitSerialPorts();
        }
        /// <summary>
        /// SerialPort对象
        /// </summary>
        private System.IO.Ports.SerialPort serialPort;
        private DispatcherTimer clockTimer;
        /// <summary>
        /// 定时器初始化
        /// </summary>
        private void InitClockTimer()
        {
            clockTimer.Interval = new TimeSpan(0, 0, 1);
            clockTimer.IsEnabled = true;
            clockTimer.Tick += ClockTimer_Tick;
            clockTimer.Start();
        }
        private void ClockTimer_Tick(object sender, EventArgs e)
        {
            UpdateTimeDate();
        }
        /// <summary>
        /// 更新时间信息
        /// </summary>
        private void UpdateTimeDate()
        {
            string timeDateString = "";
            DateTime now = DateTime.Now;
            timeDateString = string.Format("{0}年{1}月{2}日 {3}:{4}:{5}",
                now.Year,
                now.Month.ToString("00"),
                now.Day.ToString("00"),
                now.Hour.ToString("00"),
                now.Minute.ToString("00"),
                now.Second.ToString("00"));
            //timeDateTextBlock.Text = timeDateString;
        }


        #region 定时器
        /// <summary>
        /// 超时时间为50ms
        /// </summary>
        private const int TIMEOUT = 50;
        private void InitCheckTimer()
        {
            // 如果缓冲区中有数据，并且定时时间达到前依然没有得到处理，将会自动触发处理函数。
            checkTimer.Interval = new TimeSpan(0, 0, 0, 0, TIMEOUT);
            checkTimer.IsEnabled = false;
            checkTimer.Tick += CheckTimer_Tick;
        }

        private void StartCheckTimer()
        {
            checkTimer.IsEnabled = true;
            checkTimer.Start();
        }

        private void StopCheckTimer()
        {
            checkTimer.IsEnabled = false;
            checkTimer.Stop();
        }
        #endregion
        // 需要一个定时器用来，用来保证即使缓冲区没满也能够及时将数据处理掉，防止一直没有到达
        // 阈值，而导致数据在缓冲区中一直得不到合适的处理。
        private DispatcherTimer checkTimer = new DispatcherTimer();

        private void InitSerialPorts()
        {
            serialPort.DataReceived += SerialPort_DataReceived;
            InitCheckTimer();
        }
        #region EventHandler for serialPort

        // 数据接收缓冲区
        private List<byte> receiveBuffer = new List<byte>();

        // 一个阈值，当接收的字节数大于这么多字节数之后，就将当前的buffer内容交由数据处理的线程
        // 分析。这里存在一个问题，假如最后一次传输之后，缓冲区并没有达到阈值字节数，那么可能就
        // 没法启动数据处理的线程将最后一次传输的数据处理了。这里应当设定某种策略来保证数据能够
        // 在尽可能短的时间内得到处理。
        private const int THRESH_VALUE = 128;

        private bool shouldClear = true;

        /// <summary>
        /// 更新：采用一个缓冲区，当有数据到达时，把字节读取出来暂存到缓冲区中，缓冲区到达定值
        /// 时，在显示区显示数据即可。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SerialPort_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            System.IO.Ports.SerialPort sp = sender as System.IO.Ports.SerialPort;

            if (sp != null)
            {
                // 临时缓冲区将保存串口缓冲区的所有数据
                int bytesToRead = sp.BytesToRead;
                byte[] tempBuffer = new byte[bytesToRead];

                // 将缓冲区所有字节读取出来
                sp.Read(tempBuffer, 0, bytesToRead);

                // 检查是否需要清空全局缓冲区先
                if (shouldClear)
                {
                    receiveBuffer.Clear();
                    shouldClear = false;
                }
                string data = serialPort.Encoding.GetString(tempBuffer.ToArray<byte>());
                // 暂存缓冲区字节到全局缓冲区中等待处理
                receiveBuffer.AddRange(tempBuffer);

                //if (receiveBuffer.Count >= THRESH_VALUE)
                //{
                //    //Dispatcher.Invoke(new Action(() =>
                //    //{
                //    //    recvDataRichTextBox.AppendText("Process data.\n");
                //    //}));
                //    // 进行数据处理，采用新的线程进行处理。
                //    Thread dataHandler = new Thread(new ParameterizedThreadStart(ReceivedDataHandler));
                //    dataHandler.Start(receiveBuffer);
                //}

                // 启动定时器，防止因为一直没有到达缓冲区字节阈值，而导致接收到的数据一直留存在缓冲区中无法处理。
                //StartCheckTimer();

                //this.Dispatcher.Invoke(new Action(() =>
                //{
                //    // 根据显示模式显示接收到的字节.
                //    string[] date = data.Split(':');
                //    double vol;
                //    if (date.Count() > 1 && double.TryParse(date[1].Replace("|", ""), out vol) && date[1].Contains("|"))
                //    {
                //        MaxVolt.Content = String.Format("{0:N2}", vol) + " V";
                //        VoltBar.Value = vol * 100;
                //    }
                //}));
            }
        }

        #endregion

        #region 数据处理

        private void CheckTimer_Tick(object sender, EventArgs e)
        {
            // 触发了就把定时器关掉，防止重复触发。
            StopCheckTimer();

            // 只有没有到达阈值的情况下才会强制其启动新的线程处理缓冲区数据。
            if (receiveBuffer.Count < THRESH_VALUE)
            {
                //recvDataRichTextBox.AppendText("Timeout!\n");
                // 进行数据处理，采用新的线程进行处理。
                Thread dataHandler = new Thread(new ParameterizedThreadStart(ReceivedDataHandler));
                dataHandler.Start(receiveBuffer);
            }
        }


        private void ReceivedDataHandler(object obj)
        {
            List<byte> recvBuffer = new List<byte>();
            recvBuffer.AddRange((List<byte>)obj);

            if (recvBuffer.Count == 0)
            {
                return;
            }

            // 必须应当保证全局缓冲区的数据能够被完整地备份出来，这样才能进行进一步的处理。
            shouldClear = true;

            //this.Dispatcher.Invoke(new Action(() =>
            //{
            //    // 根据显示模式显示接收到的字节.
            //    string[] date = serialPort.Encoding.GetString(recvBuffer.ToArray<byte>()).Split(':');
            //    if (date[1] != "\r\n")
            //        MaxVolt.Content = date[1].Replace("\r\n", "") + "V";

            //}));

            // TO-DO：
            // 处理数据，比如解析指令等等
        }
        #endregion





        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (serialPort.IsOpen)
            {
                if (ClosePort())
                {
                    //openClosePortButton.Content = "打开";
                }
            }
            else
            {
                if (OpenPort())
                {
                    //openClosePortButton.Content = "关闭";
                }
            }
        }

        /// <summary>
        /// 查找端口
        /// </summary>
        private void FindPorts()
        {
            //portsComboBox.ItemsSource = System.IO.Ports.SerialPort.GetPortNames();
            //if (portsComboBox.Items.Count > 0)
            //{
            //    portsComboBox.SelectedIndex = 0;
            //    portsComboBox.IsEnabled = true;
            //}
            //else
            //{
            //    portsComboBox.IsEnabled = false;
            //}
        }

        private bool OpenPort()
        {
            bool flag = false;
            ConfigurePort();

            try
            {
                serialPort.Open();
                serialPort.DiscardInBuffer();
                serialPort.DiscardOutBuffer();
                //Information(string.Format("成功打开端口{0}, 波特率{1}。", serialPort.PortName, serialPort.BaudRate.ToString()));
                flag = true;
            }
            catch (Exception ex)
            {
                //Alert(ex.Message);
            }

            return flag;
        }
        private bool ClosePort()
        {
            bool flag = false;

            try
            {
                //StopAutoSendDataTimer();
                //progressBar.Visibility = Visibility.Collapsed;
                serialPort.Close();
                //Information(string.Format("成功关闭端口{0}。", serialPort.PortName));
                flag = true;
            }
            catch (Exception ex)
            {
                //Alert(ex.Message);
            }

            return flag;
        }

        private void ConfigurePort()
        {
            //serialPort.PortName = portsComboBox.Text;
            serialPort.BaudRate = 9600;
            serialPort.Parity = System.IO.Ports.Parity.None;
            serialPort.DataBits = 8;
            serialPort.StopBits = System.IO.Ports.StopBits.One;
            serialPort.Encoding = Encoding.Default;
        }

    }
}
