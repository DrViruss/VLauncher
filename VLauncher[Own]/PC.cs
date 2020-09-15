using System;
using System.Management;

namespace VLauncher_Own_
{
    internal class PC
    {
        public double getRAM()
        {
            System.Management.ObjectQuery wql = new System.Management.ObjectQuery("SELECT * FROM Win32_OperatingSystem");
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(wql);
            ManagementObjectCollection results = searcher.Get();
            double fres = 0;
            foreach (ManagementObject result in results)
            {
                fres = Math.Round((Convert.ToDouble(result["TotalVisibleMemorySize"]) / (1024 * 1024)), 2);//WTF??
            }
            return fres;
        }

        public byte getOSType()
        {
            if (System.Environment.Is64BitOperatingSystem)
                return 64;
            else
                return 32;
        }

        public string getUsername()
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.UserProfile).Split('\\')[2];
        }

        public string getAppData()
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        }


    }
}
