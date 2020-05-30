using RG_PSI_PZ3.Helpers;
using RG_PSI_PZ3.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Point = System.Windows.Point;

namespace RG_PSI_PZ3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly int zoomMax = 7;
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
            LoadXml();
        }

        private void LoadXml()
        {
            var loader = new GeographicXmlLoader()
            {
                LatitudeRange = _config.LatitudeRange,
                LongitudeRange = _config.LongitudeRange
            };

            var latlonToPlaneMapper = new LatLonToPlaneMapper(_config);
            var powerEntityMapper = new PowerEntityTo3DMapper(latlonToPlaneMapper);

            // Draw Nodes
            var substationEntities = loader.GetSubstationEntities();
            DrawNodes(substationEntities, powerEntityMapper);

            var nodeEntities = loader.GetNodeEntities();
            DrawNodes(nodeEntities, powerEntityMapper);

            var switchEntities = loader.GetSwitchEntities();
            DrawNodes(switchEntities, powerEntityMapper);

            // Draw Lines
            var lineMapper = new LineEntityTo3DMapper(latlonToPlaneMapper);
            var lineEntities = loader.GetLineEntities();
            DrawLines(lineEntities, lineMapper);
        }

        private void DrawLines(IEnumerable<LineEntity> lineEntities, LineEntityTo3DMapper mapper)
        {
            foreach (var entity in lineEntities)
            {
                var models = mapper.MapTo3D(entity);
                models.ForEach(g => _modelGroup.Children.Add(g));
            }
        }

        private void DrawNodes(IEnumerable<PowerEntity> entitites, PowerEntityTo3DMapper mapper)
        {
            foreach (var entity in entitites)
            {
                var model = mapper.MapTo3D(entity);
                _modelGroup.Children.Add(model);
            }
        }
    }
}