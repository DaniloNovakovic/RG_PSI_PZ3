﻿using RG_PSI_PZ3.Helpers;
using RG_PSI_PZ3.Helpers.Behaviors;
using RG_PSI_PZ3.Models;
using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using Point = System.Windows.Point;

namespace RG_PSI_PZ3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly int _zoomMax = 30;
        private Point _diffOffset = new Point();
        private Point _start = new Point();
        private int _zoomCurent = 1;
        private double _prevOffset = 0;
        private readonly Configuration _config;
        private readonly Storage _storage;
        private readonly LineClickBehavior _lineClickBehavior;
        private readonly PowerEntityClickBehavior _powerEntityClickBehavior;

        public MainWindow()
        {
            InitializeComponent();

            _config = new Configuration();
            _storage = new Storage();

            _lineClickBehavior = new LineClickBehavior(_storage);
            _powerEntityClickBehavior = new PowerEntityClickBehavior(this, _storage);
        }

        private void Viewport_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _viewport.CaptureMouse();
            _start = e.GetPosition(this);
            _diffOffset.X = _translateTransform.OffsetX;
            _diffOffset.Y = _translateTransform.OffsetY;

            var mouseposition = e.GetPosition(_viewport);
            var testpoint3D = new Point3D(mouseposition.X, mouseposition.Y, 0);
            var testdirection = new Vector3D(mouseposition.X, mouseposition.Y, 10);

            var pointparams = new PointHitTestParameters(mouseposition);
            var rayparams = new RayHitTestParameters(testpoint3D, testdirection);

            VisualTreeHelper.HitTest(_viewport, null, HTResult, pointparams);
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
                double offsetX = end.X - _start.X;
                double offsetY = end.Y - _start.Y;
                double w = Width;
                double h = Height;
                double translateX = (offsetX * 100) / w;
                double translateY = -(offsetY * 100) / h;

                if (e.LeftButton == MouseButtonState.Pressed)
                {
                    _translateTransform.OffsetX = _diffOffset.X + (translateX / (100 * _scaleTransform.ScaleX));
                    _translateTransform.OffsetY = _diffOffset.Y + (translateY / (100 * _scaleTransform.ScaleX));
                }

                if (e.RightButton == MouseButtonState.Pressed)
                {
                    double rotOffset = offsetY > _prevOffset ? translateY : -translateY;
                    _rotateAxisZ.Angle = (_rotateAxisZ.Angle + rotOffset / 10) % 360;
                }
                _prevOffset = offsetY;
            }
        }

        private void Viewport_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            var p = e.MouseDevice.GetPosition(this);
            double scaleX = 1;
            double scaleY = 1;
            if (e.Delta > 0 && _zoomCurent < _zoomMax)
            {
                scaleX = _scaleTransform.ScaleX + 0.1;
                scaleY = _scaleTransform.ScaleY + 0.1;
                _zoomCurent++;
                _scaleTransform.ScaleX = scaleX;
                _scaleTransform.ScaleY = scaleY;
            }
            else if (e.Delta <= 0 && _zoomCurent > -_zoomMax)
            {
                scaleX = _scaleTransform.ScaleX - 0.1;
                scaleY = _scaleTransform.ScaleY - 0.1;
                _zoomCurent--;
                _scaleTransform.ScaleX = scaleX;
                _scaleTransform.ScaleY = scaleY;
            }
        }

        private HitTestResultBehavior HTResult(HitTestResult rawresult)
        {
            if (rawresult is RayHitTestResult rayResult)
            {
                var tagProp = rayResult.ModelHit.GetValue(TagProperty);

                if (tagProp is PowerEntity powerEntity)
                {
                    _lineClickBehavior.UndoPrevClick();
                    _powerEntityClickBehavior.OnClick(powerEntity);

                    Console.WriteLine($"Clicked on {powerEntity}");
                }
                else if (tagProp is LineEntity lineEntity)
                {
                    _lineClickBehavior.OnClick(lineEntity);

                    Console.WriteLine($"Clicked on {lineEntity}");
                }
            }

            return HitTestResultBehavior.Stop;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var loader = new GeographicXmlLoader()
            {
                LatitudeRange = _config.LatitudeRange,
                LongitudeRange = _config.LongitudeRange
            };

            StorageFactory.LoadXMLToStorage(loader, _storage);

            var latlonToPlaneMapper = new LatLonToPlaneMapper(_config);
            var powerEntityMapper = new PowerEntityTo3DMapper(latlonToPlaneMapper);
            var lineMapper = new LineEntityTo3DMapper(latlonToPlaneMapper);
            var painter = new Painter3D(_modelGroup, powerEntityMapper, lineMapper);

            painter.DrawEntities(_storage);
        }

        private void Viewport_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            _viewport.CaptureMouse();
        }

        private void Viewport_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            _viewport.ReleaseMouseCapture();
        }
    }
}