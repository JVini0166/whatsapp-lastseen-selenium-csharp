using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace WhatsAppOnlineChecker
{
    public partial class Form1 : Form
    {
        IWebDriver browser = new ChromeDriver();
        String nomeArquivo = "registros.txt";

        public Form1()
        {
            InitializeComponent();
            statusLabel.Text = "Coloque o nome do contato e prossiga.";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            whatsApp();
            aindaEstaRodando();
        }

        public async void whatsApp()
        {
            browser.Url = "https://web.whatsapp.com/";
            validaAbaDoContato();
            registrarContatoEstavaOnline();
        }

        public string getCurrentTime()
        {
            String timeString;
            return timeString = DateTime.Now.ToString("HH:mm:ss");
        }

        public string getNomeContato()
        {
            String nomeContato;
            return nomeContato = textBox1.Text;
        }

        public async void aindaEstaRodando()
        {
            registroOnlineBox.Items.Add(getCurrentTime() + " relaxe, está rodando, +5min.");
            await Task.Delay(300000); //Verificar a cada 5 minutos se está rodando
            aindaEstaRodando();
        }

        public Boolean contemTagOnline()
        {
            String tagOnline = "title=\"online\"";
            return browser.PageSource.Contains(tagOnline);
        }

        public async void validaAbaDoContato()
        {
            while (!(browser.PageSource.Contains(getNomeContato()) && (contemTagOnline() || browser.PageSource.Contains("visto por") || browser.PageSource.Contains("digitando"))))
            {
                statusLabel.Text = "Não está na aba algum contato.";
                await Task.Delay(1000);
            }
            await Task.Delay(1000);
            statusLabel.Text = "Está na aba de algum contato.";
            validaAbaDoContato();
        }

        public async void registrarContatoEstavaOnline()
        {
            while (!(contemTagOnline()))
            {
                await Task.Delay(2000);
            }

            registroOnlineBox.Items.Add(getCurrentTime() + " " + getNomeContato() + " esteve online.");
            await Task.Delay(2000);
            registrarContatoEstavaOnline();
        }

        private void registroOnlineBox_SelectedIndexChanged(object sender, EventArgs e)
        {
           
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
            browser.Quit();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void salvarButton_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Tem certeza? Está salvando registro_antigo + registro_novo!", "Salvar dados em arquivo de texto", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                System.IO.StreamWriter SaveFile = new System.IO.StreamWriter(nomeArquivo, true);
                foreach (var item in registroOnlineBox.Items)
                {
                    SaveFile.WriteLine(item.ToString());
                }

                SaveFile.Close();
                MessageBox.Show("Seus registros foram salvos junto com os antigos no local do programa num arquivo chamado: " + nomeArquivo);
            }
            else if (dialogResult == DialogResult.No)
            {
                
            }

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Tem certeza? Está apagando os velhos registros e salvando somente os novos!", "Salvar dados em arquivo de texto", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                System.IO.StreamWriter SaveFile = new System.IO.StreamWriter(nomeArquivo, true);
                foreach (var item in registroOnlineBox.Items)
                {
                    SaveFile.WriteLine(item.ToString());
                }
                SaveFile.Close();
                MessageBox.Show("Seus registros antigos foram apagados e os recentes salvos no local do programa num arquivo chamado: " + nomeArquivo);
            }
            else if (dialogResult == DialogResult.No)
            {
   
            }  
        }
    }
}
