using System;
using System.IO;
using System.Security.Cryptography;
using System.Windows.Forms;

namespace VLauncher_Own_
{
    /*
     * 1. Progress bar value
     * 2. label 2 text
     * 3. Differend threads to check and Gen hash
     */
    internal class Hasher {

        private FileInfo file;
        private byte[] hash;

        public Hasher() { }

        private Hasher(FileInfo file, byte[] hash)
        {
            this.file = file;
            this.hash = hash;
        }

        public Hasher[] getDirHash(string path,TextBox textBox)
        {

            DirectoryInfo info = new DirectoryInfo(path);

            FileInfo[] files = info.GetFiles();
            DirectoryInfo[] info1 = info.GetDirectories();
            if (info1.Length != 0)
                for(int j=0; j<info1.Length;j++)
                    hashToFile(getDirHash(info1[j].FullName,textBox), textBox.Text);

            Hasher[] hashers = new Hasher[files.Length];

            SHA256 sha = SHA256.Create();

            int i = 0;
            foreach (FileInfo fileInfo in files)
            {
                FileStream fs = fileInfo.Open(FileMode.Open);
                fs.Position = 0;
                byte[] hashValue = sha.ComputeHash(fs);
                hashers[i] = new Hasher(fileInfo, hashValue);
                i += 1;
                fs.Close();
            }

            return hashers;
        }

        public void hashToFile(Hasher[] hashers,string path)
        {
            using (StreamWriter sw = new StreamWriter(path+"\\.hash", true, System.Text.Encoding.Default))
            {
                for(int i=0; i<hashers.Length;i++)
                    sw.WriteLine(hashers[i].file.FullName+"\n"+printHash(hashers[i].hash));
                sw.Close();
            }
        }

        private string printHash(byte[] hash)
        {
            string _tmp="";
            for(int i =0;i<hash.Length; i++)
            {
                _tmp += hash[i];
            }
            return _tmp;
        }

    }
}
