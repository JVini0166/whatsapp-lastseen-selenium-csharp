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
    }
}
