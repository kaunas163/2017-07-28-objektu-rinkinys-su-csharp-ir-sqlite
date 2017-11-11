using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SQLite;

namespace _2017_07_28_objektu_rinkinys_su_sqlite
{
    public partial class Form1 : Form
    {
        public string Prisijungimas { get; }
        public List<Telefonas> Telefonai { get; }
        // propg -> tab

        public Form1()
        {
            InitializeComponent();
            Telefonai = new List<Telefonas>();

            // c# turės žinoti prie kurios duomenų bazės turės jungtis, todėl tokia info pateikiama čia
            Prisijungimas = "Data Source=duombaze.db;Version=3;";
        }

        private void iseitiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // sukuriam prisijungimo prie duomenų bazės objektą
            using (var con = new SQLiteConnection(Prisijungimas))
            {
                try
                {
                    // prisijungiam prie duomenų bazės su katik sukurtu objektu
                    con.Open();

                    // patikrinam ar prie duomenų bazės prisijungti pavyko
                    /*if (con.State == ConnectionState.Open)
                    {
                        // jei prisijungti pavyko, tuomet išvedam pranešimą
                        MessageBox.Show("Prisijungta");
                    }*/

                    // kuriam sql užklausą, kuri paims reikiamus duomenis iš nurodytos duomenų bazės lentelės/lentelių
                    var sql = "SELECT modelis, istrizaine, atmintis, baterijos_talpa FROM telefonai";

                    // sukuriam komandą, kuri žino prisijungimo prie duomenų bazės duomens, bei
                    // žino kokią sql užklausą turės įvykdyti
                    using (SQLiteCommand komanda = new SQLiteCommand(sql, con))
                    {
                        // paleidžiam komandą, jos atsakymą (gautus duomenis) priskiriam prie naujo objekto
                        SQLiteDataReader skaitytuvas = komanda.ExecuteReader();

                        // skaitom gautus duomenis, kol jų turime
                        while (skaitytuvas.Read())
                        {
                            // kiekvieną duomenų bazėje esančios lentelės stupelį išskiriam į atskirus c# kintamuosius
                            var modelis = skaitytuvas["modelis"].ToString();
                            var istrizaine = Convert.ToDouble(skaitytuvas["istrizaine"]);
                            var atmintis = Convert.ToInt32(skaitytuvas["atmintis"]);
                            var baterija = Convert.ToInt32(skaitytuvas["baterijos_talpa"]);

                            // sukuriam telefono objektą iš turimų duomenų
                            var telefonas = new Telefonas(modelis, istrizaine, atmintis, baterija);

                            // sukurtą telefono objektą įkeliam į telefonų sąrašą
                            Telefonai.Add(telefonas);
                        }

                        // parodom sukurtą telefonų sąrašą formoje
                        dataGridView1.DataSource = Telefonai;

                        // statuso juostoje atnaujinam įrašų skaičių
                        toolStripStatusLabel2.Text = Telefonai.Count.ToString();
                    }

                    // nutraukiame prisijungimą nuo duomenų bazės
                    con.Close();
                }
                catch (Exception exception)
                {
                    // try bloke ieškom klaidų, jei randam, sukuriam objektą exception, kurio klaidos
                    // pranešimą galima išvesti per jo kintamąjį message
                    // tokiu atveju programa bent pasileistų įvykus tokiai klaida, o ne nulūžtų visiškai
                    MessageBox.Show(exception.Message);
                    
                    //throw;
                }
            }
        }

        private void naujasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // sukuriam įvedimo formos objektą
            using (var forma = new IvedimasForma())
            {
                // kviečiam / parodom įvedimo formos objektą
                forma.ShowDialog();

                // žiūrim ar formoje buvo paspaustas mygtukas gerai
                if (forma.DialogResult == DialogResult.OK)
                {
                    // jeigu taip, tuomet:

                    // pridedam formoje įvesto telefono duomenis į bendrą telefonų sąrašą
                    Telefonai.Add(forma.Telefonas);

                    // nuimam sąrašo atvaizdavimą nuo lentelės, sąrašas niekur nedingsta
                    dataGridView1.DataSource = null;

                    // per naują užkraunam telefonų sąrašo atvaizdavimą į lentelę
                    dataGridView1.DataSource = Telefonai;

                    // atnaujinam statuso juostoje esančių įrašų skaičių
                    toolStripStatusLabel2.Text = Telefonai.Count.ToString();

                    // sukuriam prisijungimo prie duomenų bazės objektą
                    using (var con = new SQLiteConnection(Prisijungimas))
                    {
                        try
                        {
                            // atidarom prisijungimą prie duomenų bazės
                            con.Open();
                            
                            // sukuriam komandos objektą, kuris siųs duomenų bazei užklausą / pasakys ką ji turėtų daryti
                            using (SQLiteCommand komanda = con.CreateCommand())
                            {
                                // prie komandos pridedam sql užklausos aprašymą, šiuo atveju kur ir kokius duomenis turės įrašyti
                                komanda.CommandText = "INSERT INTO telefonai (modelis, istrizaine, atmintis, baterijos_talpa) VALUES (@modelis, @istrizaine, @atmintis, @baterija)";

                                // pridedam parametrus prie jau turimus sql užklausos,
                                // ten kur buvo nurodyta @, tai ką į tas vietas įsistatome
                                komanda.Parameters.AddWithValue("@modelis", forma.Telefonas.Modelis);
                                komanda.Parameters.AddWithValue("@istrizaine", forma.Telefonas.Istrizaine);
                                komanda.Parameters.AddWithValue("@atmintis", forma.Telefonas.Atmintis);
                                komanda.Parameters.AddWithValue("@baterija", forma.Telefonas.BaterijosTalpa);
                                
                                // paleidžiame / įvykdome komandą
                                komanda.ExecuteNonQuery();
                            }

                            // uždarom prisijungimą prie duomenų bazės
                            con.Close();
                        }
                        catch (Exception exception)
                        {
                            // sugaudome bet kokias try bloke iškilusias klaidas ir neužlauždami programos
                            // išvedame pranešimą su klaidos aprašymu
                            MessageBox.Show(exception.Message);
                            //throw;
                        }
                    }
                }
            }
        }
    }
}
