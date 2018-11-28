﻿using System;

// ReSharper disable InconsistentNaming

namespace OpenCvSharp
{
    /// <summary>
    /// 
    /// </summary>
    public class QRCodeDetector : DisposableCvObject
    {
        #region Init and Disposal

        /// <summary>
        /// 
        /// </summary>
        public QRCodeDetector()
        {
            ptr = NativeMethods.objdetect_QRCodeDetector_new();               
        }
        
        /// <summary>
        /// Releases unmanaged resources
        /// </summary>
        protected override void DisposeUnmanaged()
        {
            NativeMethods.objdetect_QRCodeDetector_delete(ptr);
            base.DisposeUnmanaged();
        }

        #endregion

        #region Methods

        /// <summary>
        /// sets the epsilon used during the horizontal scan of QR code stop marker detection.
        /// </summary>
        /// <param name="epsX">Epsilon neighborhood, which allows you to determine the horizontal pattern 
        /// of the scheme 1:1:3:1:1 according to QR code standard.</param>
        public void SetEpsX(double epsX)
        {
            NativeMethods.objdetect_QRCodeDetector_setEpsX(ptr, epsX);
            GC.KeepAlive(this);
        }

        /// <summary>
        /// sets the epsilon used during the vertical scan of QR code stop marker detection.
        /// </summary>
        /// <param name="epsY">Epsilon neighborhood, which allows you to determine the vertical pattern
        /// of the scheme 1:1:3:1:1 according to QR code standard.</param>
        public void SetEpsY(double epsY)
        {
            NativeMethods.objdetect_QRCodeDetector_setEpsY(ptr, epsY);
            GC.KeepAlive(this);
        }

        /// <summary>
        /// Detects QR code in image and returns the quadrangle containing the code.
        /// </summary>
        /// <param name="img">grayscale or color (BGR) image containing (or not) QR code.</param>
        /// <param name="points">Output vector of vertices of the minimum-area quadrangle containing the code.</param>
        /// <returns></returns>
        public bool Detect(InputArray img, OutputArray points)
        {
            if (img == null)
                throw new ArgumentNullException(nameof(img));
            if (points == null)
                throw new ArgumentNullException(nameof(points));
            img.ThrowIfDisposed();
            points.ThrowIfNotReady();

            int result = NativeMethods.objdetect_QRCodeDetector_detect(ptr, img.CvPtr, points.CvPtr);

            GC.KeepAlive(img);
            GC.KeepAlive(points);
            GC.KeepAlive(this);

            return result != 0;
        }

        /// <summary>
        /// Detects QR code in image and returns the quadrangle containing the code.
        /// </summary>
        /// <param name="img">grayscale or color (BGR) image containing (or not) QR code.</param>
        /// <param name="points">Output vector of vertices of the minimum-area quadrangle containing the code.</param>
        /// <returns></returns>
        public bool Detect(InputArray img, out Point2f[] points)
        {
            bool result;
            using (var dst = new Mat())
            {
                result = Detect(img, dst);
                points = new[] {
                    dst.At<Point2f>(0),
                    dst.At<Point2f>(1),
                    dst.At<Point2f>(2),
                    dst.At<Point2f>(3)
                };
            }
            GC.KeepAlive(this);
            return result;
        }

        /// <summary>
        /// Decodes QR code in image once it's found by the detect() method.
        /// Returns UTF8-encoded output string or empty string if the code cannot be decoded.
        /// </summary>
        /// <param name="img">grayscale or color (BGR) image containing QR code.</param>
        /// <param name="points">Quadrangle vertices found by detect() method (or some other algorithm).</param>
        /// <param name="straightQrcode">The optional output image containing rectified and binarized QR code</param>
        /// <returns></returns>
        public string Decode(InputArray img, InputArray points, OutputArray straightQrcode = null)
        {
            if (img == null)
                throw new ArgumentNullException(nameof(img));
            if (points == null)
                throw new ArgumentNullException(nameof(points));
            img.ThrowIfDisposed();
            points.ThrowIfDisposed();
            straightQrcode?.ThrowIfNotReady();

            string result;
            using (var resultString = new StdString())
            {
                NativeMethods.objdetect_QRCodeDetector_decode(
                    ptr, img.CvPtr, points.CvPtr, Cv2.ToPtr(straightQrcode), resultString.CvPtr);
                result = resultString.ToString();
            }

            GC.KeepAlive(img);
            GC.KeepAlive(points);
            GC.KeepAlive(straightQrcode);
            GC.KeepAlive(this);

            return result;
        }

        /// <summary>
        /// Both detects and decodes QR code
        /// </summary>
        /// <param name="img">grayscale or color (BGR) image containing QR code.</param>
        /// <param name="points">opiotnal output array of vertices of the found QR code quadrangle. Will be empty if not found.</param>
        /// <param name="straightQrcode">The optional output image containing rectified and binarized QR code</param>
        /// <returns></returns>
        public string DetectAndDecode(InputArray img, OutputArray points = null, OutputArray straightQrcode = null)
        {
            if (img == null)
                throw new ArgumentNullException(nameof(img));
            img.ThrowIfDisposed();
            points?.ThrowIfNotReady();
            straightQrcode?.ThrowIfNotReady();

            string result;
            using (var resultString = new StdString())
            {
                NativeMethods.objdetect_QRCodeDetector_detectAndDecode(
                    ptr, img.CvPtr, Cv2.ToPtr(points), Cv2.ToPtr(straightQrcode), resultString.CvPtr);
                result = resultString.ToString();
            }

            GC.KeepAlive(img);
            GC.KeepAlive(points);
            GC.KeepAlive(straightQrcode);
            GC.KeepAlive(this);

            return result;
        }

        #endregion
    }
}