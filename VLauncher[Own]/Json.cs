﻿using System.Collections.Generic;
using System.IO;

namespace VLauncher_Own_
{
    internal class Auth
    {
        public string accessToken { get; set; }
        public string clientToken { get; set; }
        public List<AvailableProfile> availableProfiles { get; set; }
        public SelectedProfile selectedProfile { get; set; }
    }

    public class AvailableProfile
    {
        public string name { get; set; }
        public string id { get; set; }
    }

    public class SelectedProfile
    {
        public string name { get; set; }
        public string id { get; set; }
    }

    public class jsonHash
    {
        public FileInfo file { get; set; }
        public string hash { get; set; }
    
    }

}