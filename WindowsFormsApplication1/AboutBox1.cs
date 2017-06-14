using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjectTitanium
{
    partial class AboutBox1 : Form
    {
        int clickno = 0;
        public AboutBox1()
        {
            InitializeComponent();
            this.Text = String.Format("About {0}", AssemblyTitle);
            this.labelProductName.Text = AssemblyProduct;
            this.labelVersion.Text = String.Format("Version {0}", AssemblyVersion);
            this.labelCopyright.Text = AssemblyCopyright;
            this.labelCompanyName.Text = AssemblyCompany;
            this.textBoxDescription.Text = AssemblyDescription;
        }

        #region Assembly Attribute Accessors

        public string AssemblyTitle
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
                if (attributes.Length > 0)
                {
                    AssemblyTitleAttribute titleAttribute = (AssemblyTitleAttribute)attributes[0];
                    if (titleAttribute.Title != "")
                    {
                        return titleAttribute.Title;
                    }
                }
                return System.IO.Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().CodeBase);
            }
        }

        public string AssemblyVersion
        {
            get
            {
                return Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }
        }

        public string AssemblyDescription
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyDescriptionAttribute)attributes[0]).Description;
            }
        }

        public string AssemblyProduct
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyProductAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyProductAttribute)attributes[0]).Product;
            }
        }

        public string AssemblyCopyright
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyCopyrightAttribute)attributes[0]).Copyright;
            }
        }

        public string AssemblyCompany
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyCompanyAttribute)attributes[0]).Company;
            }
        }
        #endregion

        private void label1_Click(object sender, EventArgs e)
        {
            clickno++;

            // I think this could be really funny and light hearted, but it could get me in serious trouble. Fingers crossed on this one, hope it's not too much...

            switch (clickno)
            {
                case 1:
                    MessageBox.Show("Lucky Click!!\nYou found me!!!\nClick 'π' again!! :D", "Creator's Note");
                    label1.ForeColor = Color.Green;
                    break;
                case 5:
                    MessageBox.Show("Great Spirit!!\nYou're really good at clicking!!!\nDO NOT click 'π' again though.\nThanks :)", "Creator's Note");
                    label1.ForeColor = Color.Yellow;
                    break;
                case 6:
                    MessageBox.Show("You seem to have clicked me again.\nBut I'm sure it was a mistake...\nTry not to do it again though :|", "Creator's Note");
                    label1.ForeColor = Color.Red;
                    break;
                case 7:
                    MessageBox.Show("It's been fun, but I'm gonna stop now...\nNothing more to see here...", "Creator's Note");
                    label1.ForeColor = Color.Lavender;
                    break;
                case 8:
                    MessageBox.Show("...", "Creator's Note");
                    label1.ForeColor = Color.Gold;
                    break;
                case 9:
                    label1.ForeColor = Color.OrangeRed;
                    break;
                case 10:
                    label1.ForeColor = Color.YellowGreen;
                    break;
                case 11:
                    MessageBox.Show("Why you no stop clicking???", "Creator's Note");
                    label1.ForeColor = Color.Olive;
                    break;
                case 12:
                    if (MessageBox.Show("Click 'Yes' to receive your award for making it to 12 clicks!!", "Creator's Note", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) == DialogResult.Yes)
                    {
                        System.Diagnostics.Process.Start("https://youtu.be/dQw4w9WgXcQ");
                    }
                    label1.ForeColor = Color.Firebrick;
                    break;
                case 13:
                    MessageBox.Show("lol sorry for being a troll. I'll stop now.\nThanks for playing.", "Creator's Note");
                    label1.Dispose();
                    break;
                default:
                    MessageBox.Show("This app was made by Arnold, from Pi Team.\nIn case there are any errors, suggestions, or feedback, please forward them to 'asolanki@au1.ibm.com'.\nThanks for your support.", "Creator's Note");
                    label1.ForeColor = Color.FromArgb(clickno);
                    break;
            }
        }
    }
}
