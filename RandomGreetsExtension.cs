using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using iconnect;

namespace RandomGreetsExtension
{
    public class RandomGreets : IExtension
    {
        private static List<String> greetings = new List<String>();
        private static Random random = new Random();
        private static FileSystemWatcher watcher;
        private static String greetingsPath;
        private static IHostApp callback;

        public RandomGreets(IHostApp host)
        {
            callback = host;
        }

        public void ServerStarted()
        {
            greetingsPath = Path.Combine(callback.DataPath, "greetings.xml");
            LoadGreetings();
            SetupFileWatcher();
        }

        private void SetupFileWatcher()
        {
            try
            {
                String directory = Path.GetDirectoryName(greetingsPath);
                String filename = Path.GetFileName(greetingsPath);

                watcher = new FileSystemWatcher(directory, filename);
                watcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.Size;
                watcher.Changed += OnGreetingsChanged;
                watcher.EnableRaisingEvents = true;
            }
            catch { }
        }

        private void OnGreetingsChanged(object sender, FileSystemEventArgs e)
        {
            System.Threading.Thread.Sleep(100);
            LoadGreetings();
        }

        private void LoadGreetings()
        {
            try
            {
                XmlDocument xml = new XmlDocument();
                xml.PreserveWhitespace = true;
                xml.Load(greetingsPath);

                XmlNodeList nodes = xml.GetElementsByTagName("item");

                greetings.Clear();
                foreach (XmlNode n in nodes)
                    greetings.Add(n.InnerText);
            }
            catch { }
        }

        public void Dispose()
        {
            if (watcher != null)
            {
                watcher.EnableRaisingEvents = false;
                watcher.Dispose();
            }
        }

        public void Joined(IUser client)
        {
            if (greetings.Count > 0)
            {
                int index = random.Next(greetings.Count);
                String greet = greetings[index];

                greet = greet.Replace("+n", client.Name);
                greet = greet.Replace("+ip", client.ExternalIP.ToString());

                callback.Users.Ares(x => x.Print(greet));
                callback.Users.Web(x => x.Print(greet));
            }
        }

        public void CycleTick() { }
        public void UnhandledProtocol(IUser client, bool custom, byte msg, byte[] packet) { }
        public bool Joining(IUser client) { return true; }
        public void Rejected(IUser client, RejectedMsg msg) { }
        public void Parting(IUser client) { }
        public void Parted(IUser client) { }
        public bool AvatarReceived(IUser client) { return true; }
        public bool PersonalMessageReceived(IUser client, String text) { return true; }
        public void TextReceived(IUser client, String text) { }
        public String TextSending(IUser client, String text) { return text; }
        public void TextSent(IUser client, String text) { }
        public void EmoteReceived(IUser client, String text) { }
        public String EmoteSending(IUser client, String text) { return text; }
        public void EmoteSent(IUser client, String text) { }
        public void PrivateSending(IUser client, IUser target, IPrivateMsg msg) { }
        public void PrivateSent(IUser client, IUser target) { }
        public void BotPrivateSent(IUser client, String text) { }
        public void Command(IUser client, String command, IUser target, String args) { }
        public bool Nick(IUser client, String name) { return true; }
        public void Help(IUser client) { }
        public void FileReceived(IUser client, String filename, String title, MimeType type) { }
        public bool Ignoring(IUser client, IUser target) { return true; }
        public void IgnoredStateChanged(IUser client, IUser target, bool ignored) { }
        public void InvalidLoginAttempt(IUser client) { }
        public void LoginGranted(IUser client) { }
        public void AdminLevelChanged(IUser client) { }
        public void InvalidRegistration(IUser client) { }
        public bool Registering(IUser client) { return true; }
        public void Registered(IUser client) { }
        public void Unregistered(IUser client) { }
        public void CaptchaSending(IUser client) { }
        public void CaptchaReply(IUser client, String reply) { }
        public bool VroomChanging(IUser client, ushort vroom) { return true; }
        public void VroomChanged(IUser client) { }
        public bool Flooding(IUser client, byte msg) { return false; }
        public void Flooded(IUser client) { }
        public bool ProxyDetected(IUser client) { return false; }
        public void Logout(IUser client) { }
        public void Idled(IUser client) { }
        public void Unidled(IUser client, uint seconds_away) { }
        public void BansAutoCleared() { }
        public void Load() { }
        public void LinkError(ILinkError error) { }
        public void Linked() { }
        public void Unlinked() { }
        public void LeafJoined(ILeaf leaf) { }
        public void LeafParted(ILeaf leaf) { }
        public void LinkedAdminDisabled(ILeaf leaf, IUser client) { }
        public BitmapSource Icon { get { return null; } }
        public UserControl GUI { get { return null; } }
    }
}
