using System;
using System.Windows.Forms;

namespace _2017_07_28_objektu_rinkinys_su_sqlite
{
    public partial class IvedimasForma : Form
    {
        public Telefonas Telefonas { get; private set; }

        public IvedimasForma()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var modelis = textBox1.Text;
            var istrizaine = Convert.ToDouble(textBox2.Text);
            var atmintis = Convert.ToInt32(textBox3.Text);
            var baterija = Convert.ToInt32(textBox4.Text);
            Telefonas = new Telefonas(modelis, istrizaine, atmintis, baterija);
        }
    }
}
