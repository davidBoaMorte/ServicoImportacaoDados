using System;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.IO;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Utilidades
{
    public class Funcoes
    {
        private static string pathTempFile = System.AppDomain.CurrentDomain.BaseDirectory + "tmpBmp.bmp";
        private static string pathApp = System.AppDomain.CurrentDomain.BaseDirectory;
        private static int _dpi = 500;
        private static Size _sizeImageRes = new Size(512, 512);
        private static System.Drawing.RotateFlipType _imgRotation = RotateFlipType.RotateNoneFlipNone;
        private static Hashtable m_knownColors = new Hashtable((int)Math.Pow(2, 20), 1.0f);
        //private static long MaxAltura = 100L;
        //private static long MaxLargura = 100L;

        public static byte[] ConverteImagemPolegarDireito(string caminhoImagem)
        {
            System.IO.FileStream fs = null;
            byte[] btImage = null;
            try
            {
                if ((caminhoImagem.Trim().Length > 0))
                {
                    if (System.IO.File.Exists(caminhoImagem))
                    {
                        fs = new System.IO.FileStream(caminhoImagem, System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.Read);
                        var oldBtImage = btImage;
                        btImage = new byte[fs.Length + 1];
                        if (oldBtImage != null)
                            Array.Copy(oldBtImage, btImage, Math.Min(fs.Length + 1, oldBtImage.Length));
                        fs.Read(btImage, 0, System.Convert.ToInt32(fs.Length));
                        fs.Close();
                    }
                }

                return btImage;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (fs != null)
                    fs.Dispose();
                fs = null;
                btImage = null;
            }
        }

        public static byte[] ConverteImagemComRotacao(string caminhoImagem)
        {
            System.IO.FileStream fs = null;
            MemoryStream fImg = null;
            byte[] btImage = null;
            EncoderParameters encParams = null/* TODO Change to default(_) if this is not a reference type */;
            Image oImgfinal = null/* TODO Change to default(_) if this is not a reference type */;
            try
            {
                // Código de Barras 2D - FOTO
                if ((caminhoImagem.Trim().Length > 0))
                {
                    if (System.IO.File.Exists(caminhoImagem))
                    {
                        fs = new System.IO.FileStream(caminhoImagem, System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.Read);
                        var oldBtImage = btImage;
                        btImage = new byte[fs.Length + 1];
                        if (oldBtImage != null)
                            Array.Copy(oldBtImage, btImage, Math.Min(fs.Length + 1, oldBtImage.Length));
                        fs.Read(btImage, 0, System.Convert.ToInt32(fs.Length));
                        fs.Close();
                        fs = null;
                        oImgfinal = ConvertByteToImage(btImage);
                        oImgfinal.RotateFlip(RotateFlipType.Rotate90FlipNone);

                        fImg = new MemoryStream();
                        oImgfinal.Save(fImg, GetCodecInfo(System.Drawing.Imaging.ImageFormat.Tiff), encParams);
                        // oImgfinal.Save(fImg, GetCodecInfo(System.Drawing.Imaging.ImageFormat.Png), encParams)

                        btImage = fImg.ToArray();
                    }
                }

                return btImage;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (fs != null)
                    fs.Dispose();

                if (oImgfinal != null)
                    oImgfinal.Dispose();

                oImgfinal = null;

                if (fImg != null)
                    fImg.Dispose();

                fImg = null;

                if (encParams != null)
                    encParams.Dispose();

                encParams = null;

                fs = null;
                btImage = null;
            }
        }

        private static ImageCodecInfo GetCodecInfo(System.Drawing.Imaging.ImageFormat format)
        {
            ImageCodecInfo[] codecs = null;
            Guid clsid = default(Guid);
            try
            {
                clsid = format.Guid;
                codecs = ImageCodecInfo.GetImageEncoders();

                foreach (var codec in codecs)
                {
                    if (clsid.Equals(codec.FormatID))
                        return codec;
                }
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                codecs = null;
                //codec = null;
                clsid = default(Guid);
            }
        }


        private static Image ConvertByteToImage(byte[] pByte)
        {
            using (MemoryStream mStream = new MemoryStream(pByte))
            {
                return Image.FromStream(mStream);
            }
        }


        public static byte[] ConverteImagemFoto(string caminhoImagem)
        {

            Bitmap oImgFinal = null;
            Bitmap oImgTemp = null;
            MemoryStream fImg = null;
            byte[] btImage = null;
            try
            {
                if ((caminhoImagem.Trim().Length > 0))
                {
                    oImgFinal = new System.Drawing.Bitmap(caminhoImagem);
                    if (System.IO.File.Exists(caminhoImagem))
                    {
                        // ABRINDO IMAGEM NO FORMATO RGB
                        oImgFinal = (Bitmap)Image.FromFile(caminhoImagem);

                        fImg = new MemoryStream();
                        oImgFinal.Save(fImg, System.Drawing.Imaging.ImageFormat.Jpeg);
                        oImgFinal.Dispose();

                        btImage = fImg.ToArray();
                    }
                    else
                        throw new Exception("Imagem não se encontra no Diretório! Lote: " + caminhoImagem);
                }

                return btImage;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (oImgFinal != null)
                    oImgFinal.Dispose();

                oImgFinal = null;

                if (oImgTemp != null)
                    oImgTemp.Dispose();

                oImgTemp = null;

                if (fImg != null)
                    fImg.Dispose();

                fImg = null;
            }
        }

        public static byte[] ConverteImagemFoto_2D(string caminhoImagem)
        {
            System.IO.FileStream fs = null;
            byte[] btImage = null;
            try
            {
                // Código de Barras 2D - FOTO
                if ((caminhoImagem.Trim().Length > 0))
                {
                    if (System.IO.File.Exists(caminhoImagem))
                    {
                        fs = new System.IO.FileStream(caminhoImagem, System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.Read);
                        var oldBtImage = btImage;
                        btImage = new byte[fs.Length + 1];
                        if (oldBtImage != null)
                            Array.Copy(oldBtImage, btImage, Math.Min(fs.Length + 1, oldBtImage.Length));
                        fs.Read(btImage, 0, System.Convert.ToInt32(fs.Length));
                        fs.Close();
                        fs = null;
                    }
                }

                return btImage;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (fs != null)
                    fs.Dispose();

                fs = null;
                btImage = null;
            }
        }

        public static byte[] ConverteImagemAssinaturaSemRotacao(string caminhoImagem)
        {
            Bitmap oImgFinal = null;
            Bitmap oImgTemp = null;
            MemoryStream fImg = null;
            EncoderParameters encParams = null;
            System.Drawing.Imaging.Encoder encCompress = null;
            byte[] btImage = null;
            try
            {
                if ((caminhoImagem.Trim().Length > 0))
                {
                    oImgFinal = new System.Drawing.Bitmap(220, 299, System.Drawing.Imaging.PixelFormat.Format24bppRgb);

                    if (System.IO.File.Exists(caminhoImagem))
                    {
                        encParams = new EncoderParameters(1);
                        encCompress = new System.Drawing.Imaging.Encoder(System.Drawing.Imaging.Encoder.Compression.Guid);
                        encParams.Param[0] = new EncoderParameter(encCompress, 0, (long)EncoderValue.CompressionCCITT4);

                        oImgFinal = (Bitmap)System.Drawing.Image.FromFile(caminhoImagem);

                        fImg = new MemoryStream();
                        //oImgFinal.Save(fImg,  GetCodecInfo(System.Drawing.Imaging.ImageFormat.Tiff), encParams);
                        oImgFinal.Save(fImg, System.Drawing.Imaging.ImageFormat.Tiff);

                        btImage = fImg.ToArray();
                    }
                }

                return btImage;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (oImgFinal != null)
                    oImgFinal.Dispose();

                oImgFinal = null;

                if (oImgTemp != null)
                    oImgTemp.Dispose();

                oImgTemp = null;

                if (fImg != null)
                    fImg.Dispose();

                fImg = null;

                if (encParams != null)
                    encParams.Dispose();

                encParams = null;
                encCompress = null;
            }
        }

        /// <summary>
        /// Funcao para conversao de imagens de Assinatura para um format legível pelo Browser
        /// </summary>
        /// <param name="Imagem"></param>
        /// <returns></returns>
        public static byte[] ConverteTiffPng(byte[] Imagem)
        {
            Bitmap bmp = null;
            MemoryStream ms = null;
            MemoryStream ms2 = null;
            byte[] imgRetorno;

            try
            {
                ms = new MemoryStream(Imagem);
                bmp = new Bitmap(ms);

                ms2 = new MemoryStream();

                bmp.Save(ms2, System.Drawing.Imaging.ImageFormat.Png);

                imgRetorno = ms2.ToArray();
                ms.Dispose();
                ms2.Dispose();

                return imgRetorno;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (bmp != null)
                {
                    bmp.Dispose();
                    bmp = null;
                }
                if (ms != null)
                {
                    ms.Dispose();
                    ms = null;
                }
                if (ms2 != null)
                {
                    ms2.Dispose();
                    ms2 = null;
                }

            }

        }

        public static string TratarAspas(string valor)
        {
            string novoValor = "''";
            try
            {
                if (valor != null)
                {
                    novoValor = "'" + valor.Replace("'", "''") + "'";
                }
                return novoValor;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static Bitmap RecortarImagem(byte[] img, int largura, int altura)
        {
            Bitmap imgOriginal;        // Image read from disk
            Bitmap imgCropped;        // Image created from original
            Rectangle rOriginal;     // Source rectangle of original
            Rectangle rCropped;     // Destination (cropped) rectangle
            Graphics g;            // Graphics surface of destination image
            MemoryStream ms = null;

            try
            {
                // Load the original image
                ms = new MemoryStream(img);

                imgOriginal = new Bitmap(ms);

                // Calculate the cropped image dimensions and create a surface
                // on which to draw it
                imgCropped = new Bitmap(largura, altura);

                g = Graphics.FromImage(imgCropped);

                rOriginal = new Rectangle(0, 0, imgCropped.Width, imgCropped.Height);

                rCropped = rOriginal;

                // Copy the subset of the original image to the cropped image
                g.DrawImage(imgOriginal, rCropped, rOriginal, GraphicsUnit.Pixel);

                return imgCropped;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static byte[] ConverteImagemParaArrayBytes(System.Drawing.Image img, System.Drawing.Imaging.ImageFormat formato)
        {
            MemoryStream ms = null;
            try
            {
                ms = new MemoryStream();
                img.Save(ms, formato);
                return ms.ToArray();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (ms != null)
                {
                    ms.Dispose();
                    ms = null;
                }
            }
        }

        public static Bitmap ConverteImagemParaBitMap(byte[] pArrImagem)
        {
            try
            {
                ImageConverter ic = new ImageConverter();
                System.Drawing.Image img = (System.Drawing.Image)ic.ConvertFrom(pArrImagem);

                Bitmap bmp = new Bitmap(img, new Size(512, 512));
                bmp.SetResolution(500, 500);

                return bmp;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static byte[] ConverteImagemParaArrayBytes(Bitmap img, System.Drawing.Imaging.ImageFormat formato)
        {
            MemoryStream ms = null;
            try
            {
                ms = new MemoryStream();
                img.SetResolution(512, 512);
                img.Save(ms, formato);
                return ms.ToArray();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (ms != null)
                {
                    ms.Dispose();
                    ms = null;
                }
            }
        }

        public static void GravarArquivo(byte[] data, string name)
        {
            FileStream f = new FileStream(name, FileMode.Create);
            BinaryWriter bin = new BinaryWriter(f);
            bin.Write(data, 0, data.Length);
            bin.Flush();
            bin.Close();
            f.Close();
        }

        public static byte[] Redimensionar(Bitmap _img, int _Altura, int _Largura)
        {
            try
            {
                _img = new Bitmap(_img, new Size(_Largura, _Altura));
                _img.SetResolution(500, 500);
                System.IO.MemoryStream ms = null;
                ms = new System.IO.MemoryStream();
                _img.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
                return ms.ToArray();
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao redimensionar a imagem. " + ex.Message);
            }
        }

        private static byte[] RedimensionarSemSetResolution(Bitmap _img, int _Altura, int _Largura)
        {
            MemoryStream ms = new MemoryStream();
            try
            {
                Bitmap bmpRedimensionado = new Bitmap(_Largura, _Altura);
                Graphics g = Graphics.FromImage(_img);
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.DrawImage(bmpRedimensionado,
                            new Rectangle(0, 0, _img.Width, _img.Height), //retangulo de origem
                            new Rectangle(0, 0, _Largura, _Altura), //retangulo de origem
                            GraphicsUnit.Pixel);
                bmpRedimensionado.Save(ms, ImageFormat.Bmp);
                return ms.ToArray();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
            finally
            {
                if (ms != null)
                {
                    ms.Dispose();
                    ms = null;
                }
            }
        }

        public byte[] BMPtoJPG(byte[] btBmp)
        {
            byte[] btJPG = null;
            MemoryStream objMem = null;
            System.Drawing.Image Img;

            try
            {
                objMem = new MemoryStream();
                Img = ConverteArrayBytesEmImagem(btBmp);
                Img.Save(objMem, System.Drawing.Imaging.ImageFormat.Jpeg);
                btJPG = objMem.ToArray();
                return btJPG;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (objMem != null)
                {
                    objMem.Dispose();
                    objMem = null;
                }
            }
        }

        public static Bitmap ConverteArrayBytesEmImagem(byte[] img)
        {
            MemoryStream ms = null;
            try
            {
                ms = new MemoryStream(img);
                Bitmap bmp = new Bitmap(ms);
                bmp.SetResolution(500, 500);
                return bmp;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (ms != null)
                {
                    ms.Dispose();
                    ms = null;
                }
            }
        }

        public static List<byte[]> ObterListaPNE(string codPNE)
        {

            var pathPNEAuditivo = AppDomain.CurrentDomain.BaseDirectory + @"Images\PNE-Auditivo.tif";
            var pathPNEFisico = AppDomain.CurrentDomain.BaseDirectory + @"Images\PNE-Fisico.tif";
            var pathPNEVisual = AppDomain.CurrentDomain.BaseDirectory + @"Images\PNE-Visual.tif";
            var pathPNEMental = AppDomain.CurrentDomain.BaseDirectory + @"Images\PNE-Mental.tif";
            List<byte[]> ListaPNE = new List<byte[]>();

            try
            {

                // trata o PNE para sempre ter 4 digitos
                codPNE = string.Concat(codPNE, "").PadLeft(4, '0');

                if (codPNE.Contains("1"))
                    ListaPNE.Add(File.ReadAllBytes(pathPNEAuditivo));
                if (codPNE.Contains("2"))
                    ListaPNE.Add(File.ReadAllBytes(pathPNEFisico));
                if (codPNE.Contains("3"))
                    ListaPNE.Add(File.ReadAllBytes(pathPNEVisual));
                if (codPNE.Contains("4"))
                    ListaPNE.Add(File.ReadAllBytes(pathPNEMental));
                if (codPNE.Contains("5"))
                    ListaPNE.Add(File.ReadAllBytes(pathPNEMental));

                return ListaPNE;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public static string BindProperty(object property, string propertyName)
        {
            string retValue = "";

            if (propertyName.Contains("."))
            {
                PropertyInfo[] arrayProperties;
                string leftPropertyName;

                leftPropertyName = propertyName.Substring(0, propertyName.IndexOf("."));
                arrayProperties = property.GetType().GetProperties();

                foreach (PropertyInfo propertyInfo in arrayProperties)
                {
                    if (propertyInfo.Name == leftPropertyName)
                    {
                        retValue = BindProperty(
                        propertyInfo.GetValue(property, null),
                        propertyName.Substring(propertyName.IndexOf(".") + 1));
                        break;
                    }
                }
            }
            else
            {
                Type propertyType;
                PropertyInfo propertyInfo;

                propertyType = property.GetType();
                propertyInfo = propertyType.GetProperty(propertyName);
                retValue = propertyInfo.GetValue(property, null).ToString();
            }

            return retValue;
        }

        public static byte[] RotacionarChancela(byte[] pImgChancela)
        {
            Image oImgFinal = null;
            MemoryStream fImg = null;
            byte[] btImage = null;
            EncoderParameters encParams = null;
            System.Drawing.Imaging.Encoder encCompress = null;
            try
            {
                if (pImgChancela != null)
                {
                    encParams = new EncoderParameters(1);
                    encCompress = new System.Drawing.Imaging.Encoder(System.Drawing.Imaging.Encoder.Compression.Guid);
                    encParams.Param[0] = new EncoderParameter(encCompress, (int)EncoderValue.CompressionCCITT4);

                    oImgFinal = ConvertByteToImage(pImgChancela);
                    oImgFinal.RotateFlip(System.Drawing.RotateFlipType.Rotate270FlipNone);

                    fImg = new MemoryStream();
                    oImgFinal.Save(fImg, GetCodecInfo(System.Drawing.Imaging.ImageFormat.Tiff), encParams);

                    btImage = fImg.ToArray();
                }

                return btImage;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (oImgFinal != null)
                    oImgFinal.Dispose();

                oImgFinal = null;

                if (fImg != null)
                    fImg.Dispose();

                fImg = null;

                if (encParams != null)
                    encParams.Dispose();

                encParams = null;
                encCompress = null;
            }
        }


        public static bool VerificarImagemCorrompida(byte[] pImg)
        {
            MemoryStream ms = null;
            Bitmap imgTeste;
            bool bolResultado = false;

            try
            {
                ms = new MemoryStream();
                ms.SetLength(pImg.Length);
                ms.Write(pImg, 0, pImg.Length);
                imgTeste = new System.Drawing.Bitmap(ms);
            }
            catch
            {
                bolResultado = true;
            }
            finally
            {
                if (ms != null)
                {
                    ms.Close();
                    ms.Dispose();
                }
            }
            return bolResultado;
        }

        public static Bitmap RAWToBMP(byte[] raw, int w, int h, PixelFormat pf)
        {
            Bitmap bmp = new Bitmap(w, h, pf);
            BitmapData data = bmp.LockBits(new Rectangle(new Point(0, 0), bmp.Size), ImageLockMode.ReadWrite, pf);
            Marshal.Copy(raw, 0, data.Scan0, raw.Length);
            bmp.UnlockBits(data);
            return bmp;
        }

        public static byte[] ConverteImagemCB_Digital1(string caminhoImagem)
        {
            System.IO.FileStream fs = null;
            byte[] btImage = null;
            try
            {
                // Código de Barras 2D - DIGITAL 1
                if ((caminhoImagem.Trim().Length > 0))
                {
                    if (System.IO.File.Exists(caminhoImagem))
                    {
                        fs = new System.IO.FileStream(caminhoImagem, System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.Read);
                        var oldBtImage = btImage;
                        btImage = new byte[fs.Length + 1];
                        if (oldBtImage != null)
                            Array.Copy(oldBtImage, btImage, Math.Min(fs.Length + 1, oldBtImage.Length));
                        fs.Read(btImage, 0, System.Convert.ToInt32(fs.Length));
                        fs.Close();
                    }
                }

                return btImage;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (fs != null)
                    fs.Dispose();
                fs = null;
                btImage = null;
            }
        }

        public static byte[] ConverteImagemCB_Digital2(string caminhoImagem)
        {
            System.IO.FileStream fs = null;
            byte[] btImage = null;
            try
            {
                // Código de Barras 2d - DIGITAL 2
                if ((caminhoImagem.Trim().Length > 0))
                {
                    if (System.IO.File.Exists(caminhoImagem))
                    {
                        fs = new System.IO.FileStream(caminhoImagem, System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.Read);
                        var oldBtImage = btImage;
                        btImage = new byte[fs.Length + 1];
                        if (oldBtImage != null)
                            Array.Copy(oldBtImage, btImage, Math.Min(fs.Length + 1, oldBtImage.Length));
                        fs.Read(btImage, 0, System.Convert.ToInt32(fs.Length));
                        fs.Close();
                    }
                }

                return btImage;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (fs != null)
                    fs.Dispose();
                fs = null;
                btImage = null;
            }
        }

        public static Bitmap ImagemDescricaoAssinaturaAnomalia(string _text, int _largura, int _altura)
        {
            Graphics g = null;
            Bitmap _bmp = null;
            try
            {
                _bmp = new Bitmap(_largura, _altura);
                g = Graphics.FromImage(_bmp);
                g.FillRectangle(Brushes.White, new Rectangle(0, 0, _largura, _altura));
                Font ft = new Font("Arial", 15, FontStyle.Regular);
                StringFormat sf = new StringFormat();
                sf.Alignment = StringAlignment.Center;
                sf.LineAlignment = StringAlignment.Center;
                Rectangle r = new Rectangle(0, 0, _bmp.Width, ft.Height * 5);
                g.DrawString(_text, ft, Brushes.Red, r, sf);
                g.DrawImage(_bmp, 0, 0);
                return _bmp;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }

        public static byte[] imageToByteArray(System.Drawing.Image imageIn)
        {
            MemoryStream ms = new MemoryStream();
            imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
            return ms.ToArray();
        }

        public static byte[] ConverteImagemAssinatura(string caminhoImagem)
        {
            Bitmap oImgFinal = null;
            Bitmap oImgTemp = null;
            MemoryStream fImg = null;
            EncoderParameters encParams = null;
            System.Drawing.Imaging.Encoder encCompress = null;
            byte[] btImage = null;
            try
            {
                if ((caminhoImagem.Trim().Length > 0))
                {
                    oImgFinal = new System.Drawing.Bitmap(220, 299, System.Drawing.Imaging.PixelFormat.Format24bppRgb);

                    if (System.IO.File.Exists(caminhoImagem))
                    {
                        encParams = new EncoderParameters(1);
                        encCompress = new System.Drawing.Imaging.Encoder(System.Drawing.Imaging.Encoder.Compression.Guid);
                        encParams.Param[0] = new EncoderParameter(encCompress, 0, (int)EncoderValue.CompressionCCITT4);

                        oImgFinal = (Bitmap)System.Drawing.Image.FromFile(caminhoImagem);

                        fImg = new MemoryStream();
                        //oImgFinal.Save(fImg,  GetCodecInfo(System.Drawing.Imaging.ImageFormat.Tiff), encParams);
                        oImgFinal.Save(fImg, System.Drawing.Imaging.ImageFormat.Tiff);


                        btImage = fImg.ToArray();
                    }

                }

                return btImage;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (oImgFinal != null)
                    oImgFinal.Dispose();

                oImgFinal = null;

                if (oImgTemp != null)
                    oImgTemp.Dispose();

                oImgTemp = null;

                if (fImg != null)
                    fImg.Dispose();

                fImg = null;

                if (encParams != null)
                    encParams.Dispose();

                encParams = null;
                encCompress = null;
            }
        }


        public static byte[] ConverteImagemAssinatura_2D(string caminhoImagem)
        {
            System.IO.FileStream fs = null;
            byte[] btImage = null;
            try
            {
                // Código de Barras 2D - ASSINATURA
                if ((caminhoImagem.Trim().Length > 0))
                {
                    if (System.IO.File.Exists(caminhoImagem))
                    {
                        fs = new System.IO.FileStream(caminhoImagem, System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.Read);
                        var oldBtImage = btImage;
                        btImage = new byte[fs.Length + 1];
                        if (oldBtImage != null)
                            Array.Copy(oldBtImage, btImage, Math.Min(fs.Length + 1, oldBtImage.Length));
                        fs.Read(btImage, 0, System.Convert.ToInt32(fs.Length));
                        fs.Close();
                        fs = null;
                    }
                }

                return btImage;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (fs != null)
                    fs.Dispose();
                fs = null;
                btImage = null;
            }
        }


        public static byte[] imageToByteArrayJpeg(System.Drawing.Image imageIn)
        {
            try
            {
                MemoryStream ms = new MemoryStream();
                imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                return ms.ToArray();
            }
            catch
            {
                throw new Exception("Erro ao Converter a Imagem");
            }
        }

        public static Bitmap BytesToBitmap(byte[] byteArray)
        {
            try
            {
                using (MemoryStream ms = new MemoryStream(byteArray))
                {
                    Bitmap img = (Bitmap)System.Drawing.Image.FromStream(ms);
                    return img;
                }
            }
            catch
            {
                throw new Exception("Erro ao converter a imagem");
            }
        }

        public static Image ArrayByteToImage(byte[] objImagem)
        {
            Image image = null;
            MemoryStream stream = null;
            Bitmap bitmap = null;
            try
            {
                stream = new MemoryStream(objImagem);
                bitmap = new Bitmap(stream);
                image = bitmap;
            }
            catch (Exception exception1)
            {
                throw exception1;
            }
            finally
            {
                bitmap = null;
                stream.Dispose();
                stream = null;
            }
            return image;
        }

        public static byte[] fncConverterImage(Image imagem, ImageFormat formato)
        {
            MemoryStream stream = null;
            byte[] buffer;
            try
            {
                stream = new MemoryStream();
                imagem.Save(stream, formato);
                buffer = stream.ToArray();
            }
            catch (Exception exception)
            {
                throw exception;
            }
            finally
            {
                if (stream != null)
                {
                    stream.Close();
                    stream.Dispose();
                }
                stream = null;
            }
            return buffer;
        }

        public static bool isNumeric(string valor)
        {
            char[] vetor;
            bool validou = true;

            try
            {
                vetor = valor.ToCharArray();
                if (vetor != null && vetor.Length > 0)
                {
                    for (int i = 0; i < vetor.Length - 1; i++)
                    {
                        if (!char.IsNumber(vetor[i]))
                        {
                            validou = false;
                        }
                    }
                }
                return validou;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static Image AjustaContrasteAssinatura(Image imgTratar, double dblContraste)
        {
            Image image = null;
            try
            {
                float num = (float)dblContraste;
                ImageAttributes imageAttr = new ImageAttributes();
                Bitmap bitmap = new Bitmap(imgTratar);
                Graphics graphics2 = Graphics.FromImage(bitmap);
                ColorMatrix newColorMatrix = new ColorMatrix(new float[][] { new float[] { num, 0f, 0f, 0f, 0f }, new float[] { 0f, num, 0f, 0f, 0f }, new float[] { 0f, 0f, num, 0f, 0f }, new float[] { 0f, 0f, 0f, 1f, 0f }, new float[] { 0f, 0f, 0f, 0f, 1f } });
                Debug.WriteLine(num);
                GraphicsUnit pixel = GraphicsUnit.Pixel;
                Rectangle destRect = Rectangle.Round(bitmap.GetBounds(ref pixel));
                int width = bitmap.Width;
                int height = bitmap.Height;
                imageAttr.SetColorMatrix(newColorMatrix);
                graphics2.DrawImage(bitmap, destRect, 0, 0, width, height, GraphicsUnit.Pixel, imageAttr);
                graphics2.Dispose();
                image = bitmap;
            }
            catch (Exception exception1)
            {
                throw exception1;
            }
            return image;
        }

        public static Image ConverteTonsdeCinza(Image img)
        {
            Image image = null;
            try
            {
                Bitmap bitmap = (Bitmap)img;
                int num3 = bitmap.Width - 1;
                int num5 = bitmap.Height - 1;
                int num7 = num5;
                for (int i = 0; i <= num7; i++)
                {
                    int num6 = num3;
                    for (int j = 0; j <= num6; j++)
                    {
                        Color pixel = bitmap.GetPixel(j, i);
                        int red = (int)Math.Round((double)(((0.3 * pixel.R) + (0.5 * pixel.G)) + (0.2 * pixel.B)));
                        bitmap.SetPixel(j, i, Color.FromArgb(0xff, red, red, red));
                    }
                }
                image = bitmap;
            }
            catch (Exception exception1)
            {
                throw exception1;
            }
            return image;
        }

        public static object GravaArrayBytesDisco(string pstrNomeArquivo, byte[] pobjArrayByte)
        {
            object obj2 = null;
            BinaryWriter writer = null;
            try
            {
                writer = new BinaryWriter(File.Open(pstrNomeArquivo, FileMode.Create));
                writer.Write(pobjArrayByte);
                writer.BaseStream.Close();
                writer.Close();
            }
            catch (Exception exception1)
            {
                throw exception1;
            }
            return obj2;
        }

        public static void GravarImagem(byte[] Imagem, string NomeArq)
        {
            try
            {
                BinaryWriter writer = new BinaryWriter(File.Open(NomeArq, FileMode.Create));
                writer.Write(Imagem);
                writer.BaseStream.Close();
                writer.Close();
            }
            catch (Exception exception1)
            {
                throw exception1;
            }
        }

        public static byte[] ImageToArrayByte(Image pbinImagem, ImageFormat pobjFormatoImagem)
        {
            byte[] buffer = null;
            MemoryStream stream = null;
            try
            {
                stream = new MemoryStream();
                pbinImagem.Save(stream, pobjFormatoImagem);
                buffer = stream.ToArray();
            }
            catch (Exception exception1)
            {
                throw exception1;
            }
            finally
            {
                stream.Close();
                stream = null;
            }
            return buffer;
        }

        public static bool VerificarLinkInternet(string url, int tempo)
        {
            bool valor = false;
            System.Net.HttpWebRequest httpReq = null;

            try
            {
                httpReq = (HttpWebRequest)WebRequest.Create(url);
                httpReq.AllowAutoRedirect = false;
                httpReq.Timeout = tempo;

                // Chamo o host
                httpReq.GetResponse();

                // Verifico se houve respota
                valor = httpReq.HaveResponse;
            }
            catch
            {
                valor = false;
            }
            finally
            {
                if (httpReq != null)
                {
                    httpReq.Abort();
                }
                httpReq = null;
            }
            return valor;
        }

        public static string RetornaDescricaoDedo(int pIdDedo)
        {
            string ret = string.Empty;

            switch (pIdDedo)
            {
                case 1:
                    ret = "POLEGAR DIREITO";
                    break;
                case 2:
                    ret = "INDICADOR DIREITO";
                    break;
                case 3:
                    ret = "MEDIO DIREITO";
                    break;
                case 4:
                    ret = "ANELAR DIREITO";
                    break;
                case 5:
                    ret = "MINIMO DIREITO";
                    break;
                case 6:
                    ret = "POLEGAR ESQUERDO";
                    break;
                case 7:
                    ret = "INDICADOR ESQUERDO";
                    break;
                case 8:
                    ret = "MEDIO ESQUERDO";
                    break;
                case 9:
                    ret = "ANELAR ESQUERDO";
                    break;
                case 10:
                    ret = "MINIMO ESQUERDO";
                    break;
                case 11:
                    ret = "POLEGAR DIREITO PLANO";
                    break;
                case 12:
                    ret = "POLEGAR ESQUERDO PLANO";
                    break;
                case 13:
                    ret = "INDICADOR MEDIO DIREITO PLANO";
                    break;
                case 14:
                    ret = "ANELAR MINIMO DIREITO PLANO";
                    break;
                case 15:
                    ret = "INDICADOR MEDIO ESQUERDO PLANO";
                    break;
                case 16:
                    ret = "ANELAR MINIMO ESQUERDO PLANO";
                    break;
            }

            return ret;
        }

        public static byte[] CropFingerPrintImage(byte[] arrBmp)
        {
            Bitmap bmp = default(Bitmap);
            System.IO.MemoryStream ms = null;
            Bitmap objNewImg = default(Bitmap);
            System.Drawing.Graphics objGraph = null;
            Rectangle rectTmp = default(Rectangle);
            byte[] resBt = null;
            Point posRecorte = default(Point);
            Point posResultado = default(Point);

            try
            {
                //Valido a Imagem
                //==========================================             
                ms = new System.IO.MemoryStream(arrBmp);

                //Vejo se é um bitmap mesmo
                try
                {
                    bmp = new Bitmap(ms);
                    //Verifico o tamanho
                    if ((bmp.Width < _sizeImageRes.Width) | (bmp.Height < _sizeImageRes.Height))
                    {
                        throw new Exception("Tamanho da imagem resultante não pode ser menor que a imagem de entrada!");
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    if ((bmp == null) == false)
                    {
                        bmp.Dispose();
                        bmp = null;
                    }
                    if ((ms == null) == false)
                    {
                        ms.Close();
                        ms = null;
                    }
                }

                GravarArqTemporario(arrBmp);

                //Carrego a imagem processada no bitmap novo e aparo as bordas se sobrar
                //=========================================
                bmp = new Bitmap(pathTempFile);
                bmp.SetResolution(_dpi, _dpi);

                objNewImg = new Bitmap(_sizeImageRes.Width, _sizeImageRes.Height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                objNewImg.SetResolution(_dpi, _dpi);
                objGraph = System.Drawing.Graphics.FromImage(objNewImg);
                objGraph.Clear(Color.White);
                objGraph.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                CalcularPosicao(bmp.Size, _sizeImageRes, ref posRecorte, ref posResultado);

                rectTmp = new Rectangle(posRecorte.X, posRecorte.Y, _sizeImageRes.Width, _sizeImageRes.Height);
                objGraph.DrawImage(bmp, posResultado.X, posResultado.Y, rectTmp, GraphicsUnit.Pixel);

                objNewImg.SetResolution(_dpi, _dpi);
                objNewImg.RotateFlip(_imgRotation);

                //resBt = ConverteImagemParaArrayBytes(ConvertTo8bppFormat(objNewImg));

                resBt = ConverteImagemParaArrayBytes(ConvertTo8bpp(objNewImg));

                return resBt;

            }
            catch (Exception ex2)
            {
                throw ex2;
            }
            finally
            {
                if ((bmp == null) == false)
                {
                    bmp.Dispose();
                    bmp = null;
                }
                if ((ms == null) == false)
                {
                    ms.Close();
                    ms = null;
                }
                if ((objNewImg == null) == false)
                {
                    objNewImg.Dispose();
                    objNewImg = null;
                }
                if ((objGraph == null) == false)
                {
                    objGraph.Dispose();
                    objGraph = null;
                }
            }
        }

        private static void GravarArqTemporario(byte[] btArquivo)
        {
            BinaryWriter bw = default(BinaryWriter);
            FileStream fs = default(FileStream);

            try
            {
                if (File.Exists(pathTempFile))
                {
                    File.Delete(pathTempFile);
                }

                fs = new FileStream(pathTempFile, System.IO.FileMode.CreateNew);
                bw = new BinaryWriter(fs);
                bw.Write(btArquivo);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if ((bw == null) == false)
                {
                    bw.Close();
                }
                if ((fs == null) == false)
                {
                    fs.Close();
                }

                bw = null;
                fs = null;
            }
        }

        private static void CalcularPosicao(Size tamOriginal, Size tamFinal, ref Point posOrigemRecorte, ref Point posFinalReccorte)
        {
            int dif = 0;

            try
            {
                posOrigemRecorte = new Point();
                posFinalReccorte = new Point();
                //Calculo o X
                if (tamOriginal.Width <= tamFinal.Width)
                {
                    posOrigemRecorte.X = 0;
                    dif = tamFinal.Width - tamOriginal.Width;
                    posFinalReccorte.X = dif / 2;
                }
                else
                {
                    dif = tamOriginal.Width - tamFinal.Width;
                    posOrigemRecorte.X = dif / 2;
                    posFinalReccorte.X = 0;
                }

                //Calculo o Y
                if (tamOriginal.Height <= tamFinal.Height)
                {
                    posOrigemRecorte.Y = 0;
                    dif = tamFinal.Height - tamOriginal.Height;
                    posFinalReccorte.Y = dif / 2;
                }
                else
                {
                    dif = tamOriginal.Height - tamFinal.Height;
                    posOrigemRecorte.Y = dif / 2;
                    posFinalReccorte.Y = 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao calcular posicionamento para cropping. " + ex.Message, ex);
            }
        }

        public static ImageCodecInfo GetEncoderInfo(string mimeType)
        {
            ImageCodecInfo[] encoders = ImageCodecInfo.GetImageEncoders();
            for (int j = 0; j <= encoders.Length - 1; j++)
            {
                if (encoders[j].MimeType == mimeType)
                    return encoders[j];
            }

            return null;
        }

        public static Bitmap ConvertTo8bpp(Bitmap bitmap)
        {
            Bitmap newBitmap = new Bitmap(bitmap.Width, bitmap.Height, PixelFormat.Format8bppIndexed);

            ColorPalette palette = newBitmap.Palette;
            for (int i = 0; i <= 255; i++)
            {
                palette.Entries[i] = Color.FromArgb(255, i, i, i);
            }
            newBitmap.Palette = palette;

            BitmapData bmpData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, bitmap.PixelFormat);
            BitmapData bmpDataNew = newBitmap.LockBits(new Rectangle(0, 0, newBitmap.Width, newBitmap.Height), ImageLockMode.WriteOnly, PixelFormat.Format8bppIndexed);

            for (int y = 0; y <= newBitmap.Height - 1; y++)
            {
                int ys = y * bmpDataNew.Stride;
                for (int x = 0; x <= newBitmap.Width - 1; x++)
                {
                    byte cor = Convert.ToByte(GetLuminicense(bmpData, x, y));
                    Marshal.WriteByte(bmpDataNew.Scan0, ys + x, cor);
                }
            }

            bitmap.UnlockBits(bmpData);
            newBitmap.UnlockBits(bmpDataNew);

            return newBitmap;
        }

        public static double GetLuminicense(BitmapData bmpData, int x, int y)
        {
            int f = x * 3;
            byte r = Marshal.ReadByte(bmpData.Scan0, (y * bmpData.Stride) + f);
            byte g = Marshal.ReadByte(bmpData.Scan0, (y * bmpData.Stride) + f + 1);
            byte b = Marshal.ReadByte(bmpData.Scan0, (y * bmpData.Stride) + f + 2);
            return (r * 0.3) + (g * 0.59) + (b * 0.11);
        }

        public static Bitmap SetBrightness(Bitmap bitmap, Single brightness)
        {
            //Brightness should be -1 (black) to 0 (neutral) to 1 (white)
            Single[][] colorMatrixVal = {
        new Single[] {1, 0, 0, 0, 0},
        new Single[] {0, 1, 0, 0, 0},
        new Single[] {0, 0, 1, 0, 0},
        new Single[] {0, 0, 0, 1, 0},
        new Single[] {brightness, brightness, brightness, 0, 1}};

            ColorMatrix colorMatrix = new ColorMatrix(colorMatrixVal);
            ImageAttributes ia = new ImageAttributes();

            ia.SetColorMatrix(colorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

            Bitmap output = new Bitmap(bitmap.Width, bitmap.Height, bitmap.PixelFormat);
            Graphics gr = Graphics.FromImage(output);
            gr.DrawImage(bitmap, new Rectangle(0, 0, bitmap.Width, bitmap.Height), 0, 0, bitmap.Width, bitmap.Height, GraphicsUnit.Pixel, ia);
            gr.Dispose();
            return output;
        }

        public static Bitmap ConvertTo8bppFormat(Bitmap bmpSource)
        {
            int imageWidth = bmpSource.Width;
            int imageHeight = bmpSource.Height;

            Bitmap bmpDest = null;
            BitmapData bmpDataDest = null;
            BitmapData bmpDataSource = null;

            try
            {
                // Create new image with 8BPP format
                bmpDest = new Bitmap(
                    imageWidth,
                    imageHeight,
                    PixelFormat.Format8bppIndexed
                    );

                // Lock bitmap in memory
                bmpDataDest = bmpDest.LockBits(
                    new Rectangle(0, 0, imageWidth, imageHeight),
                    ImageLockMode.ReadWrite,
                    bmpDest.PixelFormat
                    );

                bmpDataSource = bmpSource.LockBits(
                    new Rectangle(0, 0, imageWidth, imageHeight),
                    ImageLockMode.ReadOnly,
                    bmpSource.PixelFormat
                );

                int pixelSize = GetPixelInfoSize(bmpDataSource.PixelFormat);
                byte[] buffer = new byte[imageWidth * imageHeight * pixelSize];
                byte[] destBuffer = new byte[imageWidth * imageHeight];

                // Read all data to buffer
                ReadBmpData(bmpDataSource, buffer, pixelSize, imageWidth, imageHeight);

                // Get color indexes
                MatchColors(buffer, destBuffer, pixelSize, bmpDest.Palette);

                // Copy all colors to destination bitmaps
                WriteBmpData(bmpDataDest, destBuffer, imageWidth, imageHeight);

                return bmpDest;
            }
            finally
            {
                if (bmpDest != null) bmpDest.UnlockBits(bmpDataDest);
                if (bmpSource != null) bmpSource.UnlockBits(bmpDataSource);
            }
        }

        /// <summary>
        /// This method matches indices from pallete ( 256 colors )
        /// for each given 24bit color
        /// </summary>
        /// <param name="buffer">Buffer that contains color for each pixel</param>
        /// <param name="destBuffer">Destination buffer that will contain index 
        /// for each color</param>
        /// <param name="pixelSize">Size of pixel info ( 24bit colors supported )</param>
        /// <param name="pallete">Colors pallete ( 256 colors )</param>
        private static void MatchColors(
            byte[] buffer,
            byte[] destBuffer,
            int pixelSize,
            ColorPalette pallete)
        {
            int length = destBuffer.Length;

            // Temp storage for color info
            byte[] temp = new byte[pixelSize];

            int palleteSize = pallete.Entries.Length;

            int mult_1 = 256;
            int mult_2 = 256 * 256;

            int currentKey = 0;

            // For each color
            for (int i = 0; i < length; i++)
            {
                // Get next color
                Array.Copy(buffer, i * pixelSize, temp, 0, pixelSize);

                // Build key for hash table
                currentKey = temp[0] + temp[1] * mult_1 + temp[2] * mult_2;

                // If hash table already contains such color - fetch it
                // Otherwise perform calculation of similar color and save it to HT
                if (!m_knownColors.ContainsKey(currentKey))
                {
                    destBuffer[i] = GetSimilarColor(pallete, temp, palleteSize);
                    m_knownColors.Add(currentKey, destBuffer[i]);
                }
                else
                {
                    destBuffer[i] = (byte)m_knownColors[currentKey];
                }
            }// for
        }

        private static int GetPixelInfoSize(PixelFormat format)
        {
            switch (format)
            {
                case PixelFormat.Format24bppRgb:
                    {
                        return 3;
                    }
                default:
                    {
                        throw new ApplicationException("Only 24bit colors supported now");
                    }
            }
        }

        /// <summary>
        /// Reads all bitmap data at once
        /// </summary>
        private static void ReadBmpData(
            BitmapData bmpDataSource,
            byte[] buffer,
            int pixelSize,
            int width,
            int height)
        {
            // Get unmanaged data start address
            int addrStart = bmpDataSource.Scan0.ToInt32();

            for (int i = 0; i < height; i++)
            {
                // Get address of next row
                IntPtr realByteAddr = new IntPtr(addrStart +
                    System.Convert.ToInt32(i * bmpDataSource.Stride)
                    );

                // Perform copy from unmanaged memory
                // to managed buffer
                Marshal.Copy(
                    realByteAddr,
                    buffer,
                    (int)(i * width * pixelSize),
                    (int)(width * pixelSize)
                );
            }
        }

        /// <summary>
        /// Writes bitmap data to unmanaged memory
        /// </summary>
        private static void WriteBmpData(
            BitmapData bmpDataDest,
            byte[] destBuffer,
            int imageWidth,
            int imageHeight)
        {
            // Get unmanaged data start address
            int addrStart = bmpDataDest.Scan0.ToInt32();

            for (int i = 0; i < imageHeight; i++)
            {
                // Get address of next row
                IntPtr realByteAddr = new IntPtr(addrStart +
                    System.Convert.ToInt32(i * bmpDataDest.Stride)
                    );

                // Perform copy from managed buffer
                // to unmanaged memory
                Marshal.Copy(
                    destBuffer,
                    i * imageWidth,
                    realByteAddr,
                    imageWidth
                );
            }
        }

        /// <summary>
        /// Returns Similar color
        /// </summary>
        /// <param name="palette"></param>
        /// <param name="color"></param>
        /// <returns></returns>
        private static byte GetSimilarColor(ColorPalette palette, byte[] color, int palleteSize)
        {
            byte minDiff = byte.MaxValue;
            byte index = 0;

            if (color.Length == 3)// Implemented for 24bpp color
            {
                // Loop all pallete ( 256 colors )
                for (int i = 0; i < palleteSize - 1; i++)
                {
                    // Calculate similar color
                    byte currentDiff = GetMaxDiff(color, palette.Entries[i]);

                    if (currentDiff < minDiff)
                    {
                        minDiff = currentDiff;
                        index = (byte)i;
                    }
                }// for
            }
            else// TODO implement it for other color types
            {
                throw new ApplicationException("Only 24bit colors supported now");
            }

            return index;
        }

        /// <summary>
        /// Return similar color
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        private static byte GetMaxDiff(byte[] a, Color b)
        {
            // Get difference between components ( red green blue )
            // of given color and appropriate components of pallete color
            byte bDiff = a[0] > b.B ? (byte)(a[0] - b.B) : (byte)(b.B - a[0]);
            byte gDiff = a[1] > b.G ? (byte)(a[1] - b.G) : (byte)(b.G - a[1]);
            byte rDiff = a[2] > b.R ? (byte)(a[2] - b.R) : (byte)(b.R - a[2]);

            // Get max difference
            byte max = bDiff > gDiff ? bDiff : gDiff;
            max = max > rDiff ? max : rDiff;

            return max;
        }

        public static byte[] ConverteImagemParaArrayBytes(Bitmap img)
        {
            System.IO.MemoryStream ms = null;
            try
            {
                ms = new System.IO.MemoryStream();
                img.SetResolution(512, 512);
                img.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
                return ms.ToArray();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (ms != null)
                {
                    ms.Dispose();
                    ms = null;
                }
            }
        }

        // ==========================================================
        // Valida o numero do CPF
        // ==========================================================
        public static bool ValidaCPF(string cpf)
        {
            int[] multiplicador1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            string tempCpf;
            string digito;
            int soma;
            int resto;

            cpf = cpf.Trim();
            cpf = cpf.Replace(".", "").Replace("-", "");

            if (cpf.Length != 11)
            {
                return false;
            }

            tempCpf = cpf.Substring(0, 9);
            soma = 0;

            for (int i = 0; i < 9; i++)
            {
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];
            }

            resto = soma % 11;

            if (resto < 2)
            {
                resto = 0;
            }
            else
            {
                resto = 11 - resto;
            }

            digito = resto.ToString();
            tempCpf = tempCpf + digito;
            soma = 0;

            for (int i = 0; i < 10; i++)
            {
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];
            }

            resto = soma % 11;

            if (resto < 2)
            {
                resto = 0;
            }
            else
            {
                resto = 11 - resto;
            }

            digito = digito + resto.ToString();
            return cpf.EndsWith(digito);
        }

        public static byte[] Reduzir(byte[] img, int compactacao)
        {
            if (compactacao == 0)
            {
                return img;
            }
            else
            {
                MemoryStream ms = null;
                Bitmap bmp = null;
                MemoryStream msOut = null;
                Bitmap bmpOut = null;
                Graphics g = null;
                try
                {
                    ms = new MemoryStream(img);
                    bmp = new Bitmap(ms);
                    msOut = new MemoryStream();
                    bmpOut = new Bitmap((compactacao * bmp.Width) / bmp.Height, compactacao);
                    g = Graphics.FromImage(bmpOut);
                    g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                    g.DrawImage(bmp, 0, 0, (compactacao * bmp.Width) / bmp.Height, compactacao);
                    bmpOut.Save(msOut, System.Drawing.Imaging.ImageFormat.Bmp);
                    return msOut.ToArray();
                }
                catch (Exception ex) { throw ex; }
                finally
                {
                    ms = null;
                    bmp = null;
                    msOut = null;
                    bmpOut = null;
                    g = null;
                }
            }
        }

        public static byte[] ReduzirEComprimirJpg(byte[] img, int compactacao)
        {
            if (compactacao == 0)
            {
                return img;
            }
            else
            {
                MemoryStream ms = null;
                MemoryStream msOut = null;
                Bitmap bmp = null;
                try
                {
                    ms = new MemoryStream(Reduzir(img, compactacao));
                    msOut = new MemoryStream();
                    bmp = new Bitmap(ms);
                    bmp.Save(msOut, System.Drawing.Imaging.ImageFormat.Jpeg);
                    return msOut.ToArray();
                }
                catch (Exception ex) { throw ex; }
                finally
                {
                    ms = null;
                    msOut = null;
                    bmp = null;
                }
            }
        }

        //public static byte[] boDescomprimirJP2K(String strCaminho)
        //{
        //    FIBITMAP dib = new FIBITMAP();

        //    try
        //    {
        //        if (!FreeImage.IsAvailable())
        //        {
        //            throw new Exception("FreeImage.dll seems to be missing.");
        //        }

        //        dib = FreeImage.LoadEx(strCaminho);
        //        FreeImage.Save(FREE_IMAGE_FORMAT.FIF_BMP, dib, "ficha.bmp", FREE_IMAGE_SAVE_FLAGS.BMP_SAVE_RLE);

        //        MemoryStream ms = new MemoryStream();
        //        FreeImage.SaveToStream(dib, ms, FreeImageAPI.FREE_IMAGE_FORMAT.FIF_BMP);

        //        //aqui temos a imagem em array de bytes
        //        byte[] ficha = ms.ToArray();

        //        FreeImage.UnloadEx(ref dib);

        //        return ficha;
        //    }
        //    catch (Exception ex)
        //    {

        //        throw ex;
        //    }
        //}

        //public static Bitmap descompactarDigitalToBmp(byte[] wsq)
        //{
        //    Bitmap img = null;
        //    try
        //    {
        //        Delta.Wsq.WsqDecoder decode = new Delta.Wsq.WsqDecoder();
        //        Delta.Wsq.RawImageData obj1 = decode.Decode(wsq);
        //        img = Delta.Wsq.Conversions.ImageInfoToGdiImage(obj1);
        //        return img;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //        img = null;
        //    }
        //}

        //public static byte[] descompactarDigitaltoByteArray(byte[] wsq)
        //{
        //    Bitmap img = null;
        //    MemoryStream ms = null;
        //    try
        //    {
        //        Delta.Wsq.WsqDecoder decode = new Delta.Wsq.WsqDecoder();
        //        Delta.Wsq.RawImageData obj1 = decode.Decode(wsq);
        //        img = Delta.Wsq.Conversions.ImageInfoToGdiImage(obj1);

        //        ms = new MemoryStream();
        //        img.Save(ms, ImageFormat.Bmp);
        //        return ms.ToArray();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //        wsq = null;
        //        ms = null;
        //    }
        //}

        private static Bitmap drawImage(byte[] data, int w, int h)
        {
            Bitmap bitmap = new Bitmap(w, h);
            for (int i = 0; i < h; i++)
            {
                for (int j = 0; j < w; j++)
                {
                    int red = data[j + (i * w)];
                    Color color = Color.FromArgb(red, red, red);
                    bitmap.SetPixel(j, i, color);
                }
            }
            return bitmap;
        }


        public static int getDigitoVerificadorCorreios(string codigoRastreamento)
        {
            //O algarismo anterior ao final "BR" é o dígito verificador calculado pelo algorítmo fornecido pela ECT. Este algorítmo está descrito abaixo : 
            //a)Multiplica-se os 8 algarismos do código, da esquerda para a direita por 8, 6, 4,2,3,5,9 e 7 respectivamente. 
            //b)A soma dos produtos é dividida por 11; 
            //c)Se o resto da divisão = 0 , o dígito verificador = 5. 
            //   Se o resto da divisão = 1 , o dígito verificador = 0. 
            //   Se o resto da divisão está entre 2 e 10 , o dígito verificador = 11 - resto. 

            int sum = 0;
            int mod = 0;
            int ret = 0;

            try
            {
                //string codigoRastreamentoSemSigla = codigoRastreamento.Substring(2);
                //string codigoRastreamentoSemSigla = codigoRastreamento;
                //string codigoRastreamentoSemDigitoVerificador = codigoRastreamentoSemSigla.Substring(0, codigoRastreamentoSemSigla.Length - 1);

                //estou recebendo o codigo de rastreamento sem o DV
                string codigoRastreamentoSemDigitoVerificador = codigoRastreamento;

                for (int i = 1; i <= codigoRastreamentoSemDigitoVerificador.Length; i++)
                {
                    switch (i)
                    {
                        case 1:
                            sum += Convert.ToInt32(codigoRastreamentoSemDigitoVerificador[i - 1]) * 8;
                            break;
                        case 2:
                            sum += Convert.ToInt32(codigoRastreamentoSemDigitoVerificador[i - 1]) * 6;
                            break;
                        case 3:
                            sum += Convert.ToInt32(codigoRastreamentoSemDigitoVerificador[i - 1]) * 4;
                            break;
                        case 4:
                            sum += Convert.ToInt32(codigoRastreamentoSemDigitoVerificador[i - 1]) * 2;
                            break;
                        case 5:
                            sum += Convert.ToInt32(codigoRastreamentoSemDigitoVerificador[i - 1]) * 3;
                            break;
                        case 6:
                            sum += Convert.ToInt32(codigoRastreamentoSemDigitoVerificador[i - 1]) * 5;
                            break;
                        case 7:
                            sum += Convert.ToInt32(codigoRastreamentoSemDigitoVerificador[i - 1]) * 9;
                            break;
                        case 8:
                            sum += Convert.ToInt32(codigoRastreamentoSemDigitoVerificador[i - 1]) * 7;
                            break;
                        default:
                            break;
                    }
                }

                mod = sum % 11;
                if (mod == 0)
                {
                    ret = 5;
                }
                else if (mod == 1)
                {
                    ret = 0;
                }
                else
                {
                    ret = 11 - mod;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ret;
        }

        public static byte[] ConverteStreamToByteArray2(Stream stream)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                byte[] byteArray = new byte[16 * 1024];
                using (MemoryStream mStream = new MemoryStream())
                {
                    int bit;
                    while ((bit = stream.Read(byteArray, 0, byteArray.Length)) > 0)
                    {
                        mStream.Write(byteArray, 0, bit);
                    }
                    return mStream.ToArray();
                }
            }
        }

    }
}
