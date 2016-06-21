using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Windows.Forms;

namespace PanZoom02
{
    public partial class Form1 : Form
    {
        public class Canvas : Panel
        {
            protected override void OnPaintBackground(PaintEventArgs aPaintEventArgs) 
            {
            }
        }

        Bitmap iBitmap;        
        BufferedGraphicsContext iCurrentContext;
        BufferedGraphics iBuffer;  
        PointF iViewPortCenter;
        float iZoom = 1.0f;

        bool iDragging = false;
        Point iLastMousePosition;

        int iMoveStep = 0;
        int iDirection = 0;

        public Form1()
        {
            InitializeComponent();
            iCurrentContext = BufferedGraphicsManager.Current;            
            Setup(false);
        }

        private void Setup(bool aResetViewport)
        {
            if (iBuffer != null)
            {
                iBuffer.Dispose();
            }

            iBuffer = iCurrentContext.Allocate(this.canvas.CreateGraphics(), this.canvas.DisplayRectangle);
            if (iBitmap != null)
            {
                if (aResetViewport)
                {
                    SetViewPort(new RectangleF(0, 0, iBitmap.Width, iBitmap.Height));
                }
            }            
            this.canvas.Focus();
            this.canvas.Invalidate();
        }        

        private void SetViewPort(RectangleF aWorldCoords)
        {           
           //Find smallest screen extent and zoom to that
            if (aWorldCoords.Height > aWorldCoords.Width)
            {
                //Use With as limiting factor
                this.iZoom = aWorldCoords.Width / iBitmap.Width;
            }
            else
                this.iZoom = aWorldCoords.Height / iBitmap.Height;

            iViewPortCenter = new PointF(aWorldCoords.X +(aWorldCoords.Width / 2.0f), aWorldCoords.Y + (aWorldCoords.Height / 2.0f));
            this.toolStripStatusLabel1.Text = "Zoom: " + ((int)(this.iZoom*100)).ToString()+"%";
            
        }

        private void SetViewPort(Rectangle aScreenCoords)
        {
        }

        private void PaintImage()
        {
            if (iBitmap != null)
            {               
                float widthZoomed = canvas.Width / iZoom;
                float heigthZoomed = canvas.Height / iZoom;

                //Do checks the reason 30,000 is used is because much over this will cause DrawImage to crash
                if (widthZoomed > 30000.0f)
                {
                    iZoom = canvas.Width / 30000.0f;
                    widthZoomed = 30000.0f;
                }                
                if (heigthZoomed > 30000.0f)
                {
                    iZoom = canvas.Height / 30000.0f;
                    heigthZoomed = 30000.0f;
                }

                //we stop at 2 because at this point you have almost zoomed into a single pixel
                if (widthZoomed < 2.0f)
                {
                    iZoom = canvas.Width / 2.0f;
                    widthZoomed = 2.0f;
                }
                if (heigthZoomed < 2.0f)
                {
                    iZoom = canvas.Height / 2.0f;
                    heigthZoomed = 2.0f;
                }

                float wz2 = widthZoomed / 2.0f;
                float hz2 = heigthZoomed / 2.0f;
                Rectangle drawRect = new Rectangle(
                    (int)(iViewPortCenter.X - wz2),
                    (int)(iViewPortCenter.Y - hz2),
                    (int)(widthZoomed), 
                    (int)(heigthZoomed));

                //this.toolStripStatusLabel1.Text = "DrawRect = " + drawRect.ToString();
                
                iBuffer.Graphics.Clear(Color.White); //Clear the Back buffer

                //Draw the image, Write image to back buffer, and render back buffer
                iBuffer.Graphics.DrawImage(iBitmap, this.canvas.DisplayRectangle, drawRect, GraphicsUnit.Pixel);
                iBuffer.Render(this.canvas.CreateGraphics());
                this.toolStripStatusLabel1.Text = 
                    string.Format("Zoom: {0}%, drawRect left {1} right {2} top {3} bottom {4}",
                                  (int)(this.iZoom * 100),
                                  drawRect.Left, drawRect.Right, drawRect.Top, drawRect.Bottom);                
            }
        }        

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                iBitmap = (Bitmap)Bitmap.FromFile(openFileDialog1.FileName);
                Setup(true);                
            }
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            Setup(false);
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            PaintImage();
        }

        private void panel1_MouseWheel(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            iZoom += iZoom * (e.Delta / 1200.0f); //the 1200.0f is any-number it just seem to work well so i use it.
            //if (e.Delta > 0) //I prefer to use the targe zoom when zooming in only, remove "if" to have it apply to zoom in and out
            {
                iViewPortCenter = 
                    new PointF(iViewPortCenter.X + ((e.X - (canvas.Width / 2)) / (2 * iZoom)), 
                               iViewPortCenter.Y + ((e.Y - (canvas.Height / 2)) / (2 * iZoom)));
            }

            this.canvas.Invalidate();            
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                iDragging = true;
            }
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (iDragging)
            {
                iViewPortCenter = new PointF(iViewPortCenter.X + ((iLastMousePosition.X - e.X)/iZoom), iViewPortCenter.Y + ((iLastMousePosition.Y- e.Y)/iZoom));                
                canvas.Invalidate();                
            }
            iLastMousePosition = e.Location;            
        }
        
        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                iDragging = false;
            }
        }

        private void moveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            iMoveStep = 0;
            iDragging = true;
            timer1.Enabled = true;
        }

        private void zoomInToolStripMenuItem_Click(object sender, EventArgs e)
        {
            iMoveStep = 0;
            iDirection = 5;
            iDragging = false;
            timer1.Enabled = true;
        }

        private void zoomOutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            iMoveStep = 0;
            iDirection = -5;
            iDragging = false;
            timer1.Enabled = true;
        }

        private void panAndZoomInToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void panAndZoomOutToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (++iMoveStep > 100)
            {
                iDragging = false;
                timer1.Enabled = false;
            }
            else if (iDragging)
            {
                //MouseEventArgs me = new MouseEventArgs(MouseButtons.None, 0,
                //                                       iLastMousePosition.X + 2,
                //                                       iLastMousePosition.Y + 2, 0);
                iViewPortCenter = 
                    new PointF(iViewPortCenter.X + (2 / iZoom), 
                               iViewPortCenter.Y + (2 / iZoom));
                canvas.Invalidate();
                iLastMousePosition = new Point(iLastMousePosition.X + 2, iLastMousePosition.Y + 2);
            }
            else
            {
                //MouseEventArgs me = new MouseEventArgs(MouseButtons.None, 0,
                //                                       (canvas.Width / 2),
                //                                       (canvas.Height / 2), iDirection);
                iZoom += iZoom * (iDirection / 1200.0f); //the 1200.0f is any-number it just seem to work well so i use it.
                iViewPortCenter = new PointF(iViewPortCenter.X / (2 * iZoom),
                                             iViewPortCenter.Y / (2 * iZoom));
                this.canvas.Invalidate();
            }
        }
    }
}