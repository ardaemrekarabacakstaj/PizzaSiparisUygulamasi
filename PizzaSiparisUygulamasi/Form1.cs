using iTextSharp.text.pdf;
using iTextSharp.text;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace PizzaSiparisUygulamasi
{
    public partial class Form1 : Form
    {
        string musteriAdi, musteriAdresi, pizzaBoyutu, siparisTarihi, pizzaCesidi;
        Pizza yeniPizza;
        double toplamPizzaFiyati;
        PizzaOrder pizzaOrder;
        private List<PizzaOrder> pizzaOrders = new List<PizzaOrder>();


        private void Form1_Load(object sender, EventArgs e)
        {
           loadJson();
        }

        private void loadJson()
        {
            
            using (StreamReader r = new StreamReader("PizzaOrder.json"))
            {
                string json = r.ReadToEnd();

                List<PizzaOrder> pizzaOrders = JsonConvert.DeserializeObject<List<PizzaOrder>>(json);
                string[] array = { pizzaOrders.ToArray().ToString() };

                StringBuilder message = new StringBuilder();
                listBox1.Items.Clear();
                foreach (var pizzaOrder in pizzaOrders)
                {
                    message.Append($"Customer Name: {pizzaOrder.CustomerName}\n");
                    message.Append($"Delivery Address: {pizzaOrder.DeliveryAddress}\n");
                    message.Append($"Order Date: {pizzaOrder.OrderDate}\n");
                    message.Append($"Total Amount: {pizzaOrder.TotalAmount}\n");
                    message.Append("Pizzas:\n");

                    foreach (var pizza in pizzaOrder.Pizzas)
                    {
                        message.Append($"  Name: {pizza.Name}\n");
                        message.Append($"  Size: {pizza.Size}\n");
                        message.Append($"  Toppings: {string.Join(", ", pizza.Toppings)}\n");
                    }

                    message.Append("====================\n\n");
                    listBox1.Items.Add(message.ToString());
                }
             

            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(listBox1.SelectedItem != null)
            {
                MessageBox.Show(listBox1.SelectedItem.ToString());
            }
            else { MessageBox.Show("Seçim Yapınız...");}
        }

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            musteriAdi = textBox1.Text.ToString();
            musteriAdresi = textBox2.Text.ToString();
            siparisTarihi = dateTimePicker1.Text.ToString();

           pizzaCesidi = comboBox1.Text.ToString();
           pizzaBoyutu = comboBox3.Text.ToString();

            List<string> list = new List<string>();
            foreach (object item in checkedListBox1.CheckedItems)
            {
                list.Add(item.ToString());
            }
            int ekMalzemeAdedi = list.Count;
            double ekMalzemeFiyati = 2.5;

            double toplamEkMalzemeFiyati = ekMalzemeFiyati * ekMalzemeAdedi;
            string ekMalzemelerList = string.Join(", ", list);

            double boyutFiyati;
            if (comboBox3.SelectedIndex >= 0)
            {
                if (comboBox3.SelectedIndex == 0)
                {
                    boyutFiyati = 3;
                }
                else if (comboBox3.SelectedIndex == 1)
                {
                    boyutFiyati = 6;
                }
                else if (comboBox3.SelectedIndex == 2)
                {
                    boyutFiyati = 10;
                }
                else
                {
                    MessageBox.Show("Geçersiz Boyut Seçimi...");
                    return;
                }
            }
            else
            {
                MessageBox.Show("Lütfen Boyutunuzu Seçiniz...");
                return;
            }

            double secilenPizzaFiyati;
            if (comboBox1.SelectedItem != null)
            {
                secilenPizzaFiyati = 5.5;
            }
            else
            {
                MessageBox.Show("Lütfen Pizzanızı Seçiniz...");
                return;
            }

            toplamPizzaFiyati = secilenPizzaFiyati + toplamEkMalzemeFiyati + boyutFiyati;

             yeniPizza = new Pizza()
            {
                Name = pizzaCesidi,
                Size = pizzaBoyutu,
                Toppings = list,
            };

            pizzaOrder = new PizzaOrder()
            {
                CustomerName = musteriAdi,
                DeliveryAddress = musteriAdresi,
                OrderDate = siparisTarihi,
                Pizzas = new List<Pizza>() { yeniPizza },
                TotalAmount = toplamPizzaFiyati,
            };
            label4.Text = " Toplam Fiyatı : "+toplamPizzaFiyati.ToString();
            listBox1.Items.Add(yeniPizza.ToString());



            pizzaOrders.Add(pizzaOrder);
            string updatedPizzaOrderJson = JsonConvert.SerializeObject(pizzaOrders, Formatting.Indented);
            File.WriteAllText("PizzaOrder.json", updatedPizzaOrderJson);
            
            loadJson();
            MessageBox.Show("Pizza JSON olarak kaydedildi: PizzaOrder.json");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Ödendi...");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                using (FileStream fs = new FileStream("Fatura.pdf", FileMode.Create))
                {
                    Document doc = new Document();
                    PdfWriter writer = PdfWriter.GetInstance(doc, fs);

                    doc.Open();
                    doc.Add(new Paragraph("Fatura Bilgileri"));
                    doc.Add(new Paragraph("--------------------------------------------"));
                    doc.Add(new Paragraph($"Müşteri Adı: {musteriAdi}"));
                    doc.Add(new Paragraph($"Müşteri Adresi: {musteriAdresi}"));
                    doc.Add(new Paragraph($"Sipariş Tarihi: {siparisTarihi}"));
                    doc.Add(new Paragraph("--------------------------------------------"));
                    doc.Add(new Paragraph($"Pizza Çeşidi: {yeniPizza.Name}"));
                    doc.Add(new Paragraph($"Pizza Boyutu: {yeniPizza.Size}"));
                    doc.Add(new Paragraph($"Ek Malzemeler: {string.Join(", ", yeniPizza.Toppings)}"));
                    doc.Add(new Paragraph("--------------------------------------------"));
                    doc.Add(new Paragraph($"Toplam Fiyat: {toplamPizzaFiyati} TL"));
                    doc.Close();
                }

                MessageBox.Show("Fatura PDF olarak kaydedildi: Fatura.pdf");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Bir hata oluştu: " + ex.Message);
            }
        }
    }
}
