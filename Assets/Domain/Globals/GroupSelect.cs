using UnityEngine;

namespace Domain.Globals
{
    public static class GroupSelect
    {
        public static Texture2D _tex;

        public static Texture2D tex
        {
            get
            {
                if (_tex == null)
                {
                    _tex = new Texture2D(1, 1);
                    _tex.SetPixel(0, 0, Color.green);
                    _tex.Apply();
                }

                return _tex;
            }
        }

        public static void DrawSelectionBox(Rect rect, Color color)
        {
            GUI.color = color;
            GUI.DrawTexture(rect, tex);
        }

        public static void DrawBoxBorder(Rect rect, float thickness, Color color)
        {
            DrawSelectionBox(new Rect(rect.xMin, rect.yMin, rect.width, thickness), color);
            DrawSelectionBox(new Rect(rect.xMin, rect.yMax - thickness, rect.width, thickness), color);
            DrawSelectionBox(new Rect(rect.xMin, rect.yMin, thickness, rect.height), color);
            DrawSelectionBox(new Rect(rect.xMax - thickness, rect.yMin, thickness, rect.height), color);
        }

        public static Rect GetScreenBox(Vector3 mousePos1, Vector3 mousePos2)
        {
            mousePos1.y = Screen.height - mousePos1.y;
            mousePos2.y = Screen.height - mousePos2.y;

            Vector3 bottomR = Vector3.Max(mousePos1, mousePos2);
            Vector3 topL = Vector3.Min(mousePos1, mousePos2);

            return Rect.MinMaxRect(topL.x, topL.y, bottomR.x, bottomR.y);

        }

        public static Bounds GetViewBounds(Camera cam, Vector3 screenPos1, Vector3 screenPos2)
        {
            Vector3 pos1 = cam.ScreenToViewportPoint(screenPos1);
            Vector3 pos2 = cam.ScreenToViewportPoint(screenPos2);

            Vector3 min = Vector3.Min(pos1, pos2);
            Vector3 max = Vector3.Max(pos1, pos2);

            min.z = cam.nearClipPlane;
            max.z = cam.farClipPlane;

            Bounds bounds = new Bounds();

            bounds.SetMinMax(min, max);

            return bounds;
        }
    }
}

