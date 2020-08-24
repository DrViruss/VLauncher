using System;
using System.Diagnostics;
using System.Windows.Forms;
using System.Management;
using System.IO;
using System.Text.Json;
using System.Net;
using System.Xml.Linq;

namespace VLauncher_Own_
{
    public partial class lform : Form
    {
        public lform()
        {
            InitializeComponent();

            //First-time folder set
            gamedir_tb.Text = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\.minecraft";
           
            //First-time RAM set
            System.Management.ObjectQuery wql = new System.Management.ObjectQuery("SELECT * FROM Win32_OperatingSystem");
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(wql);
            ManagementObjectCollection results = searcher.Get();
            double fres =0;
            foreach (ManagementObject result in results)
            {
                fres = Math.Round((Convert.ToDouble(result["TotalVisibleMemorySize"]) / (1024 * 1024)), 2);//WTF??
            }
            ram_track.Maximum = (Int32)fres*1024-512;
            ram_track.Value = ram_track.Maximum / 2;
            ram_label.Text = ram_track.Value.ToString();

            //First-time Username set
            username_tb.Text = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile).Split('\\')[2];

            //Check Java on start
            if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable("JAVA_HOME")))
            {
                myJava.Checked = true;
                myJava.Enabled = false;
            }

            //use non-legal version by default
            Login_tb.Enabled = false; 
            Passwd_tb.Enabled = false;

        }

        //-------------------TMP-Vars-------------------------\\

        string ModPack_Name = "ModPack";

        //-------------------Functions-------------------------\\

        string NeedDebug()
        {
            string jvmpath;
            if (Debug.Checked)
            {
                jvmpath = getJavaPath() +"\\java.exe";
            }
            else
            {
                jvmpath = getJavaPath()+"\\javaw.exe";
            }
            return jvmpath;
        }

        string getJavaPath()
        {
            if(!myJava.Checked)
                return Environment.GetEnvironmentVariable("JAVA_HOME")+"\\bin";
            else
            {
                //TODO: Download JAVA
                return gamedir_tb.Text + "\\Java\\bin";
            }
        }

        string[] jvmAccount()
        {
            if (hasAccount.Checked)
            {
                Auth json = new Auth();

            nowexist:
                if (File.Exists(gamedir_tb.Text + "\\mojang_profile.json"))
                    if (login())
                    {
                        json = JsonSerializer.Deserialize<Auth>(File.ReadAllText(gamedir_tb.Text + "\\mojang_profile.json"));
                        username_tb.Text = json.selectedProfile.name;
                        return new string[] { json.selectedProfile.name, json.accessToken, json.selectedProfile.id, "mojang" };
                    }
                    else if (Auth())
                        goto nowexist;  //if file exist - try login | if true - go to file check+login | if cant - go to exit
                    else
                        return null;

                else if (Auth())
                    goto nowexist; //if file NOT exist - auth and gen it | if true - go to file check+login | if cant - go to exit
                else
                    return null;
            }
            else 
            {return new string[] { username_tb.Text, "null", "00000000-0000-0000-0000-000000000000","legacy" }; }
        }

        bool Auth()
        {
            //Get accesToken and save in file
            HttpWebRequest rq = (HttpWebRequest)WebRequest.Create("https://authserver.mojang.com/authenticate");
            rq.ContentType = "application/json";
            rq.Method = "POST";
            rq.KeepAlive = false;
            try
            {
                using (StreamWriter sw = new StreamWriter(rq.GetRequestStream()))
                {
                    //Dont look here :#
                    string Sjson = "{" + '"' + "agent" + '"' + ':' + '{' + '"' + "name" + '"' + ':' + '"' + "Minecraft" + '"' + ',' + '"' + "version" + '"' + ':' + '1' + "}," + '"' + "username" + '"' + ':' + '"' + Login_tb.Text + '"' + ',' + '"' + "password" + '"' + ':' + '"' + Passwd_tb.Text + '"' + '}';

                    sw.Write(Sjson);
                    sw.Close();
                }
                HttpWebResponse response = (HttpWebResponse)rq.GetResponse();
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    Auth json = JsonSerializer.Deserialize<Auth>(reader.ReadToEnd());
                    validtoken(json);
                    string jsonString = JsonSerializer.Serialize(json);
                    File.WriteAllText(gamedir_tb.Text+ "\\mojang_profile.json", jsonString);
                    reader.Close();
                    response.Close();
                    return true;
                }
            }
            catch (System.Net.WebException ex)
            {
                if (ex.Message == "The remote server returned an error: (403) Forbidden.")
                    MessageBox.Show("Incorrect email or password");
                else if (ex.Message == "The remote name could not be resolved: 'authserver.mojang.com'")
                    MessageBox.Show("Check your internet connection");
                else
                    MessageBox.Show("Unknown ERROR: " +'\n'+'"' + ex.Message +'"'+"\n Send it to developer");

                return false;
            }
        }
        bool login()
        {
            HttpWebRequest rq = (HttpWebRequest)WebRequest.Create("https://authserver.mojang.com/refresh");
            rq.ContentType = "application/json";
            rq.Method = "POST";
            rq.KeepAlive = false;
            try
            {
                using (StreamWriter sw = new StreamWriter(rq.GetRequestStream()))
                {
                    Auth auth = JsonSerializer.Deserialize<Auth>(File.ReadAllText(gamedir_tb.Text + "\\mojang_profile.json"));
                    validtoken(auth);
                    string _tmp = '{'+ '"' + "accessToken"+'"'+':'+ '"'+auth.accessToken+'"'+','+'"'+"clientToken"+'"'+':'+ '"'+auth.clientToken+'"'+'}';
                    sw.Write(_tmp);
                    sw.Close();
                }
                HttpWebResponse response = (HttpWebResponse)rq.GetResponse();
            }
            catch (System.Net.WebException ex)
            {
                if (ex.Message == "The remote server returned an error: (403) Forbidden.")
                {
                    MessageBox.Show("Token is dead \nPlease input email and password again");
                    return false;
                }
                else if (ex.Message == "The remote server returned an error: (400) Bad Request.")
                {
                    return true;
                }
                else
                {
                    MessageBox.Show("Unknown ERROR: \n"+'"'+ex.Message+'"'+"\nShow this message to developer");
                }
            }
            return false;
        }
        void validtoken(Auth json)
        {
            if (json.clientToken[8] != '-')
            {
                char[] tmp = new char[json.clientToken.Length - 1];
                tmp = json.clientToken.ToCharArray();
                json.clientToken = "";
                for (int i = 0; i < 7; i++)
                    json.clientToken += tmp[i];
                json.clientToken += '-';
                for (int i = 6; i < 11; i++)
                    json.clientToken += tmp[i];
                json.clientToken += '-';
                for (int i = 11; i < 15; i++)
                    json.clientToken += tmp[i];
                json.clientToken += '-';
                for (int i = 15; i < 19; i++)
                    json.clientToken += tmp[i];
                json.clientToken += '-';
                for (int i = 19; i < tmp.Length - 1; i++)
                    json.clientToken += tmp[i];
            }
        }


        //-------------------Main-------------------------\\
        void launch_mc()
        {
            //TODO: Make using legal account more user-friendly :D
            string[] userinfo = jvmAccount();
            if (!(userinfo == null))
            {
                ProcessStartInfo startInfo = new ProcessStartInfo(NeedDebug());
                string Appdatapath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                //OMG
                //TODO: test another "--versionType" :D
                startInfo.Arguments = "-Djava.net.preferIPv4Stack=true -Xmn128M -Xmx" + ram_label.Text + "M -Djava.library.path=" + gamedir_tb.Text + "\\" + ModPack_Name + "\\versions\\1.12.2-forge-14.23.5.2854\\natives -cp " + gamedir_tb.Text + "\\" + ModPack_Name + "\\libraries\\net\\minecraftforge\\forge\\1.12.2-14.23.5.2854\\forge-1.12.2-14.23.5.2854.jar;" + gamedir_tb.Text + "\\" + ModPack_Name + "\\libraries\\org\\ow2\\asm\\asm-debug-all\\5.2\\asm-debug-all-5.2.jar;" + gamedir_tb.Text + "\\" + ModPack_Name + "\\libraries\\net\\minecraft\\launchwrapper\\1.12\\launchwrapper-1.12.jar;" + gamedir_tb.Text + "\\" + ModPack_Name + "\\libraries\\org\\jline\\jline\\3.5.1\\jline-3.5.1.jar;" + gamedir_tb.Text + "\\" + ModPack_Name + "\\libraries\\com\\typesafe\\akka\\akka-actor_2.11\\2.3.3\\akka-actor_2.11-2.3.3.jar;" + gamedir_tb.Text + "\\" + ModPack_Name + "\\libraries\\com\\typesafe\\config\\1.2.1\\config-1.2.1.jar;" + gamedir_tb.Text + "\\" + ModPack_Name + "\\libraries\\org\\scala-lang\\scala-actors-migration_2.11\\1.1.0\\scala-actors-migration_2.11-1.1.0.jar;" + gamedir_tb.Text + "\\" + ModPack_Name + "\\libraries\\org\\scala-lang\\scala-compiler\\2.11.1\\scala-compiler-2.11.1.jar;" + gamedir_tb.Text + "\\" + ModPack_Name + "\\libraries\\org\\scala-lang\\plugins\\scala-continuations-library_2.11\\1.0.2_mc\\scala-continuations-library_2.11-1.0.2_mc.jar;" + gamedir_tb.Text + "\\" + ModPack_Name + "\\libraries\\org\\scala-lang\\plugins\\scala-continuations-plugin_2.11.1\\1.0.2_mc\\scala-continuations-plugin_2.11.1-1.0.2_mc.jar;" + gamedir_tb.Text + "\\" + ModPack_Name + "\\libraries\\org\\scala-lang\\scala-library\\2.11.1\\scala-library-2.11.1.jar;" + gamedir_tb.Text + "\\" + ModPack_Name + "\\libraries\\org\\scala-lang\\scala-parser-combinators_2.11\\1.0.1\\scala-parser-combinators_2.11-1.0.1.jar;" + gamedir_tb.Text + "\\" + ModPack_Name + "\\libraries\\org\\scala-lang\\scala-reflect\\2.11.1\\scala-reflect-2.11.1.jar;" + gamedir_tb.Text + "\\" + ModPack_Name + "\\libraries\\org\\scala-lang\\scala-swing_2.11\\1.0.1\\scala-swing_2.11-1.0.1.jar;" + gamedir_tb.Text + "\\" + ModPack_Name + "\\libraries\\org\\scala-lang\\scala-xml_2.11\\1.0.2\\scala-xml_2.11-1.0.2.jar;" + gamedir_tb.Text + "\\" + ModPack_Name + "\\libraries\\lzma\\lzma\\0.0.1\\lzma-0.0.1.jar;" + gamedir_tb.Text + "\\" + ModPack_Name + "\\libraries\\java3d\\vecmath\\1.5.2\\vecmath-1.5.2.jar;" + gamedir_tb.Text + "\\" + ModPack_Name + "\\libraries\\net\\sf\\trove4j\\trove4j\\3.0.3\\trove4j-3.0.3.jar;" + gamedir_tb.Text + "\\" + ModPack_Name + "\\libraries\\org\\apache\\maven\\maven-artifact\\3.5.3\\maven-artifact-3.5.3.jar;" + gamedir_tb.Text + "\\" + ModPack_Name + "\\libraries\\net\\sf\\jopt-simple\\jopt-simple\\5.0.3\\jopt-simple-5.0.3.jar;" + gamedir_tb.Text + "\\" + ModPack_Name + "\\libraries\\org\\tlauncher\\patchy\\1.1\\patchy-1.1.jar;" + gamedir_tb.Text + "\\" + ModPack_Name + "\\libraries\\oshi-project\\oshi-core\\1.1\\oshi-core-1.1.jar;" + gamedir_tb.Text + "\\" + ModPack_Name + "\\libraries\\net\\java\\dev\\jna\\jna\\4.4.0\\jna-4.4.0.jar;" + gamedir_tb.Text + "\\" + ModPack_Name + "\\libraries\\net\\java\\dev\\jna\\platform\\3.4.0\\platform-3.4.0.jar;" + gamedir_tb.Text + "\\" + ModPack_Name + "\\libraries\\com\\ibm\\icu\\icu4j-core-mojang\\51.2\\icu4j-core-mojang-51.2.jar;" + gamedir_tb.Text + "\\" + ModPack_Name + "\\libraries\\net\\sf\\jopt-simple\\jopt-simple\\5.0.3\\jopt-simple-5.0.3.jar;" + gamedir_tb.Text + "\\" + ModPack_Name + "\\libraries\\com\\paulscode\\codecjorbis\\20101023\\codecjorbis-20101023.jar;" + gamedir_tb.Text + "\\" + ModPack_Name + "\\libraries\\com\\paulscode\\codecwav\\20101023\\codecwav-20101023.jar;" + gamedir_tb.Text + "\\" + ModPack_Name + "\\libraries\\com\\paulscode\\libraryjavasound\\20101123\\libraryjavasound-20101123.jar;" + gamedir_tb.Text + "\\" + ModPack_Name + "\\libraries\\com\\paulscode\\librarylwjglopenal\\20100824\\librarylwjglopenal-20100824.jar;" + gamedir_tb.Text + "\\" + ModPack_Name + "\\libraries\\com\\paulscode\\soundsystem\\20120107\\soundsystem-20120107.jar;" + gamedir_tb.Text + "\\" + ModPack_Name + "\\libraries\\io\\netty\\netty-all\\4.1.9.Final\\netty-all-4.1.9.Final.jar;" + gamedir_tb.Text + "\\" + ModPack_Name + "\\libraries\\com\\google\\guava\\guava\\21.0\\guava-21.0.jar;" + gamedir_tb.Text + "\\" + ModPack_Name + "\\libraries\\org\\apache\\commons\\commons-lang3\\3.5\\commons-lang3-3.5.jar;" + gamedir_tb.Text + "\\" + ModPack_Name + "\\libraries\\commons-io\\commons-io\\2.5\\commons-io-2.5.jar;" + gamedir_tb.Text + "\\" + ModPack_Name + "\\libraries\\commons-codec\\commons-codec\\1.10\\commons-codec-1.10.jar;" + gamedir_tb.Text + "\\" + ModPack_Name + "\\libraries\\net\\java\\jinput\\jinput\\2.0.5\\jinput-2.0.5.jar;" + gamedir_tb.Text + "\\" + ModPack_Name + "\\libraries\\net\\java\\jutils\\jutils\\1.0.0\\jutils-1.0.0.jar;" + gamedir_tb.Text + "\\" + ModPack_Name + "\\libraries\\com\\google\\code\\gson\\gson\\2.8.0\\gson-2.8.0.jar;" + gamedir_tb.Text + "\\" + ModPack_Name + "\\libraries\\com\\mojang\\authlib\\1.5.25\\authlib-1.5.25.jar;" + gamedir_tb.Text + "\\" + ModPack_Name + "\\libraries\\com\\mojang\\realms\\1.10.22\\realms-1.10.22.jar;" + gamedir_tb.Text + "\\" + ModPack_Name + "\\libraries\\org\\apache\\commons\\commons-compress\\1.8.1\\commons-compress-1.8.1.jar;" + gamedir_tb.Text + "\\" + ModPack_Name + "\\libraries\\org\\apache\\httpcomponents\\httpclient\\4.3.3\\httpclient-4.3.3.jar;" + gamedir_tb.Text + "\\" + ModPack_Name + "\\libraries\\commons-logging\\commons-logging\\1.1.3\\commons-logging-1.1.3.jar;" + gamedir_tb.Text + "\\" + ModPack_Name + "\\libraries\\org\\apache\\httpcomponents\\httpcore\\4.3.2\\httpcore-4.3.2.jar;" + gamedir_tb.Text + "\\" + ModPack_Name + "\\libraries\\it\\unimi\\dsi\\fastutil\\7.1.0\\fastutil-7.1.0.jar;" + gamedir_tb.Text + "\\" + ModPack_Name + "\\libraries\\org\\apache\\logging\\log4j\\log4j-api\\2.8.1\\log4j-api-2.8.1.jar;" + gamedir_tb.Text + "\\" + ModPack_Name + "\\libraries\\org\\apache\\logging\\log4j\\log4j-core\\2.8.1\\log4j-core-2.8.1.jar;" + gamedir_tb.Text + "\\" + ModPack_Name + "\\libraries\\org\\lwjgl\\lwjgl\\lwjgl\\2.9.4-nightly-20150209\\lwjgl-2.9.4-nightly-20150209.jar;" + gamedir_tb.Text + "\\" + ModPack_Name + "\\libraries\\org\\lwjgl\\lwjgl\\lwjgl_util\\2.9.4-nightly-20150209\\lwjgl_util-2.9.4-nightly-20150209.jar;" + gamedir_tb.Text + "\\" + ModPack_Name + "\\libraries\\com\\mojang\\text2speech\\1.10.3\\text2speech-1.10.3.jar;" + gamedir_tb.Text + "\\" + ModPack_Name + "\\versions\\1.12.2-forge-14.23.5.2854\\1.12.2-forge-14.23.5.2854.jar -Dminecraft.applet.TargetDirectory=" + gamedir_tb.Text + "\\" + ModPack_Name + " -XX:+UnlockExperimentalVMOptions -XX:+UseG1GC -XX:G1NewSizePercent=20 -XX:G1ReservePercent=20 -XX:MaxGCPauseMillis=50 -XX:G1HeapRegionSize=32M -Dfml.ignoreInvalidMinecraftCertificates=true -Dfml.ignorePatchDiscrepancies=true net.minecraft.launchwrapper.Launch --username " + userinfo[0] + " --version 1.12.2-forge-14.23.5.2854 --gameDir " + gamedir_tb.Text + "\\" + ModPack_Name + " --assetsDir " + gamedir_tb.Text + "\\assets --assetIndex 1.12 --uuid " + userinfo[2] + " --accessToken " + userinfo[1] + " --userType " + userinfo[3] + " --tweakClass net.minecraftforge.fml.common.launcher.FMLTweaker --versionType Forge --width 925 --height 530";

                //name  *nickname*
                //token null
                //uuid  00000000-0000-0000-0000-000000000000 
                Process.Start(startInfo);
            }
        }





        //-------------------Form-------------------------\\

        private void LaunchBTN_Click(object sender, EventArgs e)
        {
             launch_mc();
        }

        private void hasAccount_CheckedChanged(object sender, EventArgs e)
        {
            if (!hasAccount.Checked)
            { Login_tb.Enabled = false;Passwd_tb.Enabled = false; username_tb.Enabled = true; }
            else 
            { Login_tb.Enabled = true; Passwd_tb.Enabled = true; username_tb.Enabled = false; }
        }

        private void ChDir_btn_Click(object sender, EventArgs e)
        {
            /*
             *DirMap:
                assets = _YourFolder_\assets (different versions have own index)
                main dir = _YourFolder_\_ModPackName_ (here stored: mods, config ,version,libs etc)
             */
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() == DialogResult.OK)
                gamedir_tb.Text = fbd.SelectedPath;
        }

        private void ram_track_Scroll(object sender, EventArgs e)
        {
            ram_label.Text = ram_track.Value.ToString();
        }
    }
}
