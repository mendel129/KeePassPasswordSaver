using CredentialManagement;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LensFirstKeepassPlugin
{
    public partial class PasswordForm : Form
    {
        public PasswordForm()
        {
            InitializeComponent();
        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            string PasswordName = "KeePassPassword";
            using (var cred = new Credential())
            {
                cred.Password = tb_password.Text;
                cred.Target = PasswordName;
                cred.Type = CredentialType.Generic;
                cred.PersistanceType = PersistanceType.LocalComputer;
                cred.Save();
                
            }
            this.Close();
        }

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void PasswordForm_Load(object sender, EventArgs e)
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
                    tb_password.Text = storedpass;
                }
            }
            catch (Exception exce)
            {
                Console.Write(exce);
            }
        }
    }

}
