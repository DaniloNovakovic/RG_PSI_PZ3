using RG_PSI_PZ3.Helpers;
using RG_PSI_PZ3.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Media3D;
using Point = System.Windows.Point;

namespace RG_PSI_PZ3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly int zoomMax = 30;
        private Point diffOffset = new Point();
        private Point start = new Point();
        private int zoomCurent = 1;
        private Configuration _config;

        public MainWindow()
        {
            InitializeComponent();

            _config = new Configuration();
        }

        private void Viewport_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _viewport.CaptureMouse();
            start = e.GetPosition(this);
            diffOffset.X = _translateTransform.OffsetX;
            diffOffset.Y = _translateTransform.OffsetY;
        }

        private void Viewport_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            _viewport.ReleaseMouseCapture();
        }

        private void Viewport_MouseMove(object sender, MouseEventArgs e)
        {
            if (_viewport.IsMouseCaptured)
            {
                var end = e.GetPosition(this);
                double offsetX = end.X - start.X;
                double offsetY = end.Y - start.Y;
                double w = Width;
                double h = Height;
                double translateX = (offsetX * 100) / w;
                double translateY = -(offsetY * 100) / h;
                _translateTransform.OffsetX = diffOffset.X + (translateX / (100 * _scaleTransform.ScaleX));
                _translateTransform.OffsetY = diffOffset.Y + (translateY / (100 * _scaleTransform.ScaleX));
            }
        }

        private void Viewport_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            var p = e.MouseDevice.GetPosition(this);
            double scaleX = 1;
            double scaleY = 1;
            if (e.Delta > 0 && zoomCurent < zoomMax)
            {
                scaleX = _scaleTransform.ScaleX + 0.1;
                scaleY = _scaleTransform.ScaleY + 0.1;
                zoomCurent++;
                _scaleTransform.ScaleX = scaleX;
                _scaleTransform.ScaleY = scaleY;
            }
            else if (e.Delta <= 0 && zoomCurent > -zoomMax)
            {
                scaleX = _scaleTransform.ScaleX - 0.1;
                scaleY = _scaleTransform.ScaleY - 0.1;
                zoomCurent--;
                _scaleTransform.ScaleX = scaleX;
                _scaleTransform.ScaleY = scaleY;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var storage = LoadXmlToStorage();
            DrawEntities(storage);
        }

        private Storage LoadXmlToStorage()
        {
            var loader = new GeographicXmlLoader()
            {
                LatitudeRange = _config.LatitudeRange,
                LongitudeRange = _config.LongitudeRange
            };
            var substationEntities = loader.GetSubstationEntities();
            var nodeEntities = loader.GetNodeEntities();
            var switchEntities = loader.GetSwitchEntities();
            var lineEntities = loader.GetLineEntities();

            var storage = new Storage();
            storage.AddRange(substationEntities);
            storage.AddRange(nodeEntities);
            storage.AddRange(switchEntities);
            storage.AddValidLines(lineEntities);

            return storage;
        }

        private void DrawEntities(Storage storage)
        {
            var latlonToPlaneMapper = new LatLonToPlaneMapper(_config);
            var powerEntityMapper = new PowerEntityTo3DMapper(latlonToPlaneMapper);
            var lineMapper = new LineEntityTo3DMapper(latlonToPlaneMapper);
            var painter = new Painter3D(_modelGroup);
            painter.DrawPowerEntities(storage, powerEntityMapper);
            painter.DrawLines(storage, lineMapper);
        }
    }
}