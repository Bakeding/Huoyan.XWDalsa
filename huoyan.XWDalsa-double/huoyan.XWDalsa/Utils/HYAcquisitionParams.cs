using DALSA.SaperaLT.SapClassBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DALSA.SaperaLT.SapClassBasic.SapManager;

namespace huoyan.XWDalsa.Utils
{
    class HYAcquisitionParams
    {
        public HYAcquisitionParams()
        {
            m_ServerName = "";
            m_ResourceName = "";
            m_ConfigFileName = "";
        }

        public HYAcquisitionParams(string ServerName, string ResourceName)
        {
            m_ServerName = ServerName;
            m_ResourceName = ResourceName;
            m_ConfigFileName = "";
        }

        public HYAcquisitionParams(string ServerName, string ResourceName, string ConfigFileName)
        {
            m_ServerName = ServerName;
            m_ResourceName = ResourceName;
            m_ConfigFileName = ConfigFileName;
        }

        public string ConfigFileName
        {
            get { return m_ConfigFileName; }
            set { m_ConfigFileName = value; }
        }

        public string ServerName
        {
            get { return m_ServerName; }
            set { m_ServerName = value; }
        }

        public int ServerIndex
        {
            get { return SapManager.GetServerIndex(ServerName); }
            set { ServerIndex = value; }
        }

        public string ResourceName
        {
            get { return m_ResourceName; }
            set { m_ResourceName = value; }
        }

        public int ResourceIndex
        {
            get { return SapManager.GetResourceIndex(ServerName, ResourceType, ResourceName); }
            set { m_ResourceIndex = value; }
        }
        public ResourceType ResourceType
        {
            get { return m_ResourceType; }
            set { m_ResourceType = value; }
        }

        protected string m_ServerName;
        protected int m_ServerIndex;
        protected string m_ResourceName;
        protected int m_ResourceIndex;
        protected string m_ConfigFileName;

        /// <summary>
        ///  resourceType,Resource type to inquire, can be one of the following:
        ///  ResourceType.Acq         Frame grabber acquisition hardware
        ///  ResourceType.AcqDevice   Camera acquisition hardware (for example, Genie)
        ///  ResourceType.Display     Physical displays
        ///  ResourceType.Gio         General inputs and outputs
        ///  ResourceType.Graphic     Graphics engine
        /// </summary>
        protected ResourceType m_ResourceType;

    }
}
