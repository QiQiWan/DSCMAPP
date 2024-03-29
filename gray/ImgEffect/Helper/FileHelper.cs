﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;

namespace Gray
{
    /// <summary>
    /// 文件帮助类，负责读写创建文件
    /// </summary>
    public class FileHelper
    {
        private static FileStream fileStream;
        private static StreamReader streamReader;
        private static StreamWriter streamWriter;
        /// <summary>
        /// 读取指定文本文件
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static string ReadFile(string filePath)
        {
            string fullText = "";
            if (!File.Exists(filePath))
                CreatFile(filePath);
            using (fileStream = new FileStream(filePath, FileMode.Open))
            {
                streamReader = new StreamReader(fileStream);
                fullText = streamReader.ReadToEnd();
                streamReader.Close();
                fileStream.Close();
            }
            return fullText;
        }
        public static string[] ReadFileLine(string filePath)
        {
            List<string> fileLines = new List<string>();
            using (fileStream = new FileStream(filePath, FileMode.Open))
            {
                streamReader = new StreamReader(fileStream);
                string line;
                while ((line = streamReader.ReadLine()) != null)
                {
                    fileLines.Add(line);
                }
                streamReader.Close();
                fileStream.Close();
            }
            return fileLines.ToArray();
        }
        /// <summary>
        /// 将文件读取为二进制数组
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static byte[] ReadBuffer(string filePath)
        {
            byte[] buffer;
            using (fileStream = new FileStream(filePath, FileMode.Open))
            {
                buffer = new byte[fileStream.Length];
                fileStream.Read(buffer, 0, (int)fileStream.Length);
                fileStream.Close();
            }
            return buffer;
        }
        /// <summary>
        /// 将指定内容按照指定方式写入指定文件
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="content"></param>
        /// <param name="mode"></param>
        public static void WriteFile(string filePath, string content, WriteMode mode)
        {
            if (!File.Exists(filePath))
                CreatFile(filePath);
            lock (Common.Lock)
            {
                using (fileStream = new FileStream(filePath, FileMode.Open))
                {
                    //文件指针定位到文件尾部
                    if (mode == WriteMode.Append)
                        fileStream.Position = fileStream.Length;
                    streamWriter = new StreamWriter(fileStream);
                    streamWriter.WriteLine(content);
                    streamWriter.Close();
                    fileStream.Close();
                }
            }
        }
        public static void WriteFile(string filePath, string[] contents, WriteMode mode)
        {
            if (!File.Exists(filePath))
                CreatFile(filePath);
            if (CheckOccupy(filePath))
                throw new Exception("文件被占用!");
            lock (Common.Lock)
            {
                using (fileStream = new FileStream(filePath, FileMode.Open))
                {
                    //文件指针定位到文件尾部
                    if (mode == WriteMode.Append)
                        fileStream.Position = fileStream.Length;
                    streamWriter = new StreamWriter(fileStream);
                    for (int i = 0; i < contents.Length; i++)
                    {
                        streamWriter.WriteLine(contents[i]);
                    }
                    streamWriter.Close();
                    fileStream.Close();
                }
            }
        }
        /// <summary>
        /// 判断文件是否存在
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static bool FileExists(string filePath) => File.Exists(filePath);
        /// <summary>
        /// 创建指定文件
        /// </summary>
        /// <param name="filePath"></param>
        public static void CreatFile(string filePath)
        {
            using (fileStream = new FileStream(filePath, FileMode.Create))
            {
                streamWriter = new StreamWriter(fileStream);
                streamWriter.Write("");
                streamWriter.Close();
                fileStream.Close();
            }
        }

        [DllImport("kernel32.dll")]
        public static extern IntPtr _lopen(string lpPathName, int iReadWrite);

        [DllImport("kernel32.dll")]
        public static extern bool CloseHandle(IntPtr hObject);
        public static int OF_READWRITE = 2;
        public static int OF_SHARE_DENY_NONE = 0x40;
        public static bool CheckOccupy(string fileName)
        {
            IntPtr HFILE_ERROR = new IntPtr(-1);
            IntPtr vHandle = _lopen(fileName, OF_READWRITE | OF_SHARE_DENY_NONE);
            if (vHandle == HFILE_ERROR)
            {
                return true;
            }
            CloseHandle(vHandle);
            return false;
        }
    }
    public enum WriteMode { Append, WriteAll };
}
