using HelixToolkit.Wpf;
using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Media3D;

namespace PC
{
    public class SphereManipulator : TranslateManipulator
    {
        private Point3D lastPoint;
        private Point pointClicked;
        private Point3D? DirectionPoint;


        public int Width
        {
            get { return (int)GetValue(WidthProperty); }
            set { SetValue(WidthProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Width.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty WidthProperty =
            DependencyProperty.Register("Width", typeof(int), typeof(SphereManipulator), new PropertyMetadata(default(int), WidthPropertyChanged));

        private static void WidthPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as SphereManipulator).UpdateGeometry();
        }

        
        public int Height
        {
            get { return (int)GetValue(HeightProperty); }
            set { SetValue(HeightProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Height.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HeightProperty =
            DependencyProperty.Register("Height", typeof(int), typeof(SphereManipulator), new PropertyMetadata(default(int), HeightPropertyChanged));

        private static void HeightPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as SphereManipulator).UpdateGeometry();
        }

        protected override void UpdateGeometry()
        {
            var mb = new MeshBuilder(false, false);
            var p0 = new Point3D(0, 0, 0);
            var d = this.Direction;
            d.Normalize();
            var p1 = p0 + (d * this.Length);
            //mb.AddArrow(p0, p1, this.Diameter);
            mb.AddEllipsoid(p1, this.Width, this.Height, 4);

            this.Model.Geometry = mb.ToMesh();
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);

            var p = e.GetPosition(this.ParentViewport);
            //var direction = this.ToWorld(this.Direction);


            hitPlaneOrigin = ToWorld(this.Position);
            cam = ToWorld(Camera.LookDirection);

            var ponebene = GetHitPlanePoint(p, hitPlaneOrigin, cam);

            this.lastPoint = ToWorld((Point3D)ponebene);
            this.CaptureMouse();
        }

        private Point3D hitPlaneOrigin;
        private Vector3D cam;
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (this.IsMouseCaptured)
            {
                // Die Mausposition ermitteln
                var p = e.GetPosition(this.ParentViewport);

                //var hitPlaneOrigin = ToWorld(this.Position);
                //var cam = ToWorld(Camera.LookDirection);

                var ponebene = GetHitPlanePoint(p, hitPlaneOrigin, cam);
                ponebene = ToWorld((Point3D)ponebene);

                var ponlocal = ToLocal(ponebene.Value);
                Point3D pon = ponebene.Value;
                pon = ToLocal(pon);
                pon.Z = 0;

                this.Position = pon;


            }
        }

        private Point3D? GetNearestPoint(Point position, Point3D hitPlaneOrigin, Vector3D hitPlaneNormal)
        {
            var hpp = this.GetHitPlanePoint(position, hitPlaneOrigin, hitPlaneNormal);
            if (hpp == null)
            {
                return null;
            }

            var ray = new Ray3D(this.ToWorld(this.Position), this.ToWorld(this.Direction));
            return ray.GetNearest(hpp.Value);
        }

    }
}
