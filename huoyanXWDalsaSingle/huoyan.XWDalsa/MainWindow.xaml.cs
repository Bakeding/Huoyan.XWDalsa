using DALSA.SaperaLT.SapClassBasic;
using Fluent;
using huoyan.XWDalsa.Configurature;
using huoyan.XWDalsa.Utils;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Windows.Forms;
using winForm = System.Windows.Forms;
using static DALSA.SaperaLT.SapClassBasic.SapBuffer;
using MessageBox = System.Windows.MessageBox;
using MahApps.Metro.IconPacks;
using System.IO;

namespace huoyan.XWDalsa
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : RibbonWindow
    {

        #region 配置文件变量参数
        private ObservableCollection<String> serverNameList = new ObservableCollection<String>();

        private static float lastFrameRate = 0.0f;
        string ImgSaveType = "bmp";
        SapBuffer.FileFormat ImgFileFormat = 0;
        #endregion

        #region 通道1设置
        private ObservableCollection<String> deviceNameList = new ObservableCollection<String>();
        HYAcquisitionParams hYAcquisitionParams = new HYAcquisitionParams();
        private SapAcquisition Acq = new SapAcquisition();
        private SapAcqDevice AcqDevice = new SapAcqDevice();
        private SapBuffer Buffers = new SapBuffer();
        private SapTransfer Xfer = new SapTransfer();
        private SapView View = new SapView();
        SapLocation loc = new SapLocation();
        PictureBox pbDisplay = null;
        ImageBox pbWinFormDisplay = null;
        private ObservableCollection<String> configFileList = new ObservableCollection<String>();

        CancellationTokenSource ctsLLC, ctsSaveVideo;
        Task taskLLC, taskSaveVideo;

        bool isVideo = false;
        bool isStart = false;
        bool isImg = false;

        string saveFilePath = "D:\\SaveDataDirectory\\";
        private string _currentConfigFilePath = Environment.GetEnvironmentVariable("SAPERADIR") + "\\CamFiles\\User\\";
        public string currentConfigFilePath
        {
            get { return _currentConfigFilePath; }
            set { _currentConfigFilePath = value; }
        }
        #endregion

        #region 界面参数
        private string _statusBarMsg;

        public string statusBarMsg
        {
            get { return _statusBarMsg; }
            set
            {
                _statusBarMsg = value;
                //tbStatusBar.Text = _statusBarMsg;
                //Console.WriteLine(_statusBarMsg);
            }
        }


        #endregion
        public MainWindow()
        {
            InitializeComponent();
            HYAcquisitionSettingUtils.GetServers(serverNameList, out _statusBarMsg);

            #region 通道1初始化参数
            HYAcquisitionSettingUtils.GetConfigFilesInDirectry(currentConfigFilePath, configFileList, out _statusBarMsg);
            cbServer.ItemsSource = serverNameList;
            cbDevice.ItemsSource = deviceNameList;
            cbConfigFile.ItemsSource = configFileList;
            //currentConfigFilePath = Environment.GetEnvironmentVariable("SAPERADIR") + "\\CamFiles\\User\\";
            //tbConfigFilePath.Text = currentConfigFilePath;
            btConfigFileBrowser.ToolTip = currentConfigFilePath;
            btnImage.IsEnabled = false;
            btnVideo.IsEnabled = false;
            //btnFreeze.IsEnabled = true;
            btnFreeze.Background = Brushes.Green;

            pbDisplay = new PictureBox();
            pbDisplay.SizeMode = PictureBoxSizeMode.StretchImage;
            pbDisplay.Size = new System.Drawing.Size(640, 640);
            pbWinFormHost.Child = pbDisplay;
            //pbWinFormDisplay = new ImageBox();
            //pbWinFormHost.Child = pbWinFormDisplay;
            #endregion

        }


        private void snapAndSaveBufData(SapTransfer xfer, SapBuffer Buffer, CancellationTokenSource cts)
        {
            //System.Windows.Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() =>
            {
                // Acquire all frames
                //if ((bool)checkIsSave.IsChecked)
                {
                    //xfer.Freeze();
                    xfer.Init(true);
                    // Reset the frame rate statistics ahead of each transfer stream
                    SapXferFrameRateInfo stats = xfer.FrameRateStatistics;
                    stats.Reset();
                    xfer.AutoEmpty = false;
                    string str_dir = "";
                    try
                    {
                        while (!cts.IsCancellationRequested&& xfer.Snap(Buffer.Count))
                        {
                            //xfer.Wait(5000);

                            
                            int ii = 0;
                            for ( ii = 0; ii < 500&& (Buffer.get_State(Buffer.Count - 1) != DataState.Full && Buffer.get_State(Buffer.Count - 1) != DataState.Overflow); ii++)
                            {
                                Thread.Sleep(10);
                            }

                            //Task.Factory.StartNew(() => {
                                if ((isVideo || isImg) && !cts.IsCancellationRequested
                                    //&& (Buffer.get_State(0) == DataState.Full || Buffer.get_State(0) == DataState.Overflow) && (Buffer.get_State(Buffer.Count - 1) == DataState.Full || Buffer.get_State(Buffer.Count - 1) == DataState.Overflow) 
                                    )
                                {
                                    if (!Directory.Exists(saveFilePath))
                                    {
                                        Directory.CreateDirectory(saveFilePath);
                                    }
                                    if (isVideo)
                                    {
                                        str_dir = saveFilePath + "Video" + DateTime.Now.ToString("yyyyMMddHHmmssffff") + ".avi";
                                    SaveBufferData(Buffer, SapBuffer.FileFormat.AVI, str_dir, SetOptions(SapBuffer.FileFormat.AVI), 0);
                                        //str_dir = saveFilePath + "Img" + DateTime.Now.ToString("yyyyMMddHHmmssffff") + "." + ImgSaveType;
                                        //SaveBufferData(Buffer, ImgFileFormat, str_dir, SetOptions(ImgFileFormat), 0);
                                        //str_dir = saveFilePath + "Img" + DateTime.Now.ToString("yyyyMMddHHmmssffff") + "." + ImgSaveType;
                                        //SaveBufferData(Buffer, ImgFileFormat, str_dir, SetOptions(ImgFileFormat), Buffer.Count - 1);
                                    }

                                    else if (isImg)
                                    {
                                        for (int i = 0; i < Buffer.Count; i++)
                                        {
                                            str_dir = saveFilePath + "Img" + DateTime.Now.ToString("yyyyMMddHHmmssffff") + "." + ImgSaveType;
                                            SaveBufferData(Buffer, ImgFileFormat, str_dir, SetOptions(ImgFileFormat), i);
                                        }
                                    }
                                }
                            //});

                            
                            //else
                            //{
                            //    for (int i = 0; i < Buffer.Count; i++)
                            //    {
                            //        if ((isVideo || isImg) && !ctsSaveVideo.IsCancellationRequested 
                            //            //&& (Buffer.get_State(i) == DataState.Full || Buffer.get_State(i) == DataState.Overflow) 
                            //            )
                            //        {
                            //            str_dir = saveFilePath + "ImgTiff" + DateTime.Now.ToString("yyyyMMddHHmmssffff") + ".tiff";
                            //            SaveBufferData(Buffer, SapBuffer.FileFormat.TIFF, str_dir, SetOptions(SapBuffer.FileFormat.TIFF), 0);
                            //            Buffer.set_State(i, DataState.Empty);
                            //        }
                            //        else
                            //            break;
                            //    }
                            //}

                            if (isVideo || isImg)
                            {
                                //xfer.AutoEmpty = false;
                            }
                            else
                            {
                                for (int i = 0; i < Buffer.Count; i++)
                                {
                                    Buffer.set_State(i, DataState.Empty);
                                }
                            }

                        }
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("xfer.Wait(5000)");
                        //break;
                    }
                    SetSaveSettings(str_dir, SapBuffer.FileFormat.AVI);
                }
            }
            //));
        }

        private bool displayDataPrepare(ref HYAcquisitionParams _hYAcquisitionParams, ref SapAcquisition acq, ref SapAcqDevice camera, ref SapBuffer buf, ref SapTransfer xfer, ref SapView view, ref PictureBox _pbDisplay)
        {
            try
            {
                loc = new SapLocation(_hYAcquisitionParams.ServerName, _hYAcquisitionParams.ResourceIndex);
                if (SapManager.GetResourceCount(_hYAcquisitionParams.ServerName, SapManager.ResourceType.Acq) > 0)
                {
                    acq = new SapAcquisition(loc, currentConfigFilePath + _hYAcquisitionParams.ConfigFileName);
                    buf = new SapBufferWithTrash(30, acq, SapBuffer.MemoryType.ScatterGather);
                    xfer = new SapAcqToBuf(acq, buf);

                    // Create acquisition object
                    if (!acq.Create())
                    {
                        statusBarMsg = "Error during SapAcquisition creation!";
                        DestroysObjects(acq, camera, buf, xfer, view);
                        return false;
                    }
                    acq.EnableEvent(SapAcquisition.AcqEventType.StartOfFrame);

                }
                else if (SapManager.GetResourceCount(_hYAcquisitionParams.ServerName, SapManager.ResourceType.AcqDevice) > 0)
                {
                    camera = new SapAcqDevice(loc, _hYAcquisitionParams.ConfigFileName);
                    buf = new SapBufferWithTrash(30, AcqDevice, SapBuffer.MemoryType.ScatterGather);
                    xfer = new SapAcqDeviceToBuf(AcqDevice, buf);

                    // Create acquisition object
                    if (!camera.Create())
                    {
                        statusBarMsg = "Error during SapAcqDevice creation!";
                        DestroysObjects(acq, camera, buf, xfer, view);
                        return false;
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("配置文件或者Server选择错误！");
            }


            //view = new SapView(buf, _pbDisplay);

            SapView sv = view;
            SapBuffer sb = buf;
            PictureBox pbd = _pbDisplay;
            Action act = delegate () { sv = new SapView(sb, pbd); };
            _pbDisplay.Invoke(act);

            view = sv;


            // End of frame event
            xfer.Pairs[0].EventType = SapXferPair.XferEventType.EndOfFrame;
            xfer.XferNotify += new SapXferNotifyHandler(xfer_XferNotify);
            xfer.XferNotifyContext = view;

            try
            {
                // Create buffer object
                if (!buf.Create())
                {
                    statusBarMsg = "Error during SapBuffer creation!";
                    DestroysObjects(acq, camera, buf, xfer, view);
                    return false;
                }
            }
            catch (Exception)
            {

                MessageBox.Show("缓存错误！");
            }

            try
            {
                // Create buffer object
                if (!xfer.Create())
                {
                    statusBarMsg = "Error during SapTransfer creation!";
                    DestroysObjects(acq, camera, buf, xfer, view);
                    return false;
                }
            }
            catch (Exception)
            {

                MessageBox.Show("Transfer错误！");
            }

            try
            {
                // Create buffer object
                if (!view.Create())
                {
                    statusBarMsg = "Error during SapView creation!";
                    DestroysObjects(acq, camera, buf, xfer, view);
                    return false;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("View错误！");
            }


            //pbWinFormDisplay.OnSize();

            return true;
        }
        private void xfer_XferNotify(object sender, SapXferNotifyEventArgs args)
        {
            // refresh view
            SapView View = args.Context as SapView;
            View.Show();

            // refresh frame rate
            SapTransfer transfer = sender as SapTransfer;
            if (transfer.UpdateFrameRateStatistics())
            {
                SapXferFrameRateInfo stats = transfer.FrameRateStatistics;
                float framerate = 0.0f;

                if (stats.IsLiveFrameRateAvailable)
                    framerate = stats.LiveFrameRate;

                // check if frame rate is stalled
                if (stats.IsLiveFrameRateStalled)
                {
                    statusBarMsg = "Live Frame rate is stalled.";
                }

                // update FPS only if the value changed by +/- 0.1
                else if ((framerate > 0.0f) && (Math.Abs(lastFrameRate - framerate) > 0.1f))
                {
                    statusBarMsg = "Grabbing at " + framerate + " frames/sec";
                    lastFrameRate = framerate;
                }
            }
        }

        private bool CreateObjects(SapAcquisition acq, SapAcqDevice camera, SapBuffer buf, SapTransfer xfer, SapView view)
        {
            // Create acquisition object
            if (acq != null && !acq.Initialized)
            {
                if (acq.Create() == false)
                {
                    DestroysObjects(acq, camera, buf, xfer, view);
                    return false;
                }
            }
            // Create buffer object
            if (buf != null && !buf.Initialized)
            {
                if (!buf.Create())
                {
                    statusBarMsg = "Error during SapBuffer creation!";
                    DestroysObjects(acq, camera, buf, xfer, view);
                    return false;
                }
                buf.Clear();
            }
            // Create Xfer object
            if (xfer != null && !xfer.Initialized)
            {
                if (!xfer.Create())
                {
                    statusBarMsg = "Error during SapTransfer creation!";
                    DestroysObjects(acq, camera, buf, xfer, view);
                    return false;
                }
            }
            // Create view object
            if (view != null && !view.Initialized)
            {
                if (!view.Create())
                {
                    statusBarMsg = "Error during SapView creation!";
                    DestroysObjects(acq, camera, buf, xfer, view);
                    return false;
                }
                return false;
            }

            return true;
        }
        private void DisposeObjects(SapAcquisition acq, SapAcqDevice camera, SapBuffer buf, SapTransfer xfer, SapView view)
        {
            if (xfer != null)
            { xfer.Dispose(); xfer = null; }
            if (view != null)
            {
                view.Dispose(); view = null;
                //pictureBoxDisplay.View = null; 
            }
            if (buf != null)
            { buf.Dispose(); buf = null; }
            if (acq != null)
            { acq.Dispose(); acq = null; }
            if (camera != null)
            { camera.Dispose(); camera = null; }

        }
        private void DestroysObjects(SapAcquisition acq, SapAcqDevice camera, SapBuffer buf, SapTransfer xfer, SapView view)
        {
            try
            {
                if (xfer != null && xfer.Initialized)
                {
                    xfer.Destroy();
                    xfer.Dispose();
                }

                if (camera != null && camera.Initialized)
                {
                    camera.Destroy();
                    camera.Dispose();
                }

                if (acq != null && acq.Initialized)
                {
                    acq.Destroy();
                    acq.Dispose();
                }

                if (buf != null && buf.Initialized)
                {
                    buf.Destroy();
                    buf.Dispose();
                }

                if (view != null && view.Initialized)
                {
                    view.Destroy();
                    view.Dispose();
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("出错了:" + ex.Message.ToString());
            }

        }

        private string SetOptions(SapBuffer.FileFormat m_Type)
        {
            switch (m_Type)
            {
                case SapBuffer.FileFormat.BMP:
                    return "-format bmp";
                case SapBuffer.FileFormat.TIFF:
                    return "-format tif";
                case SapBuffer.FileFormat.CRC:
                    return "-format crc";
                case SapBuffer.FileFormat.RAW:
                    return "-format raw";
                case SapBuffer.FileFormat.JPEG:
                    return "-format jpg";
                case SapBuffer.FileFormat.JPEG2000:
                    return "-format jp2";
                case SapBuffer.FileFormat.AVI:
                    return "-format avi";
                default:
                    return "";
            }
        }
        private bool SaveBufferData(SapBuffer m_pBuffer, SapBuffer.FileFormat m_Type, string m_PathName, string m_Options, int m_StartFrame)
        {
            //m_Options = "-format avi";
            int conversionType = 0;
            bool flag = false;
            SapFormat format = m_pBuffer.Format;

            for (int i = 0; i < GloableConst.m_ConversionTable.Length; i++)
            {
                if (GloableConst.m_ConversionTable[i].m_BufferFormat == format)
                {
                    switch (m_Type)
                    {
                        case SapBuffer.FileFormat.BMP:
                            conversionType = GloableConst.m_ConversionTable[i].m_FileFormat[0];
                            break;
                        case SapBuffer.FileFormat.TIFF:
                            conversionType = GloableConst.m_ConversionTable[i].m_FileFormat[1];
                            break;
                        case SapBuffer.FileFormat.CRC:
                            conversionType = GloableConst.m_ConversionTable[i].m_FileFormat[2];
                            break;
                        case SapBuffer.FileFormat.RAW:
                            conversionType = GloableConst.m_ConversionTable[i].m_FileFormat[3];
                            break;
                        case SapBuffer.FileFormat.JPEG:
                            conversionType = GloableConst.m_ConversionTable[i].m_FileFormat[4];
                            break;
                        case SapBuffer.FileFormat.JPEG2000:
                            conversionType = GloableConst.m_ConversionTable[i].m_FileFormat[5];
                            break;
                        default:
                            break;
                    }
                    break;
                }
            }

            if (conversionType > 0)
            {
                string message = "";

                switch (conversionType)
                {
                    case GloableConst.CONV_TO_MONO8:
                        message = "Warning: data conversion to MONO8 has taken place to save the image in this file format";
                        break;
                    case GloableConst.CONV_TO_RGB8:
                        message = "Warning: data conversion to RGB888 has taken place to save the image in this file format";
                        break;
                    case GloableConst.CONV_TO_RGB10:
                        message = "Warning: data conversion to RGB101010 has taken place to save the image in this file format";
                        break;
                    case GloableConst.CONV_TO_RGB16:
                        message = "Warning: data conversion to RGB161616 has taken place to save the image in this file format";
                        break;
                }

                Console.WriteLine(message);
                //System.Windows.MessageBox.Show(message);
            }
            try
            {
                
                flag = m_pBuffer.Save(m_PathName, m_Options, m_StartFrame, 0);
            }
            catch (Exception)
            {
                return false;
                throw;
            }

            for (int i = 0; i < m_pBuffer.Count; i++)
            {
                m_pBuffer.set_State(i, DataState.Empty);
            }
            return flag;
        }

        private void LoadSaveSettings(string m_PathName, ref SapBuffer.FileFormat m_Type)
        {
            // Read file name
            string keyPath = "Software\\Teledyne DALSA\\" + winForm.Application.ProductName + "\\SapFile";
            RegistryKey regkey = Registry.CurrentUser.OpenSubKey(keyPath, true);
            if (regkey != null)
            {
                m_PathName = regkey.GetValue("Name", "").ToString();
                m_Type = (SapBuffer.FileFormat)(regkey.GetValue("Type", 0));
            }
        }

        private void SetSaveSettings(string m_PathName, SapBuffer.FileFormat m_Type)
        {
            // Write file name
            string keyPath = "Software\\Teledyne DALSA\\" + winForm.Application.ProductName + "\\SapFile";
            RegistryKey regKey = Registry.CurrentUser.CreateSubKey(keyPath);
            regKey.SetValue("Name", m_PathName);
            regKey.SetValue("Type", (int)m_Type);
        }

        private void btnFreeze_Click(object sender, RoutedEventArgs e)
        {
            if (!isStart)
            {
                //System.Windows.Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() =>
                {
                    //ctsLLC = new CancellationTokenSource();
                    ctsSaveVideo = new CancellationTokenSource();
                    //ctsSSBD = new CancellationTokenSource();

                    if (displayDataPrepare(ref hYAcquisitionParams, ref Acq, ref AcqDevice, ref Buffers, ref Xfer, ref View, ref pbDisplay))
                    {
                        isStart = true;
                        btnImage.Background = Brushes.Green;
                        btnImage.IsEnabled = true;

                        btnVideo.Background = Brushes.Green;
                        btnVideo.IsEnabled = true;
                        btnFreeze.Background = Brushes.Red;
                        btnFreeze.ToolTip = "停止";
                        btnImage.IsEnabled = true;
                        btnVideo.IsEnabled = true;
                        //btnFreeze.IsEnabled = true;
                        iconFreeze.Kind = PackIconBootstrapIconsKind.PauseCircle;

                        //View.SetScalingMode(new System.Drawing.Rectangle(0, 0, 640, 512), new System.Drawing.Rectangle(0, 0, Buffers.Width, Buffers.Height));
                        View.SetScalingMode(800.0f / Buffers.Width, 800.0f / Buffers.Width);
                        //taskLLC = Task.Factory.StartNew(() => snapAndSaveBufData(), ctsLLC.Token);
                        taskLLC = Task.Factory.StartNew(() => snapAndSaveBufData(Xfer, Buffers, ctsSaveVideo), ctsSaveVideo.Token);

                        //taskLLC = Task.Factory.StartNew(() => displayDataPrepare(ref hYAcquisitionParams, ref Acq, ref AcqDevice, ref Buffers, ref Xfer, ref View, ref pbDisplay), ctsLLC.Token);
                        //taskLLC.ContinueWith(t => Task.Factory.StartNew(() => snapAndSaveBufData(), ctsSSBD.Token));

                        //taskLLC.ContinueWith(t => Task.Factory.StartNew(() => Xfer.Grab(), ctsSSBD.Token));

                        statusBarMsg = "Grab started!";
                    }
                    else
                    {
                        MessageBox.Show("配置文件或者采集卡选择错误！");
                    }
                }
                //));
            }
            else
            {
                if (Xfer != null && Xfer.Initialized)
                {
                    isStart = false;
                    iconFreeze.Kind = PackIconBootstrapIconsKind.PlayCircle;
                    btnImage.Background = Brushes.Gray;
                    btnImage.IsEnabled = false;
                    btnVideo.Background = Brushes.Gray;
                    btnVideo.IsEnabled = false;
                    btnFreeze.Background = Brushes.Green;
                    btnFreeze.ToolTip = "开始";
                    btnImage.IsEnabled = false;
                    btnVideo.IsEnabled = false;
                    //btnFreeze.IsEnabled = false;
                    //Xfer.Freeze();
                    isVideo = false;
                    if (!ctsSaveVideo.IsCancellationRequested)
                    {
                        ctsSaveVideo.Cancel();
                    }

                    statusBarMsg = "Grab Freezed!";
                    //Xfer.Wait(1000);
                    DestroysObjects(Acq, AcqDevice, Buffers, Xfer, View);
                    loc.Dispose();
                }
            }
        }
        private void btVideo_Click(object sender, RoutedEventArgs e)
        {
            if (!isVideo)
            {
                btnVideo.Background = Brushes.Red;
                isVideo = true;
                btnImage.IsEnabled = false;
                btnImage.Background = Brushes.Gray;
                btnFreeze.IsEnabled = false;
                btnFreeze.Background = Brushes.Gray;
            }
            else
            {
                isVideo = false;
                btnFreeze.IsEnabled = true;
                btnFreeze.Background = Brushes.Red;
                btnImage.IsEnabled = true;
                btnImage.Background = Brushes.Green;
                btnVideo.Background = Brushes.Green;
            }

        }
        private void btnImage_Click(object sender, RoutedEventArgs e)
        {
            if (!isImg)
            {
                isImg = true;
                btnImage.Background = Brushes.Red;
                btnVideo.IsEnabled = false;
                btnVideo.Background = Brushes.Gray;
                btnFreeze.IsEnabled = false;
                btnFreeze.Background = Brushes.Gray;
            }
            else
            {
                isImg = false;
                btnFreeze.IsEnabled = true;
                btnFreeze.Background = Brushes.Red;
                btnVideo.IsEnabled = true;
                btnVideo.Background = Brushes.Green;
                btnImage.Background = Brushes.Green;
            }

        }
        private void cbServer_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string serverName = cbServer.SelectedItem.ToString();
            if (serverName != null && serverName != "")
            {
                HYAcquisitionSettingUtils.GetDevices(serverName, deviceNameList, out _statusBarMsg);
                hYAcquisitionParams.ServerName = serverName;
            }
        }
        private void cbDevice_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            hYAcquisitionParams.ResourceName = cbDevice.SelectedItem.ToString();
            if (true)
            {
                if (SapManager.GetResourceCount(hYAcquisitionParams.ServerName, SapManager.ResourceType.Acq) > 0)
                    hYAcquisitionParams.ResourceType = SapManager.ResourceType.Acq;
                else if (SapManager.GetResourceCount(hYAcquisitionParams.ServerName, SapManager.ResourceType.AcqDevice) > 0)
                    hYAcquisitionParams.ResourceType = SapManager.ResourceType.AcqDevice;
            }
        }

        private void rbtnImgType_Checked(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.RadioButton RB = sender as System.Windows.Controls.RadioButton;
            if (RB.IsChecked == true)
            {
                switch (RB.Content)
                {
                    case "BMP":
                        ImgSaveType = "bmp";
                        ImgFileFormat = SapBuffer.FileFormat.BMP;
                        break;
                    case "TIFF":
                        ImgSaveType = "tiff";
                        ImgFileFormat = SapBuffer.FileFormat.TIFF;
                        break;
                    default:
                        break;
                }
            }
        }

        private void cbConfigFile_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbConfigFile.SelectedItem != null)
            {
                hYAcquisitionParams.ConfigFileName = cbConfigFile.SelectedItem.ToString();
            }
            else
            {
                hYAcquisitionParams.ConfigFileName = "";
            }

        }
        private void btConfigFileBrowser_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "请选择配置文件所在目录";
            DialogResult res = dialog.ShowDialog();
            if (res == winForm.DialogResult.OK)
            {
                if (!string.IsNullOrEmpty(dialog.SelectedPath))
                {
                    currentConfigFilePath = dialog.SelectedPath;
                    btConfigFileBrowser.ToolTip = currentConfigFilePath;
                }
                configFileList.Clear();
                HYAcquisitionSettingUtils.GetConfigFilesInDirectry(currentConfigFilePath, configFileList, out _statusBarMsg);
            }
        }











    }
}
