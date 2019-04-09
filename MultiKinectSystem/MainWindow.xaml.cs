using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using Microsoft.Kinect;


namespace MultiKinectSystem
{
    public partial class MainWindow : Window
    {
        public KinectSensor KinectDevice;
        public KinectSensor KinectDevice1;
        private readonly Brush[] SkeletonBrushes;
        private Skeleton[] FrameSkeletons;
        Boolean kinect0Assign = false;
        Boolean kinect1Assign = false;
        //depthstream
        private WriteableBitmap _DepthImage;
        private Int32Rect _DepthImageRect;
        private short[] _DepthPixelData;
        private int _DepthImageStride;

        //raw depth stream
        private WriteableBitmap _RawDepthImage;
        private Int32Rect _RawDepthImageRect;
        private int _RawDepthImageStride;
        private short[] _DepthImagePixelData;
        private WriteableBitmap _RawDepthImage1;
        private Int32Rect _RawDepthImageRect1;
        private int _RawDepthImageStride1;
        private short[] _DepthImagePixelData1;
        //
        private const int LoDepthThreshold = 1220;
        private const int HiDepthThreshold = 3048;
        //color steam
        private WriteableBitmap _ColorImageBitmap;
        private WriteableBitmap _ColorImageBitmap1;
        private Int32Rect _ColorImageBitmapRect;
        private Int32Rect _ColorImageBitmapRect1;
        private int _ColorImageStride;
        private int _ColorImageStride1;
        private byte[] _ColorImagePixelData;
        private byte[] _ColorImagePixelData1;
        // fps
        private int _TotalFrames;
        private DateTime _StartFrameTime;
        public MainWindow()
        {
            InitializeComponent();
            DiscoverKinectSensor();
            CompositionTarget_Rendering();
        }



        private void DiscoverKinectSensor()
        {
            //make sure all point initail
            if (this.KinectDevice != null && this.KinectDevice.Status != KinectStatus.Connected)
            {
                this.KinectDevice = null;
            }
            if (this.KinectDevice1 != null && this.KinectDevice1.Status != KinectStatus.Connected)
            {
                this.KinectDevice1 = null;
            }
            if (this.KinectDevice == null && this.KinectDevice1 == null)
            {
                foreach (var potentialSensor in KinectSensor.KinectSensors)
                {
                    if (potentialSensor.Status == KinectStatus.Connected)
                    {
                        if (kinect0Assign == false)
                        {
                            KinectDevice = potentialSensor;
                            kinect0Assign = true;
                        }
                        else
                        {
                            KinectDevice1 = potentialSensor;
                            kinect1Assign = true;
                        }
                    }
                }
            }
        }

        // depth image
        private void KinectDevice_DepthFrameReady(object sender, DepthImageFrameReadyEventArgs e)
        {
            DepthImageFrame frame = e.OpenDepthImageFrame();
                if (frame != null)
                {
                    frame.CopyPixelDataTo(_DepthImagePixelData);
                    _RawDepthImage.WritePixels(_RawDepthImageRect, _DepthImagePixelData, _RawDepthImageStride, 0);
                }
            FramesPerSecondElement.Text = string.Format("{0:0} fps", (this._TotalFrames++ / DateTime.Now.Subtract(this._StartFrameTime).TotalSeconds));
        }
        private void KinectDevice_DepthFrameReady1(object sender, DepthImageFrameReadyEventArgs e)
        {
            DepthImageFrame frame = e.OpenDepthImageFrame();
                if (frame != null)
                {
                    frame.CopyPixelDataTo(_DepthImagePixelData);
                  _RawDepthImage1.WritePixels(_RawDepthImageRect, _DepthImagePixelData, _RawDepthImageStride, 0);
                }
            FramesPerSecondElement1.Text = string.Format("{0:0} fps", (this._TotalFrames++ / DateTime.Now.Subtract(this._StartFrameTime).TotalSeconds));
        }

        private void InitializeRawDepthImage0(DepthImageStream depthStream)
        {
            if (depthStream == null)
            {
                _RawDepthImage = null;
                _RawDepthImageRect = new Int32Rect();
                _RawDepthImageStride = 0;
                _DepthImagePixelData = null;
            }
            else
            {
                _RawDepthImage = new WriteableBitmap(depthStream.FrameWidth, depthStream.FrameHeight, 96, 96, PixelFormats.Gray16, null);
                _RawDepthImageRect = new Int32Rect(0, 0, depthStream.FrameWidth, depthStream.FrameHeight);
                _RawDepthImageStride = depthStream.FrameBytesPerPixel * depthStream.FrameWidth;
                _DepthImagePixelData = new short[depthStream.FramePixelDataLength];
            }

           DepthImage0.Source = _RawDepthImage;
        }

        private void InitializeRawDepthImage1(DepthImageStream depthStream)
        {
            if (depthStream == null)
            {
                _RawDepthImage1 = null;
                _RawDepthImageRect1 = new Int32Rect();
                _RawDepthImageStride1 = 0;
                _DepthImagePixelData1 = null;
            }
            else
            {
                _RawDepthImage1 = new WriteableBitmap(depthStream.FrameWidth, depthStream.FrameHeight, 96, 96, PixelFormats.Gray16, null);
                _RawDepthImageRect1 = new Int32Rect(0, 0, depthStream.FrameWidth, depthStream.FrameHeight);
                _RawDepthImageStride1 = depthStream.FrameBytesPerPixel * depthStream.FrameWidth;
                _DepthImagePixelData1 = new short[depthStream.FramePixelDataLength];
            }

             DepthImage1.Source = _RawDepthImage1;
        }

        private void EnableDepthImage0(KinectSensor PointKinect)
        {
            if (PointKinect != null)
            {
                if (PointKinect.Status == KinectStatus.Connected)
                {
                    PointKinect.DepthStream.Enable();
                    InitializeRawDepthImage0(PointKinect.DepthStream);
                    PointKinect.DepthFrameReady += KinectDevice_DepthFrameReady;
                    PointKinect.Start();

                    _StartFrameTime = DateTime.Now;
                }
            }
        }

        private void EnableDepthImage1(KinectSensor PointKinect)
        {
            if (PointKinect != null)
            {
                if (PointKinect.Status == KinectStatus.Connected)
                {
                    PointKinect.DepthStream.Enable();
                    InitializeRawDepthImage1(PointKinect.DepthStream);
                    PointKinect.DepthFrameReady += KinectDevice_DepthFrameReady1;
                    PointKinect.Start();

                    _StartFrameTime = DateTime.Now;
                }
            }
        }

        //color image
        private void Enable_Kinect0_ColorImage()
        {

                KinectDevice.ColorStream.Enable();
                KinectDevice.Start();

                ColorImageStream colorStream = KinectDevice.ColorStream;
                _ColorImageBitmap = new WriteableBitmap(colorStream.FrameWidth, colorStream.FrameHeight, 96, 96, PixelFormats.Bgr32, null);
                _ColorImageBitmapRect = new Int32Rect(0, 0, colorStream.FrameWidth, colorStream.FrameHeight);
                _ColorImageStride = colorStream.FrameWidth * colorStream.FrameBytesPerPixel;
                //ColorImageElement.Source = _ColorImageBitmap;
                _ColorImagePixelData = new byte[colorStream.FramePixelDataLength];
        }

        private void Enable_Kinect1_ColorImage()
        {

            KinectDevice1.ColorStream.Enable();
            KinectDevice1.Start();

            ColorImageStream colorStream1 = KinectDevice1.ColorStream;
            _ColorImageBitmap1 = new WriteableBitmap(colorStream1.FrameWidth, colorStream1.FrameHeight, 96, 96, PixelFormats.Bgr32, null);
            _ColorImageBitmapRect1 = new Int32Rect(0, 0, colorStream1.FrameWidth, colorStream1.FrameHeight);
            _ColorImageStride1 = colorStream1.FrameWidth * colorStream1.FrameBytesPerPixel;
            //ColorImageElement1.Source = _ColorImageBitmap1;
            _ColorImagePixelData1 = new byte[colorStream1.FramePixelDataLength];
        }

        private void PollColorImageStream()
        {
            if (KinectDevice == null)
            {
                //TODO: Display a message to plug-in a Kinect.
            }
            else
            {
                try
                {
                    using (ColorImageFrame frame = KinectDevice.ColorStream.OpenNextFrame(100))
                    {
                        if (frame != null)
                        {
                            frame.CopyPixelDataTo(_ColorImagePixelData);
                            _ColorImageBitmap.WritePixels(_ColorImageBitmapRect, _ColorImagePixelData, _ColorImageStride, 0);
                        }
                    }
                }
                catch (Exception ex)
                {
                    //TODO: Report an error message
                }
            }
            if (KinectDevice1 == null)
            {
                //TODO: Display a message to plug-in a Kinect.
            }
            else
            {
                try
                {
                    using (ColorImageFrame frame1 = KinectDevice1.ColorStream.OpenNextFrame(100))
                    {
                        if (frame1 != null)
                        {
                            frame1.CopyPixelDataTo(_ColorImagePixelData1);
                            _ColorImageBitmap1.WritePixels(_ColorImageBitmapRect1, _ColorImagePixelData1, _ColorImageStride1, 0);
                        }
                    }

                }
                catch (Exception ex)
                {
                    //TODO: Report an error message
                }
            }
        }

        private void CompositionTarget_Rendering()
        {
            EnableDepthImage0(KinectDevice);
            EnableDepthImage1(KinectDevice1);
        }
    }
}
