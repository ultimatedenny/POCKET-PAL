using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace POCKETPAL_API.Class
{
    public class ClassCreateIssuLog
    {
        public readonly Random random = new Random();
        public void CreateLog(string UseId, string DeviceName, string Model, string Manufacture, string OS, string msg)
        {
            int num = random.Next(1000);
            string id = Convert.ToString(num);
            string fileName = @"C:\Log\PPLOG-" + DateTime.Now.ToString("dd-MMM-yyyy HH mm ss tt") + "(" + id + ").txt";
            try
            {
                if (File.Exists(fileName))
                {
                    File.Delete(fileName);
                }
                using (StreamWriter sw = File.CreateText(fileName))
                {
                    sw.WriteLine("POCKET PAL Log: {0}", DateTime.Now.ToString());
                    sw.WriteLine("\n");
                    sw.WriteLine("Created by: " + UseId + "");
                    sw.WriteLine("Device Name: " + DeviceName + "");
                    sw.WriteLine("Device Model: " + Model + "");
                    sw.WriteLine("Device Manufacture: " + Manufacture + "");
                    sw.WriteLine("Device OS: " + OS + "");
                    sw.WriteLine("\n");
                    sw.WriteLine("Log:");
                    sw.WriteLine("============================================");
                    sw.WriteLine(msg);
                    sw.WriteLine("============================================");
                    sw.WriteLine("Please check on API Side");
                }
                using (StreamReader sr = File.OpenText(fileName))
                {
                    string s = "";
                    while ((s = sr.ReadLine()) != null)
                    {
                        Console.WriteLine(s);
                    }
                }
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.ToString());
            }
        }
    }
}