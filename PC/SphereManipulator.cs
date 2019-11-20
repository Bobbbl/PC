using HelixToolkit.Wpf;
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

        protected override void UpdateGeometry()
        {
            var mb = new MeshBuilder(false, false);
            var p0 = new Point3D(0, 0, 0);
            var d = this.Direction;
            d.Normalize();
            var p1 = p0 + (d * this.Length);
            //mb.AddArrow(p0, p1, this.Diameter);
            mb.AddEllipsoid(p1, 1, 4, 4);

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
