﻿using System;
using OpenTK;

namespace Velosip3d
{
    internal class MainCamera
    {
        Vector3 Position = Vector3.Zero;
        Vector3 Orientation = new Vector3((float)Math.PI, 0f, 0f);
        public float MoveSpeed = 0.2f;
        public float MouseSensitivity = Configs.Controls.mouseSens;

        public Matrix4 GetViewMatrix()
        {
            Vector3 lookat = new Vector3();

            lookat.X = (float)(Math.Sin(Orientation.X) * Math.Cos(Orientation.Y));
            lookat.Y = (float)Math.Sin(Orientation.Y);
            lookat.Z = (float)(Math.Cos(Orientation.X) * Math.Cos(Orientation.Y));

            return Matrix4.LookAt(Position, Position + lookat, Vector3.UnitY);
        }

        public void Move(float x, float y, float z)
        {
            Tools.LogM();

            Vector3 offset = new Vector3();

            Vector3 forward = new Vector3((float)Math.Sin(Orientation.X), 0, (float)Math.Cos(Orientation.X));
            Vector3 right = new Vector3(-forward.Z, 0, forward.X);

            offset += x * right;
            offset += y * forward;
            offset.Y += z;

            offset.NormalizeFast();
            offset = Vector3.Multiply(offset, MoveSpeed);

            Position += offset;
        }

        public void MoveToPoint(Vector3 point)
        {
            Tools.LogM();

            Position = point;
        }

        public void AddRotation(float x, float y)
        {
            Tools.LogM();

            x = x * MouseSensitivity;
            y = y * MouseSensitivity;

            Orientation.X = (Orientation.X + x) % ((float)Math.PI * 2.0f);
            Orientation.Y = Math.Max(Math.Min(Orientation.Y + y, (float)Math.PI / 2.0f - 0.1f), (float)-Math.PI / 2.0f + 0.1f);
        }
    }
}
