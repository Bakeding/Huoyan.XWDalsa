using DALSA.SaperaLT.SapClassBasic;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace huoyan.XWDalsa.Utils
{
    class HYAcquisitionSettingUtils
    {
        // Static Variables
        const int GAMMA_FACTOR = 10000;
        const int MAX_CONFIG_FILES = 36;       // 10 numbers + 26 letters

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serverNameList">server名字列表</param>
        /// <param name="msg">状态消息</param>
        /// <returns>
        /// 1:找到SapManager.ResourceType.Acq
        /// 2:找到SapManager.ResourceType.AcqDevice
        /// </returns>
        static public int GetServers(IList<string> serverNameList, out string msg)
        {
            msg = "";
            // Get total number of boards in the system
            int serverCount = SapManager.GetServerCount();
            int serverType = 0;
            if (serverCount == 0)
            {
                msg = "No device found!";
                return -1;
            }

            // Scan the boards to find those that support acquisition
            int serverIndex;
            for (serverIndex = 0; serverIndex < serverCount; serverIndex++)
            {
                if (SapManager.GetResourceCount(serverIndex, SapManager.ResourceType.Acq) != 0)
                {
                    string serverName = SapManager.GetServerName(serverIndex);
                    serverNameList.Add(serverName);
                    //Console.WriteLine(serverIndex.ToString() + ": " + serverName);
                    serverType = 1;
                }
                else if (SapManager.GetResourceCount(serverIndex, SapManager.ResourceType.AcqDevice) != 0)
                {
                    string serverName = SapManager.GetServerName(serverIndex);
                    serverNameList.Add(serverName);
                    //Console.WriteLine(serverIndex.ToString() + ": " + serverName);
                    serverType = 2;
                }
            }

            // At least one acquisition server must be available
            if (serverType<=0)
            {
                msg = "No acquisition server found!";
                return -1;
            }
            return serverType;
        }

        static public bool GetDevices(string serverName, IList<string> deviceNameList, out string msg)
        {
            msg = "";
            // Scan all the acquisition devices on that server and show menu to user
            int deviceCount = SapManager.GetResourceCount(serverName, SapManager.ResourceType.Acq);
            int cameraCount = (deviceCount == 0) ? SapManager.GetResourceCount(serverName, SapManager.ResourceType.AcqDevice) : 0;
            int allDeviceCount = deviceCount + cameraCount;

            for (int deviceIndex = 0; deviceIndex < deviceCount; deviceIndex++)
            {
                string deviceName = SapManager.GetResourceName(serverName, SapManager.ResourceType.Acq, deviceIndex);
                deviceNameList.Add(deviceName);
            }

            if (deviceCount == 0)
            {
                for (int cameraIndex = 0; cameraIndex < cameraCount; cameraIndex++)
                {
                    string cameraName = SapManager.GetResourceName(serverName, SapManager.ResourceType.AcqDevice, cameraIndex);
                    Console.WriteLine(((int)(cameraIndex + 1)).ToString() + ": " + cameraName);
                }
            }

            if (deviceNameList.Count>0)
            {
                return true;
            }
            return false;
        }

        static public bool GetConfigFilesInDirectry(string configPath, IList<string> configFileList, out string msg)// List all files in the config directory
        {
            msg = "";
            //string configPath = Environment.GetEnvironmentVariable("SAPERADIR") + "\\CamFiles\\User\\";
            if (!Directory.Exists(configPath))
            {
                msg = "Directory : "+ configPath+" Does not exist" ;
                return false;
            }

            string[] ccffiles = Directory.GetFiles(configPath, "*.ccf");
            int configFileCount = ccffiles.Length;
            string[] configFileNames = new string[configFileCount + 1];

            if (configFileCount == 0)
            {
                msg = "No config File.";
            }
            else
            {
                foreach (string ccfFileName in ccffiles)
                {
                    configFileList.Add(ccfFileName.Replace(configPath, "").Replace("\\",""));
                }

            }
            if (configFileList.Count>0)
            {
                return true;
            }

            return false;
        }

    }
}
