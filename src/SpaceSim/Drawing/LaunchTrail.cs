﻿using System.Collections.Generic;
using System.Drawing;
using SpaceSim.Physics;
using VectorMath;

namespace SpaceSim.Drawing
{
    class LaunchTrail
    {
        private List<double> _trailAngles;
        private List<double> _trailDistances;
        private List<bool> _trailPowered;

        public LaunchTrail()
        {
            _trailAngles = new List<double>();
            _trailDistances = new List<double>();
            _trailPowered = new List<bool>();
        }

        public void AddPoint(DVector2 point, IGravitationalBody parentBody, bool isPowered)
        {
            DVector2 offset = point - parentBody.Position;

            _trailAngles.Add(offset.Angle() - parentBody.Rotation);
            _trailDistances.Add(offset.Length());

            _trailPowered.Add(isPowered);
        }

        public void Draw(Graphics graphics, RectangleD cameraBounds, IGravitationalBody parentBody)
        {
            var poweredTrails = new List<RectangleF>();
            var coastTrails = new List<RectangleF>();

            for (int i = 0; i < _trailAngles.Count; i++)
            {
                double angle = _trailAngles[i] + parentBody.Rotation;
                double distance = _trailDistances[i];

                DVector2 worldPoint = DVector2.FromAngle(angle) * distance + parentBody.Position;

                if (cameraBounds.Contains(worldPoint))
                {
                    PointF localPoint = RenderUtils.WorldToScreen(worldPoint, cameraBounds);

                    if (_trailPowered[i])
                    {
                        poweredTrails.Add(new RectangleF(localPoint.X, localPoint.Y, 2, 2));
                    }
                    else
                    {
                        coastTrails.Add(new RectangleF(localPoint.X, localPoint.Y, 2, 2));
                    }
                }
            }

            RenderUtils.DrawRectangles(graphics, poweredTrails, Color.Red);
            RenderUtils.DrawRectangles(graphics, coastTrails, Color.White);
        }
    }
}
