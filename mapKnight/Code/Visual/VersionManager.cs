using System;
using System.Net;
using System.IO;

namespace mapKnight
{
    public enum VersionState
    {
        mainVersion,
        subVersion,
        Build,
        Debug,
    }

    public class Version
    {
        private int mainVersion, subVersion, Build, Debug;

        public Version(string VersionString)
        {
            string [] splittedVersion = VersionString.Split(new char [] {'.'});

            mainVersion = Convert.ToInt32(splittedVersion[0]);
            subVersion = Convert.ToInt32(splittedVersion[1]);
            Build = Convert.ToInt32(splittedVersion[2]);
            Debug = Convert.ToInt32(splittedVersion[3]);
        }

        public int getVersion(VersionState partOfVersion)
        {
            switch (partOfVersion)
            {
                case VersionState.Build:
                    return Build;
                case VersionState.Debug:
                    return Debug;
                case VersionState.mainVersion:
                    return mainVersion;
                case VersionState.subVersion:
                    return subVersion;
                default:
                    return -1;
            }
        }

        public override string ToString()
        {
            return String.Format("{0}.{1}.{2}.{3}", mainVersion.ToString(), subVersion.ToString(), Build.ToString(), Debug.ToString());
        }

        public void Count(VersionState Version)
        {
            switch (Version)
            {
                case VersionState.Build:
                    Build += 1;
                    break;
                case VersionState.Debug:
                    Debug+= 1;
                    break;
                case VersionState.mainVersion:
                    mainVersion+= 1;
                    break;
                case VersionState.subVersion:
                    subVersion+= 1;
                    break;
                default:
                    //nothing
                    break;
            }
        }
    }

    public class VersionManager
    {
        
        private string projectName;
        private WebClient Connector;

        //private url = http://dreamlo.com/lb/zkbZN5VE7kGDInRa04uxkQsCevf2n9zU6TY-ycn_WZyQ
        //settings
        //currently disbabled
        private string privateCode = "zkbZN5VE7kGDInRa04uxkQsCevf2n9zU6TY-ycn_WZyQ";
        private string publicCode = "5569a7356e51b61cac478064";
        private string providerDomain = "http://dreamlo.com/lb/";

        private string versionSaveFilePath = System.Reflection.Assembly.GetExecutingAssembly().CodeBase + "/Content/version.txt";

        public VersionManager()
        {
            StreamReader versiontxt = new StreamReader(versionSaveFilePath);
            projectVersion = new Version(versiontxt.ReadLine());
            versiontxt.Close();
        }

        private void removeEntry()
        {
            Connector.OpenRead(providerDomain + privateCode + "/delete/"+ projectName);
        }

        private void setEntry()
        {
            Connector.OpenRead(string.Format("{0}{1}/add/{2}/0/0/{3}",providerDomain , privateCode, projectName, projectVersion.ToString()));
        }

        private void updateEntry()
        {
            removeEntry();
            setEntry();
        }

        public VersionManager(string currentProjectName)
        {
            projectName = currentProjectName;
            Connector = new WebClient();
        }

        // mainVersion.subVersion.Build.Debug 
        public bool Count(VersionState Version)
        {
            projectVersion.Count(Version);
            return true;
        }

        public Version projectVersion
        { 
            get ;
            set;
        }
    }
}