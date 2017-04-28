using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using KeePass.Plugins;
using KeePassLib.Keys;
using CredentialManagement;
using KeePass.Forms;
using KeePass.Plugins;
using KeePass.Resources;
using KeePass.UI;

using KeePassLib.Utility;
using System.Windows.Forms;

Windows.Security;

namespace LensFirstKeepassPlugin
{
    public sealed class LensFirstKeepassPluginExt : Plugin
    {
        private IPluginHost m_host = null;
        private LensFirstKeepassPlugin m_prov = new LensFirstKeepassPlugin();
        private ToolStripSeparator m_tsSeparator = null;
        private ToolStripMenuItem m_tsmiMenuItem = null;

        public override bool Initialize(IPluginHost host)
        {
            m_host = host;

            ToolStripItemCollection tsMenu = m_host.MainWindow.ToolsMenu.DropDownItems;

            // Add a separator at the bottom
            m_tsSeparator = new ToolStripSeparator();
            tsMenu.Add(m_tsSeparator);

            // Add menu item 'Do Something'
            m_tsmiMenuItem = new ToolStripMenuItem();
            m_tsmiMenuItem.Text = "WindowsPasswordStorage";
            m_tsmiMenuItem.Click += this.OnMenuDoSomething;
            tsMenu.Add(m_tsmiMenuItem);

            m_host.KeyProviderPool.Add(m_prov);
            return true;
        }

        public override void Terminate()
        {
            // Remove all of our menu items
            ToolStripItemCollection tsMenu = m_host.MainWindow.ToolsMenu.DropDownItems;
            m_tsmiMenuItem.Click -= this.OnMenuDoSomething;
            tsMenu.Remove(m_tsmiMenuItem);
            tsMenu.Remove(m_tsSeparator);

            m_host.KeyProviderPool.Remove(m_prov);


        }

        private void OnMenuDoSomething(object sender, EventArgs e)
        {
            // Called when the menu item is clicked
            PasswordForm passform = new PasswordForm();
            passform.Show();
        }
    }

    public sealed class LensFirstKeepassPlugin : KeyProvider
    {
        public override string Name
        {
            get { return "Mendel's Sample Key Provider"; }
        }

        public override byte[] GetKey(KeyProviderQueryContext ctx)
        {
            string storedpass = "";
            string PasswordName = "KeePassPassword";

            try
            {
                Credential credential = new Credential { Target = PasswordName };
                if (credential.Exists())
                {
                    credential.Load();
                    storedpass = credential.Password;
                }
                else
                {
                    PasswordForm passform = new PasswordForm();
                    passform.Show();
                }
            }
            catch (Exception e)
            {
                Console.Write(e);
            }


            // Return a sample key. In a real key provider plugin, the key
            // would be retrieved from smart card, USB device, ...
            //byte[] toBytes = Encoding.ASCII.GetBytes("testpassword");
            byte[] toBytes = Encoding.ASCII.GetBytes(storedpass);

            return toBytes;
        }
    }
}
