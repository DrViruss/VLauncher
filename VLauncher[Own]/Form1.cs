using System;
using System.Diagnostics;
using System.Windows.Forms;
using System.IO;
using System.Text.Json;
using System.Net;

namespace VLauncher_Own_
{
    public partial class lform : Form
    {

        public lform()
        {
            InitializeComponent();

            PC pc = new PC();

            //First-time folder set
            gamedir_tb.Text = pc.getAppData() + "\\.minecraft";

            //First-time RAM set
            ram_track.Maximum = (Int32)pc.getRAM()*1024-512;
            ram_track.Value = ram_track.Maximum / 2;
            ram_label.Text = ram_track.Value.ToString();

            //First-time Username set
            username_tb.Text = pc.getUsername();

            //Check Java on start
            if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable("JAVA_HOME")))
            {
                myJava.Checked = true;
                myJava.Enabled = false;
            }

            //use non-legal version by default
            Login_tb.Enabled = false; 
            Passwd_tb.Enabled = false;

            //get and set modpack in ComboBox
            GetAndSetModpack();
        }
       
        //-------------------TMP-Vars-------------------------\\



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

        ///
        /// 
        ///
        /// 
        /// 
        /// 
        /// 


        string getJavaPath()
        {
            if (!myJava.Checked)
                return Environment.GetEnvironmentVariable("JAVA_HOME")+"\\bin";
            else
            {
               // if (Directory.Exists(gamedir_tb.Text+"\\Java"))
                    return gamedir_tb.Text + "\\Java\\bin";
                //else
                //{
                //    backgroundWorker1.RunWorkerAsync();
                //}
                //do
                //{
                //    Application.DoEvents();
                //    if (!backgroundWorker1.IsBusy)
                //    return gamedir_tb.Text + "\\Java\\bin";
                //} while (true);
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



        //-------------------MP_TEST-------------------------\\
        
        void GetAndSetModpack()
        {
            string link = "http://v-packs.c1.biz/ModPacks_Names.txt";
            HttpWebRequest rq = (HttpWebRequest)WebRequest.Create(link);
            rq.KeepAlive = false;

            try
            {
                HttpWebResponse response = (HttpWebResponse)rq.GetResponse();
                using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                {
                    MPList_combo.Items.Clear();
                    string[] SsS = sr.ReadToEnd().Split('\n');
                    for (int i = 0; i < SsS.Length; i++)
                        MPList_combo.Items.Add(SsS[i]);
                    MPList_combo.SelectedIndex = 0;
                }
            }
            catch
            {
                MessageBox.Show("Check your internet connection","Conection error",MessageBoxButtons.OK,MessageBoxIcon.Error);
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
                //OMG
                //TODO: test another "--versionType" :D
                    startInfo.Arguments = "-Djava.net.preferIPv4Stack=true -Xmn128M -Xmx" + ram_label.Text + "M -Djava.library.path=" + gamedir_tb.Text + "\\" + MPList_combo.Text + "\\versions\\1.12.2-forge-14.23.5.2854\\natives -cp " + gamedir_tb.Text + "\\" + MPList_combo.Text + "\\libraries\\forge-1.12.2-14.23.5.2854.jar;" + gamedir_tb.Text + "\\" + MPList_combo.Text + "\\libraries\\asm-debug-all-5.2.jar;" + gamedir_tb.Text + "\\" + MPList_combo.Text + "\\libraries\\launchwrapper-1.12.jar;" + gamedir_tb.Text + "\\" + MPList_combo.Text + "\\libraries\\jline-3.5.1.jar;" + gamedir_tb.Text + "\\" + MPList_combo.Text + "\\libraries\\akka-actor_2.11-2.3.3.jar;" + gamedir_tb.Text + "\\" + MPList_combo.Text + "\\libraries\\config-1.2.1.jar;" + gamedir_tb.Text + "\\" + MPList_combo.Text + "\\libraries\\scala-actors-migration_2.11-1.1.0.jar;" + gamedir_tb.Text + "\\" + MPList_combo.Text + "\\libraries\\scala-compiler-2.11.1.jar;" + gamedir_tb.Text + "\\" + MPList_combo.Text + "\\libraries\\scala-continuations-library_2.11-1.0.2_mc.jar;" + gamedir_tb.Text + "\\" + MPList_combo.Text + "\\libraries\\scala-continuations-plugin_2.11.1-1.0.2_mc.jar;" + gamedir_tb.Text + "\\" + MPList_combo.Text + "\\libraries\\scala-library-2.11.1.jar;" + gamedir_tb.Text + "\\" + MPList_combo.Text + "\\libraries\\scala-parser-combinators_2.11-1.0.1.jar;" + gamedir_tb.Text + "\\" + MPList_combo.Text + "\\libraries\\scala-reflect-2.11.1.jar;" + gamedir_tb.Text + "\\" + MPList_combo.Text + "\\libraries\\scala-swing_2.11-1.0.1.jar;" + gamedir_tb.Text + "\\" + MPList_combo.Text + "\\libraries\\scala-xml_2.11-1.0.2.jar;" + gamedir_tb.Text + "\\" + MPList_combo.Text + "\\libraries\\lzma-0.0.1.jar;" + gamedir_tb.Text + "\\" + MPList_combo.Text + "\\libraries\\vecmath-1.5.2.jar;" + gamedir_tb.Text + "\\" + MPList_combo.Text + "\\libraries\\trove4j-3.0.3.jar;" + gamedir_tb.Text + "\\" + MPList_combo.Text + "\\libraries\\maven-artifact-3.5.3.jar;" + gamedir_tb.Text + "\\" + MPList_combo.Text + "\\libraries\\jopt-simple-5.0.3.jar;" + gamedir_tb.Text + "\\" + MPList_combo.Text + "\\libraries\\oshi-core-1.1.jar;" + gamedir_tb.Text + "\\" + MPList_combo.Text + "\\libraries\\jna-4.4.0.jar;" + gamedir_tb.Text + "\\" + MPList_combo.Text + "\\libraries\\platform-3.4.0.jar;" + gamedir_tb.Text + "\\" + MPList_combo.Text + "\\libraries\\icu4j-core-mojang-51.2.jar;" + gamedir_tb.Text + "\\" + MPList_combo.Text + "\\libraries\\codecjorbis-20101023.jar;" + gamedir_tb.Text + "\\" + MPList_combo.Text + "\\libraries\\codecwav-20101023.jar;" + gamedir_tb.Text + "\\" + MPList_combo.Text + "\\libraries\\libraryjavasound-20101123.jar;" + gamedir_tb.Text + "\\" + MPList_combo.Text + "\\libraries\\librarylwjglopenal-20100824.jar;" + gamedir_tb.Text + "\\" + MPList_combo.Text + "\\libraries\\soundsystem-20120107.jar;" + gamedir_tb.Text + "\\" + MPList_combo.Text + "\\libraries\\netty-all-4.1.9.Final.jar;" + gamedir_tb.Text + "\\" + MPList_combo.Text + "\\libraries\\guava-21.0.jar;" + gamedir_tb.Text + "\\" + MPList_combo.Text + "\\libraries\\commons-lang3-3.5.jar;" + gamedir_tb.Text + "\\" + MPList_combo.Text + "\\libraries\\commons-io-2.5.jar;" + gamedir_tb.Text + "\\" + MPList_combo.Text + "\\libraries\\commons-codec-1.10.jar;" + gamedir_tb.Text + "\\" + MPList_combo.Text + "\\libraries\\jinput-2.0.5.jar;" + gamedir_tb.Text + "\\" + MPList_combo.Text + "\\libraries\\jutils-1.0.0.jar;" + gamedir_tb.Text + "\\" + MPList_combo.Text + "\\libraries\\gson-2.8.0.jar;" + gamedir_tb.Text + "\\" + MPList_combo.Text + "\\libraries\\authlib-1.5.25.jar;" + gamedir_tb.Text + "\\" + MPList_combo.Text + "\\libraries\\realms-1.10.22.jar;" + gamedir_tb.Text + "\\" + MPList_combo.Text + "\\libraries\\commons-compress-1.8.1.jar;" + gamedir_tb.Text + "\\" + MPList_combo.Text + "\\libraries\\httpclient-4.3.3.jar;" + gamedir_tb.Text + "\\" + MPList_combo.Text + "\\libraries\\commons-logging-1.1.3.jar;" + gamedir_tb.Text + "\\" + MPList_combo.Text + "\\libraries\\httpcore-4.3.2.jar;" + gamedir_tb.Text + "\\" + MPList_combo.Text + "\\libraries\\fastutil-7.1.0.jar;" + gamedir_tb.Text + "\\" + MPList_combo.Text + "\\libraries\\log4j-api-2.8.1.jar;" + gamedir_tb.Text + "\\" + MPList_combo.Text + "\\libraries\\log4j-core-2.8.1.jar;" + gamedir_tb.Text + "\\" + MPList_combo.Text + "\\libraries\\lwjgl-2.9.4-nightly-20150209.jar;" + gamedir_tb.Text + "\\" + MPList_combo.Text + "\\libraries\\lwjgl_util-2.9.4-nightly-20150209.jar;" + gamedir_tb.Text + "\\" + MPList_combo.Text + "\\libraries\\text2speech-1.10.3.jar;" + gamedir_tb.Text + "\\" + MPList_combo.Text + "\\versions\\1.12.2-forge-14.23.5.2854\\1.12.2-forge-14.23.5.2854.jar -Dminecraft.applet.TargetDirectory=" + gamedir_tb.Text + "\\" + MPList_combo.Text + " -XX:+UnlockExperimentalVMOptions -XX:+UseG1GC -XX:G1NewSizePercent=20 -XX:G1ReservePercent=20 -XX:MaxGCPauseMillis=50 -XX:G1HeapRegionSize=32M -Dfml.ignoreInvalidMinecraftCertificates=true -Dfml.ignorePatchDiscrepancies=true net.minecraft.launchwrapper.Launch --username " + userinfo[0] + " --version 1.12.2-forge-14.23.5.2854 --gameDir " + gamedir_tb.Text + "\\" + MPList_combo.Text + " --assetsDir " + gamedir_tb.Text + "\\assets --assetIndex 1.12 --uuid " + userinfo[2] + " --accessToken " + userinfo[1] + " --userType " + userinfo[3] + " --tweakClass net.minecraftforge.fml.common.launcher.FMLTweaker --versionType Forge --width 925 --height 530";
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


        /*
         * 1.Remove this buttons
         * 2. Check hash before starting
         * 3. Download if hash failure
         * 4. Check+Download+Run - on 1 button
         * 5. Rename essets foulder to 1.12.2
         */
        
        private void download_btn_Click(object sender, EventArgs e)
        {
            Downloader downloader = new Downloader();
            downloader.mpname = MPList_combo.Text;
            downloader.gamepath = gamedir_tb.Text;
          //downloader.Download(Downloader.DownloadType.mpname, label2, progressBar1);
          //downloader.Download(Downloader.DownloadType.java, label2, progressBar1);
            downloader.Download(Downloader.DownloadType.assets, label2, progressBar1);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Hasher hasher = new Hasher();
            //hasher.hashToFile(hasher.getDirHash(gamedir_tb.Text+"\\Java",gamedir_tb),gamedir_tb.Text+"\\Java") ;
            //hasher.hashToFile(hasher.getDirHash(gamedir_tb.Text + "\\"+MPList_combo.Text, gamedir_tb), gamedir_tb.Text);
            hasher.hashToFile(hasher.getDirHash(gamedir_tb.Text + "\\assets", gamedir_tb), gamedir_tb.Text);
        }
    }
}
